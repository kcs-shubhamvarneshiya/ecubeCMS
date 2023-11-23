$(document).ready(function () {
    if ($("#lblFlUploadTerms").text() == '') {
        $("#ImgFilePdf").hide();
    }
    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id != 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.post('/RoomBooking/RemoveSessions/', { strSessionId: $('#hdnSession').val() });
            jQuery.ajaxSetup({ async: true });
        }
    };

    $("#RoomMaster").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'RoomBookingView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('RoomBookingView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $(window).bind('resize', function () {
        SetStyle();
    });


    $('#btnSubmit').click(function () {
        if ($('#RoomBookingName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Room Type'), 'RoomBookingName');
            return false;
        }
        if ($('#Description').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Room Description'), 'Description');
            return false;
        }
        if ($('#Member').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Member Rate'), 'Member');
            return false;
        }
        if ($('#Guest').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Guest Rate'), 'Guest');
            return false;
        }
        if (parseInt($('#Member').val()) > parseInt($('#Guest').val())) {
            $('#Guest').val('');
            jAlert(kcs_Message.InputRequired('Valid Guest Rate'), 'Guest');
            return false;
        }
        if ($('#hdnImgProfilePic').val() == '') {
            if ($('#ImgProfilePic').val().trim() == '') {
                jAlert(kcs_Message.SelectRequired('Room Image'), 'ImgProfilePic');
                return false;
            }
        }
        if ($('#hdnFlUploadTerms').val() == '') {
            if ($('#FlUploadTerms').val().trim() == '') {
                jAlert(kcs_Message.SelectRequired('Room Terms & Conditions'), 'FlUploadTerms');
                return false;
            }
        }
        if (($("#tblRoomBookingDetail").getRowData().length) == 0) {
            jAlert(kcs_Message.InputRequired('Room Booking Detail'));
            return false;
        }
        //if ($('#tblRoomBookingGallary').jqGrid('getGridParam', 'records') <= 0) {
        //    jAlert(kcs_Message.SelectRequired('Room Booking Gallary'));
        //    return false;
        //}
    });

    $('#btnCancelRoomBookingDetail').click(function () {       
        $('#TaxPercentage').val('');
        $('#TaxDescription').val('');
    });

    $('#btnAddRoomBookingDetail').click(function () {
        debugger
        if ($('#TaxPercentage').val() == '') {
            jAlert(kcs_Message.InputRequired('Tax Percentage'), 'TaxPercentage');
            return false;
        }
        if ($('#TaxPercentage').val() > 100) {
            $('#TaxPercentage').val('');
            jAlert(kcs_Message.InputRequired('Valid Tax Percentage'), 'TaxPercentage');
            return false;
        }
        if ($('#TaxDescription').val() == '') {
            jAlert(kcs_Message.InputRequired('Tax Description'), 'TaxPercentage');
            return false;
        }

        $.ajax({
            url: '/RoomBooking/RoomBookingDetail/',
            type: 'POST',
            data: $('#frmRoomBooking').serialize(),
            success: function (data) {
                if (data == '1111') {
                    var postData = jQuery('#tblRoomBookingDetail').jqGrid("getGridParam", "postData");
                    jQuery('#tblRoomBookingDetail').jqGrid("setGridParam", { search: true });
                    jQuery('#tblRoomBookingDetail').trigger("reloadGrid", [{ page: 1, current: true }]);
                    LoadRoomBookingDetailGrid();
                    $('#btnCancelRoomBookingDetail').click();
                    $('#TaxPercentage').val('');
                    $('#TaxDescription').val('');

                }
                else if (data != '') {
                    jAlert(data);
                }
            }
        });
        return false;
    });

    jQuery('#btnDeleteRoomBookingDetail').on('click', function () {
        DeleteItemRoomBookingDetail();
    });

    jQuery('#btnDeleteRoomBookingGallary').on('click', function () {
        DeleteItemRoomBookingGallary();
    });

    $('#ImgProfilePic').on('change', function () {
        var fileUpload = document.getElementById("ImgProfilePic");
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.png|.gif)$");
        if (regex.test(fileUpload.value.toLowerCase())) {
            if (typeof (fileUpload.files) != "undefined") {
                var reader = new FileReader();
                reader.readAsDataURL(fileUpload.files[0]);
                reader.onload = function (e) {
                    var image = new Image();
                    image.src = reader.result;
                    image.onload = function () {
                        var height = image.height;
                        var width = image.width;
                        if (width != 251 && height != 355) {
                            jQuery('#ImgProfilePic').val('');
                            jAlert("Please upload valid Image size as 251px X 355px.");
                        }
                        checkfile("ImgProfilePic");
                        var postdata = $('#login-form').serialize();
                        var file = document.getElementById('ImgProfilePic').files[0];
                        var fd = new FormData();
                        fd.append("files", file);
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "/RoomBooking/UploadProfilePic", false);
                        xhr.send(fd);
                        $("#aRoomImage").attr('src', image.src);
                    };
                }
            }
            else {
                $('#ImgProfilePic').val('');
                jAlert("Please upload file in a correct format.");
                return false;
            }
        }
        else {
            $('#ImgProfilePic').val('');
            jAlert("Please upload file in a correct format");
            return false;
        }
    });

    $('#FlUploadTerms').on('change', function () {
        var fileUpload = document.getElementById("FlUploadTerms");
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.pdf)$");
        if (regex.test(fileUpload.value.toLowerCase())) {
            var postdata = $('#login-form').serialize();
            var file = document.getElementById('FlUploadTerms').files[0];
            var fd = new FormData();
            fd.append("files", file);
            var xhr = new XMLHttpRequest();
            xhr.open("POST", "/RoomBooking/UploadTerms", false);
            xhr.send(fd);
            $("#lblFlUploadTerms").text(file.name);
            $("#ImgFilePdf").show();
        }
        else {
            $('#FlUploadTerms').val('');
            jAlert("Please upload file in a correct format.");
            return false;
        }
    });

    LoadRoomBookingDetailGrid();
    //  LoadRoomBookingGallaryGrid();

});
function LoadRoomBookingDetailGrid() {
    jQuery('#tblRoomBookingDetail').jqGrid({
        url: '/RoomBooking/BindRoomBookingDetailGrid/',
        datatype: 'json',
        postData: { hdnSession: $('#HdnSession').val(), roomBookingId: $('#Id').val() },
        mtype: 'GET',
        colNames: [
            'Id', 'Tax Percentage', 'Tax Description', 'Action'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'TaxPercentage', index: 'TaxPercentage', align: 'left', formatter: 'integer' },
            { name: 'TaxDescription', index: 'TaxDescription', align: 'left' },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormatRoomBookingDetail }
            //{ name: 'deleteoperation', index: 'deleteoperation', align: 'left', width: 40, sortable: false, formatter: DeleteFormatRoomBookingDetail }
        ],
        pager: jQuery('#dvRoomBookingDetailFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'RoomBookingDetailName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Room Booking Detail',
        height: '100%',
        width: '100%',
        multiselect: true,
        multiselectWidth: 50,
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblRoomBookingDetail').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblRoomBookingDetail').prev()[0].innerHTML = '';
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

    SetStyle();
}

function EditItemRoomBookingDetail(objId) {
    $('#btnAddRoomBookingDetail').val('Update');
    var RowData = jQuery('#tblRoomBookingDetail').jqGrid('getRowData', objId);
    $('#RoomBookingDetailId').val(RowData.Id);
    $('#TaxPercentage').val(RowData.TaxPercentage);
    $('#TaxDescription').val(RowData.TaxDescription);
}

function DeleteItemRoomBookingDetail(objId) {
    debugger
    
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblRoomBookingDetail').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Room Booking Detail'));
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

    jConfirm(kcs_Message.DeleteConfirm('Room Booking Detail'), function (r) {
        if (r) {
            jQuery.post("/RoomBooking/DeleteRoomBookingDetail/", { strRoomBookingDetailId: objId, strSessionId: $('#hdnSession').val(), lgRoomBookingId: $('#Id').val() },
                function (data) {
                    if (data.toString() != "") {
                        $('#tblRoomBookingDetail').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
        }
    });
    return false;
}

function EditFormatRoomBookingDetail(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='EditItemRoomBookingDetail(" + options.rowId + ")'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a><a class='ml-2' href='javascript:void(0);' onclick='DeleteItemRoomBookingDetail(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

//function DeleteFormatRoomBookingDetail(cellvalue, options, rowObject) {
//    return "<a class='ml-2' href='javascript:void(0);' onclick='DeleteItemRoomBookingDetail(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
//}

function SetStyle() {
    $('#tblRoomBookingDetail').setGridWidth($('#dvRoomBookingDetail').width());
    $('#tblRoomBookingGallary').setGridWidth($('#dvRoomBookingGallary').width());
}


//$("#ImgProfilePic").change(function () {
//    checkfile("ImgProfilePic");
//    var postdata = $('#login-form').serialize();
//    var file = document.getElementById('ImgProfilePic').files[0];
//    var fd = new FormData();
//    fd.append("files", file);
//    var xhr = new XMLHttpRequest();
//    xhr.open("POST", "/RoomBooking/UploadProfilePic", false);
//    xhr.send(fd);
//});

//$('#GallaryImageFileUpload').on('change', function () {
//    var fileUpload = document.getElementById("GallaryImageFileUpload");
//    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.png|.gif)$");
//    if (regex.test(fileUpload.value.toLowerCase())) {

//        if (typeof (fileUpload.files) != "undefined") {
//            var reader = new FileReader();
//            reader.readAsDataURL(fileUpload.files[0]);
//            reader.onload = function (e) {
//                var image = new Image();
//                image.src = reader.result;
//                image.onload = function () {
//                    var height = image.height;
//                    var width = image.width;
//                    if (height > 251 || width > 355) {
//                        jQuery('#GallaryImageFileUpload').val('');
//                        jAlert("Image must not exceed 251px X 355px.");
//                    }
//                    var postdata = $('#login-form').serialize();
//                    var file = document.getElementById('GallaryImageFileUpload').files[0];
//                    var fd = new FormData();
//                    fd.append("files", file);
//                    var xhr = new XMLHttpRequest();
//                    xhr.open("POST", "/RoomBooking/UploadRoomGallary", false);
//                    xhr.send(fd);

//                };
//            }
//        }
//        else {
//            $('#GallaryImageFileUpload').val('');
//            jAlert("Uploaded File not in Correct Format.");
//            return false;
//        }
//    }
//    else {
//        $('#GallaryImageFileUpload').val('');
//        jAlert("Uploaded File not in Correct Format.");
//        return false;
//    }
//});

//$('#btnAddRoomBookingGallary').click(function () {
//    $.ajax({
//        url: '/RoomBooking/RoomBookingGallary/',
//        type: 'POST',
//        data: $('#frmRoomBooking').serialize(),
//        success: function (data) {
//            if (data == '1111') {
//                var postData = jQuery('#tblRoomBookingGallary').jqGrid("getGridParam", "postData");
//                jQuery('#tblRoomBookingGallary').jqGrid("setGridParam", { search: true });
//                jQuery('#tblRoomBookingGallary').trigger("reloadGrid", [{ page: 1, current: true }]);
//                LoadRoomBookingGallaryGrid();
//                $('#btnCancelRoomBookingGallary').click();
//            }
//            else if (data != '') {
//                jAlert(data);
//            }
//        }
//    });
//    return false;
//});

//$('#btnCancelRoomBookingDetail').click(function () {
//    $('#RoomBookingDetailId').val('0');
//    $('#TaxPercentage').val('');
//    $('#TaxDescription').val('');
//    $('#btnAddRoomBookingDetail').val('Add');
//});

//$('#btnCancelRoomBookingGallary').click(function () {
//    $('#RoomBookingGallaryId').val('0');
//    $('#GallaryImageFileUpload').val('');
//    $('#FlUploadTerms').val('');
//    $('#GallaryImage').val('');
//    $('#btnAddRoomBookingGallary').val('Add');
//});


//function LoadRoomBookingGallaryGrid() {

//    jQuery('#tblRoomBookingGallary').jqGrid({
//        url: '/RoomBooking/BindRoomBookingGallaryGrid/',
//        datatype: 'json',
//        postData: { hdnSession: $('#hdnSession').val(), RoomBookingId: $('#Id').val() },
//        mtype: 'GET',
//        colNames: [
//            'Id', 'Room Image', 'Edit', 'Delete'],
//        colModel: [
//            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
//            { name: 'GallaryImage', index: 'GallaryImage', align: 'left', formatter: 'jqGridImageFormatter' },
//            { name: 'editoperation', index: 'editoperation', align: 'left', width: 40, sortable: false, formatter: EditFormatRoomBookingGallary },
//            { name: 'deleteoperation', index: 'deleteoperation', align: 'left', width: 40, sortable: false, formatter: DeleteFormatRoomBookingGallary }
//        ],
//        pager: jQuery('#dvRoomBookingGallaryFooter'),
//        rowNum: kcs_Common.GridPageSize,
//        rowList: kcs_Common.GridPageArray,
//        sortname: 'RoomBookingGallaryName',
//        sortorder: 'asc',
//        viewrecords: true,
//        caption: 'List of Room Booking Gallary',
//        height: '100%',
//        width: '100%',
//        multiselect: true,
//        loadComplete: function (data) {
//            if (data.records == 0) {
//                $('#tblRoomBookingGallary').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
//            }
//            else {
//                $('#tblRoomBookingGallary').prev()[0].innerHTML = '';
//            }
//            jQuery('input:checkbox.cbox').uniform();
//        },
//        onSelectAll: function (aRowids, status) {
//            jQuery.uniform.update(jQuery('input:checkbox.cbox'));
//        },
//        beforeSelectRow: function (rowid, e) {
//            var $myGrid = $(this),
//            i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
//            cm = $myGrid.jqGrid('getGridParam', 'colModel');
//            return (cm[i].name === 'cb');
//        }
//    });
//    $.fn.fmatter.jqGridImageFormatter = function (cell, options, row) {
//        //return "<a href='/RoomBookingGallary/DisplayImage?strImg=" + cell + "'>" + cell + "</a>";
//        return "<a target='_blank' href=" + jQuery("#hdnAAAConfig").val() + "/Images/RoomImage/" + cell + ">" + cell + "</a>";
//    };

//    SetStyle();
//}

//function EditItemRoomBookingGallary(objId) {
//    $('#btnAddRoomBookingGallary').val('Update');
//    var RowData = jQuery('#tblRoomBookingGallary').jqGrid('getRowData', objId);
//    $('#RoomBookingGallaryId').val(RowData.Id);
//    $('#GallaryImage').val(RowData.GallaryImage);
//}

//function DeleteItemRoomBookingGallary(objId) {
//    if (objId == undefined || objId == '') {
//        var selRowIds = jQuery('#tblRoomBookingGallary').jqGrid('getGridParam', 'selarrrow');
//        if (selRowIds.length == 0) {
//            jAlert(kcs_Message.NoRecordToDelete('Room Booking Gallary'));
//            return false;
//        }
//        for (var i = 0; i < selRowIds.length; i++) {
//            if (i == 0) {
//                objId = selRowIds[i];
//            }
//            else {
//                objId += ',' + selRowIds[i];
//            }
//        }
//    }

//    jConfirm(kcs_Message.DeleteConfirm('Room Booking Gallary'), function (r) {
//        if (r) {
//            jQuery.post("/RoomBooking/DeleteRoomBookingGallary/", { strRoomBookingGallaryId: objId, strSessionId: $('#hdnSession').val(), RoomBookingId: $('#Id').val() },
//                function (data) {
//                    if (data.toString() != "") {
//                        $('#tblRoomBookingGallary').trigger('reloadGrid', [{ page: 1, current: true }]);
//                    }
//                });
//        }
//    });
//    return false;
//}

//function EditFormatRoomBookingGallary(cellvalue, options, rowObject) {
//    return "<a href='javascript:void(0);' onclick='EditItemRoomBookingGallary(" + options.rowId + ")'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a>";
//}

//function DeleteFormatRoomBookingGallary(cellvalue, options, rowObject) {
//    return "<a href='javascript:void(0);' onclick='DeleteItemRoomBookingGallary(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";
//}