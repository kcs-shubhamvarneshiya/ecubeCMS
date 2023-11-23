// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="EventBookigCommand.cs" company="string.Empty">
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
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using Serilog;

    /// <summary>
    /// Class CMSConfigCommand.
    /// </summary>
    public partial class EventBookigCommand : IEventBookigCommand
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object data context.
        /// </summary>
        private EventDataContext objDataContext = null;

        /// <summary>
        /// Gets All Account Head for drop down.
        /// </summary>
        /// <returns>List&lt; SelectList Item  &gt;.</returns>
        public List<SelectListItem> GetAllAccountHeadForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objAccountHeadList = new List<SelectListItem>();
            objAccountHeadList.Add(new SelectListItem { Text = "Select Account Head", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<CRM_GetAccountLedgerList_Result> lstAccountHead = this.objDataContext.CRM_GetAccountLedgerList_(string.Empty, string.Empty).ToList();
                    if (lstAccountHead != null && lstAccountHead.Count > 0)
                    {
                        foreach (var item in lstAccountHead)
                        {
                            objAccountHeadList.Add(new SelectListItem { Text = item.AccountHead, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objAccountHeadList;
        }



        /// <summary>
        /// Gets all event for report.
        /// </summary>
        /// <returns>List of get all today's event for report.</returns>
        public List<SelectListItem> GetAllEventForReport(DateTime eventDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objEventList = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_GetAllEventForReportResult> lstEvents = this.objDataContext.MS_GetAllEventForReport(eventDate).ToList();
                    if (lstEvents != null && lstEvents.Count > 0)
                    {
                        foreach (var item in lstEvents)
                        {
                            objEventList.Add(new SelectListItem { Text = item.EventTitle, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objEventList;
        }

        /// <summary>
        /// Events the booking report.
        /// </summary>
        /// <param name="eventId">The event identifier parameter.</param>
        /// <param name="ticketId">The ticket identifier parameter.</param>
        /// <param name="membercode">The member code parameter.</param>
        /// <param name="eventDate">The event Date parameter.</param>
        /// <returns>List of event booking report.</returns>
        public List<MS_EventBookingReportResult> EventBookingReport(int eventId, int ticketId, string membercode, DateTime eventDate, int rows, int page)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<MS_EventBookingReportResult> objEventBookingReport = new List<MS_EventBookingReportResult>();

            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    objEventBookingReport = this.objDataContext.MS_EventBookingReport(eventId, ticketId, membercode, eventDate, rows, page).ToList();
                    if (objEventBookingReport == null)
                    {
                        objEventBookingReport = new List<MS_EventBookingReportResult>();
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objEventBookingReport;
        }

        /// <summary>
        /// Gets all tax master for drop down.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; get all tax master for drop down.</returns>
        public List<SelectListItem> GetAllTaxMasterForDropDown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            List<SelectListItem> objTaxMasterList = new List<SelectListItem>();
            objTaxMasterList.Add(new SelectListItem { Text = "Select Service Tax", Value = "0" });
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_GetCRMTaxMasterAllResult> lstTaxMaster = this.objDataContext.MS_GetCRMTaxMasterAll().ToList();
                    if (lstTaxMaster != null && lstTaxMaster.Count > 0)
                    {
                        foreach (var item in lstTaxMaster)
                        {
                            objTaxMasterList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objTaxMasterList;
        }

        /// <summary>
        /// Gets the event by event identifier.
        /// </summary>
        /// <param name="lgEventId">The LONG event category identifier.</param>
        /// <returns>Event Booking Model.</returns>
        public EventBookingModel GetEventByEventId(int lgEventId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            EventBookingModel objEventModel = new EventBookingModel();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    MS_GetEventByEventIDResult item = this.objDataContext.MS_GetEventByEventID(lgEventId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objEventModel.Id = item.Id;
                        objEventModel.EventCategoryId = (int)item.EventCategoryId;
                        objEventModel.EventTitle = item.EventTitle;
                        objEventModel.EventScreen = item.EventScreen;
                        objEventModel.EventPlace = item.EventPlace;
                        objEventModel.EventDuration = item.EventDuration;
                        objEventModel.EventStartTime = item.EventStartTime;
                        objEventModel.EventEndTime = item.EventEndTime;
                        objEventModel.EvenLastRegistrationDate = (DateTime)item.EvenLastRegistrationDate;
                        objEventModel.ArtistsInfo = item.ArtistsInfo;
                        objEventModel.EventSynopsis = item.EventSynopsis;
                        objEventModel.TermsConditions = item.TermsConditions;
                        objEventModel.EventImage = item.EventImage;
                        objEventModel.EventBannerImage = item.EventBannerImage;
                        objEventModel.AccountHeadId = item.AccountHeadId != null ? item.AccountHeadId.Value : 0;
                        objEventModel.TaxMasterId = item.TaxMasterId != null ? item.TaxMasterId.Value : 0;
                        objEventModel.IsMaximumTicketForFamilyMember = Convert.ToBoolean(item.IsMaximumTicketForFamilyMember);
                        objEventModel.MaximumTicketForFamilyMember = Convert.ToInt32(item.MaximumTicketForFamilyMember);
                        objEventModel.IsCheckOutstanding = Convert.ToBoolean(item.IsCheckOutstanding);
                        objEventModel.OutstandingAmountForMember = Convert.ToInt32(item.OutstandingAmountForMember);
                        objEventModel.GuestLimit = Convert.ToInt32(item.GuestLimit);
                        objEventModel.RegistrationEndTime = item.RegistrationEndTime;
                        objEventModel.IsQRCode = item.IsQRCode;
                        objEventModel.EventEntryAfter = item.EventEntryAfter;
                        objEventModel.EventEntryBefore = item.EventEntryBefore;
                        objEventModel.IsDisplayDetail = item.IsDisplayDetail;
                        objEventModel.EventMobileImage = item.MobileBannerImage;
                        objEventModel.AllowMultipleCategory = item.AllowMultipleCategory;
                        objEventModel.EventAttachment = item.EventAttachment;
                        objEventModel.EventAttachmentDisplay = item.EventAttachmentDisplay;
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objEventModel;
        }

        /// <summary>
        /// Gets the event schedule identifier.
        /// </summary>
        /// <param name="lgEventId">The LONG event identifier.</param>
        /// <returns>EventSchedule Model.</returns>
        public EventScheduleModel GetEventScheduleId(int lgEventId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            EventScheduleModel objEventScheduleModel = new EventScheduleModel();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    MS_GetEventscheduleByIDResult item = this.objDataContext.MS_GetEventscheduleByID(lgEventId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objEventScheduleModel.Id = item.Id;
                        objEventScheduleModel.EventId = (int)item.EventId;
                        objEventScheduleModel.EventFromDate = Convert.ToDateTime(item.EventDate);
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objEventScheduleModel;
        }

        /// <summary>
        /// Gets the event rate identifier.
        /// </summary>
        /// <param name="lgEventId">The LONG event rate identifier.</param>
        /// <returns>Event Rate Model.</returns>
        public EventRateModel GetEventRateId(int lgEventId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            EventRateModel objEventRateModel = new EventRateModel();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    MS_GetEventRateByIDResult item = this.objDataContext.MS_GetEventRateByID(lgEventId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objEventRateModel.Id = item.Id;
                        objEventRateModel.EventId = (int)item.EventId;
                        objEventRateModel.EventTicketCategoryId = (int)item.EventTicketCategoryId;
                        objEventRateModel.MemberFees = (decimal)item.MemberFees;
                        objEventRateModel.GuestFees = (decimal)item.GuestFees;
                        objEventRateModel.SubMemberFees = (decimal)item.SpecialGuest;
                    }
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
            }

            return objEventRateModel;
        }

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        public long SaveEvent(EventBookingModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventDataContext())
                    {
                        MS_InsertOrUpdateEventResult result = this.objDataContext.MS_InsertOrUpdateEvent((int)objSave.Id, (int)objSave.EventCategoryId, objSave.EventTitle, objSave.EventScreen, objSave.EventPlace, objSave.EventDuration, objSave.EventStartTime, objSave.EventEndTime, objSave.RegistrationDate, objSave.ArtistsInfo, objSave.EventSynopsis, objSave.TermsConditions, objSave.EventImage, objSave.EventBannerImage, (int)MySession.Current.UserId, objSave.AccountHeadId, objSave.TaxMasterId, objSave.GuestLimit, objSave.RegistrationEndTime, objSave.IsMaximumTicketForFamilyMember, objSave.MaximumTicketForFamilyMember, objSave.IsCheckOutstanding, objSave.OutstandingAmountForMember, objSave.IsQRCode, objSave.EventEntryBefore, objSave.EventEntryAfter, objSave.IsDisplayDetail, objSave.EventMobileImage, objSave.AllowMultipleCategory, objSave.EventAttachment).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.ID;
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
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        public long SaveEventSchedule(EventScheduleModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventDataContext())
                    {
                        MS_InsertOrUpdateEventscheduleResult result = this.objDataContext.MS_InsertOrUpdateEventschedule((int)objSave.EventId, objSave.EventFromDate, objSave.EventToDate, (int)MySession.Current.UserId).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.ID;
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
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        /// <returns>System INT64.</returns>
        public long SaveEventRate(EventRateModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventDataContext())
                    {
                        MS_InsertOrUpdateEventRateResult result = this.objDataContext.MS_InsertOrUpdateEventRate(
                            (int)objSave.Id,
                            objSave.EventId,
                            objSave.EventTicketCategoryId,
                            objSave.Title,
                            objSave.MemberFees,
                            objSave.GuestFees,
                            objSave.SubMemberFees,
                            objSave.NoOfSeats,
                            //// AddBy : Karan Shah on Date : 29-MAR-2017
                            objSave.MemberSeatLimit,
                            objSave.GuestSeatLimit,
                            objSave.AffiliateMemberFee,
                            objSave.ParentFee,
                            objSave.GuestChildFee,
                            objSave.GuestRoomFee,
                            objSave.FromSerialNo,
                            objSave.ToSerialNo,
                            //// End
                            Convert.ToInt32(MySession.Current.UserId)).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.Id = result.ID;
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
        /// Deletes the event.
        /// </summary>
        /// <param name="strEventList">The string event list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_Delete Event Result.</returns>
        public MS_DeleteEventResult DeleteEvent(string strEventList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            MS_DeleteEventResult result = new MS_DeleteEventResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventDataContext())
                    {
                        result = this.objDataContext.MS_DeleteEvent(strEventList, lgDeletedBy).ToList().FirstOrDefault();
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
        /// Deletes the event schedule.
        /// </summary>
        /// <param name="strEventScheduleList">The string event schedule list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_DeleteEvent schedule Result.</returns>
        public MS_DeleteEventscheduleResult DeleteEventSchedule(string strEventScheduleList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            MS_DeleteEventscheduleResult result = new MS_DeleteEventscheduleResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventDataContext())
                    {
                        result = this.objDataContext.MS_DeleteEventschedule(strEventScheduleList, lgDeletedBy).ToList().FirstOrDefault();
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
        /// Deletes the event rate.
        /// </summary>
        /// <param name="strEventRateList">The string event rate list.</param>
        /// <param name="lgDeletedBy">The LONG deleted by.</param>
        /// <returns>MS_DeleteEventRate Result.</returns>
        public MS_DeleteEventRateResult DeleteEventRate(string strEventRateList, long lgDeletedBy)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            MS_DeleteEventRateResult result = new MS_DeleteEventRateResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new EventDataContext())
                    {
                        result = this.objDataContext.MS_DeleteEventRate(strEventRateList, lgDeletedBy).ToList().FirstOrDefault();
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
        /// Determines whether [is event exists] [the specified LONG event identifier].
        /// </summary>
        /// <param name="lgEventId">The LONG event identifier.</param>
        /// <param name="strEventTitle">Name of the string event.</param>
        /// <returns><c>true</c> if [is event exists] [the specified LONG event identifier]; otherwise, <c>false</c>.</returns>
        public bool IsEventExists(long lgEventId, string strEventTitle)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    if (this.objDataContext.Events.Where(x => x.Id != lgEventId && x.EventTitle == strEventTitle && x.IsDeleted == false).Count() > 0)
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
        /// Determines whether [is event schedule exists] [the specified LONG event schedule identifier].
        /// </summary>
        /// <param name="lgEventId">The LONG event schedule identifier.</param>
        /// <param name="eventFromDate">Name of the LONG eventDate schedule.</param>
        /// <param name="eventToDate">Name of the LONG eventDate schedule.</param>
        /// <returns><c>true</c> if [is event schedule exists] [the specified LONG event schedule identifier]; otherwise, <c>false</c>.</returns>
        public bool IsEventScheduleExists(long lgEventId, DateTime eventFromDate, DateTime eventToDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    if (this.objDataContext.EventSchedules.Where(x => x.EventId == lgEventId && x.EventDate >= eventFromDate && x.EventDate <= eventToDate && x.IsDeleted == false).Count() > 0)
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
        /// Determines whether [is event rate exists] [the specified LONG event rate identifier].
        /// </summary>
        /// <param name="eventId">Name of the LONG event rate.</param>
        /// <param name="eventTicketCategoryId">The LONG event rate identifier.</param>
        /// <returns><c>true</c> if [is event rate exists] [the specified LONG event rate identifier]; otherwise, <c>false</c>.</returns>
        public bool IsEventRateExists(long eventId, int eventTicketCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    if (this.objDataContext.EventRates.Where(x => x.EventId == eventId && x.EventTicketCategoryId == eventTicketCategoryId && x.IsDeleted == false).Count() > 0)
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
        /// Searches the event.
        /// </summary>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <param name="completedEvent">The boolean complete Event.</param>
        /// <returns>List MS_SearchEventResult.</returns>
        public List<MS_SerachEventResult> SearchEvent(int inRow, int inPage, string strSearch, string strSort, int completedEvent)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_SerachEventResult> objSearchEventList = this.objDataContext.MS_SerachEvent(inRow, inPage, strSearch, strSort, completedEvent).ToList();
                    return objSearchEventList;
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
        /// Searches the event schedule.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List MS_SearchEventSchedule Result.</returns>
        public List<MS_SearchEventScheduleResult> SearchEventSchedule(int eventId, int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_SearchEventScheduleResult> objSearchEventList = this.objDataContext.MS_SearchEventSchedule(eventId, inRow, inPage, strSearch, strSort).ToList();
                    return objSearchEventList;
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
        /// Searches the event rate.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="inRow">The in row.</param>
        /// <param name="inPage">The in page.</param>
        /// <param name="strSearch">The string search.</param>
        /// <param name="strSort">The string sort.</param>
        /// <returns>List MS_SearchEventRate Result.</returns>
        public List<MS_SearchEventRateResult> SearchEventRate(int eventId, int inRow, int inPage, string strSearch, string strSort)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (this.objDataContext = new EventDataContext())
                {
                    List<MS_SearchEventRateResult> objSearchEventList = this.objDataContext.MS_SearchEventRate(eventId, inRow, inPage, strSearch, strSort).ToList();
                    return objSearchEventList;
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
        /// Accounts the ledger list.
        /// </summary>
        /// <param name="accountHead">The account head.</param>
        /// <param name="shortName">The short name.</param>
        /// <returns>List CRM_GetAccountLedgerList_Result.</returns>
        public List<CRM_GetAccountLedgerList_Result> AccountLedgerList(string accountHead, string shortName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<CRM_GetAccountLedgerList_Result> objGetAccountLedgerList = new List<CRM_GetAccountLedgerList_Result>();
                return objGetAccountLedgerList;
            }
            catch(Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
                throw;
            }

           
        }
    }
}