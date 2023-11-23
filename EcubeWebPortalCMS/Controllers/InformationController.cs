// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-13-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="InformationController.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>InformationController.cs</summary>
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
    using Serilog;

    /// <summary>
    /// Class InformationController.
    /// </summary>
    public class InformationController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        // GET: /Information/

        /// <summary>
        /// The i information.
        /// </summary>
        private readonly IInformation iInformation = new InformationCommand();

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Information the view.
        /// </summary>
        /// <returns>ActionResult information view.</returns>
        public ActionResult InformationView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubInformation));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
                this.ViewData["blEditRights"] = getPageRights.Edit;
                this.ViewData["blDeleteRights"] = getPageRights.Delete;
                return this.View();
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Information this instance.
        /// </summary>
        /// <returns>ActionResult information.</returns>
        public ActionResult Information()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubInformation));

                InformationModel objInformationModel = new InformationModel();
                Random ran = new Random();
                objInformationModel.HdnSession = ran.Next().ToString();
                int informationId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objInformationModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        informationId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objInformationModel = this.iInformation.GetInformationById(informationId);
                        objInformationModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.Session["ImageName"] = objInformationModel.ImagePath;
                objInformationModel.ImagePath = objInformationModel.ImagePath == null ? string.Empty : "../CMSUpload/Document/Information/" + objInformationModel.InformationId + ".jpg";
                return this.View(objInformationModel);
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Information the specified object information.
        /// </summary>
        /// <param name="objInformation">The object information Parameter.</param>
        /// <returns>ActionResult information.</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Information(InformationModel objInformation)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubInformation));

                if (objInformation.InformationId == 0)
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

                if (objInformation.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                objInformation.Description = System.Net.WebUtility.HtmlDecode(objInformation.Description);

                bool blExists = this.iInformation.IsInformationExists(objInformation.InformationId, objInformation.Name);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Information", MessageType.AlreadyExist);
                }
                else
                {
                    if (this.Session["ImageName"] != null)
                    {
                        objInformation.ImagePath = this.Session["ImageName"].ToString();
                    }

                    if (objInformation.InformationId > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Information", MessageType.UpdateSuccess);
                    }

                    objInformation.InformationId = this.iInformation.SaveInformation(objInformation);
                    if (objInformation.InformationId > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Information", MessageType.Success);
                        }

                        if (this.Session["ImagePath"] != null)
                        {
                            Functions.ReSizeBanner(this.Session["ImagePath"].ToString(), this.Session["ImageName"].ToString(), Convert.ToString(objInformation.InformationId), "Information");
                        }

                        return this.View(objInformation);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Information", MessageType.Fail);
                    }
                }

                return this.RedirectToAction("InformationView", "Information");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Information", MessageType.Fail);
            information += " - " + ex.Message;
            logger.Error(ex, information);
                return this.View(objInformation);
            }
        }

        /// <summary>
        /// Binds the information grid.
        /// </summary>
        /// <param name="sidx">The sidx Parameter.</param>
        /// <param name="sord">The sord Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <returns>ActionResult bind information grid.</returns>
        [HttpGet]
        public ActionResult BindInformationGrid(string sidx, string sord, int page, int rows, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchInformationResult> objInformationList = this.iInformation.SearchInformation(rows, page, search, sidx + " " + sord);
                if (objInformationList != null)
                {
                    return this.FillGridInformation(page, rows, objInformationList);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the information.
        /// </summary>
        /// <param name="strInformationId">The string information identifier Parameter.</param>
        /// <returns>JsonResult delete information.</returns>
        public JsonResult DeleteInformation(string strInformationId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strInformation = strInformationId.Split(',');
                strInformationId = string.Empty;

                foreach (var item in strInformation)
                {
                    strInformationId += item.Decode() + ",";
                }

                strInformationId = strInformationId.Substring(0, strInformationId.Length - 1);
                DeleteInformationResult result = this.iInformation.DeleteInformation(strInformationId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Information", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Information", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Information", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Information", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Uploads the event banner image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadImage(FormCollection data)
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
                            Image oldImage = Bitmap.FromStream(file.InputStream);
                            Image newImage = this.ResizeImage(oldImage, 1440, 763);
                            try
                            {
                                newImage.Save(path, ImageFormat.Jpeg);
                            }
                            catch(Exception ex)
                            {
            information += " - " + ex.Message;
            logger.Error(ex, information);
                            }

                            newImage.Dispose();
                            this.Session["ImagePath"] = strImagepath;
                            this.Session["ImageName"] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        /// <summary>
        /// Fills the grid information.
        /// </summary>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="objInformationList">The object information list Parameter.</param>
        /// <returns>ActionResult fill grid information.</returns>
        private ActionResult FillGridInformation(int page, int rows, List<SearchInformationResult> objInformationList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objInformationList != null && objInformationList.Count > 0 ? (int)objInformationList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objInformation in objInformationList
                            select new
                            {
                                InformationName = objInformation.InformationName,
                                SequenceNo = objInformation.SequenceNo,
                                Id = objInformation.Id.ToString().Encode()
                            }).OrderBy(x => x.SequenceNo).ToArray()
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
    }
}