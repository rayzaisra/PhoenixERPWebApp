var webUrl = WEB + "/General/CentralResource";
var ajaxUrl = API + "/General/CentralResource";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "CentralResourceFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get"; 
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/list";

$(function () {
    var xid = $("#" + xform +' [name="id"]').val();
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
            initlanguage(lang_gn);
            FormLoadByDataUsingName(response.data, xform);

            $("#business_unit_id").append('<option value="' + response.data.business_unit_id + '" selected>' + response.data.business_unit_name +'</option>')
            $('#business_unit_id').setcombobox({
                data: { 'rows': 100, 'search_business_unit_type_id': 'departement' },
                url: ajaxUrlListOrganization,
                searchparam: 'search_unit_name',
                labelField: 'unit_name',
                valueField: 'id',
            });

        });
    }
    


    
    
});
  
