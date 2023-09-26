 
var webUrl = WEB + "/Hris/Termination";
var ajaxUrl = API + "/Hris/Termination";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "TerminationFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get"; 

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
            initlanguage(lang_hr);
            FormLoadByDataUsingName(response.data, xform);
        });
    }
    


    
    
});
  
