// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 10-10-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 10-10-2016
// ***********************************************************************
// <copyright file="MovieShowDetailsforPeriod.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>ksc</summary>
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

    /// <summary>
    /// Class MovieShow Details for Period.
    /// </summary>
    public class MovieShowDetailsforPeriod
    {
        /// <summary>
        /// Gets or sets the movie show period identifier.
        /// </summary>
        /// <value>The movie show period identifier.</value>
        public int MovieShowPeriodId { get; set; }

        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>The movie identifier.</value>
        public int MovieId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie.
        /// </summary>
        /// <value>The name of the movie.</value>
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        /// <summary>
        /// Gets or sets the s1.
        /// </summary>
        /// <value>Parameter s1.</value>
        [Display(Name = "1")]
        public string S1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the s2.
        /// </summary>
        /// <value>Parameter s2.</value>
        [Display(Name = "2")]
        public string S2 { get; set; }

        /// <summary>
        /// Gets or sets the s3.
        /// </summary>
        /// <value>Parameter s3.</value>
        [Display(Name = "3")]
        public string S3 { get; set; }

        /// <summary>
        /// Gets or sets the s4.
        /// </summary>
        /// <value>Parameter s4.</value>
        [Display(Name = "4")]
        public string S4 { get; set; }

        /// <summary>
        /// Gets or sets the s5.
        /// </summary>
        /// <value>Parameter s5.</value>
        [Display(Name = "5")]
        public string S5 { get; set; }

        /// <summary>
        /// Gets or sets the s6.
        /// </summary>
        /// <value>Parameter s6.</value>
        [Display(Name = "6")]
        public string S6 { get; set; }

        /// <summary>
        /// Gets or sets the s7.
        /// </summary>
        /// <value>Parameter s7.</value>
        [Display(Name = "7")]
        public string S7 { get; set; }

        /// <summary>
        /// Gets or sets the s8.
        /// </summary>
        /// <value>Parameter s8.</value>
        [Display(Name = "8")]
        public string S8 { get; set; }

        /// <summary>
        /// Gets or sets the s9.
        /// </summary>
        /// <value>Parameter s9.</value>
        [Display(Name = "9")]
        public string S9 { get; set; }

        /// <summary>
        /// Gets or sets the S10.
        /// </summary>
        /// <value>Parameter S10.</value>
        [Display(Name = "10")]
        public string S10 { get; set; }

        /// <summary>
        /// Gets or sets the S11.
        /// </summary>
        /// <value>Parameter S11.</value>
        [Display(Name = "11")]
        public string S11 { get; set; }

        /// <summary>
        /// Gets or sets the S12.
        /// </summary>
        /// <value>Parameter S12.</value>
        [Display(Name = "12")]
        public string S12 { get; set; }
    }
}