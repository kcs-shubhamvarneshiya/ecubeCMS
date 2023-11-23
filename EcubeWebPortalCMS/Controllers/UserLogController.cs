// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="UserLogController.cs" company="string.Empty">
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
    /// Class UserLogController.
    /// </summary>
    public class UserLogController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Zero-based index of the user log.
        /// </summary>
        private readonly IUserLogCommand objIUserLogCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLogController"/> class.
        /// </summary>
        /// <param name="iUserLogCommand">The i user log command.</param>
        public UserLogController(IUserLogCommand iUserLogCommand)
        {
            this.objIUserLogCommand = iUserLogCommand;
        }

        /// <summary>
        /// Users the log view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult UserLogView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.UserLog);
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
        /// Binds the user log grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindUserLogGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchUserLogResult> objUserLogList = this.objIUserLogCommand.SearchUserLog(rows, page, search, sidx + " " + sord);
                return this.FillGridUserLog(page, rows, objUserLogList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Fills the grid user log.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objUserLogList">The object user log list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridUserLog(int page, int rows, List<SearchUserLogResult> objUserLogList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objUserLogList != null && objUserLogList.Count > 0 ? objUserLogList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objUserLog in objUserLogList
                            select new
                            {
                                UserId = objUserLog.UserId.ToString(),
                                UserName = objUserLog.UserName,
                                PageId = objUserLog.PageId.ToString(),
                                Action = objUserLog.Action,
                                IpAddress = objUserLog.IpAddress,
                                AccessType = objUserLog.AccessType,
                                Location = objUserLog.Location,
                                AccessOn = objUserLog.AccessOn,
                                CreatedOn = objUserLog.CreatedOn.ToString(Functions.DateTimeFormat),
                                Id = objUserLog.Id.ToString().Encode()
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
