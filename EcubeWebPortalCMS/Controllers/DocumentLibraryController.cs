// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-21-2016
//
// Last Modified By : pratikp
// ***********************************************************************
// <copyright file="DocumentLibraryController.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>DocumentLibraryController</summary>
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
    /// Class DocumentLibraryController.
    /// </summary>
    public class DocumentLibraryController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i document library command.
        /// </summary>
        private readonly IDocumentLibraryCommand objIDocumentLibraryCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLibraryController" /> class.
        /// </summary>
        /// <param name="iDocumentLibraryCommand">The i document library command.</param>
        public DocumentLibraryController(IDocumentLibraryCommand iDocumentLibraryCommand)
        {
            this.objIDocumentLibraryCommand = iDocumentLibraryCommand;
        }

        /// <summary>
        /// Documents the library view.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult DocumentLibraryView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.DocumentLibrary);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                CommonDataContext objCommon = new CommonDataContext();
                AAAConfigSetting obj = new AAAConfigSetting();
                obj = objCommon.AAAConfigSettings.Where(x => x.KeyName == "DocViewerRootFolderPath").FirstOrDefault();

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                this.ViewData["blEditRights"] = objPermission.Edit_Right;
                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;
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
        /// Documents the library.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult DocumentLibrary()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.DocumentLibrary);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                DocumentLibraryModel objDocumentModel = new DocumentLibraryModel();
                long lgDocumentId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objDocumentModel.HdnIFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgDocumentId = Request.QueryString.ToString().LongSafe();
                        objDocumentModel = this.objIDocumentLibraryCommand.GetDocumentById(lgDocumentId);
                        objDocumentModel.HdnDocumentUpload = objDocumentModel.DocumentName;
                    }
                }
                else
                {
                    objDocumentModel.IsActive = true;
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }
                    }
                }

                CommonDataContext objCommon = new CommonDataContext();
                AAAConfigSetting obj = new AAAConfigSetting();
                obj = objCommon.AAAConfigSettings.Where(x => x.KeyName == "DocViewerRootFolderPath").FirstOrDefault();
                objDocumentModel.HdnDocumentUpload = obj.KeyValue.ToString();
                return this.View(objDocumentModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Documents the library.
        /// </summary>
        /// <param name="objDocumentModel">The object document model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DocumentLibrary(DocumentLibraryModel objDocumentModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.DocumentLibrary);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objDocumentModel.DocumentId == 0)
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (!objPermission.Edit_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objDocumentModel.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool exists = this.objIDocumentLibraryCommand.IsTitleExists(objDocumentModel.DocumentId, objDocumentModel.Title);
                if (exists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Document", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateSupport(objDocumentModel);

                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        if (this.Session["DocumentFile"] != null)
                        {
                            objDocumentModel.DocumentName = this.Session["DocumentFile"].ToString();
                        }
                        else
                        {
                            objDocumentModel.DocumentName = objDocumentModel.HdnDocumentUpload;
                        }

                        long resultId = this.objIDocumentLibraryCommand.SaveDocumentLibrary(objDocumentModel);
                        if (resultId >= 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Document", MessageType.Success);
                            return this.View(objDocumentModel);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Document", MessageType.Fail);
                        }
                    }
                }

                return this.View(objDocumentModel);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Document", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objDocumentModel);
            }
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="strDocumentIdList">The string document identifier list.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult DeleteDocument(string strDocumentIdList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strDocument = strDocumentIdList.Split(',');
                strDocumentIdList = string.Empty;
                foreach (var item in strDocument)
                {
                    strDocumentIdList += item + ",";
                }

                strDocumentIdList = strDocumentIdList.Substring(0, strDocumentIdList.Length - 1);
                CRMDeleteDocumentResult result = this.objIDocumentLibraryCommand.DeleteDocument(strDocumentIdList, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Document", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Document", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Document", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Document", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Binds the document library.
        /// </summary>
        /// <param name="sidx">The SIDX .</param>
        /// <param name="sord">The SORD .</param>
        /// <param name="page">The page .</param>
        /// <param name="rows">The rows .</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action Result.</returns>
        public ActionResult BindDocumentLibrary(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<CRMSearchDocumentResult> objDocumentList = this.objIDocumentLibraryCommand.SearchDocument(rows, page, search, sidx + " " + sord);
                return this.FillDocumentGrid(page, rows, objDocumentList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Uploads the document.
        /// </summary>
        /// <param name="data">The data .</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadDocument(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (Request.Files["files"] != null)
                {
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            this.ViewData["Success"] = "1";
                            string fileName = Path.GetFileName(file.FileName).Replace(" ", string.Empty).Replace("-", string.Empty);
                            string strDocpath = Functions.GetSettings("DocRootFolderPath") + "\\Documents\\";
                            //// string strDocpath = Server.MapPath("~/Documents/");
                            bool exists = Directory.Exists(strDocpath);
                            if (!exists)
                            {
                                Directory.CreateDirectory(strDocpath);
                            }

                            var path = Path.Combine(strDocpath, fileName);
                            Stream strm = file.InputStream;
                            //// Functions.GenerateThumbnails(0.5, strm, path);

                            Request.Files["files"].SaveAs(strDocpath + fileName);
                            this.Session["DocumentFile"] = fileName;
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
        /// Fills the document grid.
        /// </summary>
        /// <param name="page">The page .</param>
        /// <param name="rows">The rows .</param>
        /// <param name="objDocumentList">The object document list.</param>
        /// <returns>Action Result.</returns>
        private ActionResult FillDocumentGrid(int page, int rows, List<CRMSearchDocumentResult> objDocumentList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objDocumentList != null && objDocumentList.Count > 0 ? objDocumentList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedDocumentCol = objDocumentList;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objDocument in pagedDocumentCol
                            select new
                            {
                                id = objDocument.DocumentId.ToString(),
                                DocumentName = objDocument.DocumentName,
                                Title = objDocument.Title,
                                Remark = objDocument.Remark,
                                IsActive = objDocument.IsActive
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
        /// Validates the support.
        /// </summary>
        /// <param name="objDocumentModel">The object document model.</param>
        /// <returns>System String.</returns>
        private string ValidateSupport(DocumentLibraryModel objDocumentModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objDocumentModel.Title))
                {
                    strErrorMsg += Functions.AlertMessage("Document Title", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }
    }
}
