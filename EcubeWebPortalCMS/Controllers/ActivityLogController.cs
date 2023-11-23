// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="ActivityLogController.cs" company="string.Empty">
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
    /// Class ActivityLogController.
    /// </summary>
    public class ActivityLogController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i activity log command.
        /// </summary>
        private readonly IActivityLogCommand objIActivityLogCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogController"/> class.
        /// </summary>
        /// <param name="iActivityLogCommand">The i activity log command.</param>
        public ActivityLogController(IActivityLogCommand iActivityLogCommand)
        {
            this.objIActivityLogCommand = iActivityLogCommand;
        }

        /// <summary>
        /// Activity log view.
        /// </summary>
        /// <returns>An Action Result.</returns>
        /// <remarks>GHANSHYAM, 11/10/2014.</remarks>
        public ActionResult ActivityLogView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ActivityLog);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

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
        /// Bind activity log grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>An Action Result.</returns>
        /// <remarks>GHANSHYAM, 11/10/2014.</remarks>
        public ActionResult BindActivityLogGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<SearchActivityLogResult> objActivityLogList = this.objIActivityLogCommand.SearchActivityLog(rows, page, search, sidx + " " + sord);
                return this.FillGridActivityLog(page, rows, objActivityLogList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Fill grid activity log.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objActivityLogList">The list activity log.</param>
        /// <returns>An Action Result.</returns>
        /// <remarks>GHANSHYAM, 11/10/2014.</remarks>
        private ActionResult FillGridActivityLog(int page, int rows, List<SearchActivityLogResult> objActivityLogList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objActivityLogList != null && objActivityLogList.Count > 0 ? objActivityLogList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objActivityLog in objActivityLogList
                            select new
                            {
                                UserId = objActivityLog.UserId.ToString(),
                                UserName = objActivityLog.UserName,
                                PageId = objActivityLog.PageId.ToString(),
                                PageName = objActivityLog.PageName,
                                AuditComments = objActivityLog.AuditComments,
                                TableName = objActivityLog.TableName,
                                RecordId = objActivityLog.RecordId.ToString(),
                                CreatedOn = objActivityLog.CreatedOn.ToString(Functions.DateTimeFormat),
                                Id = objActivityLog.Id.ToString().Encode()
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
