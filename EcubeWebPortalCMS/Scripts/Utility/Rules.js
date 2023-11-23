var blAdd;
$(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = '/Home/Index';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('Rules');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        
        if ($('#PageContent1').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('PageContent'), 'PageContent');
            return false;
        }
    });
});