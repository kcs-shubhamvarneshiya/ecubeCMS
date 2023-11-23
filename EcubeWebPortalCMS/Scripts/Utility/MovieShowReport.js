$(document).ready(function () {
    $("#MovieShowReportMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
});
$("#MovieDate").change(function () {
    if ($("#MovieTheatreId").val() != '') {
        BindMovie();
    }
    else {
        $("#Movie").empty();
        $("#MovieShow").empty();
        $("#Movie").append("<option value=''>Select Movie</option>");
        $("#MovieShow").append("<option value=''>Select Movie Show</option>");
    }
});
$("#MovieTheatreId").change(function () {
    if ($("#MovieTheatreId").val() != '') {
        BindMovie();
        BindMovieClass();
    }
    else {
        $("#Movie").empty();
        $("#MovieShow").empty();
        $("#MovieClassId").empty();
        $("#Movie").append("<option value=''>Select Movie</option>");
        $("#MovieShow").append("<option value=''>Select Movie Show</option>");
        $("#MovieClassId").append("<option value=''>Select Class</option>");
    }
});
function BindMovie() {
    var CurrentDate = $("#MovieDate").val();
    var d = new Date(CurrentDate.split("/").reverse().join("-"));
    var dd = d.getDate();
    var mm = d.getMonth() + 1;
    var yy = d.getFullYear();
    if (mm.toString().length === 1) { mm = "0" + mm; }
    CurrentDate = yy + "-" + mm + "-" + dd;
    $.ajax({
        url: "GetMovieListByDate", data: {
            date: CurrentDate, theatreId: $("#MovieTheatreId").val()
        }, success: function (result) {
            $("#Movie").empty();
            $("#MovieShow").empty();
            var data = ""
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    data = data + "<option value='" + result[i].Value + "'>" + result[i].Text + "</option>";
                }
            }
            else {
                data = "<option value=''>Select Movie</option>";
            }
            $("#Movie").append(data);
            $("#MovieShow").append("<option value=''>Select Movie Show</option>");
        }
    });
}
function BindMovieClass() {
    $.ajax({
        url: "GetMovieClassListBytheaterId", data: { theatreId: $("#MovieTheatreId").val() }, success: function (result) {
            $("#MovieClassId").empty();
            var data = ""
            if (result != null) {
                for (i = 0; i < result.length; i++) {
                    data = data + "<option value='" + result[i].Value + "'>" + result[i].Text + "</option>";
                }
            }
            else {
                data = "<option value=''>Select Class</option>";
            }
            $("#MovieClassId").append(data);
        }
    });
}
$("#Movie").change(function () {
    $("#MovieShow").empty();
    var CurrentDate = $("#MovieDate").val();
    var d = new Date(CurrentDate.split("/").reverse().join("-"));
    var dd = d.getDate();
    var mm = d.getMonth() + 1;
    var yy = d.getFullYear();
    if (mm.toString().length === 1) { mm = "0" + mm; }
    CurrentDate = yy + "-" + mm + "-" + dd;
    var CurrentMovie = $("#Movie").val();
    if ($("#MovieTheatreId").val() != '') {
        $.ajax({
            url: "GetMovieShowListByDate", data: { date: CurrentDate, movieId: CurrentMovie, theatreId: $("#MovieTheatreId").val() }, success: function (result) {
                var data = ""
                if (result != null) {
                    for (i = 0; i < result.length; i++) {
                        data = data + "<option value=" + result[i].Value + ">" + result[i].Text + "</option>";
                    }
                }
                else {
                    data = "<option value''>Select Movie Show</option>";
                }
                $("#MovieShow").append(data);
            }
        });
    }
});
function SearchReport() {
    var CurrentDate = $("#MovieDate").val();
    var CurrentTheater = $("#MovieTheatreId").val();
    var CurrentClass = $("#MovieClassId").val();
    var CurrentMovie = $("#Movie").val();
    var CurrentShow = $("#MovieShow").val();

    if (CurrentDate == "") {
        jAlert(kcs_Message.InputRequired("Show Date"), "MovieDate");
        return false;
    } else {
        var d = new Date(CurrentDate.split("/").reverse().join("-"));
        var dd = d.getDate();
        var mm = d.getMonth() + 1;
        var yy = d.getFullYear();
        if (mm.toString().length === 1) { mm = "0" + mm; }
        CurrentDate = yy + "-" + mm + "-" + dd;
    }

    if (CurrentTheater == "") {
        jAlert(kcs_Message.SelectRequired("Screen"), "MovieTheatreId");
        return false;
    }

    if (CurrentClass == "") {
        jAlert(kcs_Message.SelectRequired("Class"), "MovieClassId");
        return false;
    }

    if (CurrentMovie == "") {
        jAlert(kcs_Message.SelectRequired("Movie"), "Movie");
        return false;
    }

    if (CurrentShow == "") {
        jAlert(kcs_Message.SelectRequired("Movie Show"), "MovieShow");
        return false;
    }

    var d = new Date(CurrentDate);
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";
    //$("#tblMovieAuditorium").remove();
    var n = weekday[d.getDay()];
    $("#tblMovieDetails").empty();
    $("#tblMovieTheatre").empty();
    $("#tblMovieAuditorium").empty();
    var Label = "";
    Label = "<tr><td>" + $("#applicationTitle").val() + "</td><td>Screen: <span>" + $("#MovieTheatreId option:selected").text() + "</td><td id='tdMemberRate'></td><td>Date: <span>" + $("#MovieDate").val() + " " + $("#MovieShow option:selected").text() + "</span></td></tr>"
    Label = Label + "<tr><td>Movie: <span>" + $("#Movie option:selected").text() + "</span></td><td>Class: <span>" + $("#MovieClassId option:selected").text() + "</td><td id='tdGuestRate'></td><td>Day: <span>" + n + "</span></td></tr>"
    $("#tblMovieDetails").append(Label);
    $.ajax({
        url: "GetMovieShowReport", data: { date: CurrentDate, theatreId: CurrentTheater, classId: CurrentClass, movieId: CurrentMovie, showId: CurrentShow }, success: function (result) {
            var data = "";
            var CurrentRowTag = "";
            var IsMultiClass = false;
            if ($("#MovieClassId option").length > 2) {
                IsMultiClass = true;
            }

            if (result != null) {
                $("#tdMemberRate").html("Member rate: <span>" + parseFloat(result[0].MemberRate).toFixed(2) + "/- </span>");
                $("#tdGuestRate").html("Guest rate: <span>" + parseFloat(result[0].GuestRate).toFixed(2) + "/- </span>");
                for (i = 0; i < result.length; i++) {
                    if (CurrentRowTag != result[i].RowTag) {
                        data = data + "<tr><td style='padding-right: 10px !important; font-weight:bold; font-size:50px; border:none;'>" + result[i].RowTag + "</td>"
                    }
                    CurrentRowTag = result[i].RowTag;
                    if (result[i].SpaceAfter > 0) {
                        for (j = 0; j < result[i].SpaceAfter; j++) {
                            data = data + "<td class='profileinner-img' style='border: 1px solid #0000; background:none;'>&nbsp;</td>"
                        }
                    }

                    if (IsMultiClass == true) {
                        data = data + "<td class='profileinner-img' style='border-radius: 0% !important;'><span>" + result[i].SeatNum + "</span><hr/>";
                        if (result[i].Relation == "Self") {
                            data = data + "<b>M-" + result[i].MCodeWithPrefix + "</b>"
                        }
                        else if (result[i].Relation == "Guest") {
                            data = data + "<b>G-" + result[i].MCodeWithPrefix + "</b>"
                        }
                        else if (result[i].Relation != "") {
                            data = data + "<b>SM-" + result[i].MCodeWithPrefix + "</b>"
                        }
                    }
                    else {
                        data = data + "<td class='profileinner-img'><span>" + result[i].SeatNum + "</span><hr/><b>" + result[i].MCodeWithPrefix + "</b><br/>";
                        if (result[i].Relation != "Guest") {
                            data = data + result[i].Name;
                        }
                        if (result[i].Relation != "") {
                            data = data + "<br/>( " + result[i].Relation + " )";
                        }
                    }
                    data = data + "</td>"

                    if (result[i].SpaceBefore > 0) {
                        for (j = 0; j < result[i].SpaceBefore; j++) {
                            data = data + "<td class='profileinner-img' style='border:none !important; background:none;'>&nbsp;</td>"
                        }
                    }

                    if (CurrentRowTag != result[i].RowTag) {
                        data = data + "</tr>"
                    }
                }
            }
            else {
                data = "<p>No record found.</p>";
            }
            if (IsMultiClass == true) {
                $("#tblMovieAuditorium").append(data);
            }
            else
                $("#tblMovieTheatre").append(data);
        }
    });
}
function Openpopup() {
    var mywindow = window.open('', '', 'height=1272,width=2000');
    var data = $("#dvMovieTheatre").html();
    mywindow.document.write('<html><head><title></title><style>');
    mywindow.document.write('#tblMovieTheatre tr {         margin:10px;         height:11.11%     }     #tblMovieTheatre tr td{         vertical-align:central;         text-align:center;         font-size:7px;         border:1px solid;       width:65px;height:65px;           padding:0px !important;         line-height:9px;     }      #tblMovieDetails {         font-size:15px;         font-weight:bold;         margin-bottom:10px;         border-bottom:1px solid black;     }      #tblMovieDetails td{         padding:10px;     }      #tblMovieDetails td span{         font-weight:normal;     }     br {        display: block;         margin: 1px 0; content: " ";             }         #tblMovieTheatre tr td b {             display:block;             margin-top:1px;             font-size:8px;         }          #tblMovieTheatre tr td span {             font-size:12px;             font-weight: bold;             line-height:12px;             display:block;             margin-top:1px;         }       hr {         padding:0;         margin:0;         border:1px solid black;     }     .profileinner-img{background:#fff; width:100%; overflow:hidden; height:100%; vertical-align:middle; padding:0px; display:inline-block;margin:5px;border-radius: 100%;}     #tblMovieAuditorium tr {margin: 10px;} #tblMovieAuditorium tr td {vertical - align: central;text - align: center; font - size: 10px; border: 1px solid; width: 16px; height: 110px; padding: 0px!important; line - height: 11px;} #tblMovieAuditorium tr td b { display: block; margin-top: 5px; font-size: 12px; writing-mode: vertical-rl; } #tblMovieAuditorium tr td span {font - size: 20px;font - weight: bold; line - height: 25px;display: block;margin - top: 3px;}');
    mywindow.document.write('</style>');
    mywindow.document.write('</head><body style="border:1px solid;" >');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10

    mywindow.print();
    mywindow.close();

    return true;
}
