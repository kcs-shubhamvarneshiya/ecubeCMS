var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {

    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    if (blAdd.toLowerCase() != "false") {
        shortcut.add("Ctrl+A", function () {
            window.location.href = '../DocumentLibrary/DocumentLibrary';
        });
    }

    if (blDelete.toLowerCase() != "false") {
        shortcut.add("Delete", function () {
            DeleteItemDocument();
        });
    }

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadDocumentLibraryGrid();

    jQuery('#btnDeleteDocument').on('click', function () {

        DeleteItemDocument();
    });
});

function LoadDocumentLibraryGrid() {
    jQuery('#txtDocumentSearch').on('keyup', function (e) {
        var postData = jQuery('#tblDocument').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtDocumentSearch').val().trim();
        jQuery('#tblDocument').jqGrid("setGridParam", { search: true });
        jQuery('#tblDocument').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblDocument').jqGrid({
        url: '/DocumentLibrary/BindDocumentLibrary/',
        postData: { search: jQuery('#txtDocumentSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
             'Id', 'Document Title', 'Document Name', 'Remark', 'Active', 'Edit', 'Delete'],
        colModel: [
            { name: 'id', index: 'id', align: 'left', key: true, hidden: true },
            { name: 'Title', index: 'Title', align: 'left' },
            { name: 'DocumentName', index: 'DocumentName', align: 'left', formatter: ViewDocument },
            { name: 'Remark', index: 'Remark', align: 'left' },
            { name: 'IsActive', index: 'IsActive', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'left', width: 40, sortable: false, formatter: EditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'left', width: 40, sortable: false, formatter: DeleteFormat }
        ],
        pager: jQuery('#dvDocumentFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'DocumentId',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        multiselectWidth: 50,
        caption: 'List of Document Library',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../DocumentLibrary/DocumentLibrary?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblDocument').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblDocument').prev()[0].innerHTML = '';
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
        jQuery('#tblDocument').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblDocument').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemDocument(objId) {

    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblDocument').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Document Library'));
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
    jConfirm(kcs_Message.DeleteConfirm('Document Library'), function (r) {
        if (r) {

            jQuery.post("/DocumentLibrary/DeleteDocument/", { strDocumentIdList: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblDocument').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblDocument').setGridWidth($('#dvDocument').width());
}

function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../DocumentLibrary/DocumentLibrary?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemDocument(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";
}

function ViewDocument(cellvalue, options, rowObject) {
    return "<a target='_blank' href=" + jQuery("#hdnDocViewerRootFolderPath").val() + "/Documents/" + cellvalue + ">" + cellvalue + "</a>";
    //return "<a target='_blank' href=" + "/Documents/" + cellvalue + ">" + cellvalue + "</a>";
}