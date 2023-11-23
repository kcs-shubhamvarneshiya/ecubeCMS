var blAdd, blEdit, blDelete, BanquetType;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    if (blAdd.toLowerCase() != "false") {
        shortcut.add("Ctrl+A", function () {
            window.location.href = '../BanquetBookingRequest/BanquetBookingRequest';
        });
    }
    $("#banquetHallMaster").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    if (blDelete.toLowerCase() != "false") {
        shortcut.add("Delete", function () {
            DeleteItemBanquetBookingRequest();
        });
    }

    $(window).bind('resize', function () {
        SetStyle();
    });

    jQuery(function () {
        jQuery.ajax({
            type: "GET",
            url: "/BanquetBookingRequest/GetBanquetType",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    //if (parseInt(selectedBanquetType) != value.Value)
                    //    $('#ddlBanquetType').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    //else
                        $('#ddlBanquetType').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#ddlBanquetType").val($('#hdnddlBanquetType').val());
            }
        });
    });
    $("#ddlBanquetType").change(function () {
            $("#hdnddlBanquetType").val(this.value);
        });
    if ($("#ddlBanquetType").val() == "") {
        $("#ddlBanquetType").val($("#hdnddlBanquetType").val());
        BanquetType = this.value;
    }
      
    jQuery('#btnDeleteBanquetBookingRequest').on('click', function () {
        DeleteItemBanquetBookingRequest();
    });

    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'BanquetBookingRequest', ('#txtBanquetBookingRequestSearch').val().trim());
    });

    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'BanquetBookingRequest', ('#txtBanquetBookingRequestSearch').val().trim());
    });

    LoadBanquetBookingRequestGrid();

});

function LoadBanquetBookingRequestGrid() {
    jQuery('#txtBanquetBookingRequestSearch').on('keyup', function (e) {
        var postData = jQuery('#tblBanquetBookingRequest').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtBanquetBookingRequestSearch').val().trim();
        jQuery('#tblBanquetBookingRequest').jqGrid("setGridParam", { search: true });
        jQuery('#tblBanquetBookingRequest').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#ddlBanquetBookingRequestSorting').on('change', function () {
        var postData = jQuery('#tblBanquetBookingRequest').jqGrid("getGridParam", "postData");
        postData.sortedby = jQuery('#ddlBanquetBookingRequestSorting').val();
        jQuery('#tblBanquetBookingRequest').jqGrid("setGridParam", { sortedby: true });
        jQuery('#tblBanquetBookingRequest').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#ddlBanquetType').on('change', function () {
        var postData = jQuery('#tblBanquetBookingRequest').jqGrid("getGridParam", "postData");
        postData.BanquetType = jQuery('#ddlBanquetType').val();
        jQuery('#tblBanquetBookingRequest').jqGrid("setGridParam", { BanquetType: true });
        jQuery('#tblBanquetBookingRequest').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

      jQuery('#tblBanquetBookingRequest').jqGrid({
        url: '/BanquetBookingRequest/BindBanquetBookingRequestGrid/',
        datatype: 'json',
          postData: { search: jQuery('#txtBanquetBookingRequestSearch').val().trim(), sortedby: jQuery('#ddlBanquetBookingRequestSorting').val().trim(), banquetType: BanquetType  },
        mtype: 'GET',
        colNames: [
            'Request Id', 'StatusId', 'Status', 'Member/Guest Name', 'Member Id', 'Booking Type', 'Inquiry For', 'Email', 'Address', 'City', 'Mobile', 'Occasion', 'From Date', 'To Date', 'Banquet Type ID', 'Banquet Type', 'Remarks', 'Admin Remarks', 'View', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, width: '80px'},
            { name: 'StatusId', index: 'StatusId', align: 'left', hidden: true},
            { name: 'Status', index: 'Status', align: 'left', width: '70px' },
            { name: 'Name', index: 'Name', align: 'left', width: '150px' },
            { name: 'MemberId', index: 'MemberId', align: 'right', formatter: 'integer', hidden: true },
            { name: 'BookingType', index: 'BookingType', align: 'center', formatter: 'checkbox', hidden: true },
            { name: 'InquiryFor', index: 'InquiryFor', align: 'right', formatter: 'integer', hidden: true },
            { name: 'EMail', index: 'EMail', align: 'left', width: '220px' },
            { name: 'Address', index: 'Address', align: 'left', hidden: true },
            { name: 'City', index: 'City', align: 'left', hidden: true },
            { name: 'MobileNo', index: 'MobileNo', align: 'left', width: '100px' },
            { name: 'Occasion', index: 'Occasion', align: 'left', width: '100px' },
            { name: 'FromDate', index: 'FromDate', align: 'left', width: '80px' },
            { name: 'ToDate', index: 'ToDate', align: 'left', width: '100px' },
            { name: 'BanquetTypeId', index: 'BanquetTypeId', align: 'left', hidden: true },
            { name: 'BanquetType', index: 'BanquetType', align: 'left', width: '80px' },
            { name: 'Remarks', index: 'Remarks', align: 'left', hidden: true, },
            { name: 'AdminRemarks', index: 'AdminRemarks', align: 'left', hidden: true },
            { name: 'editoperation', index: 'editoperation', align: 'left', width: '60px', sortable: false, formatter: ViewFormatBanquetBookingRequest },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'left', width: '80px', sortable: false, formatter: DeleteFormatBanquetBookingRequest }
        ],
        pager: jQuery('#dvBanquetBookingRequestFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Name',
        sortorder: 'desc',
        viewrecords: true,
        caption: 'List of Banquet Booking Request',
        height: '100%',
        width: '100%',
        multiselect: true,
        multiselectWidth: 50,
        shrinkToFit: false,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../BanquetBookingRequest/BanquetBookingRequest?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblBanquetBookingRequest').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblBanquetBookingRequest').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        },
        onSelectAll: function (aRowids, status) {
            jQuery.uniform.update(jQuery('input:checkbox.cbox'));
        },
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
            i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
            cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

    if (blEdit.toLowerCase() == "false") {
        jQuery('#tblBanquetBookingRequest').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblBanquetBookingRequest').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemBanquetBookingRequest(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblBanquetBookingRequest').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Banquet Booking Request'));
            return false;
        }
        for (var i = 0; i < selRowIds.length; i++) {
            if (i == 0) {
                objId = selRowIds[i];
            }
            else {
                objId += ',' + selRowIds[i];
            }
        }
    }

    jConfirm(kcs_Message.DeleteConfirm('Banquet Booking Request'), function (r) {
        if (r) {
            jQuery.post("/BanquetBookingRequest/DeleteBanquetBookingRequest/", { strBanquetBookingRequestId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblBanquetBookingRequest').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
            }
        });
        return false;
    }

function ViewFormatBanquetBookingRequest(cellvalue, options, rowObject) {
    return "<a href='../BanquetBookingRequest/BanquetBookingRequest?" + options.rowId + "'><label class='IconView' title='View' alt='' /><svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24'><path d='M15 12c0 1.654-1.346 3-3 3s-3-1.346-3-3 1.346-3 3-3 3 1.346 3 3zm9-.449s-4.252 7.449-11.985 7.449c-7.18 0-12.015-7.449-12.015-7.449s4.446-6.551 12.015-6.551c7.694 0 11.985 6.551 11.985 6.551zm-7 .449c0-2.761-2.238-5-5-5-2.761 0-5 2.239-5 5 0 2.762 2.239 5 5 5 2.762 0 5-2.238 5-5z'></path></svg></a>";
}

function DeleteFormatBanquetBookingRequest(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemBanquetBookingRequest(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black' ></path><rect x='7' y='11' width='8' height='2' fill='black'></rect><rect x='7' y='15' width='8' height='2' fill=''black'></rect></svg></a>";
}

function SetStyle() {
    $('#tblBanquetBookingRequest').setGridWidth($('#dvBanquetBookingRequest').width());
}
