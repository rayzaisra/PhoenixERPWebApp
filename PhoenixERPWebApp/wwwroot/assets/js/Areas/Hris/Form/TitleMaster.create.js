 
var webUrl = WEB + "/Hris/TitleMaster";
var ajaxUrl = API + "Hris/JobTitle";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "JobTitleFormCreate";
var ajaxUrlListJobLevel = API_HRIS + "/JobLevel/List";
var ajaxUrlListJobGrade = API_HRIS + "/JobGrade/List";
var ajaxUrlListJobTitle = ajaxUrl + "/List/";
 

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
                redirecttolink(webUrl);
            } else {
                DangerNotif(response.message);
            }
        });
   
 
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);



});



