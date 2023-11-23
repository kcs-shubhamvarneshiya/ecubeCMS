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
/// 
namespace EcubeWebPortalCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using EcubeWebPortalCMS.Common;
    public partial class EventBookingForSponsorModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int EventId { get; set; }
        public string SponsorName { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string BookDate { get; set; }
        public string GuestImage { get; set; }
    }
}