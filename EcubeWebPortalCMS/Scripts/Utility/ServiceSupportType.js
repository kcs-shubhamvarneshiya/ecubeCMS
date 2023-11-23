$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'ServiceSupportTypeView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });
   
    DisplayMessage('ServiceSupportTypeView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#TypeName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Name'), 'TypeName');
            return false;
        }
        if ($('#Decsription').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Decsription'), 'Decsription');
            return false;
        }
     });

});
