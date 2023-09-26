 
var webUrl = WEB + "/ProjectManagement/brand";
var ajaxUrl = API + "/ProjectManagement/CompanyBrand";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlCreateBrand = ajaxUrl + "/CreateBrand";
var xform = "BrandFormCreate";
var ajaxUrlListCompany = API + "/ProjectManagement/CompanyBrand/companylist";
var ajaxUrlListAccount = API + "/ProjectManagement/CompanyBrand/accountlist";
var ajaxUrlListLegal = API + "/ProjectManagement/CompanyBrand/legallist";
var ajaxUrlListAffilate = API + "/ProjectManagement/CompanyBrand/AffiliateList";
var ajaxUrlListDivision = API + "/ProjectManagement/CompanyBrand/divisionlist";
var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
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
    //setFormAction(xform, ajaxUrlCreate);
    setFormAction(xform, ajaxUrlCreateBrand);
    initlanguage(lang_gn);
    $('#company_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListCompany,
        searchparam: 'search',
        labelField: 'company_name',
        valueField: 'id',
    });
    $('#legal_enitity_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListLegal,
        searchparam: 'search',
        labelField: 'name',
        valueField: 'id',
    });
    $('#account_name').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListAccount,
        searchparam: 'search',
        labelField: 'name',
        valueField: 'name',
    });
 
    $('#division_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListDivision,
        searchparam: 'search',
        labelField: 'name',
        valueField: 'id',
    });
    
    $('#sub_brandtag').tagbox({
        onChange: function () {
            var xtag = $('#sub_brandtag').val();

            $('#sub_brand').val(xtag);
        }
    });
    $('#brand_account').setcombobox({
        data: { 'rows': 100 },
        url: ajax,
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
