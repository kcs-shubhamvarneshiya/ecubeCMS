using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EcubeWebPortalCMS.Common
{
    

    public enum Common
    {
        [Description("Banquet/Hall Master")]
        BanquetHallMaster,

        [Description("Banquet/Hall Request")]
        BanquetHallRequest,

        [Description("Banquet/Room Configuration")]
        BanquetRoomConfiguration,

        [Description("Banquet/Room Email Log")]
        BanquetRoomEmailLog,

        [Description("Banquet/Room SMS Log")]
        BanquetRoomSMSLog,

        [Description("Room Master")]
        RoomMaster,

        [Description("Room Request")]
        RoomRequest,

        [Description("Create Event")]
        CreateEvent,

        [Description("Event Booking Report")]
        EventBookingReport,

        [Description("Event Category")]
        EventCategory,

        [Description("Event Ticket")]
        EventTicket,

        [Description("Event Ticket Category")]
        EventTicketCategory,

        [Description("Feedback Questionnaire")]
        FeedbackQuestionnaire,

        [Description("Club Facility")]
        ClubFacility,

        [Description("Club Facility Category")]
        ClubFacilityCategory,

        [Description("Club Information")]
        ClubInformation,

        [Description("Mobile Notification")]
        MobileNotification,

        [Description("Mobile Post")]
        MobilePost,

        [Description("Create Movie")]
        CreateMovie,

        [Description("Movie Show")]
        MovieShow,

        [Description("Movie Show Assign")]
        MovieShowAssign,

        [Description("Movie Show Report")]
        MovieShowReport,

        [Description("Movie Theatre")]
        MovieTheatre,

        [Description("Movie Ticket")]
        MovieTicket,

        [Description("Movie Booking Report")]
        MovieBookingReport,

        [Description("Mobile Menu")]
        MobileMenu,

        [Description("Mobile Banner")]
        MobileBanner,

        [Description("Inquiry Category")]
        InquiryCategory,
    }
}