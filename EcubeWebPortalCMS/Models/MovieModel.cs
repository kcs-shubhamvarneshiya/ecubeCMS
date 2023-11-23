// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-08-2016
// ***********************************************************************
// <copyright file="MovieModel.cs" company="KCSPL">
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
    /// Class MovieModel.
    /// </summary>
    public partial class MovieModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie.
        /// </summary>
        /// <value>The name of the movie.</value>
        public string MovieName { get; set; }

        /// <summary>
        /// Gets or sets the movie category.
        /// </summary>
        /// <value>The movie category.</value>
        public string MovieCategory { get; set; }

        /// <summary>
        /// Gets or sets the movie rating.
        /// </summary>
        /// <value>The movie rating.</value>
        public string MovieRating { get; set; }

        /// <summary>
        /// Gets or sets the movie language.
        /// </summary>
        /// <value>The movie language.</value>
        public string MovieLanguage { get; set; }

        /// <summary>
        /// Gets or sets the type of the movie content.
        /// </summary>
        /// <value>The type of the movie content.</value>
        public string MovieContentType { get; set; }

        /// <summary>
        /// Gets or sets the type of the movie.
        /// </summary>
        /// <value>The type of the movie.</value>
        public string MovieType { get; set; }

        /// <summary>
        /// Gets or sets the movie model list.
        /// </summary>
        /// <value>The movie model list.</value>
        public List<MovieModel> MovieModelList { get; set; }

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
        /// Gets or sets the upload image.
        /// </summary>
        /// <value>The upload image.</value>
        public string UploadImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the movie theatre show identifier.
        /// </summary>
        /// <value>The movie theatre show identifier.</value>
        public long MovieTheatreShowId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre show.
        /// </summary>
        /// <value>The name of the movie theatre show.</value>
        public string MovieTheatreShowName { get; set; }

        /// <summary>
        /// Gets or sets the show key date.
        /// </summary>
        /// <value>The show key date.</value>
        public DateTime? ShowKeyDate { get; set; }

        /// <summary>
        /// Gets or sets the string show key date.
        /// </summary>
        /// <value>The string show key date.</value>
        public string StrShowKeyDate { get; set; }

        /// <summary>
        /// Gets or sets the movie theatre identifier.
        /// </summary>
        /// <value>The movie theatre identifier.</value>
        public long MovieTheatreId { get; set; }

        /// <summary>
        /// Gets or sets the movie show identifier.
        /// </summary>
        /// <value>The movie show identifier.</value>
        public long MovieShowId { get; set; }

        /// <summary>
        /// Gets or sets the member amount.
        /// </summary>
        /// <value>The member amount.</value>
        public decimal MemberAmount { get; set; }

        /// <summary>
        /// Gets or sets the guest amount.
        /// </summary>
        /// <value>The guest amount.</value>
        public decimal GuestAmount { get; set; }

        /// <summary>
        /// Gets or sets the mobile image.
        /// </summary>
        /// <value>The mobile image.</value>
        public string MobileImage { get; set; }

        /// <summary>
        /// Gets or sets the show mobile image.
        /// </summary>
        /// <value>The show mobile image.</value>
        public string ShowMobileImage { get; set; }

        /// <summary>
        /// Gets or sets the application description.
        /// </summary>
        /// <value>The application description.</value>
        public string AppDescription { get; set; }

        /// <summary>
        /// Gets or sets the LST movie theatre.
        /// </summary>
        /// <value>The LST movie theatre.</value>
        public List<SelectListItem> LstMovieTheatre { get; set; }

        /// <summary>
        /// Gets or sets the LST movie shows.
        /// </summary>
        /// <value>The LST movie shows.</value>
        public List<SelectListItem> LstMovieshows { get; set; }

        /// <summary>
        /// Gets or sets the Before Minutes of the movie.
        /// </summary>
        /// <value>The Before Minutes of the movie.</value>
        public int? BeforeMinutes { get; set; }

        /// <summary>
        /// Gets or sets the After Minutes of the movie.
        /// </summary>
        /// <value>The After Minutes of the movie.</value>
        public int? AfterMinutes { get; set; }
    }
}
