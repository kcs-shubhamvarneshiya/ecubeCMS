// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-19-2016
// ***********************************************************************
// <copyright file="UserController.cs" company="string.Empty">
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
    /// Class UserController.
    /// </summary>
    public class UserController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Zero-based index of the user.
        /// </summary>
        private readonly IUserCommand objIUserCommand = null;

        /// <summary>
        /// Zero-based index of the role.
        /// </summary>
        private readonly IRoleCommand objIRoleCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="iUserCommand">The i user command.</param>
        /// <param name="iRoleCommand">The i role command.</param>
        public UserController(IUserCommand iUserCommand, IRoleCommand iRoleCommand)
        {
            this.objIUserCommand = iUserCommand;
            this.objIRoleCommand = iRoleCommand;
        }

        /// <summary>
        /// Users the view.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult UserView()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
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
        /// Users this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult User()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                UserModel objUser = new UserModel();
                long lgUserId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objUser.HdnIFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgUserId = Request.QueryString.ToString().Decode().LongSafe();
                        objUser = this.objIUserCommand.GetUserByUserId(lgUserId);
                        ViewBag.Password = objUser.Password;
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                this.BindDropDownListForUser(objUser, true);
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
        /// Users the specified object user.
        /// </summary>
        /// <param name="objUser">The object user.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult User(UserModel objUser)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objUser.Id == 0)
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

                if (objUser.HdnIFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objIUserCommand.IsUserExists(objUser.Id, objUser.UserName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateUser(objUser);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objIUserCommand.SaveUser(objUser);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Success);
                            this.BindDropDownListForUser(objUser, false);
                            return this.View(objUser);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Fail);
                        }
                    }
                }

                this.BindDropDownListForUser(objUser, true);
                return this.View(objUser);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Fail);
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objUser);
            }
        }

        /// <summary>
        /// Binds the user grid.
        /// </summary>
        /// <param name="sidx">Parameter SIDX.</param>
        /// <param name="sord">Parameter SORD.</param>
        /// <param name="page">Parameter page.</param>
        /// <param name="rows">Parameter rows.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="search">The search.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult BindUserGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                List<SearchUserResult> objUserList = this.objIUserCommand.SearchUser(rows, page, search, sidx + " " + sord);
                return this.FillGrid(page, rows, objUserList);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty);
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="strUserId">The string user identifier.</param>
        /// <returns>JSON  Result.</returns>
        public JsonResult DeleteUser(string strUserId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string[] strUser = strUserId.Split(',');
                strUserId = string.Empty;
                foreach (var item in strUser)
                {
                    strUserId += item.Decode() + ",";
                }

                strUserId = strUserId.Substring(0, strUserId.Length - 1);
                DeleteUserResult result = this.objIUserCommand.DeleteUser(strUserId, MySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>JSON  Result.</returns>
        public JsonResult GetUser()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.Json(this.objIUserCommand.GetAllUserForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Binds the drop down list for user.
        /// </summary>
        /// <param name="objUser">The object user.</param>
        /// <param name="blBindDropDownFromDb">IF set to <c>true</c> [BI bind drop down from database].</param>
        public void BindDropDownListForUser(UserModel objUser, bool blBindDropDownFromDb)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (blBindDropDownFromDb)
                {
                    objUser.RoleList = this.objIRoleCommand.GetAllRoleForDropDown().ToList();
                }
                else
                {
                    objUser.RoleList = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="objUser">The object user.</param>
        /// <returns>Return System.String.</returns>
        private string ValidateUser(UserModel objUser)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objUser.FirstName))
                {
                    strErrorMsg += Functions.AlertMessage("First Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.SurName))
                {
                    strErrorMsg += Functions.AlertMessage("Surname", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.MobileNo))
                {
                    strErrorMsg += Functions.AlertMessage("Mobile No", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.EmailID))
                {
                    strErrorMsg += Functions.AlertMessage("Email Id", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.UserName))
                {
                    strErrorMsg += Functions.AlertMessage("User Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.Password))
                {
                    strErrorMsg += Functions.AlertMessage("Password", MessageType.InputRequired) + "<br/>";
                }

                if (objUser.RoleId == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Role", MessageType.SelectRequired) + "<br/>";
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
        /// <param name="objUserList">The object user list.</param>
        /// <returns>Action  Result.</returns>
        private ActionResult FillGrid(int page, int rows, List<SearchUserResult> objUserList)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                int pageSize = rows;
                int totalRecords = objUserList != null && objUserList.Count > 0 ? objUserList[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedUserCol = objUserList;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objUser in pagedUserCol
                            select new
                            {
                                id = objUser.Id.ToString().Encode(),
                                FirstName = objUser.FirstName,
                                SurName = objUser.SurName,
                                MobileNo = objUser.MobileNo,
                                EmailID = objUser.EmailID,
                                UserName = objUser.UserName,
                                Address = objUser.Address,
                                RoleName = objUser.RoleName,
                                IsActive = objUser.IsActive ? "Active" : "Inactive"
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