
var webUrl = WEB + "/Hris/PTKPSetting";
var ajaxUrl = API + "/Hris/PTKPSetting";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "PTKPSettingFormCreate";
var ajaxUrlListSalaryComponent = API_HRIS + "/PTKPSetting/GetWithDetil";

$(function () {
    /*$('#salary_component_id').setcombobox({
        data: { 'rows': 100, 'salary_component': 'salary_component' },
        url: ajaxUrlListSalaryComponent,
        //url: ajaxUrlList,
        searchparam: 'id',
        labelField: 'salary_component',
        valueField: 'id',
        searchField: 'salary_component'
    });*/

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
