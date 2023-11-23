// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-21-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-21-2016
// ***********************************************************************
// <copyright file="RulesController.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>RulesController</summary>
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
    /// Class RulesController.
    /// </summary>
    public class RulesController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object i vendor command.
        /// </summary>
        private readonly ICustomPageCommand objICustomPageCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RulesController" /> class.
        /// </summary>
        /// <param name="iCustomPageCommand">The i custom page command.</param>
        public RulesController(ICustomPageCommand iCustomPageCommand)
        {
            this.objICustomPageCommand = iCustomPageCommand;
        }

        /// <summary>
        /// Vendors this instance.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult Rules()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string pageName = "Rules";
                int pageId = 0;
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Rules);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                CustomPageModel objVendor = new CustomPageModel();
                objVendor = this.objICustomPageCommand.GetCustomPageById(pageId, pageName);

                return this.View(objVendor);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

                return this.View();
            }
        }

        /// <summary>
        /// Vendors the specified object vendor.
        /// </summary>
        /// <param name="objVendor">The object vendor.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult Rules(CustomPageModel objVendor)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Rules);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objVendor.Id == 0)
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objVendor.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateRules(objVendor);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    if (objVendor.Id > 0)
                    {
                        this.ViewData["Message"] = Functions.AlertMessage("Rules", MessageType.Success);
                    }

                    objVendor.Id = this.objICustomPageCommand.SaveCustomPage(objVendor);
                    if (objVendor.Id > 0)
                    {
                        this.ViewData["Success"] = "1";
                        if (this.ViewData["Message"] == null || this.ViewData["Message"].ToString() == string.Empty)
                        {
                            this.ViewData["Message"] = Functions.AlertMessage("Rules", MessageType.Success);
                        }

                        return this.View(objVendor);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Rules", MessageType.Fail);
                    }
                }

                return this.View(objVendor);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Rules", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);

                return this.View(objVendor);
            }
        }

        /// <summary>
        /// Validates the vendor.
        /// </summary>
        /// <param name="objVendor">The object vendor.</param>
        /// <returns>System String.</returns>
        private string ValidateRules(CustomPageModel objVendor)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objVendor.PageContent))
                {
                    strErrorMsg += Functions.AlertMessage("Page Content", MessageType.InputRequired) + "<br/>";
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
