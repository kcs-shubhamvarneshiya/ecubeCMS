// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-29-2016
// ***********************************************************************
// <copyright file="RoomBookingRequestModel.cs" company="string.Empty">
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
    /// Class RoomBookingRequestModel.
    /// </summary>
    public partial class RoomBookingRequestModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Parameter name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>The member identifier.</value>
        public long? MemberId { get; set; }

        /// <summary>
        /// Gets or sets the MemberCode.
        /// </summary>
        /// <value>The MemberCode identifier.</value>
        public string MemberCode { get; set; }

        /// <summary>
        /// Gets or sets the refer member identifier.
        /// </summary>
        /// <value>The refer MemberReferredId.</value>
        public long? MemberReferredId { get; set; }

        /// <summary>
        /// Gets or sets the ReferredMemberCode.
        /// </summary>
        /// <value>The ReferredMemberCode identifier.</value>
        public string ReferredMemberCode { get; set; }

        /// <summary>
        /// Gets or sets the inquiry for.
        /// </summary>
        /// <value>The inquiry for.</value>
        public long? InquiryFor { get; set; }

        /// <summary>
        /// Gets or sets the e mail.
        /// </summary>
        /// <value>The e mail.</value>
        public string EMail { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>Parameter city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the mobile no.
        /// </summary>
        /// <value>Parameter mobile no.</value>
        public string MobileNo { get; set; }

        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        /// <value>The member.</value>
        public long? Member { get; set; }

        /// <summary>
        /// Gets or sets the adults.
        /// </summary>
        /// <value>The adults.</value>
        public long? Adults { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public long? Children { get; set; }

        /// <summary>
        /// Gets or sets the checked in date.
        /// </summary>
        /// <value>The checked in date.</value>
        public DateTime? CheckedInDate { get; set; }

        /// <summary>
        /// Gets or sets the string checked in date.
        /// </summary>
        /// <value>The string checked in date.</value>
        public string StrCheckedInDate { get; set; }

        /// <summary>
        /// Gets or sets the checked out date.
        /// </summary>
        /// <value>The checked out date.</value>
        public DateTime? CheckedOutDate { get; set; }

        /// <summary>
        /// Gets or sets the string checked out date.
        /// </summary>
        /// <value>The string checked out date.</value>
        public string StrCheckedOutDate { get; set; }

        /// <summary>
        /// Gets or sets the typeId of the room.
        /// </summary>
        /// <value>The typeId of the room.</value>
        public string RoomTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the room.
        /// </summary>
        /// <value>The type of the room.</value>
        public string RoomType { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>The remarks.</value>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the StatusId.
        /// </summary>
        /// <value>The StatusId.</value>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>The Status.</value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the AdminRemarks.
        /// </summary>
        /// <value>The AdminRemarks.</value>
        public string AdminRemarks { get; set; }

        /// <summary>
        /// Gets or sets the room booking request model list.
        /// </summary>
        /// <value>The room booking request model list.</value>
        public List<RoomBookingRequestModel> RoomBookingRequestModelList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}
