// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-13-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="InformationModel.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>InformationModel.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Class InformationModel.
    /// </summary>
    public class InformationModel
    {
        /// <summary>
        /// Gets or sets the information identifier.
        /// </summary>
        /// <value>The information identifier.</value>
        public int InformationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The Information name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [AllowHtml]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the HDN session.
        /// </summary>
        /// <value>The HDN session.</value>
        public string HdnSession { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        /// <summary>
        /// Gets or sets the sequence no.
        /// </summary>
        /// <value>The sequence no.</value>
        public int? SequenceNo { get; set; }
    }
}