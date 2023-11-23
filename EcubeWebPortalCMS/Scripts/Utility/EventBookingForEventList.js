var eventId = 0;
$(document).ready(function () {

    $("#EventBookingForMember").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
   // DisplayMessage('EventTicketCategoryView');
});

$("#RelationId").change(function () {
    $("#hfdRelationId").val(this.value);
});

function Redirect(Type) {
    //debugger;

     if (Type == 'Print') {
        $("#imgeventPrint").attr("href", "/EventBookingForTicket/EventBookingForPrintToken");
        $("#imgeventPrint").click();
        return;
    }

    if ($("#hidEventId").val() == undefined || $("#hidEventId").val() == null || $("#hidEventId").val() == '' ||  $("#hidEventId").val() == '0') {
        jAlert('Please select Event.');
        return;
    }
    else {
        if (Type == 'Member') {
            $("#imgeventMember").attr("href", "/EventBookingForTicket/EventBookingForMember?EventID=" + $("#hidEventId").val());
            $("#imgeventMember").click();
            return;
        }
        else if (Type == 'Guest') {
            $("#imgeventGuest").attr("href", "/EventBookingForTicket/EventBookingForGuest?EventID=" + $("#hidEventId").val());
            $("#imgeventGuest").click();
            return;
           
        }
        else if (Type == 'Parent') {
            $("#imgeventParent").attr("href", "/EventBookingForTicket/EventBookingForParents?EventID=" + $("#hidEventId").val());
            $("#imgeventParent").click();
            return;
        }
        else if (Type == 'Affiliated') {
            $("#imgeventAffiliated").attr("href", "/EventBookingForTicket/EventBookingForAffiliate?EventID=" + $("#hidEventId").val());
            $("#imgeventAffiliated").click();
            return;
        }
        else if (Type == 'Sponser') {
            $("#imgeventSponser").attr("href", "/EventBookingForTicket/EventBookingForSponsor?EventID=" + $("#hidEventId").val());
            $("#imgeventSponser").click();
            return;
        }
       
    }

}

function fillevetnFees(id) {
    eventId = id;
    $("#hidEventId").empty();    
    $("#hidEventId").val(id);
    //$(function () {
    //    $.ajax({
    //        type: "GET",
    //        url: "/EventBookingForTicket/GetEventFees?eventId=" + id,
    //        datatype: "Json",
    //        success: function (data) {
    //            $.each(data, function (index, value) {
    //                $('#EventFeesId').append('<option value="' + value.Value + '">' + value.Value + '</option>');
    //            });
    //            $("#EventFeesId").val($('#hfdEventFeesId').val());
    //        }
    //    });
    //});
}