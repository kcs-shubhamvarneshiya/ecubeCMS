// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="RoomBookingRequestCommand.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class RoomBookingRequestCommand.
    /// </summary>
    public partial class RoomBookingRequestCommand : IRoomBookingRequestCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private RoomBookingRequestDataContext objDataContext = null;

        /// <summary>
        /// Gets all room booking request for drop down.
        /// </summary>
        /// <returns>List&lt;SelectList Item &gt;.</returns>
        public List<SelectListItem> GetAllRoomBookingRequestForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objRoomBookingRequestList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new RoomBookingRequestDataContext())
                {
                    objRoomBookingRequestList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetRoomBookingRequestAllResult> objRoomBookingRequestResultList = this.objDataContext.MS_GetRoomBookingRequestAll().ToList();
                    if (objRoomBookingRequestResultList != null && objRoomBookingRequestResultList.Count > 0)
                    {
                        foreach (var item in objRoomBookingRequestResultList)
                        {
                            objRoomBookingRequestList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoomBookingRequestList;
        }

        /// <summary>
        /// Gets the room booking request by room booking request identifier.
        /// </summary>
        /// <param name="lgRoomBookingRequestId">The long room booking request identifier.</param>
        /// <returns>RoomBookingRequest Model.</returns>
        public RoomBookingRequestModel GetRoomBookingRequestByRoomBookingRequestId(long lgRoomBookingRequestId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            RoomBookingRequestModel objRoomBookingRequestModel = new RoomBookingRequestModel();
            try
            {
                using (this.objDataContext = new RoomBookingRequestDataContext())
                {
                    MS_GetRoomBookingRequestByIdResult item = this.objDataContext.MS_GetRoomBookingRequestById(lgRoomBookingRequestId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objRoomBookingRequestModel.Id = item.Id;
                        objRoomBookingRequestModel.Name = item.Name;
                        objRoomBookingRequestModel.MemberId = item.MemberId;
                        objRoomBookingRequestModel.MemberCode = item.MemberCode;
                        objRoomBookingRequestModel.ReferredMemberCode = item.ReferredMemberCode;
                        objRoomBookingRequestModel.MemberReferredId = item.MemberReferredID;
                        objRoomBookingRequestModel.InquiryFor = item.InquiryFor;
                        objRoomBookingRequestModel.EMail = item.EMail;
                        objRoomBookingRequestModel.Address = item.Address;
                        objRoomBookingRequestModel.City = item.City;
                        objRoomBookingRequestModel.MobileNo = item.MobileNo;
                        objRoomBookingRequestModel.Member = item.Member;
                        objRoomBookingRequestModel.Adults = item.Adults;
                        objRoomBookingRequestModel.Children = item.Children;
                        objRoomBookingRequestModel.StrCheckedInDate = item.CheckedInDate != null ? item.CheckedInDate.Value.ToString(Functions.DateFormat) : string.Empty;
                        objRoomBookingRequestModel.StrCheckedOutDate = item.CheckedOutDate != null ? item.CheckedOutDate.Value.ToString(Functions.DateFormat) : string.Empty;
                        objRoomBookingRequestModel.RoomTypeId = item.RoomTypeNo;
                        objRoomBookingRequestModel.RoomType = item.RoomType;
                        objRoomBookingRequestModel.Remarks = item.Remarks;
                        objRoomBookingRequestModel.Status = item.Status;
                        objRoomBookingRequestModel.AdminRemarks = item.AdminRemarks;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoomBookingRequestModel;
        }

        /// <summary>
        /// Saves the room booking request.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveRoomBookingRequest(RoomBookingRequestModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingRequestDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateRoomBookingRequest(objSave.Id, objSave.MemberId, objSave.Name, objSave.InquiryFor, objSave.EMail, objSave.Address, objSave.City, objSave.MobileNo, objSave.Member, objSave.Adults, objSave.Children, objSave.StrCheckedInDate, objSave.StrCheckedOutDate, objSave.RoomTypeId, objSave.Remarks, objSave.Status, objSave.AdminRemarks, MySession.Current.UserId, PageMaster.RoomBookingRequest).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.INsertedID;
                        }
                    }

                    scope.Complete();
                }

                return objSave.Id;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the room booking request.
        /// </summary>
        /// <param name="strRoomBookingRequestIdList">The string room booking request identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBookingRequest Result.</returns>
        public MS_DeleteRoomBookingRequestResult DeleteRoomBookingRequest(string strRoomBookingRequestIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteRoomBookingRequestResult result = new MS_DeleteRoomBookingRequestResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingRequestDataContext())
                    {
                        result = this.objDataContext.MS_DeleteRoomBookingRequest(strRoomBookingRequestIdList, lgDeletedBy, PageMaster.RoomBookingRequest).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is room booking request exists] [the specified long room booking request identifier].
        /// </summary>
        /// <param name="lgRoomBookingRequestId">The long room booking request identifier.</param>
        /// <param name="strName">Name of the string.</param>
        /// <returns><c>true</c> if [is room booking request exists] [the specified long room booking request identifier]; otherwise, <c>false</c>.</returns>
        public bool IsRoomBookingRequestExists(long lgRoomBookingRequestId, string strName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new RoomBookingRequestDataContext())
                {
                    if (this.objDataContext.RoomBookingRequests.Where(x => x.Id != lgRoomBookingRequestId && x.Name == strName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Searches the room booking request.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="sortedby">The sorted by.</param>
        /// <param name="roomType">Type of the room.</param>
        /// <returns>List&lt;MS_SearchRoomBookingRequest Result &gt;.</returns>
        public List<MS_SearchRoomBookingRequestResult> SearchRoomBookingRequest(int inRow, int inPage, string strSearch, string strSort, int sortedby, long roomType)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new RoomBookingRequestDataContext())
                {
                    List<MS_SearchRoomBookingRequestResult> objSearchRoomBookingRequestList = this.objDataContext.MS_SearchRoomBookingRequest(inRow, inPage, strSearch, strSort, sortedby, roomType).ToList();
                    return objSearchRoomBookingRequestList;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Gets the type of the room.
        /// </summary>
        /// <returns>The List&lt;SelectList Item &gt;.</returns>
        public List<SelectListItem> GetRoomType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                RoomBookingDataContext dbRB = new RoomBookingDataContext();
                List<SelectListItem> lstRoomType = new List<SelectListItem>();
                foreach (var item in dbRB.RoomBooking.Where(x => x.IsDeleted == false))
                {
                    lstRoomType.Add(new SelectListItem
                    {
                        Text = item.RoomBookingName,
                        Value = item.Id.ToString()
                    });
                }

                return lstRoomType;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
    }
}
