mydata = new Array();
var names = ["SNO", "SeriolNo", "MemberNo", 'Member_Name', 'Type', 'MemberType', 'Relation', 'Date','ReceiptNo',"Photo","Print"];
var data = [];
var selectedMemberNo = 0;
$(document).ready(function () {

    $("#EventBookingForMember").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    
    $(function () {
        $.ajax({
            type: "GET",
            url: "/EventBookingForTicket/GetMemberCodeDropdown",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    if (parseInt(selectedMemberNo) != value.Value)
                        $('#MemberId').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    else
                        $('#MemberId').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#MemberId").val($('#hfdMemberNoId').val());
            }
        });
    });

    $("#MemberId").change(function () {
        $("#hfdMemberNoId").val(this.value);
    });
    if ($("#MemberId").val() == "") {
        $("#MemberId").val($("#hfdMemberNoId").val());
    }


    for (var i = 0; i < data.length; i++) {
        mydata[i] = {};
        for (var j = 0; j < data[i].length; j++) {
            mydata[i][names[j]] = data[i][j];
        }
    }

    for (var i = 0; i <= mydata.length; i++) {
        $("#tblPrintToken").jqGrid('addRowData', i + 1, mydata[i]);
    }

    
});

$('.Cancel').click(function () {
    window.location.href = 'EventBookingForEventList';
});

function Openpopup() {
    
    var mywindow = window.open('', '', 'height=1000px');
    var data = $("#DivTicket").html();
    var htmldata = "";
    htmldata += '<html><head> <style>';    
    htmldata += ' #DivBord1, #DivBord2, #DivDash, #DivBord3 { border:none !important;}';    
    htmldata += '</style>'
    htmldata += '</head><body>';
    htmldata += data;
    htmldata += '</body></html>';
    //htmldata = htmldata.replace(/600/ig, '500').replace(/175/ig, '105').replace(/1200/ig, '1080').replace(/120/ig, '80');
    console.log(htmldata);
    mywindow.document.write(htmldata);
    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10
    mywindow.print();    
    mywindow.close();

    return true;
}




$('.BtnPrint').click(function () {
    window.print();
});

function SelectedIndexChangedMemberId() {
    LoadEventPrintGrid();
}

function LoadEventPrintGrid() {
    debugger;
    var type 
    if ($("#TicketType").val() == 'Member') {
        type = 1;
    }
    else if ($("#TicketType").val() == 'Guest') {
        type = 2;
    }
    else if ($("#TicketType").val() == 'Parent') {
        type = 3;
    }
    else if ($("#TicketType").val() == 'Affiliate') {
        type = 4;
    }

    else if ($("#TicketType").val() == 'Sponser') {
        type =5;
    }

    $("#tblPrintToken").GridUnload();
    jQuery('#tblPrintToken').jqGrid({
        url: '/EventBookingForTicket/BindEventPrintTokenGrid/',
        datatype: 'json',
        postData: { MemberNo: parseInt($('#MemberId').val()), SeriolNo: parseInt($("#SeriolNo").val()), TicketType: parseInt(type), EventBookingID: $("#hdnEDID").val() },//parseInt($("#TicketType").val())        
        mtype: 'GET',
        colNames: ["Id", "S.No", "Serial No", "MemberNo", "Member Name", 'Type', 'Relation', 'Date', 'ReceiptNo', "Image", "Print"],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'rows', index: 'rows', width: 60 },
            { name: 'SeriolNo',            index: 'SeriolNo',            width: 90        },
            { name: 'MemberNo',            index: 'MemberNo',            width: 80        },
            { name: 'Member_Name',            index: 'Member_Name',            width: 80        },
            { name: 'Type',            index: 'Type',            width: 80        },        
            { name: 'Relation',            index: 'Relation',            width: 80        },
            { name: 'Date',            index: 'Date',            width: 80        },
            { name: 'RECEIPTNo',            index: 'ReceiptNo',            width: 80        },
            //{ name: 'Photo', index: 'Photo', width: 80, formatter: function () { return "<img src='' id='img1' alt='my image' style = 'width: 50px; height: 50px;' />"; } },
            { name: 'Photo', index: 'Photo', align: 'Center', width: '100px', formatter: formatImage },
            //{ name: 'Print', index: 'Print', width: 80, formatter: function () { return "<input type='button' id='btnPrint' value='Print'/>"; } }
            { name: 'Print', index: 'editoperation', align: 'left', sortable: false, width: '100px', formatter: EditFormatEventBooking }
          
          ],
        
       // pager: jQuery('#dvEventBookingFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Id',
        sortorder: 'desc',
        viewrecords: true,
        caption: 'Print Token Details',
        height: '100%',
        width: '100%',
        //multiselect: true,
        //multiselectWidth: 50,
        shrinkToFit: false,
        selectChange: function (evt, ui) {
            address = this.selection().address();

        },

        loadComplete: function (data) {
            debugger;
            if (data.records == 0) {
                $('#tblPrintToken').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblPrintToken').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
        },
        buttonclick: function (row) {

            debugger;
            alert('Hello');
            // get the data of the row that was clicked
            var rowData = $('#grid').jqxGrid('getrowdata', row);

            // perform some action with the row data, e.g. show a popup
            alert('Editing row with name ' + rowData.name);
        },
        
        
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
                i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
                cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

   

    function formatImage(cellValue, options, rowObject) {
        var imageHtml = "<img style='width: 80px; height: 90px;' src='" + cellValue + "'>";
        return imageHtml;
    }

    function EditFormatEventBooking(cellvalue, options, rowObject) {        

        return "<a Target = '_blank'  href='../EventBookingForTicket/EventBookingForTicketPrint?Id=" + options.rowId + "'>  <svg xmlns='http://www.w3.org/2000/svg' width='30' height='30' viewBox='0 0 24 24' fill='none'><path d='M18 16.75H16C15.8011 16.75 15.6103 16.671 15.4697 16.5303C15.329 16.3897 15.25 16.1989 15.25 16C15.25 15.8011 15.329 15.6103 15.4697 15.4697C15.6103 15.329 15.8011 15.25 16 15.25H18C18.3315 15.25 18.6495 15.1183 18.8839 14.8839C19.1183 14.6495 19.25 14.3315 19.25 14V10C19.25 9.66848 19.1183 9.35054 18.8839 9.11612C18.6495 8.8817 18.3315 8.75 18 8.75H6C5.66848 8.75 5.35054 8.8817 5.11612 9.11612C4.8817 9.35054 4.75 9.66848 4.75 10V14C4.75 14.3315 4.8817 14.6495 5.11612 14.8839C5.35054 15.1183 5.66848 15.25 6 15.25H8C8.19891 15.25 8.38968 15.329 8.53033 15.4697C8.67098 15.6103 8.75 15.8011 8.75 16C8.75 16.1989 8.67098 16.3897 8.53033 16.5303C8.38968 16.671 8.19891 16.75 8 16.75H6C5.27065 16.75 4.57118 16.4603 4.05546 15.9445C3.53973 15.4288 3.25 14.7293 3.25 14V10C3.25 9.27065 3.53973 8.57118 4.05546 8.05546C4.57118 7.53973 5.27065 7.25 6 7.25H18C18.7293 7.25 19.4288 7.53973 19.9445 8.05546C20.4603 8.57118 20.75 9.27065 20.75 10V14C20.75 14.7293 20.4603 15.4288 19.9445 15.9445C19.4288 16.4603 18.7293 16.75 18 16.75Z' fill='#000000'></path><path d='M16 8.75C15.8019 8.74741 15.6126 8.66756 15.4725 8.52747C15.3324 8.38737 15.2526 8.19811 15.25 8V4.75H8.75V8C8.75 8.19891 8.67098 8.38968 8.53033 8.53033C8.38968 8.67098 8.19891 8.75 8 8.75C7.80109 8.75 7.61032 8.67098 7.46967 8.53033C7.32902 8.38968 7.25 8.19891 7.25 8V4.5C7.25 4.16848 7.3817 3.85054 7.61612 3.61612C7.85054 3.3817 8.16848 3.25 8.5 3.25H15.5C15.8315 3.25 16.1495 3.3817 16.3839 3.61612C16.6183 3.85054 16.75 4.16848 16.75 4.5V8C16.7474 8.19811 16.6676 8.38737 16.5275 8.52747C16.3874 8.66756 16.1981 8.74741 16 8.75Z' fill='#000000'></path><path d='M15.5 20.75H8.5C8.16848 20.75 7.85054 20.6183 7.61612 20.3839C7.3817 20.1495 7.25 19.8315 7.25 19.5V12.5C7.25 12.1685 7.3817 11.8505 7.61612 11.6161C7.85054 11.3817 8.16848 11.25 8.5 11.25H15.5C15.8315 11.25 16.1495 11.3817 16.3839 11.6161C16.6183 11.8505 16.75 12.1685 16.75 12.5V19.5C16.75 19.8315 16.6183 20.1495 16.3839 20.3839C16.1495 20.6183 15.8315 20.75 15.5 20.75ZM8.75 19.25H15.25V12.75H8.75V19.25Z' fill='#000000'></path></svg></a > ";

       // return "<a Target = '_blank'  href='../EventBookingForTicket/EventBookingForTicketPrint?Id=" + options.rowId + "'>  <svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></a >";
    }
}




