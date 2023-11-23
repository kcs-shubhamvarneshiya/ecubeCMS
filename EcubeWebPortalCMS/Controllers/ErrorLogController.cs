// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="ErrorLogController.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The EcubeWebPortalCMS namespace.
/// </summary>
namespace EcubeWebPortalCMS
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
    /// Class ErrorLogController.
    /// </summary>
    public class ErrorLogController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i error log command.
        /// </summary>
        private readonly IErrorLogCommand objIErrorLogCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorLogController"/> class.
        /// </summary>
        /// <param name="iErrorLogCommand">The i error log command.</param>
        public ErrorLogController(IErrorLogCommand iErrorLogCommand)
        {
            this.objIErrorLogCommand = iErrorLogCommand;
        }

        /// <summary>
        /// Errors the log view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult ErrorLogView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.ErrorLog);
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
        /// Binds the error log grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindErrorLogGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<SearchErrorLogResult> objErrorLogList = this.objIErrorLogCommand.SearchErrorLog(rows, page, search, sidx + " " + sord);
                return this.FillGrid(page, rows, objErrorLogList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Fills the grid.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objErrorLogList">The object error log list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGrid(int page, int rows, List<SearchErrorLogResult> objErrorLogList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objErrorLogList != null && objErrorLogList.Count > 0 ? objErrorLogList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedErrorLogCol = objErrorLogList;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objErrorLog in pagedErrorLogCol
                            select new
                            {
                                Id = objErrorLog.Id,
                                PageName = objErrorLog.PageName,
                                MethodName = objErrorLog.MethodName,
                                ErrorType = objErrorLog.ErrorType,
                                ErrorMessage = objErrorLog.ErrorMessage,
                                ErrorDetails = objErrorLog.ErrorDetails,
                                ErrorDate = objErrorLog.ErrorDate.ToString(Functions.DateTimeFormat),
                                UserName = objErrorLog.UserName
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