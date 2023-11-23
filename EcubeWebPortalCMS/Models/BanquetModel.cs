// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="BanquetModel.cs" company="string.Empty">
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
    /// Class BanquetModel.
    /// </summary>
    public partial class BanquetModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the banquet.
        /// </summary>
        /// <value>The name of the banquet.</value>
        public string BanquetName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the minimum person Capacity.
        /// </summary>
        /// <value>The minimum person Capacity.</value>
        public long? MinPersonCapcity { get; set; }

        /// <summary>
        /// Gets or sets the maximum person Capacity.
        /// </summary>
        /// <value>The maximum person Capacity.</value>
        public long? MaxPersonCapcity { get; set; }

        /// <summary>
        /// Gets or sets the profile pic.
        /// </summary>
        /// <value>The profile pic.</value>
        public string ProfilePic { get; set; }

        /// <summary>
        /// Gets or sets the terms.
        /// </summary>
        /// <value>The terms.</value>
        public string Terms { get; set; }

        /// <summary>
        /// Gets or sets the HDN AAA configuration.
        /// </summary>
        /// <value>The HDN AAA configuration.</value>
        public string HdnAAAConfig { get; set; }

        /// <summary>
        /// Gets or sets the HDN image profile pic.
        /// </summary>
        /// <value>The HDN image profile pic.</value>
        public string HdnImgProfilePic { get; set; }

        /// <summary>
        /// Gets or sets the HDN FI upload terms.
        /// </summary>
        /// <value>The HDN FI upload terms.</value>
        public string HdnFlUploadTerms { get; set; }

        /// <summary>
        /// Gets or sets the banquet model list.
        /// </summary>
        /// <value>The banquet model list.</value>
        public List<BanquetModel> BanquetModelList { get; set; }

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
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the banquet detail identifier.
        /// </summary>
        /// <value>The banquet detail identifier.</value>
        public long BanquetDetailId { get; set; }

        /// <summary>
        /// Gets or sets the gallery image.
        /// </summary>
        /// <value>The gallery image.</value>
        public string GallaryImage { get; set; }
    }
}
