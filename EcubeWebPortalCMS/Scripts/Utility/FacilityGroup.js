$(document).ready(function () {
    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id !== 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.ajaxSetup({ async: true });
        }
    };
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'FacilityGroupView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#Facility").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
   
    DisplayMessage('FacilityGroupView');

    $(window).bind('resize', function () {
        SetStyle();
    });

    $('#btnSubmit').click(function () {
        if ($('#Name').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Facility Name'), 'Name');
            return false;
        }

        if ($('#ddlCategoryList').val() === "0") {
            jAlert(kcs_Message.SelectRequired('Category Name'), 'Name');
            return false;
        }

        if ($('#SequenceNo').val() === "0" || $('#SequenceNo').val().trim() === '') {
            jAlert(kcs_Message.InputRequired('Valid Sequence No'), 'Name');
            return false;
        }       
    });

    $('#BannerImage').on('change', function () {
        var fileUpload = document.getElementById("BannerImage");
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
                        var postdata = $('#frmFacilityGroup').serialize();
                        var file = document.getElementById('BannerImage').files[0];
                        var fd = new FormData();
                        fd.append("files", file);
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "/FacilityCategory/UploadGroupBannerImage", false);
                        xhr.send(fd);
                        $("#aBannerImageName").attr('src', image.src);

                    };
                }
            }
            else {
                $('#BannerImage').val('');
                jAlert("Allowed Format:.jpg, .gif, .png");
                return false;
            }
        }
        else {
            $('#BannerImage').val('');
            jAlert("Allowed Format:.jpg, .gif, .png");
            return false;
        }
    });

    $('#IconImage').on('change', function () {
        var fileUpload = document.getElementById("IconImage");
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
                        if (height < 108 || width < 108) {
                            jQuery('#IconImage').val('');
                            jAlert("Allowed Size: 108 X 108 in icon image");
                            return false;
                        }
                        var postdata = $('#frmFacilityGroup').serialize();
                        var file = document.getElementById('IconImage').files[0];
                        var fd = new FormData();
                        fd.append("files", file);
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "/FacilityCategory/UploadGroupIconImage", false);
                        xhr.send(fd);
                        $("#aIconImageName").attr('src', image.src);

                    };
                }
            }
            else {
                $('#IconImage').val('');
                jAlert("Allowed Format:.jpg, .gif, .png");
                return false;
            }
        }
        else {
            $('#IconImage').val('');
            jAlert("Allowed Format:.jpg, .gif, .png");
            return false;
        }
    });
});