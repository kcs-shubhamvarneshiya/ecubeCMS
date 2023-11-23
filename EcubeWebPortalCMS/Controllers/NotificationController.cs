// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-15-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="NotificationController.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>NotificationController.cs</summary>
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
    /// Class NotificationController.
    /// </summary>
    public class NotificationController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        // GET: /Notification/

        /// <summary>
        /// The i notification.
        /// </summary>
        private readonly INotification iNotification = new NotificationCommand();

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult index.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Notifications the view.
        /// </summary>
        /// <returns>ActionResult notification view.</returns>
        public ActionResult NotificationView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobileNotification));
                if (getPageRights == null || getPageRights.PageId == 0 || getPageRights.ViewAll == false || getPageRights.ViewDetail == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = getPageRights.Add;
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
        /// Notifications this instance.
        /// </summary>
        /// <returns>ActionResult notification.</returns>
        public ActionResult Notification()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobileNotification));

                NotificationModel objNotificationModel = new NotificationModel();
                Random ran = new Random();
                objNotificationModel.HdnSession = ran.Next().ToString();
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objNotificationModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objNotificationModel);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Notifications the specified object notification.
        /// </summary>
        /// <param name="objNotification">The object notification Parameter.</param>
        /// <returns>ActionResult notification.</returns>
        [HttpPost]
        public ActionResult Notification(NotificationModel objNotification)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.MobileNotification));

                if (getPageRights.Add == false)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                if (objNotification.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateNotification(objNotification);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    objNotification.Id = this.iNotification.SaveNotification(objNotification);
                    if (objNotification.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Notification", MessageType.Success);
                        }

                        return this.View(objNotification);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Notification", MessageType.Fail);
                    }
                }

                return this.View(objNotification);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Notification", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objNotification);
            }
        }

        /// <summary>
        /// Binds the notification grid.
        /// </summary>
        /// <param name="sidx">The sidx Parameter.</param>
        /// <param name="sord">The sord Parameter.</param>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="search">The search Parameter.</param>
        /// <returns>ActionResult bind notification grid.</returns>
        [HttpGet]
        public ActionResult BindNotificationGrid(string sidx, string sord, int page, int rows, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchNotificationResult> objNotificationList = this.iNotification.SearchNotification(rows, page, search, sidx + " " + sord);
                if (objNotificationList != null)
                {
                    return this.FillGridNotification(page, rows, objNotificationList);
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
        /// Fills the grid notification.
        /// </summary>
        /// <param name="page">The page Parameter.</param>
        /// <param name="rows">The rows Parameter.</param>
        /// <param name="objNotificationList">The object notification list Parameter.</param>
        /// <returns>ActionResult fill grid notification.</returns>
        private ActionResult FillGridNotification(int page, int rows, List<SearchNotificationResult> objNotificationList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objNotificationList != null && objNotificationList.Count > 0 ? (int)objNotificationList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objNotification in objNotificationList
                            select new
                            {
                                NotificationTitles = objNotification.NotificationTitle,
                                Description = objNotification.NotificationDescription,
                                Id = objNotification.Id.ToString().Encode()
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
        /// Validates the notification.
        /// </summary>
        /// <param name="objNotification">The object notification Parameter.</param>
        /// <returns>System.String validate notification.</returns>
        private string ValidateNotification(NotificationModel objNotification)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objNotification.NotificationTitle))
                {
                    strErrorMsg += Functions.AlertMessage("Notification Title", MessageType.InputRequired) + "<br/>";
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
    }
}