// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Vasudev.Patel
// Created          : 02-13-2023
//
// ***********************************************************************
// <copyright file="EventRateModel.cs" company="string.Empty">
//     Copyright © 2023
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

    public partial class EventBookingForEventListModel
    {
        /// <summary>
        /// Gets or sets the event list.
        /// </summary>
        /// <value>The event list.</value>
        public List<EventList> EventList { get; set; }

        /// <summary>
        /// Gets or sets the document root directory path.
        /// </summary>
        /// <value>The document root directory path.</value>
        public string DocRootDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the Event Fees list.
        /// </summary>
        /// <value>The Event Fees list.</value>
        public List<SelectListItem> EventFeesList { get; set; }

        public int EventFeesId { get; set; }
    }
}