var blAddSchedule, blEditSchedule, blDeleteSchedule, blAddRate, blEditRate, blDeleteRate;
$(document).ready(function () {
    $("#Event").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    blAddSchedule = $('#hfAddSchedule').val();
    blEditSchedule = $('#hfEditSchedule').val();
    blDeleteSchedule = $('#hfDeleteSchedule').val();
    clientName = $('#hdnClientName').val();

    blAddRate = $('#hfAddRate').val();
    blEditRate = $('#hfEditRate').val();
    blDeleteRate = $('#hfDeleteRate').val();
   if ($("#lblEventAttachment").text() == '') {
        $("#ImgFile").hide();
    }

    if ($("#lblEventAttachment").text() == '') {
        $("#ImgFile").hide();
    }

    $(".dmmyyyyFromToDatePicker").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        //buttonImage: "../Calendar/CalendarIcn.png",
        //showOn: "both",
        buttonText: "Select date",
        beforeShow: function customRange(input) {

            if ($(input).attr('class').indexOf('FromDatePicker') != -1) {
                var ToDate = $('.ToDatePicker').datepicker("getDate");
                return {
                    minDate: 0,
                    maxDate: ToDate == null ? +30 : ToDate,
                };
            } else if ($(input).attr('class').indexOf('ToDatePicker') != -1) {
                var fromDate = $('.FromDatePicker').datepicker("getDate");
                return {
                    minDate: fromDate === null ? 0 : fromDate,
                };
            }
        }
    });

    $('#IsMaximumTicketForFamilyMember').click(function () {
        ShowHideMaximumTicketForFamilyMember(this.checked);
    });
    
    if ($('#IsCheckOutstanding').is(':checked')) {
        $("#divMemberOutstanding").css("visibility", "visible");
    }
    else { $("#divMemberOutstanding").css("visibility", "hidden"); }
    
    $('#IsCheckOutstanding').click(function () {
        if ($(this).is(":checked")) {
            $("#divMemberOutstanding").css("visibility", "visible");
        } else {
            $("#divMemberOutstanding").css("visibility", "hidden");
        }
    });

    $(".EventLastRegistration").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        minDate: 0,
        buttonText: "Select date"
    });

    $("#dvEvent").show();
    if ($('#hfdId').val() == "0") {
        $("#dvSchedule").hide();
        $("#dvRate").hide();
    }
    else {
        LoadEventScheduleGrid();
        LoadEventRateGrid();
    }

    $('#EventTicketCategoryId').val(0);
    $('#RegistrationDate').datepicker({ minDate: new Date(1999, 10 - 1, 25) });
    var currentDate = new Date();
    $("#ObjEventScheduleModel_EventFromDate").datepicker({ minDate: currentDate, defaultDate: currentDate });

    $(function () {
        $.ajax({
            type: "GET",
            url: "/EventBooking/GetEventCategory",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    if (parseInt(selectedEventCategory) != value.Value)
                        $('#EventCategoryId').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    else
                        $('#EventCategoryId').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#EventCategoryId").val($('#hfdEventCategoryId').val());
            }
        });
    });

    $("#EventCategoryId").change(function () {
        $("#hfdEventCategoryId").val(this.value);
    });
    if ($("#EventCategoryId").val() == "") {
        $("#EventCategoryId").val($("#hfdEventCategoryId").val());
    }

    $(function () {
        $.ajax({
            type: "GET",
            url: "/EventBooking/GetEventTicketCategory",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    if (parseInt(selectedEventTicketCategory) != value.Value)
                        $('#EventTicketCategoryId').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    else
                        $('#EventTicketCategoryId').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#EventTicketCategoryId").val($('#hfdEventTicketCategoryId').val());
            }
        });
    });

    $("#EventTicketCategoryId").change(function () {
        $("#hfdEventTicketCategoryId").val(this.value);
    });
    if ($("#EventTicketCategoryId").val() == "") {
        $("#EventTicketCategoryId").val($("#hfdEventTicketCategoryId").val());
    }

    $(function () {
        $.ajax({
            type: "GET",
            url: "/EventBooking/GetAccountHead",
            datatype: "Json",
            success: function (data) {
                $.each(data, function (index, value) {
                    if (parseInt(selectedAccounthead) != value.Value)
                        $('#AccountheadId').append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    else
                        $('#AccountheadId').append('<option value="' + value.Value + '" selected=true>' + value.Text + '</option>');
                });
                $("#AccountheadId").val($('#hfdAccountheadId').val());
            }
        });
    });

    $("#AccountheadId").change(function () {
        $("#hfdAccountheadId").val(this.value);
    });
    if ($("#AccountheadId").val() == "") {
        $("#AccountheadId").val($("#hfdAccountheadId").val());
    }
    ShowHideMaximumTicketForFamilyMember($('#IsMaximumTicketForFamilyMember')[0].checked
        //ShowHideMemberOutstanding($('#IsCheckOutstanding')[0].checked)
    );
});

$('#btnEventSubmit').click(function () {
    var IsEvent = ValidateEvent();
    if (IsEvent) {
        
        var artistsInfo = CKEDITOR.instances.ArtistsInfo.getData();
        var eventSynopsis = CKEDITOR.instances.EventSynopsis.getData();
        var termsConditions = CKEDITOR.instances.TermsConditions.getData();
        //var dt = $('#OutstandingAmountForMember').val();
        var date = $('#RegistrationDate').val().split("/");
        var newDate = date[1] + "/" + date[0] + "/" + date[2];
        kcs_Message.startWait();
        $.ajax({
            url: "/EventBooking/EventSave",
            type: 'POST',
            data: {
                Id: $('#hfdId').val(),
                EventCategoryId: $('#EventCategoryId').val(),
                EventTitle: $('#EventTitle').val(),
                EventScreen: $('#EventScreen').val(),
                EventPlace: $('#EventPlace').val(),
                EventDuration: $('#EventDuration').val(),
                EventStartTime: $('#EventStartTime').val(),
                EventEndTime: $('#EventEndTime').val(),
                EventEntryBefore: $('#EventEntryBefore').val(),
                EventEntryAfter: $('#EventEntryAfter').val(),
                RegistrationDate: newDate,
                ArtistsInfo: artistsInfo.toString(),
                EventSynopsis: eventSynopsis.toString(),
                TermsConditions: termsConditions.toString(),
                EventImage: $('#EventImage').val(),
                EventBannerImage: $('#EventBannerImage').val(),
                EventMobileImage: $('#EventMobileImage').val(),
                EventAttachment: $('#EventAttachment').val(),
                HfnEventImage: $('#hdnEventImage').val(),
                HdnEventBannerImage: $('#hdnEventBannerImage').val(),
                HdnEventMobileImage: $('#hdnEventMobileImage').val(),
                HdnEventAttachment: $('#hdnEventAttachment').val(),
                AccountHeadId: $('#AccountHeadId').val(),
                TaxMasterId: $('#TaxMasterId').val(),
                GuestLimit: $('#GuestLimit').val(),
                RegistrationEndTime: $('#RegistrationEndTime').val(),
                IsMaximumTicketForFamilyMember: $('#IsMaximumTicketForFamilyMember')[0].checked,
                MaximumTicketForFamilyMember: $('#MaximumTicketForFamilyMember').val(),
                IsCheckOutstanding: $('#IsCheckOutstanding')[0].checked,
                //OutstandingAmountForMember:dt,
                OutstandingAmountForMember: $('#OutstandingAmountForMember').val(),
                IsQRCode: $('input[id=IsQRCode]:checked').val(),
                IsDisplayDetail: $('#IsDisplayDetail')[0].checked,
                AllowMultipleCategory: $('#AllowMultipleCategory')[0].checked
            },
            datatype: "Json",
            success: function (data) {
                kcs_Message.stopWait();
                if (Math.floor(data) > 0 && $.isNumeric(data)) {
                    jAlert("Event Submitted Successfully.");
                    $('#hfdId').val(data);
                    setTimeout(function () { LoadEventScheduleGrid(); }, 1000);
                    $("#dvSchedule").show();
                    $('#hfRegistrationDate').val($('#RegistrationDate').val());
                }
                else {
                    jAlert(data);
                }
            }
        });
    }
});

$("#btnScheduleAdd").click(function () {
    var IsSchedule = ValidateSchedule();
    if (IsSchedule) {
        var date = $('#ObjEventScheduleModel_EventFromDate').val().split("/");
        var fDate = date[1] + "/" + date[0] + "/" + date[2];
        var Todate = $('#ObjEventScheduleModel_EventToDate').val().split("/");
        var tDate = Todate[1] + "/" + Todate[0] + "/" + Todate[2];
        $.ajax({
            url: "/EventBooking/SaveEventSchedule",
            type: 'POST',
            data: {
                Id: $('#hfdScheduleId').val(),
                EventId: $('#hfdId').val(),
                FromDate: fDate,
                ToDate: tDate
            },
            datatype: "Json",
            success: function (data) {
                if (data == "Event Schedule Submitted Successfully.") {
                    $("#ObjEventScheduleModel_EventFromDate").val('');
                    $("#ObjEventScheduleModel_EventToDate").val('');
                    jAlert(data);
                    $('#tblEventSchedule').trigger('reloadGrid', [{ page: 1, current: true }]);
                }
                else {
                    jAlert(data);
                }
                var currentDate = new Date();
                $("#ObjEventScheduleModel_EventFromDate").datepicker({ minDate: currentDate, defaultDate: currentDate });
                $('#hfdScheduleId').val("");
                $("#ObjEventScheduleModel_EventFromDate").attr("disabled", false);
            }
        });
    }
});

$("#btnRateAdd").click(function () {
    var IsRate = ValidateRate();
    var postData = $("#frmEvent").serializeArray();
    if (IsRate) {
        $.ajax({
            url: "/EventBooking/SaveEventRate",
            type: 'POST',
            data: postData,//{
            //Id: $('#hfdRateId').val(),
            //EventId: $('#hfdId').val(),
            //EventTicketCategoryId: $('#EventTicketCategoryId').val(),
            //MemberFees: $('#ObjEventRateModel_MemberFees').val(),
            //GuestFees: $('#ObjEventRateModel_GuestFees').val(),
            //NoOfSeats: $('#ObjEventRateModel_NoOfSeats').val(),
            //SubMemberFees: $('#ObjEventRateModel_SubMemberFees').val(),
            //MemberSeatLimit: $('#ObjEventRateModel_MemberSeatLimit').val(),
            //GuestSeatLimit : $('#ObjEventRateModel_GuestSeatLimit').val()
            //},
            datatype: "Json",
            success: function (data) {
                if (data == "Event Rate Submitted Successfully.") {
                    jAlert(data);
                    $('#tblEventRate').trigger('reloadGrid', [{ page: 1, current: true }]);
                }
                else {
                    jAlert(data);
                }
                $('#EventTicketCategoryId').val("");
                $('#AccountheadId').val("");
                $('#ObjEventRateModel_MemberFees').val("");
                $('#ObjEventRateModel_GuestFees').val("");
                $('#ObjEventRateModel_NoOfSeats').val("");
                $('#ObjEventRateModel_SubMemberFees').val("");
                $('#ObjEventRateModel_MemberSeatLimit').val("");
                $('#ObjEventRateModel_GuestSeatLimit').val("");
                $('#hfdRateId').val("");
                $('#EventTicketCategoryId').focus();
                $("#EventTicketCategoryId").attr("disabled", false);
            }
        });
    }
});

$("#btnRateAddUmed").click(function () {
    var IsRate = ValidateRate(1);
    var postData = $("#frmEvent").serializeArray();
    if (IsRate) {
        $.ajax({
            url: "/EventBooking/SaveEventRate",
            type: 'POST',
            data: postData,//{
            //Id: $('#hfdRateId').val(),
            //EventId: $('#hfdId').val(),
            //EventTicketCategoryId: $('#EventTicketCategoryId').val(),
            //MemberFees: $('#ObjEventRateModel_MemberFees').val(),
            //GuestFees: $('#ObjEventRateModel_GuestFees').val(),
            //NoOfSeats: $('#ObjEventRateModel_NoOfSeats').val(),
            //SubMemberFees: $('#ObjEventRateModel_SubMemberFees').val(),
            //MemberSeatLimit: $('#ObjEventRateModel_MemberSeatLimit').val(),
            //GuestSeatLimit : $('#ObjEventRateModel_GuestSeatLimit').val()
            //},
            datatype: "Json",
            success: function (data) {
                if (data == "Event Rate Submitted Successfully.") {
                    jAlert(data);
                    $('#tblEventRate').trigger('reloadGrid', [{ page: 1, current: true }]);
                }
                else {
                    jAlert(data);
                }
                $('#EventTicketCategoryId').val("");
                $('#AccountheadId').val("");
                $('#ObjEventRateModel_MemberFees').val("");
                $('#ObjEventRateModel_GuestFees').val("");        
                $("#ObjEventRateModel_SubMemberFees").val("");
                $("#ObjEventRateModel_AffiliateMemberFee").val("");
                $("#ObjEventRateModel_ParentFee").val("");
                $("#ObjEventRateModel_GuestChildFee").val("");
                $("#ObjEventRateModel_GuestRoomFee").val("");
                $("#ObjEventRateModel_FromSerialNo").val("");
                $("#ObjEventRateModel_ToSerialNo").val("");
                $("#ObjEventRateModel_ToSerialNo").val("");
                $('#ObjEventRateModel_NoOfSeats').val("");
                $('#EventTicketCategoryId').focus();
                $("#EventTicketCategoryId").attr("disabled", false);
            }
        });
    }
});

$("#btnScheduleNext").click(function () {
    $("#dvRate").show();
    LoadEventRateGrid();
});

$(".Cancel").click(function () {
    window.location = "/EventBooking/EventBookingView";
});

$('#EventImage').on('change', function () {
    var fileUpload = document.getElementById("EventImage");    
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.png|.gif)$");
    if (regex.test(fileUpload.value.toLowerCase())) {
       if (typeof (fileUpload.files) != "undefined") {
           var reader = new FileReader();
            
            reader.readAsDataURL(fileUpload.files[0]);
            reader.onload = function (e) {
               var image = new Image();
                image.src = reader.result;
                image.onload = function () {
                   checkfile("EventImage");
                    var postdata = $('#login-form').serialize();
                    var file = document.getElementById('EventImage').files[0];
                    var fd = new FormData();
                    fd.append("files", file);
                    var xhr = new XMLHttpRequest();
                    //xhr.open("POST", "/EventBooking/UploadEventImage", false);
                    xhr.open("POST", "/EventBooking/UploadEventImage", false);
                    
                    xhr.send(fd);
                    $("#aEventImage").attr('src', image.src);
                 };
            }
        }
        else {
            $('#EventImage').val('');
            jAlert("Please Upload File In A Correct Format.");
            return false;
        }
    }
    else {
        $('#EventImage').val('');
        jAlert("Please Upload File In A Correct Format.");
        return false;
    }
});


$('#EventBannerImage').on('change', function () {
    var fileUpload = document.getElementById("EventBannerImage");
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.png|.gif)$");
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
                    checkfile("EventBannerImage");
                    var postdata = $('#login-form').serialize();
                    var file = document.getElementById('EventBannerImage').files[0];
                    var fd = new FormData();
                    fd.append("files", file);
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", "/EventBooking/UploadEventBannerImage", false);
                    xhr.send(fd);
                    $("#aEventBannerImage").attr('src', image.src);
                };
            }
        }
        else {
            $('#aEventBannerImage').val('');
            jAlert("Please Upload File In A Correct Format.");
            return false;
        }
    }
    else {
        $('#aEventBannerImage').val('');
        jAlert("Please Upload File In A Correct Format.");
        return false;
    }
});

$('#EventAttachment').on('change', function () {
   var fileUpload = document.getElementById("EventAttachment");
    var size = 10 * 1024 * 1024
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.pdf|.xlsx|.docx|.jpg|.jpeg|.png)$");
    if (fileUpload.files[0].size > size) {
        jAlert("Please upload valid Image size as 10 MB.");
        return false;
    }
   
    if (regex.test(fileUpload.value.toLowerCase())) {
        var postdata = $('#login-form').serialize();
        var file = document.getElementById('EventAttachment').files[0];
       
        var fd = new FormData();
        fd.append("files", file);
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/EventBooking/UploadEventAttachment", false);
        xhr.send(fd);
      $("#lblEventAttachment").text(file.name);
        var file = $(this).val().split('.').pop();
        var image = new Image();
        if (file.toLowerCase() === "png" || file.toLowerCase() === "jpg" || file.toLowerCase() === "jpeg") {
            var reader = new FileReader();
            reader.readAsDataURL(fileUpload.files[0]);
            reader.onload = function (e) {
                image.src = reader.result;
                $("#DocFile").hide();
                $("#PDFfile").hide();
                $("#XlsFile").hide();
                $("#AttachmentImage").attr('src', image.src).show();
            }
        }
        //if (file.toLowerCase() === "png") {
        //    $("#DocFile").hide();
        //    $("#PDFfile").hide();
        //    $("#XlsFile").hide();
        //    $("#AttachmentImage").attr('src', image.src).show();
        //}
        //else if (file.toLowerCase() === "jpg") {
        //    $("#DocFile").hide();
        //    $("#PDFfile").hide();
        //    $("#XlsFile").hide();
        //    $("#AttachmentImage").attr('src', image.src).show();
        //}
        else  if (file.toLowerCase() === "xlsx") {
            $("#AttachmentImage").hide();
            $("#DocFile").hide();
            $("#PDFfile").hide();
            $("#XlsFile").show();
        }
        else if (file.toLowerCase() === "pdf") {
            $("#AttachmentImage").hide();
            $("#DocFile").hide();
            $("#XlsFile").hide();
            $("#PDFfile").show();
        }
        else if (file.toLowerCase() === "docx") {
            $("#AttachmentImage").hide();
            $("#PDFfile").hide();
            $("#XlsFile").hide();
            $("#DocFile").show();
        }
    }
    else {
        $('#EventAttachment').val('');
        jAlert("Please upload file in a correct format.");
        return false;
    }
});

$('#EventMobileImage').on('change', function () {
   var fileUpload = document.getElementById("EventMobileImage");
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.png|.gif)$");
    if (regex.test(fileUpload.value.toLowerCase())) {
        if (typeof (fileUpload.files) !== "undefined") {
           var reader = new FileReader();
            reader.readAsDataURL(fileUpload.files[0]);
            reader.onload = function (e) {
               var image = new Image();
                image.src = reader.result;
                image.onload = function () {
                 var height = image.height;
                    var width = image.width;
                    checkfile("EventMobileImage");
                    var postdata = $('#login-form').serialize();
                    var file = document.getElementById('EventMobileImage').files[0];
                    var fd = new FormData();
                    fd.append("files", file);
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", "/EventBooking/UploadMobileImage", false);
                    xhr.send(fd);
                    $("#aEventMobileImage").attr('src', image.src);
                };
            }
        }
        else {
            $('#EventImage').val('');
            jAlert("Please Upload File In A Correct Format.");
            return false;
        }
    }
    else {
        $('#EventImage').val('');
        jAlert("Please Upload File In A Correct Format.");
        return false;
    }
});

function ValidateEvent() {
    if ($('#EventCategoryId').val() == null || $('#EventCategoryId').val() == "" || $('#EventCategoryId').val() == 0) {
        jAlert(kcs_Message.SelectRequired('Event Category'), 'EventCategoryId');
        return false;
    }
    if ($('#EventTitle').val() == '') {
        jAlert(kcs_Message.InputRequired('Event Title'), 'EventTitle');
        return false;
    }
    if ($('#EventScreen').val() == '') {
        jAlert(kcs_Message.InputRequired('Event Screen'), 'EventScreen');
        return false;
    }
    if ($('#EventPlace').val() == '') {
        jAlert(kcs_Message.InputRequired('Event Place'), 'EventPlace');
        return false;
    }
    if ($('#EventEntryBefore').val() == '') {
        jAlert(kcs_Message.InputRequired('Event entry before start time'), 'EventPlace');
        return false;
    }
    if ($('#EventEntryAfter').val() == '') {
        jAlert(kcs_Message.InputRequired('Event entry before after time'), 'EventPlace');
        return false;
    }

    ////   --- Start : Code commented by Karan Shah on date : 27-MAR-2017 as per ticket : KA-03-77
    ////if ($('#EventDuration').val() == '') {
    ////    jAlert(kcs_Message.InputRequired('Event Duration'), 'EventDuration');
    ////    return false;
    ////}
    ////   -- End

    if ($('#RegistrationDate').val() == '') {
        jAlert(kcs_Message.InputRequired('Last Registration Date'), 'RegistrationDate');
        return false;
    }
    else {
        var selecteddate = $('#RegistrationDate').val();
        var date = selecteddate.substring(0, 2);
        var month = selecteddate.substring(3, 5);
        var year = selecteddate.substring(6, 10);
        var myDate = new Date(year, month - 1, date);
        var today = new Date();
        if (myDate < today) {
            jAlert(kcs_Message.InputRequired('Last Registration Date Greater Than Current Date'), 'RegistrationDate');
            return false;
        }
    }    
    if ($('#AccountHeadId').val() == null || $('#AccountHeadId').val() == "" || $('#AccountHeadId').val() == 0) {
        jAlert(kcs_Message.SelectRequired('Account Head'), 'AccountHeadId');
        return false;
    }
    if ($('#EventStartTime').val() == '') {
        jAlert(kcs_Message.InputRequired('Event Start Time.'), 'EventStartTime');
        return false;
    }
    if ($('#EventEndTime').val() == '') {
        jAlert(kcs_Message.InputRequired('Event End Time.'), 'EventEndTime');
        return false;
    }

    ////   --- Start : Code commented by Karan Shah on date : 27-MAR-2017 as per ticket : KA-03-77
    ////if ($('#TaxMasterId').val() == null || $('#TaxMasterId').val() == "" || $('#TaxMasterId').val() == 0) {
    ////    jAlert(kcs_Message.SelectRequired('Service Tax'), 'TaxMasterId');
    ////    return false;
    ////}
    ////  -- End

    if ($('#GuestLimit').val() == null || $('#GuestLimit').val() == "") {
        jAlert(kcs_Message.SelectRequired('Guest Limit'), 'GuestLimit');
        return false;
    }

    if ($('#RegistrationEndTime').val() == '') {
        jAlert(kcs_Message.InputRequired('Event Registration End Time.'), 'RegistrationEndTime');
        return false;
    }

    if ($('#IsMaximumTicketForFamilyMember')[0].checked == false) {
        if ($('#MaximumTicketForFamilyMember').val() == null || $('#MaximumTicketForFamilyMember').val() == "") {
            jAlert(kcs_Message.SelectRequired('Maximum Ticket For Family Member.'), 'MaximumTicketForFamilyMember');
            return false;
        }
    }
  
    ////   --- Start : Code commented by Karan Shah on date : 27-MAR-2017 as per ticket : KA-03-77
    ////if ($('#hdnEventBannerImage').val() == '' || $('#hdnEventBannerImage').val() == null) {
    ////    if ($('#EventBannerImage').val() == '') {
    ////        jAlert(kcs_Message.SelectRequired('Event Banner Image'), 'EventBannerImage');
    ////        return false;
    ////    }
    ////    else {
    ////        checkfile("EventBannerImage");
    ////    }
    ////}
    ////if ($('#hdnEventImage').val() == '' || $('#hdnEventImage').val() == null) {
    ////    if ($('#EventImage').val() == '') {
    ////        jAlert(kcs_Message.SelectRequired('Event List Image'), 'EventImage');
    ////        return false;
    ////    }
    ////    else {
    ////        checkfile("EventImage");
    ////    }
    ////}
    ////   -- End



    return true;
}

function ValidateSchedule() {
    var fullDate = new Date()
    var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);
    var currentDate = fullDate.getDate() + "/" + twoDigitMonth + "/" + fullDate.getFullYear();

    if ($('#ObjEventScheduleModel_EventFromDate').val() == '') {
        jAlert(kcs_Message.SelectRequired('Event From Date'), 'ObjEventScheduleModel_EventFromDate');
        return false;
    }
    if ($('#ObjEventScheduleModel_EventToDate').val() == '') {
        jAlert(kcs_Message.SelectRequired('Event To Date'), 'ObjEventScheduleModel_EventToDate');
        return false;
    }
    else {
        var fromDate = $('#ObjEventScheduleModel_EventFromDate').val();
        var fdate = fromDate.substring(0, 2);
        var fmonth = fromDate.substring(3, 5);
        var fyear = fromDate.substring(6, 10);
        var myfDate = new Date(fyear, fmonth - 1, fdate);
        var todate = $('#ObjEventScheduleModel_EventToDate').val();
        var tdate = todate.substring(0, 2);
        var tmonth = todate.substring(3, 5);
        var tyear = todate.substring(6, 10);
        var mytDate = new Date(tyear, tmonth - 1, tdate);
        if (mytDate < myfDate) {
            jAlert(kcs_Message.InputRequired('Event to Date  greater than Event from Date'), 'ObjEventScheduleModel_EventToDate');
            return false;
        }
    }
    return true;
}

function ValidateRate(Id) {
    if (Id != 1) {
        if ($('#EventTicketCategoryId').val() === "") {
            jAlert(kcs_Message.SelectRequired('Event Ticket Category'), 'EventTicketCategoryId');
            return false;
        }
        if ($('#ObjEventRateModel_MemberFees').val() === '') {
            jAlert(kcs_Message.InputRequired('Member Fees'), 'ObjEventRateModel_MemberFees');
            return false;
        }
        if ($('#ObjEventRateModel_GuestFees').val() === '') {
            jAlert(kcs_Message.InputRequired('Guest Fees'), 'ObjEventRateModel_GuestFees');
            return false;
        }
        if ($('#ObjEventRateModel_SubMemberFees').val() === '') {
            jAlert(kcs_Message.InputRequired('Sub Member Fees'), 'ObjEventRateModel_SubMemberFees');
            return false;
        }
        if ($('#ObjEventRateModel_NoOfSeats').val() === '') {
            jAlert(kcs_Message.InputRequired('No.Of.Seats'), 'ObjEventRateModel_NoOfSeats');
            return false;
        }
        //// AddBy : Karan Shah on Date : 28-MAR-2017
        //// Validate Member Seat Limit must not be zero or blank
        if ($('#ObjEventRateModel_MemberSeatLimit').val() === '') {
            jAlert(kcs_Message.InputRequired('Member Seat Limit'), 'ObjEventRateModel_MemberSeatLimit');
            return false;
        }
        //// Validate Guest Seat Limit must not be zero or blank
        if ($('#ObjEventRateModel_GuestSeatLimit').val() === '') {
            jAlert(kcs_Message.InputRequired('Guest Seat Limit'), 'ObjEventRateModel_GuestSeatLimit');
            return false;
        }

        //// AddBy : Karan Shah on Date : 28-MAR-2017
        //// Description : No of Seats is equals to sum of Member seat and Guest Seat.
        if ($('#ObjEventRateModel_NoOfSeats').val() !== '' && $('#ObjEventRateModel_MemberSeatLimit').val() !== '' && $('#ObjEventRateModel_GuestSeatLimit').val() !== '') {
            if (parseInt($('#ObjEventRateModel_NoOfSeats').val()) !== (parseInt($('#ObjEventRateModel_MemberSeatLimit').val()) + parseInt($('#ObjEventRateModel_GuestSeatLimit').val()))) {
                jAlert("Total of member seats limit and guest seats limit should equal to no. of seats.");
                return false;
            }
        }
    }
    else {
        if ($('#EventTicketCategoryId').val() === "") {
            jAlert(kcs_Message.SelectRequired('Event Ticket Category'), 'EventTicketCategoryId');
            return false;
        }
        if ($('#ObjEventRateModel_MemberFees').val() === '') {
            jAlert(kcs_Message.InputRequired('Member Fees'), 'ObjEventRateModel_MemberFees');
            return false;
        }
        if ($('#ObjEventRateModel_GuestFees').val() === '') {
            jAlert(kcs_Message.InputRequired('Guest Fees'), 'ObjEventRateModel_GuestFees');
            return false;
        }
        if ($('#ObjEventRateModel_SubMemberFees').val() === '') {
            jAlert(kcs_Message.InputRequired('Sub Member Fees'), 'ObjEventRateModel_SubMemberFees');
            return false;
        }
        //if ($('#ObjEventRateModel_NoOfSeats').val() === '') {
        //    jAlert(kcs_Message.InputRequired('No.Of.Seats'), 'ObjEventRateModel_NoOfSeats');
        //    return false;
        //}
        ////// AddBy : Karan Shah on Date : 28-MAR-2017
        ////// Validate Member Seat Limit must not be zero or blank
        //if ($('#ObjEventRateModel_MemberSeatLimit').val() === '') {
        //    jAlert(kcs_Message.InputRequired('Member Seat Limit'), 'ObjEventRateModel_MemberSeatLimit');
        //    return false;
        //}
        ////// Validate Guest Seat Limit must not be zero or blank
        //if ($('#ObjEventRateModel_GuestSeatLimit').val() === '') {
        //    jAlert(kcs_Message.InputRequired('Guest Seat Limit'), 'ObjEventRateModel_GuestSeatLimit');
        //    return false;
        //}

        ////// AddBy : Karan Shah on Date : 28-MAR-2017
        ////// Description : No of Seats is equals to sum of Member seat and Guest Seat.
        //if ($('#ObjEventRateModel_NoOfSeats').val() !== '' && $('#ObjEventRateModel_MemberSeatLimit').val() !== '' && $('#ObjEventRateModel_GuestSeatLimit').val() !== '') {
        //    if (parseInt($('#ObjEventRateModel_NoOfSeats').val()) !== (parseInt($('#ObjEventRateModel_MemberSeatLimit').val()) + parseInt($('#ObjEventRateModel_GuestSeatLimit').val()))) {
        //        jAlert("Total of member seats limit and guest seats limit should equal to no. of seats.");
        //        return false;
        //    }
        //}
    }
    return true;
}

function LoadEventScheduleGrid() {
    jQuery('#tblEventSchedule').jqGrid({
        url: '/EventBooking/BindEventScheduleGrid/',
        datatype: 'json',
        postData: { eventId: $('#hfdId').val() },
        mtype: 'GET',
        colNames: [
            'Id', 'Event Date', 'Edit', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'EventDate', index: 'EventDate', width: 100, align: 'left', formatter: 'date', formatoptions: { srcformat: 'ts', newformat: 'd/m/Y' } },

            { name: 'editoperation', index: 'editoperation', align: 'center', width: 30, sortable: false, formatter: EditFormatEventSchedule, hidden: true },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 30, sortable: false, formatter: DeleteFormatEventSchedule }
        ],
        pager: jQuery('#dvEventScheduleFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'EventBookingName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Configuration',
        height: '100%',
        width: '100%',
        multiselect: false,
        ondblClickRow: function (rowid) {
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblEventSchedule').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblEventSchedule').prev()[0].innerHTML = '';
            }

        },

        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
                i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
                cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

    SetScheduleStyle();
}

function LoadEventRateGrid() {
   jQuery('#tblEventRate').jqGrid({
        url: '/EventBooking/BindEventRateGrid/',
        datatype: 'json',
        mtype: 'GET',
        postData: { eventId: $('#hfdId').val() },
        colNames: 
            (clientName != 'UmedClub' ?
                ['Id', 'Event Ticket Category Id', 'Ticket Category', 'Member Fees', 'Sub Member Fees', 'Guest Fees', 'No.Of.Seats', 'Member Seat Limit', 'Guest Seat Limit', 'Edit', 'Delete'] :
                ['Id', 'Event Ticket Category Id', 'Ticket Category', 'Member Fees', 'Sub Member Fees', 'Guest Fees', 'Affiliate Fees', 'Parent Fees', 'Guest Child Fees', 'Guest Room Fees', 'From SerialNo', 'To SerialNo', 'No.Of.Seats','Edit', 'Delete']),
        colModel: 
            (clientName != 'UmedClub' ?
                [{ name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
                { name: 'EventTicketCategoryId', index: 'EventTicketCategoryId', align: 'left', key: true, hidden: true },
                { name: 'EventTicketCategoryName', index: 'EventTicketCategoryName', align: 'left' },
                { name: 'MemberFees', index: 'MemberFees', width: 65, align: 'left' },
                { name: 'SubMember', index: 'SubMember', width: 65, align: 'left' },
                { name: 'GuestFees', index: 'GuestFees', width: 65, align: 'left' },
                { name: 'NoOfSeats', index: 'NoOfSeats', width: 65, align: 'left' },
                { name: 'MemberSeatLimit', index: 'MemberSeatLimit', width: 65, align: 'left' },
                { name: 'GuestSeatLimit', index: 'GuestSeatLimit', width: 65, align: 'left' },
                { name: 'editoperation', index: 'editoperation', align: 'center', width: 30, sortable: false, formatter: EditFormatEventRate },
                { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 30, sortable: false, formatter: DeleteFormatEventRate }]
                : [{ name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
                { name: 'EventTicketCategoryId', index: 'EventTicketCategoryId', align: 'left', key: true, hidden: true },
                { name: 'EventTicketCategoryName', index: 'EventTicketCategoryName', align: 'left' },
                { name: 'MemberFees', index: 'MemberFees', width: 65, align: 'left' },
                { name: 'SubMember', index: 'SubMember', width: 65, align: 'left' },
                { name: 'GuestFees', index: 'GuestFees', width: 65, align: 'left' },
                { name: 'AffiliateMemberFee', index: 'AffiliateMemberFee', width: 65, align: 'left' },
                { name: 'ParentFee', index: 'ParentFee', width: 65, align: 'left' },
                { name: 'GuestChildFee', index: 'GuestChildFee', width: 65, align: 'left' },
                { name: 'GuestRoomFee', index: 'GuestRoomFee', width: 65, align: 'left' },
                { name: 'FromSerialNo', index: 'FromSerialNo', width: 65, align: 'left' },
                { name: 'ToSerialNo', index: 'ToSerialNo', width: 65, align: 'left' },
                { name: 'NoOfSeats', index: 'NoOfSeats', width: 65, align: 'left' },
                { name: 'editoperation', index: 'editoperation', align: 'center', width: 30, sortable: false, formatter: EditFormatEventRate },
                { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 30, sortable: false, formatter: DeleteFormatEventRate }]),
        pager: jQuery('#dvEventRateFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'EventBookingName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Configuration',
        height: '100%',
        width: '100%',
        multiselect: false,
        ondblClickRow: function (rowid) {
        },
        loadComplete: function (data) {
            debugger;
            if (data.records == 0) {
                $('#tblEventRate').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblEventRate').prev()[0].innerHTML = '';
            }
        },
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
                i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
                cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });
    SetRateStyle();
}

function EditFormatEventSchedule(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' id=" + options.rowId + " onclick='FillEventSchedule(\"" + options.rowId + "\")'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></label></a>";
}

function FillEventSchedule(Object) {
    var options = $("#tblEventSchedule").getRowData(Object);
    $('#hfdScheduleId').val(Object);
    $("#ObjEventScheduleModel_EventFromDate").val(options.EventDate);
    $("#ObjEventScheduleModel_EventFromDate").attr("disabled", true);
}


function EditFormatEventRate(cellvalue, options, rowObject) {
    var Ischecked = sessionStorage.getItem('Ischecked');
    if (Ischecked === "true") {
        return "<label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></label></a>";
    }
    else {
        return "<a href='javascript:void(0);'  onclick='FillEventRate(\"" + options.rowId + "\")'><label class='IconEdit' title='Edit' alt='' /><svg width='24' height='24' viewBox='0 0 24 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M23.1066 4.94119L18.9965 0.816307C18.4601 0.310626 17.7572 0.0204732 17.0215 0.0010429C16.2858 -0.0183874 15.5686 0.234261 15.0065 0.710927L1.50632 14.2598C1.02146 14.7505 0.719569 15.3937 0.651311 16.0814L0.0063045 22.3591C-0.0139023 22.5795 0.0146061 22.8018 0.0897972 23.01C0.164988 23.2182 0.285011 23.4071 0.441309 23.5634C0.581471 23.7029 0.747697 23.8133 0.930455 23.8882C1.11321 23.9631 1.30891 24.0011 1.50632 24H1.64132L7.89639 23.4279C8.58159 23.3594 9.22246 23.0564 9.71141 22.5698L23.2116 9.02092C23.7355 8.46536 24.0187 7.724 23.999 6.95928C23.9794 6.19456 23.6584 5.46886 23.1066 4.94119ZM7.62639 20.417L3.12634 20.8386L3.53134 16.3223L12.0064 7.92195L16.0565 11.9866L7.62639 20.417ZM18.0065 9.96934L13.9865 5.93478L16.9115 2.92391L21.0065 7.03374L18.0065 9.96934Z' fill='black' /></svg></label></a>";
    }
}

function FillEventRate(rowId) {
    if (clientName != 'Umed') {
        var options = $("#tblEventRate").getRowData(rowId);
        $('#hfdRateId').val(rowId),
            $("#EventTicketCategoryId").val(options.EventTicketCategoryId);
        $("#hfdEventTicketCategoryId").val(options.EventTicketCategoryId);
        $("#ObjEventRateModel_MemberFees").val(options.MemberFees);
        $("#ObjEventRateModel_GuestFees").val(options.GuestFees);
        $("#ObjEventRateModel_SubMemberFees").val(options.SubMember);

        $("#ObjEventRateModel_AffiliateMemberFee").val(options.AffiliateMemberFee);
        $("#ObjEventRateModel_ParentFee").val(options.ParentFee);
        $("#ObjEventRateModel_GuestChildFee").val(options.GuestChildFee);
        $("#ObjEventRateModel_GuestRoomFee").val(options.GuestRoomFee);
        $("#ObjEventRateModel_FromSerialNo").val(options.FromSerialNo);
        $("#ObjEventRateModel_ToSerialNo").val(options.ToSerialNo);




        $("#ObjEventRateModel_NoOfSeats").val(options.NoOfSeats);
        $("#ObjEventRateModel_MemberSeatLimit").val(options.MemberSeatLimit);
        $("#ObjEventRateModel_GuestSeatLimit").val(options.GuestSeatLimit);
        $("#EventTicketCategoryId").attr("disabled", true);
    }
    else {
        var options = $("#tblEventRate").getRowData(rowId);
        $('#hfdRateId').val(rowId),
            $("#EventTicketCategoryId").val(options.EventTicketCategoryId);
        $("#hfdEventTicketCategoryId").val(options.EventTicketCategoryId);
        $("#ObjEventRateModel_MemberFees").val(options.MemberFees);
        $("#ObjEventRateModel_GuestFees").val(options.GuestFees);
        $("#ObjEventRateModel_SubMemberFees").val(options.SubMember);
        $("#ObjEventRateModel_AffiliateMemberFee").val(options.AffiliateMemberFee);
        $("#ObjEventRateModel_ParentFee").val(options.ParentFee);
        $("#ObjEventRateModel_GuestChildFee").val(options.GuestChildFee);
        $("#ObjEventRateModel_GuestRoomFee").val(options.GuestRoomFee);
        $("#ObjEventRateModel_FromSerialNo").val(options.FromSerialNo);
        $("#ObjEventRateModel_ToSerialNo").val(options.ToSerialNo);
        $("#ObjEventRateModel_NoOfSeats").val(options.NoOfSeats);
        $("#EventTicketCategoryId").attr("disabled", true);
    }
}

function DeleteFormatEventSchedule(cellvalue, options, rowObject) {
    var Ischecked = sessionStorage.getItem('Ischecked');
    if (Ischecked === "true") {
        return "<label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";
    } else {
        return "<a href='javascript:void(0);' onclick='DeleteItemEventSchedule(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";
    }
}

function DeleteFormatEventRate(cellvalue, options, rowObject) {
    var Ischecked = sessionStorage.getItem('Ischecked');
    if (Ischecked === "true") {
        return "<label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";

    } else {
        return "<a href='javascript:void(0);' onclick='DeleteItemEventRate(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /><svg width='22' height='24' viewBox='0 0 22 24' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M20.4 4.8H15.6V3.6C15.6 2.64522 15.2207 1.72955 14.5456 1.05442C13.8705 0.379285 12.9548 0 12 0H9.6C8.64522 0 7.72955 0.379285 7.05441 1.05442C6.37928 1.72955 6 2.64522 6 3.6V4.8H1.2C0.88174 4.8 0.576515 4.92643 0.351472 5.15147C0.126428 5.37652 0 5.68174 0 6C0 6.31826 0.126428 6.62348 0.351472 6.84853C0.576515 7.07357 0.88174 7.2 1.2 7.2H2.4V20.4C2.4 21.3548 2.77928 22.2705 3.45442 22.9456C4.12955 23.6207 5.04522 24 6 24H15.6C16.5548 24 17.4705 23.6207 18.1456 22.9456C18.8207 22.2705 19.2 21.3548 19.2 20.4V7.2H20.4C20.7183 7.2 21.0235 7.07357 21.2485 6.84853C21.4736 6.62348 21.6 6.31826 21.6 6C21.6 5.68174 21.4736 5.37652 21.2485 5.15147C21.0235 4.92643 20.7183 4.8 20.4 4.8ZM8.4 3.6C8.4 3.28174 8.52643 2.97652 8.75147 2.75147C8.97651 2.52643 9.28174 2.4 9.6 2.4H12C12.3183 2.4 12.6235 2.52643 12.8485 2.75147C13.0736 2.97652 13.2 3.28174 13.2 3.6V4.8H8.4V3.6ZM16.8 20.4C16.8 20.7183 16.6736 21.0235 16.4485 21.2485C16.2235 21.4736 15.9183 21.6 15.6 21.6H6C5.68174 21.6 5.37652 21.4736 5.15147 21.2485C4.92643 21.0235 4.8 20.7183 4.8 20.4V7.2H16.8V20.4Z' fill='black'/><rect x='7' y='11' width='8' height='2' fill='black'/><rect x='7' y='15' width='8' height='2' fill='black'/></svg></a>";

    }
}


function DeleteItemEventSchedule(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblEventSchedule').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Event Schedule'));
            return false;
        }
        for (var i = 0; i < selRowIds.length; i++) {
            if (i == 0) {
                objId = selRowIds[i];
            }
            else {
                objId += ',' + selRowIds[i];
            }
        }
    }

    jConfirm(kcs_Message.DeleteConfirm('Event Schedule'), function (r) {
        if (r) {
            kcs_Message.startWait();
            jQuery.post("/EventBooking/DeleteEventSchedule/", { strEventscheduleId: objId },
                function (data) {
                    if (data.toString() != "") {
                        kcs_Message.stopWait();
                        jAlert(data);
                        $('#tblEventSchedule').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
        }
    });
    return false;
}

function DeleteItemEventRate(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblEventRate').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Event Rate'));
            return false;
        }
        for (var i = 0; i < selRowIds.length; i++) {
            if (i == 0) {
                objId = selRowIds[i];
            }
            else {
                objId += ',' + selRowIds[i];
            }
        }
    }

    jConfirm(kcs_Message.DeleteConfirm('Event Rate'), function (r) {
        if (r) {
            jQuery.post("/EventBooking/DeleteEventRate/", { strEventRateId: objId },
                function (data) {
                    if (data.toString() != "") {
                        jAlert(data);
                        $('#tblEventRate').trigger('reloadGrid', [{ page: 1, current: true }]);
                    }
                });
        }
    });
    return false;
}

function SetScheduleStyle() {
    $('#tblEventSchedule').setGridWidth($('#dvEventSchedule').width());
}

function SetRateStyle() {
    $('#tblEventRate').setGridWidth($('#dvEventRate').width());
}

function ShowHideMaximumTicketForFamilyMember(value) {
    if (value == false) {
        $('#divMaximumTicketForFamilyMember').show();
    } else {
        $('#divMaximumTicketForFamilyMember').hide();
    }
}

function ShowHideMemberOutstanding(value) {
    if (value == false) {
        $('#divMemberOutstanding').show();
    } else {
        $('#divMemberOutstanding').hide();
    }
}