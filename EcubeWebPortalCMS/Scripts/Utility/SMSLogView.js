jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });

    $("#SMSLog").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    jQuery(function () {
        jQuery.ajax({
            type: "GET",
            url: "/SMSLog/GetbookingType",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    //if (parseInt(selectedBookingType) != value.Value)
                    //    $('#ddlBookingType').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    //else
                    $('#ddlBookingType').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#ddlBookingType").val($('#hdnddlBookingType').val());
            }
        });
    });
    $("#ddlBookingType").change(function () {
        $("#hdnddlBookingType").val(this.value);
    });
    if ($("#ddlBookingType").val() == "") {
        $("#ddlBookingType").val($("#hdnddlBookingType").val());
    }

    LoadSMSLogGrid();
    LoadStatus();

});


function LoadStatus() {

    var dFormat = "dd/mm/yy";
    jQuery('.SearchDate').each(function () { //loop through each input
        jQuery("#" + this.id).mask('99/99/9999');
    });

    $(".SearchDate").datepicker({
        
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
        dateFormat: dFormat,
        beforeShow: function customRange(input) {
            if (input.id === 'txtFromDate') {
                var abc = $('#txtToDate').datepicker("getDate");
                return {
                    maxDate: abc == null ? 0 : $('#txtToDate').datepicker("getDate"),
                };
            } else if (input.id === 'txtToDate') {
                return {
                    minDate: $('#txtFromDate').datepicker("getDate"),
                };
            }
        }
    });

    jQuery('.SearchDate').on('blur', function (event) {
        var strChallanDate = '';

        if (jQuery(this).val() != null) {
            strChallanDate = jQuery(this).val().toString();

            if (strChallanDate != '' && strChallanDate != '__/__/____') {
                if (strChallanDate.length < 10) {
                    jQuery(this).val(''); //jAlert('Valid format dd/MM/yyyy.', jQuery(this).attr("id").toString());
                }
                else if (strChallanDate.charAt(2) != '/' || strChallanDate.charAt(5) != '/') {
                    //jAlert('Valid format dd/MM/yyyy.', jQuery(this).attr("id").toString());
                }
                else {
                    var year, month, day;
                    if (dFormat == "dd/mm/yy") {
                        year = parseInt(strChallanDate.substring(6, 10).toString());
                        month = parseInt(strChallanDate.substring(3, 5).toString()) - 1; // Months start with 0 in JavaScript - June
                        day = parseInt(strChallanDate.substring(0, 2).toString());
                    }
                    else if (dFormat == "mm/dd/yy") {
                        year = parseInt(strChallanDate.substring(6, 10).toString());
                        month = parseInt(strChallanDate.substring(0, 2).toString()) - 1; // Months because the months start with 0 in JavaScript - June
                        day = parseInt(strChallanDate.substring(3, 5).toString());
                    }
                    var myDate = new Date(year, month, day);
                    if ((myDate.getMonth() != month) || (myDate.getDate() != day) || (myDate.getFullYear() != year) || myDate.getFullYear() < 1753) {
                        //jAlert('Date is invalid.', jQuery(this).attr("id").toString());
                        //jQuery(this).val('');
                    }
                }
            }
        }
    });
}

function LoadSMSLogGrid() {

    jQuery('#btnSearch').click(function (e) {
        var StartDate = document.getElementById('txtFromDate').value;
        var EndDate = document.getElementById('txtToDate').value;
        var eDate = new Date(EndDate);
        var sDate = new Date(StartDate);
        if (StartDate != '' && StartDate != '' && sDate > eDate) {
            alert("Please ensure that the To Date is greater than or equal to the From Date.");
            return false;
        }

        var postData = jQuery('#tblSMSLog').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtSMSLogSearch').val().trim(),
        postData.bookingType = jQuery('#ddlBookingType').val(),
        postData.fromDate = jQuery('#txtFromDate').val().trim(),
        postData.toDate = jQuery('#txtToDate').val().trim(),
        postData.status = jQuery('#ddlStatus').val().trim()
        jQuery('#tblSMSLog').jqGrid("setGridParam", { search: true });
        jQuery('#tblSMSLog').jqGrid("setGridParam", { bookingType: true });
        jQuery('#tblSMSLog').jqGrid("setGridParam", { fromDate: true });
        jQuery('#tblSMSLog').jqGrid("setGridParam", { toDate: true });
        jQuery('#tblSMSLog').jqGrid("setGridParam", { status: true });
        jQuery('#tblSMSLog').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    //jQuery('#txtSMSLogSearch').on('keyup', function (e) {
    //    var postData = jQuery('#tblSMSLog').jqGrid("getGridParam", "postData");
    //    postData.search = jQuery('#txtSMSLogSearch').val().trim();
    //    jQuery('#tblSMSLog').jqGrid("setGridParam", { search: true });
    //    jQuery('#tblSMSLog').trigger("reloadGrid", [{ page: 1, current: true }]);
    //    SetStyle();
    //});

    //jQuery('#ddlBookingType').on('change', function () {
    //    var postData = jQuery('#tblSMSLog').jqGrid("getGridParam", "postData");
    //    postData.BookingType = jQuery('#ddlBookingType').val();
    //    jQuery('#tblSMSLog').jqGrid("setGridParam", { BookingType: true });
    //    jQuery('#tblSMSLog').trigger("reloadGrid", [{ page: 1, current: true }]);
    //    SetStyle();
    //});

    jQuery('#tblSMSLog').jqGrid({
        url: '/SMSLog/BindSMSLogGrid/',
        datatype: 'json',
        postData: {
            search: jQuery('#txtSMSLogSearch').val().trim(),
            bookingType: jQuery('#ddlBookingType').val(),
            fromDate: jQuery('#txtFromDate').val().trim(),
            toDate: jQuery('#txtToDate').val().trim(),
            status: jQuery('#ddlStatus').val().trim()
        },
        mtype: 'GET',
        colNames: [
            'Id', 'Relavent Id', 'Module Name', 'Mobile No', 'Text', 'Sent On', 'Status'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true, width: '80px' },
            { name: 'RelaventId', index: 'RelaventId', align: 'right', formatter: 'integer', hidden: true, width: '0px' },
            { name: 'ModuleName', index: 'ModuleName', align: 'left', width: '100px' },
            { name: 'MobileNo', index: 'MobileNo', align: 'left', width: '100px' },
            { name: 'text', index: 'text', align: 'left', width: '300px' },
            { name: 'SentOn', index: 'SentOn', align: 'left', width: '150px' },
            { name: 'Status', index: 'Status', align: 'left', width: '80px' }
        ],
        pager: jQuery('#dvSMSLogFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'SentOn',
        sortorder: 'DESC',
        viewrecords: true,
        caption: 'List of SMS Log',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblSMSLog').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblSMSLog').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        }
    });
    SetStyle();
}

function SetStyle() {
    $('#tblSMSLog').setGridWidth($('#dvSMSLog').width());
}
