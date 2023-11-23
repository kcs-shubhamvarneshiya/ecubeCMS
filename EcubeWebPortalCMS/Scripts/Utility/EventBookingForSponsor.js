var mydata = new Array();
var data = [];
var selectedMemberNo = 0;
$(document).ready(function () {
    $("#EventBookingForMember").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

   
    $(function () {
            Webcam.set({
                width: 170,
                height: 170,
                image_format: 'jpeg',
                jpeg_quality: 90
            });
            Webcam.attach('#webcam');
            $("#btnCapture").click(function () {
                Webcam.snap(function (data_uri) {
                    $("#imgCapture")[0].src = data_uri;
                    $("#btnUpload").removeAttr("disabled");
                });
            });
            $("#btnUpload").click(function () {
                $.ajax({
                    type: "POST",
                    url: "CS.aspx/SaveCapturedImage",
                    data: "{data: '" + $("#imgCapture")[0].src + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) { }
                });
            });
        });
});


function SelectedIndexChangedMemberNoId() {
    $('#BookingDate').val("");
    $.ajax({
        type: "GET",
        url: "/EventBookingForTicket/GetEventDateDropdown",
        datatype: "Json",
        data: { EventId: $("#hdnEventID").val(), MemberId: 0 },
        success: function (data) {

            $.each(data, function (index, value) {
                if (parseInt(selectedMemberNo) != value.Value)
                    $('#BookingDate').append('<option value="' + value.EventDate + '">' + value.EventDate + '</option>');
                else
                    $('#BookingDate').append('<option value="' + value.EventDate + '" selected=true>' + value.EventDate + '</option>');
            });
            if ($("#BookingDate").val() != "") {
                LoadEventMemberGrid();
            }
        }
    });

}

function SelectedIndexChangedBookingDate() {
    LoadEventMemberGrid();
}


function LoadEventMemberGrid() {

    
    $.ajax({
        type: "GET",
        url: "/EventBookingForTicket/GetAffiliateEventRate",
        datatype: "Json",
        data: { MemberId: $('#MemberId').val(), EventId: parseInt($("#hdnEventID").val()), BookingDate: $('#BookingDate').val() },
        success: function (data) {
            //$('#Balance').val('');
            //$('#txtTotal').val('');
            //$('#Amount').val('');
            $('#txth5').text('');
            $('#txth6').text('');
            //$('#Balance').val(CurrentOutstanding);
            $('#hdnGuestLimit').val('0');
            $('#hdnToSerialNo').val('');
            $('#hdnCuurentBooking').val('');
            if (data.records == 0) {
            }
            else {
                var CurrentOutstanding
                var NoOfSeats
                var CuurentBooking
                var CurrentTicketNumber
                var ToSerialNo
                var GuestFees

                CurrentOutstanding = data.rows[0].CurrentOutstanding;
                GuestFees = data.rows[0].GuestFees;
                NoOfSeats = data.rows[0].NoOfSeats;
                CuurentBooking = data.rows[0].CuurentBooking;
                CurrentTicketNumber = data.rows[0].CurrentTicketNumber;
                ToSerialNo = data.rows[0].ToSerialNo;
                Guestlimit = data.rows[0].GuestLimit;

                var text = '';
                var text2 = '';
                if (NoOfSeats != undefined && NoOfSeats != '' && ToSerialNo != '' && ToSerialNo != undefined) {
                    text = ' * Day Booking limit is ' + NoOfSeats + ' and  Current day already booking is ' + CuurentBooking + '.'
                    text2 = ' * Last Ticket serial no.is ' + ToSerialNo + ' and Last booked Ticket serial no. is ' + CurrentTicketNumber + '.'
                }
                $('#txth5').text(text);
                $('#txth6').text(text2);
               // $('#Amount').val(GuestFees);
                $('#hdnGuestLimit').val(Guestlimit);
                $('#hdnToSerialNo').val(ToSerialNo);
                $('#hdnCuurentBooking').val(CuurentBooking);
            }
        }
    });

}

$('#btnSave').click(function () {


    if ($("#hdnEventID").val() == undefined || $("#hdnEventID").val() == 0) {
        jAlert("Please select event try again.");
        return;
    }
    if ($("#SponsorName").val() == undefined || $("#SponsorName").val() == 0) {
        jAlert("Please Enter Sponsor Name");
        return;
    }
    if ($("#CompanyName").val() == undefined || $("#CompanyName").val() == 0) {
        jAlert("Please Enter Company Name");
        return;
    }
    if ($('#BookingDate').val() == "" || $('#BookingDate').val() == undefined) {
        jAlert("Please Select Booking Date.");
        return;
    }
    if ($('#City').val() == "" || $('#City').val() == undefined) {
        jAlert("Please Select City.");
        return;
    }
    if ($("#imgCapture")[0].src == "" || $("#imgCapture")[0].src == undefined) {
        jAlert("Please Capture Image.");
        return;
    }
    else if ($('#hdnToSerialNo').val() == "" && $('#hdnCuurentBooking').val() == "") {
        jAlert("Current and Last booking count not found.");
        return;
    }
    else if ((1 + parseInt($('#hdnCuurentBooking').val())) > parseInt($('#hdnToSerialNo').val())) {
        jAlert("Booking day limit is over.");
        return;
    }

    var requestData = {};
    requestData.MemberId = $('#hfdMemberId').val();
    requestData.SponsorName = $('#SponsorName').val();
    requestData.CompanyName = $('#CompanyName').val();    
    requestData.City = $('#City').val();
    requestData.BookDate = $("#BookingDate").val();
    requestData.GuestImage = $("#imgCapture")[0].src;
    requestData.EventId = $("#hdnEventID").val();
        $.ajax({
            url: "/EventBookingForTicket/SaveEventBookingForUmedSponsor",
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            data: JSON.stringify(requestData),          
            success: function (data) {              
                kcs_Message.stopWait();
                if (data.message == 'Event Booking Submitted Successfully.') {
                    kcs_Message.stopWait();
                    $("#btnSave").removeAttr("disabled");
                    jAlert("Event ticket submitted successfully.");
                    document.getElementById('popup_ok').setAttribute('onclick', 'RedirectOnView(' + data.id +');');
                }
                else {
                    jAlert(data.message);
                }
            }
        });
});


function RedirectOnView(id) {
    window.location.href = '../../EventBookingForTicket/EventBookingForTicketPrint?EMId=' + id;
}

$('.Cancel').click(function () {
    window.location.href = 'EventBookingForEventList';
});

