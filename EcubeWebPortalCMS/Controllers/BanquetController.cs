// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-27-2016
// ***********************************************************************
// <copyright file="BanquetController.cs" company="string.Empty">
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
    /// Class BanquetController.
    /// </summary>
    public class BanquetController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i banquet command.
        /// </summary>
        private readonly IBanquetCommand objIBanquetCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanquetController"/> class.
        /// </summary>
        /// <param name="iBanquetCommand">The i banquet command.</param>
        public BanquetController(IBanquetCommand iBanquetCommand)
        {
            this.objIBanquetCommand = iBanquetCommand;
        }

        /// <summary>
        /// Banquets the view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult BanquetView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetHallMaster));
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
        /// Banquets this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Banquet()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetHallMaster));

                BanquetModel objBanquetModel = new BanquetModel();
                Random ran = new Random();
                objBanquetModel.HdnSession = ran.Next().ToString();
                long lgBanquetId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objBanquetModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgBanquetId = Request.QueryString.ToString().Decode().LongSafe();
                        objBanquetModel = this.objIBanquetCommand.GetBanquetByBanquetId(lgBanquetId);
                        objBanquetModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.BindDropDownListForBanquet(objBanquetModel, true);
                this.CreateDetails(objBanquetModel.HdnSession, lgBanquetId);
                //// HdnAaaConfig
                CommonDataContext objCommon = new CommonDataContext();
                AAAConfigSetting obj = new AAAConfigSetting();
                obj = objCommon.AAAConfigSettings.Where(x => x.KeyName == "DocViewerRootFolderPath").FirstOrDefault();
                objBanquetModel.HdnAAAConfig = obj.KeyValue.ToString();
                objBanquetModel.HdnImgProfilePic = objBanquetModel.ProfilePic;
                objBanquetModel.HdnFlUploadTerms = objBanquetModel.Terms;
                return this.View(objBanquetModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Banquets the specified object banquet.
        /// </summary>
        /// <param name="objBanquet">The object banquet.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult Banquet(BanquetModel objBanquet)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetHallMaster));

                if (objBanquet.Id == 0)
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

                if (objBanquet.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIBanquetCommand.IsBanquetExists(objBanquet.Id, objBanquet.BanquetName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Banquet", MessageType.AlreadyExist);
                }
                else
                {
                    // string strErrorMsg = ValidateBanquet(objBanquet);
                    // if (!string.IsNullOrEmpty(strErrorMsg))
                    // {
                    // this.ViewData["Success"] = "0";
                    // this.ViewData["Message"] = strErrorMsg;
                    // }
                    // else
                    // {
                    List<BanquetModel> objBanquetDetailList = (List<BanquetModel>)Session["BanquetDetail" + objBanquet.HdnSession];
                    if (this.Session["ProfilePic"] != null)
                    {
                        objBanquet.ProfilePic = Session["ProfilePic"].ToString();
                    }
                    else
                    {
                        objBanquet.ProfilePic = objBanquet.HdnImgProfilePic;
                    }

                    if (this.Session["TermsFile"] != null)
                    {
                        objBanquet.Terms = Session["TermsFile"].ToString();
                    }
                    else
                    {
                        objBanquet.Terms = objBanquet.HdnFlUploadTerms;
                    }

                    objBanquet.Id = this.objIBanquetCommand.SaveBanquet(objBanquet, objBanquetDetailList);
                    if (objBanquet.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        this.ViewData["Message"] = Functions.AlertMessage("Banquet", MessageType.Success);
                        this.BindDropDownListForBanquet(objBanquet, false);
                        return this.View(objBanquet);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Banquet", MessageType.Fail);
                    }
                }

                this.BindDropDownListForBanquet(objBanquet, true);
                return this.View(objBanquet);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Banquet", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                this.BindDropDownListForBanquet(objBanquet, true);
                return this.View(objBanquet);
            }
        }

        /// <summary>
        /// Binds the banquet grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindBanquetGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<MS_SearchBanquetResult> objBanquetList = this.objIBanquetCommand.SearchBanquet(rows, page, search, sidx + " " + sord);
                //// if (objBanquetList != null && objBanquetList.Count > 0)
                return this.FillGridBanquet(page, rows, objBanquetList);
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
        /// Deletes the banquet.
        /// </summary>
        /// <param name="strBanquetId">The string banquet identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteBanquet(string strBanquetId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string[] strBanquet = strBanquetId.Split(',');
                strBanquetId = string.Empty;
                foreach (var item in strBanquet)
                {
                    strBanquetId += item.Decode() + ",";
                }

                strBanquetId = strBanquetId.Substring(0, strBanquetId.Length - 1);
                MS_DeleteBanquetDetailResult resultDtl = this.objIBanquetCommand.DeleteBanquetDetail(strBanquetId, MySession.Current.UserId);
                MS_DeleteBanquetResult result = this.objIBanquetCommand.DeleteBanquet(strBanquetId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Banquet", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Banquet", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Banquet", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Banquet", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the banquet.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetBanquet()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objIBanquetCommand.GetAllBanquetForDropDown(), JsonRequestBehavior.AllowGet);
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
        /// <param name="lgBanquetId">The long banquet identifier.</param>
        public void CreateDetails(string strSessionId, long lgBanquetId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                Session.Add("BanquetDetail" + strSessionId, this.objIBanquetCommand.GetBanquetDetailByBanquetId(lgBanquetId));
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
                Session.Remove("BanquetDetail" + strSessionId);
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
                            //// int MaxContentLength = 555 * 396; //3 MB

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
                                string strImagepath = Functions.GetSettings("DocRootFolderPath") + "\\Images\\BanquetImage\\ProfilePic\\";
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
                            int maxContentLength = 1024 * 1024 * 3; ////3 MB

                            if (!Functions.CheckFileExtension(file.FileName, Functions.AllowedFileExtensionsForPDF))
                            {
                                this.ViewData["Success"] = "0";
                                this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Please upload file of type: " + string.Join(", ", Functions.AllowedFileExtensionsForPDF));
                            }
                            else if (file.ContentLength > maxContentLength)
                            {
                                this.ViewData["Success"] = "0";
                                this.ViewData["Message"] = Functions.AlertMessage(string.Empty, MessageType.StaticMessage, "Your file is too large, maximum allowed size is: " + maxContentLength);
                            }
                            else
                            {
                                this.ViewData["Success"] = "1";
                                string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("_", string.Empty);
                                string strImagepath = Functions.GetSettings("DocRootFolderPath") + "\\PDF\\Banquet\\";

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
        /// Binds the drop down list for banquet.
        /// </summary>
        /// <param name="objBanquetModel">The object banquet model.</param>
        /// <param name="blBindDropDownFromDb">IF set to <c>true</c> [BL bind drop down from database].</param>
        private void BindDropDownListForBanquet(BanquetModel objBanquetModel, bool blBindDropDownFromDb)
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

        /// <summary>
        /// Validates the banquet.
        /// </summary>
        /// <param name="objBanquet">The object banquet.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateBanquet(BanquetModel objBanquet)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objBanquet.BanquetName))
                {
                    strErrorMsg += Functions.AlertMessage("Banquet Name", MessageType.InputRequired) + "<br/>";
                }

                if (this.Session["BanquetDetail" + objBanquet.HdnSession] == null)
                {
                    this.CreateDetails(objBanquet.HdnSession, objBanquet.Id);
                }

                List<BanquetModel> objBanquetDetailList = (List<BanquetModel>)Session["BanquetDetail" + objBanquet.HdnSession];
                if (objBanquetDetailList == null || objBanquetDetailList.Where(x => x.IsDeleted == false).Count() <= 0)
                {
                    strErrorMsg += Functions.AlertMessage("Banquet Detail", MessageType.RecordInGridRequired) + "<br/>";
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
        /// Fills the grid banquet.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objBanquetList">The object banquet list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridBanquet(int page, int rows, List<MS_SearchBanquetResult> objBanquetList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objBanquetList != null && objBanquetList.Count > 0 ? objBanquetList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objBanquet in objBanquetList
                            select new
                            {
                                BanquetName = objBanquet.BanquetName,
                                Description = objBanquet.Description,
                                MinPersonCapcity = objBanquet.MinPersonCapcity != null ? objBanquet.MinPersonCapcity.Value.ToString() : string.Empty,
                                MaxPersonCapcity = objBanquet.MaxPersonCapcity != null ? objBanquet.MaxPersonCapcity.Value.ToString() : string.Empty,
                                ProfilePic = objBanquet.ProfilePic,
                                Terms = objBanquet.Terms,
                                Id = objBanquet.Id.ToString().Encode()
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
    }
}
