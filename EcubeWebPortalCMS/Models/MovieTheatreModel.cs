// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 06-13-2017
// ***********************************************************************
// <copyright file="MovieTheatreModel.cs" company="string.Empty">
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
    /// Class MovieTheatreModel.
    /// </summary>
    public partial class MovieTheatreModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre.
        /// </summary>
        /// <value>The name of the movie theatre.</value>
        public string MovieTheatreName { get; set; }

        /// <summary>
        /// Gets or sets the theatre floor.
        /// </summary>
        /// <value>The theatre floor.</value>
        public string TheatreFloor { get; set; }

        /// <summary>
        /// Gets or sets the movie theatre model list.
        /// </summary>
        /// <value>The movie theatre model list.</value>
        public List<MovieTheatreModel> MovieTheatreModelList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre .
        /// </summary>
        /// <value>The value INT.</value>
        public long? ServiceId { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre Name.
        /// </summary>
        /// <value>The value INT.</value>
        public int MemberSubServiceIdName { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre Name.
        /// </summary>
        /// <value>The value INT.</value>
        public int GuestSubServiceIdName { get; set; }

        /// <summary>
        /// Gets or sets Service list.
        /// </summary>
        /// <value>The Service list.</value>
        public List<SelectListItem> LstService { get; set; }
    }
}
