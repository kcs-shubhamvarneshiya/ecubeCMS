using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public interface IEventTicketCommand
    {
        List<EventTicketModel> GetEventBookingDetailResult(int? Id);
        List<MS_GetBookedEventBookingResult> GetBookedEventBookingResult(long? memberId, string eventBookingId, string memberCode);      
    }
}
