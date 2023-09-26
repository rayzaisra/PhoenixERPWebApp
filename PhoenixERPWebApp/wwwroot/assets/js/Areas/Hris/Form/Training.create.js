 
var webUrl = WEB + "/Hris/Training";
var ajaxUrl = API + "Hris/Training";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "TrainingFormCreate";
 

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

function ShowBonding() {
  
    var rev = $('#training_cost').val();
    if (parseInt(rev) >= parseInt(15000000)) {
        $("#bonding_detail_div").css({ display: "block" });
    }

}
