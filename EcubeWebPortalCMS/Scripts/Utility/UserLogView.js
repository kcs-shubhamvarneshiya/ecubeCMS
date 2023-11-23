jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });
    LoadUserLogGrid();
});
function LoadUserLogGrid() {

    jQuery('#txtUserLogSearch').on('keyup', function (e) {
        var postData = jQuery('#tblUserLog').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtUserLogSearch').val().trim();
        jQuery('#tblUserLog').jqGrid("setGridParam", { search: true });
        jQuery('#tblUserLog').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblUserLog').jqGrid({
        url: '/UserLog/BindUserLogGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtUserLogSearch').val().trim() },
        mtype: 'GET',
        colNames: [
            'Id', 'User Name', 'Page Id', 'Action', 'Ip Address', 'Access Type', 'Location', 'Access On', 'Created On'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'UserName', index: 'UserName', align: 'left' },
            { name: 'PageId', index: 'PageId', align: 'right', formatter: 'integer', hidden: true },
            { name: 'Action', index: 'Action', align: 'left' },
            { name: 'IpAddress', index: 'IpAddress', align: 'left' },
            { name: 'AccessType', index: 'AccessType', align: 'left' },
            { name: 'Location', index: 'Location', align: 'left' },
            { name: 'AccessOn', index: 'AccessOn', align: 'left' },
            { name: 'CreatedOn', index: 'CreatedOn', align: 'left' }
        ],
        pager: jQuery('#dvUserLogFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'CreatedOn',
        sortorder: 'desc',
        viewrecords: true,
        caption: 'List of User Log',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblUserLog').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblUserLog').prev()[0].innerHTML = '';
            }
        }
    });

    SetStyle();
}

function SetStyle() {
    $('#tblUserLog').setGridWidth($('#dvUserLog').width());
}
