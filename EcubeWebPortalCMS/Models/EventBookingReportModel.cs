// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Maulik Shah
// Created          : 29-Apr-2017
//
// Last Modified By : Maulik Shah
// Last Modified On : 04-29-2017
// ***********************************************************************
// <copyright file="EventBookingReportModel.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>EventBookingReportModel.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Class EventBookingReportModel.
    /// </summary>
    public class EventBookingReportModel
    {
        /// <summary>
        /// Gets or sets the list event.
        /// </summary>
        /// <value>The list event.</value>
        public List<SelectListItem> LstEvent { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>The ticket identifier.</value>
        public string TicketId { get; set; }

        /// <summary>
        /// Gets or sets the member code.
        /// </summary>
        /// <value>The member code.</value>
        public string MemberCode { get; set; }

        /// <summary>
        /// Gets or sets the Event Date.
        /// </summary>
        /// <value>The Event Date.</value>
        public string EventDate { get; set; }
    }
}