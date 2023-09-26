var webUrl = WEB + "/ProjectManagement/brand";
var ajaxUrl = API + "/ProjectManagement/companybrand";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "BrandFormTeam";
var ajaxUrlGet = ajaxUrl + "/Getwithteam";
var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
var ajaxUrlListActor = API_PM + "/AccountManagement/listactor";
var ajaxUrlPost = ajaxUrl + "/UpdateTeam";
var tableList = "CompanyBrandTeamList";
var vselactor = "";
var tempteam = [];
var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "checktd",
        orderable: false,
        "render": function (data, type, row, meta) {
            var checked = "";
            $.each(tempteam, function (key, item) {
                if (item.id == row.id) {
                    checked = "checked";
                }
            });
            retval = '<label class="mt-checkbox"><input ' + checked+' type = "checkbox" id = "check_' +row.id+'" value = "1" ><span></span></label>';
            retval += ' ';
return retval;
        }
    }, {
        "data": null,
        "title": "#",
        "sClass": "acttd",
        orderable: false,
        "render": function (data, type, row, meta) {
            var checked = "";
            retval = replaceAll(vselactor, "item_id", 'act_as_' + row.id);
            $.each(tempteam, function (key, item) {
                if (item.id === row.id) {

                    retval = replaceAll(retval, 'value="' + item.act_as + '"', 'value="' + item.act_as + '" selected ');
                }
            });
           
            return retval;
        }
    }, {
        "data": "app_username",
        "title": "user name",
        "sClass": "",
        orderable: true
    }, {
        "data": "app_fullname",
        "title": "full name",
        "sClass": "",
        orderable: true
    }
];
fnCompanyBrandTeam = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlListAccountManagement, {'rows':100}, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex);
    },
    savedata: function () {
        var valid = true;
        var data = TABLES[tableList].rows().data();
        var dataform = [];
        data.each(function (item, index) {
            if ($("#check_" + item.id).is(':checked')) {
                var actx = $("#act_as_" + item.id).val();
                if (actx != '') {
                    dataform.push({
                        "id": item.id,
                        "act_as": $("#act_as_" + item.id).val()
                    });
                } else {
                    valid = false;
                }
 
            }
           
        });
        if (valid) {
            var xid = $("#" + xform + ' [name="id"]').val();
            if (xid) {
                datapost = {
                    'company_brand_id': xid,
                    'team': dataform
                };
 
                ajaxPost(ajaxUrlPost, datapost, function (response) {
                    var respact = parseJson(response);
                    SuccessNotif("Data Updated");
                });
            }
        } else {
            DangerNotif("Please Complete the Form");
        }

    }
}

$(function () {
    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlListActor, {}, function (response) {
            var respact = parseJson(response);
            vselactor = '<select id="item_id"><option value="">-</value>';
            $.each(respact.rows, function (key, item) {
                vselactor += '<option value="' + item.id + '">' + item.name +'</value>';
            });
            vselactor += '</select>';
        })
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            tempteam = [];
            if (response.data) {
                if (response.data.team) {
                    tempteam = response.data.team;
                }
            }
            
            setFormAction(xform, ajaxUrlUpdate);
            fnCompanyBrandTeam.generatelist();
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
            FormLoadByDataUsingID(response.data, xform);

              
        });
    }





});

