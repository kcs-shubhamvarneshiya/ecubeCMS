// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-08-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MovieShowAssignModel.cs" company="KCSPL">
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
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Class MovieShowAssignModel.
    /// </summary>
    public class MovieShowAssignModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the string start date.
        /// </summary>
        /// <value>The string start date.</value>
        public string StrStartDate { get; set; }

        /// <summary>
        /// Gets or sets the string end date.
        /// </summary>
        /// <value>The string end date.</value>
        public string StrEndDate { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the LST movie show period.
        /// </summary>
        /// <value>The LST movie show period.</value>
        public List<MS_GetMovieShowPeriodResult> LstMovieShowPeriod { get; set; }

        /// <summary>
        /// Gets or sets the LST movie show.
        /// </summary>
        /// <value>The LST movie show.</value>
        public List<MovieShow> LstMovieShow { get; set; }

        /// <summary>
        /// Gets or sets the LST movie show details.
        /// </summary>
        /// <value>The LST movie show details.</value>
        public List<MovieShowDetailsforPeriod> LstMovieShowDetails { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie.
        /// </summary>
        /// <value>The name of the movie.</value>
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s1.
        /// </summary>
        /// <value><c>true</c> if s1; otherwise, <c>false</c>.</value>
        [Display(Name = "1")]
        public bool S1 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s2.
        /// </summary>
        /// <value><c>true</c> if s2; otherwise, <c>false</c>.</value>
        [Display(Name = "2")]
        public bool S2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s3.
        /// </summary>
        /// <value><c>true</c> if s3; otherwise, <c>false</c>.</value>
        [Display(Name = "3")]
        public bool S3 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s4.
        /// </summary>
        /// <value><c>true</c> if s4; otherwise, <c>false</c>.</value>
        [Display(Name = "4")]
        public bool S4 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s5.
        /// </summary>
        /// <value><c>true</c> if s5; otherwise, <c>false</c>.</value>
        [Display(Name = "5")]
        public bool S5 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s6.
        /// </summary>
        /// <value><c>true</c> if s6; otherwise, <c>false</c>.</value>
        [Display(Name = "6")]
        public bool S6 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s7.
        /// </summary>
        /// <value><c>true</c> if s7; otherwise, <c>false</c>.</value>
        [Display(Name = "7")]
        public bool S7 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s8.
        /// </summary>
        /// <value><c>true</c> if s8; otherwise, <c>false</c>.</value>
        [Display(Name = "8")]
        public bool S8 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MovieShowAssignModel"/> is s9.
        /// </summary>
        /// <value><c>true</c> if s9; otherwise, <c>false</c>.</value>
        [Display(Name = "9")]
        public bool S9 { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre .
        /// </summary>
        /// <value>The value INT.</value>
        public int MovieTheatreId { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre Name.
        /// </summary>
        /// <value>The value INT.</value>
        public int MovieTheatreName { get; set; }

        /// <summary>
        /// Gets or sets Movie Theatre Screen list.
        /// </summary>
        /// <value>The value INT.</value>
        public List<SelectListItem> LstMovieTheatreScreen { get; set; }

        /// <summary>
        /// Gets or sets the date to display movie.
        /// </summary>
        /// <value>The date to display movie.</value>
        public string DateToDisplayMovie { get; set; }

        /// <summary>
        /// Gets or sets the time to display movie.
        /// </summary>
        /// <value>The time to display movie.</value>
        public string TimeToDisplayMovie { get; set; }
    }
}