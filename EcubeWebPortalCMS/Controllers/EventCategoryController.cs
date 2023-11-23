// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="EventCategoryController.cs" company="string.Empty">
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
    /// Class EventCategoryController.
    /// </summary>
    public class EventCategoryController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object ICMS configuration command.
        /// </summary>
        private readonly IEventCategoryCommand objIEventCategoryCommand = null;

        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCategoryController" /> class.
        /// </summary>
        /// <param name="iEventCategoryCommand">The i CMS configuration command.</param>
        public EventCategoryController(IEventCategoryCommand iEventCategoryCommand)
        {
            this.objIEventCategoryCommand = iEventCategoryCommand;
        }

        /// <summary>
        /// CMSs the configuration view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult EventCategoryView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventCategory));
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
        public ActionResult EventCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventCategory));

                EventCategoryModel objEventCategoryModel = new EventCategoryModel();
                int lgEventCategoryId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objEventCategoryModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgEventCategoryId = Request.QueryString.ToString().Decode().IntSafe();
                        objEventCategoryModel = this.objIEventCategoryCommand.GetEventCategoryByEventCategoryId(lgEventCategoryId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objEventCategoryModel);
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
        /// <param name="objEventCategory">The object CMS configuration.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult EventCategory(EventCategoryModel objEventCategory)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventCategory));

                if (objEventCategory.Id == 0)
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

                if (objEventCategory.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIEventCategoryCommand.IsEventCategoryExists(objEventCategory.Id, objEventCategory.EventCategoryName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateEventCategory(objEventCategory);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objEventCategory.Id = this.objIEventCategoryCommand.SaveEventCategory(objEventCategory);
                        if (objEventCategory.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.Success);
                            return this.View(objEventCategory);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.Fail);
                        }
                    }
                }

                return this.View(objEventCategory);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEventCategory);
            }
        }

        /// <summary>
        /// Saves the Event Category.
        /// </summary>
        /// <param name="objEventCategory">The object CMS configuration.</param>
        /// <returns>The Action Result.</returns>
        public ActionResult SaveEventCategory(EventCategoryModel objEventCategory)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventCategory));

                if (objEventCategory.Id == 0)
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

                if (objEventCategory.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false; //// objIEventCategoryCommand.IsEventCategoryExists(objEventCategory.Id, objEventCategory.PersonName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateEventCategory(objEventCategory);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objEventCategory.Id = this.objIEventCategoryCommand.SaveEventCategory(objEventCategory);
                        if (objEventCategory.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.Success);
                            return this.View(objEventCategory);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.Fail);
                        }
                    }
                }

                return this.View(objEventCategory);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Category", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEventCategory);
            }
        }

        /// <summary>
        /// Binds the Bind Event Category Grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindEventCategoryGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                List<MS_SearchEventCategoryResult> objEventCategoryList = this.objIEventCategoryCommand.SearchEventCategory(rows, page, search, sidx + " " + sord);
                if (objEventCategoryList != null && objEventCategoryList.Count > 0)
                {
                    return this.FillGridEventCategory(page, rows, objEventCategoryList);
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
        /// <param name="strEventCategoryId">The string CMS configuration identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteEventCategory(string strEventCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string[] strEventCategory = strEventCategoryId.Split(',');
                strEventCategoryId = string.Empty;
                foreach (var item in strEventCategory)
                {
                    strEventCategoryId += item.Decode() + ",";
                }

                strEventCategoryId = strEventCategoryId.Substring(0, strEventCategoryId.Length - 1);
                MS_DeleteEventCategoryResult result = this.objIEventCategoryCommand.DeleteEventCategory(strEventCategoryId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Event Category", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Event Category", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Event Category", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Event Category", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the CMS configuration.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetEventCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                return this.Json(this.objIEventCategoryCommand.GetAllEventCategoryForDropDown(), JsonRequestBehavior.AllowGet);
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
        /// <param name="objEventCategoryList">The object CMS configuration list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridEventCategory(int page, int rows, List<MS_SearchEventCategoryResult> objEventCategoryList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int pageSize = rows;
                int totalRecords = objEventCategoryList != null && objEventCategoryList.Count > 0 ? objEventCategoryList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEventCategory in objEventCategoryList
                            select new
                            {
                                EventCategoryName = objEventCategory.EventCategoryName,
                                Id = objEventCategory.Id.ToString().Encode()
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
        /// <param name="objEventCategory">The object CMS configuration.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateEventCategory(EventCategoryModel objEventCategory)
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
