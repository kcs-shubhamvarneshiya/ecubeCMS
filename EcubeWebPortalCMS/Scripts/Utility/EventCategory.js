$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'EventCategoryView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    $("#EventCategoryMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");

    DisplayMessage('EventCategoryView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#EventCategoryName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Event Category Name'), 'EventCategoryName');
            return false;
        }
    });
});

