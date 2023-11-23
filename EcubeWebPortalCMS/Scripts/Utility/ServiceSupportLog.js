jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });
    
    LoadSupportLogGrid();
});

function LoadSupportLogGrid() {
    
    jQuery('#txtServiceSupportLogSearch').on('keyup', function (e) {
        var postData = jQuery('#tblSupportLog').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtServiceSupportLogSearch').val().trim();        
        jQuery('#tblSupportLog').jqGrid("setGridParam", { search: true });        
        jQuery('#tblSupportLog').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#ddlstatus').on('change', function () {
      
        var postData = jQuery('#tblSupportLog').jqGrid("getGridParam", "postData");
        postData.status = jQuery('#ddlstatus').val();
        jQuery('#tblSupportLog').jqGrid("setGridParam", { status: true });
        jQuery('#tblSupportLog').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#tblSupportLog').jqGrid({
        url: '/ServiceSupportLog/BindSupportLogGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtServiceSupportLogSearch').val().trim(), status: jQuery('#ddlstatus').val() },
        mtype: 'GET',
        colNames: [
            'Request#', 'Support Type', 'Request Detail','Member', 'Status', 'Modified By', 'Date'],
        colModel: [
            { name: 'ServiceSupportId', index: 'ServiceSupportId', align: 'left', key: true },
            { name: 'TypeName', index: 'TypeName', align: 'left' },
            { name: 'Description', index: 'Description', align: 'left' },
            { name: 'MemberCode', index: 'MemberCode', align: 'left' },
            { name: 'StatusDesc', index: 'StatusDesc', align: 'left'},            
            { name: 'FirstName', index: 'FirstName', align: 'left' },
            { name: 'CreatedOn', index: 'CreatedOn', align: 'left' }
        ],
        pager: jQuery('#dvSupportLogFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'ServiceSupportId',
        sortorder: 'desc',
        viewrecords: true,
        caption: 'List of Service Support Log',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblSupportLog').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblSupportLog').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        }
    });
    SetStyle();
}

function SetStyle() {
    $('#tblSupportLog').setGridWidth($('#dvSupportLog').width());
}
