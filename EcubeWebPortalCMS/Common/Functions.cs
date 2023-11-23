// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="Functions.cs" company="string.Empty">
//     Copyright �  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace EcubeWebPortalCMS.Common
{
    using EcubeWebPortalCMS.Models;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Class Functions.
    /// </summary>
    public static class Functions
    {
        public static readonly SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LogConnectionString"].ConnectionString);

        /// <summary>
        /// The date format.
        /// </summary>
        public static readonly string DateFormat = "dd/MM/yyyy", DateTimeFormat = "dd/MM/yyyy hh:mm tt", StrFail = "Fail", strSuccess = "Success", StrUnAuthorise = "You Are Not Authorized";

        /// <summary>
        /// The allowed file extensions.
        /// </summary>
        public static readonly string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpeg" };

        public static readonly string[] AllowedFileExtensionsForAttachment = new string[] { ".pdf", ".xlsx", ".docx", ".jpg", ".png", ".jpeg" };

        /// <summary>
        /// The allowed file extensions for PDF.
        /// </summary>
        public static readonly string[] AllowedFileExtensionsForPDF = new string[] { ".pdf" };

        /// <summary>
        /// The cookie name.
        /// </summary>
        private static readonly string CookieName = "kcsremuser";

        public static readonly string Phrase = "abcdefghijklmnopqrstuvwxyz1234567890";

        /// <summary>
        /// Checks the page permission.
        /// </summary>
        /// <param name="lgPageId">The long page identifier.</param>
        /// <returns>Get Page Permission  Result.</returns>
        public static GetPagePermissionResult CheckPagePermission(long lgPageId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                CommonDataContext objCommon = new CommonDataContext();
                GetPagePermissionResult objRights = objCommon.GetPagePermission(lgPageId, MySession.Current.UserId, MySession.Current.RoleId).FirstOrDefault();
                if (objRights == null)
                {
                    objRights = new GetPagePermissionResult();
                }

                return objRights;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return new GetPagePermissionResult();
            }
        }

        /// <summary>
        /// Alerts the message.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="msgType">Type of the MSG.</param>
        /// <param name="message">The message.</param>
        /// <returns>The System.String.</returns>
        public static string AlertMessage(string tableName, MessageType msgType, string message = "")
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (msgType == MessageType.Success)
                {
                    return tableName + " Submitted Successfully.";
                }
                else if (msgType == MessageType.Fail)
                {
                    return tableName + " Not Submitted Successfully.";
                }
                else if (msgType == MessageType.DeleteSucess)
                {
                    return tableName + "(s) Deleted Successfully.";
                }
                else if (msgType == MessageType.DeleteFail)
                {
                    return tableName + "(s) Delete Failure.";
                }
                else if (msgType == MessageType.DeletePartial)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        return "Following " + tableName + "(s) Can Not Be Deleted Due To Reference.<br/>" + message;
                    }
                    else
                    {
                        return "Some " + tableName + "(s) Can Not Be Deleted Due To Reference.";
                    }
                }
                else if (msgType == MessageType.AlreadyExist)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        return message + " Already Exists.";
                    }
                    else
                    {
                        return tableName + " Already Exists.";
                    }
                }
                else if (msgType == MessageType.InputRequired)
                {
                    return "Please Enter " + tableName + ".";
                }
                else if (msgType == MessageType.SelectRequired)
                {
                    return "Please Select " + tableName + ".";
                }
                else if (msgType == MessageType.UpdateSuccess)
                {
                    return tableName + " updated Successfully.";
                }
                else if (msgType == MessageType.DuplicateRows)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        return message + " has duplicates rows.";
                    }
                    else
                    {
                        return tableName + " has duplicates rows.";
                    }
                }
                else if (msgType == MessageType.DeleteMenuSubMenu)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        return "Following " + tableName + "(s) Can Not Be Deleted Due To Reference.<br/>" + message;
                    }
                    else
                    {
                        return "" + tableName + " which has multiple Sub Menus cannot be deleted from here. You can delete it from Edit Menu.";
                    }
                }
                else
                {
                    return message;
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
        /// Updates the cookies.
        /// </summary>
        /// <param name="strUserName">Name of the string user.</param>
        /// <param name="strPassword">The string password.</param>
        /// <param name="strUserId">The string user identifier.</param>
        /// <param name="strFullName">Full name of the string.</param>
        /// <param name="strRemember">The string remember.</param>
        /// <param name="strRoleId">The string role identifier.</param>
        public static void UpdateCookies(string strUserName, string strPassword, string strUserId, string strFullName, string strRemember, string strRoleId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                HttpCookie hcUser = new HttpCookie(MySession.Current.CookiesName);
                hcUser.HttpOnly = true;
                hcUser.Values["username"] = strUserName;
                if (string.IsNullOrEmpty(strPassword))
                {
                    strPassword = string.Empty;
                }

                hcUser.Values["password"] = strPassword;
                hcUser.Values["userid"] = strUserId;
                hcUser.Values["fullname"] = strFullName;
                hcUser.Values["rememberme"] = strRemember;
                hcUser.Values["roleid"] = strRoleId;
                hcUser.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(hcUser);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }
        }

        /// <summary>
        /// Logouts the user.
        /// </summary>
        public static void LogoutUser()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                HttpCookie hcUser = HttpContext.Current.Request.Cookies[MySession.Current.CookiesName];
                if (hcUser != null)
                {
                    hcUser.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(hcUser);
                }

                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Contents.RemoveAll();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Expires = 60;
                HttpContext.Current.Response.AddHeader("pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("cache-control", "private");
                HttpContext.Current.Response.CacheControl = "no-cache";
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }
        }

        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="strKey">The string key.</param>
        /// <returns>The System.String.</returns>
        public static string GetCookieValue(string strKey)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (HttpContext.Current.Request.Cookies[strKey] != null)
                {
                    return HttpContext.Current.Request.Cookies[strKey].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the cookie value.
        /// </summary>
        /// <param name="strKey">The string key.</param>
        /// <param name="strValue">The string value.</param>
        /// <returns>The System.String.</returns>
        public static string SetCookieValue(string strKey, string strValue)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                HttpContext.Current.Response.Cookies[strKey].Value = strValue;
                HttpContext.Current.Response.Cookies[strKey].Expires = DateTime.Now.AddMinutes(1); // add expiry time
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the remember me.
        /// </summary>
        /// <param name="strKey">The string key.</param>
        /// <returns>The System.String.</returns>
        public static string GetRememberMe(string strKey)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies[CookieName] != null)
                {
                    switch (strKey)
                    {
                        case "password":
                            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[CookieName].Values["password"]) ? null : System.Web.HttpContext.Current.Request.Cookies[CookieName].Values["password"].ToString();

                        case "username":
                            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[CookieName].Values[strKey]) ? string.Empty : System.Web.HttpContext.Current.Request.Cookies[CookieName].Values[strKey].ToString();

                        case "rememberme":
                            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[CookieName].Values[strKey]) ? null : System.Web.HttpContext.Current.Request.Cookies[CookieName].Values[strKey].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the remember me.
        /// </summary>
        /// <param name="rememberMe">IF set to <c>true</c> [remember me].</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="strUserId">The string user identifier.</param>
        /// <param name="strFullName">Full name of the string.</param>
        /// <param name="strRoleId">The string role identifier.</param>
        /// <param name="path">Parameter path.</param>
        public static void SetRememberMe(bool rememberMe, string userName, string password, string strUserId, string strFullName, string strRoleId, string path)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (rememberMe)
                {
                    HttpCookie hcUser = new HttpCookie(CookieName);
                    hcUser.Values["rememberme"] = "true";
                    hcUser.Values["username"] = userName;
                    hcUser.Values["password"] = password;
                    hcUser.Values["userid"] = strUserId;
                    hcUser.Values["fullname"] = strFullName;
                    hcUser.Values["roleid"] = strRoleId;
                    hcUser.Values["DirectoryPath"] = path;
                    hcUser.Expires = DateTime.Now.AddDays(30);
                    System.Web.HttpContext.Current.Response.Cookies.Add(hcUser);
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(CookieName))
                    {
                        HttpCookie hcAccount = System.Web.HttpContext.Current.Request.Cookies[CookieName];
                        hcAccount.Expires = DateTime.Now.AddDays(-1);
                        System.Web.HttpContext.Current.Response.Cookies.Add(hcAccount);
                    }
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
        /// Creates the root directory.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="path">The  path.</param>
        /// <returns>The System.String.</returns>
        public static string CreateRootDirectory(string pageName, string path)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                CommonDataContext objDataContext = new CommonDataContext();
                AAAConfigSetting objSetting = objDataContext.AAAConfigSettings.Where(x => x.KeyName == "DocRootFolderPath").FirstOrDefault();
                if (objSetting != null)
                {
                    path = objSetting.KeyValue + path;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                return path;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Gets the root directory.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <returns>The System.String.</returns>
        public static string GetRootDirectory(string pageName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                CommonDataContext objDataContext = new CommonDataContext();
                string strDocPath = objDataContext.AAAConfigSettings.Where(a => a.KeyName == "DocRootFolderPath").FirstOrDefault().KeyValue.ToString();
                string strKeyValue = objDataContext.AAAConfigSettings.Where(a => a.KeyName == pageName).FirstOrDefault().KeyValue.ToString();
                return strDocPath;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return null;
            }
        }

        /// <summary>
        /// Determines whether [is IP  address valid].
        /// </summary>
        /// <returns><c>true</c> IF [is IP  address valid]; otherwise, <c>false</c>.</returns>
        public static bool IsIpAddressValid()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                IPAddressDataContext objCommon = new IPAddressDataContext();
                IPAddress objIPAddress = objCommon.IPAddresses.Where(x => x.IPAddressName == GetIPAddress() && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();

                if (objIPAddress != null)
                {
                    return objIPAddress.IsActive;
                }

                return false;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Gets the IP  address.
        /// </summary>
        /// <returns>The System.String.</returns>
        public static string GetIPAddress()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strIpAddress;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                }
                else
                {
                    strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return strIpAddress;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the access on.
        /// </summary>
        /// <returns>The System.String.</returns>
        public static string GetAccessOn()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strAccessOn;
                if (HttpContext.Current.Request.UserAgent.ToLower().Contains("android"))
                {
                    strAccessOn = "android";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
                {
                    strAccessOn = "ipad";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("macintosh"))
                {
                    strAccessOn = "macintosh";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("blackberry"))
                {
                    strAccessOn = "blackberry";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone"))
                {
                    strAccessOn = "iphone";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("symbianos"))
                {
                    strAccessOn = "symbianos";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("j2me"))
                {
                    strAccessOn = "j2me";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("palmos"))
                {
                    strAccessOn = "palmos";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("bada"))
                {
                    strAccessOn = "bada";
                }
                else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("rim"))
                {
                    strAccessOn = "rim";
                }
                else
                {
                    strAccessOn = "Web";
                }

                return strAccessOn;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the type of the access.
        /// </summary>
        /// <returns>The System.String.</returns>
        public static string GetAccessType()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strAccessType;
                if (HttpContext.Current.Request.UserAgent.ToLower().Contains("android") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("macintosh") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("blackberry") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("symbianos") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("j2me") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("palmos") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("bada") ||
                    HttpContext.Current.Request.UserAgent.ToLower().Contains("rim"))
                {
                    strAccessType = "Mobile";
                }
                else
                {
                    strAccessType = "Browser";
                }

                return strAccessType;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <returns>The System.String.</returns>
        public static string GetLocation()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string strIpAddress = GetIPAddress();
                if (strIpAddress.Length > 3)
                {
                    string strxml = "&lt;root>abc</root>";
                    string strExterUrl = GetSettings("GetIP2CountryUrl");
                    string strurl = strExterUrl + "&ip=" + strIpAddress;
                    System.Net.HttpWebRequest hwrreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strurl);
                    byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(strxml);
                    hwrreq.Method = "POST";
                    hwrreq.ContentType = "text/xml;charset=utf-8";
                    hwrreq.ContentLength = requestBytes.Length;
                    Stream requestStream = hwrreq.GetRequestStream();
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();
                    System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)hwrreq.GetResponse();
                    StreamReader steamreader = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                    string strBrowse = steamreader.ReadToEnd();
                    steamreader.Close();
                    res.Close();
                    char[] chSep = new char[] { ';' };
                    string[] browserlist = strBrowse.Split(chSep);
                    return browserlist[4].ToString();
                }

                return "Local System";
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return "Local System";
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>The System.String.</returns>
        public static string GetSettings(string keyName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                CommonDataContext objCommon = new CommonDataContext();
                AAAConfigSetting objSetting = objCommon.AAAConfigSettings.Where(x => x.KeyName == keyName).FirstOrDefault();
                if (objSetting != null)
                {
                    return objSetting.KeyValue;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        public static GetPropertyInfoResult GetPropertyInfo()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            GetPropertyInfoResult getProperties = new GetPropertyInfoResult();

            try
            {
                using (CommonDataContext commonData = new CommonDataContext())
                {
                    getProperties = commonData.GetPropertyInfo().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            if (getProperties == null || string.IsNullOrEmpty(getProperties.Name))
            {
                getProperties = new GetPropertyInfoResult();
                getProperties.Name = string.Empty;
            }

            return getProperties;
        }

        /// <summary>
        /// Writes the specified ex.
        /// </summary>
        /// <param name="ex">Parameter ex.</param>
        /// <param name="strProcedureName">Name of the string procedure.</param>
        /// <param name="lgPageId">The long page identifier.</param>
        /// <param name="lgUserId">The long user identifier.</param>
        public static void Write(Exception ex, string strProcedureName, long lgPageId, long lgUserId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                InsertErrorLog(ex, strProcedureName, lgPageId, lgUserId);
            }
            catch (Exception e)
            {
                information += " - " + e.Message;
                logger.Error(e, information);

            }
        }

        /// <summary>
        /// Writes the specified ex.
        /// </summary>
        /// <param name="ex">Parameter ex.</param>
        /// <param name="strprocedureName">Name of the store procedure.</param>
        /// <param name="lgPageId">The long page identifier.</param>
        public static void Write(Exception ex, string strprocedureName, long lgPageId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                InsertErrorLog(ex, strprocedureName, lgPageId, 1);
            }
            catch (Exception e)
            {
                information += " - " + e.Message;
                logger.Error(e, information);

            }
        }

        /// <summary>
        /// Gets the status for web service response.
        /// </summary>
        /// <param name="strStatus">The string status.</param>
        /// <param name="strErrorText">The string error text.</param>
        /// <param name="strMethodName">Name of the string method.</param>
        /// <returns>The System.String.</returns>
        public static string GetStatusForWebServiceResponse(string strStatus, string strErrorText, string strMethodName)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string status = "\"Status\": \"" + strStatus + "\", \"ErrorText\": \"" + strErrorText + "\", \"" + strMethodName + "Result\": ";
                return status;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return string.Empty;
            }
        }

        /// <summary>
        /// Validates the web service access.
        /// </summary>
        /// <param name="strPassword">The string password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ValidateWebServiceAccess(string strPassword)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                if (strPassword.Replace(" ", "+") == GetSettings("ApplicationName").EncryptString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }
        }

        /// <summary>
        /// Gets the file header hexadecimal code.
        /// </summary>
        /// <param name="fileExt">The file ext.</param>
        /// <returns>The System.String.</returns>
        public static string GetFileHeaderHexCode(string fileExt)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add(".bmp", "424D");
                dictionary.Add(".gif", "47494638");
                dictionary.Add(".jpeg", "FFD8FF");
                dictionary.Add(".jpg", "FFD8FF");
                dictionary.Add(".png", "89504E470D0A1A0A");
                dictionary.Add(".tif", "492049");
                dictionary.Add(".tiff", "492049");
                return dictionary[fileExt];
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Validates the uploaded file header.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="extensions">The extensions.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ValidateUploadedFileHeader(string fileName, byte[] bytes, string[] extensions)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            string strExtension;
            try
            {
                //string hexString = string.Empty;
                //for (int i = 0; i < bytes.Length; i++)
                //{
                //    hexString += bytes[i].ToString("X2");
                //    strExtension = Path.GetExtension(fileName).ToUpper();
                //    if (string.IsNullOrEmpty(hexString))
                //    {
                //        return false;
                //    }
                //    else
                //    {
                //        foreach (string ext in extensions)
                //        {
                //            if (strExtension == ext.ToUpper() && hexString.Contains(GetFileHeaderHexCode(ext.ToLower())))
                //            {
                //                return true;
                //            }
                //        }
                //    }
                //}

                strExtension = Path.GetExtension(fileName).ToUpper();

                foreach (string ext in extensions)
                {
                    if (strExtension == ext.ToUpper())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Checks the file extension.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="extensions">The extensions.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckFileExtension(string fileName, string[] extensions)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            string extension;
            try
            {
                extension = Path.GetExtension(fileName).ToUpper();
                foreach (string ext in extensions)
                {
                    if (extension == ext.ToUpper())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Generates the thumbnails.
        /// </summary>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        public static void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                using (var image = System.Drawing.Image.FromStream(sourcePath))
                {
                    var newWidth = (int)(image.Width * scaleFactor);
                    var newHeight = (int)(image.Height * scaleFactor);
                    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbGraph.DrawImage(image, imageRectangle);
                    thumbnailImg.Save(targetPath, image.RawFormat);
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);

            }
        }

        /// <summary>
        /// Converts the date time.
        /// </summary>
        /// <param name="date">The date parameter.</param>
        /// <returns>DateTime convert date time.</returns>
        public static DateTime ConvertDateTime(string date)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                string[] dateArray = null;
                dateArray = date.Contains("_") == false ? dateArray = date.Split('/') : dateArray = null;
               
                date = dateArray != null ? dateArray[1] + "/" + GetMonthName(Convert.ToInt32(dateArray[0])) + "/" + dateArray[2] : null;
               
                return Convert.ToDateTime(date);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Gets the name of the month.
        /// </summary>
        /// <param name="month">The month parameter.</param>
        /// <returns>System.String get month name.</returns>
        public static string GetMonthName(int month)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                switch (month)
                {
                    case 1:
                        return "Jan";

                    case 2:
                        return "Feb";

                    case 3:
                        return "Mar";

                    case 4:
                        return "Apr";

                    case 5:
                        return "May";

                    case 6:
                        return "June";

                    case 7:
                        return "Jul";

                    case 8:
                        return "Aug";

                    case 9:
                        return "Sep";

                    case 10:
                        return "Oct";

                    case 11:
                        return "Nov";

                    case 12:
                        return "Dec";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Smarts the resize.
        /// </summary>
        /// <param name="strImageFile">The string image file.</param>
        /// <param name="objMaxSize">Maximum size of the object.</param>
        /// <param name="enuType">Type of the ENU.</param>
        /// <returns>Bit map parameter.</returns>
        public static Bitmap SmartResize(string strImageFile, Size objMaxSize, ImageFormat enuType)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            Bitmap objImage = null;
            try
            {
                objImage = new Bitmap(strImageFile);
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw ex;
            }

            if (objImage.Width > objMaxSize.Width || objImage.Height > objMaxSize.Height)
            {
                Size objSize;
                int intWidthOverrun = 0;
                int intHeightOverrun = 0;
                if (objImage.Width > objMaxSize.Width)
                {
                    intWidthOverrun = objImage.Width - objMaxSize.Width;
                }

                if (objImage.Height > objMaxSize.Height)
                {
                    intHeightOverrun = objImage.Height - objMaxSize.Height;
                }

                double dblRatio;
                double dblWidthRatio = (double)objMaxSize.Width / (double)objImage.Width;
                double dblHeightRatio = (double)objMaxSize.Height / (double)objImage.Height;
                if (dblWidthRatio < dblHeightRatio)
                {
                    dblRatio = dblWidthRatio;
                }
                else
                {
                    dblRatio = dblHeightRatio;
                }

                objSize = new Size((int)((double)objImage.Width * dblWidthRatio), (int)((double)objImage.Height * dblHeightRatio));
                Bitmap objNewImage = Resize(objImage, objSize, enuType);
                objImage.Dispose();
                return objNewImage;
            }
            else
            {
                return objImage;
            }
        }

        /// <summary>
        /// Resizes the specified IMG photo.
        /// </summary>
        /// <param name="imgPhoto">The IMG photo.</param>
        /// <param name="objSize">Size of the object.</param>
        /// <param name="enuType">Type of the ENU.</param>
        /// <returns>Bit map parameter.</returns>
        public static Bitmap Resize(Bitmap imgPhoto, Size objSize, ImageFormat enuType)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();
            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;

                int destX = 0;
                int destY = 0;
                int destWidth = objSize.Width;
                int destHeight = objSize.Height;

                Bitmap bmphoto;
                if (enuType == ImageFormat.Png)
                {
                    bmphoto = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppArgb);
                }
                else if (enuType == ImageFormat.Gif)
                {
                    bmphoto = new Bitmap(destWidth, destHeight); ////PixelFormat.Format8bppIndexed should be the right value for a GIF, but will throw an error with some GIF images so it's not safe to specify.
                }
                else
                {
                    bmphoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
                }

                ////For some reason the resolution properties will be 96, even when the source image is different, so this matching does not appear to be reliable.
                ////bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                ////If you want to override the default 96dpi resolution do it here
                ////bmPhoto.SetResolution(72, 72);

                Graphics grphoto = Graphics.FromImage(bmphoto);
                grphoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grphoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
                grphoto.Dispose();
                return bmphoto;

            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }

        /// <summary>
        /// Res the size banner.
        /// </summary>
        /// <param name="imagePath">The image path Parameter.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderId">The folder identifier Parameter.</param>
        /// <param name="module">The module Parameter.</param>
        /// <param name="type">The type Parameter.</param>
        public static void ReSizeBanner(string imagePath, string fileName, string folderId, string module, string type = "Banner")
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            fileName = fileName.Replace(" ", string.Empty).Replace("_", string.Empty);
            if (!File.Exists(imagePath + fileName))
            {
                return;
            }

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            List<ImageSize> imageSize = GetImageSize();
            if (module.ToLower() == "information")
            {
                try
                {
                    string newPath = imagePath + "\\" + module;

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }

                    if (System.IO.File.Exists(imagePath + "\\" + fileName))
                    {
                        if (!System.IO.File.Exists(newPath + "\\" + folderId + ".jpg"))
                        {
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\" + folderId + ".jpg");
                        }
                        else
                        {
                            System.IO.File.Delete(newPath + "\\" + folderId + ".jpg");
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\" + folderId + ".jpg");
                        }
                    }
                }
                catch (Exception ex)
                {
                    information += " - " + ex.Message;
                    logger.Error(ex, information);
                }
            }
            else if (imageSize != null && imageSize.Count > 0)
            {
                foreach (var item in imageSize)
                {
                    System.Drawing.Size objMaxSize = new Size();
                    System.Drawing.Bitmap objNewImage;
                    string extenstion = string.Empty;
                    if (type == "Banner")
                    {
                        objMaxSize = new System.Drawing.Size(item.ImageWidth, item.ImageHeight);
                        objNewImage = Functions.SmartResize(imagePath + "\\" + fileName, objMaxSize, System.Drawing.Imaging.ImageFormat.Jpeg);
                        extenstion = ".jpg";
                    }
                    else if (type == "Icon")
                    {
                        objMaxSize = new System.Drawing.Size(item.IconWidth.Value, item.IconHeight.Value);
                        objNewImage = Functions.SmartResize(imagePath + "\\" + fileName, objMaxSize, fileName.ToLower().EndsWith(".png") ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg);
                        extenstion = fileName.ToLower().EndsWith(".png") ? ".png" : ".jpg";
                    }
                    else
                    {
                        objMaxSize = new System.Drawing.Size(item.ImageWidth, item.ImageHeight);
                        objNewImage = Functions.SmartResize(imagePath + "\\" + fileName, objMaxSize, System.Drawing.Imaging.ImageFormat.Jpeg);
                        extenstion = ".jpg";
                    }

                    //// Re Image Path
                    string newPath = string.Empty;
                    if (folderId.Contains("_"))
                    {
                        string[] listObj = folderId.Split('_');
                        newPath = imagePath + "\\" + module + "\\" + listObj[0] + "\\" + listObj[1];
                    }
                    else if (type.ToLower() == "icon")
                    {
                        newPath = imagePath + "\\" + module + "\\" + folderId + "\\Icon";
                    }
                    else
                    {
                        newPath = imagePath + "\\" + module + "\\" + folderId;
                    }

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }

                    if (type == "Banner" && module == "Facility")
                    {
                        objNewImage.Save(newPath + "\\Banner" + extenstion, System.Drawing.Imaging.ImageFormat.Jpeg);
                        objNewImage.Dispose();
                    }
                    else
                    {
                        objNewImage.Save(newPath + "\\" + item.ImageName + extenstion, extenstion == ".png" ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg);
                        objNewImage.Dispose();
                    }

                    if (System.IO.File.Exists(imagePath + "\\" + fileName))
                    {
                        if (!System.IO.File.Exists(newPath + "\\Original" + extenstion))
                        {
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\Original" + extenstion);
                        }
                        else
                        {
                            System.IO.File.Delete(newPath + "\\Original" + extenstion);
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\Original" + extenstion);
                        }
                    }
                }
            }

            if (module.ToLower() != "post" && module.ToLower()!= "mobilebanner")
            {
                System.IO.File.Delete(imagePath + "\\" + fileName);
            }
        }

        /// <summary>
        /// Gets the notification all.
        /// </summary>
        /// <returns>List NotificationModel.</returns>
        public static List<ImageSize> GetImageSize()
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                CMSConfigDataContext objDataContext = new CMSConfigDataContext();
                using (objDataContext = new CMSConfigDataContext())
                {
                    return objDataContext.ImageSizes.ToList();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                return new List<ImageSize>();
            }
        }

        /// <summary>
        /// Inserts the error log.
        /// </summary>
        /// <param name="ex">The ex Parameter.</param>
        /// <param name="strMethodName">Name of the string method.</param>
        /// <param name="lgPageId">The page identifier Parameter.</param>
        /// <param name="lgUserId">The user identifier Parameter.</param>
        private static void InsertErrorLog(Exception ex, string strMethodName, long lgPageId, long lgUserId)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                ErrorLogDataContext objDataContext = new ErrorLogDataContext();
                objDataContext.InsertOrUpdateErrorLog("0".LongSafe(), lgPageId, strMethodName, ex.GetType().ToString(), ex.Message, ex.StackTrace == null ? string.Empty : Convert.ToString(ex.StackTrace), DateTime.Now, lgUserId, string.Empty, lgUserId, lgPageId);
            }
            catch
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
        }


        public static string EncryptCode(this string stringToEncrypt)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            byte[] results;
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();



            //// Step 1. We hash the phrase String using MD5
            //// We use the MD5 hash generator as the result is a 128 bit byte array
            //// which is a valid length for the TripleDES encoder we use below



            MD5CryptoServiceProvider hashProvider = null;
            TripleDESCryptoServiceProvider tripleAlgorithm = null;
            try
            {
                hashProvider = new MD5CryptoServiceProvider();
                byte[] tripleKey = hashProvider.ComputeHash(utf8.GetBytes(Phrase));



                //// Step 2. Create a new TripleDESCryptsServiceProvider object
                tripleAlgorithm = new TripleDESCryptoServiceProvider();



                //// Step 3. Setup the encoder
                tripleAlgorithm.Key = tripleKey;
                tripleAlgorithm.Mode = CipherMode.ECB;
                tripleAlgorithm.Padding = PaddingMode.PKCS7;



                //// Step 4. Convert the input string to a byte[]
                byte[] dataToEncrypt = utf8.GetBytes(stringToEncrypt);



                //// Step 5. Attempt to encrypt the string
                try
                {
                    ICryptoTransform encryption = tripleAlgorithm.CreateEncryptor();
                    results = encryption.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                }
                finally
                {
                    //// Clear the TripleDes and Hash provider services of any sensitive information
                    tripleAlgorithm.Clear();
                    hashProvider.Clear();
                }
            }
            catch (Exception ex)
            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
                throw;
            }
            finally
            {
                if (tripleAlgorithm != null)
                {
                    tripleAlgorithm.Dispose();
                }



                if (hashProvider != null)
                {
                    hashProvider.Dispose();
                }
            }



            //// Step 6. Return the encrypted string as a base64 encoded string
            ////return Convert.ToBase64String(results);



            return System.Web.HttpServerUtility.UrlTokenEncode(results);
        }




        public static string Decrypt(this string stringToDecrypt)
        {
            var logger = new LoggerConfiguration().WriteTo.MSSqlServer(conn.ConnectionString.ToString(), "Logs").CreateLogger();
            string information = AppConstants.ProjectName + "/" + AppConstants.Functions + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString();

            string decryptString = string.Empty;

            try

            {

                byte[] results = null;

                UTF8Encoding utf8 = new UTF8Encoding();



                //// Step 1. We hash the phraseString using MD5

                //// We use the MD5 hash generator as the result is a 128 bit byte array

                //// which is a valid length for the TripleDES encoder we use below



                MD5CryptoServiceProvider hashProvider = null;

                TripleDESCryptoServiceProvider tripleAlgorithm = null;

                try

                {

                    hashProvider = new MD5CryptoServiceProvider();

                    byte[] tripleKey = hashProvider.ComputeHash(utf8.GetBytes(Phrase));



                    //// Step 2. Create a new TripleDESCryptsServiceProvider object

                    tripleAlgorithm = new TripleDESCryptoServiceProvider();



                    //// Step 3. Setup the decoder

                    tripleAlgorithm.Key = tripleKey;

                    tripleAlgorithm.Mode = CipherMode.ECB;

                    tripleAlgorithm.Padding = PaddingMode.PKCS7;



                    //// Step 4. Convert the input string to a byte[]

                    //// Convert.FromBase64String(message);

                    byte[] dataToDecrypt = HttpServerUtility.UrlTokenDecode(stringToDecrypt);



                    //// Step 5. Attempt to decrypt the string

                    try

                    {

                        ICryptoTransform decrypter = tripleAlgorithm.CreateDecryptor();

                        results = decrypter.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);

                    }

                    catch (Exception ex)

                    {
                        information += " - " + ex.Message;
                        logger.Error(ex, information);

                    }

                    finally

                    {

                        //// Clear the TripleDes and Hash provider services of any sensitive information

                        tripleAlgorithm.Clear();

                        hashProvider.Clear();

                    }

                }

                catch (Exception ex)

                {
                    information += " - " + ex.Message;
                    logger.Error(ex, information);
                }

                finally

                {

                    if (tripleAlgorithm != null)

                    {

                        tripleAlgorithm.Dispose();

                    }



                    if (hashProvider != null)

                    {

                        hashProvider.Dispose();

                    }

                }



                //// Step 6. Return the decrypted string in UTF8 format



                if (results != null)

                {

                    decryptString = utf8.GetString(results);

                }

                else

                {

                    decryptString = string.Empty;

                }

            }

            catch (Exception ex)

            {
                information += " - " + ex.Message;
                logger.Error(ex, information);
            }

            return decryptString;

        }

        /// <summary>
        /// Res the size banner.
        /// </summary>
        /// <param name="imagePath">The image path Parameter.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folderId">The folder identifier Parameter.</param>
        /// <param name="module">The module Parameter.</param>
        /// <param name="type">The type Parameter.</param>
        public static void ReSizeImage(string imagePath, string fileName, string folderId, string module, string type)
        {
            fileName = fileName.Replace(" ", string.Empty).Replace("_", string.Empty);
            if (!File.Exists(imagePath + fileName))
            {
                return;
            }            
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            List<ImageSize> imageSize = GetImageSize();
            if (module.ToLower() == "information")
            {
                try
                {
                    string newPath = imagePath + "\\" + module;

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }

                    if (System.IO.File.Exists(imagePath + "\\" + fileName))
                    {
                        if (!System.IO.File.Exists(newPath + "\\" + folderId + ".jpg"))
                        {
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\" + folderId + ".jpg");
                        }
                        else
                        {
                            System.IO.File.Delete(newPath + "\\" + folderId + ".jpg");
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\" + folderId + ".jpg");
                        }
                    }
                }
                catch
                {
                }
            }
            else if (imageSize != null && imageSize.Count > 0)
            {
                foreach (var item in imageSize)
                {
                    System.Drawing.Size objMaxSize = new Size();
                    System.Drawing.Bitmap objNewImage;
                    string extenstion = string.Empty;
                    if (type == "Banner")
                    {
                        objMaxSize = new System.Drawing.Size(item.ImageWidth, item.ImageHeight);
                        objNewImage = Functions.SmartResize(imagePath + "\\" + fileName, objMaxSize, System.Drawing.Imaging.ImageFormat.Jpeg);
                        extenstion = ".jpg";
                    }
                    else if (type == "Icon")
                    {
                        objMaxSize = new System.Drawing.Size(item.IconWidth.Value, item.IconHeight.Value);
                        objNewImage = Functions.SmartResize(imagePath + "\\" + fileName, objMaxSize, fileName.ToLower().EndsWith(".png") ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg);
                        extenstion = fileName.ToLower().EndsWith(".png") ? ".png" : ".jpg";
                    }
                    else
                    {
                        objMaxSize = new System.Drawing.Size(item.ImageWidth, item.ImageHeight);
                        objNewImage = Functions.SmartResize(imagePath + "\\" + fileName, objMaxSize, System.Drawing.Imaging.ImageFormat.Jpeg);
                        extenstion = ".jpg";
                    }

                    //// Re Image Path
                    string newPath = string.Empty;
                    if (folderId.Contains("_"))
                    {
                        string[] listObj = folderId.Split('_');
                        newPath = imagePath + "\\" + module + "\\" + listObj[0] + "\\" + listObj[1];
                    }
                    else if (type.ToLower() == "icon")
                    {
                        newPath = imagePath + "\\" + module + "\\" + folderId + "\\Icon";
                    }
                    else
                    {
                        newPath = imagePath + "\\" + module + "\\" + folderId;
                    }

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }

                    if (type == "Banner" && module == "Facility")
                    {
                        objNewImage.Save(newPath + "\\Banner" + extenstion, System.Drawing.Imaging.ImageFormat.Jpeg);
                        objNewImage.Dispose();
                    }
                    else
                    {
                        objNewImage.Save(newPath + "\\" + item.ImageName + extenstion, extenstion == ".png" ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg);
                        objNewImage.Dispose();
                    }

                    if (System.IO.File.Exists(imagePath + "\\" + fileName))
                    {
                        if (!System.IO.File.Exists(newPath + "\\Original" + extenstion))
                        {
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\Original" + extenstion);
                        }
                        else
                        {
                            System.IO.File.Delete(newPath + "\\Original" + extenstion);
                            System.IO.File.Copy(imagePath + "\\" + fileName, newPath + "\\Original" + extenstion);
                        }
                    }
                }
            }

            if (module.ToLower() != "post")
            {
                System.IO.File.Delete(imagePath + "\\" + fileName);
            }
        }

        public static class DuplicateFields
        {
            public const string None ="No Duplicate";
            public const string MenuKey = "Menu Key";
            public const string MenuDisplayName = "Menu Display Name";
            public const string SeqNo = "Sequence No";
            public const string AllFields = "Mobile Menu";
        }

    }
}

// Added by Sahil to Check file upload header 18-07-2016
// public static void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
// {
// using (var image = System.Drawing.Image.FromStream(sourcePath))
// {
// var newWidth = (int)(image.Width * scaleFactor);
// var newHeight = (int)(image.Height * scaleFactor);
// var thumbnailImg = new Bitmap(newWidth, newHeight);
// var thumbGraph = Graphics.FromImage(thumbnailImg);
// thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
// thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
// thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
// var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
// thumbGraph.DrawImage(image, imageRectangle);
// thumbnailImg.Save(targetPath, image.RawFormat);
// }
// }