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
    public class EventBookingForPrintTokenModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int SeriolNo { get; set; }
        public string MemberNo { get; set; }
        public string Member_Name { get; set; }
        public string MainMember { get; set; }
        public string Type { get; set; }
        public string MemberType { get; set; }
        public string Relation { get; set; }
        public string Date { get; set; }
        public string ReceiptNo { get; set; }
        public string Photo { get; set; }
        public string QR { get; set; }        

        public List<SelectListItem> TicketType { get; set; }
        public List<EventBookingForPrintTokenModel> PrintList { get; set; }

    }
}