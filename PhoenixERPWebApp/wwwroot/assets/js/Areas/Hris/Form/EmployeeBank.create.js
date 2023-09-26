 
var webUrl = WEB + "/Hris/EmployeeBank";
var ajaxUrl = API + "/Hris/EmployeeBank";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "EmployeeBankFormCreate";
 

$(function () {
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                redirecttolink(webUrl)
            } else {
                DangerNotif(response.message);
            }
        });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);
});
