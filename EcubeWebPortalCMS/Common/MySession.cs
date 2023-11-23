// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MySession.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Common namespace.
/// </summary>
namespace EcubeWebPortalCMS.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Class MySession.
    /// </summary>
    public class MySession
    {
        /// <summary>
        /// Common Name of cookies.
        /// </summary>
        public string CookiesName = "kcsremuser";

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static MySession Current
        {
            get
            {
                return new MySession();
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int UserId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.CookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.CookiesName].Values["userid"]))
                {
                    return HttpContext.Current.Request.Cookies[this.CookiesName].Values["userid"].ToString().IntSafe();
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.CookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.CookiesName].Values["username"]))
                {
                    return HttpContext.Current.Request.Cookies[this.CookiesName].Values["username"].ToString();
                }

                return "Administrator";
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.CookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.CookiesName].Values["password"]))
                {
                    return HttpContext.Current.Request.Cookies[this.CookiesName].Values["password"].ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string Fullname
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.CookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.CookiesName].Values["fullname"]))
                {
                    return HttpContext.Current.Request.Cookies[this.CookiesName].Values["fullname"].ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the remember me.
        /// </summary>
        /// <value>The remember me.</value>
        public string Rememberme
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.CookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.CookiesName].Values["rememberme"]))
                {
                    return HttpContext.Current.Request.Cookies[this.CookiesName].Values["rememberme"].ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the role id.
        /// </summary>
        /// <value>The role id.</value>
        public int RoleId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.CookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.CookiesName].Values["roleid"]))
                {
                    return HttpContext.Current.Request.Cookies[this.CookiesName].Values["roleid"].ToString().IntSafe();
                }

                return 0;
            }
        } 
    }
}