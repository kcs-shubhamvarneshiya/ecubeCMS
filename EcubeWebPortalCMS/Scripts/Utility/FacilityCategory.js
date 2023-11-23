$(document).ready(function () {
    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id !== 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.ajaxSetup({ async: true });
        }
    };
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'FacilityCategoryView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#FacilityCategory").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('FacilityCategoryView');

    $(window).bind('resize', function () {
        SetStyle();
    });

    $('#btnSubmit').click(function () {
        if ($('#CategoryName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Facility Category Name'), 'Name');
            return false;
        }

        if ($('#SequenceNo').val() === "0" || $('#SequenceNo').val().trim() === '') {
            jAlert(kcs_Message.InputRequired('Valid Sequence No'), 'Name');
            return false;
        }
    });

    $('#ImagePath').on('change', function () {      
        var fileUpload = document.getElementById("ImagePath");
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
                            jQuery('#ImagePath').val('');
                            jAlert("Allowed Size: 1440 X 750 in category image");
                            return false;
                        }
                        var postdata = $('#frmFacilityCategory').serialize();
                        var file = document.getElementById('ImagePath').files[0];
                        var fd = new FormData();
                        fd.append("files", file);
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "/FacilityCategory/UploadCategoryImage", false);
                        xhr.send(fd);
                        $("#aImageName").attr('src', image.src);

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
    });
});