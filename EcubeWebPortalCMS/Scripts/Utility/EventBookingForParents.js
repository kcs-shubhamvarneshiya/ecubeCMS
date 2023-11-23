var mydata = new Array();
var names = ["SNO", "Name", "Relation", "Photo", "PaymentType","Balance", "Amount"];
var data = [];
var selectedMemberNo = 0;
$(document).ready(function () {
    $("#EventBookingForMember").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    
    $(function () {
        $.ajax({
            type: "GET",
            url: "/EventBookingForTicket/GetMemberCodeDropdown",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    if (parseInt(selectedMemberNo) != value.Value)
                        $('#MemberId').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    else
                        $('#MemberId').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#MemberId").val($('#hfdMemberNoId').val());
            }
        });
    });

    $("#MemberId").change(function () {
        $("#hfdMemberNoId").val(this.value);
    });
    if ($("#MemberId").val() == "") {
        $("#MemberId").val($("#hfdMemberNoId").val());
    }

    jQuery("#tblGuest").jqGrid({
        datatype: "local",
        height: 250,
        colNames: [" ", "Parent Name", "Relation", "GuestImage", "Amount","Action"],
        colModel: [{
            name: 'SNO',
            index: 'SNO',
            width: 60,
            hidden: true
        },
        {
            name: 'ParentName',
            index: 'ParentName',
            width: 90
        },
        {
            name: 'ParentRelation',
            index: 'Relation',
            width:80
        },
        {
            name: 'GuestImage',
            index: 'GuestImage',
            width: 80,
           formatter: function () {
                return "<img src='' id='img1' alt='my image' style = 'width: 50px; height: 50px;' />";
            }
        }, 
        {
            name: 'Amount',
            index: 'Amount',
            width: 80

            },
            { name: 'Delete', index: 'editoperation', align: 'left', sortable: false, width: '100px', formatter: DeleteFormatEventSchedule }
        ],
        caption: "Guest Event Details",
    });

   
    
    for (var i = 0; i < data.length; i++) {
        mydata[i] = {};
        for (var j = 0; j < data[i].length; j++) {
            mydata[i][names[j]] = data[i][j];
        }
    }

    for (var i = 0; i <= mydata.length; i++) {
        $("#tblGuest").jqGrid('addRowData', i + 1, mydata[i]);
    }

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


function SelectedIndexChangedBookingDate() {
    LoadEventMemberGrid();
}

function LoadEventMemberGrid(Refresh) {
  

    $.ajax({
        type: "GET",
        url: "/EventBookingForTicket/GetParentEventRate",
        datatype: "Json",
        data: { MemberId: $('#MemberId').val(), EventId: parseInt($("#hdnEventID").val()), BookingDate: $('#BookingDate').val() },
        success: function (data) {

            
            $('#Balance').val('');
            $('#txtTotal').val('');
            $('#Amount').val('');            
            $('#txth5').text('');
            $('#txth6').text('');
            $('#hdnGuestLimit').val('0');
            $('#hdnToSerialNo').val('');
            $('#hdnCuurentBooking').val('');
            $("#btnSave").show();
            $("#btnAddGuest").show();
            if (data.records == 0) {

            }
            else {

                if (data.rows[0].ProceedFlag == 1) {
                    $("#btnSave").hide();
                    $("#btnAddGuest").hide();
                    jAlert("Member Outstanding limit is Exceeded!");
                }
                else {
                    $("#btnSave").show();
                    $("#btnAddGuest").show();
                }

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
                $('#Balance').val(CurrentOutstanding);

                $('#hdnGuestLimit').val(Guestlimit);
                $('#hdnToSerialNo').val(ToSerialNo);
                $('#hdnCuurentBooking').val(CuurentBooking); 

            }

        }
    });
}



function DeleteFormatEventSchedule(cellvalue, options, rowObject) {


    return "<a href='javascript:void(0);' onclick='DeleteItemEventSchedule(\"" + rowObject.SNO + "\",\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";
}


function DeleteItemEventSchedule(objId, rowId) {

    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblGuest').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Event Schedule'));
            return false;
        }

    }

    jConfirm(kcs_Message.DeleteConfirm('Parent Record'), function (r) {
        if (r) {


            let index = mydata.findIndex(row => row.SNO === parseInt(objId));
            var total = 0;

            // remove the row from the array
            if (index !== -1) {
                mydata.splice(index, 1);
                $("#tblGuest").jqGrid('delRowData', rowId);
                $('#tblGuest').trigger('reloadGrid', [{ page: 1, current: true }]);

                for (var i = 0; i < mydata.length; i++) {
                    total = total + parseFloat(mydata[i].Amount);


                    //  $("#tblGuest").jqGrid('setCell', i + 2, 1, (i + 1).toString());

                }
            }
            $("#txtTotal").val(total.toString());

        }
    });
    return false;
}

function SelectedIndexChangedMemberNoId() {
    //debugger;
    $('#BookingDate').val("");
    $.ajax({
        type: "GET",
        url: "/EventBookingForTicket/GetEventDateDropdown",
        datatype: "Json",
        data: { EventId: $("#hdnEventID").val(), MemberId: $('#MemberId').val() },
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


$('#btnAddGuest').click(function () {

    if ($('#ParentName').val() == "" || $('#ParentName').val == undefined) {
        jAlert("Please Enter Parent Name.");
        return;
    }

    if ($('#BookingDate').val() == "" || $('#BookingDate').val() == undefined) {
        jAlert("Please Select Booking Date.");
        return;
    }

    if ($('#Amount').val() == "" || $('#Amount').val() == undefined) {
        jAlert("Please Enter Parent Fee.");
        return;
    }

    if ($('#hfdRelationId').val() == "" || $('#hfdRelationId').val() == undefined) {
        jAlert("Please Select Relation.");
        return;
    }

    if ($("#imgCapture")[0].src == "" || $("#imgCapture")[0].src == undefined) {
        jAlert("Please Capture Image.");
        return;
    }
    
            data[0] = {};
            data[0]['SNO']  = mydata.length + 1,
            data[0]['ParentName'] = $('#ParentName').val(),
            data[0]['ParentRelation'] = $('#hfdRelationId').val(),
            data[0]['GuestImage'] = $("#imgCapture")[0].src;        
            data[0]['Amount'] = $("#Amount").val(),

    mydata.push(data[0]);
    objRow = mydata.length + 1;
    $("#tblGuest").jqGrid('addRowData', mydata.length + 1, data[0]);
    for (var i = mydata.length - 1; i < mydata.length; i++) {
        jQuery('#tblGuest tr:eq(' + mydata.length + ')').find('#img1')[0].src = $("#imgCapture")[0].src
    }

    $('#ParentName').val("");
    $('#hfdRelationId').val("");
    $("#RelationId").val("0");    
    $("#BookingDate").attr("disabled", "disabled");
    $("#imgCapture").attr("src", "")[0];

    var amt = 0;
    var Famt = 0;
    amt = parseFloat($("#Amount").val());
    if ($("#txtTotal").val() == undefined || $("#txtTotal").val() == "") {
        Famt = 0
    }
    else {
        Famt = parseFloat($("#txtTotal").val())
    }

    Famt = Famt + amt
    $("#txtTotal").val(Famt.toString());
    
});

$('#btnSave').click(function () {  


    if ($("#MemberId").val() == undefined || $("#MemberId").val() == 0) {
        jAlert("Please select member");
        return;
    }
    if ($("#hdnEventID").val() == undefined || $("#hdnEventID").val() == 0) {
        jAlert("Please select event try again.");
        return;
    }
    if ($("#txtTotal").val() == undefined || $("#txtTotal").val() == null) {
        jAlert("Tatal amount not found.");
        return;
    }
    if ($("#paymentName").val() == undefined || $("#paymentName").val() == 0) {
        jAlert("Please select Payment Mode.");
        return;
    }

    if (mydata.length == 0) {
        jAlert("Please add atleast one Parent.");
        return;
    }

    var total = 0;
    for (var i = 0; i < mydata.length; i++) {
        total += parseFloat(mydata[i].Amount);
    }

    if (total != parseFloat($("#txtTotal").val())) {
        jAlert("Tatal amount and added Parent amount total does not match.");
        return;
    }


    if (parseInt($('#hdnGuestLimit').val() > mydata.length)) {
        jAlert("Guest Limit Exceeded!");
        kcs_Message.stopWait();
        return false;
    }
    else if ($('#hdnToSerialNo').val() == "" && $('#hdnCuurentBooking').val() == "") {
        jAlert("Current and Last booking count not found.");
        return;
    }
    else if ((mydata.length + parseInt($('#hdnCuurentBooking').val())) > parseInt($('#hdnToSerialNo').val())) {
        jAlert("Booking day limit is over.");
        return;
    }

    
    var requestData = {};
    requestData.EventId = $("#hdnEventID").val();
    requestData.MemberId = $('#MemberId').val();
    requestData.Balance = $("#Balance").val();
    requestData.BookingDate = $("#BookingDate").val();
    //requestData.MemberName = $('#MemberName').val();
    //requestData.ParentName = $('#ParentName').val();
    //requestData.ParentRelation = $("#RelationId option:selected").text();//$('#hfdRelationId').val();

    // Bank   
    if ($("#hdnPaymentType").val() == 'Other' || $("#hdnPaymentType").val() == 'Cheque') {
        requestData.Date = $("#Date").val();
    }
    else if ($("#hdnPaymentType").val() == 'Credit Card') {
        requestData.Date = $("#Date1").val();
    }
    //requestData.paymentName = $("#paymentName").val();
    requestData.PaymentId = $("#paymentName").val();
    requestData.ChequeNo = $("#ChequeNo").val();
    requestData.Branch = $("#Branch").val();
    requestData.BankInfo = $("#BankInfo").val();
    requestData.CardNo = $("#CardNo").val();
    requestData.CardHolderName = $("#CardHolderName").val();
    requestData.Amount = $("#txtTotal").val();
    requestData.GuestList = mydata;

        $.ajax({
            url: "/EventBookingForTicket/SaveEventBookingForParents",
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

   // }
});


function RedirectOnView(id) {
    window.location.href = '../../EventBookingForTicket/EventBookingForTicketPrint?EMId=' + id;
}

$('.Cancel').click(function () {
    window.location.href = 'EventBookingForEventList';
});

$("#RelationId").change(function () {    
    $("#hfdRelationId").val(this.value);
});



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