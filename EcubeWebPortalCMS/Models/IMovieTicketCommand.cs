using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public interface IMovieTicketCommand
    {
        MovieTicketModel GetMovieTicketDetails(int? Id);
        List<MS_GetMemberBookedMovieTicketListResult> GetMemberBookedMovieTicketList(string id, string memberCode, string ticketNo);
        
    }
}