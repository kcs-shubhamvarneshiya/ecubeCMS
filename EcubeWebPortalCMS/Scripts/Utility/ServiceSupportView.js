var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadServiceSupportGrid();
});

function LoadServiceSupportGrid() {
    jQuery('#txtServiceSupportSearch').on('keyup', function (e) {
        var postData = jQuery('#tblServiceSupport').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtServiceSupportSearch').val().trim();
        jQuery('#tblServiceSupport').jqGrid("setGridParam", { search: true });
        jQuery('#tblServiceSupport').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblServiceSupport').jqGrid({
        url: '/ServiceSupport/BindServiceSupportGrid/',
        postData: { search: jQuery('#txtServiceSupportSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Request#', 'Support Type', 'Request Detail', 'Member', 'Status', 'Date', 'Reply', 'Print'],
        colModel: [
            { name: 'id', index: 'id', align: 'left' },
            { name: 'TypeName', index: 'TypeName', align: 'left' },
            { name: 'Description', index: 'Description', align: 'left' },
             { name: 'Member', index: 'Member', align: 'left' },
             { name: 'StatusDesc', index: 'StatusDesc', align: 'left' },
              { name: 'CreatedOn', index: 'CreatedOn', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'left', width: 40, sortable: false, formatter: EditFormat },
            { name: 'printoperation', index: 'printoperation', align: 'left', width: 40, sortable: false, formatter: PrintFormat }
        ],
        pager: jQuery('#dvServiceSupportFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Id',
        sortorder: 'desc',
        viewrecords: true,
        multiselect: false,
        caption: 'List of ServiceSupport',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../ServiceSupport/ServiceSupport?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblServiceSupport').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblServiceSupport').prev()[0].innerHTML = '';
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
        jQuery('#tblServiceSupport').jqGrid('hideCol', ['editoperation']);
    }

    SetStyle();
}

function SetStyle() {
    $('#tblServiceSupport').setGridWidth($('#dvServiceSupport').width());
}

function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../ServiceSupport/ServiceSupport?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a>";
}

function PrintFormat(cellvalue, options, rowObject) {
    return "<a href='../ServiceSupport/ServiceSupportReport?" + options.rowId + "'><label class='IconPrint' title='Print' alt=''/></a>";
}