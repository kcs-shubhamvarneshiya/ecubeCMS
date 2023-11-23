// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-15-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-18-2017
// ***********************************************************************
// <copyright file="FacilityCategoryController.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>FacilityCategoryController.cs</summary>
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
    /// Class FacilityCategoryController.
    /// </summary>
    public class FacilityCategoryController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        // GET: /FacilityCategory/

        /// <summary>
        /// The i facility category..
        /// </summary>
        private readonly IFacilityCategory iFacilityCategory = new FacilityCategoryCommand();

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult index.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Facilities the category view.
        /// </summary>
        /// <returns>ActionResult facility category view.</returns>
        public ActionResult FacilityCategoryView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubFacilityCategory));
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
        /// Facilities the category.
        /// </summary>
        /// <returns>ActionResult facility category.</returns>
        public ActionResult FacilityCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubFacilityCategory));

                FacilityCategoryModel objFacilityCategoryModel = new FacilityCategoryModel();
                Random ran = new Random();
                objFacilityCategoryModel.HdnSession = ran.Next().ToString();
                int categoryId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objFacilityCategoryModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        categoryId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objFacilityCategoryModel = this.iFacilityCategory.GetCategoryById(categoryId);
                        objFacilityCategoryModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.Session["ImageName"] = objFacilityCategoryModel.ImagePath;
                objFacilityCategoryModel.ImagePath = objFacilityCategoryModel.ImagePath == null ? string.Empty : "../CMSUpload/Document/FacilityCategory/" + objFacilityCategoryModel.CategoryId + "/Original.jpg";
                return this.View(objFacilityCategoryModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Facilities the category.
        /// </summary>
        /// <param name="objFacilityCategory">The object facility category Parameter.</param>
        /// <returns>ActionResult facility category.</returns>
        [HttpPost]
        public ActionResult FacilityCategory(FacilityCategoryModel objFacilityCategory)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubFacilityCategory));

                if (objFacilityCategory.CategoryId == 0)
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

                if (objFacilityCategory.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.iFacilityCategory.IsCategoryExists(objFacilityCategory.CategoryId, objFacilityCategory.CategoryName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Facility Category", MessageType.AlreadyExist);
                    return this.View(objFacilityCategory);
                }
                else
                {
                    if (this.Session["ImageName"] != null)
                    {
                        objFacilityCategory.ImagePath = this.Session["ImageName"].ToString();
                    }
                    else
                    {
                        objFacilityCategory.ImagePath = string.Empty;
                    }

                    if (objFacilityCategory.CategoryId > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Facility Category", MessageType.UpdateSuccess);
                    }

                    objFacilityCategory.CategoryId = this.iFacilityCategory.SaveCategory(objFacilityCategory);
                    if (objFacilityCategory.CategoryId > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Facility Category", MessageType.Success);
                        }

                        if (this.Session["ImagePath"] != null)
                        {
                            Functions.ReSizeBanner(this.Session["ImagePath"].ToString(), this.Session["ImageName"].ToString(), Convert.ToString(objFacilityCategory.CategoryId), "FacilityCategory");
                        }

                        return this.View(objFacilityCategory);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Facility Category", MessageType.Fail);
                    }
                }

                return this.RedirectToAction("FacilityCategoryView", "FacilityCategory");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("FacilityCategory", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objFacilityCategory);
            }
        }

        /// <summary>
        /// Binds the facility category grid.
        /// </summary>
        /// <param name="sidx">The sidx Parameter.</param>
        /// <param name="sord">The sord Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <returns>ActionResult bind facility category grid.</returns>
        [HttpGet]
        public ActionResult BindFacilityCategoryGrid(string sidx, string sord, int page, int rows, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchFacilityCategoryResult> objFacilityCategoryList = this.iFacilityCategory.SearchFacilityCategory(rows, page, search, sidx + " " + sord);
                if (objFacilityCategoryList != null)
                {
                    return this.FillGridFacilityCategory(page, rows, objFacilityCategoryList);
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
        /// Uploads the event banner image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadCategoryImage(FormCollection data)
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
                            catch (Exception ex)
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
        /// Uploads the group banner image.
        /// </summary>
        /// <param name="data">The data Parameter.</param>
        /// <returns>JsonResult upload group banner image.</returns>
        public JsonResult UploadGroupBannerImage(FormCollection data)
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
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);
                            }

                            newImage.Dispose();
                            this.Session["BannerImagePath"] = strImagepath;
                            this.Session["BannerImageName"] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        /// <summary>
        /// Uploads the group icon image.
        /// </summary>
        /// <param name="data">The data Parameter.</param>
        /// <returns>JsonResult upload group icon image.</returns>
        public JsonResult UploadGroupIconImage(FormCollection data)
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
                                newImage.Save(path, file.ContentType.ToLower().Contains("png") ? ImageFormat.Png : ImageFormat.Jpeg);
                            }
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);

                            }

                            newImage.Dispose();
                            this.Session["IconImagePath"] = strImagepath;
                            this.Session["IconImageName"] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        /// <summary>
        /// Facilities the group view.
        /// </summary>
        /// <returns>ActionResult facility group view.</returns>
        public ActionResult FacilityGroupView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubFacility));
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
        /// Binds the facility group grid.
        /// </summary>
        /// <param name="sidx">The sidx Parameter.</param>
        /// <param name="sord">The sord Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <returns>ActionResult bind facility group grid.</returns>
        [HttpGet]
        public ActionResult BindFacilityGroupGrid(string sidx, string sord, int page, int rows, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchFacilityGroupResult> objFacilityGroupList = this.iFacilityCategory.SearchFacilityGroup(rows, page, search, sidx + " " + sord);
                if (objFacilityGroupList != null)
                {
                    return this.FillGridFacilityGroup(page, rows, objFacilityGroupList);
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
        /// Facilities the category.
        /// </summary>
        /// <returns>ActionResult facility category.</returns>
        public ActionResult FacilityGroup()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubFacility));

                FacilityGroupModel objFacilityGroupModel = new FacilityGroupModel();
                Random ran = new Random();
                objFacilityGroupModel.HdnSession = ran.Next().ToString();
                int facilityId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objFacilityGroupModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        facilityId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objFacilityGroupModel = this.iFacilityCategory.GetFacilityById(facilityId);
                        objFacilityGroupModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                objFacilityGroupModel.CategoryList = this.iFacilityCategory.GetAllCategoryList();
                SelectListItem selectListItem = objFacilityGroupModel.CategoryList.Where(x => x.Value == objFacilityGroupModel.CategoryId.ToString()).FirstOrDefault();
                if (selectListItem != null)
                {
                    selectListItem.Selected = true;
                }

                objFacilityGroupModel.HdnSession = ran.Next().ToString();
                this.Session["BannerImageName"] = objFacilityGroupModel.BannerImage;
                objFacilityGroupModel.BannerImage = objFacilityGroupModel.BannerImage == null ? string.Empty : "../CMSUpload/Document/Facility/" + objFacilityGroupModel.Id + "/Original.jpg";
                this.Session["IconImageName"] = objFacilityGroupModel.IconImage;
                objFacilityGroupModel.IconImage = objFacilityGroupModel.IconImage == null ? string.Empty : "../CMSUpload/Document/Facility/" + objFacilityGroupModel.Id + "/Icon/Original.jpg";
                return this.View(objFacilityGroupModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Facilities the group.
        /// </summary>
        /// <param name="objFacilityGroup">The object facility group Parameter.</param>
        /// <returns>ActionResult facility group.</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FacilityGroup(FacilityGroupModel objFacilityGroup)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.ClubFacility));

                if (objFacilityGroup.Id == 0)
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

                if (objFacilityGroup.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                objFacilityGroup.Description = System.Net.WebUtility.HtmlDecode(objFacilityGroup.Description);

                bool blExists = this.iFacilityCategory.IsCategoryExists(objFacilityGroup.Id, objFacilityGroup.Name);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Facility Group", MessageType.AlreadyExist);
                    objFacilityGroup.CategoryList = this.iFacilityCategory.GetAllCategoryList();
                    return this.View(objFacilityGroup);
                }
                else
                {
                    if (this.Session["BannerImageName"] != null)
                    {
                        objFacilityGroup.BannerImage = this.Session["BannerImageName"].ToString();
                    }
                    else
                    {
                        objFacilityGroup.BannerImage = string.Empty;
                    }

                    if (this.Session["IconImageName"] != null)
                    {
                        objFacilityGroup.IconImage = this.Session["IconImageName"].ToString();
                    }
                    else
                    {
                        objFacilityGroup.IconImage = string.Empty;
                    }

                    if (objFacilityGroup.Id > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Facility", MessageType.UpdateSuccess);
                    }

                    objFacilityGroup.Id = this.iFacilityCategory.SaveFacility(objFacilityGroup);
                    if (objFacilityGroup.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Facility", MessageType.Success);
                        }

                        if (this.Session["BannerImagePath"] != null)
                        {
                            Functions.ReSizeBanner(this.Session["BannerImagePath"].ToString(), this.Session["BannerImageName"].ToString(), Convert.ToString(objFacilityGroup.Id), "Facility", "Banner");
                        }

                        if (this.Session["IconImagePath"] != null)
                        {
                            Functions.ReSizeBanner(this.Session["IconImagePath"].ToString(), this.Session["IconImageName"].ToString(), Convert.ToString(objFacilityGroup.Id), "Facility", "Icon");
                        }

                        return this.RedirectToAction("FacilityGroupView", "FacilityCategory");
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Facility Group", MessageType.Fail);
                    }
                }

                return this.RedirectToAction("FacilityGroupView", "FacilityCategory");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("FacilityCategory", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objFacilityGroup);
            }
        }

        /// <summary>
        /// Deletes the facility category.
        /// </summary>
        /// <param name="strCategoryId">The string category identifier Parameter.</param>
        /// <returns>JsonResult delete facility category.</returns>
        public JsonResult DeleteFacilityCategory(string strCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strInformation = strCategoryId.Split(',');
                strCategoryId = string.Empty;

                foreach (var item in strInformation)
                {
                    strCategoryId += item.Decode() + ",";
                }

                strCategoryId = strCategoryId.Substring(0, strCategoryId.Length - 1);
                DeleteFacilityCategoryResult result = this.iFacilityCategory.DeleteCategory(strCategoryId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Facility Category", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Facility Category", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Facility Category", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Facility Category", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Deletes the facility.
        /// </summary>
        /// <param name="strFacilityId">The string facility identifier Parameter.</param>
        /// <returns>JsonResult delete facility.</returns>
        public JsonResult DeleteFacility(string strFacilityId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strInformation = strFacilityId.Split(',');
                strFacilityId = string.Empty;

                foreach (var item in strInformation)
                {
                    strFacilityId += item.Decode() + ",";
                }

                strFacilityId = strFacilityId.Substring(0, strFacilityId.Length - 1);
                DeleteFacilityResult result = this.iFacilityCategory.DeleteFacility(strFacilityId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Facility", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Facility", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Facility", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Facility", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Fills the grid facility category.
        /// </summary>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="objFacilityCategoryList">The object facility category list Parameter.</param>
        /// <returns>ActionResult fill grid facility category.</returns>
        private ActionResult FillGridFacilityCategory(int page, int rows, List<SearchFacilityCategoryResult> objFacilityCategoryList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objFacilityCategoryList != null && objFacilityCategoryList.Count > 0 ? (int)objFacilityCategoryList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objFacilityCategory in objFacilityCategoryList
                            select new
                            {
                                SequenceNo = objFacilityCategory.SequenceNo,
                                Name = objFacilityCategory.CategoryName,
                                Id = objFacilityCategory.Id.ToString().Encode()
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

        /// <summary>
        /// Fills the grid facility group.
        /// </summary>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="objFacilityGroupList">The object facility group list Parameter.</param>
        /// <returns>ActionResult fill grid facility group.</returns>
        private ActionResult FillGridFacilityGroup(int page, int rows, List<SearchFacilityGroupResult> objFacilityGroupList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objFacilityGroupList != null && objFacilityGroupList.Count > 0 ? (int)objFacilityGroupList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objFacility in objFacilityGroupList
                            select new
                            {
                                SequenceNo = objFacility.SequenceNo,
                                Name = objFacility.Name,
                                FacilityName = objFacility.CategoryName,
                                Id = objFacility.Id.ToString().Encode()
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
    }
}