// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="AuthConfig.cs" company="string.Empty">
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
    using System.Linq;
    using System.Text;
    using EcubeWebPortalCMS.Models;
    using Microsoft.Web.WebPages.OAuth;

    /// <summary>
    /// Class Config.
    /// </summary>
    public static class AuthConfig
    {
        /// <summary>
        /// Registers the authentication.
        /// </summary>
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166
            // OAuthWebSecurity.RegisterMicrosoftClient(
            // clientId: "",
            // clientSecret: "");
            // OAuthWebSecurity.RegisterTwitterClient(
            // consumerKey: "",
            // consumerSecret: "");          
            // OAuthWebSecurity.RegisterFacebookClient(
            // appId: "",
            // appSecret: "");
            // OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
