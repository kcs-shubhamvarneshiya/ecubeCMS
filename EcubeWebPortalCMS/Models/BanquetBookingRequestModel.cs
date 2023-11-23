// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-28-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-29-2016
// ***********************************************************************
// <copyright file="BanquetBookingRequestModel.cs" company="string.Empty">
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
    /// Class BanquetBookingRequestModel.
    /// </summary>
    public partial class BanquetBookingRequestModel
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
        /// Gets or sets a value indicating whether [booking type].
        /// </summary>
        /// <value><c>null</c> if [booking type] contains no value, <c>true</c> if [booking type]; otherwise, <c>false</c>.</value>
        public bool? BookingType { get; set; }

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
        /// <value>The mobile no.</value>
        public string MobileNo { get; set; }

        /// <summary>
        /// Gets or sets the occasion.
        /// </summary>
        /// <value>The occasion.</value>
        public string Occasion { get; set; }

        /// <summary>
        /// Gets or sets the booking date.
        /// </summary>
        /// <value>The booking date.</value>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets the string booking date.
        /// </summary>
        /// <value>The string booking date.</value>
        public string StrFromDate { get; set; }

        /// <summary>
        /// Gets or sets the typeId of the banquet.
        /// </summary>
        /// <value>The typeId of the banquet.</value>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets or sets the string booking date.
        /// </summary>
        /// <value>The string booking date.</value>
        public string StrToDate { get; set; }

        /// <summary>
        /// Gets or sets the typeId of the banquet.
        /// </summary>
        /// <value>The typeId of the banquet.</value>
        public string BanquetTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the banquet.
        /// </summary>
        /// <value>The type of the banquet.</value>
        public string BanquetType { get; set; }

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
        /// Gets or sets the banquet booking request model list.
        /// </summary>
        /// <value>The banquet booking request model list.</value>
        public List<BanquetBookingRequestModel> BanquetBookingRequestModelList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }
    }
}
