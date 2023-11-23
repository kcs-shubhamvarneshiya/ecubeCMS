// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : karan.shah
// Created          : 09-20-2017
//
// Last Modified By : karan.shah
// Last Modified On : 09-20-2017
// ***********************************************************************
// <copyright file="PostModel.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>PostModel.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Class PostModel.
    /// </summary>
    public class PostModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The post name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image1.
        /// </summary>
        /// <value>The image1.</value>
        public string Image1 { get; set; }

        /// <summary>
        /// Gets or sets the image2.
        /// </summary>
        /// <value>The image2.</value>
        public string Image2 { get; set; }

        /// <summary>
        /// Gets or sets the image3.
        /// </summary>
        /// <value>The image3.</value>
        public string Image3 { get; set; }

        /// <summary>
        /// Gets or sets the image4.
        /// </summary>
        /// <value>The image4.</value>
        public string Image4 { get; set; }

        /// <summary>
        /// Gets or sets the image5.
        /// </summary>
        /// <value>The image5.</value>
        public string Image5 { get; set; }

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
    }
}