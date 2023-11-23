// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="BanquetBookingRequestCommand.cs" company="string.Empty">
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
    /// Class BanquetBookingRequestCommand.
    /// </summary>
    public partial class BanquetBookingRequestCommand : IBanquetBookingRequestCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private BanquetBookingRequestDataContext objDataContext = null;

        /// <summary>
        /// Gets all banquet booking request for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        public List<SelectListItem> GetAllBanquetBookingRequestForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objBanquetBookingRequestList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new BanquetBookingRequestDataContext())
                {
                    objBanquetBookingRequestList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetBanquetBookingRequestAllResult> objBanquetBookingRequestResultList = this.objDataContext.MS_GetBanquetBookingRequestAll().ToList();
                    if (objBanquetBookingRequestResultList != null && objBanquetBookingRequestResultList.Count > 0)
                    {
                        foreach (var item in objBanquetBookingRequestResultList)
                        {
                            objBanquetBookingRequestList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objBanquetBookingRequestList;
        }

        /// <summary>
        /// Gets the banquet booking request by banquet booking request identifier.
        /// </summary>
        /// <param name="lgBanquetBookingRequestId">The long banquet booking request identifier.</param>
        /// <returns>BanquetBooking Request  Model.</returns>
        public BanquetBookingRequestModel GetBanquetBookingRequestByBanquetBookingRequestId(long lgBanquetBookingRequestId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            BanquetBookingRequestModel objBanquetBookingRequestModel = new BanquetBookingRequestModel();
            try
            {
                using (this.objDataContext = new BanquetBookingRequestDataContext())
                {
                    MS_GetBanquetBookingRequestByIdResult item = this.objDataContext.MS_GetBanquetBookingRequestById(lgBanquetBookingRequestId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objBanquetBookingRequestModel.Id = item.Id;
                        objBanquetBookingRequestModel.Name = item.Name;
                        objBanquetBookingRequestModel.MemberCode = item.MemberCode;
                        objBanquetBookingRequestModel.MemberId = item.MemberId;
                        objBanquetBookingRequestModel.ReferredMemberCode = item.ReferredMemberCode;
                        objBanquetBookingRequestModel.MemberReferredId = item.MemberReferredID;
                        objBanquetBookingRequestModel.BookingType = item.BookingType;
                        objBanquetBookingRequestModel.InquiryFor = item.InquiryFor;
                        objBanquetBookingRequestModel.EMail = item.EMail;
                        objBanquetBookingRequestModel.Address = item.Address;
                        objBanquetBookingRequestModel.City = item.City;
                        objBanquetBookingRequestModel.MobileNo = item.MobileNo;
                        objBanquetBookingRequestModel.Occasion = item.Occasion;
                        objBanquetBookingRequestModel.StrFromDate = item.FromDate != null ? item.FromDate.Value.ToString(Functions.DateFormat) : string.Empty;
                        objBanquetBookingRequestModel.StrToDate = item.ToDate != null ? item.ToDate.Value.ToString(Functions.DateFormat) : string.Empty;
                        objBanquetBookingRequestModel.BanquetTypeId = item.BanquetTypeId;
                        objBanquetBookingRequestModel.BanquetType = item.BanquetType;
                        objBanquetBookingRequestModel.Remarks = item.Remarks;
                        objBanquetBookingRequestModel.Status = item.Status;
                        objBanquetBookingRequestModel.AdminRemarks = item.AdminRemarks;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objBanquetBookingRequestModel;
        }

        /// <summary>
        /// Saves the banquet booking request.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveBanquetBookingRequest(BanquetBookingRequestModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BanquetBookingRequestDataContext())
                    {
                        ////var result = this.objDataContext.MS_InsertOrUpdateBanquetBookingRequest(objSave.Id, objSave.MemberId, objSave.Name, Convert.ToBoolean(objSave.BookingType), Convert.ToInt64(objSave.InquiryFor), objSave.EMail, objSave.Address, objSave.City, objSave.MobileNo, objSave.Occasion, objSave.StrFromDate, objSave.StrToDate, objSave.BanquetTypeId, objSave.Remarks, Convert.ToInt32(objSave.Status), objSave.AdminRemarks, MySession.Current.UserId, PageMaster.BanquetBookingRequest).FirstOrDefault();
                        var result = this.objDataContext.MS_InsertOrUpdateBanquetBookingRequest(objSave.Id, objSave.MemberId, objSave.Name, Convert.ToBoolean(objSave.BookingType), Convert.ToInt64(objSave.InquiryFor), objSave.EMail, objSave.Address, objSave.City, objSave.MobileNo, objSave.Occasion, objSave.StrFromDate, objSave.StrToDate, objSave.BanquetTypeId, objSave.Remarks, Convert.ToString(objSave.MemberReferredId), Convert.ToInt32(objSave.Status), objSave.AdminRemarks, MySession.Current.UserId, PageMaster.BanquetBookingRequest).FirstOrDefault();
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
        /// Deletes the banquet booking request.
        /// </summary>
        /// <param name="strBanquetBookingRequestIdList">The string banquet booking request identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteBanquetBooking Request  Result.</returns>
        public MS_DeleteBanquetBookingRequestResult DeleteBanquetBookingRequest(string strBanquetBookingRequestIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteBanquetBookingRequestResult result = new MS_DeleteBanquetBookingRequestResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new BanquetBookingRequestDataContext())
                    {
                        result = this.objDataContext.MS_DeleteBanquetBookingRequest(strBanquetBookingRequestIdList, lgDeletedBy, PageMaster.BanquetBookingRequest).ToList().FirstOrDefault();
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
        /// Determines whether [is banquet booking request exists] [the specified long banquet booking request identifier].
        /// </summary>
        /// <param name="lgBanquetBookingRequestId">The long banquet booking request identifier.</param>
        /// <param name="strName">Name of the string.</param>
        /// <returns><c>true</c> if [is banquet booking request exists] [the specified long banquet booking request identifier]; otherwise, <c>false</c>.</returns>
        public bool IsBanquetBookingRequestExists(long lgBanquetBookingRequestId, string strName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new BanquetBookingRequestDataContext())
                {
                    if (this.objDataContext.BanquetBookingRequests.Where(x => x.Id != lgBanquetBookingRequestId && x.Name == strName && x.IsDeleted == false).Count() > 0)
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
        /// Searches the banquet booking request.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="sortedby">The sorted by.</param>
        /// <param name="banquetType">Type of the banquet.</param>
        /// <returns>List&lt;MS_SearchBanquet Booking Request Result  &gt;.</returns>
        public List<MS_SearchBanquetBookingRequestResult> SearchBanquetBookingRequest(int inRow, int inPage, string strSearch, string strSort, int sortedby, long banquetType)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new BanquetBookingRequestDataContext())
                {
                    List<MS_SearchBanquetBookingRequestResult> objSearchBanquetBookingRequestList = this.objDataContext.MS_SearchBanquetBookingRequest(inRow, inPage, strSearch, strSort, sortedby, banquetType).ToList();
                    return objSearchBanquetBookingRequestList;
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
        /// Gets the type of the banquet.
        /// </summary>
        /// <returns>List&lt;Select List Item  &gt;.</returns>
        public List<SelectListItem> GetBanquetType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                BanquetDataContext dbB = new BanquetDataContext();
                List<SelectListItem> lstBanquetType = new List<SelectListItem>();
                foreach (var item in dbB.Banquets.Where(x => x.IsDeleted == false))
                {
                    lstBanquetType.Add(new SelectListItem
                    {
                        Text = item.BanquetName,
                        Value = item.Id.ToString()
                    });
                }

                return lstBanquetType;
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
