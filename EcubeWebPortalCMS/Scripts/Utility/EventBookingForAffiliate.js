var mydata = new Array();
var names = ["SNO", "AffiliateClubName", "Balance",'City', "Photo", "Amount"];
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

$("#AffiliateId").change(function () {
    $("#AffiliateId").val(this.value);
    SelectedIndexChangedMemberNoId();
});

function LoadEventMemberGrid() {

    debugger;
    $.ajax({
        type: "GET",
        url: "/EventBookingForTicket/GetAffiliateEventRate",
        datatype: "Json",
        data: { MemberId: $('#MemberId').val(), EventId: parseInt($("#hdnEventID").val()), BookingDate: $('#BookingDate').val() },
        success: function (data) {
            $('#Balance').val('');
            //$('#txtTotal').val('');
            $('#Amount').val('');
            $('#txth5').text('');
            $('#txth6').text('');    
            $('#Balance').val(CurrentOutstanding);
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
                $('#Amount').val(GuestFees);
                $('#hdnGuestLimit').val(Guestlimit);
                $('#hdnToSerialNo').val(ToSerialNo);
                $('#hdnCuurentBooking').val(CuurentBooking);            
            }
        }
    });
    
}

$("#paymentName").on('change', function () {
    $.ajax({
        url: "/EventBookingForTicket/GetPaymentTypeDropdown",
        type: 'GET',
        contentType: 'application/json;',
        data: { name: $(this).find(":selected").text() },
        dataType: 'json',
        success: function (data) {
            $("#hdnPaymentType").val('')

            var currentDate = new Date();
            // Get the day, month, and year
            var day = currentDate.getDate();
            var month = currentDate.getMonth() + 1; // getMonth() returns 0-11, so add 1 to get the actual month
            var year = currentDate.getFullYear();
            // Create a formatted string with the date
            var dateString = (day < 10 ? "0" : "") + day + "/" + (month < 10 ? "0" : "") + month + "/" + year;

            // Set the formatted date string as the text of an HTML element using jQuery
            $("#myElement").text(dateString);
            if (data == 'Other' || data == 'Cheque') {
                $("#hdnPaymentType").val(data)

                if (data == 'Other') {
                    $("#RefrenceNo").attr("placeholder", "Refrence No");
                }
                else {
                    $("#RefrenceNo").attr("placeholder", "Cheque No");
                }
                $("#Date").val(dateString)
                $("#divOther").show();
                $("#divCard").hide();
            }
            if (data == 'Credit Card') {
                $("#hdnPaymentType").val(data)
                $("#Date1").val(dateString)
                $("#divOther").hide();
                $("#divCard").show();
            }
        }
    });
});

$('#btnSave').click(function () {
 
    if ($("#hdnEventID").val() == undefined || $("#hdnEventID").val() == 0) {
        jAlert("Please select event try again.");
        return;
    }

    if ($("#AffiliateId").val() == undefined || $("#AffiliateId").val() == 0) {
        jAlert("Please select Affiliate Member Club");
        return;
    }

    if ($('#BookingDate').val() == "" || $('#BookingDate').val() == undefined) {
        jAlert("Please Select Booking Date.");
        return;
    }

    if ($("#AffiliateMemberCode").val() == undefined || $("#AffiliateMemberCode").val() == 0) {
        jAlert("Please select Member Code.");
        return;
    }

    if ($("#MemberName").val() == undefined || $("#MemberName").val() == 0) {
        jAlert("Please select Affiliate Member Name.");
        return;
    }

    if ($("#City").val() == undefined || $("#City").val() == 0) {
        jAlert("Please select City.");
        return;
    }

    if ($("#Amount").val() == undefined || $("#Amount").val() == null) {
        jAlert("Tatal amount not found.");
        return;
    }

    if ($("#imgCapture")[0].src == "" || $("#imgCapture")[0].src == undefined) {
        jAlert("Please Capture Image.");
        return;
    }

    if ($("#paymentName").val() == undefined || $("#paymentName").val() == 0) {
        jAlert("Please select Payment Mode.");
        return;
    }

    //if (parseInt($('#hdnGuestLimit').val() >= 1)) {
    //    jAlert("Guest Limit Exceeded!");
    //    kcs_Message.stopWait();
    //    return false;
    //}
    else if ($('#hdnToSerialNo').val() == "" && $('#hdnCuurentBooking').val() == "") {
        jAlert("Current and Last booking count not found.");
        return;
    }
    else if ((1 + parseInt($('#hdnCuurentBooking').val())) > parseInt($('#hdnToSerialNo').val())) {
        jAlert("Booking day limit is over.");
        return;
    }

    var requestData = {};
    requestData.EventId = $("#hdnEventID").val();
    requestData.MemberId = $('#hfdMemberId').val();
    requestData.AffiliateMemberCode = $('#AffiliateMemberCode').val();
    requestData.MemberName = $('#MemberName').val();
    requestData.AffiliateMemberClubId = $('#AffiliateId').val();
    requestData.AffiliateMemberClubName = $('#AffiliateId').val();
    requestData.BookingDate = $("#BookingDate").val();
    requestData.City = $('#City').val();
    requestData.Amount = $("#Amount").val()
    requestData.GuestImage = $("#imgCapture")[0].src;

    //Bank
    requestData.PaymentId = $("#paymentName").val()
    if ($("#hdnPaymentType").val() == 'Other' || $("#hdnPaymentType").val() == 'Cheque') {
        requestData.Date = $("#Date").val();
    }
    else if ($("#hdnPaymentType").val() == 'Credit Card') {
        requestData.Date = $("#Date1").val();
    }

    requestData.ChequeNo = $("#ChequeNo").val()
    requestData.Branch = $("#Branch").val()
    requestData.BankInfo = $("#BankInfo").val()
    requestData.CardNo = $("#CardNo").val()
    requestData.CardHolderName = $("#CardHolderName").val()

    $.ajax({
        url: "/EventBookingForTicket/SaveEventBookingForUmedAffiliateClub",
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
                document.getElementById('popup_ok').setAttribute('onclick', 'RedirectOnView(' + data.id + ');');
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
