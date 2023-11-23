$(document).ready(function () {
    window.onbeforeunload = function (e) {
        if (e.explicitOriginalTarget.id !== 'btnSubmit') {
            jQuery.ajaxSetup({ async: false });
            jQuery.ajaxSetup({ async: true });
        }
    };
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'NotificationView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#Notification").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('NotificationView');

    $(window).bind('resize', function () {
        SetStyle();
    });

    $('#btnSubmit').click(function () {
        if ($('#Name').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Notification Name'), 'Name');
            return false;
        }
    });
});