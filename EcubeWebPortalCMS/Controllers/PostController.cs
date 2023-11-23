// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-20-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-20-2017
// ***********************************************************************
// <copyright file="PostController.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>PostController.cs</summary>
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
    /// Class PostController.
    /// </summary>
    public class PostController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        // GET: /Post/

        /// <summary>
        /// The i post.
        /// </summary>
        private readonly IPost iPost = new PostCommand();

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
        /// Posts the view.
        /// </summary>
        /// <returns>ActionResult post view.</returns>
        public ActionResult PostView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobilePost));
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
        /// Binds the post grid.
        /// </summary>
        /// <param name="sidx">The sidx Parameter.</param>
        /// <param name="sord">The sord Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <returns>ActionResult bind post grid.</returns>
        [HttpGet]
        public ActionResult BindPostGrid(string sidx, string sord, int page, int rows, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchPostResult> objPostList = this.iPost.SearchPost(rows, page, search, sidx + " " + sord);
                if (objPostList != null)
                {
                    return this.FillGridPost(page, rows, objPostList);
                }
                else
                {
                    return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
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
        /// Post this instance.
        /// </summary>
        /// <returns>ActionResult Post.</returns>
        public ActionResult Post()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobilePost));

                PostModel objPostModel = new PostModel();
                Random ran = new Random();
                objPostModel.HdnSession = ran.Next().ToString();
                string cacheclear = DateTime.Now.ToString("ddMMyyhhmmssffff");
                int postId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objPostModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        postId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objPostModel = this.iPost.GetPostById(postId);
                        objPostModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    this.Session["ImagePath"] = null;
                    this.Session["ImageName1"] = null;
                    this.Session["ImageName2"] = null;
                    this.Session["ImageName3"] = null;
                    this.Session["ImageName4"] = null;
                    this.Session["ImageName5"] = null;
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.Session["ImageName1"] = objPostModel.Image1;
                objPostModel.Image1 = objPostModel.Image1 == null ? string.Empty : "../CMSUpload/Document/Post/" + objPostModel.Id + "/1/Original.jpg" + "?" + cacheclear;

                this.Session["ImageName2"] = objPostModel.Image2;
                objPostModel.Image2 = objPostModel.Image2 == null ? string.Empty : "../CMSUpload/Document/Post/" + objPostModel.Id + "/2/Original.jpg" + "?" + cacheclear;

                this.Session["ImageName3"] = objPostModel.Image3;
                objPostModel.Image3 = objPostModel.Image3 == null ? string.Empty : "../CMSUpload/Document/Post/" + objPostModel.Id + "/3/Original.jpg" + "?" + cacheclear;

                this.Session["ImageName4"] = objPostModel.Image4;
                objPostModel.Image4 = objPostModel.Image4 == null ? string.Empty : "../CMSUpload/Document/Post/" + objPostModel.Id + "/4/Original.jpg" + "?" + cacheclear;

                this.Session["ImageName5"] = objPostModel.Image5;
                objPostModel.Image5 = objPostModel.Image5 == null ? string.Empty : "../CMSUpload/Document/Post/" + objPostModel.Id + "/5/Original.jpg" + "?" + cacheclear;

                return this.View(objPostModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Post the specified object Post.
        /// </summary>
        /// <param name="objPost">The object Post Parameter.</param>
        /// <returns>ActionResult Post.</returns>
        [HttpPost]
        public ActionResult Post(PostModel objPost)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobilePost));

                if (objPost.Id == 0)
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

                if (objPost.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.iPost.IsPostExists(objPost.Id, objPost.Name);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Post", MessageType.AlreadyExist);
                    return this.View(objPost);
                }
                else
                {
                    string strErrorMsg = this.ValidatePost(objPost);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        if (this.Session["ImageName1"] != null)
                        {
                            objPost.Image1 = this.Session["ImageName1"].ToString();
                        }
                        if (this.Session["ImageName2"] != null)
                        {
                            objPost.Image2 = this.Session["ImageName2"].ToString();
                        }

                        if (this.Session["ImageName3"] != null)
                        {
                            objPost.Image3 = this.Session["ImageName3"].ToString();
                        }

                        if (this.Session["ImageName4"] != null)
                        {
                            objPost.Image4 = this.Session["ImageName4"].ToString();
                        }

                        if (this.Session["ImageName5"] != null)
                        {
                            objPost.Image5 = this.Session["ImageName5"].ToString();
                        }

                        if (objPost.Id > 0)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Post", MessageType.UpdateSuccess);
                        }

                        objPost.Id = this.iPost.SavePost(objPost);
                        if (objPost.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                            {
                                this.ViewData["Message"] = Functions.AlertMessage("Post", MessageType.Success);
                            }

                            for (int counter = 1; counter <= 5; counter++)
                            {
                                if (this.Session["ImagePath" + counter.ToString()] != null)
                                {
                                    if (!string.IsNullOrEmpty((string)this.Session["ImageName" + counter.ToString()]))
                                    {
                                        Functions.ReSizeBanner(this.Session["ImagePath" + counter.ToString()].ToString(), this.Session["ImageName" + counter.ToString()].ToString(), Convert.ToString(objPost.Id) + "_" + counter.ToString(), "Post");
                                    }
                                }
                            }

                            for (int counter = 1; counter <= 5; counter++)
                            {
                                if (this.Session["ImagePath" + counter.ToString()] != null)
                                {
                                    if (!string.IsNullOrEmpty((string)this.Session["ImageName" + counter.ToString()]))
                                    {
                                        if (System.IO.File.Exists(this.Session["ImagePath" + counter.ToString()].ToString() + "\\" + this.Session["ImageName" + counter.ToString()].ToString()))
                                        {
                                            System.IO.File.Delete(this.Session["ImagePath" + counter.ToString()].ToString() + "\\" + this.Session["ImageName" + counter.ToString()].ToString());
                                        }
                                    }
                                }
                            }

                            return this.View(objPost);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Post", MessageType.Fail);
                        }
                    }
                }

                return this.RedirectToAction("PostView", "Post");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Post", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objPost);
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
                    string sequence = this.Request["sequence"];
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
                            this.Session["ImagePath" + sequence] = strImagepath;
                            this.Session["ImageName" + sequence] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
        }

        /// <summary>
        /// Deletes the Post.
        /// </summary>
        /// <param name="strPostId">The string Post identifier Parameter.</param>
        /// <returns>JsonResult delete Post.</returns>
        public JsonResult DeletePost(string strPostId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strPost = strPostId.Split(',');
                strPostId = string.Empty;

                foreach (var item in strPost)
                {
                    strPostId += item.Decode() + ",";
                }

                strPostId = strPostId.Substring(0, strPostId.Length - 1);
                DeletePostResult result = this.iPost.DeletePost(strPostId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Post", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Post", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Post", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Post", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Fills the grid Post.
        /// </summary>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="objPostList">The object Post list Parameter.</param>
        /// <returns>ActionResult fill grid Post.</returns>
        private ActionResult FillGridPost(int page, int rows, List<SearchPostResult> objPostList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objPostList != null && objPostList.Count > 0 ? (int)objPostList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objPost in objPostList
                            select new
                            {
                                PostName = objPost.Name,
                                PostDescription = objPost.Description,
                                Id = objPost.Id.ToString().Encode()
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
        /// Validates the Post.
        /// </summary>
        /// <param name="objPost">The object Post Parameter.</param>
        /// <returns>System.String validate Post.</returns>
        private string ValidatePost(PostModel objPost)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objPost.Name))
                {
                    strErrorMsg += Functions.AlertMessage("Post Name", MessageType.InputRequired) + "<br/>";
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