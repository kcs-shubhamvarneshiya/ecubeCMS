// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : darpan
// Created          : 11-12-2016
//
// Last Modified By : darpan
// Last Modified On : 11-12-2016
// ***********************************************************************
// <copyright file="ServiceSupportLogController.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>ServiceSupportLogController.cs</summary>
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
    /// Class ServiceSupportLogController.
    /// </summary>
    public class ServiceSupportLogController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object support log variable.
        /// </summary>
        private readonly IServiceSupportLogCommand objSupportLog = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSupportLogController"/> class.
        /// </summary>
        /// <param name="iSupportLog">The i support log.</param>
        public ServiceSupportLogController(IServiceSupportLogCommand iSupportLog)
        {
            this.objSupportLog = iSupportLog;
        }

        /// <summary>
        /// Services the support log.
        /// </summary>
        /// <returns>ActionResult service support log.</returns>
        public ActionResult ServiceSupportLog()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ServiceSupportLog);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                ServiceSupportLogModel objUser = new ServiceSupportLogModel();
                this.BindddlforSupport(objUser, true);

                return this.View(objUser);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Binds the support log grid.
        /// </summary>
        /// <param name="sidx">The six parameter.</param>
        /// <param name="sord">The sort parameter.</param>
        /// <param name="page">The page parameter.</param>
        /// <param name="rows">The rows parameter.</param>
        /// <param name="filters">The filters parameter.</param>
        /// <param name="search">The search parameter.</param>
        /// <param name="status">The status parameter.</param>
        /// <returns>ActionResult bind support log grid.</returns>
        public ActionResult BindSupportLogGrid(string sidx, string sord, int page, int rows, string filters, string search, string status)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<CRM_SearchServiceSupportLogResult> objSearch = this.objSupportLog.SearchServiceSupportLog(rows, page, search, status, sidx + " " + sord);
                return this.FillGridServiceSupportLog(page, rows, objSearch);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Bind for the support.
        /// </summary>
        /// <param name="objSupport">The object support parameter.</param>
        /// <param name="blBindDropDownFromDb">If set to <c>true</c> [bind drop down from database].</param>
        public void BindddlforSupport(ServiceSupportLogModel objSupport, bool blBindDropDownFromDb)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (blBindDropDownFromDb)
                {
                    objSupport.StatusList = this.objSupportLog.GetAllStatus().ToList();
                }
                else
                {
                    objSupport.StatusList = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Fills the grid service support log.
        /// </summary>
        /// <param name="page">The page parameter.</param>
        /// <param name="rows">The rows parameter.</param>
        /// <param name="objSearchService">The object search service parameter.</param>
        /// <returns>ActionResult fill grid service support log.</returns>
        private ActionResult FillGridServiceSupportLog(int page, int rows, List<CRM_SearchServiceSupportLogResult> objSearchService)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objSearchService != null && objSearchService.Count > 0 ? (int)objSearchService[0].RowNum : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objsearchsupport in objSearchService
                            select new
                            {
                                MemberCode = objsearchsupport.FName + ' ' + objsearchsupport.LName + '-' + objsearchsupport.MemberCode,
                                TypeName = objsearchsupport.TypeName,
                                StatusDesc = objsearchsupport.StatusDesc,
                                Description = objsearchsupport.Description,
                                FirstName = objsearchsupport.FirstName,
                                CreatedOn = objsearchsupport.Date.ToString(),
                                ServiceSupportId = objsearchsupport.ServiceSupportId.ToString()
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
