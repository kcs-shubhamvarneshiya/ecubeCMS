// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-10-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-10-2016
// ***********************************************************************
// <copyright file="ServiceSupportController.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportController</summary>
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
    /// Class ServiceSupportController.
    /// </summary>
    public class ServiceSupportController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i service support command.
        /// </summary>
        private readonly IServiceSupportCommand objIServiceSupportCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSupportController"/> class.
        /// </summary>
        /// <param name="iobjIServiceSupportCommand">The service support command.</param>
        public ServiceSupportController(IServiceSupportCommand iobjIServiceSupportCommand)
        {
            this.objIServiceSupportCommand = iobjIServiceSupportCommand;
        }

        /// <summary>
        /// Services the support view.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult ServiceSupportView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupport);
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
        /// Services the support .
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult ServiceSupport()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupport);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ServiceSupportModel objServiceSupport = new ServiceSupportModel();
                long lgServiceSupportId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objServiceSupport.HdnIFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgServiceSupportId = Request.QueryString.ToString().LongSafe();
                        objServiceSupport = this.objIServiceSupportCommand.GetServiceSupportById(lgServiceSupportId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.BindDropDownListForStatus(objServiceSupport, true);
                this.BindDropDownListForSupportType(objServiceSupport, true);
                return this.View(objServiceSupport);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Services the support.
        /// </summary>
        /// <param name="objServiceSupport">The object service support.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ServiceSupport(ServiceSupportModel objServiceSupport)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupport);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objServiceSupport.Id == 0)
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

                if (objServiceSupport.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                long lgServiceSupportId = 0;
                string strErrorMsg = this.ValidateServiceSupport(objServiceSupport);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                    lgServiceSupportId = Request.QueryString.ToString().LongSafe();
                    objServiceSupport = this.objIServiceSupportCommand.GetServiceSupportById(lgServiceSupportId);
                }
                else
                {
                    if (objServiceSupport.Id > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Service Support", MessageType.Success);
                    }

                    objServiceSupport.Id = this.objIServiceSupportCommand.SaveServiceSupport(objServiceSupport);
                    if (objServiceSupport.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Service Support", MessageType.Success);
                        }

                        this.BindDropDownListForStatus(objServiceSupport, true);
                        this.BindDropDownListForSupportType(objServiceSupport, true);
                        return this.View(objServiceSupport);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Service Support", MessageType.Fail);
                    }
                }

                this.BindDropDownListForStatus(objServiceSupport, true);
                this.BindDropDownListForSupportType(objServiceSupport, true);

                return this.View(objServiceSupport);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Service Support", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objServiceSupport);
            }
        }

        /// <summary>
        /// Binds the service support grid.
        /// </summary>
        /// <param name="sidx">The SIDX .</param>
        /// <param name="sord">The SORD .</param>
        /// <param name="page">The page .</param>
        /// <param name="rows">The rows .</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action Result.</returns>
        public ActionResult BindServiceSupportGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<CRMSearchServiceSupportResult> objServiceSupportList = this.objIServiceSupportCommand.SearchServiceSupport(rows, page, search, sidx + " " + sord);
                return this.FillGrid(page, rows, objServiceSupportList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Binds the drop down list for status.
        /// </summary>
        /// <param name="objServiceSupport">The object service support.</param>
        /// <param name="blBindDropDownFromDb">If set to <c>true</c> [bind drop down from database].</param>
        public void BindDropDownListForStatus(ServiceSupportModel objServiceSupport, bool blBindDropDownFromDb)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (blBindDropDownFromDb)
                {
                    objServiceSupport.StatusList = this.objIServiceSupportCommand.GetStatusForDropDown().ToList();
                }
                else
                {
                    objServiceSupport.StatusList = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Binds the type of the drop down list for support.
        /// </summary>
        /// <param name="objServiceSupport">The object service support.</param>
        /// <param name="blBindDropDownFromDb">If set to <c>true</c> [bind drop down from database].</param>
        public void BindDropDownListForSupportType(ServiceSupportModel objServiceSupport, bool blBindDropDownFromDb)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (blBindDropDownFromDb)
                {
                    objServiceSupport.SupportTypeList = this.objIServiceSupportCommand.GetSupportTypeForDropDown().ToList();
                }
                else
                {
                    objServiceSupport.SupportTypeList = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Services the support report.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult ServiceSupportReport()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupport);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ServiceSupportModel objServiceSupport = new ServiceSupportModel();
                long lgServiceSupportId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objServiceSupport.HdnIFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgServiceSupportId = Request.QueryString.ToString().LongSafe();
                        objServiceSupport = this.objIServiceSupportCommand.GetServiceReportById(lgServiceSupportId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objServiceSupport);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Gets the service support report.
        /// </summary>
        /// <param name="servicesupportId">The service support identifier.</param>
        /// <returns>JSON Result.</returns>
        public JsonResult GetServiceSupportReport(int servicesupportId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objIServiceSupportCommand.GetServiceSupportReportById(servicesupportId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;

            }
        }

        /// <summary>
        /// Validates the service support.
        /// </summary>
        /// <param name="objServiceSupport">The object service support.</param>
        /// <returns>System String.</returns>
        private string ValidateServiceSupport(ServiceSupportModel objServiceSupport)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objServiceSupport.StatusId.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Status", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objServiceSupport.Response))
                {
                    strErrorMsg += Functions.AlertMessage("Response", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objServiceSupport.ResponseBy))
                {
                    strErrorMsg += Functions.AlertMessage("Responsible Person", MessageType.InputRequired) + "<br/>";
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
        /// Fills the grid.
        /// </summary>
        /// <param name="page">The page .</param>
        /// <param name="rows">The rows .</param>
        /// <param name="objServiceSupportList">The object service support list.</param>
        /// <returns>Action Result.</returns>
        private ActionResult FillGrid(int page, int rows, List<CRMSearchServiceSupportResult> objServiceSupportList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objServiceSupportList != null && objServiceSupportList.Count > 0 ? objServiceSupportList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedServiceSupportCol = objServiceSupportList;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objServiceSupport in pagedServiceSupportCol
                            select new
                            {
                                id = objServiceSupport.Id.ToString(),
                                TypeName = objServiceSupport.TypeName,
                                Description = objServiceSupport.Description,
                                Member = objServiceSupport.Fname + ' ' + objServiceSupport.Lname + '-' + objServiceSupport.MemberCode,
                                StatusDesc = objServiceSupport.StatusDesc,
                                CreatedOn = objServiceSupport.CreatedOn.ToString()
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
