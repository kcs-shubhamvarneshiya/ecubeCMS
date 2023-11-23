// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-27-2016
// ***********************************************************************
// <copyright file="RoomBookingController.cs" company="string.Empty">
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
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class RoomBookingController.
    /// </summary>
    public class RoomBookingController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i room booking command.
        /// </summary>
        private readonly IRoomBookingCommand objIRoomBookingCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomBookingController"/> class.
        /// </summary>
        /// <param name="iRoomBookingCommand">The i room booking command.</param>
        public RoomBookingController(IRoomBookingCommand iRoomBookingCommand)
        {
            this.objIRoomBookingCommand = iRoomBookingCommand;
        }

        /// <summary>
        /// Rooms the booking view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult RoomBookingView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.RoomMaster));
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
        /// Rooms the booking.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult RoomBooking()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.RoomMaster));

                RoomBookingModel objRoomBookingModel = new RoomBookingModel();
                Random ran = new Random();
                objRoomBookingModel.HdnSession = ran.Next().ToString();

                long lgRoomBookingId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objRoomBookingModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgRoomBookingId = Request.QueryString.ToString().Decode().LongSafe();
                        objRoomBookingModel = this.objIRoomBookingCommand.GetRoomBookingByRoomBookingId(lgRoomBookingId);
                        objRoomBookingModel.HdnSession = ran.Next().ToString();
                        objRoomBookingModel.HdnImgProfilePic = objRoomBookingModel.ProfilePic;
                        objRoomBookingModel.HdnFlUploadTerms = objRoomBookingModel.Terms;
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.BindDropDownListForRoomBooking(objRoomBookingModel, true);
                this.CreateDetails(objRoomBookingModel.HdnSession, lgRoomBookingId);

                CommonDataContext objCommon = new CommonDataContext();
                AAAConfigSetting obj = new AAAConfigSetting();
                obj = objCommon.AAAConfigSettings.Where(x => x.KeyName == "DocViewerRootFolderPath").FirstOrDefault();
                objRoomBookingModel.HdnAaaConfig = obj.KeyValue.ToString();
                return this.View(objRoomBookingModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Rooms the booking.
        /// </summary>
        /// <param name="objRoomBooking">The object room booking.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult RoomBooking(RoomBookingModel objRoomBooking)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.RoomMaster));

                if (objRoomBooking.Id == 0)
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

                if (objRoomBooking.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIRoomBookingCommand.IsRoomBookingExists(objRoomBooking.Id, objRoomBooking.RoomBookingName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Room Booking", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateRoomBooking(objRoomBooking);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Room Booking Details", MessageType.Fail);
                    }
                    else
                    {
                        List<RoomBookingModel> objRoomBookingDetailList = (List<RoomBookingModel>)Session["RoomBookingDetail" + objRoomBooking.HdnSession];
                        List<RoomBookingModel> objRoomBookingGallaryList = (List<RoomBookingModel>)Session["RoomBookingGallary" + objRoomBooking.HdnSession];
                        if (this.Session["ProfilePic"] != null)
                        {
                            objRoomBooking.ProfilePic = this.Session["ProfilePic"].ToString();
                        }
                        else
                        {
                            objRoomBooking.ProfilePic = objRoomBooking.HdnImgProfilePic;
                        }

                        if (this.Session["TermsFile"] != null)
                        {
                            objRoomBooking.Terms = this.Session["TermsFile"].ToString();
                        }
                        else
                        {
                            objRoomBooking.Terms = objRoomBooking.HdnFlUploadTerms;
                        }

                        objRoomBooking.Id = this.objIRoomBookingCommand.SaveRoomBooking(objRoomBooking, objRoomBookingDetailList, objRoomBookingGallaryList);
                        if (objRoomBooking.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Room Booking", MessageType.Success);
                            this.BindDropDownListForRoomBooking(objRoomBooking, false);
                            return this.View(objRoomBooking);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Room Booking", MessageType.Fail);
                        }
                    }
                }

                this.BindDropDownListForRoomBooking(objRoomBooking, true);
                return this.View(objRoomBooking);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Room Booking", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                this.BindDropDownListForRoomBooking(objRoomBooking, true);
                return this.View(objRoomBooking);
            }
        }

        /// <summary>
        /// Binds the room booking grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindRoomBookingGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_SearchRoomBookingResult> objRoomBookingList = this.objIRoomBookingCommand.SearchRoomBooking(rows, page, search, sidx + " " + sord);
                ////  if (objRoomBookingList != null && objRoomBookingList.Count > 0)
                return this.FillGridRoomBooking(page, rows, objRoomBookingList);
                //// else return this.Json(string.Empty);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the room booking.
        /// </summary>
        /// <param name="strRoomBookingId">The string room booking identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteRoomBooking(string strRoomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strRoomBooking = strRoomBookingId.Split(',');
                strRoomBookingId = string.Empty;
                foreach (var item in strRoomBooking)
                {
                    strRoomBookingId += item.Decode() + ",";
                }

                strRoomBookingId = strRoomBookingId.Substring(0, strRoomBookingId.Length - 1);
                MS_DeleteRoomBookingResult result = this.objIRoomBookingCommand.DeleteRoomBooking(strRoomBookingId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Room Booking", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Room Booking", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Room Booking", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Room Booking", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the room booking.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetRoomBooking()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIRoomBookingCommand.GetAllRoomBookingForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Creates the details.
        /// </summary>
        /// <param name="strSessionId">The string session identifier.</param>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        public void CreateDetails(string strSessionId, long lgRoomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                Session.Add("RoomBookingDetail" + strSessionId, this.objIRoomBookingCommand.GetRoomBookingDetailByRoomBookingId(lgRoomBookingId));
                Session.Add("RoomBookingGallary" + strSessionId, this.objIRoomBookingCommand.GetRoomBookingGallaryByRoomBookingId(lgRoomBookingId));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Removes the sessions.
        /// </summary>
        /// <param name="strSessionId">The string session identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult RemoveSessions(string strSessionId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                Session.Remove("RoomBookingDetail" + strSessionId);
                Session.Remove("RoomBookingGallary" + strSessionId);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return this.Json(string.Empty);
        }

        /// <summary>
        /// Uploads the profile pic.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult UploadProfilePic(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (Request.Files["files"] != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            //// int MaxContentLength = 355 * 251 ; //3 MB

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
                                string strImagepath = Functions.GetSettings("DocRootFolderPath") + "\\Images\\RoomImage\\ProfilePic\\";
                                bool exists = Directory.Exists(strImagepath);
                                if (!exists)
                                {
                                    Directory.CreateDirectory(strImagepath);
                                }

                                var path = Path.Combine(strImagepath, fileName);
                                Stream strm = file.InputStream;
                                //// Functions.GenerateThumbnails(0.5, strm, path);
                                Request.Files["files"].SaveAs(strImagepath + Request.Files["files"].FileName);
                                this.Session["ProfilePic"] = file.FileName;
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
        /// Uploads the terms.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult UploadTerms(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (Request.Files["files"] != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            int maxContentLength = 1024 * 1024 * 3;

                            if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensionsForPDF))
                            {
                                this.ViewData["Success"] = "0";
                                this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensionsForPDF));
                            }
                            else if (file.ContentLength > maxContentLength)
                            {
                                this.ViewData["Success"] = "0";
                                this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Your file is too large, maximum allowed size is: " + maxContentLength + " ");
                            }
                            else
                            {
                                this.ViewData["Success"] = "1";
                                string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                                string strImagepath = Functions.GetSettings("DocRootFolderPath") + "\\PDF\\RoomBooking\\";
                                bool exists = Directory.Exists(strImagepath);
                                if (!exists)
                                {
                                    Directory.CreateDirectory(strImagepath);
                                }

                                var path = Path.Combine(strImagepath, fileName);
                                Stream strm = file.InputStream;
                                //// Functions.GenerateThumbnails(0.5, strm, path);

                                Request.Files["files"].SaveAs(strImagepath + Request.Files["files"].FileName);
                                this.Session["TermsFile"] = file.FileName;
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
        /// Rooms the booking detail.
        /// </summary>
        /// <param name="objRoomBooking">The object room booking.</param>
        /// <returns>JSON  Result.</returns>
        [HttpPost]
        public JsonResult RoomBookingDetail(RoomBookingModel objRoomBooking)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = this.ValidateRoomBookingDetail(objRoomBooking);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    return this.Json(strErrorMsg);
                }

                if (this.Session["RoomBookingDetail" + objRoomBooking.HdnSession] == null)
                {
                    this.CreateDetails(objRoomBooking.HdnSession, objRoomBooking.Id);
                }

                List<RoomBookingModel> objRoomBookingDetailList = (List<RoomBookingModel>)Session["RoomBookingDetail" + objRoomBooking.HdnSession];
                RoomBookingModel objRoomBookingModel = new RoomBookingModel();
                if (objRoomBooking.RoomBookingDetailId > 0)
                {
                    objRoomBookingModel = objRoomBookingDetailList.Where(x => x.Id == objRoomBooking.RoomBookingDetailId).FirstOrDefault();
                }

                if (objRoomBookingModel == null)
                {
                    objRoomBookingModel = new RoomBookingModel();
                }

                if (objRoomBookingModel.Id == 0)
                {
                    objRoomBookingModel.Id = objRoomBookingDetailList.Count() + 1;
                }

                objRoomBookingModel.TaxPercentage = objRoomBooking.TaxPercentage;
                objRoomBookingModel.TaxDescription = objRoomBooking.TaxDescription;
                objRoomBookingModel.IsDeleted = false;
                if (objRoomBooking.RoomBookingDetailId == 0)
                {
                    objRoomBookingDetailList.Add(objRoomBookingModel);
                }

                Session.Add("RoomBookingDetail" + objRoomBooking.HdnSession, objRoomBookingDetailList);
                return this.Json("1111");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Binds the room booking detail grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="hdnSession">The HDN session.</param>
        /// <param name="roomBookingId">The room booking identifier.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindRoomBookingDetailGrid(string sidx, string sord, int page, int rows, string filters, string hdnSession, long roomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.Session["RoomBookingDetail" + hdnSession] == null)
                {
                    this.CreateDetails(hdnSession, roomBookingId);
                }

                List<RoomBookingModel> objRoomBookingDetailList = (List<RoomBookingModel>)Session["RoomBookingDetail" + hdnSession];
                objRoomBookingDetailList = objRoomBookingDetailList.Where(x => x.IsDeleted == false).ToList();
                ////if (objRoomBookingDetailList != null && objRoomBookingDetailList.Count > 0)
                return this.FillGridRoomBookingDetail(page, rows, objRoomBookingDetailList);
                //// else return this.Json(string.Empty);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the room booking detail.
        /// </summary>
        /// <param name="strRoomBookingDetailId">The string room booking detail identifier.</param>
        /// <param name="strSessionId">The string session identifier.</param>
        /// <param name="lgRoomBookingId">The long room booking identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteRoomBookingDetail(string strRoomBookingDetailId, string strSessionId, long lgRoomBookingId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (this.Session["RoomBookingDetail" + strSessionId] == null)
                {
                    this.CreateDetails(strSessionId, lgRoomBookingId);
                    return this.Json(string.Empty);
                }

                List<RoomBookingModel> objRoomBookingDetailList = (List<RoomBookingModel>)Session["RoomBookingDetail" + strSessionId];
                string[] strRoomBookingDetail = strRoomBookingDetailId.Split(',');
                foreach (var item in strRoomBookingDetail)
                {
                    var result = objRoomBookingDetailList.FirstOrDefault(x => x.Id == item.LongSafe());
                    if (result != null)
                    {
                        result.IsDeleted = true;
                    }
                }

                Session.Add("RoomBookingDetail" + strSessionId, objRoomBookingDetailList);
                return this.Json("Success");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Fills the grid room booking detail.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objRoomBookingDetailList">The object room booking detail list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridRoomBookingDetail(int page, int rows, List<RoomBookingModel> objRoomBookingDetailList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = 50; // objRoomBookingDetailList != null ? objRoomBookingDetailList.Count : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objRoomBookingDetail in objRoomBookingDetailList
                            select new
                            {
                                Id = objRoomBookingDetail.Id.ToString(),
                                TaxPercentage = objRoomBookingDetail.TaxPercentage.ToString(),
                                TaxDescription = objRoomBookingDetail.TaxDescription.ToString()
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
        /// Validates the room booking detail.
        /// </summary>
        /// <param name="objRoomBookingDetail">The object room booking detail.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateRoomBookingDetail(RoomBookingModel objRoomBookingDetail)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
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
        /// Validates the room booking.
        /// </summary>
        /// <param name="objRoomBooking">The object room booking.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateRoomBooking(RoomBookingModel objRoomBooking)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objRoomBooking.RoomBookingName))
                {
                    strErrorMsg += Functions.AlertMessage("Room Booking Name", MessageType.InputRequired) + "<br/>";
                }

                if (this.Session["RoomBookingDetail" + objRoomBooking.HdnSession] == null)
                {
                    this.CreateDetails(objRoomBooking.HdnSession, objRoomBooking.Id);
                }

                List<RoomBookingModel> objRoomBookingDetailList = (List<RoomBookingModel>)Session["RoomBookingDetail" + objRoomBooking.HdnSession];
                if (objRoomBookingDetailList == null || objRoomBookingDetailList.Where(x => x.IsDeleted == false).Count() <= 0)
                {
                    strErrorMsg += Functions.AlertMessage("Room TAX Detail", MessageType.RecordInGridRequired) + "<br/>";
                }

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
        /// Fills the grid room booking.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objRoomBookingList">The object room booking list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridRoomBooking(int page, int rows, List<MS_SearchRoomBookingResult> objRoomBookingList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objRoomBookingList != null && objRoomBookingList.Count > 0 ? objRoomBookingList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objRoomBooking in objRoomBookingList
                            select new
                            {
                                RoomBookingName = objRoomBooking.RoomBookingName,
                                Description = objRoomBooking.Description,
                                Member = objRoomBooking.Member != null ? objRoomBooking.Member.Value.ToString() : string.Empty,
                                Guest = objRoomBooking.Guest != null ? objRoomBooking.Guest.Value.ToString() : string.Empty,
                                ProfilePic = objRoomBooking.ProfilePic,
                                Terms = objRoomBooking.Terms,
                                Id = objRoomBooking.Id.ToString().Encode()
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
        /// Binds the drop down list for room booking.
        /// </summary>
        /// <param name="objRoomBookingModel">The object room booking model.</param>
        /// <param name="blBindDropDownFromDb">IF set to <c>true</c> [BL bind drop down from database].</param>
        private void BindDropDownListForRoomBooking(RoomBookingModel objRoomBookingModel, bool blBindDropDownFromDb)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (blBindDropDownFromDb)
                {
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }
    }
}
