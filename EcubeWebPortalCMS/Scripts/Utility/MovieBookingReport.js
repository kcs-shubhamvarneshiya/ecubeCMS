$(document).ready(function () {
    $('#btnSearch').click(function (e) {
        if (validationMovie()) {
            LoadGrid();
        }
        else {
            $("#dvMovieReport").hide();
        }
    });
});

function LoadGrid() {
    var EDate = '';
    if ($('#MovieDate').val() != '') {
        var date = $('#MovieDate').val().split("/");
        EDate = date[1] + "/" + date[0] + "/" + date[2];
    }
    $("#tblMovieReport").GridUnload();
    $('#tblMovieReport').jqGrid({
        url: '/MovieShowReport/BindMovieReportGrid/',
        datatype: 'json',
        postData: {
            movieDate: EDate,
            movieId: parseInt($('#Movie').val()),
            movieTheatreId: parseInt($('#MovieTheatreId').val()),
            showId: parseInt($('#MovieShow').val()),
            ticketId: parseInt(jQuery('#TicketId').val()),
            memberCode: jQuery('#MemberCode').val(),
        },
        mtype: 'GET',
        colNames: [
            'Serial Id', 'Member Code', 'Member Name', 'Movie Name', 'Movie Show', 'Checked In?', 'Checked In Time', 'Checked In By'],
        colModel: [
            { name: 'BookingId', index: 'BookingId', align: 'left' },
            { name: 'MCodeWithPrefix', index: 'MCodeWithPrefix', align: 'left' },
            { name: 'MemberName', index: 'MemberName', align: 'left' },
            { name: 'MovieName', index: 'MovieName', align: 'left' },
            { name: 'MovieShowName', index: 'MovieShowName', align: 'left' },
            { name: 'IsCheckedIn', index: 'IsCheckedIn', align: 'left' },
            { name: 'CheckedInTime', index: 'CheckedInTime', align: 'left' },
            { name: 'CheckedInBy', index: 'CheckedInBy', align: 'left' }
        ],
        rowList: [],
        pgbuttons: false,
        pgtext: null,
        viewrecords: false,
        sortname: '',
        sortorder: 'DESC',
        viewrecords: true,
        caption: 'Movie Booking Report',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            $("#dvMovieReport").show();
            if (data.records == 0) {
                $('#tblMovieReport').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblMovieReport').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        }
    });
    SetMovieStyle();
}

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
    }
    else {
        $("#Movie").empty();
        $("#MovieShow").empty();
        $("#Movie").append("<option value=''>Select Movie</option>");
        $("#MovieShow").append("<option value=''>Select Movie Show</option>");
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
function validationMovie() {
    debugger
    if ((($('#MovieDate').val() == '') || (parseInt($('#MovieTheatreId').val()) == 0) || ($('#Movie').val() == "") || ($('#MovieShow').val() == "")) && ($('#TicketId').val() == "") && ($('#MemberCode').val() == "")) {
        if (($('#MovieDate').val() == '')) {
            jAlert(kcs_Message.InputRequired('Date'), 'MovieDate');
            return false;
        }
        else if ($('#MovieTheatreId').val() == "") {
            jAlert(kcs_Message.SelectRequired('Movie Theatre'), 'MovieTheatreId');
            return false;
        }
        else if (($('#Movie').val() == "")) {
            jAlert(kcs_Message.SelectRequired('Movie'), 'Movie');
            return false;
        }
        else if (($('#MovieShow').val() == "")) {
            jAlert(kcs_Message.SelectRequired('Movie Show'), 'MovieShow');
            return false;
        }
        else {
            jAlert(kcs_Message.InputRequired('Date'), 'MovieDate');
            return false;
        }
    }
    else if ((($('#MovieDate').val() == '') || (parseInt($('#MovieTheatreId').val()) == 0) || ($('#Movie').val() == "") || ($('#MovieShow').val() == "")) && ($('#TicketId').val() == "") && ($('#MemberCode').val() == "")) {
        jAlert(kcs_Message.InputRequired('Ticket Id'), 'TicketId');
        return false;
    }
    return true;
}

function SetMovieStyle() {
    $('#tblMovieReport').setGridWidth($('#dvMovieReport').width());
}

