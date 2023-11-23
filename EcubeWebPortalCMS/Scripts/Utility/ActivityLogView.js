jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });
    LoadActivityLogGrid();
});
function LoadActivityLogGrid() {

    jQuery('#txtActivityLogSearch').on('keyup', function (e) {
        var postData = jQuery('#tblActivityLog').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtActivityLogSearch').val().trim();
        jQuery('#tblActivityLog').jqGrid("setGridParam", { search: true });
        jQuery('#tblActivityLog').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblActivityLog').jqGrid({
        url: '/ActivityLog/BindActivityLogGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtActivityLogSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'User Name', 'Page Name', 'Audit Comments', 'Table Name', 'Record Id', 'Created On'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'UserName', index: 'UserName', align: 'left' },
            { name: 'PageName', index: 'PageName', align: 'left' },
            { name: 'AuditComments', index: 'AuditComments', align: 'left' },
            { name: 'TableName', index: 'TableName', align: 'left' },
            { name: 'RecordId', index: 'RecordId', align: 'right', formatter: 'integer', hidden: true },
            { name: 'CreatedOn', index: 'CreatedOn', align: 'left' },
        ],
        pager: jQuery('#dvActivityLogFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'CreatedOn',
        sortorder: 'desc',
        viewrecords: true,
        caption: 'List of Activity Log',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblActivityLog').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblActivityLog').prev()[0].innerHTML = '';
            }
        }
    });

    SetStyle();
}

function SetStyle() {
    $('#tblActivityLog').setGridWidth($('#dvActivityLog').width());
}
