$(document).ready(function () {
    //Set Active Menu
    setNavigation();
});
function setNavigation() {
    if (IsContain("Form_Elements") || IsContain("Widgets") || IsContain("FormWizard") || IsContain("MultiDatesPicker")) {
        $("#Menu").addClass("in");
        return;
    }
    else if (IsContain("DocumentUpload") || IsContain("MultipleFileUpload") || IsContain("ckEditor") || IsContain("DataTable") || IsContain("HighChart") || IsContain("FullCalendar") || IsContain("Bill")) {
        $("#Demo").addClass("in");
        return;
    }
    else if (IsContain("UserDisplay") || IsContain("RoleDisplay") || IsContain("ErrorLog") || IsContain("GoogleMap") || IsContain("Zip")) {
        $("#Utility").addClass("in");
        return;
    }
    else if (IsContain("Country") || IsContain("State") || IsContain("City")) {
        $("#Master").addClass("in");
        return;
    }
    else if (IsContain("eLog") || IsContain("ClientInfo") || IsContain("CreateZip") || IsContain("EmailService") || IsContain("Extension") || IsContain("Functions") || IsContain("jQueryDatatableParam") || IsContain("Common") || IsContain("TaskFactory") || IsContain("ServerSideCaching") || IsContain("ReportingFrameWork")) {
        $("#UtilityDll").addClass("in");
        return;
    }
    else if (IsContain("Table") || IsContain("SP") || IsContain("Function")) {
        $("#Sql").addClass("in");
        return;
    }
}
function IsContain(url) {
    var pgurl = window.location.toString();
    if (pgurl.indexOf(url) > -1) { return true; }
    else { return false; }
}