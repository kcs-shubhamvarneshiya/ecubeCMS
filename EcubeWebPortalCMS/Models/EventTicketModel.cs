using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcubeWebPortalCMS.Models
{
    public class EventTicketModel
    {
        public string EventTitle { get; set; }
        public int? EventBookingId { get; set; }
        public int EventId { get; set; }
        public string EventDate { get; set; }
        public string EventStartTime { get; set; }
        public string MemberCode { get; set; }
        public string UserPhoto1 { get; set; }
        public string QRCodePath { get; set; }
        public bool IsQRCode { get; set; }
        public string EventTicketCategoryName { get; set; }
        public int EventTicketNo { get; set; }
        public string PaymentType { get; set; }
        public string UserName { get; set; }
    }
}