// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Savan Marakna
// Created          : 11-10-2023
//
// ***********************************************************************
// <copyright file="InquiryCategoryModel.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;

    /// <summary>
    /// Class InquiryCategoryModel.
    /// </summary>
    public class InquiryCategoryModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Inquiry category.
        /// </summary>
        /// <value>The name of the Inquiry category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the image name of the Inquiry category.
        /// </summary>
        /// <value>The image name of the Inquiry category.</value>
        public string CategoryImage { get; set; }

        /// <summary>
        /// Gets or sets the image name of the Inquiry category.
        /// </summary>
        /// <value>The image name of the Inquiry category.</value>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the image name of the Inquiry category.
        /// </summary>
        /// <value>The image name of the Inquiry category.</value>
        public string Email { get; set; }

        public int SeqNo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>True</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        public string HdnSession { get; set; }
    }
}