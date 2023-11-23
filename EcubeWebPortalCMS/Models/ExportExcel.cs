// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : Maulik Shah
// Created          : 11-Apr-2017
//
// Last Modified By : Maulik Shah
// Last Modified On : 04-11-2017
// ***********************************************************************
// <copyright file="ExportExcel.cs" company="KCSPL">
//     Copyright ©  2016
// </copyright>
// <summary>ExportExcel.cs</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace EcubeWebPortalCMS.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Class ExportExcel.
    /// </summary>
    public class ExportExcel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportExcel"/> class.
        /// </summary>
        public ExportExcel()
        {
            this.Status = string.Empty;
        }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public string Filters { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>The search.</value>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the type of the booking.
        /// </summary>
        /// <value>The type of the booking.</value>
        public string BookingType { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>From date.</value>
        public string FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>Selected To date.</value>
        public string ToDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets all items.
        /// </summary>
        /// <value>All items.</value>
        public List<SelectListItem> AllItems { get; set; }
    }
}