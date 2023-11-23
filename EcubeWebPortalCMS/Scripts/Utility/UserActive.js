jQuery(document).ready(function () {
    UserActive();
    //$('<iframe id="Detect" style="display:none" name="Detect" src="/Common/DetectBrowserClosePage/">').appendTo("body").ready(function () {
    //    setTimeout(function () {
    //        $('#Detect').contents().find('body').append(myContent);
    //    }, 5000);
    //});
});
function UserActive() {
    jQuery.ajaxSetup({ async: false });
    var result = "";
    jQuery.post("/Common/CheckUserActive/", {}, function (data) {
        result = data.toString();
    }, undefined, false);
    jQuery.ajaxSetup({ async: true });
    if (result != "") {
        window.location = result;
    }
    return false;
}