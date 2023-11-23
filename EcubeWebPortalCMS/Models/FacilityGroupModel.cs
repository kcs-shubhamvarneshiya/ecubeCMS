// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-18-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-18-2017
// ***********************************************************************
// <copyright file="FacilityGroupModel.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>FacilityGroupModel.cs</summary>
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
    /// Class FacilityGroupModel.
    /// </summary>
    public class FacilityGroupModel
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>The category identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the sequence no.
        /// </summary>
        /// <value>The sequence no.</value>
        public int? SequenceNo { get; set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [AllowHtml]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the banner image.
        /// </summary>
        /// <value>The banner image.</value>
        public string BannerImage { get; set; }

        /// <summary>
        /// Gets or sets the icon image.
        /// </summary>
        /// <value>The icon image.</value>
        public string IconImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        /// <summary>
        /// Gets or sets the HDN session.
        /// </summary>
        /// <value>The HDN session.</value>
        public string HdnSession { get; set; }

        /// <summary>
        /// Gets or sets the category list.
        /// </summary>
        /// <value>The category list.</value>
        public List<SelectListItem> CategoryList { get; set; }
    }
}