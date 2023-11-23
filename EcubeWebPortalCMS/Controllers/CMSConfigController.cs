// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="CMSConfigController.cs" company="string.Empty">
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
    using System.Linq;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class CMSConfigController.
    /// </summary>
    public class CMSConfigController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object ICMS configuration command.
        /// </summary>
        private readonly ICMSConfigCommand objICMSConfigCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="CMSConfigController" /> class.
        /// </summary>
        /// <param name="iCMSConfigCommand">The i CMS configuration command.</param>
        public CMSConfigController(ICMSConfigCommand iCMSConfigCommand)
        {
            this.objICMSConfigCommand = iCMSConfigCommand;
        }

        /// <summary>
        /// CMSs the configuration view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult CMSConfigView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetRoomConfiguration));
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
        public ActionResult CMSConfig()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetRoomConfiguration));

                CMSConfigModel objCMSConfigModel = new CMSConfigModel();
                long lgCMSConfigId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objCMSConfigModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgCMSConfigId = Request.QueryString.ToString().Decode().LongSafe();
                        objCMSConfigModel = this.objICMSConfigCommand.GetCMSConfigByCMSConfigId(lgCMSConfigId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objCMSConfigModel);
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
        /// <param name="objCMSConfig">The object CMS configuration.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult CMSConfig(CMSConfigModel objCMSConfig)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetRoomConfiguration));

                if (objCMSConfig.Id == 0)
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

                if (objCMSConfig.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false; //// objICMSConfigCommand.IsCMSConfigExists(objCMSConfig.Id, objCMSConfig.PersonName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateCMSConfig(objCMSConfig);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objCMSConfig.Id = this.objICMSConfigCommand.SaveCMSConfig(objCMSConfig);
                        if (objCMSConfig.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.Success);
                            return this.View(objCMSConfig);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.Fail);
                        }
                    }
                }

                return this.View(objCMSConfig);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objCMSConfig);
            }
        }

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objCMSConfig">The object CMS configuration.</param>
        /// <returns>The Action Result.</returns>
        public ActionResult SaveCMSConfig(CMSConfigModel objCMSConfig)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.BanquetRoomConfiguration));
                if (objCMSConfig.Id == 0)
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

                if (objCMSConfig.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false; //// objICMSConfigCommand.IsCMSConfigExists(objCMSConfig.Id, objCMSConfig.PersonName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateCMSConfig(objCMSConfig);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objCMSConfig.Id = this.objICMSConfigCommand.SaveCMSConfig(objCMSConfig);
                        if (objCMSConfig.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.Success);
                            return this.View(objCMSConfig);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.Fail);
                        }
                    }
                }

                return this.View(objCMSConfig);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Configuration", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objCMSConfig);
            }
        }

        /// <summary>
        /// Binds the CMS configuration grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindCMSConfigGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<MS_SearchCMSConfigResult> objCMSConfigList = this.objICMSConfigCommand.SearchCMSConfig(rows, page, search, sidx + " " + sord);
                if (objCMSConfigList != null && objCMSConfigList.Count > 0)
                {
                    return this.FillGridCMSConfig(page, rows, objCMSConfigList);
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
        /// Deletes the CMS configuration.
        /// </summary>
        /// <param name="strCMSConfigId">The string CMS configuration identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteCMSConfig(string strCMSConfigId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string[] strCMSConfig = strCMSConfigId.Split(',');
                strCMSConfigId = string.Empty;
                foreach (var item in strCMSConfig)
                {
                    strCMSConfigId += item.Decode() + ",";
                }

                strCMSConfigId = strCMSConfigId.Substring(0, strCMSConfigId.Length - 1);
                MS_DeleteCMSConfigResult result = this.objICMSConfigCommand.DeleteCMSConfig(strCMSConfigId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Configuration", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Configuration", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Configuration", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Configuration", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the CMS configuration.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetCMSConfig()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objICMSConfigCommand.GetAllCMSConfigForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fills the grid CMS configuration.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objCMSConfigList">The object CMS configuration list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridCMSConfig(int page, int rows, List<MS_SearchCMSConfigResult> objCMSConfigList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objCMSConfigList != null && objCMSConfigList.Count > 0 ? objCMSConfigList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objCMSConfig in objCMSConfigList
                            select new
                            {
                                Module = objCMSConfig.Module,
                                PersonName = objCMSConfig.PersonName,
                                EmailID = objCMSConfig.EmailID,
                                MobileNo = objCMSConfig.MobileNo,
                                Id = objCMSConfig.Id.ToString().Encode()
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
        /// <param name="objCMSConfig">The object CMS configuration.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateCMSConfig(CMSConfigModel objCMSConfig)
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
    }
}
