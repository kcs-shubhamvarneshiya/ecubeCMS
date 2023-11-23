var blAddSchedule, blEditSchedule, blDeleteSchedule, blAddRate, blEditRate, blDeleteRate;

$(document).ready(function () {
    $("#EventBookingForMember").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    /* start EventMemberDDL*/
    $(function () {
        $.ajax({
            type: "GET",
            url: "/EventBookingForTicket/GetMemberCodeDropdown",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    if (parseInt(selectedMemberNo) != value.Value)
                        $('#MemberNoId').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    else
                        $('#MemberNoId').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#MemberNoId").val($('#hfdMemberNoId').val());
            }
        });
    });

    $("#MemberNoId").change(function () {
        $("#hfdMemberNoId").val(this.value);
    });
    if ($("#MemberNoId").val() == "") {
        $("#MemberNoId").val($("#hfdMemberNoId").val());
    }

    /* END EventMemberDDL*/

    $('#txtEventBookingSearch').hide();

    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'EventBooking', ('#txtEventBookingSearch').val().trim());
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'EventBooking', ('#txtEventBookingSearch').val().trim());
    });

    $("#dvEvent").show();
    if ($('#hfdId').val() == "0") {
        $("#dvSchedule").hide();
        $("#dvRate").hide();
    }
    else {
        LoadEventScheduleGrid();
        LoadEventRateGrid();
    }


});



$('#txtsearch').on('click', function () {
    $('#txtEventBookingSearch').toggle('slow');
    $('#txtsearch').toggleClass('close');
});

$("#chkEventMember").click(function () {
    alert('hi');
    var postData = jQuery('#tblEventMember').jqGrid("getGridParam", "postData");
    postData.search = jQuery('#txtEventBookingSearch').val().trim();
    postData.compeletedEvent = $("#chkEventMember").is(':checked');
    jQuery('#tblEventMember').jqGrid("setGridParam", { search: true });
    jQuery('#tblEventMember').jqGrid("setGridParam", { compeletedEvent: true });
    jQuery('#tblEventMember').trigger("reloadGrid", [{ page: 1, current: true }]);
    //SetStyle();
});




function ValidateEvent() {
    if ($('#MemberNoId').val() == null || $('#MemberNoId').val() == "" || $('#MemberNoId').val() == 0) {
        jAlert(kcs_Message.SelectRequired('Member No'), 'MemberNoId');
        return false;
    }
}

function SelectedIndexChangedMemberNoId() {
    $.ajax({
        type: "GET",
        url: "/EventBookingForTicket/GetEventDateDropdown",
        datatype: "Json",
        data: { EventId: $("#hdnEventID").val(), MemberId: $('#MemberNoId').val() },
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
    $("#tblEventMember").GridUnload();
    jQuery('#tblEventMember').jqGrid({
        url: '/EventBookingForTicket/BindEventGrid/',
        datatype: 'json',
        postData: { memberid: $('#MemberNoId').val(), eventid: $("#hdnEventID").val(), BookingDate: $('#BookingDate').val() },
        mtype: 'GET',
        colNames: [
            'No', 'Id', 'MemberName', 'Photo', 'MemberFees', 'Relation', 'Age', 'CurrentOutstanding', 'IsMaximumTicketForFamilyMember', 'MaximumTicketForFamilyMember', 'Booked', 'CuurentBooking', 'NoOfSeats', 'CurrentTicketNumber', 'ToSerialNo'],
        colModel: [
            { name: 'No', index: 'No', align: 'left', key: true, hidden: true },
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'MemberName', index: 'MemberName', align: 'left', width: '170px' },
            { name: 'Photo', index: 'Photo', align: 'left', width: '100px', formatter: formatImage },
            { name: 'MemberFees', index: 'MemberFees', align: 'left', width: '80px' },
            { name: 'Relation', index: 'Relation', align: 'left', width: '50px' },
            { name: 'Age', index: 'Age', align: 'left', width: '50px' },
            { name: 'CurrentOutstanding', index: 'CurrentOutstanding', align: 'Center', hidden: true },
            { name: 'IsMaximumTicketForFamilyMember', index: 'IsMaximumTicketForFamilyMember', align: 'Center', key: true, hidden: true },
            { name: 'MaximumTicketForFamilyMember', index: 'MaximumTicketForFamilyMember', align: 'Center', key: true, hidden: true },
            { name: 'Booked', index: 'Booked', align: 'Center', hidden: true },
            { name: 'CuurentBooking', index: 'CuurentBooking', align: 'Center', hidden: true },
            { name: 'NoOfSeats', index: 'NoOfSeats', align: 'Center', hidden: true },
            { name: 'CurrentTicketNumber', index: 'CurrentTicketNumber', align: 'Center', hidden: true },
            { name: 'ToSerialNo', index: 'ToSerialNo', align: 'Center', hidden: true }
        ]
        ,
        //pager: jQuery('#dvEventBookingFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Id',
        sortorder: 'desc',
        //viewrecords: true,
        caption: 'List of Configuration',
        height: '100%',
        width: '100%',
        multiselect: true,
        multiselectWidth: 50,
        shrinkToFit: false,
        selectChange: function (evt, ui) {
            address = this.selection().address();

        },

        loadComplete: function (data) {

            if (data.records == 0) {
                $('#BalanceAmount').val('');
                $('#txtTotal').val('');
                $('#txth5').text('');
                $('#txth6').text('');
                $('#tblEventMember').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
                $("#btnSubmit").show()
            }
            else {

                if (data.rows[0].ProceedFlag == 1) {
                    $("#btnSubmit").hide()
                    jAlert("Member Outstanding limit is Exceeded!");
                }
                else {
                    $("#btnSubmit").show()
                }

                $('#txtTotal').val('');
                $('#txth5').text('');
                $('#txth6').text('');
                var gridData = $("#tblEventMember").jqGrid('getRowData');
                var CurrentOutstanding
                var NoOfSeats
                var CuurentBooking
                var CurrentTicketNumber
                var ToSerialNo

                var bc = 0
                for (var i = 0; i < gridData.length; i++) {
                    var rowData = gridData[i];
                    CurrentOutstanding = rowData["CurrentOutstanding"];
                    NoOfSeats = rowData["NoOfSeats"];
                    CuurentBooking = rowData["CuurentBooking"];
                    CurrentTicketNumber = rowData["CurrentTicketNumber"];
                    ToSerialNo = rowData["ToSerialNo"];

                    if (rowData["Booked"] == 1) {
                        bc = bc + 1;
                        //$("#cb_tblEventMember").attr("disabled", "disabled");                        
                        //var cbs = $("tr.jqgrow > td > input.cbox", i);                        
                        $("#cb_tblEventMember").remove();
                        var cbs = $("tr.jqgrow:eq(" + i + ") > td > input.cbox")
                        cbs.remove();
                    }
                }


                var text = '';
                var text2 = '';
                if (NoOfSeats != undefined && NoOfSeats != '' && ToSerialNo != '' && ToSerialNo != undefined) {
                    text = ' * Day Booking limit is ' + NoOfSeats + ' and  Current day already booking is ' + CuurentBooking + '.'
                    text2 = ' * Last Ticket serial no. is ' + ToSerialNo + ' and Last booked Ticket serial no. is ' + CurrentTicketNumber + '.'
                }
                $('#txth5').text(text);
                $('#txth6').text(text2);
                $('#hdnBookedCount').val(bc);
                $('#BalanceAmount').val(CurrentOutstanding);
                $('#tblEventMember').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var total = parseFloat($('#txtTotal').val()) || 0;
            total = 0;
            if (iCol == 0) { // 2 is the index of the is_active column
                var isChecked1 = $(e.target).is(':checked');
                var rowData = $('#tblEventMember').getRowData(rowid);

                jQuery.uniform.update(jQuery('input:checkbox.cbox'));
                var ids = $("#tblEventMember").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {

                    var GetValue = 'input#jqg_tblEventMember_' + ids[i] + '.cbox';
                    var isChecked = $(GetValue).is(':checked');

                    var rowData1 = $('#tblEventMember').getRowData(ids[i]);
                    if (isChecked) {
                        total += parseFloat(rowData1.MemberFees);
                    }
                }
                $('#txtTotal').val(total.toFixed(2));
            }
        },
        onSelectAll: function (aRowids, status) {

            jQuery.uniform.update(jQuery('input:checkbox.cbox'));

            var total = 0;
            var ids = $("#tblEventMember").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var rowData = $('#tblEventMember').getRowData(ids[i]);
                if (status == true) {
                    total += parseFloat(rowData.MemberFees);
                }
            }
            $('#txtTotal').val(total.toFixed(2));
        },
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
                i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
                cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

    function formatImage(cellValue, options, rowObject) {
        var imageHtml = "<img style='width: 80px; height: 90px;' src='" + cellValue + "'>";
        return imageHtml;
    }

    //if (blEdit.toLowerCase() == "false") {
    //    jQuery('#tblEventMember').jqGrid('hideCol', ['editoperation']);
    //}

    //if (blDelete.toLowerCase() == "false") {
    //    jQuery('#tblEventMember').jqGrid('hideCol', ['deleteoperation']);
    //}

    SetStyle();
}

function SetStyle() {
    $('#tblEventMember').setGridWidth($('#dvEventBooking').width());
}


$("#jqxgrid").bind('cellendedit', function (event) {


    if (event.args.value) {
        $("#jqxgrid").jqxGrid('selectrow', event.args.rowindex);
    }
    else {
        $("#jqxgrid").jqxGrid('unselectrow', event.args.rowindex);
    }
});

$('.Cancel').click(function () {
    window.location.href = 'EventBookingForEventList';
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


function RedirectOnView(id) {

    window.location.href = '../../EventBookingForTicket/EventBookingForTicketPrint?EMId=' + id;
}

$('#btnSubmit').click(function () {


    var mydata = new Array();
    var data = [];
    var SelectdIds = new Array()
    var total = 0;

    var rows = $('#tblEventMember tbody tr')

    rows.each(function () {
        var row = $(this)
        var checkbox = row.find('aria-selected');
        if (checkbox.context.ariaSelected === "true") {
            SelectdIds.push(row[0].id);
        }
    });

    if ($("#hdnEventID").val() == undefined || $("#hdnEventID").val() == 0) {
        jAlert("Please select event try again.");
        return;
    }

    if (SelectdIds.length <= 0) {
        jAlert("Please select atleast one member.");
        return;
    }

    var rowData = $('#tblEventMember').getRowData(SelectdIds[0])
    if (rowData.IsMaximumTicketForFamilyMember == false) {

        if ((SelectdIds.length + parseInt($('#hdnBookedCount').val())) > parseInt(rowData.MaximumTicketForFamilyMember)) {
            jAlert("Maximum " + rowData.MaximumTicketForFamilyMember + " ticket allowed for members.");
            kcs_Message.stopWait();
            return false;
        }
    }

    else if ($("#paymentName").val() == undefined || $("#paymentName").val() == 0) {
        jAlert("Please select Payment Mode.");
        return;
    }

    for (var i = 0; i < SelectdIds.length; i++) {
        var rowData1 = $('#tblEventMember').getRowData(SelectdIds[i]);
        const imagePath = rowData1.Photo.substring(rowData1.Photo.lastIndexOf('/') + 1)
        data[0] = {};
        data[0]['SNO'] = mydata.length + 1,
            data[0]['MemberID'] = rowData1.Id,
            data[0]['MemberName'] = rowData1.MemberName,
            data[0]['Image'] = imagePath.replace("\">", ""),
            data[0]['Relation'] = rowData1.Relation,
            data[0]['Age'] = rowData1.Age,
            data[0]['Amount'] = rowData1.MemberFees
        mydata.push(data[0]);

        total += parseFloat(rowData1.MemberFees);
    }


    if ($("#txtTotal").val() == undefined || $("#txtTotal").val() == null) {
        jAlert("Tatal amount not found.");
        return;
    }
    else if (total != parseFloat($("#txtTotal").val())) {
        jAlert("Tatal amount and Seleted member value does not match.");
        return;
    }
    else if (rowData.CuurentBooking == undefined && rowData.NoOfSeats == undefined) {
        jAlert("Current and Last booking count not found.");
        return;
    }
    else if ((mydata.length + parseInt(rowData.CuurentBooking)) > parseInt(rowData.NoOfSeats)) {
        jAlert("Booking day limit is over.");
        return;
    }


    var requestData = {};
    requestData.EventId = $("#hdnEventID").val();
    requestData.MainMemberId = $('#MemberNoId').val();
    requestData.BookDate = $("#BookingDate").val();
    requestData.BalanceAmount = parseFloat($("#BalanceAmount").val());
    requestData.TotalAmount = parseFloat($("#txtTotal").val());
    // Bank    
    if ($("#hdnPaymentType").val() == 'Other' || $("#hdnPaymentType").val() == 'Cheque') {
        requestData.Date = $("#Date").val();
    }
    else if ($("#hdnPaymentType").val() == 'Credit Card') {
        requestData.Date = $("#Date1").val();
    }
    requestData.ChequeNo = $("#RefrenceNo").val();
    requestData.Branch = $("#Branch").val();
    requestData.BankInfo = $("#BankInfo").val();
    requestData.CardNo = $("#CardNo").val();
    requestData.CardHolderName = $("#CardHolderName").val();
    requestData.PaymentId = $("#paymentName").val();
    requestData.MemberDataList = mydata;

    $.ajax({
        url: "/EventBookingForTicket/SaveEventBookingForMember",
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


