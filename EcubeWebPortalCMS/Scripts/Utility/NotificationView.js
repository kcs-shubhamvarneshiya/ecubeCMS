var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    $('#txtNotificationSearch').hide();
    blAdd = jQuery('#hfAdd').val();
    $("#Notification").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    LoadNotificationGrid();

    $(window).bind('resize', function () {
        SetStyle();
    });

    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'Notification', ('#txtNotificationSearch').val().trim());
    });

    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Notification', ('#txtNotificationSearch').val().trim());
    });
});

$('#txtsearch').on('click', function () {
    $('#txtNotificationSearch').toggle('slow');
    $('#txtsearch').toggleClass('close');
});

//$('#txtsearch').on('mouseover', function () {
//    $('#txtNotificationSearch').show();
//});
//$('#txtsearch').on('mouseout', function () {
//    $('#txtNotificationSearch').hide();
//});

function LoadNotificationGrid() {

    jQuery('#txtNotificationSearch').on('keyup', function (e) {
        var postData = jQuery('#tblNotification').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtNotificationSearch').val().trim();
        jQuery('#tblNotification').jqGrid("setGridParam", { search: true });
        jQuery('#tblNotification').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblNotification').jqGrid({
        url: '/Notification/BindNotificationGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtNotificationSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'Title', 'Description'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'NotificationTitles', index: 'NotificationTitles', align: 'left', width: '100px' },
            { name: 'Description', index: 'Description', align: 'left', width: '650px' }
        ],
        pager: jQuery('#dvNotificationFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Id',
        sortorder: 'DESC',
        viewrecords: true,
        caption: 'List of Notification',
        height: '100%',
        width: '100%',
        shrinkToFit: false,
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblNotification').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblNotification').prev()[0].innerHTML = '';
            }
        },
      
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
            i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
            cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

    SetStyle();
}

function SetStyle() {
    $('#tblNotification').setGridWidth($('#dvNotification').width());
}