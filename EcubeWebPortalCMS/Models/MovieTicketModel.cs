using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcubeWebPortalCMS.Models
{
    public class MovieTicketModel
    {
        public string MovieTheatreName { get; set; }
        public int? TicketId { get; set; }
        public int? MovieBookingId { get; set; }
        public DateTime ShowDate { get; set; }
        public string SHowTime { get; set; }
        public string Screen { get; set; }
        public decimal FinalPrice { get; set; }
        public string MovieName { get; set; }
        public string Seats { get; set; }
        public string PANNumber { get; set; }
        public string MemberName { get; set; }
        public string ClubName { get; set; }
        public int NoOfSeat { get; set; }
        public string ClassName { get; set; }
        public string MemberCode { get; set; }
        public string ShowKeyDate { get; set; }
        public string StartTime { get; set; }
        public int? MemberId { get; set; }

        public decimal GuestCount { get; set; }
        public decimal MemberCount { get; set; }
        public decimal GuestRate { get; set; }
        public decimal MemberRate { get; set; }
        public string Email;
        public string QRCodeImage;
    }
}