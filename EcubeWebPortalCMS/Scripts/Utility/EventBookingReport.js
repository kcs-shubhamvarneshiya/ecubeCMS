jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadEventReportGrid();
    $("#dvEventReport").hide();

    $("#EventBookingReport").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    $("#EventDate").change(function () {
        if ($('#EventDate').val() != '') {
            var date = $('#EventDate').val().split("/");
            EDate = date[1] + "/" + date[0] + "/" + date[2];
            $.ajax({
                type: "GET",
                url: "/EventBooking/GetAllEventForReportByEventDate",
                data: {
                    eventDate: EDate
                },
                datatype: "Json",
                success: function (data) {
                    if (data.length > 0) {
                        $('#ddlEvent').html('');
                        $('#ddlEvent').append('<option value="0">Select Event</option>');
                        $.each(data, function (index, value) {
                            $('#ddlEvent').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                        });
                    }
                }
            });
        }
        else {
            $('#ddlEvent').html('');
            $('#ddlEvent').append('<option value="0">Select Event</option>');
        }
    });
});
function validation() {
    if ($('#EventDate').val() == '') {
        jAlert(kcs_Message.InputRequired('Date'), 'EventDate');
        return false;
    }
    else if (parseInt($('#ddlEvent').val()) == 0) {
        jAlert(kcs_Message.SelectRequired('Event'), 'ddlEvent');
        return false;
    }
    //else if ($('#TicketId').val() == '' && $('#MemberCode').val() == '') {
    //    jAlert(kcs_Message.InputRequired('Serial Id Or Member Code'), 'ddlEvent');
    //    return false;
    //}
    return true;
}
function LoadEventReportGrid() {

    jQuery('#btnSearch').click(function (e) {
        if (validation()) {
            $("#dvEventReport").show();
            var date = $('#EventDate').val().split("/");
            var EDate = date[1] + "/" + date[0] + "/" + date[2];

            var postData = $('#tblEventReport').jqGrid("getGridParam", "postData");
            postData.eventId = parseInt($('#ddlEvent').val()),
                postData.ticketId = parseInt($('#TicketId').val()),
                postData.memberCode = $('#MemberCode').val(),
                postData.eventDate = EDate,
                $('#tblEventReport').jqGrid("setGridParam", { EventId: true });
            $('#tblEventReport').jqGrid("setGridParam", { TicketId: true });
            $('#tblEventReport').jqGrid("setGridParam", { MemberCode: true });
            $('#tblEventReport').trigger("reloadGrid", [{ page: 1, current: true }]);
            SetStyle();
        }
        else {
            $("#dvEventReport").hide();
        }
    });
    var EDate = '';
    if ($('#EventDate').val() != '') {
        var date = $('#EventDate').val().split("/");
        EDate = date[1] + "/" + date[0] + "/" + date[2];
    }
    jQuery('#tblEventReport').jqGrid({
        url: '/EventBooking/BindEventReportGrid/',
        datatype: 'json',
        postData: {
            eventId: parseInt(jQuery('#ddlEvent').val()),
            ticketId: parseInt(jQuery('#TicketId').val()),
            memberCode: jQuery('#MemberCode').val(),
            eventDate: EDate
        },
        mtype: 'GET',
        colNames: [
            'ID', 'Serial Id', 'Member Code', 'Member Name', 'Member Type', 'Checked In?', 'Checked In Time', 'Checked In By'],
        colModel: [
            { name: 'ID', index: '', align: 'left', hidden: true },
            { name: 'BookingId', index: 'BookingId', align: 'left' },
            { name: 'MCodeWithPrefix', index: 'MCodeWithPrefix', align: 'left' },
            { name: 'MemberName', index: 'MemberName', align: 'left' },
            { name: 'MemberType', index: 'MemberType', align: 'left' },
            { name: 'IsCheckedIn', index: 'IsCheckedIn', align: 'left' },
            { name: 'CheckedInTime', index: 'CheckedInTime', align: 'left' },
            { name: 'CheckedInBy', index: 'CheckedInBy', align: 'left' }
        ],
        pager: jQuery('#dvEventReportFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Id',
        sortorder: 'desc',
        viewrecords: true,
        //rowList: [],
        //pgbuttons: false,
        //pgtext: null,
        //viewrecords: false,
        //sortname: '',
        //sortorder: 'DESC',
        //viewrecords: true,
        caption: 'Event Booking Report',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblEventReport').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblEventReport').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        }
    });
    SetStyle();
}

function SetStyle() {
    $('#tblEventReport').setGridWidth($('#dvEventReport').width());
}