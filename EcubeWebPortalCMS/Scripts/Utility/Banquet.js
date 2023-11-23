$(document).ready(function () {

    //jQuery('#Description').keypress(function (key) {
    //    
    //    if (key.keyCode == 8 || key.keyCode == 9 || (key.charCode > 47 && key.charCode < 58) || key.keyCode == 37 || key.keyCode == 38 || key.keyCode == 39 || key.keyCode == 40 || key.keyCode == 46 || key.charCode == 32) { return true; }
    //    if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && (key.charCode != 45)) return false;
    //});
    if ($("#lblFlUploadTerms").text() == '') {
        $("#ImgFilePdf").hide();
    }

    $("#HallMaster").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id != 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.post('/Banquet/RemoveSessions/', { strSessionId: $('#hdnSession').val() });
            jQuery.ajaxSetup({ async: true });
        }
    };

    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'BanquetView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('BanquetView');

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
        if ($('#BanquetName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Banquet Name'), 'BanquetName');
            return false;
        }
        if ($('#Description').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Banquet Description'), 'Description');
            return false;
        }
        if ($('#MinPersonCapcity').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Banquet Minimum Capcity'), 'MinPersonCapcity');
            return false;
        }
        //
        if ($('#MinPersonCapcity').val().trim() < 1) {
            $('#MinPersonCapcity').val('');
            jAlert(kcs_Message.InputRequired('Valid Banquet Minimum Capcity'), 'MinPersonCapcity');
            return false;
        }
        if ($('#MaxPersonCapcity').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Banquet Maximum Capacity'), 'MaxPersonCapcity');
            return false;
        }
        if ($('#MaxPersonCapcity').val().trim() < 1) {
            $('#MaxPersonCapcity').val('');
            jAlert(kcs_Message.InputRequired('Valid Banquet Maximum Capacity'), 'MaxPersonCapcity');
            return false;
        }
        if ($('#MinPersonCapcity').val() > $('#MaxPersonCapcity').val()) {
            jAlert('Minimum Capacity Should not More than Maximum Capacity');
            return false;
        }
        if ($('#hdnImgProfilePic').val() == '') {
            if ($('#ImgProfilePic').val() == '') {
                jAlert(kcs_Message.SelectRequired('Banquet Image'), 'ImgProfilePic');
                return false;
            }
            else {
                checkfile("ImgProfilePic");
            }
        }
        if ($('#hdnFlUploadTerms').val() == '') {
            if ($('#FlUploadTerms').val().trim() == '') {
                jAlert(kcs_Message.SelectRequired('Banquet Terms & Conditions'), 'FlUploadTerms');
                return false;
            }
        }

        //if ($('#tblBanquetDetail').jqGrid('getGridParam', 'records') <= 0) {
        //    jAlert(kcs_Message.SelectRequired('Banquet Detail'));
        //    return false;
        //}
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
                        if (width != 396 || height != 555) {
                            jQuery('#ImgProfilePic').val('');
                            jAlert("Please upload valid Image size as 396px X 555px.");
                        }
                        checkfile("ImgProfilePic");
                        var postdata = $('#login-form').serialize();
                        var file = document.getElementById('ImgProfilePic').files[0];
                        var fd = new FormData();
                        fd.append("files", file);
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "/Banquet/UploadProfilePic", false);
                        xhr.send(fd);
                        $("#aBanquetImage").attr('src', image.src);
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
            xhr.open("POST", "/Banquet/UploadTerms", false);
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

    // LoadBanquetDetailGrid();

});

//$('#btnAddBanquetDetail').click(function () {
//    $.ajax({
//        url: '/Banquet/BanquetDetail/',
//        type: 'POST',
//        data: $('#frmBanquet').serialize(),
//        success: function (data) {
//            if (data == '1111') {
//                var postData = jQuery('#tblBanquetDetail').jqGrid("getGridParam", "postData");
//                jQuery('#tblBanquetDetail').jqGrid("setGridParam", { search: true });
//                jQuery('#tblBanquetDetail').trigger("reloadGrid", [{ page: 1, current: true }]);
//                LoadBanquetDetailGrid();
//                $('#btnCancelBanquetDetail').click();
//            }
//            else if (data != '') {
//                jAlert(data);
//            }
//        }
//    });
//    return false;
//});

//$("#ImgProfilePic").change(function () {
//    checkfile("ImgProfilePic");
//    var postdata = $('#login-form').serialize();
//    var file = document.getElementById('ImgProfilePic').files[0];
//    var fd = new FormData();
//    fd.append("files", file);
//    var xhr = new XMLHttpRequest();
//    xhr.open("POST", "/Banquet/UploadProfilePic", false);
//    xhr.send(fd);
//});


//$('#btnCancelBanquetDetail').click(function () {
//    $('#BanquetDetailId').val('0');
//    $('#GallaryImage').val('');
//    $('#GallaryImageFileUpload').val('');
//    $('#btnAddBanquetDetail').val('Add');
//});

//jQuery('#btnDeleteBanquetDetail').on('click', function () {
//    DeleteItemBanquetDetail();
//});


//function LoadBanquetDetailGrid() {
//   //
//    jQuery('#tblBanquetDetail').jqGrid({
//        url: '/Banquet/BindBanquetDetailGrid/',
//        datatype: 'json',
//        postData: { hdnSession: $('#hdnSession').val(), lgBanquetId: $('#Id').val() },
//        mtype: 'GET',
//        colNames: [
//            'Id', 'Banquet Image', 'Edit', 'Delete'],
//        colModel: [
//            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
//            { name: 'GallaryImage', index: 'GallaryImage', align: 'center', formatter: 'jqGridImageFormatter' },
//            { name: 'editoperation', index: 'editoperation', align: 'left', width: 40, sortable: false, formatter: EditFormatBanquetDetail },
//            { name: 'deleteoperation', index: 'deleteoperation', align: 'left', width: 40, sortable: false, formatter: DeleteFormatBanquetDetail }
//        ],
//        pager: jQuery('#dvBanquetDetailFooter'),
//        rowNum: kcs_Common.GridPageSize,
//        rowList: kcs_Common.GridPageArray,
//        sortname: 'BanquetDetailName',
//        sortorder: 'asc',
//        viewrecords: true,
//        caption: 'List of Banquet Detail',
//        height: '100%',
//        width: '100%',
//        multiselect: true,
//        loadComplete: function (data) {
//            if (data.records == 0) {
//                $('#tblBanquetDetail').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
//            }
//            else {
//                $('#tblBanquetDetail').prev()[0].innerHTML = '';
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
//        // return "<a href='/BanquetDetail/DisplayImage?strImg=" + cell + "'>" + cell + "</a>";
//        return "<a target='_blank' href=" + jQuery("#hdnAAAConfig").val() + "/Images/BanquetImage/" + cell + ">" + cell + "</a>";
//    };
//    SetStyle();
//}

//function EditItemBanquetDetail(objId) {
//    $('#btnAddBanquetDetail').val('Update');
//    var RowData = jQuery('#tblBanquetDetail').jqGrid('getRowData', objId);
//    $('#BanquetDetailId').val(RowData.Id);
//    $('#GallaryImage').val(RowData.GallaryImage);
//}

//function DeleteItemBanquetDetail(objId) {
//    if (objId == undefined || objId == '') {
//        var selRowIds = jQuery('#tblBanquetDetail').jqGrid('getGridParam', 'selarrrow');
//        if (selRowIds.length == 0) {
//            jAlert(kcs_Message.NoRecordToDelete('Banquet Detail'));
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

//    jConfirm(kcs_Message.DeleteConfirm('Banquet Detail'), function (r) {
//        if (r) {
//            alert(r);
//            jQuery.post("/Banquet/DeleteBanquetDetail/", { strBanquetDetailId: objId, strSessionId: $('#hdnSession').val(), lgBanquetId: $('#Id').val() },
//                 function (data) {
//                     if (data.toString() != "") {
//                         $('#tblBanquetDetail').trigger('reloadGrid', [{ page: 1, current: true }]);
//                     }
//                 });
//        }
//    });
//    return false;
//}

//function EditFormatBanquetDetail(cellvalue, options, rowObject) {
//    return "<a href='javascript:void(0);' onclick='EditItemBanquetDetail(" + options.rowId + ")'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a>";
//}

//function DeleteFormatBanquetDetail(cellvalue, options, rowObject) {
//    return "<a href='javascript:void(0);' onclick='DeleteItemBanquetDetail(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
//}

function SetStyle() {
    $('#tblBanquetDetail').setGridWidth($('#dvBanquetDetail').width());
}

//$('#FlUploadTerms').on('change', function () {
//    var IsValidatePdfFile = ValidatePdfFile();
//        var postdata = $('#login-form').serialize();
//        var file = document.getElementById('FlUploadTerms').files[0];
//        var fd = new FormData();
//        fd.append("files", file);
//        var xhr = new XMLHttpRequest();
//        xhr.open("POST", "/Banquet/UploadTerms", false);
//        xhr.send(fd);
//});


//$("#gallaryimagefileupload").change(function () {

//    if ($('#gallaryimagefileupload').val() == '') {
//        jalert(kcs_message.inputrequired('banquet image'), 'gallaryimagefileupload');
//        return false;
//    }
//    else {
//        return checkfile("gallaryimagefileupload");
//    }
//    var postdata = $('#login-form').serialize();
//    var file = document.getelementbyid('gallaryimagefileupload').files[0];
//    var fd = new formdata();
//    fd.append("files", file);
//    var xhr = new xmlhttprequest();
//    xhr.open("post", "/banquet/uploadbanquetdetail", false);
//    xhr.send(fd);
//});


//$('#ImgProfilePic').on('change', function () {
//    var fileUpload = document.getElementById("ImgProfilePic");
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
//                    if (height > 396 || width > 555) {
//                        jQuery('#ImgProfilePic').val('');
//                        jAlert("Image must not exceed 396px X 555px.");
//                    }
//                    var postdata = $('#login-form').serialize();
//                    var file = document.getElementById('ImgProfilePic').files[0];
//                    var fd = new FormData();
//                    fd.append("files", file);
//                    var xhr = new XMLHttpRequest();
//                    xhr.open("POST", "/Banquet/UploadProfilePic", false);
//                    xhr.send(fd);

//                };
//            }
//        }
//        else {
//            $('#ImgProfilePic').val('');
//            jAlert("Uploaded File not in Correct Format.");
//            return false;
//        }
//    }
//    else {
//        $('#ImgProfilePic').val('');
//        jAlert("Uploaded File not in Correct Format.");
//        return false;
//    }
//});

//$('#gallaryimagefileupload').on('change', function () {
//    alert("aa");
//    var fileupload = document.getelementbyid("gallaryimagefileupload");
//    var regex = new regexp("([a-za-z0-9\s_\\.\-:])+(.jpg|.png|.gif)$");
//    if (regex.test(fileupload.value.tolowercase())) {

//        if (typeof (fileupload.files) != "undefined") {
//            var reader = new filereader();
//            reader.readasdataurl(fileupload.files[0]);
//            reader.onload = function (e) {
//                var image = new image();
//                image.src = reader.result;
//                image.onload = function () {
//                    var height = image.height;
//                    var width = image.width;
//                    if (height > 396 || width > 555) {
//                        jquery('#gallaryimagefileupload').val('');
//                        jalert("image must not exceed 396px x 555px.");
//                    }
//                    var postdata = $('#login-form').serialize();
//                    var file = document.getelementbyid('gallaryimagefileupload').files[0];
//                    var fd = new formdata();
//                    fd.append("files", file);
//                    var xhr = new xmlhttprequest();
//                    xhr.open("post", "/banquet/uploadbanquetdetail", false);
//                    xhr.send(fd);
//                };
//            }
//        }
//        else {
//            $('#gallaryimagefileupload').val('');
//            jalert("uploaded file not in correct format.");
//            return false;
//        }
//    }
//    else {
//        $('#gallaryimagefileupload').val('');
//        jalert("uploaded file not in correct format.");
//        return false;
//    }
//});