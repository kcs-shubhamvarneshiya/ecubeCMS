// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anjali Prajapati
// Created          : 18-04-2019
//
// Last Modified By : anjali Prajapati
// Last Modified On : 18-04-2019
// ***********************************************************************
// <copyright file="MovieShowRate.cs" company="kcspl">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    public class MovieShowRateModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Movie Show Period identifier.
        /// </summary>
        /// <value>The Movie Show Period Theatre identifier.</value>
        public int MovieShowPeriodId { get; set; }

        /// <summary>
        /// Gets or sets the class identifier.
        /// </summary>
        /// <value>The class identifier.</value>
        public int ClassId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre.
        /// </summary>
        /// <value>The name of the movie theatre.</value>
        public decimal MemberRate { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre.
        /// </summary>
        /// <value>The name of the movie theatre.</value>
        public decimal GuestRate { get; set; }
    }
}