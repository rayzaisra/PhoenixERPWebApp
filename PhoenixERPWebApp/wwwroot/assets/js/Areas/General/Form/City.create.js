 
var webUrl = WEB + "/General/City";
var ajaxUrl = API + "/General/City";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "CityFormCreate";
var ajaxUrlCountry = API + "/General/Country";

$(function () {
    $('#country_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlCountry,
        searchparam: 'search_country_name',
        labelField: 'country_name',
        valueField: 'id',
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
    initlanguage(lang_gn);
});
