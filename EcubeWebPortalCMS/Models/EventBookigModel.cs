// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-23-2016
//
// Last Modified By : Maulik Shah
// Last Modified On : 04-25-2017
// ***********************************************************************
// <copyright file="EventBookigModel.cs" company="string.Empty">
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
    /// Class CMSConfigModel.
    /// </summary>
    public partial class EventBookingModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the event category identifier.
        /// </summary>
        /// <value>The event category identifier.</value>
        public int EventCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the event title.
        /// </summary>
        /// <value>The event title.</value>
        public string EventTitle { get; set; }

        /// <summary>
        /// Gets or sets the event screen.
        /// </summary>
        /// <value>The event screen.</value>
        public string EventScreen { get; set; }

        /// <summary>
        /// Gets or sets the event place.
        /// </summary>
        /// <value>The event place.</value>
        public string EventPlace { get; set; }

        /// <summary>
        /// Gets or sets the duration of the event.
        /// </summary>
        /// <value>The duration of the event.</value>
        public string EventDuration { get; set; }

        /// <summary>
        /// Gets or sets the event start time.
        /// </summary>
        /// <value>The event start time.</value>
        public string EventStartTime { get; set; }

        /// <summary>
        /// Gets or sets the event end time.
        /// </summary>
        /// <value>The event end time.</value>
        public string EventEndTime { get; set; }

        /// <summary>
        /// Gets or sets the even last registration date.
        /// </summary>
        /// <value>The even last registration date.</value>
        public DateTime EvenLastRegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        /// <value>The registration date.</value>
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the artists information.
        /// </summary>
        /// <value>The artists information.</value>
        [AllowHtml]
        public string ArtistsInfo { get; set; }

        /// <summary>
        /// Gets or sets the event synopsis.
        /// </summary>
        /// <value>The event synopsis.</value>
        [AllowHtml]
        public string EventSynopsis { get; set; }

        /// <summary>
        /// Gets or sets the terms conditions.
        /// </summary>
        /// <value>The terms conditions.</value>
        [AllowHtml]
        public string TermsConditions { get; set; }

        /// <summary>
        /// Gets or sets the event image.
        /// </summary>
        /// <value>The event image.</value>
        public string EventImage { get; set; }

        /// <summary>
        /// Gets or sets the event banner image.
        /// </summary>
        /// <value>The event banner image.</value>
        public string EventBannerImage { get; set; }

        /// <summary>
        /// Gets or sets the event Attchment .
        /// </summary>
        /// <value>The event attachment.</value>
        public string EventAttachment { get; set; }

        public string EventAttachmentDisplay { get; set; }

        /// <summary>
        /// Gets or sets the HDN event image.
        /// </summary>
        /// <value>The HDN event image.</value>
        public string HfnEventImage { get; set; }

        /// <summary>
        /// Gets or sets the HDN event banner image.
        /// </summary>
        /// <value>The HDN event banner image.</value>
        public string HdnEventBannerImage { get; set; }

        /// <summary>
        /// Gets or sets the HDN event attachment.
        /// </summary>
        /// <value>The HDN attachment.</value>
        public string HfnEventAttachment { get; set; }

        /// <summary>
        /// Gets or sets the step sequence.
        /// </summary>
        /// <value>The step sequence.</value>
        public int StepSeq { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [HDN i frame].
        /// </summary>
        /// <value><c>true</c> if [HDN i frame]; otherwise, <c>false</c>.</value>
        public bool HdnIFrame { get; set; }

        /// <summary>
        /// Gets or sets the LST event schedule model.
        /// </summary>
        /// <value>The LST event schedule model.</value>
        public EventScheduleModel ObjEventScheduleModel { get; set; }

        /// <summary>
        /// Gets or sets the object event rate model.
        /// </summary>
        /// <value>The object event rate model.</value>
        public EventRateModel ObjEventRateModel { get; set; }

        /// <summary>
        /// Gets or sets the account head identifier.
        /// </summary>
        /// <value>The account head identifier.</value>
        public long AccountHeadId { get; set; }

        /// <summary>
        /// Gets or sets the account head list.
        /// </summary>
        /// <value>The account head list.</value>
        public List<SelectListItem> AccountHeadList { get; set; }

        /// <summary>
        /// Gets or sets the tax master identifier.
        /// </summary>
        /// <value>The tax master identifier.</value>
        public long TaxMasterId { get; set; }

        /// <summary>
        /// Gets or sets the tax master list.
        /// </summary>
        /// <value>The tax master list.</value>
        public List<SelectListItem> TaxMasterList { get; set; }

        /// <summary>
        /// Gets or sets the guest limit.
        /// </summary>
        /// <value>The guest limit.</value>
        public int GuestLimit { get; set; }

        /// <summary>
        /// Gets or sets the maximum ticket for family member.
        /// </summary>
        /// <value>The maximum ticket for family member.</value>
        public int? MaximumTicketForFamilyMember { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is maximum ticket for family member.
        /// </summary>
        /// <value><c>null</c> if [is maximum ticket for family member] contains no value, <c>true</c> if [is maximum ticket for family member]; otherwise, <c>false</c>.</value>
        public bool IsMaximumTicketForFamilyMember { get; set; }

        public bool IsCheckOutstanding { get; set; }

        public int OutstandingAmountForMember { get; set; }

        /// <summary>
        /// Gets or sets the registration end time.
        /// </summary>
        /// <value>The registration end time.</value>
        public string RegistrationEndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is QR code.
        /// </summary>
        /// <value><c>true</c> if this instance is QR code; otherwise, <c>false</c>.</value>
        public bool IsQRCode { get; set; }

        /// <summary>
        /// Gets or sets the event entry before.
        /// </summary>
        /// <value>The event entry before.</value>
        public int EventEntryBefore { get; set; }

        /// <summary>
        /// Gets or sets the event entry after.
        /// </summary>
        /// <value>The event entry after.</value>
        public int EventEntryAfter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is display detail.
        /// </summary>
        /// <value><c>true</c> if this instance is display detail; otherwise, <c>false</c>.</value>
        public bool IsDisplayDetail { get; set; }

        /// <summary>
        /// Gets or sets the event mobile image.
        /// </summary>
        /// <value>The event mobile image.</value>
        public string EventMobileImage { get; set; }

        /// <summary>
        /// Gets or sets the HDN event mobile image.
        /// </summary>
        /// <value>The HDN event mobile image.</value>
        public string HdnEventMobileImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is maximum ticket for family member.
        /// </summary>
        /// <value><c>null</c> if [is maximum ticket for family member] contains no value, <c>true</c> if [Allow Multiple Category booking for member in single event.]; otherwise, <c>false</c>.</value>
        public bool AllowMultipleCategory { get; set; }


    }
}
