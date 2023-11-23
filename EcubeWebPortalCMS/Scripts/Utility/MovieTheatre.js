var memberServiceList = [], guestServiceList = [], member = 1, guest = 2;
var defaultDataList = [{ Text: "Select", Value: "0" }];
$(document).ready(function () {
    GetSubServicelist();
    BindMovieTheaterClass();
    $("#MovieTheatreMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'MovieTheatreView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('MovieTheatreView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#MovieTheatreName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Movie Theatre Name'), 'MovieTheatreName');
            return false;
        }
        if ($('#ServiceId').val().trim() == '0') {
            jAlert(kcs_Message.SelectRequired('Service'), 'ServiceId');
            return false;
        }
        var ischeck = true;
        if ($("#divClass > div").length > 0) {
            $("#divClass > div").each(function () {
                var classId = $(this).find("input[type=hidden]").val();
                var className = $(this).find(".className").html();
                if ($('#MServiceId_' + classId).val() == '0' && ischeck == true) {
                    jAlert(kcs_Message.SelectRequired('Member Service for ' + className), '#MServiceId_' + classId);
                    ischeck = false;
                }
                if ($('#GServiceId_' + classId).val() == '0' && ischeck == true) {
                    jAlert(kcs_Message.SelectRequired('Guest Service for ' + className), '#GServiceId_' + classId);
                    ischeck = false;
                }
            });
        }
        if (ischeck == false)
            return false;
        var MovieTheatreModel = {};
        MovieTheatreModel.Id = $('#Id').val();
        MovieTheatreModel.MovieTheatreName = $('#MovieTheatreName').val();
        MovieTheatreModel.TheatreFloor = $('#TheatreFloor').val();
        MovieTheatreModel.ServiceId = $('#ServiceId').val();
        var MovieTheatreClassModel = new Array();
        $("#divClass > div").each(function () {
            var classModel = {};
            if ($(this).find("input[type=hidden]").val() != undefined && $(this).find("input[type=hidden]").val() != '') {
                classModel.Id = $(this).find("input[type=hidden]").val();
                classModel.MemberSubServiceId = $("#MServiceId_" + classModel.Id).val();
                classModel.GuestSubServiceId = $("#GServiceId_" + classModel.Id).val();
                MovieTheatreClassModel.push(classModel);
            }
        });
        var data = { movieTheatreModel: MovieTheatreModel, movieTheatreClassModel: MovieTheatreClassModel };
        $.ajax({
            url: '/MovieTheatre/SaveMovieTheatre',
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            data: JSON.stringify(data),
            error: function (xhr, errorType, exception) {
                var r = jQuery.parseJSON(xhr.responseText);
                alert(r.Message);
            },
            success: function (data) {
                if (data == 'Movie Theatre Submitted Successfully.') {
                    jAlert(data);
                    document.getElementById('popup_ok').setAttribute('onclick', 'RedirectOnView();');
                }
                else {
                    jAlert(data);
                }
            }
        });
    });
    $("#ServiceId").change(function () { GetSubServicelist(); });
});
function RedirectOnView() {
    window.location.href = '../MovieTheatre/MovieTheatreView';
}
function GetSubServicelist() {
    var serviceId = $("#ServiceId").val();
    if (serviceId != "0") {
        jQuery.post("/MovieTheatre/GetSubServiceList/", { serviceId: serviceId },
            function (data) {
                if (data != null) {
                    if (data.MemberServiceList.length > 0) {
                        memberServiceList = data.MemberServiceList;
                    }
                    else {
                        memberServiceList = defaultDataList;
                    }
                    if (data.GuestServiceList.length > 0) {
                        guestServiceList = data.GuestServiceList;
                    }
                    else {
                        guestServiceList = defaultDataList;
                    }
                    BindSubServicelist();
                }
            });
    }
    else {
        memberServiceList = defaultDataList;
        guestServiceList = defaultDataList;
        BindSubServicelist();
    }
}

function BindMovieTheaterClass() {
    var theaterId = $("#Id").val();
    jQuery.post("/MovieTheatre/GetMovieTheatreClass/", { theaterId: theaterId, movieShowPeriodId: 0 },
        function (data) {
            if (data != null) {
                if (data.length > 0) {
                    var memberOptionList = "";
                    if (memberServiceList != null && memberServiceList != '') {
                        $.each(memberServiceList, function (index, item) {
                            memberOptionList = memberOptionList + '<option value=' + item.Value + '>' + item.Text + '</option>';
                        });
                    }
                    var guestOptionList = "";
                    if (guestServiceList != null && guestServiceList != '') {
                        $.each(guestServiceList, function (index, item) {
                            guestOptionList = guestOptionList + '<option value=' + item.Value + '>' + item.Text + '</option>';
                        });
                    }
                    $.each(data, function (index, item) {
                        var classHtml = '<div class="row">' +
                            '<div class="col-md-12 className"><h5 class="mb-3">' + item.ClassName + '</h5><div class="separator mb-4 mt-2"></div></div><input type="hidden" id="hdfclassId" value=' + item.Id + '>' +
                            '</div><div class="row">' +
                            '<div class="col-md-4 mb-3 pb-3"><h5 class="mb-3">Member Service <span class="f_req text-danger">*</span></h5><select class="form-control" id="MServiceId_' + item.Id + '">' + memberOptionList + '</select></div>' +
                            '<div class="col-md-4 mb-3 pb-3"><h5 class="mb-3">Guest Service <span class="f_req text-danger">*</span></h5><select class="form-control" id="GServiceId_' + item.Id + '">' + guestOptionList + '</select>' +
                            '</div></div>';
                        $("#divClass").append(classHtml);
                        $("#MServiceId_" + item.Id).val(item.MemberSubServiceId);
                        $("#GServiceId_" + item.Id).val(item.GuestSubServiceId);
                    });
                }
                else {
                    $("#divServicehead").addClass('hide');
                }
            }
        });
}

function BindSubServicelist() {
    if ($("#divClass > div").length > 0) {
        var memberOptionList = "";
        if (memberServiceList != null && memberServiceList != '') {
            $.each(memberServiceList, function (index, item) {
                memberOptionList = memberOptionList + '<option value=' + item.Value + '>' + item.Text + '</option>';
            });
        }
        var guestOptionList = "";
        if (guestServiceList != null && guestServiceList != '') {
            $.each(guestServiceList, function (index, item) {
                guestOptionList = guestOptionList + '<option value=' + item.Value + '>' + item.Text + '</option>';
            });
        }
        $("#divClass > div").each(function () {
            var classId = $(this).find("input[type=hidden]").val();
            $("#MServiceId_" + classId).html(memberOptionList);
            $("#GServiceId_" + classId).html(guestOptionList);
        });
    }
}