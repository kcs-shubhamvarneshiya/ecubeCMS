$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'EventTicketCategoryView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });
    $("#EventTicketCategoryMenu").addClass("MenuActive");
    $(".MenuActive").parent().parent().css("display", "block");
    DisplayMessage('EventTicketCategoryView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#EventTicketCategoryName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Event Ticket Category Name'), 'EventTicketCategoryName');
            return false;
        }     
    });
});