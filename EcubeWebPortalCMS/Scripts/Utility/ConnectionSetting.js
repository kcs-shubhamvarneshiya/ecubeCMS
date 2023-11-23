$(document).ready(function () {

    if ($('#IsAllowToChangeConfig').val().trim() == 'True') {
        $('input:text[title], input:password[title], textarea[title]').tipsy({ trigger: 'focus', gravity: 'w' });
        $('#ServerName').focus();

        jQuery('#btnSubmit').click(function () {
            if ($('#ServerName').val().trim() == '') {
                alert('Please Enter ServerName');
                return false;
            }
            if ($('#CatalogName').val().trim() == '') {
                alert('Please Enter CatalogName');
                return false;
            }
            ProgressBar();
            $.ajax({
                url: '/ConnectionSetting/ChangeConnection',
                type: 'POST',
                data: $("#frmConnectionSetting").serialize(),
                success: function (data) {
                    if (data == '1111') {
                        alert('Connection strings change successfully.', 'strUserName');
                        
                        window.location.href = '/home/login/';
                    }
                    else {
                        alert('Please Enter ServerName', 'strUserName');
                    }
                }
            });
            return false;
        });
    }
    else {
        alert('Access denied!!!');
        window.location = "/Home/Login";
    }
});

function ProgressBar() {
    jQuery("BODY").append('<div id="p_overlay"></div>');
    jQuery("#p_overlay").css({
        position: 'absolute',
        zIndex: 99998,
        top: '0px',
        left: '0px',
        width: '100%',
        height: jQuery(document).height(),
        background: '#FFF',
        opacity: '.50'
    });
    if (document.getElementById('loading_layer') != null) {
        document.getElementById('loading_layer').style.display = 'block';
    }
}

//Close progress bar
function CloseProgressBar() {
    jQuery("#p_overlay").remove();
    if (document.getElementById('loading_layer') != null) {
        document.getElementById('loading_layer').style.display = 'none';
    }
}