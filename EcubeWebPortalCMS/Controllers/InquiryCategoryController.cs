
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
    /// Class InquiryCategoryController.
    /// </summary>
    public class InquiryCategoryController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object ICMS configuration command.
        /// </summary>
        private readonly IInquiryCategoryCommand objIInquiryCategoryCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;
        public const string InquiryCategoryMsg = "Inquiry Category";
        public const string SequenceNoMsg = "Sequence No";

        ///// <summary>
        ///// Initializes a new instance of the <see cref="InquiryCategoryController" /> class.
        ///// </summary>
        ///// <param name="iInquiryCategoryCommand">The i CMS configuration command.</param>
        public InquiryCategoryController(IInquiryCategoryCommand iInquiryCategoryCommand)
        {
            this.objIInquiryCategoryCommand = iInquiryCategoryCommand;
        }

        /// <summary>
        /// CMSs the configuration view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult InquiryCategoryView()
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
        /// CMSs the configuration.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult InquiryCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobilePost));

                InquiryCategoryModel objInquiryCategoryModel = new InquiryCategoryModel();
                Random ran = new Random();
                string cacheclear = DateTime.Now.ToString("ddMMyyhhmmssffff");
                int lgInquiryCategoryId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!Convert.ToBoolean(getPageRights.Add))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objInquiryCategoryModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!Convert.ToBoolean(getPageRights.Edit) || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgInquiryCategoryId = this.Request.QueryString.ToString().Decode().IntSafe();

                        var obj = this.objIInquiryCategoryCommand.GetInquiryCategoryByInquiryCategoryId(lgInquiryCategoryId);
                        if (obj != null)
                        {

                            objInquiryCategoryModel.HdnSession = ran.Next().ToString();
                            objInquiryCategoryModel.Id = obj.Id;
                            objInquiryCategoryModel.Category = obj.Category;
                            objInquiryCategoryModel.MobileNumber = obj.MobileNumber;
                            objInquiryCategoryModel.Email = obj.Email;
                            objInquiryCategoryModel.SeqNo = obj.SeqNo;
                            objInquiryCategoryModel.CategoryImage = obj.CategoryImage;
                        }

                        this.Session["ICImageName"] = objInquiryCategoryModel.CategoryImage;
                        string fileExtImage1 = Path.GetExtension(objInquiryCategoryModel.CategoryImage);
                        objInquiryCategoryModel.CategoryImage = objInquiryCategoryModel.CategoryImage == null ? string.Empty : "../CMSUpload/Document/InquiryCategory/" + objInquiryCategoryModel.Id + "/Original" + fileExtImage1 + "?" + cacheclear;
                    }
                }
                else
                {
                    if (!Convert.ToBoolean(getPageRights.Add))
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objInquiryCategoryModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult InquiryCategory(InquiryCategoryModel inquiryCategoryModel)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobilePost));

                if (inquiryCategoryModel.Id == 0)
                {
                    if (!Convert.ToBoolean(getPageRights.Add))
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (!Convert.ToBoolean(getPageRights.Edit))
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (inquiryCategoryModel.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIInquiryCategoryCommand.IsInquiryCategoryExists(inquiryCategoryModel.Id, inquiryCategoryModel.Category, inquiryCategoryModel.SeqNo);

                bool blSeqExists = this.objIInquiryCategoryCommand.GetDuplicateSeq(inquiryCategoryModel.Id, inquiryCategoryModel.Category, inquiryCategoryModel.SeqNo);

                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage(InquiryCategoryMsg, MessageType.AlreadyExist);
                    return this.View(inquiryCategoryModel);
                }
                else if (blSeqExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage(SequenceNoMsg, MessageType.AlreadyExist);
                    return this.View(inquiryCategoryModel);
                }
                else
                {
                    if (this.Session["CategoryImage"] != null)
                    {
                        inquiryCategoryModel.CategoryImage = this.Session["CategoryImage"].ToString();
                    }

                    if (inquiryCategoryModel.Id > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage(InquiryCategoryMsg, MessageType.UpdateSuccess);
                    }

                    if (inquiryCategoryModel.CategoryImage == null)
                    {
                        var obj = this.objIInquiryCategoryCommand.GetInquiryCategoryByInquiryCategoryId((int)inquiryCategoryModel.Id);
                        inquiryCategoryModel.CategoryImage = obj.CategoryImage;
                    }
                    inquiryCategoryModel.Id = this.objIInquiryCategoryCommand.SaveInquiryCategory(inquiryCategoryModel);

                    if (inquiryCategoryModel.Id > 0)
                    {

                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage(InquiryCategoryMsg, MessageType.Success);
                        }
                        if (!string.IsNullOrEmpty(this.Session["ICImagePath"].ToString()) && inquiryCategoryModel.Id > 0)
                        {
                            Functions.ReSizeImage(this.Session["ICImagePath"].ToString(), inquiryCategoryModel.CategoryImage.ToString(), Convert.ToString(inquiryCategoryModel.Id), "InquiryCategory", "Banner");
                        }

                        return this.View(inquiryCategoryModel);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage(InquiryCategoryMsg, MessageType.Fail);
                    }

                }

                return this.RedirectToAction("InquiryCategoryView", "InquiryCategory");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage(InquiryCategoryMsg, MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(inquiryCategoryModel);
            }
        }

        /// <summary>
        /// Binds the Bind Inquiry Category Grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindInquiryCategoryGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {

                List<MS_SearchInquiryCategoryResult> objInquiryCategoryList = this.objIInquiryCategoryCommand.SearchInquiryCategory(rows, page, search, sidx + " " + sord);
                if (objInquiryCategoryList != null)
                {
                    return this.FillGridInquiryCategory(page, rows, objInquiryCategoryList);
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
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Fills the grid CMS configuration.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objInquiryCategoryList">The object CMS configuration list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridInquiryCategory(int page, int rows, List<MS_SearchInquiryCategoryResult> objInquiryCategoryList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objInquiryCategoryList != null && objInquiryCategoryList.Count > 0 ? objInquiryCategoryList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objInquiryCategory in objInquiryCategoryList
                            select new
                            {
                                SeqNo = objInquiryCategory.SeqNo,
                                Email = objInquiryCategory.Email,
                                MobileNo = objInquiryCategory.MobileNo,
                                Category = objInquiryCategory.Category,
                                Id = objInquiryCategory.Id.ToString().Encode()
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
        /// Validates the CMS configuration.
        /// </summary>
        /// <param name="objInquiryCategory">The object CMS configuration.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateInquiryCategory(InquiryCategoryModel objInquiryCategory)
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
                            Image newImage = this.ResizeImage(oldImage, 130, 98);
                            try
                            {
                                newImage.Save(path);
                            }
                            catch (Exception ex)
                            {
                                information += " - " + ex.Message;
                                logger.Error(ex, information);
                            }

                            oldImage.Dispose();
                            this.Session["ICImagePath"] = strImagepath;
                            this.Session["ICImageName"] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

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

        /// <summary>
        /// Deletes the CMS configuration.
        /// </summary>
        /// <param name="strInquiryCategoryId">The string CMS configuration identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteInquiryCategory(string strInquiryCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string[] strInquiryCategory = strInquiryCategoryId.Split(',');
                strInquiryCategoryId = string.Empty;
                foreach (var item in strInquiryCategory)
                {
                    strInquiryCategoryId += item.Decode() + ",";
                }

                strInquiryCategoryId = strInquiryCategoryId.Substring(0, strInquiryCategoryId.Length - 1);
                DeleteInquiryCategoryResult result = this.objIInquiryCategoryCommand.DeleteInquiryCategory(strInquiryCategoryId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Inquiry Category", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Inquiry Category", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Inquiry Category", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Inquiry Category", MessageType.DeleteFail));
            }
        }
    }
}
