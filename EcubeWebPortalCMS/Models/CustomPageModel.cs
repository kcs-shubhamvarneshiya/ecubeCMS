// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-12-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-24-2016
// ***********************************************************************
// <copyright file="CustomPageModel.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>CustomPageModel.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Class CustomPageModel.
    /// </summary>
    public class CustomPageModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>The name of the page.</value>
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the content of the page.
        /// </summary>
        /// <value>The content of the page.</value>
        [AllowHtml]
        public string PageContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is login.
        /// </summary>
        /// <value><c>true</c> If this instance is login; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> If [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}