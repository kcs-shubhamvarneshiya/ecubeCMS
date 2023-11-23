using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EcubeWebPortalCMS.Models
{
    public interface IEventBookingForMemberCommand
    {
        List<SelectListItem> GetMemberCodeDropdown();

        /// <summary>
        /// Gets all Relation Ship for drop down.
        /// </summary>
        /// <returns>List&lt;SelectListItem&gt; get all Relation Ship for drop down.</returns>
        List<SelectListItem> GetRelationshipDropDown();

        List<SelectListItem> GetEventFeesDropDown(int eventId);

        List<MS_GetEventDateByIDResult> GetEventDateByID(int eventid,int memberid);
        List<GetMemberDetailsWithEventRateResult> SearchEvent(int memberid, int eventid,string bookingDate);
        List<GetMemberDetailsForGuestWithEventRateResult> GetEventRate(int memberid, int eventid, string bookingDate,int type);


        /// <summary>
        /// Events List.
        /// </summary>
        /// <returns>List of event.</returns>
        List<EventList> GetAllEvent();

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <param name="objSave">The object save.</param>
        long SaveEventBookingForUmed(EventBookingForGuestModel objSave);
        List<SelectListItem> GetAffiliateDropDown();
        List<SelectListItem> GetPaymentDropDown();
        string GetPaymentDropDown(string name);
        long SaveEventBookingForUmedMember(EventBookingForMemberModel objSave);
        long SaveEventBookingForUmedAffiliateClub(EventBookingForAffiliateModel objSave);
        long SaveEventBookingForUmedSponsor(EventBookingForSponsorModel objSave);
        long SaveEventBookingForUmedParents(EventBookingForParentsModel objSave);
        List<EventBookingPrintTokenResult> SearchEventPrintToken(int memberno, int serialNo,int tickettype, int EventBookingID, int EventEDID);
        List<GetMemberDetailsForParentWithEventRateResult> GetParentEventRate(int memberid, int eventid, string bookingDate);
        List<GetMemberDetailsForAffWithEventRateResult> GetAffiliateEventRate(int memberid, int eventid, string bookingDate);

        List<MS_GetEventBookingDetailbyEventIDResult> GetEventBookingDetailResult(int id);

        


        

    }
}
