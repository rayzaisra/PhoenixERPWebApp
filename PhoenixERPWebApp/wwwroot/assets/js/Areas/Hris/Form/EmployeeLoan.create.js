
var webUrl = WEB + "/Hris/EmployeeLoan";
var ajaxUrl = API + "/Hris/EmployeeLoan";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "EmployeeLoanFormCreate";
var ajaxUrlListSalaryComponent = API_HRIS + "/LoanCategory/GetWithDetil";

$(function () {
    $('#loan_category_id').setcombobox({
        data: { 'rows': 100, 'name': 'name' },
        url: ajaxUrlListSalaryComponent,
        //url: ajaxUrlList,
        searchparam: 'id',
        labelField: 'name',
        valueField: 'id',
        searchField: 'name'
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
