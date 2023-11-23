// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="RoomBookingModel.cs" company="string.Empty">
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
    /// Class RoomBookingModel.
    /// </summary>
    public partial class RoomBookingModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the room booking.
        /// </summary>
        /// <value>The name of the room booking.</value>
        public string RoomBookingName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        /// <value>The member.</value>
        public decimal? Member { get; set; }

        /// <summary>
        /// Gets or sets the guest.
        /// </summary>
        /// <value>The guest.</value>
        public decimal? Guest { get; set; }

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
        public string HdnAaaConfig { get; set; }

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
        /// Gets or sets the room booking model list.
        /// </summary>
        /// <value>The room booking model list.</value>
        public List<RoomBookingModel> RoomBookingModelList { get; set; }

        /// <summary>
        /// Gets or sets the room booking model gallery list.
        /// </summary>
        /// <value>The room booking model gallery list.</value>
        public List<RoomBookingModel> RoomBookingModelGallaryList { get; set; }

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
        /// Gets or sets the room booking detail identifier.
        /// </summary>
        /// <value>The room booking detail identifier.</value>
        public long RoomBookingDetailId { get; set; }

        /// <summary>
        /// Gets or sets the tax percentage.
        /// </summary>
        /// <value>The tax percentage.</value>
        public decimal? TaxPercentage { get; set; }

        /// <summary>
        /// Gets or sets the tax description.
        /// </summary>
        /// <value>The tax description.</value>
        public string TaxDescription { get; set; }

        /// <summary>
        /// Gets or sets the room booking gallery identifier.
        /// </summary>
        /// <value>The room booking gallery identifier.</value>
        public long RoomBookingGallaryId { get; set; }

        /// <summary>
        /// Gets or sets the gallery image.
        /// </summary>
        /// <value>The gallery image.</value>
        public string GallaryImage { get; set; }
    }
}