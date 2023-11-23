// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MovieShowReportModel.cs" company="KCSPL">
//     Copyright (c) KCSPL. All rights reserved.
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
    /// Class MovieShowReportModel.
    /// </summary>
    public partial class MovieShowReportModel
    {
        /// <summary>
        /// Gets or sets the LST movie date.
        /// </summary>
        /// <value>The LST movie date.</value>
        public List<SelectListItem> LstMovieDate { get; set; }

        /// <summary>
        /// Gets or sets the LST movie.
        /// </summary>
        /// <value>The LST movie.</value>
        public List<SelectListItem> LstMovie { get; set; }

        /// <summary>
        /// Gets or sets the LST movie shows.
        /// </summary>
        /// <value>The LST movie shows.</value>
        public List<SelectListItem> LstMovieShows { get; set; }

        /// <summary>
        /// Gets or sets the LST movie show report.
        /// </summary>
        /// <value>The LST movie show report.</value>
        public List<MS_MovieShowReportResult> LstMovieShowReport { get; set; }

        /// <summary>
        /// Gets or sets the movie date.
        /// </summary>
        /// <value>The movie date.</value>
        public string MovieDate { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre .
        /// </summary>
        /// <value>The value INT.</value>
        public int MovieTheatreId { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre Screen list.
        /// </summary>
        /// <value>The value INT.</value>
        public List<SelectListItem> LstMovieTheatreScreen { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre .
        /// </summary>
        /// <value>The value INT.</value>
        public int MovieClassId { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre Screen list.
        /// </summary>
        /// <value>The value INT.</value>
        public List<SelectListItem> LstMovieTheatreClass { get; set; }

        /// <summary>
        /// Gets or sets the movie.
        /// </summary>
        /// <value>The movie.</value>
        public string Movie { get; set; }

        /// <summary>
        /// Gets or sets the movie show.
        /// </summary>
        /// <value>The movie show.</value>
        public string MovieShow { get; set; }

        /// <summary>
        /// Gets or sets the windows title. Used in Movie show report page. keyName of "AAAConfigSettings" is "eCubePOSWindowTitle".
        /// </summary>
        /// <value>The windows title.</value>
        public string WindowsTitle { get; set; }

        public string TicketId { get; set; }

        public int MovieId { get; set; }

        public int ShowId { get; set; }

        public string MemberCode { get; set; }

    }
}