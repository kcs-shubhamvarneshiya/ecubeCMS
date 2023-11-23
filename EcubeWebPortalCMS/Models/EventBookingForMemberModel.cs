using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public class EventBookingForMemberModel
    {
        //public List<SelectListItem> LstEvent { get; set; }      
        public List<SelectListItem> MemberCodeList { get; set; }
        public List<SelectListItem> PaymentList { get; set; }
       // public List<EventSeatAvailability> EventSeatAvailability { get; set; }

        public int Id { get; set; }        
        public string Name { get; set; }

        public int MemberNo { get; set; }
        public int MainMemberId { get; set; }
        public string MemberName { get; set; }
        public string BookDate { get; set; }        

        public string MemberID { get; set; }           
        public string Image { get; set; }
        public string Relation { get; set; }
        public int Age { get; set; }
        public decimal Amount { get; set; }


        public decimal BalanceAmount { get; set; }
        public string PaymentMode { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentId { get; set; }
        public string PaymentName { get; set; }        
        public string ChequeNo { get; set; }
        public string CardNo { get; set; }
        public string CardHolderName { get; set; }
        public string Date { get; set; }
        public string Branch { get; set; }
        public string BankInfo { get; set; }

        public int EventId { get; set; }
        public List<EventBookingForMemberModel> MemberDataList { get; set; }
    }
}