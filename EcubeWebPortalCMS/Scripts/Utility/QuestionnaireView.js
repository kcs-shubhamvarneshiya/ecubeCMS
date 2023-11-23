var blAdd, blEdit, blDelete, recursionSMS = false, recursionEmail = false, isRecursionCreated = false;
jQuery(document).ready(function () {
    $('#txtQuestionnaireSearch').hide();
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();
    $("#Questionnaire").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");


    $("#Questionnaire").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    LoadQuestionnaireGrid();

    $(window).bind('resize', function () {
        SetStyle();
    });

    jQuery('#btnDeleteQuestionnaire').on('click', function () {
        DeleteItemQuestionnaire();
    });

    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'Questionnaire', ('#txtQuestionnaireSearch').val().trim());
    });

    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Questionnaire', ('#txtQuestionnaireSearch').val().trim());
    });

    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Questionnaire', ('#txtQuestionnaireSearch').val().trim());
    });

    jQuery('#btnEmail').on('click', function () {
        if (ValidateRecursion()) {
            recursionSMS = false;
            recursionEmail = true;
            isRecursionCreated = true;
            UpdateRecursionDetails();
        }
    });

    jQuery('#btnSMS').on('click', function () {
        if (ValidateRecursion()) {
            recursionSMS = true;
            recursionEmail = false;
            isRecursionCreated = true;
            UpdateRecursionDetails();
        }
    });

    jQuery('#btnBoth').on('click', function () {
        if (ValidateRecursion()) {
            recursionSMS = true;
            recursionEmail = true;
            isRecursionCreated = true;
            UpdateRecursionDetails();
        }
    });

    jQuery('#btnRemove').on('click', function () {
        jConfirm("Are you sure want remove this Recursion?", function (r) {
            if (r) {
                isRecursionCreated = false;
                UpdateRecursionDetails();
            }
        })


    });

    LoadStatus();
    BindRecursionType();
});

$('#txtsearch').on('click', function () {
    $('#txtQuestionnaireSearch').toggle('slow');
    $('#txtsearch').toggleClass('close');
});

//$('#txtsearch').on('mouseover', function () {
//    $('#txtQuestionnaireSearch').show();
//});
//$('#txtQuestionnaireSearch').on('mouseout', function () {
//    $('#txtQuestionnaireSearch').hide();
//});

function LoadStatus() {

    var dFormat = "dd/mm/yy";
    var tFormat = "h:MM tt";
    jQuery('.SearchDate').each(function () { //loop through each input
        jQuery("#" + this.id).mask('99/99/9999');
    });

    $(".SearchDate").datepicker({

        changeMonth: true,
        changeYear: true,
        maxDate: 0,
        dateFormat: dFormat,
        beforeShow: function customRange(input) {
            if (input.id === 'txtFromDate') {
                var abc = $('#txtToDate').datepicker("getDate");
                return {
                    maxDate: abc == null ? 0 : $('#txtToDate').datepicker("getDate"),
                };
            } else if (input.id === 'txtToDate') {
                return {
                    minDate: $('#txtFromDate').datepicker("getDate"),
                };
            }
        }
    });

    $(".SearchTime").timepicker({

        changeMonth: true,
        changeYear: true,
        maxDate: 0,
        dateFormat: tFormat
    });

    jQuery('.SearchDate').on('blur', function (event) {
        var strChallanDate = '';

        if (jQuery(this).val() != null) {
            strChallanDate = jQuery(this).val().toString();

            if (strChallanDate != '' && strChallanDate != '__/__/____') {
                if (strChallanDate.length < 10) {
                    jQuery(this).val('');
                }
                else if (strChallanDate.charAt(2) != '/' || strChallanDate.charAt(5) != '/') {
                }
                else {
                    var year, month, day;
                    if (dFormat == "dd/mm/yy") {
                        year = parseInt(strChallanDate.substring(6, 10).toString());
                        month = parseInt(strChallanDate.substring(3, 5).toString()) - 1; // Months start with 0 in JavaScript - June
                        day = parseInt(strChallanDate.substring(0, 2).toString());
                    }
                    else if (dFormat == "mm/dd/yy") {
                        year = parseInt(strChallanDate.substring(6, 10).toString());
                        month = parseInt(strChallanDate.substring(0, 2).toString()) - 1; // Months because the months start with 0 in JavaScript - June
                        day = parseInt(strChallanDate.substring(3, 5).toString());
                    }
                    var myDate = new Date(year, month, day);
                    if ((myDate.getMonth() != month) || (myDate.getDate() != day) || (myDate.getFullYear() != year) || myDate.getFullYear() < 1753) {
                    }
                }
            }
        }
    });
}
function LoadQuestionnaireGrid() {

    jQuery('#txtQuestionnaireSearch').on('keyup', function (e) {
        var postData = jQuery('#tblQuestionnaire').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtQuestionnaireSearch').val().trim();
        postData.isActive = $("#IsActive:checkbox:checked").length > 0 ? 'true' : 'false';
        jQuery('#tblQuestionnaire').jqGrid("setGridParam", { search: true });
        jQuery('#tblQuestionnaire').trigger("reloadGrid", [{ page: 1, current: true }]);
        SetStyle();
    });

    jQuery('#IsActive').change(function (e) {
        var postData = jQuery('#tblQuestionnaire').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtQuestionnaireSearch').val().trim();
        postData.isActive = $("#IsActive:checkbox:checked").length > 0 ? 'true' : 'false';
        jQuery('#tblQuestionnaire').jqGrid("setGridParam", { search: true });
        jQuery('#tblQuestionnaire').trigger("reloadGrid", [{ page: 1, current: true }]);
        //SetStyle();
    });

    var isActive = $("#IsActive:checkbox:checked").length > 0 ? 'true' : 'false';
    jQuery('#tblQuestionnaire').jqGrid({
        url: '/Questionnaire/BindQuestionnaireGrid/',
        datatype: 'json',
        postData: { search: jQuery('#txtQuestionnaireSearch').val().trim(), isActive: isActive },
        mtype: 'GET',
        colNames: [
            'Id', 'Questionnaire Title', 'Description', 'Is Active',/*'Auto Link', 'Send Recursion',*/ 'Edit', 'Feedback'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'QuestionnaireTitle', index: 'QuestionnaireTitle', align: 'left', width: '200px' },
            { name: 'Description', index: 'Description', align: 'left', width: '200px' },
            { name: 'ActiveLink', index: 'ActiveLink', width: '100px', align: 'left' },
            //{ name: 'linkoperation', index: 'linkoperation', align: 'left', width: 40, sortable: false, formatter: LinkFormatQuestionnaire },
            //{ name: 'Recursionoperation', index: 'Recursionoperation', align: 'left', width: 40, sortable: false, formatter: RecursionFormatQuestionnaire },
            { name: 'editoperation', index: 'editoperation', align: 'left', width: '100px', sortable: false, formatter: EditFormatQuestionnaire },
            { name: 'feedbackoperation', index: 'feedbackoperation', align: 'left', width: '100px', sortable: false, formatter: FeedbackFormatQuestionnaire },
        ],
        pager: jQuery('#dvQuestionnaireFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Id',
        sortorder: 'DESC',
        viewrecords: true,
        caption: 'List of Questionnaire',
        height: '100%',
        width: '100%',
        multiselect: false,
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() !== "false") {
                window.location.href = '../Questionnaire/Questionnaire?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblQuestionnaire').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblQuestionnaire').prev()[0].innerHTML = '';
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

    if (blEdit.toLowerCase() === "false") {
        jQuery('#tblQuestionnaire').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() === "false") {
        jQuery('#tblQuestionnaire').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
}

function DeleteItemQuestionnaire(objId) {
    if (objId == undefined || objId === '') {
        var selRowIds = jQuery('#tblQuestionnaire').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length === 0) {
            jAlert(kcs_Message.NoRecordToDelete('Questionnaire'));
            return false;
        }
        for (var i = 0; i < selRowIds.length; i++) {
            if (i === 0) {
                objId = selRowIds[i];
            }
            else {
                objId += ',' + selRowIds[i];
            }
        }
    }

    jConfirm(kcs_Message.DeleteConfirm('Questionnaire'), function (r) {
        if (r) {
            jQuery.post("/Questionnaire/DeleteQuestionnaire/", { strQuestionnaireId: objId },
                function (data) {
                    if (data.toString() !== "") {
                        jAlert(data);
                        $('#tblQuestionnaire').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
        }
    });
    return false;
}

function EditFormatQuestionnaire(cellvalue, options, rowObject) {
    return "<a href='../Questionnaire/Questionnaire?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a>";
}

function FeedbackFormatQuestionnaire(cellvalue, options, rowObject) {
    return "<a title='View given Feedback' href='../FeedBack/FeedBack?" + options.rowId + "'>" + rowObject.AnswerCount + "</a>";
}

function LinkFormatQuestionnaire(cellvalue, options, rowObject) {
    if (rowObject.IsActive == true) {
        return "<a href='javascript:void(0);' onclick='ShowLink(\"" + rowObject.Link + "\")'><label class='IconLink' title='Link' alt='' /></label></a>";
    }
    else {
        return "";
    }
}

function SetStyle() {
    $('#tblQuestionnaire').setGridWidth($('#dvQuestionnaire').width());
}

function ShowLink(Link) {
    jQuery.ajaxSetup({ async: true });
    $('#divQuestionnaireLink').modal('show');
    $("#txtFeedbackUrl").val(Link).select();
    $("#txtFeedbackUrl").select();
};
function RecursionFormatQuestionnaire(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='OpenRecursion(this)' isCreated='" + rowObject.IsRecursionCreated + "' qId='" + rowObject.QuestionnaireId + "' rType='" + rowObject.RecursionTypeId + "' fDate='" + rowObject.RecursionFrom + "' tDate='" + rowObject.RecursionTo + "'><label class='' title='Recursion' alt='' />Send</lable></a>";
};
function OpenRecursion(ele) {
    $("#hdfQuestionnaireId").val($(ele).attr('qId'));
    $("#hdfRecursionCreated").val($(ele).attr('isCreated'));
    if ($(ele).attr('isCreated') == 'true') {
        var fDate = $(ele).attr('fDate');
        if (fDate != '') {
            var currentdate = new Date(parseInt(fDate.substr(6)));
            $('#txtFromDate').val(((currentdate.getDate().length === 1) ? currentdate.getDate() : '0' + currentdate.getDate()) + "/" + (((currentdate.getMonth().length + 1) === 1) ? (currentdate.getMonth() + 1) : '0' + (currentdate.getMonth() + 1)) + "/" + currentdate.getFullYear());
            $('#txtTime').val(currentdate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3"));
        }
        var tDate = $(ele).attr('tDate');
        if (tDate != '') {
            var currentdate = new Date(parseInt(tDate.substr(6)));
            $('#txtToDate').val(((currentdate.getDate().length === 1) ? currentdate.getDate() : '0' + currentdate.getDate()) + "/" + (((currentdate.getMonth().length + 1) === 1) ? (currentdate.getMonth() + 1) : '0' + (currentdate.getMonth() + 1)) + "/" + currentdate.getFullYear());
        }
        if ($(ele).attr('rType') != '') {
            $('input[name=RecursionType]').filter('[value="' + $(ele).attr('rType') + '"]').attr('checked', true);
        }
        $('#btnEmail').hide();
        $('#btnSMS').hide();
        $('#btnBoth').hide();
        $('#btnRemove').show();
    }
    else {
        ResetRecursionDateTime();
        $('#btnEmail').show();
        $('#btnSMS').show();
        $('#btnBoth').show();
        $('#btnRemove').hide();
    }
    $('#divRecursion').modal('show');
}
function ResetRecursionDateTime() {
    var currentdate = new Date();
    var date = ((currentdate.getDate() > 10) ? currentdate.getDate() : '0' + currentdate.getDate()) + "/" + (((currentdate.getMonth().length + 1) > 10) ? (currentdate.getMonth() + 1) : '0' + (currentdate.getMonth() + 1)) + "/" + currentdate.getFullYear()
    $('#txtFromDate').val(date);
    $('#txtToDate').val(date);
    $('#txtTime').val(currentdate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3"));
}
function BindRecursionType() {
    $.ajax({
        type: "GET",
        url: "/Questionnaire/GetAllRecursionType",
        dataType: "json",
        success: function (data) {
            var table = $("[id*=tblRadioLists]");
            $.each(data, function (index, value) {
                var rdb = '<div class="span12"><input type = "radio" id = "chk' + value.RecursionTypeName + '" name = "RecursionType" value = ' + value.Id + ' /> <span>' + value.RecursionTypeName + '</span></div >';
                $('#divRecursionType').append(rdb);
            });
        }
    });
}
function ValidateRecursion() {
    if ($("#txtFromDate").val().trim() == "") {
        jAlert("Please Enter Start Date", "txtFromDate");
        return false;
    }
    if ($("#txtToDate").val().trim() == "") {
        jAlert("Please Enter End Date", "txtToDate");
        return false;
    }
    if ($("#txtTime").val().trim() == "") {
        jAlert("Please Enter Time", "txtTime");
        return false;
    }
    if ($('input[name=RecursionType]:checked').length == 0) {
        jAlert("Please select Recursion Type");
        return false;
    } else { return true; }
}
function UpdateRecursionDetails() {
    var requestData = {};
    requestData.Id = $("#hdfQuestionnaireId").val();
    if (isRecursionCreated) {
        requestData.RecursionTypeId = $('input[name=RecursionType]:checked').val();
        requestData.RecursionFrom = $("#txtFromDate").val().trim() + ' ' + $("#txtTime").val().trim();
        requestData.RecursionTo = $("#txtToDate").val().trim() + ' ' + $("#txtTime").val().trim();
        requestData.RecursionSMS = recursionSMS;
        requestData.RecursionEmail = recursionEmail;
    }
    requestData.IsRecursionCreated = isRecursionCreated;
    $.ajax({
        url: '/Questionnaire/UpdateRecursionDetailsByQuestionnaireId',
        type: 'POST',
        contentType: 'application/json;',
        dataType: 'json',
        data: JSON.stringify(requestData),
        success: function (data) {
            if (data > 0) {
                jAlert('Record updated Successfully', "QuestionnaireTitle", function () {
                    window.location.href = 'QuestionnaireView';
                });
            } else {
                jAlert("Record is not updated Successfully", "QuestionnaireTitle");
            }
        }
    });
}