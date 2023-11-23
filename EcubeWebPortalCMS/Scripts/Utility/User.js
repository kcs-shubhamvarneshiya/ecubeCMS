$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'UserView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('UserView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#FirstName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('First Name'), 'FirstName');
            return false;
        }
        if ($('#SurName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Last Name'), 'SurName');
            return false;
        }
        if ($('#MobileNo').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Mobile No'), 'MobileNo');
            return false;
        }
        if ($('#EmailID').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Email Id'), 'EmailID');
            return false;
        }
        if ($('#UserName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Username'), 'UserName');
            return false;
        }
        if ($('#Password').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Password'), 'Password');
            return false;
        }
      // if ($('#Address').val().trim() == '') {
      //     jAlert(kcs_Message.InputRequired('Address'), 'Address');
      //     return false;
      // }
        if ($('#RoleId').val() == '') {
            jAlert(kcs_Message.SelectRequired('Role'), 'RoleId');
            return false;
        }
    });

});