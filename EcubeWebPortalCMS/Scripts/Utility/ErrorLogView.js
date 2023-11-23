jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });
    LoadErrorLogGrid();
});

function LoadErrorLogGrid() {
    jQuery('#txtErrorLogSearch').on('keyup', function (e) {
        var postData = jQuery('#tblErrorLog').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtErrorLogSearch').val().trim();
        jQuery('#tblErrorLog').jqGrid("setGridParam", { search: true });
        jQuery('#tblErrorLog').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblErrorLog').jqGrid({
        url: '/ErrorLog/BindErrorLogGrid/',
        postData: { search: jQuery('#txtErrorLogSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Page Name', 'Method Name', 'Error Type', 'Error Message', 'Error Details', 'Error Date', 'User Name'],
        colModel: [
            { name: 'PageName', index: 'PageName', align: 'left' },
            { name: 'MethodName', index: 'MethodName', align: 'left' },
            { name: 'ErrorType', index: 'ErrorType', align: 'left' },
            { name: 'ErrorMessage', index: 'ErrorMessage', align: 'left' },
            { name: 'ErrorDetails', index: 'ErrorDetails', align: 'left' },
            { name: 'ErrorDate', index: 'ErrorDate', align: 'left' },
            { name: 'UserName', index: 'UserName', align: 'left' }
        ],
        pager: jQuery('#dvErrorLogFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'ErrorDate',
        sortorder: 'DESC',
        viewrecords: true,
        caption: 'List of Error Log',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblErrorLog').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblErrorLog').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        }
    });
    SetStyle();
}

function SetStyle() {
    $('#tblErrorLog').setGridWidth($('#dvErrorLog').width());
}