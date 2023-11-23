//const { debug } = require("util");

//const { debug } = require("node:util");

var i = 0, contactReferenceCount = 0, deletedQuestions = '', emailField = 0, mobileField = 0, questionType, QuestionId = 0, contactcount = 0;

$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            jConfirm("Are you sure want to Cancel?", function (result) {
                if (result) {
                    window.location.href = 'QuestionnaireView';
                }
            });
        } else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#DivImageSetup").addClass('hide');
    $("#InversCalculation").addClass('hide');

    $("#Questionnaire").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    $('.reset').click(function () {
        jConfirm("Are you sure want to reset Questionnaire?", function (result) {
            if (result) {
                if ($("#Id").val() != "0") {
                    window.location.href = 'Questionnaire?' + $("#hdnId").val();
                } else {
                    window.location.href = 'Questionnaire';
                }
            }
        });
    });
    $("#QuestionTypeId").change(function () {
        QuestionTypeIdChange();
    });

    $("#txtStartValue").blur(function () {
        MinimumMaximumValueChange();
    });

    $("#txtImage").change(function () {
        readURL(this, '');
    });

    $('.GraphicImage').each(function () {
        $("#" + this.id).change(function () {
            readURL(this, $(this).attr('data-index'));
        });
    });

    $("#txtEndValue").blur(function () {
        MinimumMaximumValueChange();
    });
    $("#txtMOMSCount").blur(function () {
        
        if ($("#txtMOMSCount").val().trim() == "") {
            jAlert("Please Enter No.of Options", "OptionsCount");
            return false;
        }
        var count = parseInt($("#txtMOMSCount").val());
        $("#dvMOMSTextbox").html("");
        $("#dvMOMSTextbox").append('<div id="divQ0" class="col-8 row d-flex align-items-center mb-3 mt-4" />');
        $("#divQ0").append("<h5 class=\"col-5 span2 marginleft10 mb-3\">Alias Name</h5>");
        var questionType = parseInt($("#QuestionTypeId").val());
        for (var j = 1; j <= count; j++) {
            var div = $("#dvMOMSTextbox").append('<div id="divQ' + j + '" class="controls col-8 pb-3 mb-3 row d-flex align-items-start" />');
            $("#divQ" + j).append("<span class=\"col-5 span2\"><input type=\"text\" onblur=\"return checkComma(this);\" onkeypress=\"return isComma(event)\" id=\"txtMOMSOption" + j + "\" placeholder=\"Option " + j + "\" class=\"form-control\" tabindex=\"31\" /></span>");
            if (questionType == 6) {
                $("#divQ" + j).append("<span class=\"col-md-3 col-12 span2 d-flex align-items-center\"><input type=\"radio\" id=\"rdbDefault" + j + "\" name=\"DefaultValue\" value=\"" + j + "\" class='marginleft10' tabindex=\"31\" />&nbsp;Unmark NA</span>");
                $("#divQ" + j).append("<span class=\"col-4 d-flex align-items-center\"><input type=\"radio\" id=\"rdbNA" + j + "\" name=\"NAValue\" value=\"" + j + "\" class='marginleft10' tabindex=\"32\" />&nbsp; Mark as NA </span><br/>");
            }
            else if (questionType == 1 || questionType == 5 || questionType == 18 || questionType == 21) {
                $("#divQ" + j).append("<span class=\"col-md-3 col-12 d-flex align-items-center\"><input type=\"radio\" id=\"rdbNA" + j + "\" name=\"NAValue\" value=\"" + j + "\" class='marginleft10' tabindex=\"32\" />&nbsp;Mark as NA</span><br/>");
            }
        }
        // Set this attribute on form control as when we press enter in textbox, it will reload the page.
        $('#frmQuestionnaire').attr('action', 'javascript:void(0);');
        if (questionType == 6) {
            $("#divQ0").append("<span class=\"col-md-3 col-12 span2 d-flex my-2\"><input id=\"bnResetRad\" class=\"btn btn-secondary btn-inverse resetDefault\" type=\"button\" tabindex=\"23\" value=\"Unmark Default\" /></span>&nbsp;");
            $("#divQ0").append("<span class=\"col-md-3 col-12 span2 d-flex my-2\"><input id=\"bnResetNaRad\" class=\"btn btn-secondary btn-inverse resetNA\" type=\"button\" tabindex=\"23\" value=\"Unmark NA\" /></span>");
        }
        else if (questionType == 1 || questionType == 5 || questionType == 18 || questionType == 21) {
            $("#divQ0").append("<span class=\"col-md-3 col-12 span2 d-flex my-2\"><input id=\"bnResetNaRad\" class=\"btn btn-secondary btn-inverse resetNA\" type=\"button\" tabindex=\"23\" value=\"Unmark NA\" /></span>");
        }
        $("#txtMOMSOption1").focus();
    });
    $("#dvMOMSTextbox").on("click", ".resetDefault", function (e) {
        $('#dvMOMSTextbox input[name=DefaultValue]').each(function () {
            $(this).removeAttr('checked');
        });
    });
    $("#dvMOMSTextbox").on("click", ".resetNA", function (e) {
        $('#dvMOMSTextbox input[name=NAValue]').each(function () {
            $(this).removeAttr('checked');
        });
    });
    $("#divTemplate").on("click", ".imgClose", function (e) {
        var div = $(this).parent("span")[0] === undefined ?
            $(this).parent("div").parent("div")[0].id :
            $(this).parent("span").parent("div").parent("div").parent("div")[0].id;
        jConfirm("Are you sure to delete the question?", function (result) {
            if (result) {
                if ($("#" + div).attr('data-questiontype').trim() == '10') {
                    emailField--;
                } else if ($("#" + div).attr('data-questiontype').trim() == '11') {
                    mobileField--;
                }
                var masterId = Number($("#" + div).attr('data-masterid').trim());
                if (masterId > 0) {
                    if (deletedQuestions == '') {
                        deletedQuestions = masterId;
                    } else {
                        deletedQuestions += ',' + masterId;
                    }
                }
                $("#" + div).parent("div").remove();
                DisplayTotalQuestionCount();
                $('#hdnQuestionCount').val('1');                
            }
        });
    });
    $("#divTemplate").on("click", ".close", function (e) {
        var modal = $(this).parent("div").parent("div");
        modal.css("display", "none");
        $(".modal-backdrop").removeClass("show").removeClass("fade").removeClass("modal-backdrop");
        document.body.style.overflow = 'auto';
        //modal.css("display", "none");

    });
    $("#btnAdd").click(function () {
        $('#hdnQuestionCount').val('1');
        $('#hdnPageName').val('Questionnaire');
        if ($("#txtQuestionTitle").val().trim() == "") {
            jAlert("Please Enter Question Title", "txtQuestionTitle");
            return false;
        }
        if ($("#QuestionTypeId").val().trim() == "") {
            jAlert("Please Select Question Type", "inQuestionTypeId");
            return false;
        }
        if ($("#txtTitleColor").val().trim() == "") {
            jAlert("Please Select Title Color", "txtTitleColor");
            return false;
        }
        if ($("#txtFontSize").val().trim() == '') {
            jAlert("Please Enter Font Size", "txtFontSize");
            return false;
        } else if (parseFloat($("#txtFontSize").val().trim()) < 10) {
            jAlert("Minimum Font Size should be 10", "txtFontSize");
            return false;
        }
        if ($("#txtMargin").val().trim() == '') {
            jAlert("Please Enter Margin", "txtMargin");
            return false;
        }
        if (CheckEmptyOptionsTextBox($("#QuestionTypeId").val())) {
            if ($("#chkIsRepetitive").is(":checked")) {
                if ($('#txtRepetitiveGroupNo').val() === "") {
                    jAlert("Please Enter questions group no value", "txtRepetitiveGroupNo");
                    return false;
                }
                if ($('#txtRepetitiveGroupName').val() === "") {
                    jAlert("Please Enterquestions group name value", "txtRepetitiveGroupName");
                    return false;
                }
                var retval = AddQuestionsGroupList($('#txtRepetitiveGroupNo').val(), $('#txtRepetitiveGroupName').val());
                if (retval == false) {
                    return false;
                }
            }

            AddQuestion();
        }
    });
    $("#btnSubmit").click(function () {
        if ($("#QuestionnaireTitle").val().trim() == "") {
            jAlert("Please Enter Questionnaire Title", "QuestionnaireTitle");
            return false;
        }
        if ($("#rbtnTest").is(":checked") && $("#TestTime").val().trim() == "") {
            jAlert("Please Enter Test Time", "TestTime");
            return false;
        }
        if ($("#rbtnTest").is(":checked") && $("#LastTestDate").val().trim() == "") {
            jAlert("Please Select Test Last Date", "LastTestDate");
            return false;
        }
        if ($('#divTemplate .divOriginal').children().length <= 0) {
            jAlert("Please Add Questions", "txtQuestionTitle");
            return false;
        }
        if (emailField > 1) {
            jAlert("You can add Active Email only once.", "txtQuestionTitle");
            return false;
        }
        if (mobileField > 1) {
            jAlert("You can add Mobile only once.", "txtQuestionTitle");
            return false;
        }
        if ($("#Id").val() != "0") {
            jConfirm("Are you sure want to Update Questionnaire?", function (result) {
                if (result) {
                    SaveQuestion();
                }
            });
        } else {
            SaveQuestion();
        }
    });
    $("#divTemplate").on("click", ".saveButton", function (e) {
        var divArray = new Array();
        var weight = 0;
        $('#divTemplate').children().each(function () {
            if ($(this).hasClass('divOriginal')) {
                divArray.push($(this).children().attr('id'));
            }
        });
        var QueId = $(this).attr('id');
        var QType = $(this).attr('data-questiontypeid');
        for (var i = 0; i < divArray.length; i++) {
            var ctrl = $("#" + divArray[i]);
            var masterId = Number(ctrl.attr('data-masterid'));
            if (masterId != 0 && masterId != Number(QueId)) {
                weight += parseFloat(ctrl.attr('data-weight'));
            }
        }

        var Que = { Hint: '' };
        Que.QuestionId = QueId;
        Que.QuestionTypeId = QType;
        Que.QuestionTitle = $("#txtQuestionTitle_" + QueId).val().trim();
        Que.Position = $("#txtPosition_" + QueId).val().trim();
        if (Que.QuestionTitle.trim() == '') {
            jAlert("Please Enter Question Title", "txtQuestionTitle_" + QueId);
            return false;
        }
        //Que.ShortName = $("#txtShortName_" + QueId).val().trim();
        Que.ShortName = Que.QuestionTitle;
        Que.FontSize = $("#txtFontSize_" + QueId).val().trim();
        if (Que.FontSize == '' || parseFloat(Que.FontSize) < 10) {
            jAlert("Please Enter Font Size. Minimum Font Size should be 10.", "txtFontSize_" + QueId);
            return false;
        }
        Que.Margin = $("#txtMargin_" + QueId).val().trim();
        if (Que.Margin.trim() == '') {
            jAlert("Please Enter Margin", "txtMargin_" + QueId);
            return false;
        }
        if ($("#chkIsActive_" + QueId).is(":checked")) {
            Que.IsActive = true;
        } else {
            Que.IsActive = false;
        }
        if (QType != 16 & QType != 23) {
            if ($("#chkIsRequired_" + QueId).is(":checked")) {
                Que.Required = true;
            } else {
                Que.Required = false;
            }
        }
        else {
            Que.Required = false;
        }
        if (QType != 17 & QType != 23) {
            if ($("#chkDisplayInSummary_" + QueId).is(":checked")) {
                Que.IsDisplayInSummary = true;
            } else {
                Que.IsDisplayInSummary = false;
            }
        }
        else {
            Que.IsDisplayInSummary = false;
        }
        if ($("#chkAllowDecimal_" + QueId).is(":checked")) {
            Que.AllowDecimal = true;
        } else {
            Que.AllowDecimal = false;
        }

        if ($("#chkDisplayInDetail_" + QueId).is(":checked")) {
            Que.IsDisplayInDetail = true;
        } else {
            Que.IsDisplayInDetail = false;
        }

        if ($("#chkIsTitleBold_" + QueId).is(":checked")) {
            Que.IsTitleBold = true;
        } else {
            Que.IsTitleBold = false;
        }
        if ($("#chkIsTitleItalic_" + QueId).is(":checked")) {
            Que.IsTitleItalic = true;
        } else {
            Que.IsTitleItalic = false;
        }
        if ($("#chkIsTitleUnderline_" + QueId).is(":checked")) {
            Que.IsTitleUnderline = true;
        } else {
            Que.IsTitleUnderline = false;
        }

        if (QType == 7) {
            if ($("#chkIsCommentCompulsory_" + QueId).is(":checked")) {
                Que.blIsCommentCompulsory = true;
            } else {
                Que.blIsCommentCompulsory = false;
            }
        }
        Que.TitleTextColor = $("#txtTitleColor_" + QueId).val().trim();

        if (QType == 3 || QType == 4 || QType == 12 || QType == 13 || QType == 27 || QType == 28 || QType == 29 || QType == 30) {
            if ($("#txtMaxlength_" + QueId).val().trim() == "") {
                jAlert("Please Enter Max Length", "txtMaxlength_" + QueId);
                return false;
            }
            Que.MaxLength = $("#txtMaxlength_" + QueId).val().trim();
        } else {
            Que.MaxLength = 0;
        }
        if (QType == 4) {
            Que.blIsNumeric = $("#chkIsNumeric_" + QueId).is(":checked");
            Que.blIsEmail = $("#chkIsEmail_" + QueId).is(":checked");
        } else if (QType == 1 || QType == 5 || QType == 6 || QType == 18 || QType == 21) {
            Que.LstOption = new Array();
            var p = 1;
            var divArray = new Array();
            $("#divOptions_" + QueId).children().each(function () {
                divArray.push($(this).attr('id'));
            });
        }

        if (QType == 1 || QType == 2 || QType == 5 || QType == 6 || QType == 7) {
            var DisplayType;
            if ($("#rdbHorizontal_" + QueId).is(":checked")) {
                DisplayType = "Horizontal";
            } else {
                DisplayType = "Vertical";
            }
            Que.OptionDisplayType = DisplayType;
        }
        if (QType == 19) {
            Que.TableGroupName = '';//$("#txtTableGroup_" + QueId).val().trim();
        } else {
            Que.TableGroupName = '';
        }
        Que.ImagePath = null;
        if (QType == 23) {
            UploadGraphicsImage(QueId, QueId);
            if (typeof($("#txtImage" + QueId).attr('data-ImageName')) == "undefined") {
                Que.ImagePath = $("#ImgGraphic" + QueId).attr('data-ImageName');
            }
            else {
                Que.ImagePath = $("#txtImage" + QueId).attr('data-ImageName');
            }
            Que.ImageHeight = $("#txtImageHeight_" + QueId).val().trim();
            Que.ImageWidth = $("#txtImageWidth_" + QueId).val().trim();
            Que.ImageAlign = $("#dpImageAlign_" + QueId).val().trim();
        }
        Que.QuestionnaireId = $("#Id").val();
        
        $.ajax({
            url: '/Questionnaire/UpdateQuestionnaireQuestion',
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            data: JSON.stringify(Que),
            success: function (data) {
                if (data.status) {
                    jAlert(data.msg, "txtQuestionnaireTitle", function () {
                        $("#divEditTemplate_" + QueId).modal('toggle');
                        window.location.href = "Questionnaire?" + $("#hdnId").val();
                    });
                } else {
                    jAlert(data.msg, "strQuestionnaireTitle");
                }
            }
        });
        return false;
    });
    //$("#txtQuestionTitle").blur(function () {
    //    if ($("#QuestionTypeId").val() != 16) {
    //        if ($("#txtQuestionTitle").val().length > 18) {
    //            var str = $("#txtQuestionTitle").val();
    //            var res = str.substring(0, 18);
    //            document.getElementById("txtShortName").value = res;
    //        }
    //        else {
    //            document.getElementById("txtShortName").value = $("#txtQuestionTitle").val();
    //        }
    //    }
    //    else {
    //        document.getElementById("txtShortName").value = $("#txtQuestionTitle").val();
    //    }
    //});
});

function QuestionTypeIdChange() {
    $("#divDisplayInSummary").removeClass('hide');  
    var questionTypeId = $("#QuestionTypeId").val();
    $("#DivImageSetup").addClass('hide');
    $("#InversCalculation").addClass('hide');
    if (questionTypeId == 5 || questionTypeId == 18) {
        $("#InversCalculation").removeClass('hide');
    }
    if (questionTypeId == 5 || questionTypeId == 6 || questionTypeId == 18 || questionTypeId == 21) {
        $("#dvMOMS").removeClass('hide');
        $("#txtMOMSCount").val('');
        $("#dvMOMSTextbox").html("");
    } else {
        $("#dvMOMS").addClass('hide');
        $("#txtMOMSCount").val('');
        $("#dvMOMSTextbox").html("");
    }
    if (questionTypeId == 1 || questionTypeId == 2 || questionTypeId == 5 || questionTypeId == 6 || questionTypeId == 7) {
        $("#dvOptionsDisplayType").removeClass('hide');
        $("#rdbHorizontal").attr('checked', true);
    } else {
        $("#dvOptionsDisplayType").addClass('hide');
        $("#rdbHorizontal").attr('checked', true);
    }
    if (questionTypeId == 16) {
        $("#chkRequired").attr('disabled', true);
        //$("#txtShortName").removeAttr('maxlength');

    } else {
        $("#chkRequired").attr('disabled', false);
        //$("#txtShortName").attr('maxlength', '18');
    }

    if (questionTypeId == 3 || questionTypeId == 4 || questionTypeId == 12 || questionTypeId == 13 || questionTypeId == 27 || questionTypeId == 28 || questionTypeId == 29 || questionTypeId == 30) {
        $("#divMaxLength").removeClass('hide');
        $("#divHint").removeClass('hide');
        if (questionTypeId == 4 || questionTypeId == 27 || questionTypeId == 28 || questionTypeId == 29 || questionTypeId == 30) {
            $('#txtMaxLength').val('999');
        } else {
            $('#txtMaxLength').val('3000');
        }
    } else {
        $("#divMaxLength").addClass('hide');
        $("#divHint").addClass('hide');
    }
    if (questionTypeId == 1) {
        if ($("#rbtnTest").is(":checked")) {
            $("#divStartValue").removeClass('hide');
        }
        else {
            $("#divStartValue").removeClass('hide');
        }
    }
    else {
        $("#divStartValue").addClass('hide');
    }

    if (questionTypeId == 19) {
        $("#divAllowDecimal").removeClass('hide');
    } else {
        $("#divAllowDecimal").addClass('hide');
    }

    if (questionTypeId == 7 || questionTypeId == 14 || questionTypeId == 15) {
        $("#divYesNoWeight").removeClass('hide');
        $("#chkCompulsory").removeClass('hide');

    } else {
        $("#divYesNoWeight").addClass('hide');
        $("#chkCompulsory").addClass('hide');
    }

    ////## Database Reference Question
    if (questionTypeId == 26) {
        $("#divDatabaseReference").removeClass('hide');
    } else {
        $("#divDatabaseReference").addClass('hide');
    }
    if (questionTypeId == 17) {
        $("#divDisplayInSummary").addClass('hide');
        $("#IsSignature").removeClass('hide');
    } else {
        $("#divDisplayInSummary").removeClass('hide');
        $("#IsSignature").addClass('hide');
    }

    if (questionTypeId == 25) {
        $("#divMandatory").addClass('hide');
        $("#divDisplayInSummary").addClass('hide');
        $("#divDisplayInDetails").addClass('hide');
    }
    else {
        $("#divMandatory").removeClass('hide');
        $("#divDisplayInSummary").removeClass('hide');
        $("#divDisplayInDetails").removeClass('hide');
    }

    if (questionTypeId == 1 || questionTypeId == 5 || questionTypeId == 6 || questionTypeId == 7 || questionTypeId == 18 || questionTypeId == 21) {
        MultipleValue();
        if ($("#rbtnTest").is(":checked")) {
            $("#divPIEscalation").addClass('hide');

        }
        else {
            $("#divPIEscalation").removeClass('hide');
        }
        $("#chkGraphsView").removeClass('hide');
        if (questionTypeId == 1) {
            $("#chkTableView").addClass('hide');
        }
        else {
            $("#chkTableView").removeClass('hide');
        }
    }
    else {
        $("#divPIEscalation").addClass('hide');
        $("#chkGraphsView").addClass('hide');
        $("#chkTableView").addClass('hide');
        MultipleValue();
    }

    if (questionTypeId == 10 || questionTypeId == 11) {
        $("#IsAnonymous").removeClass('hide');
    }
    else {
        $("#IsAnonymous").addClass('hide');
    }

    /*-Add For Group Repetitive Quetions DT.16-Mar-2017.*/
    if (questionTypeId == 16 || questionTypeId == 23 || questionTypeId == 25) {
        $("#divIsRepetitive").addClass('hide');
    }
    else {
        $("#divIsRepetitive").removeClass('hide');
    }

    $("#chkRequired").prop("checked", false);
    $("#chkIsDisplayInSummary").prop("checked", false);
    $("#chkIsRepetitive").prop("checked", false);/*---------------------------------------------Add For Group Repetitive Quetions DT.16-Mar-2017.*/
    $("#chkIsDisplayInDetail").prop("checked", true);
    jQuery.uniform.update(jQuery('input:checkbox, input:radio'));

    if ($("#rbtnContactReference").is(":checked")) {
        $("#chkRequired").prop("checked", "checked");
        $("#chkRequired").attr('disabled', 'disabled');
        jQuery.uniform.update("#chkRequired");
        $("#chkIsActive").prop("checked", "checked");
        $("#chkIsActive").attr('disabled', 'disabled');
        jQuery.uniform.update("#chkIsActive");
    }
    else {
        $("#chkRequired").prop("checked", false);
        $("#chkRequired").attr('disabled', false);
        jQuery.uniform.update("#chkRequired");
        $("#chkIsActive").attr('disabled', false);
        jQuery.uniform.update("#chkIsActive");
    }

    if (questionTypeId == 23) {
        $("#txtImageHeight").val(200);
        $("#txtImageWidth").val(200);
        $("#dpImageAlign").val("Left");
        $("#txtImage").val("");
        $('#ImgGraphic').attr('src', '');
        $("#divGraphics").removeClass('hide');
        $("#divMandatory").addClass('hide');
        $("#DivImageSetup").removeClass('hide');
        $("#divDisplayInSummary").addClass('hide');
    } else {
        $("#divGraphics").addClass('hide');
        $("#divMandatory").removeClass('hide');
        if (questionTypeId == 17) {
            $("#divDisplayInSummary").addClass('hide');
        }
        else {
            $("#divDisplayInSummary").removeClass('hide');
        }
    }
};

function AddQuestion() {
    var captchaId = 0
    $(".divOriginal-panel").each(function () {
        if ($(this).attr('data-questiontype').trim() == '25') {
            captchaId++;
        }
    });
    var ShortName = '', QType = '';
    //if ($("#txtShortName").val() == "") {
    //    ShortName = $("#txtQuestionTitle").val();
    //} else {
    //    ShortName = $("#txtShortName").val();
    //}
    QType = $("#QuestionTypeId option:selected").text();
    i++;
    var questionTypeId = $("#QuestionTypeId").val();

    var isActive = false;
    if ($("#chkIsActive").is(":checked")) {
        isActive = true;
    }

    var isRequired = false;
    if ($("#chkRequired").is(":checked") && questionTypeId != 16 && questionTypeId != 23) {
        isRequired = true;
    }
    var isCompulsory = false;
    if ($("#chkIsCommentCompulsory").is(":checked") && questionTypeId == 7) {
        isCompulsory = true;
    }
    var isDisplayInSummary = false;
    if ($("#chkIsDisplayInSummary").is(":checked") && questionTypeId != 17 && questionTypeId != 23) {
        isDisplayInSummary = true;
    }
    var isSignature = false;
    if ($("#chkIsSignature").is(":checked") && questionTypeId == 17) {
        isSignature = true;
    }
    var isAnonymous = false;
    if ($("#chkIsAnonymous").is(":checked") && questionTypeId != 17 && questionTypeId != 23) {
        isAnonymous = true;
    }

    var isDisplayInDetail = false;
    if ($("#chkIsDisplayInDetail").is(":checked")) {
        isDisplayInDetail = true;
    }

    var isAllowDecimal = false;
    if ($("#chkAllowDecimal").is(":checked")) {
        isAllowDecimal = true;
        QType = 'Decimal Field';
    }

    /*----Add For Group Repetitive Quetions DT.16-Mar-2017.*/
    var isRepetitive = false;
    if ($("#chkIsRepetitive").is(":checked") && questionTypeId != 16 && questionTypeId != 23) {
        isRepetitive = true;
    }

    var isDisplayInGraphs = false;
    if ($("#chkShowInGraphs").is(":checked")) {
        isDisplayInGraphs = true;
    }
    var isDisplayInTableView = false;
    if ($("#chkShowInTableView").is(":checked")) {
        isDisplayInTableView = true;
    }
    var PIEscalationValue = $("#txtPIEscalationValue").val();
    var MultipleRoutingValue = $("#txtPIMultipleRoutingValue").val();

    var Hint = $("#txtHint").val();
    var FontSize = $("#txtFontSize").val();
    var Margin = $("#txtMargin").val();

    var DisplayType = '';
    if (questionTypeId == 1 || questionTypeId == 2 || questionTypeId == 5 || questionTypeId == 6 || questionTypeId == 7) {
        if ($("#rdbHorizontal").is(":checked")) {
            DisplayType = "Horizontal";
        } else {
            DisplayType = "Vertical";
        }
    } else {
        DisplayType = '';
    }

    var maxLength = 0;
    if ($("#txtMaxLength").val().trim() != "") {
        maxLength = $("#txtMaxLength").val().trim();
    }
    var isBold = false, isItalic = false, isUnderline = false, textColor = $("#txtTitleColor").val().trim();
    if ($("#chkIsTitleBold").is(":checked")) {
        isBold = true;
    }
    if ($("#chkIsTitleItalic").is(":checked")) {
        isItalic = true;
    }
    if ($("#chkIsTitleUnderline").is(":checked")) {
        isUnderline = true;
    }
    var SeenClientQuestionId = '';
    if ($("#rbtnSeenClientReference").is(":checked")) {
        SeenClientQuestionId = $("#SeenClientQuestionId").val();
        contactReferenceCount++;
    }
    var ContactQuestionId = '';
    if ($("#rbtnContactReference").is(":checked")) {
        ContactQuestionId = $("#ContactQuestionId").val();
        contactcount++;
    }

    var yesWeight = 0, noWeight = 0;
    if (questionTypeId == 7 || questionTypeId == 14 || questionTypeId == 15) {
        yesWeight = $("#txtYesWeight").val();
        noWeight = $("#txtNoWeight").val();
    }
    var DisplayCalculationOption = '';
    var DisplaySummaryOption = '';
    if (questionTypeId == 5 || questionTypeId == 18) {
        if ($("#CalculateSelected").is(":checked")) {
            DisplayCalculationOption = "1";
        } else {
            DisplayCalculationOption = "2";
        }

        if ($("#showitemselected").is(":checked")) {
            DisplaySummaryOption = "1";
        } else {
            DisplaySummaryOption = "2";
        }
    } else {
        DisplayCalculationOption = '';
        DisplaySummaryOption = '';
    }
    var ImageHeight = 0;
    var Imagewidth = 0;
    var Imagealign = ''
    if (questionTypeId == 23) {
        ImageHeight = $("#txtImageHeight").val();
        Imagewidth = $("#txtImageWidth").val();
        Imagealign = $("#dpImageAlign").val();
    }

    var startValue = 0, endValue = 0;
    if (questionTypeId == 1) {
        startValue = $("#txtStartValue").val();
        endValue = $("#txtEndValue").val();
    }
    var weight = 0;

    $("<div id=\"divRating" + i + "\" class=\"divOriginal ui-widget-content divOriginalui-widget\">").appendTo("#divTemplate");
    $("#divRating" + i).append("<div id=\"dvDetails" + i + "\" class=\"divOriginal-panel clearfix\" data-questiontype='" + questionTypeId + "' data-ContactQuestionId='" + ContactQuestionId + "' data-SeenClientQuestionId='" + SeenClientQuestionId + "' data-index='" + i + "' data-masterid='0' >");
    $("#dvDetails" + i).append("<div id=\"dvTitle" + i + "\" class=\"divOriginaltext-panel\" ></div>");
    $("#dvTitle" + i).append("<span class=\"divOriginal-text\" style='font-size:" + FontSize + "Px !important; color:" + textColor + ";" + (isBold ? "font-weight: bold;" : "font-weight:normal") + (isItalic ? "font-style: italic;" : "") + (isUnderline ? "text-decoration: underline;" : "") + "'>" + $("#txtQuestionTitle").val() + "</span>");
    if (isRequired) {
        $("#dvTitle" + i).append("<span style=\"color:Red;\"> *</span>");
    }
    var Quetitle = $("#txtQuestionTitle").val().trim();
    $("#dvTitle" + i).append("<input type='hidden' id='hdnQuestionType" + i + "' value='" + questionTypeId + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnQuestionTitle" + i + "' value='' />");
    $("#hdnQuestionTitle" + i).val(Quetitle);
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsRequired" + i + "' value='" + isRequired + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsSignature" + i + "' value='" + isSignature + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsDisplayInSummary" + i + "' value='" + isDisplayInSummary + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsDisplayInDetail" + i + "' value='" + isDisplayInDetail + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsAllowDecimal" + i + "' value='" + isAllowDecimal + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnisAnonymous" + i + "' value='" + isAnonymous + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnShortName" + i + "' value='" + ShortName + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnHint" + i + "' value='" + Hint + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnDisplayType" + i + "' value='" + DisplayType + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnMaxLength" + i + "' value='" + maxLength + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsBold" + i + "' value='" + isBold + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsItalic" + i + "' value='" + isItalic + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsUnderLine" + i + "' value='" + isUnderline + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnTextColor" + i + "' value='" + textColor + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnFontSize" + i + "' value='" + FontSize + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnMargin" + i + "' value='" + Margin + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnStartValue" + i + "' value='" + startValue + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnEndValue" + i + "' value='" + endValue + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnSeenClientQuestionId" + i + "' value='" + SeenClientQuestionId + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnContactQuestionId" + i + "' value='" + ContactQuestionId + "' />");
    //$("#dvTitle" + i).append("<input type='hidden' id='hdnTableGroupName" + i + "' value='" + tableGroupName + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnWeight" + i + "' value='" + weight + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnYesWeight" + i + "' value='" + yesWeight + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnNoWeight" + i + "' value='" + noWeight + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsActive" + i + "' value='" + isActive + "' />");

    $("#dvTitle" + i).append("<input type='hidden' id='hdnPIEscalationValue" + i + "' value='" + PIEscalationValue + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnDisplayInGraphs" + i + "' value='" + isDisplayInGraphs + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnDisplayInTableView" + i + "' value='" + isDisplayInTableView + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnIsCompulsory" + i + "' value='" + isCompulsory + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnMultipleRoutingValue" + i + "' value='" + MultipleRoutingValue + "' />");

    //Graphic Image Set
    $("#dvTitle" + i).append("<input type='hidden' id='hdimageheight" + i + "' value='" + ImageHeight + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdimagewidth" + i + "' value='" + Imagewidth + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdimagealign" + i + "' value='" + Imagealign + "' />");

    // Invser calculation 
    $("#dvTitle" + i).append("<input type='hidden' id='hdnCaclculationoption" + i + "' value='" + DisplayCalculationOption + "' />");
    $("#dvTitle" + i).append("<input type='hidden' id='hdnSummaryOption" + i + "' value='" + DisplaySummaryOption + "' />");

    $("#dvDetails" + i).append("<div id=\"dvQueType" + i + "\" class=\"divOriginaltext-right\" style=\"float:right;\" ></div>");
    $("#dvQueType" + i).append("<span class='right'>(" + QType + ")</span>");
    $("#dvQueType" + i).append("<img src=\"../../Content/Image/Delete.png\" alt=\"Close\" class=\"imgClose\" />");

    $("#dvDetails" + i).append("<div id=\"dvControl" + i + "\" class=\"divOriginaltext-value\" ></div>");

    if (questionTypeId == 1) {
        var count = endValue - startValue + 1;
        for (var j = 1; j <= count; j++) {
            var txt = "#txtMOMSOption" + j, txtWeight = "#txtWeight" + j;
            var optionText = $(txt).val();
            $("#dvControl" + i).append("<input type=\"radio\" id=\"rbMOSS" + i + j + "\" name=\"MOSS" + i + "\" value=\"" + (parseInt(startValue) + j - 1) + "\" data-text=\"" + optionText + "\" data-weight=\"" + $(txtWeight).val() + "\" class='quecenter' />" + optionText + " ");
            if (DisplayType == "Vertical") {
                $("#dvControl" + i).append("<br/>");
            }
        }
        var NA = "";
        if ($('input[name=NAValue]:checked').val() != "") {
            NA = $('input[name=NAValue]:checked').val();
            NA = $("#rdbNA" + NA).val();
        }
        $("#dvControl" + i).append("<input type=\"hidden\" id=\"DefaultNAHidden" + i + "\" value=\"" + NA + "\" />");
    } else if (questionTypeId == 2) {
        for (var row = 0; row < 10; row++) {
            $("#dvControl" + i).append("<input type=\"radio\" class='quecenter' id=\"rbtnNPS" + i + "\" name=\"NPS" + i + "\" value=\"" + row + "\" />" + row);
        }
    } else if (questionTypeId == 3) {
        $("#dvControl" + i).append("<textarea id=\"txtLong" + i + "\" /> ");
    } else if (questionTypeId == 4 || questionTypeId == 8 || questionTypeId == 9 || questionTypeId == 10 || questionTypeId == 11 || questionTypeId == 12 || questionTypeId == 19 || questionTypeId == 20 || questionTypeId == 22 || questionTypeId == 24 || questionTypeId == 26 || questionTypeId == 27 || questionTypeId == 28 || questionTypeId == 29 || questionTypeId == 30) {
        $("#dvControl" + i).append("<input type=\"text\" id=\"txtShort" + i + "\" /> ");
        if (questionTypeId == 10) {
            emailField++;
        } else if (questionTypeId == 11) {
            mobileField++;
        } else if (questionTypeId == 26) {
            if ($('#uploadExcelFile').val() !== '') {
                $("#dvControl" + i).append("<div id=\"divuploadFile" + i + "\"><input type=\"file\" id=\"uploadExcelFile" + i + "\" /></div>");
                var clone = $('#uploadExcelFile').clone();
                clone.attr('id', 'uploadExcelFile' + i + '');
                $('#divuploadFile' + i + '').html(clone);
                $('#uploadExcelFile' + i + '').removeClass('fileUploadcontrol hide');
                $('#uploadExcelFile' + i + '').attr('disabled', 'disabled');
            }
        }
    } else if (questionTypeId == 5) {
        var count = $("#txtMOMSCount").val();
        for (var j = 1; j <= count; j++) {
            var txt = "#txtMOMSOption" + j, txtWeight = "#txtWeight" + j, txtPoint = "#txtPoint" + j;
            var optionText = $(txt).val();
            $("#dvControl" + i).append("<input type=\"checkbox\" id=\"chkMOMS" + i + j + "\" name=\"MOMS" + i + "\" value=\"" + optionText + "\" data-weight=\"" + $(txtWeight).val() + "\" data-point=\"0\" class='quecenter' />" + optionText + " ");
            if (DisplayType == "Vertical") {
                $("#dvControl" + i).append("<br/>");
            }
        }
        var NA = "";
        if ($('input[name=NAValue]:checked').val() != "") {
            NA = $('input[name=NAValue]:checked').val();
            NA = $("#rdbNA" + NA).val();
        }
        $("#dvControl" + i).append("<input type=\"hidden\" id=\"DefaultNAHidden" + i + "\" value=\"" + NA + "\" />");
    } else if (questionTypeId == 6) {
        var count = $("#txtMOMSCount").val();
        for (var j = 1; j <= count; j++) {
            var txt = "#txtMOMSOption" + j, txtWeight = "#txtWeight" + j, txtPoint = "#txtPoint" + j;
            var optionText = $(txt).val();
            $("#dvControl" + i).append("<input type=\"radio\" id=\"rbMOSS" + i + j + "\" name=\"MOSS" + i + "\" value=\"" + optionText + "\" data-weight=\"" + $(txtWeight).val() + "\" data-point=\"0\" class='quecenter' />" + optionText + " ");
            if (DisplayType == "Vertical") {
                $("#dvControl" + i).append("<br/>");
            }
        }
        var Default = "";
        if ($('input[name=DefaultValue]:checked').val() != "") {
            Default = $('input[name=DefaultValue]:checked').val();
            Default = $("#txtMOMSOption" + Default).val();
        }
        $("#dvControl" + i).append("<input type=\"hidden\" id=\"DefaultValueHidden" + i + "\" value=\"" + Default + "\" />");
        var NA = "";
        if ($('input[name=NAValue]:checked').val() != "") {
            NA = $('input[name=NAValue]:checked').val();
            NA = $("#txtMOMSOption" + NA).val();
        }
        $("#dvControl" + i).append("<input type=\"hidden\" id=\"DefaultNAHidden" + i + "\" value=\"" + NA + "\" />");
    } else if (questionTypeId == 7) {
        $("#dvControl" + i).append("<input type=\"radio\" id=\"rbYes" + i + "\" name=\"YesNo" + i + "\" value=\"Yes\" class='quecenter' />Yes");
        $("#dvControl" + i).append("<input type=\"radio\" id=\"rbNo" + i + "\" name=\"YesNo" + i + "\" value=\"No\" class='quecenter' />No");
        $("#dvControl" + i).append("<br/>");
        $("#dvControl" + i).append("<input type=\"text\" id=\"txtcomment" + i + "\" />");

    } else if (questionTypeId == 12 || questionTypeId == 13) {
        $("#dvControl" + i).append("<textarea id=\"txtPositive" + i + "\" /> ");
    } else if (questionTypeId == 14 || questionTypeId == 15) {
        $("#dvControl" + i).append("<input type=\"radio\" id=\"rbConfirmYes" + i + "\" name=\"ConfirmYesNo" + i + "\" class='quecenter' />Yes");
        $("#dvControl" + i).append("<input type=\"radio\" id=\"rbConfirmNo" + i + "\" name=\"ConfirmYesNo" + i + "\" class='quecenter' />No");
        $("#dvControl" + i).append("<br/>");
        $("#dvControl" + i).append("<textarea id=\"txtConfirmTitle" + i + "\"  /> ");
    } else if (questionTypeId == 16) {
        $("#dvControl" + i).append("<label id=\"lbl" + i + "\" /> ");
    } else if (questionTypeId == 17 && isSignature == false) {
        $("#dvControl" + i).append("<input type=\"file\" id=\"txtBrowse" + i + "\" /> ");
    } else if (questionTypeId == 17 && isSignature == true) {
        $("#dvControl" + i).append("<textarea id=\"txtSign" + i + "\" /> ");
    } else if (questionTypeId == 18 || questionTypeId == 21) {
        var count = $("#txtMOMSCount").val();
        $("#dvControl" + i).append("<select id=\"ddlMOMS" + i + "\" name=\"ddlMOMS" + i + "\">");
        if (questionTypeId == 21) {
            $("#ddlMOMS" + i).append("<option value=\"Select" + "\" data-weight=\"0" + "\" data-point=\"0\">Select</option>");
        }
        for (var j = 1; j <= count; j++) {
            var txt = "#txtMOMSOption" + j, txtWeight = "#txtWeight" + j, txtPoint = "#txtPoint" + j;
            var optionText = $(txt).val();
            $("#ddlMOMS" + i).append("<option value=\"" + optionText + "\" data-weight=\"" + $(txtWeight).val() + "\" data-point=\"0\">" + optionText + "</option>");
        }
        $("#dvControl" + i).append("</select>");
        var NA = "";
        if ($('input[name=NAValue]:checked').val() != "") {
            NA = $('input[name=NAValue]:checked').val();
            NA = $("#txtMOMSOption" + NA).val();
        }
        $("#dvControl" + i).append("<input type=\"hidden\" id=\"DefaultNAHidden" + i + "\" value=\"" + NA + "\" />");
    } else if (questionTypeId == 23) {
        var ImageHeight = $("#txtImageHeight").val();
        var Imagewidth = $("#txtImageWidth").val();
        var Imagealign = $("#dpImageAlign").val();
        $("#dvControl" + i).css("max-height", ImageHeight);
        $("#dvControl" + i).css("max-width", Imagewidth);
        $("#dvControl" + i).css("text-align", Imagealign);
        $("#dvControl" + i).append("<img src='' id='ImageGraphic" + i + "' alt='Loading...'  style='max-height:" + ImageHeight + "px; max-width:" + Imagewidth + "px' /> ");
        UploadGraphicsImage(i, '');
    }
    $("#divRating" + i).append("</div>");
    DisableControls("dvControl" + i);
    ClearControls();
    DisplayTotalQuestionCount();
    $.uniform.restore();
    jQuery('input:radio,input:checkbox').uniform();
};

function CheckEmptyOptionsTextBox(questionType) {
    if (questionType == 1) {
        if ($("#txtStartValue").val().trim() == "") {
            jAlert("Please Enter Minimum Value", "txtStartValue");
            return false;
        }
        if ($("#txtEndValue").val().trim() == "") {
            jAlert("Please Enter Maximum Value", "txtEndValue");
            return false;
        }
        var count = parseInt($("#txtEndValue").val()) - parseInt($("#txtStartValue").val()) + 1;
        if (count <= 0) {
            jAlert("Please Enter valid Minimum or Maximum value", "txtStartValue");
            return false;
        }
        var Error = "";
        for (var j = 1; j <= count; j++) {
            var txt = "#txtMOMSOption" + j;
            var optionText = $(txt).val();
            if (optionText == "") {
                Error += j + ", ";
            }
        }
        if (Error != "") {
            Error = Error.substring(0, Error.length - 1);
            jAlert("Please Enter Options" + Error + " value", "txtMOMSOption1");
            return false;
        }
        for (var j = 1; j <= count; j++) {
            var txt = "#txtWeight" + j;
            var optionText = $(txt).val();
            if (optionText == "") {
                Error += j + ", ";
            }
        }
        if (Error != "") {
            Error = Error.substring(0, Error.length - 1);
            jAlert("Please Enter Options" + Error + " Weight", "txtWeight1");
            return false;
        }
    } else if (questionType == 5 || questionType == 6 || questionType == 18 || questionType == 21) {
        var count = $("#txtMOMSCount").val();
        if (count == '' || parseInt(count) == 0) {
            jAlert("Please Enter No of Options", "txtMOMSCount");
            return false;
        }
        var Error = "";
        for (var j = 1; j <= count; j++) {
            var txt = "#txtMOMSOption" + j;
            var optionText = $(txt).val().trim();
            if (optionText == "") {
                Error += j + ", ";
            }
        }
        if (Error != "") {
            Error = Error.substring(0, Error.length - 1);
            jAlert("Please Enter Options" + Error + " value", "txtMOMSOption1");
            return false;
        }

        for (var j = 1; j <= count; j++) {
            var txt = "#txtWeight" + j;
            var optionText = $(txt).val();
            if (optionText == "") {
                Error += j + ", ";
            }
        }
        if (Error != "") {
            Error = Error.substring(0, Error.length - 1);
            jAlert("Please Enter Options" + Error + " Weight", "txtWeight1");
            return false;
        }
    } else if (questionType == 3 || questionType == 4 || questionType == 12 || questionType == 13) {
        if ($("#txtMaxLength").val().trim() == "") {
            jAlert("Please Enter Max Length", "txtMaxLength");
            return false;
        }
    } else if (questionType == 7 || questionType == 14 || questionType == 15) {
        if ($("#txtYesWeight").val() == "") {
            jAlert("Please Enter Weight for Yes", "txtYesWeight");
            return false;
        }
        if ($("#txtNoWeight").val() == "") {
            jAlert("Please Enter Weight for No", "txtNoWeight");
            return false;
        }
    } else if (questionType == 23) {
        if (document.getElementById("txtImage").files.length == 0) {
            jAlert("Please select Graphic Image", "txtImage");
            return false;
        }
    } else if (questionType == 26) {
        if (document.getElementById("uploadExcelFile").files.length == 0) {
            var r = confirm("Confirmation: \n\n " + "Are you sure to add Question without Import options");
            if (r == true) {
                return true;
            } else {
                return false;
            }
        }
    }

    return true;
};
function MultipleValue() {
    var questionTypeId = $("#QuestionTypeId").val();
    if (questionTypeId == 1 || questionTypeId == 5 || questionTypeId == 6 || questionTypeId == 7 || questionTypeId == 18 || questionTypeId == 21) {
        if ($("#chkMultipleRouting").is(":checked")) {
            $("#divMultipleRoutingPI").removeClass('hide');
            $("#txtPIMultipleRoutingValue").val(0);
        }
        else {
            $("#divMultipleRoutingPI").addClass('hide');
            $("#txtPIMultipleRoutingValue").val(0);
        }
    }
    else {
        $("#divMultipleRoutingPI").addClass('hide');
        $("#txtPIMultipleRoutingValue").val(0);
    }
    if ($("#Id").val() != "0") {
        $(".divOriginal").each(function () {
            i++;
            if ($("#chkMultipleRouting").is(":checked")) {
                $(".MultipleRouting").removeClass("hide");
            }
            else {
                $(".MultipleRouting").addClass("hide");
                $(".mrv").val(0);
            }
        });
    }
}
function DisableControls(divId) {
    $("#" + divId).children("input").attr("disabled", true);
    $("#" + divId).children("select").attr("disabled", true);
    $("#" + divId).children("textarea").attr("disabled", true);
};
function ClearControls() {
    var showQ26 = JSON.parse($("#divDatabaseReference").hasClass('hide'));
    if (!showQ26) {
        $('#divDatabaseReference').addClass('hide');
        $('#txtFile').val('');
        $('#uploadExcelFile').val('');
    }

    $("#chkIsSignature").prop("checked", false);
    $("#IsSignature").addClass('hide');

    /*---------------------------------------------Add For Group Repetitive Quetions DT.16-Mar-2017.*/
    $("#chkIsRepetitive").prop("checked", false);
    $('#tblExitingGroupList tr').each(function () {
        if (!this.rowIndex)
            return;

        var radiobuttonId = this.cells[1].innerHTML;
        $("#" + radiobuttonId).prop("checked", false);

    });

    $("#divRepetitiveBox").addClass('hide');
    $("#txtRepetitiveGroupNo").val("");
    $("#txtRepetitiveGroupName").val("");

    $("#txtQuestionTitle").val("");
    //$("#txtShortName").val("");
    $("#QuestionTypeId").val("");
    if (!$("#dvMOMS").hasClass('Hide')) {
        $("#txtMOMSCount").val("");
        $("#dvMOMSTextbox").html("");
        $("#dvMOMS").addClass('hide');
    }
    if (!$("#dvOptionsDisplayType").hasClass('Hide')) {
        $("#rdbHorizontal").attr('checked', true);
        $("#dvOptionsDisplayType").addClass('hide');
    }
    if (!$("#divMaxLength").hasClass('Hide')) {
        $("#divMaxLength").addClass('hide');
        $("#txtMaxLength").val("");
        $("#txtHint").val("");
    }

    if (!$("#divStartValue").hasClass('Hide')) {
        $("#divStartValue").addClass('hide');
        $("#txtStartValue").val("");
        $("#txtEndValue").val("");
    }
    $("#divHint").addClass('hide');
    $("#chkIsActive").prop("checked", true);
    $("#chkRequired").prop("checked", false);
    $("#chkIsDisplayInSummary").prop("checked", false);
    $("#chkIsDisplayInDetail").prop("checked", true);

    $("#divAllowDecimal").addClass('hide');
    $("#chkAllowDecimal").prop("checked", false);
    $("#DivImageSetup").addClass('hide');
    $("#InversCalculation").addClass('hide');
    $("#chkIsTitleBold").prop("checked", false);
    $("#chkIsTitleItalic").prop("checked", false);
    $("#chkIsTitleUnderline").prop("checked", false);
    $("#chkIsCommentCompulsory").prop("checked", false);
    $("#chkIsAnonymous").prop("checked", false);
    $("#IsAnonymous").addClass('hide');
    $("#chkCompulsory").addClass('hide');
    $("#txtTitleColor").val("#000000");
    $("#txtFontSize").val("13");
    $("#txtMargin").val("20");
    $('#divColorPicker').data('color', '#000000');
    $("#divColorPicker").colorpicker('update');
    $("#divGraphics").addClass('hide');
    $("#txtImage").val("");
    $('#ImgGraphic').attr('src', '');
    jQuery.uniform.update(jQuery('input:checkbox, input:radio'));
    $("#QuestionTypeId").removeAttr("disabled");
    $("#SeenClientQuestionId").val("");
    $("#txtYesWeight").val("0");
    $("#txtNoWeight").val("0");
    $("#txtPIEscalationValue").val("0");
    if ($("#rbtnTest").is(":checked")) {
        $("#dcFixedBenchMark").focus();
        MultipleValue();
        EsacalationPIValue();

    }
    else {
        $("#txtQuestionTitle").focus();
        MultipleValue();
        EsacalationPIValue();
    }
    if (contactReferenceCount > 0) {
        $("#SeenClientId").attr("disabled", "disabled");
    } else {
        $("#SeenClientId").removeAttr("disabled");
    }
    if (contactcount > 0) {
        $("#ContactId").attr("disabled", "disabled");
    } else {
        $("#ContactId").removeAttr("disabled");
    }


};
function EsacalationPIValue() {
    var questionTypeId = $("#QuestionTypeId").val();
    if (questionTypeId == 1 || questionTypeId == 5 || questionTypeId == 6 || questionTypeId == 7 || questionTypeId == 18 || questionTypeId == 21) {
        if ($("#rbtnTest").is(":checked")) {
            $("#divPIEscalation").addClass('hide');

        }
        else {
            $("#divPIEscalation").removeClass('hide');
        }
    }
    else {
        $("#divPIEscalation").addClass('hide');
    }
}
function DisplayTotalQuestionCount() {
    $('#lblTotalQuestionCount').html("TotalQuestions : " + $('#divTemplate .divOriginal').children().length);
};
function MinimumMaximumValueChange() {
    var questionType = parseInt($("#QuestionTypeId").val());
    if ($("#txtStartValue").val() == "" || $("#txtEndValue").val() == "" || questionType != 1) {
        return;
    }
    var start = parseInt($("#txtStartValue").val());
    var count = parseInt($("#txtEndValue").val()) - start + 1;

    $("#dvMOMSTextbox").html("");
    $("#dvMOMSTextbox").append('<div id="divQ0" class="col-12 col-md-8 row-fluid feedback-count" />');
    $("#divQ0").append("<span class=\"span2 col-5 font-weight-bold\">Alias Name</span>");
    $("#divQ0").append('<span class=\"span2 col-5 col-12  pl-0\"><input id=\"bnResetNaRad\" class=\"btn btn-secondary btn-inverse resetNA\" type=\"button\" tabindex=\"23\" value=\"Unmark NA\" /></span>');
    for (var j = 1; j <= count; j++) {
        $("#dvMOMSTextbox").append('<div id="divQ' + j + '" class="col-12 d-flex align-items-center" />');
        $("#divQ" + j).append("<span class=\"span2 col-3\"><input type=\"text\" onblur=\"return checkComma(this);\" onkeypress=\"return isComma(event)\" id=\"txtMOMSOption" + j + "\" placeholder=\"" + "Alias For Value" + " " + (start + j - 1) + "\" value=\"" + (start + j - 1) + "\" class=\"span12 col-12\" tabindex=\"31\" /></span>");
        $("#divQ" + j).append("<span class=\"col-md-3 col-12 d-flex align-items-center pl-4\"><input type=\"radio\" id=\"rdbNA" + j + "\" name=\"NAValue\" value=\"" + j + "\" class='marginleft10' tabindex=\"32\" />&nbsp;Mark as NA </span><br/><br/><br/>");
    }
    $("#txtMOMSOption1").focus();
    $('#frmQuestionnaire').attr('action', 'javascript:void(0);');
};
function UploadGraphicsImage(objIndex, objSource) {
    if (document.getElementById('txtImage' + objSource).files.length > 0) {
        var formData = new FormData();
        var file = document.getElementById('txtImage' + objSource).files[0];
        formData.append('txtImage', file);
        ProgressBar();
        $.ajax({
            type: "POST",
            url: '/Questionnaire/UploadFiles',
            data: formData,
            filetype: '',
            contentType: false,
            processData: false,
            async: false,
            success: function (data) {
                CloseProgressBar();
                if (data.status) {
                    $("#ImageGraphic" + objIndex).attr('src', "../CMSUpload/TempQuestions/" + data.msg);
                    $("#ImageGraphic" + objIndex).attr('data-ImageName', data.msg);
                    if (objSource != '') {
                        $("#txtImage" + objIndex).attr('data-ImageName', data.msg);
                    }
                } else {
                    jAlert(data.msg, 'txtImage');
                }
            },
            error: function (error) {
                jAlert("Problem in uploading...", 'txtImage');
                CloseProgressBar();
            }
        });
    }
};
function SaveQuestion() {
    
    var QuestionList = new Array();
    var divArray = new Array();
    var weight = 0;
    $('#divTemplate').children().each(function () {
        if ($(this).hasClass('divOriginal')) {
            divArray.push($(this).children().attr('id'));
        }
    });

    for (var i = 0; i < divArray.length; i++) {
        var Option = { Position: 0, Name: '', Value: '', DefaultValue: '', NaValue: '' };
        var Que = { QuestionTypeId: 0, Position: 0, QuestionTitle: '', ShortName: '', Required: true, DisplayInSummary: true, DisplayInDetail: true, IsActive: true, MaxLength: 0, Hint: '', EscalationRegex: 0, OptionDisplayType: '', TableGroupName: '', FontSize: 0, Margin: 0, LstOption: Option, EscalationValue: 0, ImageHeight: 0, ImageWidth: 0, ImageAlign: "", CalculationOptionId: 0, SummaryOptionId: 0 };
        var ctrl = $("#" + divArray[i]);
        var QType = ctrl.attr('data-questiontype');
        var masterId = Number(ctrl.attr('data-masterid'));
        if (masterId == 0) {
            var p = 1, index;
            Que.QuestionId = 0;
            Que.QuestionTypeId = parseInt(QType);
            Que.Position = i + 1;
            index = ctrl.attr('data-index');
            Que.QuestionTitle = $("#hdnQuestionTitle" + index).val();
            Que.ShortName = Que.QuestionTitle;
            Que.IsActive = $("#hdnIsActive" + index).val() == 'true' ? true : false;
            Que.Required = $("#hdnIsRequired" + index).val() == 'true' ? true : false;
            Que.IsDisplayInSummary = $("#hdnIsDisplayInSummary" + index).val() == 'true' ? true : false;
            Que.IsDisplayInDetail = $("#hdnIsDisplayInDetail" + index).val() == 'true' ? true : false;
            Que.MaxLength = parseInt($("#hdnMaxLength" + index).val());
            Que.Hint = $("#hdnHint" + index).val();
            Que.OptionDisplayType = $("#hdnDisplayType" + index).val();
            Que.IsTitleBold = $("#hdnIsBold" + index).val() == 'true' ? true : false;
            Que.IsTitleItalic = $("#hdnIsItalic" + index).val() == 'true' ? true : false;
            Que.IsTitleUnderline = $("#hdnIsUnderLine" + index).val() == 'true' ? true : false;
            Que.TitleTextColor = $("#hdnTextColor" + index).val();
            Que.TableGroupName = '';
            //Que.TableGroupName = QType == 19 ? $("#hdnTableGroupName" + index).val() : '';
            Que.FontSize = $("#hdnFontSize" + index).val();
            Que.Margin = $("#hdnMargin" + index).val();
            Que.ImagePath = QType == 23 ? $("#ImageGraphic" + index).attr('data-ImageName') : null;
            Que.ImageHeight = QType == 23 ? $("#hdimageheight" + index).val() : 0;
            Que.ImageWidth = QType == 23 ? $("#hdimagewidth" + index).val() : 0;
            Que.ImageAlign = QType == 23 ? $("#hdimagealign" + index).val() : '';
            Que.CalculationOptionId = $("#hdnCaclculationoption" + index).val();
            Que.SummaryOptionId = $("#hdnSummaryOption" + index).val();
            if (QType == 1) {
                Que.LstOption = new Array();
                p = 1;
                $("input[type='radio'][name='MOSS" + index + "']").each(function () {
                    Option = { Position: p, Name: $(this).attr('data-text'), Value: this.value, DefaultValue: false, NaValue: $("#DefaultNAHidden" + index).val() == this.value ? true : false };
                    Que.LstOption.push(Option);
                    p++;
                });
            } else if (QType == 5) {
                Que.LstOption = new Array();
                p = 1;
                $("input[type='checkbox'][name='MOMS" + index + "']").each(function () {
                    Option = { Position: p, Name: this.value, Value: this.value, DefaultValue: false, NaValue: $("#DefaultNAHidden" + index).val() == this.value ? true : false, point: $(this).attr('data-point') };
                    Que.LstOption.push(Option);
                    p++;
                });
            } else if (QType == 6) {
                Que.LstOption = new Array();
                p = 1;
                $("input[type='radio'][name='MOSS" + index + "']").each(function () {
                    Option = { Position: p, Name: this.value, Value: this.value, DefaultValue: $("#DefaultValueHidden" + index).val() == this.value ? true : false, NaValue: $("#DefaultNAHidden" + index).val() == this.value ? true : false, point: $(this).attr('data-point') };
                    Que.LstOption.push(Option);
                    p++;
                });
            } else if (QType == 18 || QType == 21) {
                Que.LstOption = new Array();
                p = 1;
                $("#ddlMOMS" + index + " option").each(function () {
                    Option = { Position: p, Name: this.value, Value: this.value, DefaultValue: false, NaValue: $("#DefaultNAHidden" + index).val() == this.value ? true : false, point: $(this).attr('data-point') };
                    Que.LstOption.push(Option);
                    p++;
                });
            } else if (QType == 26) {
                try {
                    var checkFile = $("#uploadExcelFile" + index).length;
                    var formData = new FormData();
                    if (checkFile > 0) {
                        var file = $("#uploadExcelFile" + index).files;
                        if (file == undefined) {
                            file = $("#uploadExcelFile" + index)[0].files[0];
                        }
                        formData.append("ImportOptions", file);
                        $.ajax({
                            type: "POST",
                            url: '/Questionnaire/ImportOptionsExcelFileData',
                            data: formData,
                            filetype: '.xls',
                            MaxFileSize: 200000,
                            dataType: 'json',
                            contentType: false,
                            processData: false,
                            async: false,
                            success: function (response) {
                                if (response.Status) {
                                    var data = response.Options;
                                    Que.LstOption = new Array();

                                    for (var i in data) {
                                        Option = { Position: 0, Name: response.Options[i], Value: response.Options[i], blDefaultValue: false, weight: 0, point: 0 };
                                        Que.LstOption.push(Option);
                                    }
                                }
                                else {
                                    Que.LstOption = null;
                                }
                            },
                            error: function (response) {
                                Que.LstOption = null;
                            }
                        });
                    }
                    else {
                        Que.LstOption = null;
                    }

                } catch (e) {
                    Que.LstOption = null;
                }
            }
            else {
                Que.LstOption = null;
            }
            QuestionList.push(Que);
        } else {
            Que.QuestionId = masterId;
            QuestionList.push(Que);
        }
    }

    var requestData = {};
    requestData.Id = $("#Id").val();
    requestData.QuestionnaireTitle = $("#QuestionnaireTitle").val().trim();
    requestData.Description = $("#Description").val().trim();
    requestData.DeletedQuestionId = deletedQuestions;
    requestData.lstQuestion = QuestionList;
    requestData.IsActive = false;
    if ($("#chkActive").is(":checked")) {
        requestData.IsActive = true;
    }
    $.ajax({
        url: '/Questionnaire/SaveQuestionnaire',
        type: 'POST',
        contentType: 'application/json;',
        dataType: 'json',
        data: JSON.stringify(requestData),
        success: function (data) {
            if (data.status) {
                jAlert(data.msg, "QuestionnaireTitle", function () {
                    window.location.href = 'QuestionnaireView';
                });
            } else {
                jAlert(data.msg, "QuestionnaireTitle");
            }
        }
    });
    return false;
};
function readURL(input, objTarget) {
    try {
        var file = input.files[0];
        if (file.type == "image/png" || file.type == "image/jpeg" || file.type == "image/jpg" || file.type == "image/gif") {
        } else {
            $(this).val('');
            jAlert("Please Select image file only." + ".", 'txtImage');
            return false;
        }
        if (input.files && input.files[0]) {
            ProgressBar();
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#ImgGraphic' + objTarget).attr('src', e.target.result);
                CloseProgressBar();
            };
            reader.readAsDataURL(input.files[0]);
        }
    } catch (e) {
        CloseProgressBar();
    }
};;