var webUrl = WEB + "/ProjectManagement/brand";
var ajaxUrl = API + "/ProjectManagement/companybrand";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "BrandFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get"; 
var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
var ajaxUrlListCompany = API + "/ProjectManagement/CompanyBrand/companylist";
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

            $("#company_id").append('<option value="' + response.data.company_id + '" selected>' + response.data.company_name +'</option>')
            $('#company_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlListCompany,
                searchparam: 'search',
                labelField: 'company_name',
                valueField: 'id',
            });
            $('#sub_brandtag').val($('#sub_brand').val());
            $('#sub_brandtag').tagbox({
                onChange: function () {
                    var xtag = $('#sub_brandtag').val();
 
                    $('#sub_brand').val(xtag);
                }
            });

            $('#brand_account').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlListAccountManagement,
                maxItems: 0,
                searchparam: 'search_app_username',
                labelField: 'app_fullname',
                valueField: 'user_id',
                render: function (data) {
                    if (data.odata) {
                        var stateNo = mUtil.getRandomInt(0, 7);
                        var states = [
                            'success',
                            'brand',
                            'danger',
                            'accent',
                            'warning',
                            'metal',
                            'primary',
                            'info'];
                        var state = states[stateNo];
                        return '<div class="m-card-user m-card-user--sm">' +
                            '<div class="m-card-user__pic">' +
                            '<div class="m-card-user__no-photo m--bg-fill-' + state + '"><span>' + data.odata.app_fullname.substring(0, 1) + '</span></div>' +
                            '</div>' +
                            '<div class="m-card-user__details">' +
                            '<span class="m-card-user__name">' + data.odata.app_fullname + '</span>' +
                            '<a href="javascript:return;" class="m-card-user__email m-link">' + data.odata.app_username + '</a>' +
                            '</div></div>';
                    }
                }
            });
        });
    }
    


    
    
});
  
