$(document).ready(function () {

    $('#frmForgotPassword').hide();
    $('#btnLoginScreen').hide();

    form_wrapper = $('.login_box');

    $('#btnForgotPassword').on('click', function (e) {
        var target = $(this).attr('href');
        $(form_wrapper.find('form:visible')).fadeOut(400, function () {
            form_wrapper.stop().animate(500, function () {
                $('#btnLoginScreen').show();
                $(target).fadeIn(400);
                jQuery('#UserName').val('');
                jQuery('#Password').val('');
                jQuery('#EmailID').focus();
            });

        });
        $('#btnForgotPassword').fadeOut(400);
        e.preventDefault();
    });

    $('#btnLoginScreen').on('click', function (e) {
        var target = $(this).attr('href');
        $(form_wrapper.find('form:visible')).fadeOut(400, function () {
            form_wrapper.stop().animate(500, function () {
                $('#btnLoginScreen').hide();
                $('#btnForgotPassword').show();
                $(target).fadeIn(400);
                jQuery('#strEmailID').val('');
                jQuery('#strUserName').focus();
            });
        });
        $('#btnLoginScreen').fadeOut(400);
        e.preventDefault();
    });

    jQuery('#btnSignIn').click(function () {
        if (jQuery('#UserName').val().trim() == '' && jQuery('#UserName').val().trim() == 'undefined') {
            jAlert('Please Enter Username', 'strUserName');
            return false;
        }
        if (jQuery('#Password').val().trim() == '' && jQuery('#Password').val().trim() == 'undefined') {
            jAlert('Please Enter Password', 'strPassword');
            return false;
        }
        jQuery('#errorInfo').hide();
        $.ajax({
            url: '/Home/ValidateLogin',
            type: 'POST',
            data: $("#LoginForm").serialize(),
            success: function (data) {
                if (data == '2222') {
                    jQuery('#UserName').val('');
                    jQuery('#Password').val('');
                    jQuery('#errorInfo').html('Invalid Username Or Password');
                    jQuery('#errorInfo').show();
                }
                else if (data == '3333') {
                    jQuery('#UserName').val('');
                    jQuery('#Password').val('');
                    jQuery('#errorInfo').html('User Is Already Logged In From Somewhere Else.');
                    jQuery('#errorInfo').show();
                }
                else if (data == '1111') {
                    jQuery('#UserName').val('');
                    jQuery('#Password').val('');
                    jQuery('#errorInfo').html('We Are Getting Some Problem. Please Try After Some Time Or Contact Your Administrator.');
                    jQuery('#errorInfo').show();
                }
                else {
                    ProgressBar();
                    $("#LoginForm").submit();
                }
                jQuery('#UserName').focus();
            }
        });
        return false;
    });

    jQuery('#btnSendPassword').click(function () {
        if (jQuery('#EmailID').val().trim() == '') {
            jAlert('Please Enter Email Address', 'strEmailID');
            return false;
        }
        ProgressBar();
        $.ajax({
            url: '/Home/ValidateLogin',
            type: 'POST',
            data: $("#frmForgotPassword").serialize(),
            success: function (data) {
                CloseProgressBar();
                if (data == '1111') {
                    jAlert("Email Address Is Not Registered.", 'strEmailID');
                }
                else if (data == '2222') {
                    jAlert("Password Is Sent To Registered Email Address.", 'strEmailID', null, function () {
                        $('#btnLoginScreen').click();
                    });
                }
                else {
                    jAlert(data, 'EmailID');
                }
            }
        });
        return false;
    });

    jQuery('input:checkbox, input:radio').uniform();
    jQuery('input:text[title], input:password[title], textarea[title]').tipsy({ trigger: 'focus', gravity: 'w' });
    jQuery('#errorInfo').hide();
    jQuery('#UserName').focus();

    jQuery('.emailid').on('blur', function (event) {
        if (jQuery(this).val().trim() != "") {
            var filter = /^([\w-\.]+)@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (!filter.test(jQuery(this).val().trim())) {
                jAlert('Invalid Email Address..', jQuery(this).attr("id").toString());
                return false;
            }
        }
        return true;
    });
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