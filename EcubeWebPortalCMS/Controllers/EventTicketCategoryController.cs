// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="EventTicketCategoryController.cs" company="string.Empty">
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
    /// Class EventTicketCategoryController.
    /// </summary>
    public class EventTicketCategoryController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The object ICMS configuration command.
        /// </summary>
        private readonly IEventTicketCategoryCommand objIEventTicketCategoryCommand = null;


        private UserCommand userCommand;
        private GetPageRightsByUserIdResult getPageRights;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTicketCategoryController" /> class.
        /// </summary>
        /// <param name="iEventTicketCategoryCommand">The i CMS configuration command.</param>
        public EventTicketCategoryController(IEventTicketCategoryCommand iEventTicketCategoryCommand)
        {
            this.objIEventTicketCategoryCommand = iEventTicketCategoryCommand;
        }

        /// <summary>
        /// CMSs the configuration view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult EventTicketCategoryView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventTicketCategory));
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
        public ActionResult EventTicketCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventTicketCategory));

                EventTicketCategoryModel objEventTicketCategoryModel = new EventTicketCategoryModel();
                int lgEventTicketCategoryId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (getPageRights.Add == false)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objEventTicketCategoryModel.HdnIFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (getPageRights.Edit == false || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgEventTicketCategoryId = Request.QueryString.ToString().Decode().IntSafe();
                        objEventTicketCategoryModel = this.objIEventTicketCategoryCommand.GetEventTicketCategoryByEventTicketCategoryId(lgEventTicketCategoryId);
                    }
                }
                else
                {
                    if (getPageRights.Add == false)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objEventTicketCategoryModel);
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
        /// <param name="objEventTicketCategory">The object CMS configuration.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult EventTicketCategory(EventTicketCategoryModel objEventTicketCategory)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventTicketCategory));

                if (objEventTicketCategory.Id == 0)
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

                if (objEventTicketCategory.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIEventTicketCategoryCommand.IsEventTicketCategoryExists(objEventTicketCategory.Id, objEventTicketCategory.EventTicketCategoryName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateEventTicketCategory(objEventTicketCategory);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objEventTicketCategory.Id = this.objIEventTicketCategoryCommand.SaveEventTicketCategory(objEventTicketCategory);
                        if (objEventTicketCategory.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.Success);
                            return this.View(objEventTicketCategory);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.Fail);
                        }
                    }
                }

                return this.View(objEventTicketCategory);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEventTicketCategory);
            }
        }

        /// <summary>
        /// Saves the CMS configuration.
        /// </summary>
        /// <param name="objEventTicketCategory">The object CMS configuration.</param>
        /// <returns>The Action Result.</returns>
        public ActionResult SaveEventTicketCategory(EventTicketCategoryModel objEventTicketCategory)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                userCommand = new UserCommand();
                getPageRights = userCommand.GetPageRights(MySession.Current.UserId, Functions.GetEnumDescription(Common.EventTicketCategory));

                if (objEventTicketCategory.Id == 0)
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

                if (objEventTicketCategory.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = false; //// objIEventTicketCategoryCommand.IsEventTicketCategoryExists(objEventTicketCategory.Id, objEventTicketCategory.PersonName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateEventTicketCategory(objEventTicketCategory);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objEventTicketCategory.Id = this.objIEventTicketCategoryCommand.SaveEventTicketCategory(objEventTicketCategory);
                        if (objEventTicketCategory.Id > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.Success);
                            return this.View(objEventTicketCategory);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.Fail);
                        }
                    }
                }

                return this.View(objEventTicketCategory);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Event Ticket Category", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objEventTicketCategory);
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
        public ActionResult BindEventTicketCategoryGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<MS_SearchEventTicketCategoryResult> objEventTicketCategoryList = this.objIEventTicketCategoryCommand.SearchEventTicketCategory(rows, page, search, sidx + " " + sord);
                if (objEventTicketCategoryList != null && objEventTicketCategoryList.Count > 0)
                {
                    return this.FillGridEventTicketCategory(page, rows, objEventTicketCategoryList);
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
        /// <param name="strEventTicketCategoryId">The string CMS configuration identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteEventTicketCategory(string strEventTicketCategoryId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strEventTicketCategory = strEventTicketCategoryId.Split(',');
                strEventTicketCategoryId = string.Empty;
                foreach (var item in strEventTicketCategory)
                {
                    strEventTicketCategoryId += item.Decode() + ",";
                }

                strEventTicketCategoryId = strEventTicketCategoryId.Substring(0, strEventTicketCategoryId.Length - 1);
                MS_DeleteEventTicketCategoryResult result = this.objIEventTicketCategoryCommand.DeleteEventTicketCategory(strEventTicketCategoryId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Event Ticket Category", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Event Ticket Category", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Event Ticket Category", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Event Ticket Category", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the CMS configuration.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetEventTicketCategory()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIEventTicketCategoryCommand.GetAllEventTicketCategoryForDropDown(), JsonRequestBehavior.AllowGet);
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
        /// <param name="objEventTicketCategoryList">The object CMS configuration list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGridEventTicketCategory(int page, int rows, List<MS_SearchEventTicketCategoryResult> objEventTicketCategoryList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objEventTicketCategoryList != null && objEventTicketCategoryList.Count > 0 ? objEventTicketCategoryList[0].Total.Value : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objEventTicketCategory in objEventTicketCategoryList
                            select new
                            {
                                EventTicketCategoryName = objEventTicketCategory.EventTicketCategoryName,
                                Id = objEventTicketCategory.Id.ToString().Encode()
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
        /// <param name="objEventTicketCategory">The object CMS configuration.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateEventTicketCategory(EventTicketCategoryModel objEventTicketCategory)
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
