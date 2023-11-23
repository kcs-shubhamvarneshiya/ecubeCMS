$(document).ready(function () {
    $("#strToDate").prop('disabled', true);
    $(".clsstrFromDate").datepicker({
        dateFormat: "dd/mm/yy",
        minDate: 0, // Restrict to today's date and future dates
        onSelect: function (selectedDate) {

            var selectedDateParts = selectedDate.split("/");
            var day = parseInt(selectedDateParts[0]);
            var month = parseInt(selectedDateParts[1]) - 1;
            var year = parseInt(selectedDateParts[2]);

            var fromDate = new Date(year, month, day);

            var minToDate = new Date(fromDate);
            minToDate.setDate(fromDate.getDate()); // Set minimum "To Date" to the day after "From Date"

            // Update the "To Date" Datepicker options
            $(".clsstrToDate").datepicker("option", "minDate", minToDate);
            $("#strToDate").prop('disabled', false);
        }
    });

    $(".clsstrToDate").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        minDate: 0,
        buttonText: "Select date"
    });

    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id !== 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.ajaxSetup({ async: true });
        }
    };
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'BannerView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#Banner").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('BannerView');

    $('#btnSubmit').click(function () {

        if ($('#BannerTitle').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Banner Title'), 'Banner Title');
            return false;
        }
        if ($('#strFromDate').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('From Date'), 'From Date');
            return false;
        }
        if ($('#strToDate').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('To Date'), 'To Date');
            return false;
        }
        var BannerDesc = CKEDITOR.instances.BannerDescription.getData();
        if (BannerDesc == null || BannerDesc == "") {
            jAlert(kcs_Message.InputRequired('Banner Description'), 'Banner Description');
            return false;
        }
        if ($('#BannerId').val() == '0') {
            if (($('#Image1').val().trim() == '') && ($('#Image2').val().trim() == '') && ($('#Image3').val().trim() == '') && ($('#Image4').val().trim() == '') && ($('#Image5').val().trim() == '')) {
                jAlert('At least one image is required.');
                return false;
            }
        }
        $("#strToDate").prop('disabled', false);
        CKEDITOR.replace('BannerDescription', { enterMode: CKEDITOR.ENTER_BR, shiftEnterMode: CKEDITOR.ENTER_BR });
    });

    $('.image-input').on('change', function () {
        var inputField = $(this);
        validateImage($(this).attr('id'), function (isValid) {
            if (isValid) {
                var imageId = inputField.attr('id');
                var closeButtonHtml = `<button class="text-danger close mr-1 close-btn" type="button" data-image="${imageId}">
                                <h3>
                                    <svg width="22" height="24" viewBox="0 0 22 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <rect width="22" height="24" rx="2" fill="#CC1F1F" />
                                        <path d="M14 9L8 15" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                        <path d="M8 9L14 15" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                    </svg>
                                </h3>
                            </button>`;
                var closeButton = $(closeButtonHtml);
                closeButton.addClass('close-btn');
                closeButton.insertAfter(inputField);

                // Remove new Image: Start
                $('.close-btn').on('click', function () {
                    var imageId = $(this).data('image');
                    var imageInput = $('#' + imageId);
                    removeImage(imageInput.attr('id'));
                    $(this).hide();
                });
                // Remove new Image: End
            }
        });
    });

    // Remove exisring Image: Start
    $('.close-btn').on('click', function () {
        var imageId = $(this).data('image');
        removeImage(imageId);
        $(this).hide();
    });
    // Remove exisring Image: End

});

function removeImage(controlId) {
    jQuery('#' + controlId).val('');
    $('#' + controlId).val('');

    jQuery("input[name='" + controlId + "']").val('');
    $("input[name='" + controlId + "']").val('');

    jQuery('#a' + controlId).val('');
    $('#a' + controlId).attr('src', '/Content/Image/noprofile-img.jpg');

    var postdata = $('#frmBanner').serialize();
    var file = document.getElementById(controlId).files[0];
    var fd = new FormData();
    fd.append("files", file);
    fd.append("sequence", controlId.replace("Image", ""));
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Banner/RemoveImage", false);
    xhr.send(fd);
}

function validateImage(controlId, callback) {

    var fileUpload = document.getElementById(controlId);
    var regex = new RegExp("([a-zA-Z0-9\\s_\\.\-:])+(.jpg|.jpeg|.png|.gif)$");
    jQuery.support.cors = true;
    if (regex.test(fileUpload.files[0].name.toLowerCase())) {

        if (typeof (fileUpload.files) !== "undefined") {
            var reader = new FileReader();
            reader.readAsDataURL(fileUpload.files[0]);
            reader.onload = function (e) {
                var image = new Image();
                image.src = reader.result;
                image.onload = function () {
                    var height = image.height;
                    var width = image.width;
                    if (height != 750 || width != 1400) {
                        jQuery('#' + controlId).val('');
                        $('#' + controlId).val('');
                        jAlert("Allowed Size: 1400 X 750 in banner image.");
                        callback(false);
                    } else {
                        var postdata = $('#frmBanner').serialize();
                        var file = document.getElementById(controlId).files[0];
                        var fd = new FormData();
                        fd.append("files", file);
                        fd.append("sequence", controlId.replace("Image", ""));
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "/Banner/UploadImage", false);
                        xhr.send(fd);
                        $("#a" + controlId).attr('src', image.src);
                        callback(true);
                    }

                };
            }
        }
        else {
            $('#ImagePath').val('');
            $('#' + controlId).val('');
            jAlert("Valid formats include: .jpg, .gif, .png, with filenames that do not contain special characters");
            callback(false);
        }
    }
    else {
        $('#ImagePath').val('');
        $('#' + controlId).val('');
        jAlert("Valid formats include: .jpg, .gif, .png, with filenames that do not contain special characters");
        callback(false);
    }
}