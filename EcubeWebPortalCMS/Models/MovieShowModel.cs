// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="MovieShowModel.cs" company="string.Empty">
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
    /// Class MovieShowModel.
    /// </summary>
    public partial class MovieShowModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie show.
        /// </summary>
        /// <value>The name of the movie show.</value>
        public string MovieShowName { get; set; }

        /// <summary>
        /// Gets or sets the show start date.
        /// </summary>
        /// <value>The show start date.</value>
        public DateTime ShowStartDate { get; set; }

        /// <summary>
        /// Gets or sets the string show start date.
        /// </summary>
        /// <value>The string show start date.</value>
        public string StrShowStartDate { get; set; }

        /// <summary>
        /// Gets or sets the show end date.
        /// </summary>
        /// <value>The show end date.</value>
        public DateTime ShowEndDate { get; set; }

        /// <summary>
        /// Gets or sets the string show end date.
        /// </summary>
        /// <value>The string show end date.</value>
        public string StrShowEndDate { get; set; }

        /// <summary>
        /// Gets or sets the movie show model list.
        /// </summary>
        /// <value>The movie show model list.</value>
        public List<MovieShowModel> MovieShowModelList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }
    }
}
