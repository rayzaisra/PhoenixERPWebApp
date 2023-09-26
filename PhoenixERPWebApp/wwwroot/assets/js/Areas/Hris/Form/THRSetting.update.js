
var webUrl = WEB + "/Hris/THRSetting";
var ajaxUrl = API + "/Hris/THRSetting";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "THRSettingFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlListSalaryComponent = API_HRIS + "/SalaryComponent/GetWithDetil";

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

    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            setFormAction(xform, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                var xret = $("#" + xform).valid();
                if (xret) startprocess();
                return xret;
            }, function (response, statusText, xhr, $form) {
                endprocess();
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    redirecttolink(webUrl);
                } else {
                    DangerNotif(response.message);
                }
            });
            InitForm(xform, optionsForm);
            setFormAction(xform, ajaxUrlUpdate);
            initlanguage(lang_hr);
            var record = response.data;

            //alert(record.salary_component_id);
            FormLoadByDataUsingName(response.data, xform);
            //$('#salary_component_id').cmSetLookup(record.salary_component_id, record.salary_component);
        });
    }





});

