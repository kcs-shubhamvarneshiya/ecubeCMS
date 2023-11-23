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
    public partial class EventBookingForParentsModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }

        public decimal Balance { get; set; }

        public string MemberName { get; set; }
        public string ParentName { get; set; }
        public string ParentRelation { get; set; }
        public string GuestImage { get; set; }
        public decimal Amount { get; set; }
        public int EventId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentName { get; set; }
        public string ChequeNo { get; set; }
        public string CardNo { get; set; }
        public string CardHolderName { get; set; }
        public string Date { get; set; }
        public string Branch { get; set; }
        public string BankInfo { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingDate { get; set; }
        public List<EventBookingForParentsModel> GuestList { get; set; }

        /// <summary>
        /// Gets or sets the relationship list.
        /// </summary>
        /// <value>The relationship list.</value>
        public List<SelectListItem> RelationshipList { get; set; }

        /// <summary>
        /// Gets or sets the member list.
        /// </summary>
        /// <value>The member list.</value>
        public List<SelectListItem> MemberList { get; set; }
        public List<SelectListItem> PaymentList { get; set; }
    }
}