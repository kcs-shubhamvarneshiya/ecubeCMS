// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="EventBookingController.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace EcubeWebPortalCMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Newtonsoft.Json;
    using Serilog;

    /// <summary>
    /// Class EventBookingController.
    /// </summary>
    public class EventBookingController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i event booking command.
        /// </summary>
        private readonly IEventBookigCommand objIEventBookingCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        CommonDataContext objCommon = new CommonDataContext();
        AAAConfigSetting objConfig = new AAAConfigSetting();


        /// <summary>
        /// Initializes a new instance of the <see cref="EventBookingController"/> class.
        /// </summary>
        /// <param name="iEventBookingCommand">The i event booking command.</param>
        public EventBookingController(IEventBookigCommand iEventBookingCommand)
        {
            this.objIEventBookingCommand = iEventBookingCommand;
            this.objConfig = objCommon.AAAConfigSettings.Where(x => x.KeyName == "ClubName2").FirstOrDefault();

        }

        /// <summary>
        /// Events the category.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult EventBooking()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));
                string cacheclear = DateTime.Now.ToString("ddMMyyhhmmssffff");
                EventBookingModel objEventModel = new EventBookingModel();
                int lgEventId = 0;
                this.ViewData["ClubName"] = this.objConfig.KeyValue;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objEventModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgEventId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objEventModel = this.objIEventBookingCommand.GetEventByEventId(lgEventId);
                        objEventModel.RegistrationDate = objEventModel.EvenLastRegistrationDate.ToString("dd/MM/yyyy").Replace("-", "/");

                        this.Session["EventImage"] = objEventModel.EventImage;
                        string eventPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/CMSUpload/Images/Event/EventImage/";
                        objEventModel.EventImage = objEventModel.EventImage == null ? string.Empty : eventPath + Convert.ToString(objEventModel.EventImage) + "?" + cacheclear;

                        this.Session["EventBannerImage"] = objEventModel.EventBannerImage;
                        string eventBannerPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/CMSUpload/Images/Event/EventBannerImage/";
                        objEventModel.EventBannerImage = objEventModel.EventBannerImage == null ? string.Empty : eventBannerPath + Convert.ToString(objEventModel.EventBannerImage) + "?" + cacheclear;

                        this.Session["EventAttachment"] = objEventModel.EventAttachmentDisplay;
                        string eventAttachmentPath = Request.Url.GetLeftPart(UriPartial.Authority) + "/CMSUpload/Images/Event/EventAttachment/";
                        objEventModel.EventAttachment = objEventModel.EventAttachment == null ? string.Empty : eventAttachmentPath + Convert.ToString(objEventModel.EventAttachmentDisplay);


                        if (objEventModel.EventMobileImage != null && objEventModel.EventMobileImage != string.Empty)
                        {
                            objEventModel.EventMobileImage = "../CMSUpload/Document/Event/" + objEventModel.Id + "/Original.jpg" + "?" + cacheclear;
                        }
                    }
                }
                else
                {
                    this.Session["EventAttachment"] = null;
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }

                    objEventModel.AccountHeadId = -1;
                }

                objEventModel.TaxMasterList = this.objIEventBookingCommand.GetAllTaxMasterForDropDown();
                objEventModel.AccountHeadList = this.objIEventBookingCommand.GetAllAccountHeadForDropDown();
                return this.View(objEventModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// CMSs the Event Category View.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult EventBookingView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                CommonDataContext objCommon = new CommonDataContext();
                AAAConfigSetting obj = new AAAConfigSetting();
                obj = objCommon.AAAConfigSettings.Where(x => x.KeyName == "DocViewerRootFolderPath").FirstOrDefault();


                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
                return this.View(obj);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }
        /// <summary>
        /// Gets the event category.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpGet]
        public JsonResult GetEventCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                EventCategoryCommand dbCategory = new EventCategoryCommand();
                return this.Json(dbCategory.GetAllEventCategoryForDropDown().ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the event category.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpGet]
        public JsonResult GetEventTicketCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                EventTicketCategoryCommand dbCategory = new EventTicketCategoryCommand();
                return this.Json(dbCategory.GetAllEventTicketCategoryForDropDown().ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the Account Head.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpGet]
        public JsonResult GetAccountHead()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                EventBookigCommand dbAcHead = new EventBookigCommand();
                return this.Json(dbAcHead.GetAllAccountHeadForDropDown().ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Uploads the event image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadEventImage(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();


            if (this.Request.Files["files"] != null)
            {
                if (this.Request.Files.Count > 0)
                {
                    var file = this.Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] bytes = new byte[20];
                        file.InputStream.Read(bytes, 0, 20);
                        if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensions))
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensions));
                        }
                        else if (!Functions.ValidateUploadedFileHeader(file.FileName, bytes, Functions.AllowedFileExtensions))
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensions));
                        }
                        else
                        {
                            this.ViewData["Success"] = "1";
                            string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                            string strImagepath = this.Request.MapPath("../CMSUpload/Images/Event/EventImage");

                            bool exists = Directory.Exists(strImagepath);
                            ////  var tempPath = "E:\\KCS\\Karnavati\\Karnavati_QC\\CMSUpload\\Images\\Event\\EventImage\\";

                            if (!exists)
                            {
                                Directory.CreateDirectory(strImagepath);
                                ////Directory.CreateDirectory(tempPath);
                            }

                            var path = Path.Combine(strImagepath, fileName);
                            Image oldImage = Bitmap.FromStream(file.InputStream);
                            Image newImage = this.ResizeImage(oldImage, 1200, 418);
                            try
                            {
                                newImage.Save(path, ImageFormat.Jpeg);
                            }
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);
                            }

                            newImage.Dispose();
                            this.Session["EventImage"] = file.FileName;
                        }
                    }
                }
            }


            return this.Json("1111");
        }

        /// <summary>
        /// Uploads the event banner image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadEventBannerImage(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            if (this.Request.Files["files"] != null)
            {
                if (this.Request.Files.Count > 0)
                {
                    var file = this.Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] bytes = new byte[20];
                        file.InputStream.Read(bytes, 0, 20);
                        if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensions))
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensions));
                        }
                        else if (!Functions.ValidateUploadedFileHeader(file.FileName, bytes, Functions.AllowedFileExtensions))
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensions));
                        }
                        else
                        {
                            this.ViewData["Success"] = "1";
                            string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                            string strImagepath = this.Request.MapPath("../CMSUpload/Images/Event/EventBannerImage");// Functions.GetSettings("DocRootFolderPath") + "\\Images\\Event\\EventBannerImage\\";
                            bool exists = Directory.Exists(strImagepath);
                            //// var tempPath = "E:\\KCS\\Karnavati\\Karnavati_QC\\CMSUpload\\Images\\Event\\EventBannerImage\\";
                            if (!exists)
                            {
                                Directory.CreateDirectory(strImagepath);
                                //// Directory.CreateDirectory(tempPath);
                            }

                            var path = Path.Combine(strImagepath, fileName);
                            Image oldImage = Bitmap.FromStream(file.InputStream);
                            Image newImage = this.ResizeImage(oldImage, 1200, 418);
                            try
                            {
                                newImage.Save(path, ImageFormat.Jpeg);
                            }
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);
                            }

                            newImage.Dispose();
                            this.Session["EventBannerImage"] = file.FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        public JsonResult UploadEventAttachment(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (this.Request.Files["files"] != null)
                {
                    if (this.Request.Files.Count > 0)
                    {
                        var file = this.Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            byte[] bytes = new byte[20];
                            file.InputStream.Read(bytes, 0, 20);
                            if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensionsForAttachment))
                            {
                                this.ViewData["Success"] = "0";
                                this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensionsForAttachment));
                            }
                            else if (!Functions.ValidateUploadedFileHeader(file.FileName, bytes, Functions.AllowedFileExtensionsForAttachment))
                            {
                                this.ViewData["Success"] = "0";
                                this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensionsForAttachment));
                            }
                            else
                            {
                                this.ViewData["Success"] = "1";
                                string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                                string strAttachmentpath = this.Request.MapPath("../CMSUpload/Images/Event/EventAttachment/");
                                bool exists = Directory.Exists(strAttachmentpath);
                                //// var tempPath = "E:\\KCS\\Karnavati\\Karnavati_QC\\CMSUpload\\Images\\Event\\EventBannerImage\\";
                                if (!exists)
                                {
                                    Directory.CreateDirectory(strAttachmentpath);
                                    //// Directory.CreateDirectory(tempPath);
                                }

                                var path = Path.Combine(strAttachmentpath, fileName);
                                Stream strm = file.InputStream;
                                //// Functions.GenerateThumbnails(0.5, strm, path);
                                Request.Files["files"]
                                       .SaveAs(strAttachmentpath + Request.Files["files"].FileName);
                                this.Session["EventAttachment"] = file.FileName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }

            return this.Json("1111");
        }

        /// <summary>
        /// Saves the event category.
        /// </summary>
        /// <param name="objEventBooking">The object event booking.</param>
        /// <returns>Action Result.</returns>
        public ActionResult EventSave(EventBookingModel objEventBooking)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));
                string cashecleaner = DateTime.Now.ToString("ddMMyyhhmmssffff");
                if (objEventBooking.Id == 0)
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (getPageRights.Edit == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objEventBooking.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIEventBookingCommand.IsEventExists(objEventBooking.Id, objEventBooking.EventTitle);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event", MessageType.AlreadyExist);
                    return this.Json(Functions.AlertMessage("Event", MessageType.AlreadyExist));
                }
                else
                {
                    string strErrorMsg = this.ValidateEvent(objEventBooking);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                        return this.Json(strErrorMsg);
                    }
                    else
                    {
                        if (this.Session["EventImage"] != null)
                        {
                            objEventBooking.EventImage = this.Session["EventImage"].ToString().Replace(" ", string.Empty).Replace("_", string.Empty);
                        }
                        else
                        {
                            objEventBooking.EventImage = string.IsNullOrEmpty(objEventBooking.HfnEventImage) ? objEventBooking.HfnEventImage : objEventBooking.HfnEventImage.Replace(" ", string.Empty).Replace("_", string.Empty);
                        }

                        if (this.Session["EventBannerImage"] != null)
                        {
                            objEventBooking.EventBannerImage = this.Session["EventBannerImage"].ToString().Replace(" ", string.Empty).Replace("_", string.Empty);
                        }
                        else
                        {
                            objEventBooking.EventBannerImage = string.IsNullOrEmpty(objEventBooking.HdnEventBannerImage) ? objEventBooking.HdnEventBannerImage : objEventBooking.HdnEventBannerImage.Replace(" ", string.Empty).Replace("_", string.Empty);
                        }
                        if (this.Session["EventAttachment"] != null)
                        {
                            objEventBooking.EventAttachment = this.Session["EventAttachment"].ToString() + "?" + cashecleaner;
                        }
                        else
                        {
                            objEventBooking.EventAttachment = string.IsNullOrEmpty(objEventBooking.HfnEventAttachment) ? objEventBooking.HfnEventAttachment : objEventBooking.HfnEventAttachment + "?" + cashecleaner;
                        }

                        if (this.Session["EventMobileImage"] != null)
                        {
                            objEventBooking.EventMobileImage = this.Session["EventMobileImage"].ToString().Replace(" ", string.Empty).Replace("_", string.Empty);
                        }
                        else
                        {
                            objEventBooking.EventMobileImage = string.IsNullOrEmpty(objEventBooking.HdnEventMobileImage) ? objEventBooking.HdnEventMobileImage : objEventBooking.HdnEventMobileImage.Replace(" ", string.Empty).Replace("_", string.Empty);
                        }

                        objEventBooking.Id = this.objIEventBookingCommand.SaveEvent(objEventBooking);
                        if (objEventBooking.Id > 0)
                        {
                            this.ViewData["blAddScheduleRights"] = getPageRights.Add;
                            this.ViewData["blEditScheduleRights"] = getPageRights.Edit;
                            this.ViewData["blDeleteScheduleRights"] = getPageRights.Delete;
                            //this.Session["EventImage"] = null;
                            //this.Session["EventBannerImage"] = null;
                            //this.Session["EventAttachment"] = null;
                            this.ViewData["Success"] = "1";
                            if (this.Session["EventImagePath"] != null)
                            {
                                Functions.ReSizeBanner(this.Session["EventImagePath"].ToString(), this.Session["EventMobileImage"].ToString(), Convert.ToString(objEventBooking.Id), "Event");
                            }

                            this.ViewData["Message"] = Functions.AlertMessage("Event", MessageType.Success);

                            return this.Json(objEventBooking.Id);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event", MessageType.Fail);
                            return this.Json(Functions.AlertMessage("Event", MessageType.Fail));
                        }
                    }
                }

                return this.View(objEventBooking);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEventBooking);
            }
        }

        /// <summary>
        /// Saves the event schedule.
        /// </summary>
        /// <param name="objEventSchedule">The object event schedule.</param>
        /// <returns>Action Result.</returns>
        public ActionResult SaveEventSchedule(EventScheduleModel objEventSchedule)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (objEventSchedule.Id == 0)
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (getPageRights.Edit == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objEventSchedule.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false;
                if (objEventSchedule.Id == 0)
                {
                    blExists = this.objIEventBookingCommand.IsEventScheduleExists(objEventSchedule.EventId, Functions.ConvertDateTime(objEventSchedule.FromDate), Functions.ConvertDateTime(objEventSchedule.ToDate));
                }

                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Schedule", MessageType.AlreadyExist);
                    return this.Json(Functions.AlertMessage("Event Schedule", MessageType.AlreadyExist));
                }
                else
                {
                    string strErrorMsg = string.Empty;

                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                        return this.Json("Even last registration date should be smaller than event dates.");
                    }
                    else
                    {
                        objEventSchedule.EventFromDate = Functions.ConvertDateTime(objEventSchedule.FromDate);
                        objEventSchedule.EventToDate = Functions.ConvertDateTime(objEventSchedule.ToDate);
                        objEventSchedule.Id = this.objIEventBookingCommand.SaveEventSchedule(objEventSchedule);
                        if (objEventSchedule.Id > 0)
                        {
                            this.ViewData["blAddRateRights"] = getPageRights.Add;
                            this.ViewData["blEditRateRights"] = getPageRights.Edit;
                            this.ViewData["blDeleteRateRights"] = getPageRights.Delete;

                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Schedule", MessageType.Success);
                            //// return this.View(objEventSchedule);
                            return this.Json(Functions.AlertMessage("Event Schedule", MessageType.Success));
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Schedule", MessageType.Fail);
                            return this.Json(Functions.AlertMessage("Event Schedule", MessageType.Fail));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Schedule", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEventSchedule);
            }
        }

        /// <summary>
        /// Saves the event rate.
        /// </summary>
        /// <param name="objEvent">The object event Parameter.</param>
        /// <returns>ActionResult save event rate.</returns>
        public ActionResult SaveEventRate(EventBookingModel objEvent)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                objEvent.ObjEventRateModel.EventId = Convert.ToInt32(objEvent.Id);
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.CreateEvent));

                if (objEvent.ObjEventRateModel.Id == 0)
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (getPageRights.Edit == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objEvent.ObjEventRateModel.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false;
                if (objEvent.ObjEventRateModel.Id == 0)
                {
                    blExists = this.objIEventBookingCommand.IsEventRateExists(objEvent.ObjEventRateModel.EventId, objEvent.ObjEventRateModel.EventTicketCategoryId);
                }

                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.AlreadyExist);
                    return this.Json(Functions.AlertMessage("Event Rate", MessageType.AlreadyExist));
                }
                else
                {
                    string strErrorMsg = string.Empty; ///// this.ValidateEvent(objEventSchedule);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objEvent.ObjEventRateModel.Id = this.objIEventBookingCommand.SaveEventRate(objEvent.ObjEventRateModel);
                        if (objEvent.ObjEventRateModel.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Success);
                            //// return this.View(objEventRate);
                            return this.Json(Functions.AlertMessage("Event Rate", MessageType.Success));
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Fail);
                            return this.Json(Functions.AlertMessage("Event Rate", MessageType.Fail));
                        }
                    }
                }

                return this.View(objEvent.ObjEventRateModel);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Rate", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEvent.ObjEventRateModel);
            }
        }

        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="strEventId">The string event identifier.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult DeleteEvent(string strEventId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strEvent = strEventId.Split(',');
                strEventId = string.Empty;
                foreach (var item in strEvent)
                {
                    strEventId += item.Decode() + ",";
                }

                strEventId = strEventId.Substring(0, strEventId.Length - 1);
                MS_DeleteEventResult result = this.objIEventBookingCommand.DeleteEvent(strEventId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Event ", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Event ", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Event ", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Event ", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Deletes the event schedule.
        /// </summary>
        /// <param name="strEventscheduleId">The string event schedule identifier.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult DeleteEventSchedule(string strEventscheduleId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                MS_DeleteEventscheduleResult result = this.objIEventBookingCommand.DeleteEventSchedule(strEventscheduleId, Convert.ToInt64(MySession.Current.UserId));
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Event Schedule", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Event Schedule", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Event Schedule", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

                return this.Json(Functions.AlertMessage("Event Schedule", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="strEventRateId">The string event identifier.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult DeleteEventRate(string strEventRateId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                ////string[] strEvent = strEventRateId.Split(',');
                ////strEventRateId = string.Empty;
                ////foreach (var item in strEvent)
                ////{
                ////    strEventRateId += item.Decode() + ",";
                ////}
                ////strEventRateId = strEventRateId.Substring(0, strEventRateId.Length - 1);
                MS_DeleteEventRateResult result = this.objIEventBookingCommand.DeleteEventRate(strEventRateId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Event Rate", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Event Rate", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Event Rate", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Event Rate", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Binds the Bind Event Category Grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <param name="compeletedEvent">The completed Event.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindEventGrid(string sidx, string sord, int page, int rows, string filters, string search, bool compeletedEvent)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_SerachEventResult> objEventList = this.objIEventBookingCommand.SearchEvent(rows, page, search, sidx + " " + sord, compeletedEvent == true ? 1 : 0);
                return this.FillGridEvent(page, rows, objEventList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Binds the event schedule grid.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="sidx">PARAMETER SIDX.</param>
        /// <param name="sord">PARAMETER SORD.</param>
        /// <param name="page">PARAMETER page.</param>
        /// <param name="rows">PARAMETER rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action Result.</returns>
        public ActionResult BindEventScheduleGrid(int eventId, string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                ////  int eventId = 25;
                List<MS_SearchEventScheduleResult> objEventScheduleList = this.objIEventBookingCommand.SearchEventSchedule(eventId, rows, page, search, sidx + " " + sord);
                return this.FillGridEventSchedule(page, rows, objEventScheduleList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Binds the event rate grid.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="sidx">PARAMETER SIDX.</param>
        /// <param name="sord">PARAMETER SORD.</param>
        /// <param name="page">PARAMETER page.</param>
        /// <param name="rows">PARAMETER rows.</param>
        /// <param name="filters">PARAMETER filters.</param>
        /// <param name="search">PARAMETER search.</param>
        /// <returns>Action Result.</returns>
        public ActionResult BindEventRateGrid(int eventId, string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_SearchEventRateResult> objEventRateList = this.objIEventBookingCommand.SearchEventRate(eventId, rows, page, search, sidx + " " + sord);
                return this.FillGridEventRate(page, rows, objEventRateList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Binds the event report grid.
        /// </summary>
        /// <param name="sidx">PARAMETER SIDX.</param>
        /// <param name="sord">PARAMETER SORD.</param>
        /// <param name="page">PARAMETER page.</param>
        /// <param name="rows">PARAMETER rows.</param>
        /// <param name="eventId">The event identifier parameter.</param>
        /// <param name="ticketId">The ticket identifier parameter.</param>
        /// <param name="memberCode">The member code parameter.</param>
        /// <returns>ActionResult bind event report grid.</returns>
        public ActionResult BindEventReportGrid(string sidx, string sord, int page, int rows, string eventId, string ticketId, string memberCode, string eventDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_EventBookingReportResult> objEventList = new List<MS_EventBookingReportResult>();
                if (!string.IsNullOrEmpty(eventDate) && !eventDate.Contains("__/__/____"))
                {
                    
                    objEventList = this.objIEventBookingCommand.EventBookingReport(eventId.IntSafe(), ticketId.IntSafe(), memberCode, Functions.ConvertDateTime(eventDate), rows, page);
                }
                return this.EventReportGrid(page, rows, objEventList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Events the booking report.
        /// </summary>
        /// <returns>ActionResult event booking report.</returns>
        public ActionResult EventBookingReport()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventBookingReport));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                EventBookingReportModel objEventBookingReportModel = new EventBookingReportModel();
                return this.View(objEventBookingReportModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Uploads the event banner image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadMobileImage(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            if (this.Request.Files["files"] != null)
            {
                if (this.Request.Files.Count > 0)
                {
                    var file = this.Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] bytes = new byte[20];
                        file.InputStream.Read(bytes, 0, 20);
                        if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensions))
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensions));
                        }
                        else
                        {
                            this.ViewData["Success"] = "1";
                            string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                            string strImagepath = this.Request.MapPath("../CMSUpload/Document/");
                            bool exists = Directory.Exists(strImagepath);
                            if (!exists)
                            {
                                Directory.CreateDirectory(strImagepath);
                            }

                            var path = Path.Combine(strImagepath, fileName);
                            Image newImage = Bitmap.FromStream(file.InputStream);
                            try
                            {
                                newImage.Save(path, ImageFormat.Jpeg);
                            }
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);
                            }

                            newImage.Dispose();
                            this.Session["EventImagePath"] = strImagepath;
                            this.Session["EventMobileImage"] = file.FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        /// <summary>
        /// Fills the grid event.
        /// </summary>
        /// <param name="page">PARAMETER page.</param>
        /// <param name="rows">PARAMETER rows.</param>
        /// <param name="objEventList">The object event list.</param>
        /// <returns>Action Result.</returns>
        private ActionResult FillGridEvent(int page, int rows, List<MS_SerachEventResult> objEventList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objEventList != null && objEventList.Count > 0 ? objEventList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventList
                            select new
                            {
                                EventCategoryName = objEvent.EventCategoryName,
                                EventTitle = objEvent.EventTitle,
                                EventScreen = objEvent.EventScreen,
                                EventPlace = objEvent.EventPlace,
                                EventDuration = objEvent.EventDuration,
                                EventStartTime = objEvent.EventStartTime,
                                EventEndTime = objEvent.EventEndTime,
                                EvenLastRegistrationDate = objEvent.EvenLastRegistrationDate,
                                ArtistsInfo = objEvent.ArtistsInfo,
                                EventSynopsis = objEvent.EventSynopsis,
                                TermsConditions = objEvent.TermsConditions,
                                EventImage = objEvent.EventImage,
                                EventBannerImage = objEvent.EventBannerImage,
                                StepSeq = objEvent.StepSeq,
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

        /// <summary>
        /// Fills the grid event schedule.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objEventScheduleList">The object event schedule list.</param>
        /// <returns>Action Result.</returns>
        private ActionResult FillGridEventSchedule(int page, int rows, List<MS_SearchEventScheduleResult> objEventScheduleList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objEventScheduleList != null && objEventScheduleList.Count > 0 ? objEventScheduleList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEvent in objEventScheduleList
                            select new
                            {
                                EventDate = Convert.ToDateTime(objEvent.EventDate),
                                Id = objEvent.Id
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

        /// <summary>
        /// Fills the grid event rate.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objEventRateList">The object event rate list.</param>
        /// <returns>Action Result.</returns>
        private ActionResult FillGridEventRate(int page, int rows, List<MS_SearchEventRateResult> objEventRateList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objEventRateList != null && objEventRateList.Count > 0 ? objEventRateList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                if (this.objConfig.KeyValue.ToString() != "UmedClub")
                {
                    var jsonData = new
                    {
                        total = totalPages,
                        page,
                        records = totalRecords,
                        rows = (from objEvent in objEventRateList
                                select new
                                {
                                    EventId = (int)objEvent.EventId,
                                    EventTicketCategoryId = (int)objEvent.EventTicketCategoryId,
                                    MemberFees = (decimal)objEvent.MemberFees,
                                    GuestFees = (decimal)objEvent.GuestFees,
                                    SubMember = (decimal)objEvent.SubMember,
                                    NoOfSeats = (int)objEvent.NoOfSeats,
                                    //// AddBy : Karan Shah on Date : 29-MAR-2017
                                    MemberSeatLimit = (int)objEvent.MemberSeatLimit,
                                    GuestSeatLimit = (int)objEvent.GuestSeatLimit,
                                    //// End
                                    Id = objEvent.Id,
                                    EventTicketCategoryName = objEvent.EventTicketCategoryName
                                }).ToArray()
                    };
                    return this.Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var jsonData = new
                    {
                        total = totalPages,
                        page,
                        records = totalRecords,
                        rows = (from objEvent in objEventRateList
                                select new
                                {
                                    EventId = (int)objEvent.EventId,
                                    EventTicketCategoryId = (int)objEvent.EventTicketCategoryId,
                                    MemberFees = (decimal)objEvent.MemberFees,
                                    GuestFees = (decimal)objEvent.GuestFees,
                                    SubMember = (decimal)objEvent.SubMember,
                                    AffiliateMemberFee = (decimal)objEvent.AffiliateMemberFee,
                                    ParentFee = (decimal)objEvent.ParentFee,
                                    GuestChildFee = (decimal)objEvent.GuestChildFee,
                                    GuestRoomFee = (decimal)objEvent.GuestRoomFee,
                                    FromSerialNo = (int)objEvent.FromSerialNo,
                                    ToSerialNo = (int)objEvent.ToSerialNo,
                                    NoOfSeats = (int)objEvent.NoOfSeats,
                                    //// End
                                    Id = objEvent.Id,
                                    EventTicketCategoryName = objEvent.EventTicketCategoryName
                                }).ToArray()
                    };
                    return this.Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Events the report grid.
        /// </summary>
        /// <param name="page">The page parameter.</param>
        /// <param name="rows">The rows parameter.</param>
        /// <param name="objEventBookingReport">The object event booking report parameter.</param>
        /// <returns>ActionResult event report grid.</returns>
        private ActionResult EventReportGrid(int page, int rows, List<MS_EventBookingReportResult> objEventBookingReport)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (objEventBookingReport != null && objEventBookingReport.Any())
                {
                    int pageSize = rows;
                    var firstRecord = objEventBookingReport.FirstOrDefault();
                    int totalRecords = firstRecord?.Total ?? 0; // Check if firstRecord is null
                    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                    var jsonData = new
                    {
                        total = totalPages,
                        page,
                        records = totalRecords,
                        rows = (from objEvent in objEventBookingReport
                                select new
                                {
                                    MemberName = objEvent.MemberName,
                                    MemberType = objEvent.MemberType,
                                    MCodeWithPrefix = objEvent.MCodeWithPrefix,
                                    BookingId = objEvent.BookingId,
                                    IsCheckedIn = objEvent.IsCheckedIn ? "Yes" : "No",
                                    CheckedInTime = objEvent.CheckedInTime == null ? string.Empty : objEvent.CheckedInTime.Value.ToString("dd/MM/yyyy hh:mm tt"), // Fixed date format
                                    CheckedInBy = objEvent.CheckedInBy
                                }).ToArray()
                    };
                    return this.Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Handle the case where objEventBookingReport is null or empty
                    return this.Json(new { total = 0, page, records = 0, rows = new object[0] }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the event.
        /// </summary>
        /// <param name="objEventBooking">The object event booking.</param>
        /// <returns>System String.</returns>
        private string ValidateEvent(EventBookingModel objEventBooking)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (objEventBooking.EventStartTime != null && objEventBooking.EventEndTime != null)
                {
                    var currentDate = DateTime.Now;
                    string eventStartTime = currentDate.Year.ToString() + "-" + currentDate.Month.ToString() + "-" + currentDate.Day.ToString() + " " + objEventBooking.EventStartTime;
                    string eventEndTime = currentDate.Year.ToString() + "-" + currentDate.Month.ToString() + "-" + currentDate.Day.ToString() + " " + objEventBooking.EventEndTime;
                    DateTime dateEventStartTime = Convert.ToDateTime(eventStartTime);
                    DateTime dateEventEndTime = Convert.ToDateTime(eventEndTime);

                    if (dateEventStartTime >= dateEventEndTime)
                    {
                        strErrorMsg = "Event End Time should be greater than Event Start Time.";
                    }
                }

                ////if (this.IsSubmitAllowed("Event", objEventBooking.RegistrationDate, string.Empty, objEventBooking.Id))
                ////{
                ////    strErrorMsg = "Even last registration date should be smaller than event dates.";
                ////}

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="image">The image parameter.</param>
        /// <param name="width">The width parameter.</param>
        /// <param name="height">The height parameter.</param>
        /// <returns>System.Drawing.Bitmap resize image.</returns>
        private System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }


        /// <summary>
        /// Gets the event For Report By Event Date.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpGet]
        public JsonResult GetAllEventForReportByEventDate(DateTime eventDate)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                EventBookigCommand dbCategory = new EventBookigCommand();
                return this.Json(dbCategory.GetAllEventForReport(eventDate).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }


    }
}