// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="HomeController.cs" company="string.Empty">
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
    /// Class HomeController.
    /// </summary>
    public class HomeController : Controller
    {
        public readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// Zero-based index of the user.
        /// </summary>
        private readonly IUserCommand objIUserCommand = null;

        /// <summary>
        /// Zero-based index of the user log.
        /// </summary>
        private readonly IUserLogCommand objIUserLogCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="iUserCommand">The i user command.</param>
        /// <param name="iUserLogCommand">The i user log command.</param>
        public HomeController(IUserCommand iUserCommand, IUserLogCommand iUserLogCommand)
        {
            this.objIUserCommand = iUserCommand;
            this.objIUserLogCommand = iUserLogCommand;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Index()
        {
            UserModel objLogin = new UserModel();
            objLogin.ClubName = Functions.GetSettings("ClubName2");

            return this.View(objLogin);
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Login()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (Functions.GetSettings("ActiveIPRestriction") == "1")
                {
                    if (!Functions.IsIpAddressValid())
                    {
                        return this.RedirectToAction("UserUnAuthorize");
                    }
                }

                UserModel objLogin = new UserModel();
                if (Functions.GetRememberMe("rememberme") == "true")
                {
                    objLogin.UserName = Functions.GetRememberMe("username");
                    objLogin.Password = Functions.GetRememberMe("password");
                    ViewBag.password = Functions.GetRememberMe("password");
                    objLogin.RememberMe = Convert.ToBoolean(Functions.GetRememberMe("rememberme"));
                }

                objLogin.ClubName = Functions.GetSettings("ClubName2");

                return this.View(objLogin);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Logins the specified object login.
        /// </summary>
        /// <param name="objLogin">The object login.</param>
        /// <returns>Action  Result.</returns>
        [HttpPost]
        public ActionResult Login(UserModel objLogin)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserModel objUser = this.objIUserCommand.CRMUserLogin(objLogin.UserName, objLogin.Password);
                if (objUser != null)
                {


                    Functions.UpdateCookies(objUser.UserName, objUser.Password, objUser.Id.ToString(), objUser.FirstName + " " + objUser.SurName, objLogin.RememberMe.ToString(), objUser.RoleId.ToString());

                    UserLogModel objhistory = new UserLogModel();
                    objhistory.UserId = objUser.Id;
                    objhistory.PageId = PageMaster.UserLog;
                    objhistory.Action = "Login";
                    objhistory.IPAddress = Functions.GetIPAddress();
                    objhistory.AccessType = Functions.GetAccessType();
                    objhistory.Location = Functions.GetLocation();
                    objhistory.AccessOn = Functions.GetAccessOn();
                    long ret = this.objIUserLogCommand.SaveLoginHistory(objhistory);
                    if (ret > 0)
                    {
                        UserModel objCurrentUserDetails = new UserModel();
                        objCurrentUserDetails = this.objIUserCommand.GetUserByUserId(objUser.Id);
                        objUser.IsLogin = true;
                        objUser.Password = objLogin.Password;
                        objUser.FirstName = string.IsNullOrEmpty(objUser.FirstName) ? objCurrentUserDetails.FirstName : objUser.FirstName;
                        objUser.SurName = string.IsNullOrEmpty(objUser.SurName) ? objCurrentUserDetails.SurName : objUser.SurName;
                        objUser.MobileNo = string.IsNullOrEmpty(objUser.MobileNo) ? objCurrentUserDetails.MobileNo : objUser.MobileNo;
                        objUser.EmailID = string.IsNullOrEmpty(objUser.EmailID) ? objCurrentUserDetails.EmailID : objUser.EmailID;
                        objUser.Address = string.IsNullOrEmpty(objUser.Address) ? objCurrentUserDetails.Address : objUser.Address;


                        long retLogin = this.objIUserCommand.SaveUser(objUser);

                        ClientInfo client = new ClientInfo();

                        // If there are clients in Application
                        if (HttpContext.Application["ClientInfo"] != null)
                        {
                            List<ClientInfo> clientList = (List<ClientInfo>)HttpContext.Application["ClientInfo"];
                            client.ClientID = this.Session.SessionID;
                            client.ActiveTime = DateTime.Now;
                            client.RefreshTime = DateTime.Now;
                            client.UserID = objUser.Id;
                            if (ClientInfo.GetClinetInfoByClientID(clientList, this.Session.SessionID).UserID == 0)
                            {
                                clientList.Add(client);
                                HttpContext.Application["ClientInfo"] = clientList;
                            }
                        }
                        else
                        {
                            List<ClientInfo> clientList = new List<ClientInfo>();
                            client.ClientID = this.Session.SessionID;
                            client.ActiveTime = DateTime.Now;
                            client.RefreshTime = DateTime.Now;
                            client.UserID = objUser.Id;
                            clientList.Add(client);
                            HttpContext.Application["ClientInfo"] = clientList;
                        }
                    }

                    return this.RedirectToAction("Index", "Home");
                }

                return this.View(objLogin);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View(objLogin);
            }
        }

        /// <summary>
        /// Validates the login.
        /// </summary>
        /// <param name="objLogin">The object login.</param>
        /// <returns>JSON  Result.</returns>
        [HttpPost]
        public JsonResult ValidateLogin(UserModel objLogin)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserModel objUser = this.objIUserCommand.CRMUserLogin(objLogin.UserName, objLogin.Password);
                if (objUser != null && objUser.Id > 0)
                {
                    return this.Json(objUser.EmailID);
                }

                return this.Json("2222");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json("1111");
            }
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult Logout()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserLogModel objUserLog = new UserLogModel();
                objUserLog.UserId = MySession.Current.UserId;
                objUserLog.PageId = PageMaster.UserLog;
                objUserLog.Action = "Logout";
                objUserLog.IPAddress = Functions.GetIPAddress();
                objUserLog.AccessType = Functions.GetAccessType();
                objUserLog.Location = Functions.GetLocation();
                objUserLog.AccessOn = Functions.GetAccessOn();
                long lgUserId = this.objIUserLogCommand.SaveLoginHistory(objUserLog);
                UserModel objUser = new UserModel();
                if (lgUserId >= 0)
                {
                    objUser = this.objIUserCommand.GetUserByUserId(objUserLog.UserId);
                    if (objUser != null)
                    {
                        objUser.IsLogin = false;
                        this.objIUserCommand.SaveUser(objUser);
                    }

                    List<ClientInfo> clientList = (List<ClientInfo>)HttpContext.Application["ClientInfo"];
                    if (clientList != null)
                    {
                        ClientInfo client = ClientInfo.GetClinetInfoByClientID(clientList, this.Session.SessionID);
                        clientList.Remove(client);
                    }
                }

                Functions.LogoutUser();
                ViewData.Clear();
                TempData.Clear();
                return this.RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="strCurrentPwd">The string current password.</param>
        /// <param name="strNewPwd">The string new password.</param>
        /// <returns>Action  Result.</returns>
        public ActionResult ChangePassword(string strCurrentPwd, string strNewPwd)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (MySession.Current.Password == Extension.EncryptString(strCurrentPwd))
                {
                    this.objIUserCommand.ChangePassword(MySession.Current.UserId, strNewPwd);
                    Functions.UpdateCookies(MySession.Current.UserName, Extension.EncryptString(strNewPwd), MySession.Current.UserId.ToString(), MySession.Current.Fullname, MySession.Current.Rememberme, MySession.Current.RoleId.ToString());
                    return this.Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("CurrentWrong", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Forgot the password.
        /// </summary>
        /// <param name="objLogin">The object login.</param>
        /// <returns>JSON  Result.</returns>
        [HttpPost]
        public JsonResult ForgotPassword(UserModel objLogin)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                UserModel objUser = this.objIUserCommand.GetUserByEmailId(objLogin.EmailID);
                if (objUser == null || objUser.Id == 0)
                {
                    return this.Json("1111");
                }

                string strMessage = Functions.GetSettings("ForgotPasswordTemplate");
                strMessage = strMessage.Replace("##ApplicationName##", Functions.GetSettings("ApplicationName"));
                strMessage = strMessage.Replace("##FullName##", objUser.FirstName + " " + objUser.SurName);
                strMessage = strMessage.Replace("##Password##", objUser.Password.DecryptString());
                if (EmailService.SendEmail(objUser.EmailID, "Forgot Password", strMessage))
                {
                    EmailService.SaveEmailLog(objUser.Id, 1, strMessage, objUser.EmailID, string.Empty, string.Empty, MySession.Current.UserId, PageMaster.Common);
                    return this.Json("2222");
                }
                else
                {
                    return this.Json("We Are Getting Some Problem While Sending Email. Please Contact Administrator.");
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.Json("We Are Getting Some Problem While Sending Email. Please Contact Administrator.");
            }
        }

        /// <summary>
        /// Permissions the redirect page.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult PermissionRedirectPage()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                if (MySession.Current.UserId == 0)
                {
                    return this.RedirectToAction("Login");
                }

                return this.View();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Users the un authorize.
        /// </summary>
        /// <returns>Action  Result.</returns>
        public ActionResult UserUnAuthorize()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + this.GetType().Name + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                return this.View();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return this.View();
            }
        }
    }
}
