// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="RoomBookingCommand.cs" company="string.Empty">
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
    /// Class RoomBookingCommand.
    /// </summary>
    public partial class RoomBookingCommand : IRoomBookingCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private RoomBookingDataContext objDataContext = null;

        /// <summary>
        /// Gets all room booking for drop down.
        /// </summary>
        /// <returns>List&lt;SelectListItem   &gt;.</returns>
        public List<SelectListItem> GetAllRoomBookingForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<SelectListItem> objRoomBookingList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new RoomBookingDataContext())
                {
                    objRoomBookingList.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<MS_GetRoomBookingAllResult> objRoomBookingResultList = this.objDataContext.MS_GetRoomBookingAll().ToList();
                    if (objRoomBookingResultList != null && objRoomBookingResultList.Count > 0)
                    {
                        foreach (var item in objRoomBookingResultList)
                        {
                            objRoomBookingList.Add(new SelectListItem { Text = item.RoomBookingName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoomBookingList;
        }

        /// <summary>
        /// Gets the room booking by room booking identifier.
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>RoomBooking Model.</returns>
        public RoomBookingModel GetRoomBookingByRoomBookingId(long lgRoomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            RoomBookingModel objRoomBookingModel = new RoomBookingModel();
            try
            {
                using (this.objDataContext = new RoomBookingDataContext())
                {
                    MS_GetRoomBookingByIdResult item = this.objDataContext.MS_GetRoomBookingById(lgRoomBookingId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objRoomBookingModel.Id = item.Id;
                        objRoomBookingModel.RoomBookingName = item.RoomBookingName;
                        objRoomBookingModel.Description = item.Description;
                        objRoomBookingModel.Member = item.Member;
                        objRoomBookingModel.Guest = item.Guest;
                        objRoomBookingModel.ProfilePic = item.ProfilePic;
                        objRoomBookingModel.Terms = item.Terms;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoomBookingModel;
        }

        /// <summary>
        /// Saves the room booking.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <param name="objRoomBookingDetailList">The object room booking detail list.</param>
        /// <param name="objRoomBookingGallaryList">The object room booking gallery list.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveRoomBooking(RoomBookingModel objSave, List<RoomBookingModel> objRoomBookingDetailList, List<RoomBookingModel> objRoomBookingGallaryList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                long roomBookingId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateRoomBooking(objSave.Id, objSave.RoomBookingName, objSave.Description, objSave.Member, objSave.Guest, objSave.ProfilePic, objSave.Terms, MySession.Current.UserId, PageMaster.RoomBooking).FirstOrDefault();
                        if (result != null)
                        {
                            roomBookingId = result.InsertedId;
                            if (objRoomBookingDetailList != null && objRoomBookingDetailList.Count > 0)
                            {
                                foreach (var item in objRoomBookingDetailList)
                                {
                                    if (item.IsDeleted && item.RoomBookingDetailId > 0)
                                    {
                                        this.objDataContext.MS_DeleteRoomBookingTextDetail(item.RoomBookingDetailId.ToString(), MySession.Current.UserId, PageMaster.RoomBooking);
                                    }
                                    else if (!item.IsDeleted)
                                    {
                                        this.objDataContext.MS_InsertOrUpdateRoomBookingTextDetail(item.RoomBookingDetailId, roomBookingId, item.TaxPercentage, item.TaxDescription, MySession.Current.UserId, PageMaster.RoomBooking);
                                    }
                                }
                            }

                            if (objRoomBookingGallaryList != null && objRoomBookingGallaryList.Count > 0)
                            {
                                foreach (var item in objRoomBookingGallaryList)
                                {
                                    if (item.IsDeleted && item.RoomBookingGallaryId > 0)
                                    {
                                        this.objDataContext.MS_DeleteRoomBookingGallary(item.RoomBookingGallaryId.ToString(), MySession.Current.UserId, PageMaster.RoomBooking);
                                    }
                                    else if (!item.IsDeleted)
                                    {
                                        this.objDataContext.MS_InsertOrUpdateRoomBookingGallary(item.RoomBookingGallaryId, roomBookingId, item.GallaryImage, MySession.Current.UserId, PageMaster.RoomBooking);
                                    }
                                }
                            }
                        }
                    }

                    scope.Complete();
                }

                return roomBookingId;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the room booking.
        /// </summary>
        /// <param name="strRoomBookingIdList">The string room booking identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBooking Result.</returns>
        public MS_DeleteRoomBookingResult DeleteRoomBooking(string strRoomBookingIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteRoomBookingResult result = new MS_DeleteRoomBookingResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingDataContext())
                    {
                        result = this.objDataContext.MS_DeleteRoomBooking(strRoomBookingIdList, lgDeletedBy, PageMaster.RoomBooking).ToList().FirstOrDefault();
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
        /// Determines whether [is room booking exists] [the specified long room booking identifier].
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <param name="strRoomBookingName">Name of the string room booking.</param>
        /// <returns><c>true</c> if [is room booking exists] [the specified long room booking identifier]; otherwise, <c>false</c>.</returns>
        public bool IsRoomBookingExists(long lgRoomBookingId, string strRoomBookingName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new RoomBookingDataContext())
                {
                    if (this.objDataContext.RoomBooking.Where(x => x.Id != lgRoomBookingId && x.RoomBookingName == strRoomBookingName && x.IsDeleted == false).Count() > 0)
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
        /// Searches the room booking.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List&lt;MS_SearchRoomBookingResult   &gt;.</returns>
        public List<MS_SearchRoomBookingResult> SearchRoomBooking(int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (this.objDataContext = new RoomBookingDataContext())
                {
                    List<MS_SearchRoomBookingResult> objSearchRoomBookingList = this.objDataContext.MS_SearchRoomBooking(inRow, inPage, strSearch, strSort).ToList();
                    return objSearchRoomBookingList;
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
        /// Saves the room booking detail.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveRoomBookingDetail(RoomBookingModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateRoomBookingTextDetail(objSave.RoomBookingDetailId, objSave.Id, objSave.TaxPercentage, objSave.TaxDescription, MySession.Current.UserId, PageMaster.RoomBooking).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.InsertedId;
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
        /// Deletes the room booking detail.
        /// </summary>
        /// <param name="strRoomBookingDetailIdList">The string room booking detail identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBookingTextDetail Result.</returns>
        public MS_DeleteRoomBookingTextDetailResult DeleteRoomBookingDetail(string strRoomBookingDetailIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteRoomBookingTextDetailResult result = new MS_DeleteRoomBookingTextDetailResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingDataContext())
                    {
                        result = this.objDataContext.MS_DeleteRoomBookingTextDetail(strRoomBookingDetailIdList, lgDeletedBy, PageMaster.RoomBooking).ToList().FirstOrDefault();
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
        /// Gets the room booking detail by room booking identifier.
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>List&lt;RoomBookingModel   &gt;.</returns>
        public List<RoomBookingModel> GetRoomBookingDetailByRoomBookingId(long lgRoomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<RoomBookingModel> objRoomBookingDetailList = new List<RoomBookingModel>();
            try
            {
                using (this.objDataContext = new RoomBookingDataContext())
                {
                    List<MS_GetRoomBookingTextDetailByRoomBookingIdResult> objRoomBookingDetailResultList = this.objDataContext.MS_GetRoomBookingTextDetailByRoomBookingId(lgRoomBookingId).ToList();
                    long lgCount = 1;
                    if (objRoomBookingDetailResultList != null && objRoomBookingDetailResultList.Count > 0)
                    {
                        foreach (var item in objRoomBookingDetailResultList)
                        {
                            var objRoomBookingModel = new RoomBookingModel();
                            objRoomBookingModel.RoomBookingDetailId = item.Id;
                            objRoomBookingModel.Id = lgCount;
                            objRoomBookingModel.TaxPercentage = item.TaxPercentage;
                            objRoomBookingModel.TaxDescription = item.TaxDescription;
                            objRoomBookingDetailList.Add(objRoomBookingModel);
                            lgCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoomBookingDetailList;
        }

        /// <summary>
        /// Saves the room booking gallery.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>Returns System.INT64.</returns>
        public long SaveRoomBookingGallary(RoomBookingModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingDataContext())
                    {
                        var result = this.objDataContext.MS_InsertOrUpdateRoomBookingGallary(objSave.RoomBookingGallaryId, objSave.Id, objSave.GallaryImage, MySession.Current.UserId, PageMaster.RoomBooking).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.InsertedId;
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
        /// Deletes the room booking gallery.
        /// </summary>
        /// <param name="strRoomBookingGallaryIdList">The string room booking gallery identifier list.</param>
        /// <param name="lgDeletedBy">The long deleted by.</param>
        /// <returns>MS_DeleteRoomBooking Gallery  Result.</returns>
        public MS_DeleteRoomBookingGallaryResult DeleteRoomBookingGallary(string strRoomBookingGallaryIdList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            MS_DeleteRoomBookingGallaryResult result = new MS_DeleteRoomBookingGallaryResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new RoomBookingDataContext())
                    {
                        result = this.objDataContext.MS_DeleteRoomBookingGallary(strRoomBookingGallaryIdList, lgDeletedBy, PageMaster.RoomBooking).ToList().FirstOrDefault();
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
        /// Gets the room booking gallery by room booking identifier.
        /// </summary>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>List&lt;RoomBookingModel   &gt;.</returns>
        public List<RoomBookingModel> GetRoomBookingGallaryByRoomBookingId(long lgRoomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            List<RoomBookingModel> objRoomBookingGallaryList = new List<RoomBookingModel>();
            try
            {
                using (this.objDataContext = new RoomBookingDataContext())
                {
                    List<MS_GetRoomBookingGallaryByRoomBookingIdResult> objRoomBookingGallaryResultList = this.objDataContext.MS_GetRoomBookingGallaryByRoomBookingId(lgRoomBookingId).ToList();
                    long lgCount = 1;
                    if (objRoomBookingGallaryResultList != null && objRoomBookingGallaryResultList.Count > 0)
                    {
                        foreach (var item in objRoomBookingGallaryResultList)
                        {
                            var objRoomBookingModel = new RoomBookingModel();
                            objRoomBookingModel.RoomBookingGallaryId = item.Id;
                            objRoomBookingModel.Id = lgCount;
                            objRoomBookingModel.GallaryImage = item.GallaryImage;
                            objRoomBookingGallaryList.Add(objRoomBookingModel);
                            lgCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return objRoomBookingGallaryList;
        }
    }
}
