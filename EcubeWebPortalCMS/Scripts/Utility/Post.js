$(document).ready(function () {
    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id !== 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.ajaxSetup({ async: true });
        }
    };
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'PostView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#Post").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('PostView');

    $(window).bind('resize', function () {
        SetStyle();
    });

    $('#btnSubmit').click(function () {
        if ($('#Name').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Post Name'), 'Name');
            return false;
        }
    });

    $('#Image1').on('change', function () {
        validateImage($(this).attr('id'));
    });
    $('#Image2').on('change', function () {
        validateImage($(this).attr('id'));
    });
    $('#Image3').on('change', function () {
        validateImage($(this).attr('id'));
    });
    $('#Image4').on('change', function () {
        validateImage($(this).attr('id'));
    });
    $('#Image5').on('change', function () {
        validateImage($(this).attr('id'));
    });
});

function validateImage(controlId) {
    var fileUpload = document.getElementById(controlId);
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.png|.gif)$");
    jQuery.support.cors = true;
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
                    if (height < 750 || width < 1400) {
                        jQuery('#' + controlId).val('');
                        jAlert("Allowed Size: 1440 X 750 in post image.");
                        return false;
                    }
                    var postdata = $('#frmPost').serialize();
                    var file = document.getElementById(controlId).files[0];
                    var fd = new FormData();
                    fd.append("files", file);
                    fd.append("sequence", controlId.replace("Image", ""));
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", "/Post/UploadImage", false);
                    xhr.send(fd);
                    $("#a" + controlId).attr('src', image.src);
                };
            }
        }
        else {
            $('#ImagePath').val('');
            jAlert("Allowed Format:.jpg, .gif, .png");
            return false;
        }
    }
    else {
        $('#ImagePath').val('');
        jAlert("Allowed Format:.jpg, .gif, .png");
        return false;
    }
}