$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'ServiceSupportView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('ServiceSupportView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#StatusId').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Status'), 'Status');
            return false;
        }
        if ($('#Response').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Response'), 'Response');
            return false;
        }
        if ($('#ResponseBy').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Responsive Person'), 'ResponseBy');
            return false;
        }
    });
});