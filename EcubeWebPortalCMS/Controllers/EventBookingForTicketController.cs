// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Vasudev Patel
// Created          : 21-Feb-2023
//
// ***********************************************************************
// <copyright file="EventBookingForTicketController.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

namespace EcubeWebPortalCMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Newtonsoft.Json;
    using Serilog;

    //using System.ComponentModel;
    //using System.IO;    
    //using System.Net;   
    //using System.Drawing; 


    public class EventBookingForTicketController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        private readonly IEventBookingForMemberCommand objIEventBookingForMemberCommand = null;
        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        CommonDataContext objCommon = new CommonDataContext();
        AAAConfigSetting objConfig = new AAAConfigSetting();
        public EventBookingForTicketController(IEventBookingForMemberCommand iEventBookingForMemberCommand)
        {
            this.objIEventBookingForMemberCommand = iEventBookingForMemberCommand;
            this.objConfig = objCommon.AAAConfigSettings.Where(x => x.KeyName == "ClubName2").FirstOrDefault();
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EventBookingForMember()
        {
            this.ViewData["SelectEventID"] = this.Request.QueryString["EventID"];
            EventBookingForMemberModel objEventMemberModel = new EventBookingForMemberModel();
            objEventMemberModel.MemberCodeList = this.objIEventBookingForMemberCommand.GetMemberCodeDropdown();
            objEventMemberModel.PaymentList = this.objIEventBookingForMemberCommand.GetPaymentDropDown();
            //  objEventMemberModel.EventSeatAvailability = this.objIEventBookingForMemberCommand.GetEventDateByID(this.Request.QueryString["EventID"], Convert.ToInt32(MySession.Current.MemberCode.ToString()));
            return View(objEventMemberModel);
            //return View();
        }
        [HttpGet]
        public JsonResult GetMemberCodeDropdown()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EventBookingForMemberCommand dbEventBookingForMember = new EventBookingForMemberCommand();
                return this.Json(dbEventBookingForMember.GetMemberCodeDropdown().ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetEventDateDropdown(int memberid, int eventid)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EventBookingForMemberCommand dbEventBookingForMember = new EventBookingForMemberCommand();
                return this.Json(dbEventBookingForMember.GetEventDateByID(eventid, memberid).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveEventBookingForMember(EventBookingForMemberModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                //objSave.EvnetId = Convert.ToInt32(11);
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                objSave.Id = (int)this.objIEventBookingForMemberCommand.SaveEventBookingForUmedMember(objSave);
                if (objSave.Id > 0)
                {
                    EventTicket(objSave.Id);
                    this.ViewData["Success"] = "1";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Success);
                    var responseObj = new { id = objSave.Id, message = Functions.AlertMessage("Event Booking", MessageType.Success) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -5)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Member aleady booked", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Member aleady booked", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -2)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Member booking day limit over", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Member booking day limit over", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -3)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("More than ticket end number", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("More than ticket end number, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -6)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Member is inactive", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Member is Inactive", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                else
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objSave);
            }

            return this.View(objSave);
        }


        public ActionResult BindEventGrid(string memberid, int eventid, string BookingDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (string.IsNullOrEmpty(memberid))
                {
                    memberid = "0";
                }

                List<GetMemberDetailsWithEventRateResult> objEventList = this.objIEventBookingForMemberCommand.SearchEvent(Convert.ToInt32(memberid), eventid, BookingDate);
                return this.FillGridEvent(Convert.ToInt32(memberid), eventid, objEventList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }
        private ActionResult FillGridEvent(int page, int rows, List<GetMemberDetailsWithEventRateResult> objEventList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                //int totalRecords = objEventList != null && objEventList.Count > 0 ? objEventList[0].Total.Value : 0;
                int totalRecords = 2;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventList
                            select new
                            {
                                MemberName = objEvent.MemberName,
                                Relation = objEvent.Relation,
                                Age = objEvent.Age,
                                Photo = objEvent.ImageName,
                                MemberFees = objEvent.MemberFees,
                                CurrentOutstanding = objEvent.CurrentOutstanding,
                                MaximumTicketForFamilyMember = objEvent.MaximumTicketForFamilyMember,
                                IsMaximumTicketForFamilyMember = objEvent.IsMaximumTicketForFamilyMember,
                                Booked = objEvent.Booked,
                                GuestLimit = objEvent.GuestLimit,
                                CuurentBooking = objEvent.CuurentBooking,
                                NoOfSeats = objEvent.NoOfSeats,
                                CurrentTicketNumber = objEvent.CurrentTicketNumber,
                                ToSerialNo = objEvent.ToSerialNo,
                                ProceedFlag = objEvent.ProceedFlag,

                                //EventStartTime = objEvent.EventStartTime,
                                //EventEndTime = objEvent.EventEndTime,
                                //EvenLastRegistrationDate = objEvent.EvenLastRegistrationDate,
                                //ArtistsInfo = objEvent.ArtistsInfo,
                                //EventSynopsis = objEvent.EventSynopsis,
                                //TermsConditions = objEvent.TermsConditions,
                                //EventImage = objEvent.EventImage,
                                //EventBannerImage = objEvent.EventBannerImage,
                                //StepSeq = objEvent.StepSeq,
                                Id = objEvent.Id.ToString()//.Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPaymentTypeDropdown(string name)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objIEventBookingForMemberCommand.GetPaymentDropDown(name), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }


        //******* End Bind Member Grid ***********
        public ActionResult EventBookingForGuest()
        {
            this.ViewData["SelectEventID"] = this.Request.QueryString["EventID"];
            EventBookingForGuestModel objEventGuest = new EventBookingForGuestModel();
            objEventGuest.RelationshipList = this.objIEventBookingForMemberCommand.GetRelationshipDropDown();
            objEventGuest.MemberList = this.objIEventBookingForMemberCommand.GetMemberCodeDropdown();
            objEventGuest.PaymentList = this.objIEventBookingForMemberCommand.GetPaymentDropDown();
            //objEventGuest.MemberId = 22293;
            //objEventGuest.Amount = ticketFees;
            return View(objEventGuest);
        }
        public ActionResult GetEventRate(string MemberId, int EventId, string BookingDate, int Type)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (string.IsNullOrEmpty(MemberId))
                {
                    MemberId = "0";
                }

                List<GetMemberDetailsForGuestWithEventRateResult> objEventList = this.objIEventBookingForMemberCommand.GetEventRate(Convert.ToInt32(MemberId), EventId, BookingDate, Type);
                return this.FillGridEventGuest(Convert.ToInt32(MemberId), EventId, objEventList);

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }
        private ActionResult FillGridEventGuest(int page, int rows, List<GetMemberDetailsForGuestWithEventRateResult> objEventList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                //int totalRecords = objEventList != null && objEventList.Count > 0 ? objEventList[0].Total.Value : 0;
                int totalRecords = 2;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventList
                            select new
                            {
                                MemberName = objEvent.MemberName,
                                Relation = objEvent.Relation,
                                Age = objEvent.Age,
                                Photo = objEvent.ImageName,
                                GuestFees = objEvent.GuestFees,
                                CurrentOutstanding = objEvent.CurrentOutstanding,
                                MaximumTicketForFamilyMember = objEvent.MaximumTicketForFamilyMember,
                                IsMaximumTicketForFamilyMember = objEvent.IsMaximumTicketForFamilyMember,
                                Booked = objEvent.Booked,
                                GuestLimit = objEvent.GuestLimit,
                                CuurentBooking = objEvent.CuurentBooking,
                                NoOfSeats = objEvent.NoOfSeats,
                                CurrentTicketNumber = objEvent.CurrentTicketNumber,
                                ToSerialNo = objEvent.ToSerialNo,
                                ProceedFlag = objEvent.ProceedFlag,

                                //EventStartTime = objEvent.EventStartTime,
                                //EventEndTime = objEvent.EventEndTime,
                                //EvenLastRegistrationDate = objEvent.EvenLastRegistrationDate,
                                //ArtistsInfo = objEvent.ArtistsInfo,
                                //EventSynopsis = objEvent.EventSynopsis,
                                //TermsConditions = objEvent.TermsConditions,
                                //EventImage = objEvent.EventImage,
                                //EventBannerImage = objEvent.EventBannerImage,
                                //StepSeq = objEvent.StepSeq,
                                Id = objEvent.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveEventBookingForGuest(EventBookingForGuestModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                //objSave.EvnetId = Convert.ToInt32(11);
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }


                objSave.Id = (int)this.objIEventBookingForMemberCommand.SaveEventBookingForUmed(objSave);
                if (objSave.Id > 0)
                {
                    EventTicket(objSave.Id);
                    this.ViewData["Success"] = "1";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Success);
                    var responseObj = new { id = objSave.Id, message = Functions.AlertMessage("Event Booking", MessageType.Success) };
                    return this.Json(responseObj);
                }


                if (objSave.Id == -2)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Guest booking day limit over", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Guest booking day limit over, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -3)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("More than ticket end number", MessageType.Fail);

                    var responseObj = new { id = 0, message = Functions.AlertMessage("More than ticket end number, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                else
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objSave);
            }
            return this.View(objSave);
        }



        /// <summary>
        /// Gets the Event Fees.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpGet]
        public JsonResult GetEventFees(int eventId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EventBookingForMemberCommand dbEventFees = new EventBookingForMemberCommand();
                return this.Json(dbEventFees.GetEventFeesDropDown(eventId).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EventBookingForEventList()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            userCommand = new UserCommand();
            getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));
            EventBookingForEventListModel objEventBookingForMember = new EventBookingForEventListModel();

            try
            {

                List<EventList> lstGetAllEventLists = new List<EventList>();
                lstGetAllEventLists = objIEventBookingForMemberCommand.GetAllEvent();


                //objEventBookingForMember.EventFeesList = this.objIEventBookingForMemberCommand.GetEventFeesDropDown(0);
                this.objConfig = objCommon.AAAConfigSettings.Where(x => x.KeyName == "DocViewerRootFolderPath").FirstOrDefault();
                objEventBookingForMember.DocRootDirectoryPath = this.objConfig.KeyValue.ToString();
                objEventBookingForMember.EventList = lstGetAllEventLists;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return this.View(objEventBookingForMember);
        }

        public ActionResult EventBookingForSponsor()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EventBookingForSponsorModel objEventAffiliate = new EventBookingForSponsorModel();
                this.ViewData["SelectEventID"] = this.Request.QueryString["EventID"];
                return View(objEventAffiliate);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
        public ActionResult SaveEventBookingForUmedSponsor(EventBookingForSponsorModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                //objSave.EvnetId = Convert.ToInt32(11);
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }


                objSave.Id = (int)this.objIEventBookingForMemberCommand.SaveEventBookingForUmedSponsor(objSave);
                if (objSave.Id > 0)
                {
                    EventTicket(objSave.Id);
                    this.ViewData["Success"] = "1";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Success);
                    var responseObj = new { id = objSave.Id, message = Functions.AlertMessage("Event Booking", MessageType.Success) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -2)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Sponsor Member ID Not Found. Please Contact Administrator", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Sponsor Member ID Not Found. Please Contact Administrator, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -3)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("More than ticket end number", MessageType.Fail);

                    var responseObj = new { id = 0, message = Functions.AlertMessage("More than ticket end number, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }


                else
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objSave);
            }
            return this.View(objSave);
        }


        public ActionResult GetParentEventRate(string MemberId, int EventId, string BookingDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (string.IsNullOrEmpty(MemberId))
                {
                    MemberId = "0";
                }

                List<GetMemberDetailsForParentWithEventRateResult> objEventList = this.objIEventBookingForMemberCommand.GetParentEventRate(Convert.ToInt32(MemberId), EventId, BookingDate);
                return this.FillGridEventParent(Convert.ToInt32(MemberId), EventId, objEventList);

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }
        private ActionResult FillGridEventParent(int page, int rows, List<GetMemberDetailsForParentWithEventRateResult> objEventList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                //int totalRecords = objEventList != null && objEventList.Count > 0 ? objEventList[0].Total.Value : 0;
                int totalRecords = 2;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventList
                            select new
                            {
                                MemberName = objEvent.MemberName,
                                Relation = objEvent.Relation,
                                Age = objEvent.Age,
                                Photo = objEvent.ImageName,
                                GuestFees = objEvent.ParentFees,
                                CurrentOutstanding = objEvent.CurrentOutstanding,
                                MaximumTicketForFamilyMember = objEvent.MaximumTicketForFamilyMember,
                                IsMaximumTicketForFamilyMember = objEvent.IsMaximumTicketForFamilyMember,
                                Booked = objEvent.Booked,
                                GuestLimit = objEvent.ParentLimit,
                                CuurentBooking = objEvent.CuurentBooking,
                                NoOfSeats = objEvent.NoOfSeats,
                                CurrentTicketNumber = objEvent.CurrentTicketNumber,
                                ToSerialNo = objEvent.ToSerialNo,
                                ProceedFlag = objEvent.ProceedFlag,

                                //EventStartTime = objEvent.EventStartTime,
                                //EventEndTime = objEvent.EventEndTime,
                                //EvenLastRegistrationDate = objEvent.EvenLastRegistrationDate,
                                //ArtistsInfo = objEvent.ArtistsInfo,
                                //EventSynopsis = objEvent.EventSynopsis,
                                //TermsConditions = objEvent.TermsConditions,
                                //EventImage = objEvent.EventImage,
                                //EventBannerImage = objEvent.EventBannerImage,
                                //StepSeq = objEvent.StepSeq,
                                Id = objEvent.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EventBookingForParents()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                this.ViewData["SelectEventID"] = this.Request.QueryString["EventID"];
                EventBookingForParentsModel objEventParents = new EventBookingForParentsModel();
                objEventParents.RelationshipList = this.objIEventBookingForMemberCommand.GetRelationshipDropDown();
                objEventParents.MemberList = this.objIEventBookingForMemberCommand.GetMemberCodeDropdown();
                objEventParents.PaymentList = this.objIEventBookingForMemberCommand.GetPaymentDropDown();
                //objEventGuest.MemberId = 22293;
                //objEventParents.Amount = ticketFees;
                return View(objEventParents);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
        public ActionResult SaveEventBookingForParents(EventBookingForParentsModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                //objSave.EvnetId = Convert.ToInt32(11);
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }


                objSave.Id = (int)this.objIEventBookingForMemberCommand.SaveEventBookingForUmedParents(objSave);
                if (objSave.Id > 0)
                {
                    EventTicket(objSave.Id);
                    this.ViewData["Success"] = "1";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Success);
                    var responseObj = new { id = objSave.Id, message = Functions.AlertMessage("Event Booking", MessageType.Success) };
                    return this.Json(responseObj);
                }


                if (objSave.Id == -2)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Guest booking day limit over", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Guest booking day limit over", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -3)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("More than ticket end number", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("More than ticket end number", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                else
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objSave);
            }
            return this.View(objSave);
        }



        public ActionResult GetAffiliateEventRate(string MemberId, int EventId, string BookingDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (string.IsNullOrEmpty(MemberId))
                {
                    MemberId = "0";
                }

                List<GetMemberDetailsForAffWithEventRateResult> objEventList = this.objIEventBookingForMemberCommand.GetAffiliateEventRate(Convert.ToInt32(MemberId), EventId, BookingDate);
                return this.FillGridEventAffiliate(Convert.ToInt32(MemberId), EventId, objEventList);

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }
        private ActionResult FillGridEventAffiliate(int page, int rows, List<GetMemberDetailsForAffWithEventRateResult> objEventList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                //int totalRecords = objEventList != null && objEventList.Count > 0 ? objEventList[0].Total.Value : 0;
                int totalRecords = 2;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventList
                            select new
                            {
                                //MemberName = objEvent.MemberName,
                                //Relation = objEvent.Relation,
                                //Age = objEvent.Age,
                                //Photo = objEvent.ImageName,
                                GuestFees = objEvent.AffiliateFees,
                                CurrentOutstanding = objEvent.CurrentOutstanding,
                                MaximumTicketForFamilyMember = objEvent.MaximumTicketForFamilyMember,
                                IsMaximumTicketForFamilyMember = objEvent.IsMaximumTicketForFamilyMember,
                                Booked = objEvent.Booked,
                                GuestLimit = objEvent.ParentLimit,
                                CuurentBooking = objEvent.CuurentBooking,
                                NoOfSeats = objEvent.NoOfSeats,
                                CurrentTicketNumber = objEvent.CurrentTicketNumber,
                                ToSerialNo = objEvent.ToSerialNo,
                                ProceedFlag = objEvent.ProceedFlag,
                                Id = objEvent.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EventBookingForAffiliate()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                this.ViewData["SelectEventID"] = this.Request.QueryString["EventID"];
                EventBookingForAffiliateModel objEventAffiliate = new EventBookingForAffiliateModel();
                objEventAffiliate.MemberList = this.objIEventBookingForMemberCommand.GetMemberCodeDropdown();
                objEventAffiliate.AffiliateList = this.objIEventBookingForMemberCommand.GetAffiliateDropDown();
                objEventAffiliate.PaymentList = this.objIEventBookingForMemberCommand.GetPaymentDropDown();
                //objEventGuest.MemberId = 22293;
                //objEventAffiliate.Amount = ticketFees;
                return View(objEventAffiliate);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
        public ActionResult SaveEventBookingForUmedAffiliateClub(EventBookingForAffiliateModel objSave)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                //objSave.EvnetId = Convert.ToInt32(11);
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }


                objSave.Id = (int)this.objIEventBookingForMemberCommand.SaveEventBookingForUmedAffiliateClub(objSave);
                if (objSave.Id > 0)
                {
                    EventTicket(objSave.Id);
                    this.ViewData["Success"] = "1";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Success);

                    var responseObj = new { id = objSave.Id, message = Functions.AlertMessage("Event Booking", MessageType.Success) };
                    return this.Json(responseObj);
                }

                if (objSave.Id == -2)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Affiliate Member ID Not Found. Please Contact Administrator", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Affiliate Member ID Not Found. Please Contact Administrator, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                if (objSave.Id == -3)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("More than ticket end number", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("More than ticket end number, Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
                else
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Booking", MessageType.Fail);
                    var responseObj = new { id = 0, message = Functions.AlertMessage("Event Booking", MessageType.Fail) };
                    return this.Json(responseObj);
                }
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objSave);
            }
            return this.View(objSave);
        }


        public ActionResult EventBookingForTicketPrint()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                this.ViewData["EDID"] = this.Request.QueryString["Id"];
                this.ViewData["EMID"] = this.Request.QueryString["EMId"];
                EventBookingForPrintTokenModel objEventPrintToken = new EventBookingForPrintTokenModel();

                List<EventBookingForPrintTokenModel> otherList = new List<EventBookingForPrintTokenModel>();

                var list = this.objIEventBookingForMemberCommand.SearchEventPrintToken(0, 0, 0, Convert.ToInt32(this.Request.QueryString["EMId"]), Convert.ToInt32(this.Request.QueryString["Id"]));

                EventTicketCommand eventTicketCommand = new EventTicketCommand();



                foreach (var item in list)
                {
                    DateTime dateValue;
                    if (DateTime.TryParse(item.Date, out dateValue))
                    {
                        otherList.Add(new EventBookingForPrintTokenModel
                        {
                            Id = Convert.ToInt32(item.Id),
                            SeriolNo = Convert.ToInt32(item.SeriolNo),
                            MemberNo = item.MemberNo,
                            Member_Name = item.Member_Name,
                            MainMember = item.mainMember,
                            Type = item.Type,
                            Relation = item.Relation,
                            Date = item.Date,
                            ReceiptNo = item.RECEIPTNo.ToString(),
                            Photo = item.Photo,
                            //QR = eventTicketCommand.EventQRNAME(item.Id.ToString(), Convert.ToDateTime(item.Date))
                            QR = eventTicketCommand.EventQRNAME(item.Id.ToString(), dateValue)

                        });
                    }
                    else
                    {
                        logger.Error("Invalid date format for item.Id: " + item.Id);
                    }
                }
                objEventPrintToken.PrintList = otherList;
                return View(objEventPrintToken);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        public ActionResult EventBookingForPrintToken()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                this.ViewData["EDID"] = this.Request.QueryString["Id"];
                EventBookingForPrintTokenModel objEventPrintToken = new EventBookingForPrintTokenModel();
                objEventPrintToken.TicketType = this.objIEventBookingForMemberCommand.GetPaymentDropDown();
                return View(objEventPrintToken);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }
        public ActionResult BindEventPrintTokenGrid(int MemberNo, int SeriolNo, int TicketType, int EventBookingID)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                List<EventBookingPrintTokenResult> objEventList = this.objIEventBookingForMemberCommand.SearchEventPrintToken(MemberNo, SeriolNo, TicketType, EventBookingID, 0);
                return this.FillGridEventPrintToken(MemberNo, SeriolNo, TicketType, objEventList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }
        private ActionResult FillGridEventPrintToken(int page, int rows, int tickettype, List<EventBookingPrintTokenResult> objEventList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                //int totalRecords = objEventList != null && objEventList.Count > 0 ? objEventList[0].Total.Value : 0;
                int totalRecords = 2;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventList
                            select new
                            {
                                Id = objEvent.Id,
                                MemberNo = objEvent.MemberNo,
                                SeriolNo = objEvent.SeriolNo,
                                Type = objEvent.Type,
                                Member_Name = objEvent.Member_Name,
                                //MemberType = objEvent.MemberType,
                                Relation = objEvent.Relation,
                                Date = objEvent.Date,
                                ReceiptNo = objEvent.RECEIPTNo,
                                Photo = objEvent.Photo
                                //EventStartTime = objEvent.EventStartTime,
                                //EventEndTime = objEvent.EventEndTime,
                                //EvenLastRegistrationDate = objEvent.EvenLastRegistrationDate,
                                //ArtistsInfo = objEvent.ArtistsInfo,
                                //EventSynopsis = objEvent.EventSynopsis,
                                //TermsConditions = objEvent.TermsConditions,
                                //EventImage = objEvent.EventImage,
                                //EventBannerImage = objEvent.EventBannerImage,
                                //StepSeq = objEvent.StepSeq,
                                //Id = objEvent.id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public void EventTicket(int id)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                EventTicketCommand eventTicketCommand = new EventTicketCommand();

                var listEventBookingDetail = this.objIEventBookingForMemberCommand.GetEventBookingDetailResult(id);
                foreach (var item in listEventBookingDetail)
                {
                    //if (item.IsQRCode)
                    //{
                    //item.QRCodePath = 
                    eventTicketCommand.GenerateEventQRCode(item.Id.ToString(), item.BookDate);
                    //}
                    //else
                    //{
                    //    item.QRCodePath = eventTicketCommand.GenerateEventBarCode(item.EventBookingId.ToString(), null);
                    //}
                }

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                // return this.Json(string.Empty);
            }
        }


        public ActionResult EventBookingForPrintTokenPrint(string MemberNo)
        {
            return View();
        }

    }
}
