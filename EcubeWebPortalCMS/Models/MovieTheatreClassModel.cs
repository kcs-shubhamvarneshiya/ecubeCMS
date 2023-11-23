// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anjali Prajapati
// Created          : 17-04-2019
//
// Last Modified By : anjali Prajapati
// Last Modified On : 17-04-2019
// ***********************************************************************
// <copyright file="MovieTheatreClassModel.cs" company="kcspl">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    public class MovieTheatreClassModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Theatre identifier.
        /// </summary>
        /// <value>The Theatre identifier.</value>
        public long TheatreId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre.
        /// </summary>
        /// <value>The name of the movie theatre.</value>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre.
        /// </summary>
        /// <value>The name of the movie theatre.</value>
        public int MemberSubServiceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie theatre.
        /// </summary>
        /// <value>The name of the movie theatre.</value>
        public int GuestSubServiceId { get; set; }
    }
}