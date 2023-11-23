// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 07-23-2016
// ***********************************************************************
// <copyright file="EventRateModel.cs" company="string.Empty">
//     Copyright © 2016
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
    /// Class EventRateModel.
    /// </summary>
    public partial class EventRateModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the event ticket category identifier.
        /// </summary>
        /// <value>The event ticket category identifier.</value>
        public int EventTicketCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Account head identifier.
        /// </summary>
        /// <value>The Account head identifier.</value>
        public int AccountheadId { get; set; } 

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the member fees.
        /// </summary>
        /// <value>The member fees.</value>
        public decimal MemberFees { get; set; }

        /// <summary>
        /// Gets or sets the special guest.
        /// </summary>
        /// <value>The special guest.</value>
        public decimal SubMemberFees { get; set; }

        /// <summary>
        /// Gets or sets the guest fees.
        /// </summary>
        /// <value>The guest fees.</value>
        public decimal GuestFees { get; set; }

        /// <summary>
        /// Gets or sets the Affiliate Member Fees.
        /// </summary>
        /// <value>The special guest.</value>
        public decimal AffiliateMemberFee { get; set; }

        /// <summary>
        /// Gets or sets the Parent Fees.
        /// </summary>
        /// <value>The special guest.</value>
        public decimal ParentFee { get; set; }

        /// <summary>
        /// Gets or sets the Guest Child Fee.
        /// </summary>
        /// <value>The special guest.</value>
        public decimal GuestChildFee { get; set; }

        /// <summary>
        /// Gets or sets the Guest Room Fees.
        /// </summary>
        /// <value>The special guest.</value>
        public decimal GuestRoomFee { get; set; }
               

        /// <summary>
        /// Gets or sets the From Serial No.
        /// </summary>
        /// <value>The no of seats.</value>
        public int FromSerialNo { get; set; }

        /// <summary>
        /// Gets or sets the To Serial No.
        /// </summary>
        /// <value>The no of seats.</value>
        public int ToSerialNo { get; set; }

        /// <summary>
        /// Gets or sets the no of seats.
        /// </summary>
        /// <value>The no of seats.</value>
        public int NoOfSeats { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        //// AddBy : Karan Shah on Date: 29-MAR-2017

        /// <summary>
        /// Gets or sets the member seat limit.
        /// </summary>
        /// <value>The member seat limit.</value>
        public short MemberSeatLimit { get; set; }

        /// <summary>
        /// Gets or sets the guest seat limit.
        /// </summary>
        /// <value>The guest seat limit.</value>
        public short GuestSeatLimit { get; set; }
    }
}
