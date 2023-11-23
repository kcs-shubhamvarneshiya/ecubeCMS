// ***********************************************************************
// Assembly         : EcubeWebPortalCMS.Model
// Author           : Vasudev patel
// Created          : 13-02-2022
//
// ***********************************************************************
// <copyright file="EventList.cs" company="KCSPL">
//     Copyright ©  2023
// </copyright>
// <summary>EventList.cs</summary>
// ***********************************************************************

/// <summary>
/// The Model namespace.
/// </summary>

namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class EventList.
    /// </summary>
    public class EventList
    {
        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the event image path.
        /// </summary>
        /// <value>The event image path.</value>
        public string EventImage { get; set; }

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>The event date.</value>
        public string EventDate { get; set; }

        /// <summary>
        /// Gets or sets the object event list.
        /// </summary>
        /// <value>The object event list.</value>
        public List<EventList> ObjEventList { get; set; }

        /// <summary>
        /// Gets or sets the event place.
        /// </summary>
        /// <value>The event place.</value>
        public string EventPlace { get; set; }

        /// <summary>
        /// Gets or sets the event title.
        /// </summary>
        /// <value>The event title.</value>
        public string EventTitle { get; set; }

        /// <summary>
        /// Gets or sets the document root directory path.
        /// </summary>
        /// <value>The document root directory path.</value>
        public string DocRootDirectoryPath { get; set; }
    }
}