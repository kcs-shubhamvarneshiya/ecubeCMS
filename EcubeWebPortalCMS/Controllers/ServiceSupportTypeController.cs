// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : darpan
// Created          : 11-10-2016
//
// Last Modified By : darpan
// Last Modified On : 11-11-2016
// ***********************************************************************
// <copyright file="ServiceSupportTypeController.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportTypeController.cs</summary>
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
    using System.Web;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    using EcubeWebPortalCMS.Models;
    using Serilog;

    /// <summary>
    /// Class ServiceSupportTypeController.
    /// </summary>
    public class ServiceSupportTypeController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object service support type variable.
        /// </summary>
        private readonly IServiceSupportTypeCommand objServiceSupportType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSupportTypeController"/> class.
        /// </summary>
        /// <param name="iServiceSupportTypeCommand">The i service support type command.</param>
        public ServiceSupportTypeController(IServiceSupportTypeCommand iServiceSupportTypeCommand)
        {
            this.objServiceSupportType = iServiceSupportTypeCommand;
        }

        /// <summary>
        /// Services the support type view.
        /// </summary>
        /// <returns>ActionResult service support type view.</returns>
        public ActionResult ServiceSupportTypeView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupportType);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                this.ViewData["blEditRights"] = objPermission.Edit_Right;
                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;
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
        /// Services the type of the support.
        /// </summary>
        /// <returns>ActionResult service support type.</returns>
        public ActionResult ServiceSupportType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupportType);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ServiceSupportTypeModel objsupport = new ServiceSupportTypeModel();
                long lgsupportId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objsupport.HdnIFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgsupportId = Request.QueryString.ToString().Decode().LongSafe();
                        objsupport = this.objServiceSupportType.GetServiceSupportTypeById(Convert.ToInt32(lgsupportId));
                    }
                }
                else
                {
                    objsupport.IsActive = true;
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }
                    }
                }

                return this.View(objsupport);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Services the type of the support.
        /// </summary>
        /// <param name="objsupport">The object support parameter.</param>
        /// <returns>ActionResult service support type.</returns>
        [HttpPost]
        public ActionResult ServiceSupportType(ServiceSupportTypeModel objsupport)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupportType);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objsupport.Id == 0)
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

                if (objsupport.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool exists = this.objServiceSupportType.IsTypeNameExists(objsupport.Id, objsupport.TypeName);
                if (exists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("ServiceSupportType", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateSupport(objsupport);

                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objServiceSupportType.SaveServiceSupportType(objsupport);
                        if (resultId >= 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("ServiceSupportType", MessageType.Success);
                            return this.View(objsupport);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("ServiceSupportType", MessageType.Fail);
                        }
                    }
                }

                return this.View(objsupport);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("ServiceSupportType", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objsupport);
            }
        }

        /// <summary>
        /// Binds the type of the service support.
        /// </summary>
        /// <param name="sidx">The six parameter.</param>
        /// <param name="sord">The sort parameter.</param>
        /// <param name="page">The page parameter.</param>
        /// <param name="rows">The rows parameter.</param>
        /// <param name="filters">The filters parameter.</param>
        /// <param name="search">The search parameter.</param>
        /// <returns>ActionResult bind service support type.</returns>
        public ActionResult BindServiceSupportType(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<CRMSearchServiceSupportTypeResult> objSearch = this.objServiceSupportType.SearchServiceSupportType(rows, page, search, sidx + " " + sord);
                return this.FillGridServiceSupportType(page, rows, objSearch);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the type of the service support.
        /// </summary>
        /// <param name="strSupportIdList">The string support identifier list parameter.</param>
        /// <returns>Result delete service support type.</returns>
        public JsonResult DeleteServiceSupportType(string strSupportIdList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strSupport = strSupportIdList.Split(',');
                strSupportIdList = string.Empty;
                foreach (var item in strSupport)
                {
                    strSupportIdList += item.Decode() + ",";
                }

                strSupportIdList = strSupportIdList.Substring(0, strSupportIdList.Length - 1);
                CRM_DeleteServiceSupportTypeResult result = this.objServiceSupportType.DeleteServiceSupportType(strSupportIdList, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("ServiceSupportType", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("ServiceSupportType", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("ServiceSupportType", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("ServiceSupportType", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Fills the type of the grid service support.
        /// </summary>
        /// <param name="page">The page parameter.</param>
        /// <param name="rows">The rows parameter.</param>
        /// <param name="objSearchService">The object search service parameter.</param>
        /// <returns>ActionResult fill grid service support type.</returns>
        private ActionResult FillGridServiceSupportType(int page, int rows, List<CRMSearchServiceSupportTypeResult> objSearchService)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objSearchService != null && objSearchService.Count > 0 ? (int)objSearchService[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objsearchsupport in objSearchService
                            select new
                            {
                                TypeName = objsearchsupport.TypeName,
                                Description = objsearchsupport.Description,
                                IsActive = objsearchsupport.IsActive,
                                Id = objsearchsupport.Id.ToString().Encode()
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
        /// <param name="objSupport">The object support parameter.</param>
        /// <returns>System.String validate support.</returns>
        private string ValidateSupport(ServiceSupportTypeModel objSupport)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objSupport.TypeName))
                {
                    strErrorMsg += Functions.AlertMessage("Type Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objSupport.Description))
                {
                    strErrorMsg += Functions.AlertMessage("Description", MessageType.InputRequired) + "<br/>";
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
