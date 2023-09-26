 
var webUrl = WEB + "/General/CentralResource";
var ajaxUrl = API + "/General/CentralResource";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "CentralResourceFormCreate";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/list";

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
    $('#business_unit_id').setcombobox({
        data: { 'rows': 100, 'search_business_unit_type_id': 'departement' },
        url: ajaxUrlListOrganization,
        searchparam: 'search_unit_name',
        labelField: 'unit_name',
        valueField: 'id',
    });
});
