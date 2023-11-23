
$(document).ready(function () {

    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id !== 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.ajaxSetup({ async: true });
        }
    };
    $('.cancel').click(function () {
            window.location.href = 'InquiryCategoryView';
    });


    $("#InquiryCategoryMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('InquiryCategoryView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#Category').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Inquiry Category Name'), 'Category');
            return false;
        }

        if ($('#Id').val() == '0') {
            if (($('#CategoryImage').val().trim() == '') ) {
                jAlert('Please select a image for the Category Image.');
                return false;
            }
        }

        var mobileNumberValue = $('#MobileNumber').val().trim();
        if (mobileNumberValue.length !== 10 || !/^[0-9]+$/.test(mobileNumberValue)) {
            jAlert('Mobile Number must be exactly 10 numeric characters.', 'MobileNumber');
            return false;
        }

        var seqNoValue = $('#SeqNo').val().trim();
        if (seqNoValue == '' || seqNoValue == '0') {
            jAlert('Please enter a valid Sequence No.', 'SeqNo');
            return false;
        }

        var emailValue = $('#Email').val().trim();
        if (emailValue === '') {
            jAlert('Email is required.', 'Email');
            return false;
        }

        if (!isValidEmail(emailValue)) {
            jAlert('Please enter a valid email address.', 'Email');
            return false;
        }

        // Function to check if the input is a valid email address
        function isValidEmail(email) {
            // Regular expression for basic email validation
            var emailPattern = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
            return emailPattern.test(email);
        }

    });

    $('#CategoryImage').on('change', function () {
        validateImage($(this).attr('id'));
    });
});

function validateImage(controlId) {
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
                    if (height !== 98 || width !== 130) {
                        jQuery('#' + controlId).val('');
                        $('#' + controlId).val('');
                        jAlert("Allowed Size: 130 X 98 in Inquiry Category image.");
                        return false;
                    }
                    var postdata = $('#frmInquiryCategory').serialize();
                    var file = document.getElementById(controlId).files[0];
                    var fd = new FormData();
                    fd.append("files", file);
                    fd.append("sequence", controlId.replace("Image", ""));
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", "/InquiryCategory/UploadImage", false);
                    xhr.send(fd);
                    $("#a" + controlId).attr('src', image.src);
                };
            }
        }
        else {
            $('#ImagePath').val('');
            $('#' + controlId).val('');
            jAlert("Valid formats include: .jpg, .gif, .png, with filenames that do not contain special characters");
            return false;
        }
    }
    else {
        $('#ImagePath').val('');
        $('#' + controlId).val('');
        jAlert("Valid formats include: .jpg, .gif, .png, with filenames that do not contain special characters");
        return false;
    }
}