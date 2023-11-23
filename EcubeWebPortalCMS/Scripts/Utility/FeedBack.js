var AnswerList;
$(document).ready(function () {
    BindFeedBackAnswer();
});
function BindFeedBackAnswer() {
    var queId = $("#hdfQuestionnaireId").val();
    var mcode = $("#txtMemberCode").val();
    var fdate = $("#txtFromDate").val().split('/');
    var fromdate = fdate[1] + "/" + fdate[0] + "/" + fdate[2];
    var tdate = $("#txtToDate").val().split('/');
    var todate = tdate[1] + "/" + tdate[0] + "/" + tdate[2];
    $.ajax({
        url: "/FeedBack/GetFeedbackAnwserList",
        dataType: "json",
        data: {
            questionnaireId: queId, memberCode: mcode, fromDate: fromdate, toDate: todate
        },
        success: function (data) {
            AnswerList = data;
            $("#dvFeedBack").html('');
            if (data.length > 0) {
                $("#spantitle").html(data[0].QuestionnaireTitle);
                var datalist = data.filter(({ IsDisplayInSummary }) => IsDisplayInSummary === true);
                if (datalist.length > 0) {
                    $("#divNoRecords").css("display", "none");
                    var AnswerId = 0;
                    $.each(datalist, function (i) {
                        if (AnswerId != datalist[i].AnswerId) {
                            AnswerId = datalist[i].AnswerId;
                            var tblDetails = '<div class="ui-jqgrid ui-widget ui-widget-content ui-corner-all">' +
                                '<div class="ui-state-default ui-jqgrid-hdiv" style = "width: 100%;">' +
                                '<table class="ui-jqgrid-htable" style="width: 100%;" Id="tbl_' + datalist[i].AnswerId + '"><thead>' +
                                '<tr class="ui-jqgrid-labels"><th class="ui-state-default ui-th-column ui-th-ltr ui-state-hover d-flex justify-content-between align-items-center border-bottom"><span class="span11">Sr no. ' + datalist[i].AnswerId + '</span><a class="btn btn-inverse btn-primary btn-table-white" href="#" onclick="OnFeedBackDetails(' + datalist[i].AnswerId + ');">View Details</a></th></tr></thead>' +
                                '<tbody><tr class="ui-widget-content jqgrow ui-row-ltr"><td><span><b>' + datalist[i].AnswerDate + '</b></span><br/><span style="white-space: pre-line;">' + datalist[i].QuestionTitle + ' : ' + datalist[i].Detail + '</span></td></tr></tbody></table>' +
                                '</div></div>';
                            if (AnswerId == 0) {
                                $("#dvFeedBack").append(tblDetails);
                            }
                            else {
                                $("#dvFeedBack").append(tblDetails + '<br/>');
                            }
                        }
                        else {
                            $("#tbl_" + datalist[i].AnswerId + " tbody tr td").append("<br/><span style='white-space: pre-line;'>" + datalist[i].QuestionTitle + " : " + datalist[i].Detail + '</span>');
                        }
                    });
                }
                else {
                    $("#divNoRecords").css("display", "none");
                    var AnswerId = 0;
                    $.each(data, function (i) {
                        if (AnswerId != data[i].AnswerId) {
                            AnswerId = data[i].AnswerId;
                            var tblDetails = '<div class="ui-jqgrid ui-widget ui-widget-content ui-corner-all">' +
                                '<div class="ui-state-default ui-jqgrid-hdiv" style = "width: 100%;">' +
                                '<table class="ui-jqgrid-htable" style="width: 100%;" Id="tbl_' + data[i].AnswerId + '"><thead>' +
                                '<tr class="ui-jqgrid-labels"><th class="ui-state-default ui-th-column ui-th-ltr d-flex justify-content-between align-items-center border-bottom"><span class="span11">Sr no. ' + data[i].AnswerId + '</span><a class="btn btn-inverse btn-primary btn-table-white" href="#" onclick="OnFeedBackDetails(' + data[i].AnswerId + ');">View Details</a></th></tr></thead>' +
                                '<tbody><tr class="ui-widget-content jqgrow ui-row-ltr"><td><span><b>' + data[i].AnswerDate + '</b></span></td></tr></tbody></table>' +
                                '</div></div>';
                            if (AnswerId == 0) {
                                $("#dvFeedBack").append(tblDetails);
                            }
                            else {
                                $("#dvFeedBack").append(tblDetails);
                            }
                        }
                    });
                }
            }
            else {
                $("#divNoRecords").css("display", "");
            }

        }
    });
}
function SearchFeedBackList() {
    if ($('#txtFromDate').val() != '' && $('#txtToDate').val() != '') {
        if ($('#txtFromDate').val() > $('#txtToDate').val()) {
            jAlert('From Date must be less than To Date.', 'txtFromDate');
            return false;
        }
    }
    BindFeedBackAnswer();
}

function OnFeedBackDetails(AnsId) {
    $('#spanTitle').html("Sr no. " + AnsId);
    $('#tblFeedBackDetails tbody tr td').html('');
    var datalist = AnswerList.filter(({ AnswerId }) => AnswerId === AnsId);
    $.each(datalist, function (i) {
        var details = '';
        if (datalist[i].IsDisplayInDetail == true) {
            if (datalist[i].QuestionTypeId == 17) {
                details = "<span><b>" + datalist[i].QuestionTitle + "</b></span>";
                var attatchmentList = datalist[i].Detail.split(',');
                $.each(attatchmentList, function (a) {
                    var uploadedFileExtension = getFileExtension(attatchmentList[a]).toString().toLowerCase();
                    if (uploadedFileExtension == "png" || uploadedFileExtension == "jpg" || uploadedFileExtension == "jpeg" || uploadedFileExtension == "gif") {
                        details += '<span class="member-details--block"><span><a target="_blank" title="Click to download" href="' + $("#hdfAttatchmentURL").val() + attatchmentList[a] + '" class=""><img style="width: 200px; height: 200px;" src="' + $("#hdfAttatchmentURL").val() + attatchmentList[a] + '"/>' + attatchmentList[a] + '</a></span></span>';
                    }
                    else {
                        details += '<span class="member-details--block"><span><a target="_blank" title="Click to download" href="' + $("#hdfAttatchmentURL").val() + attatchmentList[a] + '" class="">' + attatchmentList[a] + '</a></span></span>';
                    }
                });
            }
            else if (datalist[i].QuestionTypeId == 23) {
                details = "<span class='member-details--block'><span><b>" + datalist[i].QuestionTitle + "</b></span><span><img src='../CMSUpload/Questions/" + datalist[i].Detail + "' height='" + datalist[i].ImageHeight + "' width='" + datalist[i].ImageWidth + "' /></span ></span >";
            }
            else {
                details = "<span class='member-details--block'><span><b>" + datalist[i].QuestionTitle + "</b></span><span class='feedbackDetails'>" + datalist[i].Detail + "</span ></span >";
            }

            if (i == 0) {
                $('#tblFeedBackDetails tbody tr td').append('<span><b>' + datalist[i].AnswerDate + '</b></span>' + details);
            }
            else {
                $('#tblFeedBackDetails tbody tr td').append(details);
            }
        }
    });
    $("#divDetails").css("display", "");
    $("#divList").css("display", "none");
    $("#divFilter").css("display", "none");
}
function OnBackToList() {
    $("#divDetails").css("display", "none");
    $("#divList").css("display", "");
    $("#divFilter").css("display", "");

}
function getFileExtension(name) {
    var found = name.lastIndexOf('.') + 1;
    return (found > 0 ? name.substr(found) : "");
}