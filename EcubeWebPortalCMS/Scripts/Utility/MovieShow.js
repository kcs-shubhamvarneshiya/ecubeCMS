$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'MovieShowView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });
    $("#MovieShowMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('MovieShowView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#strShowStartDate').val().trim() == '') {
            jAlert(kcs_Message.SelectRequired('Show Start Date'), 'strShowStartDate');
            return false;
        }
        if ($('#strShowEndDate').val().trim() == '') {
            jAlert(kcs_Message.SelectRequired('Show End Date'), 'strShowEndDate');
            return false;
        }
    });
    
});

