// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : pratikp
// Created          : 11-21-2016
//
// Last Modified By : pratikp
// Last Modified On : 11-21-2016
// ***********************************************************************
// <copyright file="DocumentLibrary.cs" company="KCS">
//     Copyright ©  2016
// </copyright>
// <summary>DocumentLibrary</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Class DocumentLibrary.
    /// </summary>
    public class DocumentLibrary
    {
        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>The document identifier.</value>
        public long DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>The name of the document.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}