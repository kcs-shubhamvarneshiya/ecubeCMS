// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="RoleController.cs" company="string.Empty">
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
    /// Class RoleController.
    /// </summary>
    public class RoleController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Zero-based index of the role.
        /// </summary>
        private readonly IRoleCommand objIRoleCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="iRoleCommand">The i role command.</param>
        public RoleController(IRoleCommand iRoleCommand)
        {
            this.objIRoleCommand = iRoleCommand;
        }

        /// <summary>
        /// Roles the view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult RoleView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
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
        /// Roles this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Role()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                RoleModel objRole = new RoleModel();
                long lgRoleId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objRole.HdnIFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgRoleId = Request.QueryString.ToString().Decode().LongSafe();
                        objRole = this.objIRoleCommand.GetRoleByRoleId(lgRoleId);
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                return this.View(objRole);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }

        /// <summary>
        /// Roles the specified object role.
        /// </summary>
        /// <param name="objRole">The object role.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult Role(RoleModel objRole)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (objRole.Id == 0)
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

                if (objRole.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIRoleCommand.IsRoleExists(objRole.Id, objRole.RoleName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateRole(objRole);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objIRoleCommand.SaveRole(objRole);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.Success);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.Fail);
                        }
                    }
                }

                return this.View(objRole);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objRole);
            }
        }

        /// <summary>
        /// Binds the role grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindRoleGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchRoleResult> objRoleList = this.objIRoleCommand.SearchRole(rows, page, search, sidx + " " + sord);
                return this.FillGrid(page, rows, objRoleList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="strRoleId">The string role identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteRole(string strRoleId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strRole = strRoleId.Split(',');
                strRoleId = string.Empty;
                foreach (var item in strRole)
                {
                    strRoleId += item.Decode() + ",";
                }

                strRoleId = strRoleId.Substring(0, strRoleId.Length - 1);
                DeleteRoleResult result = this.objIRoleCommand.DeleteRole(strRoleId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Role", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Role", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Role", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("Role", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetRole()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIRoleCommand.GetAllRoleForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Binds the role permission grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="lgRoleId">The long role identifier.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindRolePermissionGrid(string sidx, string sord, int page, int rows, string filters, long lgRoleId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<GetPagePermissionResult> objRolePermissionList = this.objIRoleCommand.GerRolePermissionByRoleId(lgRoleId);
                return this.FillRollPermissionGrid(objRolePermissionList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates the role.
        /// </summary>
        /// <param name="objRole">The object role.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateRole(RoleModel objRole)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objRole.RoleName))
                {
                    strErrorMsg += Functions.AlertMessage("Role Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objRole.Rights))
                {
                    strErrorMsg += "No Rights Are Selected." + "<br/>";
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

        /// <summary>
        /// Fills the grid.
        /// </summary>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="objRoleList">The object role list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGrid(int page, int rows, List<SearchRoleResult> objRoleList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objRoleList != null && objRoleList.Count > 0 ? objRoleList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedRoleCol = objRoleList;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objRole in pagedRoleCol
                            select new
                            {
                                id = objRole.Id.ToString().Encode(),
                                RoleName = objRole.RoleName,
                                Description = objRole.Description
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
        /// Fills the roll permission grid.
        /// </summary>
        /// <param name="objRolePermissionList">The object role permission list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillRollPermissionGrid(List<GetPagePermissionResult> objRolePermissionList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = objRolePermissionList.Count;
                int totalRecords = objRolePermissionList != null ? objRolePermissionList.Count : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page = 1,
                    records = totalRecords,
                    rows = (from objRole in objRolePermissionList
                            select new
                            {
                                Id = objRole.Id,
                                PageId = objRole.PageId,
                                RoleId = objRole.RoleId,
                                ModuleName = objRole.ModuleName,
                                PageName = objRole.PageName,
                                DispalyName = objRole.DispalyName,
                                View_Right = objRole.View_Right,
                                Add_Right = objRole.Add_Right,
                                Edit_Right = objRole.Edit_Right,
                                Delete_Right = objRole.Delete_Right,
                                Export_Right = objRole.Export_Right
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