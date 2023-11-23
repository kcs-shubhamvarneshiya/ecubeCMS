$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#HdnIFrame').val().toLowerCase() == 'false') {
            window.location.href = 'DocumentLibraryView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('DocumentLibraryView');

    shortcut.add("Ctrl+S", function () {
        $("#btnSubmit").click();
    });

    shortcut.add("esc", function () {
        $("#btnCancel").click();
    });

    $('#btnSubmit').click(function () {
        if ($('#Title').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Document Title'), 'Title');
            return false;
        }
        if ($('#hdnFileUploadDocument').val() == '') {
            if ($('#FileUploadDocument').val().trim() == '') {
                jAlert(kcs_Message.SelectRequired('Document'), 'FileUploadDocument');
                return false;
            }
        }
    });

    $('#FileUploadDocument').on('change', function () {
        var fileUpload = document.getElementById("FileUploadDocument");
        var allowedFiles = [".exe"];
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.exe)$");
         if (!regex.test(fileUpload.value.toLowerCase())) {
                var postdata = $('#login-form').serialize();
                var file = document.getElementById('FileUploadDocument').files[0];
                var fd = new FormData();
                fd.append("files", file);
                var xhr = new XMLHttpRequest();
                xhr.open("POST", "/DocumentLibrary/UploadDocument", false);
                xhr.send(fd);
        }
        else {
            $('#FileUploadDocument').val('');
            jAlert("Please do not upload files having extension: <b>" + allowedFiles.join(', ') + "</b>.");
            return false;
        }
    });
});