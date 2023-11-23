using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using EcubeWebPortalCMS.Common;
using EcubeWebPortalCMS.Models;
using Serilog;

namespace EcubeWebPortalCMS.Controllers
{
    public class BannerController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ConnectionString);

        // GET: /Post/

        /// <summary>
        /// The i post.
        /// </summary>
        private readonly iBanner iBanner = new BannerCommand();

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;
        // GET: /Banner/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BannerView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.Common.MobileBanner));

                #region rights code
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }
                #endregion

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
        /// Banner this instance.
        /// </summary>
        /// <returns>ActionResult Banner.</returns>
        public ActionResult Banner()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.Common.MobileBanner));
                BannerModel bannerModel = new BannerModel();

                Random ran = new Random();
                bannerModel.HdnSession = ran.Next().ToString();
                string cacheclear = DateTime.Now.ToString("ddMMyyhhmmssffff");

                int BannerId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        bannerModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }
                        BannerId = this.Request.QueryString.ToString().Decode().IntSafe();
                        var obj = this.iBanner.GetBannerById(BannerId);
                        if (obj != null)
                        {
                            bannerModel.HdnSession = ran.Next().ToString();
                            bannerModel.BannerId = obj.BannerId;
                            bannerModel.BannerTitle = obj.BannerTitle;
                            bannerModel.BannerDescription = obj.BannerDescription;
                            bannerModel.strFromDate = obj.FromDate;
                            bannerModel.strToDate = obj.ToDate;
                            bannerModel.Image1 = obj.Image1;
                            bannerModel.Image2 = obj.Image2;
                            bannerModel.Image3 = obj.Image3;
                            bannerModel.Image4 = obj.Image4;
                            bannerModel.Image5 = obj.Image5;
                            bannerModel.IsActive = obj.IsActive;
                        }
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
               
                
                this.Session["ImageName1"] = bannerModel.Image1;
                string fileExtImage1 = Path.GetExtension(bannerModel.Image1);
                bannerModel.Image1 = bannerModel.Image1 == null ? string.Empty : "../CMSUpload/Document/MobileBanner/" + bannerModel.BannerId + "/1/Original" + fileExtImage1+ "?" + cacheclear;

                this.Session["ImageName2"] = bannerModel.Image2;
                string fileExtImage2 = Path.GetExtension(bannerModel.Image2);
                bannerModel.Image2 = bannerModel.Image2 == null ? string.Empty : "../CMSUpload/Document/MobileBanner/" + bannerModel.BannerId + "/2/Original" + fileExtImage2 + "?" + cacheclear;

                this.Session["ImageName3"] = bannerModel.Image3;
                string fileExtImage3 = Path.GetExtension(bannerModel.Image3);
                bannerModel.Image3 = bannerModel.Image3 == null ? string.Empty : "../CMSUpload/Document/MobileBanner/" + bannerModel.BannerId + "/3/Original" + fileExtImage3 +"?" + cacheclear;

                this.Session["ImageName4"] = bannerModel.Image4;
                string fileExtImage4 = Path.GetExtension(bannerModel.Image4);
                bannerModel.Image4 = bannerModel.Image4 == null ? string.Empty : "../CMSUpload/Document/MobileBanner/" + bannerModel.BannerId + "/4/Original" + fileExtImage4 + "?" + cacheclear;

                this.Session["ImageName5"] = bannerModel.Image5;
                string fileExtImage5 = Path.GetExtension(bannerModel.Image5);
                bannerModel.Image5 = bannerModel.Image5 == null ? string.Empty : "../CMSUpload/Document/MobileBanner/" + bannerModel.BannerId + "/5/Original" + fileExtImage5 + "?" + cacheclear;

                return this.View(bannerModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult Banner(BannerModel bannerModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.Common.MobileBanner));
                #region rights code

                if (bannerModel.BannerId == 0)
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
                #endregion rights code
                if (bannerModel.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }
                // Parse the date string into a DateTime object
                DateTime FromDate = DateTime.ParseExact(bannerModel.strFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime ToDate = DateTime.ParseExact(bannerModel.strToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                bannerModel.FromDate = FromDate;
                bannerModel.ToDate = ToDate;

                bool blExists = this.iBanner.IsBannerExists(bannerModel.BannerId, bannerModel.BannerTitle);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Banner", MessageType.AlreadyExist);
                    return this.View(bannerModel);
                }
                else
                {
                    if (this.Session["ImageName1"] != null)
                    {
                        bannerModel.Image1 = this.Session["ImageName1"].ToString();
                    }
                    if (this.Session["ImageName2"] != null)
                    {
                        bannerModel.Image2 = this.Session["ImageName2"].ToString();
                    }

                    if (this.Session["ImageName3"] != null)
                    {
                        bannerModel.Image3 = this.Session["ImageName3"].ToString();
                    }

                    if (this.Session["ImageName4"] != null)
                    {
                        bannerModel.Image4 = this.Session["ImageName4"].ToString();
                    }

                    if (this.Session["ImageName5"] != null)
                    {
                        bannerModel.Image5 = this.Session["ImageName5"].ToString();
                    }

                    if (bannerModel.BannerId > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Banner", MessageType.UpdateSuccess);
                    }

                    bannerModel.BannerId = this.iBanner.SaveBanner(bannerModel);

                    if (bannerModel.BannerId > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Banner", MessageType.Success);
                        }

                        for (int counter = 1; counter <= 5; counter++)
                        {
                            if (this.Session["ImagePath" + counter.ToString()] != null)
                            {
                                if (!string.IsNullOrEmpty((string)this.Session["ImageName" + counter.ToString()]))
                                {
                                    Functions.ReSizeBanner(this.Session["ImagePath" + counter.ToString()].ToString(), this.Session["ImageName" + counter.ToString()].ToString(), Convert.ToString(bannerModel.BannerId) + "_" + counter.ToString(), "MobileBanner");
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

                        return this.View(bannerModel);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Banner", MessageType.Fail);
                    }

                }

                return this.RedirectToAction("BannerView", "Banner");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Banner", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(bannerModel);
            }
        }

        [HttpGet]
        public ActionResult BindBannerGrid(string sidx, string sord, int page, int rows, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchBannerResult> objBannerList = this.iBanner.SearchBanner(rows, page, search, sidx + " " + sord);
                if (objBannerList != null)
                {
                    return this.FillGridBanner(page, rows, objBannerList);
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

        private ActionResult FillGridBanner(int page, int rows, List<SearchBannerResult> objBannerList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objBannerList != null && objBannerList.Count > 0 ? (int)objBannerList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objPost in objBannerList
                            select new
                            {
                                ToDate = Convert.ToDateTime(objPost.ToDate.ToString()).ToShortDateString().Replace("-", "/"),
                                FromDate = Convert.ToDateTime(objPost.FromDate.ToString()).ToShortDateString().Replace("-", "/"),
                                BannerTitle = objPost.BannerTitle,
                                BannerId = objPost.BannerId.ToString().Encode()
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

        public JsonResult RemoveImage(FormCollection data)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            string sequence = this.Request["sequence"];
            this.Session["ImageName" + sequence] = null;

            return this.Json("1111");
        }

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

        public JsonResult DeleteBanner(string strBannerId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strBanner = strBannerId.Split(',');
                strBannerId = string.Empty;

                foreach (var item in strBanner)
                {
                    strBannerId += item.Decode() + ",";
                }

                strBannerId = strBannerId.Substring(0, strBannerId.Length - 1);
                DeleteBannerResult result = this.iBanner.DeleteBanner(strBannerId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Banner", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Banner", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Banner", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Banner", MessageType.DeleteFail));
            }
        }
    }
}
