using EcubeWebPortalCMS.Common;
using EcubeWebPortalCMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Controllers
{
    public class MenuMasterController : Controller
    {
        //
        // GET: /MenuMaster/

        /// <summary>
        /// The i menu master..
        /// </summary>
        private readonly IMenuMaster iMenuMaster = new MenuMasterCommand();
        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        [HttpGet]
        public ActionResult MobileMenuList()
        {
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.Common.MobileMenu));
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return this.View();
            }
        }

        [HttpGet]
        public ActionResult MenuMobile()
        {
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.Common.MobileMenu));
                MenuMobileModel objMenuMobileModel = new MenuMobileModel();

                Random ran = new Random();
                objMenuMobileModel.HdnSession = ran.Next().ToString();
                int menuId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objMenuMobileModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        menuId = this.Request.QueryString.ToString().Decode().IntSafe();
                        objMenuMobileModel = this.iMenuMaster.GetMenuDetailsByMenuId(menuId);
                        objMenuMobileModel.HdnSession = ran.Next().ToString();
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objMenuMobileModel != null && objMenuMobileModel.MenuMobileModelList.Count() > 0)
                {
                    foreach (var subMenuMobileModel in objMenuMobileModel.MenuMobileModelList)
                    {
                        string fileExtension = Path.GetExtension(subMenuMobileModel.SubIconImages);
                        subMenuMobileModel.SubMenuIcon = subMenuMobileModel.SubIconImages;
                        subMenuMobileModel.SubIconImages = subMenuMobileModel.SubIconImages == null ? string.Empty : "../CMSUpload/Document/MobileIcon/" + subMenuMobileModel.SubMenuId + "/Icon/Original" + fileExtension;
                    }
                }
                return this.View(objMenuMobileModel);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult MenuMobile(MenuMobileModel objmenuMobileModel)
        {
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.Common.MobileMenu));

                if (objmenuMobileModel.MenuId == 0)
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

                if (objmenuMobileModel.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.iMenuMaster.IsMenuExists(objmenuMobileModel.MenuId, objmenuMobileModel.MenuKey, objmenuMobileModel.MenuDisplayName, objmenuMobileModel.SeqNo);

                string GetDuplicateFields = this.iMenuMaster.GetDuplicateField(objmenuMobileModel.MenuId, objmenuMobileModel.MenuKey, objmenuMobileModel.MenuDisplayName, objmenuMobileModel.SeqNo);

                bool hasDuplicatesInAnyColumn =
                     objmenuMobileModel.MenuMobileModelList.Any(item => objmenuMobileModel.MenuMobileModelList.Count(x => x.SubMenuKey == item.SubMenuKey && !x.SubisDeleted) > 1) ||
                     objmenuMobileModel.MenuMobileModelList.Any(item => objmenuMobileModel.MenuMobileModelList.Count(x => x.SubMenuDisplayName == item.SubMenuDisplayName && !x.SubisDeleted) > 1) ||
                     objmenuMobileModel.MenuMobileModelList.Any(item => objmenuMobileModel.MenuMobileModelList.Count(x => x.SubSeqNo == item.SubSeqNo && !x.SubisDeleted) > 1);


                if (blExists || hasDuplicatesInAnyColumn)
                {
                    this.ViewData["Success"] = "0";
                    if (blExists)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage(GetDuplicateFields, MessageType.AlreadyExist);
                    }
                    if (hasDuplicatesInAnyColumn)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Sub Menu Mobile", MessageType.DuplicateRows);
                    }
                    return this.View(objmenuMobileModel);
                }
                else
                {

                    if (objmenuMobileModel.MenuId > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Menu Mobile", MessageType.UpdateSuccess);
                    }

                    objmenuMobileModel.MenuId = this.iMenuMaster.MenuMobile(objmenuMobileModel);
                    if (objmenuMobileModel.MenuId > 0)
                    {
                        if (objmenuMobileModel != null && objmenuMobileModel.MenuMobileModelList.Count() > 0)
                        {
                            foreach (var subMenuMobileModel in objmenuMobileModel.MenuMobileModelList)
                            {
                                subMenuMobileModel.SubIconImages = subMenuMobileModel.SubMenuIcon;
                            }
                        }

                        List<SubMenuMobileModel> subMenuMobileModels = new List<SubMenuMobileModel>();

                        subMenuMobileModels = this.iMenuMaster.SubMenuMobile(objmenuMobileModel.MenuMobileModelList, objmenuMobileModel.MenuId);

                        if (this.Session["MenuIconImagePath"] != null)
                        {
                            if (subMenuMobileModels.Count() > 0)
                            {
                                foreach (var subMenuMobileModel in subMenuMobileModels)
                                {
                                    Functions.ReSizeImage(this.Session["MenuIconImagePath"].ToString(), subMenuMobileModel.SubIconImages.ToString(), Convert.ToString(subMenuMobileModel.SubMenuId), "MobileIcon", "Icon");
                                }
                            }
                        }

                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Menu Mobile", MessageType.Success);
                        }

                        return this.View(objmenuMobileModel);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Mobile Menu", MessageType.Fail);
                    }
                }

                return this.RedirectToAction("MobileMenuList", "MenuMaster");
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("MenuMobile", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return this.View(objmenuMobileModel);
            }
        }

        public JsonResult DeleteMobileMenu(string strMenuId)
        {
            try
            {
                string[] strInformation = strMenuId.Split(',');
                strMenuId = string.Empty;

                foreach (var item in strInformation)
                {
                    strMenuId += item.Decode() + ",";
                }

                strMenuId = strMenuId.Substring(0, strMenuId.Length - 1);
                DeleteMobileMenuResult result = this.iMenuMaster.DeleteMobileMenu(strMenuId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Mobile Menu", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Mobile Menu", MessageType.DeleteMenuSubMenu, result.Name));
                }

                return this.Json(Functions.AlertMessage("Mobile Menu", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.MenuMobile, MySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Mobile Menu", MessageType.DeleteFail));
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
        /// <returns>ActionResult bind menu master grid.</returns>
        [HttpGet]
        public ActionResult BindMenuMasterGrid(string sidx, string sord, int page, int rows, string search)
        {
            try
            {
                List<SearchMenuMobileResult> objMenuMobileList = this.iMenuMaster.SearchMenuMobile(rows, page, search, sidx + " " + sord);
                if (objMenuMobileList != null)
                {
                    return this.FillGridMenuMobile(page, rows, objMenuMobileList);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }
        /// <summary>
        /// Fills the grid menu mobile
        /// </summary>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="objFacilityCategoryList">The object facility category list Parameter.</param>
        /// <returns>ActionResult fill grid facility category.</returns>
        private ActionResult FillGridMenuMobile(int page, int rows, List<SearchMenuMobileResult> objSearchMenuMobileList)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = objSearchMenuMobileList != null && objSearchMenuMobileList.Count > 0 ? (int)objSearchMenuMobileList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objMenuMobile in objSearchMenuMobileList
                            select new
                            {
                                RowNum = objMenuMobile.RowNum,
                                SubMenu = objMenuMobile.SubMenu,
                                SeqNo = objMenuMobile.SeqNo,
                                MenuDisplayName = objMenuMobile.MenuDisplayName,
                                MenuKey = objMenuMobile.MenuKey,
                                MenuId = objMenuMobile.MenuId.ToString().Encode()
                            })/*.OrderBy(x => x.MenuId).ToArray()*/
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FacilityCategory, MySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Uploads the menu icon image.
        /// </summary>
        /// <param name="data">Parameter data.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult UploadMenuIconImage(FormCollection data)
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
                            //Image newImage = this.ResizeImage(oldImage, 1440, 763);
                            try
                            {
                                //oldImage.Save(path, ImageFormat.Jpeg);
                                file.SaveAs(path); // Save the file as is, without changing the format
                            }
                            catch
                            {
                            }

                            oldImage.Dispose();
                            this.Session["MenuIconImagePath"] = strImagepath;
                            this.Session["MenuIconImageName"] = this.Request.Files["files"].FileName;
                        }
                    }
                }
            }

            return this.Json("1111");
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
