$(document).ready(function () {

    $('#dvMovieTicket').hide();
    $(window).bind('resize', function () {
        SetStyle();
    });   
    $("#MovieTicketMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    LoadMovieTicketGrid();
    jQuery('#btnSearch').click(function (e) {
        $('#dvMovieTicket').show();
        LoadMovieTicketGrid();
    });
});

function LoadMovieTicketGrid() {
    $("#tblMovieTicket").GridUnload();
    $('#tblMovieTicket').jqGrid({
        url: '/MovieTicket/MovieTicketGrid/',
        postData: { id: null, memberCode: $('#txtMemberCode').val(), ticketNo: $('#txtTicketId').val() },
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Theatre', 'Movie Name', 'Ticket Id', 'Show Date', 'Show Time', 'Print'],
        colModel: [
            {  name: 'MovieTheatreName', index: 'MovieTheatreName', align: 'center', key: true },
            {  name: 'MovieName', index: 'MovieName', align: 'center', key: true },
            {  name: 'MovieBookingId', index: 'MovieBookingId', align: 'center' },
            {  name: 'ShowKeyDate', index: 'ShowKeyDate', align: 'center' },
            {  name: 'StartTime', index: 'StartTime', align: 'center' },
            { name: 'MovieBookingId', index: 'MovieBookingId', width: 40,align: 'center', sortable: false, formatter: Link },
        ],
        pager: jQuery("#dvMovieTicketFooter"),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        emptyrecords: 'No records',
        sortname: 'id',
        sortorder: 'Desc',
        viewrecords: true,
        caption: 'List of Movie',
        height: '100%',
        width: '100%',
        multiselect: false,
    });

    SetStyle();
    $("#load_tblMovieTicket").css("display", "none");
}

function SetStyle() {
    $('#tblMovieTicket').setGridWidth($('#dvMovieTicket').width());
}

function Link(cellValue, options, rowdata, action) {
    var url = "'../MovieTicket/MovieTicket?id=" + rowdata.MovieBookingId + "'";
    var name = "'_blank'";
    return '<a href="javascript:void(0)" onclick="window.open(' + url + ',' + name + ')" class="print" ><label class="IconEdit" title="Print"></label><svg width="43" height="43" viewBox="0 0 43 43" fill="none" xmlns="http://www.w3.org/2000/svg"><path d = "M7.62903 34.5806H9.94469V40.9194C9.94469 42.0687 10.876 43 12.0253 43H30.9746C32.124 43 33.0553 42.0687 33.0553 40.9194V34.5806H35.371C39.5776 34.5806 43 31.1582 43 26.9515V16.0485C43 11.8418 39.5776 8.41944 35.371 8.41944H33.0553V2.08065C33.0553 0.931283 32.124 0 30.9747 0H12.0253C10.876 0 9.94469 0.931283 9.94469 2.08065V8.41943H7.62903C3.42237 8.41943 0 11.8418 0 16.0485V26.9515C0 31.1582 3.42237 34.5806 7.62903 34.5806ZM28.894 38.8387H14.106V23.5806H28.894V38.8387H28.894ZM14.106 4.16129H28.894V8.41943H14.106V4.16129ZM4.16129 16.0485C4.16129 14.1365 5.71703 12.5807 7.62903 12.5807H35.371C37.283 12.5807 38.8387 14.1365 38.8387 16.0485V26.9515C38.8387 28.8635 37.283 30.4193 35.371 30.4193H33.0553V23.5668C34.1396 23.495 34.9991 22.6025 34.9991 21.5C34.9991 20.3506 34.0678 19.4194 32.9185 19.4194H10.0815C8.93215 19.4194 8.00087 20.3506 8.00087 21.5C8.00087 22.6025 8.86036 23.495 9.94471 23.5668V30.4193H7.62903C5.71703 30.4193 4.16129 28.8635 4.16129 26.9515V16.0485Z" fill = "white" /></svg></a>';
}

