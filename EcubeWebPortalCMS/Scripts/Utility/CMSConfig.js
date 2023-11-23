$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'CMSConfigView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#Configuration").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    DisplayMessage('CMSConfigView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#PersonName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Person Name'), 'PersonName');
            return false;
        }
        if ($('#EmailID').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Email Id'), 'EmailID');
            return false;
        }
        if (!ValidateEmail($("#EmailID").val())) {
            jAlert("Invalid EmailID.", 'EmailID');
            return false;
        }
        if ($('#MobileNo').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Mobile No'), 'MobileNo');
            return false;
        }
    });
});

function ValidateEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);
};
