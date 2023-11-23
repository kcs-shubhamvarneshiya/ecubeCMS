var selectedShowList = new Array();
var deletedShowList = new Array();
var existService = true;
$(document).ready(function () {
    BindMovieTheaterClass();
    if ($("#hdnShowId").val() != "0") {
        BindSelectedShowList();
    }
    if ($("#MovieTheatreId").val() == "0" || $("#MovieTheatreId").val() == "") {
        $("#MovieTheatreId").val($("#MovieTheatreId option:eq(1)").val());
        BindMovieTheaterClass();
    }
    $("#MovieShowAssignMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    $("input:checkbox").on('click', function () {
        // in the handler, 'this' refers to the box clicked on
        var IsExists = "";
        var $box = $(this);
        var movieid = $box.attr("id").split('-')[0].replace("(", "").replace(")", "")
        var ShowId = $box.attr("id").split('-')[1].replace("(", "").replace(")", "")
        var showTime = $(this).closest('tbody').find("#" + ShowId).html().trim();
        var startTime = get24Hr(showTime.split('-')[0].trim());
        var endTime = get24Hr(showTime.split('-')[1].trim());
        if ($("#MovieTheatreId").val() == '') {
            jAlert(kcs_Message.SelectRequired('Screen'), 'MovieTheatreId');
            if ($box.is(":checked")) {
                $box.parent("span").removeClass("checked");
                $box.prop("checked", false);
            } else {
                $box.prop("checked", true);
                $box.parent("span").addClass("checked");
            }
            return false;
        }
        else {
            var theatreId = $("#MovieTheatreId").val();
        }
        if ($('#StrStartDate').val() == '') {
            jAlert(kcs_Message.InputRequired('Start Date'), 'StrStartDate');
            if ($(this).is(":checked")) {
                $(this).parent("span").removeClass("checked");
                $(this).prop("checked", false);
            } else {
                $(this).prop("checked", true);
                $(this).parent("span").addClass("checked");
            }
            return false;
        }
        else {
            var StartDate = $('#StrStartDate').val().split("/")[2] + '-' + $('#StrStartDate').val().split("/")[1] + '-' + $('#StrStartDate').val().split("/")[0];
        }
        if ($('#StrEndDate').val() == '') {
            jAlert(kcs_Message.InputRequired('End Date'), 'StrEndDate');
            if ($(this).is(":checked")) {
                $(this).parent("span").removeClass("checked");
                $(this).prop("checked", false);
            } else {
                $(this).prop("checked", true);
                $(this).parent("span").addClass("checked");
            }
            return false;
        }
        else {
            var EndDate = $('#StrEndDate').val().split("/")[2] + '-' + $('#StrEndDate').val().split("/")[1] + '-' + $('#StrEndDate').val().split("/")[0];
        }

        var a1 = selectedShowList.filter(x => x.StartTime <= startTime && x.EndTime >= startTime && x.ShowId != ShowId).length;
        var a2 = selectedShowList.filter(x => x.StartTime <= endTime && x.EndTime >= endTime && x.ShowId != ShowId).length;
        var a3 = selectedShowList.filter(x => startTime <= x.StartTime && endTime >= x.StartTime && x.ShowId != ShowId).length;
        var a4 = selectedShowList.filter(x => startTime <= x.EndTime && endTime >= x.EndTime && x.ShowId != ShowId).length;;

        if (a1 > 0 || a2 > 0 || a3 > 0 || a4 > 0) {
            jAlert('Show is already exists and booked for other movie');
            if ($(this).is(":checked")) {
                $(this).parent("span").removeClass("checked");
                $(this).prop("checked", false);
            } else {
                $(this).prop("checked", true);
                $(this).parent("span").addClass("checked");
            }
            return false;
        }
        if ($box.is(":checked") == false && $("#hdnShowId").val() != "0") {
            $.post("/MovieShowAssign/IsMovieTicketBooked", { fromDate: StartDate, toDate: EndDate, movieId: movieid, theatreId: theatreId, showId: ShowId },
                function (returndata) {
                    if (returndata != "") {
                        jAlert(returndata);
                        IsExists = "1";
                    }
                    else {
                        IsExists = "0";
                    }
                })
        } else {
            $.post("/MovieShowAssign/IsMovieShowExists", { fromDate: StartDate, toDate: EndDate, movieId: 0, theatreId: theatreId, showId: ShowId },
                function (returndata) {
                    if (returndata != "") {
                        if ($("#hdnShowId").val() != "0") {
                            var d1 = deletedShowList.filter(x => x.StartTime <= startTime && x.EndTime >= startTime && x.ShowId != ShowId).length;
                            var d2 = deletedShowList.filter(x => x.StartTime <= endTime && x.EndTime >= endTime && x.ShowId != ShowId).length;
                            var d3 = deletedShowList.filter(x => startTime <= x.StartTime && endTime >= x.StartTime && x.ShowId != ShowId).length;
                            var d4 = deletedShowList.filter(x => startTime <= x.EndTime && endTime >= x.EndTime && x.ShowId != ShowId).length;
                            var d5 = deletedShowList.filter(x => startTime == x.StartTime && endTime == x.EndTime && x.ShowId == ShowId).length;
                            if (d1 > 0 || d2 > 0 || d3 > 0 || d4 > 0 || d5 > 0) {
                                IsExists = "0";
                            }
                            else {
                                jAlert(returndata);
                                IsExists = "1";
                            }
                        }
                        else {
                            jAlert(returndata);
                            IsExists = "1";
                        }
                    }
                    else {
                        IsExists = "0";
                    }
                })
        }
        if ($box.is(":checked") && IsExists != "1" && IsExists != "") {
            // the name of the box is retrieved using the .attr() method
            // as it is assumed and expected to be immutable
            var group = "input:checkbox[name='" + $box.attr("name") + "']";
            // the checked state of the group/box on the other hand will change
            // and the current value is retrieved using .prop() method
            $(group).parent("span").removeClass("checked");
            $(group).prop("checked", false);
            $box.prop("checked", true);
            $box.parent("span").addClass("checked");
        } else {
            if ($box.is(":checked") == false && IsExists == "1") {
                $box.prop("checked", true);
                $box.parent("span").addClass("checked");
            }
            else {
                $box.prop("checked", false);
                $box.parent("span").removeClass("checked");
            }
        }
        if ($box.is(":checked")) {
            var selectedShow = {};
            selectedShow.MovieId = movieid;
            selectedShow.ShowId = ShowId;
            selectedShow.StartTime = (startTime);
            selectedShow.EndTime = (endTime);
            selectedShowList.push(selectedShow);
        }
        else {
            if ($("#hdnShowId").val() != "0") {
                if (deletedShowList.filter(x => x.ShowId == ShowId).length > 0) {
                    deletedShowList = deletedShowList.filter(x => x.MovieId != movieid && x.ShowId != ShowId);
                }
                if (selectedShowList.filter(x => x.MovieId == movieid && x.ShowId == ShowId).length > 0) {
                    deletedShowList.push(selectedShowList.find(x => x.MovieId == movieid && x.ShowId == ShowId));
                }
            }
            selectedShowList = selectedShowList.filter(x => x.MovieId != movieid || x.ShowId != ShowId);
        }
    });
    $("#btnSubmit").on('click', function () {
        var strStartDate = $('#hdnStartDate').val();
        var strEndDate = $('#hdnEndDate').val();
        window.location.href = "/MovieShowAssign/MovieShowAssignView?StartDate=" + strStartDate + "&EndDate=" + strEndDate;
    })
    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]"),
            results = regex.exec(url);
        if (!results) return '';
        if (!results[2]) return 'Yes';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
    $("#btnSave").on('click', function () {
        if (existService == false) {
            jAlert('Please assign member and guest service for Same Screen. OR contact your system administrator');
            return false;
        }
        else if ($('#MovieTheatreId').val() == '') {
            jAlert(kcs_Message.InputRequired('Movie Screens'), 'MovieTheatreId');
            return false;
        }
        else if ($('#StrStartDate').val() == '') {
            jAlert(kcs_Message.InputRequired('Start Date'), 'StrStartDate');
            return false;
        }
        else if ($('#StrEndDate').val() == '') {
            jAlert(kcs_Message.InputRequired('End Date'), 'StrEndDate');
            return false;
        }
        else if ($('#DateToDisplayMovie').val() == '' || $('#TimeToDisplayMovie').val() == '') {
            jAlert(kcs_Message.InputRequired('Date and Time to Display Movie'), 'DateToDisplayMovie');
            return false;
        }
        else if ($("input:checked").length == 0) {
            jAlert(kcs_Message.SelectRequired('at least one Movie / Show Time'));
            return false;
        }
        else {
            if ($("#divClass > div").length > 0) {
                var ischeck = true;
                $("#divClass > div").each(function () {
                    var classId = $(this).find("input[type=hidden]").val();
                    var className = $(this).find(".className").html();
                    if ($('#MemberRate_' + classId).val() == '' && ischeck == true) {
                        jAlert(kcs_Message.InputRequired('Member Rate for ' + className), '#MemberRate_' + classId);
                        ischeck = false;
                    }
                    if ($('#GuestRate_' + classId).val() == '' && ischeck == true) {
                        jAlert(kcs_Message.InputRequired('Guest Rate for ' + className), '#GuestRate_' + classId);
                        ischeck = false;
                    }
                });
                if (ischeck == false) {
                    return false;
                }
            }
            var param = getParameterByName("", window.location.href);
            var MovieTheatreId = $("#MovieTheatreId").val();
            var strStartDate = $('#StrStartDate').val().split("/")[2] + '-' + $('#StrStartDate').val().split("/")[1] + '-' + $('#StrStartDate').val().split("/")[0];
            var strEndDate = $('#StrEndDate').val().split("/")[2] + '-' + $('#StrEndDate').val().split("/")[1] + '-' + $('#StrEndDate').val().split("/")[0];
            var timeToDisplayMovie = $("#TimeToDisplayMovie").val();
            var showId = $("#hdnShowId").val();
            kcs_Message.startWait();
            $("#btnSave").attr("disabled");
            $.post("/MovieShowAssign/CheckExistMovieBooking", { showId: showId },

                function (data) {
                    debugger
                    if (data == 'False') {
                        var XML = '';
                        if (XML == '') {
                            XML = "<ROOT><StartDate>" + $('#StrStartDate').val() + "</StartDate><EndDate>" + $('#StrEndDate').val() + "</EndDate>" + "<DateToDisplayMovie>" + $('#DateToDisplayMovie').val() + "</DateToDisplayMovie><TimeToDisplayMovie>" + timeToDisplayMovie + "</TimeToDisplayMovie>" + "<Movies>";
                        }

                        var MovieId = '';
                        $("input:checked").each(function (index, element) {
                            var data = this.id;
                            var CurMoiveId = data.split('-')[0].replace("(", "").replace(")", "");
                            var CurShowId = data.split('-')[1].replace("(", "").replace(")", "");
                            if (MovieId === '') {
                                XML = XML + "<Movie><Id>" + CurMoiveId + "</Id><Shows>"
                            }
                            else if (CurMoiveId !== MovieId) {
                                XML = XML + "</Shows></Movie><Movie><Id>" + CurMoiveId + "</Id><Shows>"
                            }
                            XML = XML + "<ShowId>" + CurShowId + "</ShowId>"
                            MovieId = CurMoiveId;
                        });

                        XML = XML + "</Shows></Movie></Movies></ROOT>";

                        var rateModel = new Array();
                        $("#divClass > div").each(function () {
                            debugger
                            var classModel = {};
                            classModel.Id = $(this).find("#hdfRateId").val();
                            classModel.MovieShowPeriodId = showId;
                            classModel.ClassId = $(this).find("#hdfclassId").val();
                            classModel.MemberRate = $("#MemberRate_" + classModel.ClassId).val();
                            classModel.GuestRate = $("#GuestRate_" + classModel.ClassId).val();
                            if (classModel.ClassId !== undefined && classModel.MemberRate !== undefined && classModel.GuestRate !== undefined) {
                                rateModel.push(classModel);
                            }
                        });
                        var data = { showData: escape(XML), movieTheatreId: MovieTheatreId, movieShowRateModel: rateModel, movieShowPeriodId: showId };
                        $.ajax({
                            url: '/MovieShowAssign/AssignMovieShows',
                            type: 'POST',
                            contentType: 'application/json;',
                            dataType: 'json',
                            data: JSON.stringify(data),
                            error: function (xhr, errorType, exception) {
                                var r = jQuery.parseJSON(xhr.responseText);
                                alert(r.Message);
                            },
                            success: function (data) {
                                if (data > 0) {
                                    kcs_Message.stopWait();
                                    $("#btnSave").removeAttr("disabled");
                                    jAlert("Shows Assign successfully!!");
                                    var url = "/MovieShowAssign/MovieShowAssignDisplay";
                                    document.getElementById('popup_ok').setAttribute('onclick', 'window.location.href="' + url + '"');
                                }
                                else {
                                    kcs_Message.stopWait();
                                    $("#btnSave").removeAttr("disabled");
                                    jAlert("Shows is not assign successfully!!");
                                }
                            }
                        });
                    }
                    else {
                        kcs_Message.stopWait();
                        $("#btnSave").removeAttr("disabled");
                        jAlert("Shows are already booked, So Shows are not updated!!")
                    }
                });
            $("#btnSave").removeAttr("disabled");
            return false;
        }
    });
    $('#btnCancel').on('click', function () {
        window.location.href = "/MovieShowAssign/MovieShowAssignDisplay";
    });
    $("#MovieTheatreId").change(function () {
        BindMovieTheaterClass();
    });
});

function BindMovieTheaterClass() {
    var theaterId = $("#MovieTheatreId").val();
    var showId = $("#hdnShowId").val();
    if (theaterId != "") {
        $("#divClassRate").removeClass("hide");
        jQuery.post("/MovieTheatre/GetMovieTheatreClass/", { theaterId: theaterId, movieShowPeriodId: showId },
            function (data) {
                if (data != null) {
                    $("#divClass").html('');
                    $.each(data, function (index, item) {
                        //var classHtml = '<div class=""><div class=""><div class=""><div class="d-flex">' +
                        //    '<div class="mb-3 pb-3"><h5 class="mb-2">&nbsp;</h5><h5 class="className">' + item.ClassName + '</h5><input type="hidden" id="hdfclassId" value=' + item.Id + '><input type="hidden" id="hdfRateId" value=' + item.RateId + '></div>' +
                        //    '<div class="mb-3 pb-3 w-50 pr-md-3"><h5 class="mb-2">Member Rate <span class="f_req text-danger" >*</span></h5><input type="text" class="form-control" id="MemberRate_' + item.Id + '"/></div>' +
                        //    '<div class="mb-3 pb-3 w-50 pl-md-3"><h5 class="mb-2">Guest Rate <span class="f_req text-danger" >*</span></h5><input type="text" class="form-control" id="GuestRate_' + item.Id + '"/>' +
                        //    '</div></div></div></div></div>';
                        var classHtml = '<div class="col-2 pl-0"><h5 class="className">' + item.ClassName + '</h5><input type="hidden" id="hdfclassId" value=' + item.Id + '><input type="hidden" id="hdfRateId" value=' + item.RateId + '></div>'
                        if (item.MemberSubServiceId == '0') {
                            classHtml += '<div class="col-5 pl-0 mb-3"><h5 class="mb-2">Member Rate <span class="f_req text-danger" >*</span></h5><input type="text" class="form-control" disabled id="MemberRate_' + item.Id + '"/></div>'
                        }
                        else {
                            classHtml += '<div class="col-5 pl-0 mb-3"><h5 class="mb-2">Member Rate <span class="f_req text-danger" >*</span></h5><input type="text" class="form-control" id="MemberRate_' + item.Id + '"/></div>'
                        }
                        if (item.GuestSubServiceId == '0') {
                            classHtml += '<div class="col-5 pl-0 mb-3"><h5 class="mb-2">Guest Rate <span class="f_req text-danger" >*</span></h5><input type="text" class="form-control" disabled id="GuestRate_' + item.Id + '"/>'
                        }
                        else {
                            classHtml += '<div class="col-5 pl-0 mb-3"><h5 class="mb-2">Guest Rate <span class="f_req text-danger" >*</span></h5><input type="text" class="form-control" id="GuestRate_' + item.Id + '"/>'
                        }
                        classHtml += '</div>';
                        $("#divClass").append(classHtml);
                        $("#MemberRate_" + item.Id).val(item.MemberRate);
                        $("#GuestRate_" + item.Id).val(item.GuestRate);
                        if (existService == true && (item.MemberSubServiceId == '0' || item.MemberSubServiceId == '0'))
                            existService = false;
                    });
                }
            });
    }
    else {
        $("#divClassRate").addClass("hide");
        $("#divClass").html('');
    }
}

function get24Hr(time) {
    var hours = Number(time.match(/^(\d+)/)[1]);
    var AMPM = time.match(/\s(.*)$/)[1];
    if (AMPM == "PM" && hours < 12) hours = hours + 12;
    if (AMPM == "AM" && hours == 12) hours = hours - 12;

    var minutes = Number(time.match(/:(\d+)/)[1]);
    hours = hours * 100 + minutes;
    return hours;
}

function BindSelectedShowList() {
    $("input:checked").each(function (index, element) {
        var $box = $(this);
        var movieid = $box.attr("id").split('-')[0].replace("(", "").replace(")", "")
        var ShowId = $box.attr("id").split('-')[1].replace("(", "").replace(")", "")
        var showTime = $(this).closest('tbody').find("#" + ShowId).html().trim();
        var startTime = get24Hr(showTime.split('-')[0].trim());
        var endTime = get24Hr(showTime.split('-')[1].trim());
        var selectedShow = {};
        selectedShow.MovieId = movieid;
        selectedShow.ShowId = ShowId;
        selectedShow.StartTime = (startTime);
        selectedShow.EndTime = (endTime);
        selectedShowList.push(selectedShow);
    });
}
