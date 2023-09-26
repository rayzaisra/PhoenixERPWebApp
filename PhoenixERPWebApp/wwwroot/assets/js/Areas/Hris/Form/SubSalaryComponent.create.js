
var webUrl = WEB + "/Hris/SubSalaryComponent";
var ajaxUrl = API + "/Hris/SubSalaryComponent";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "SubSalaryComponentFormCreate";
var ajaxUrlListSalaryComponent = API_HRIS + "/SalaryComponent/GetWithDetil";

$(function () {
    $('#salary_component_id').setcombobox({
        data: { 'rows': 100, 'salary_component': 'salary_component' },
        url: ajaxUrlListSalaryComponent,
        //url: ajaxUrlList,
        searchparam: 'id',
        labelField: 'salary_component',
        valueField: 'id',
        searchField: 'salary_component'
    });

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
