 
var webUrl = WEB + "/General/RoleAccessMenu";
var ajaxUrl = API + "/General/RoleAccessMenu";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "RoleAccessMenuFormCreate";
 

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
    initlanguage(lang_gn);
});
