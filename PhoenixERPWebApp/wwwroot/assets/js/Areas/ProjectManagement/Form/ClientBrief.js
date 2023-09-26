var ajaxUrl = API + "/ProjectManagement/ClientBrief";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "ClientBriefList";
var formCreate = "ClientBriefFormCreate";
var formUpdate = "ClientBriefFormUpdate";
var formSearch = "ClientBriefFormSearch";
var modalCreate = "ClientBriefModalCreate";
var modalUpdate = "ClientBriefModalUpdate";
var modalSearch = "ClientBriefModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
}
var columnsIndex = [
	{
		"data": "Id",
		"title": "",
		"sClass": "ecol x20",
		orderable: false,
        render: function (data, type, row, meta) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/clientbrief/update/' + row.id + '"><i class="la la-edit"></i> Edit</a>';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/clientbrief/job/' + row.id + '"><i class="la la-edit"></i> Job</a>';
            retval += '</div>';
            retval +='</span>';
            return retval;
		}
		
	},
	{
		"data": "brand_name",
		"title": "Brand",
		"sClass": "lcol",
		orderable: true,
	},
	{
        "data": "accountmanager",
		"title": "Account Management",
		"sClass": "rcol",
        orderable: true,
        "render": function (data, type, row) {
            rdata = '';
            if (data) {
                //rdata += '<ul>';
                xmn = 0;
                $.each(data, function (key, value) {
                    //rdata += '<li>' + value.app_username + '</li>';
                    if (xmn > 0) rdata += ', ';
                    rdata += value;
                    xmn++;
                });
                //rdata += '</<ul>';
            }
            return rdata;
		}
	},
	{
		"data": "campaign_name",
		"title": "Campaign Name",
		"sClass": "rcol",
        orderable: true
	},
	{
		"data": "job_name",
		"title": "Job Name",
		"sClass": "rcol",
		orderable: false,
		"render": function (data, type, row) {
            rdata = '';
            if (data) {
                //rdata += '<ul>';
                xmn = 0;
                $.each(data, function (key, value) {
                    //rdata += '<li>' + value.app_username + '</li>';
                    if (xmn > 0) rdata += ', ';
                    rdata += value;
                    xmn++;
                });
                //rdata += '</<ul>';
            }
            return rdata;
		}
	},
    {
        "data": "job_number",
        "title": "Job Number",
        "sClass": "rcol",
        orderable: false,
        "render": function (data, type, row) {
            rdata = '';
            if (data) {
                //rdata += '<ul>';
                xmn = 0;
                $.each(data, function (key, value) {
                    //rdata += '<li>' + value.app_username + '</li>';
                    if (xmn > 0) rdata += ', ';
                    rdata += '<a href="' + WEB + '/projectmanagement/job/detail/' + value+'" class="fakelink">'+value+'</a>';
                    xmn++;
                });
                //rdata += '</<ul>';
            }
            return rdata;
        }
    },
	{
		"data": "business_unit_name",
		"title": "Business Unit",
		"sClass": "rcol",
		orderable: false
	},
	{
		"data": "deadline",
		"title": "Deadline",
		"sClass": "rcol",
		orderable: false,
		"render": function (data) {
			var date = new Date(data);
			var month = date.getMonth() + 1;
			return date.getDate() + "/" + month + "/" + date.getFullYear();
		}
	},
	{
		"data": "final_delivery",
		"title": "Final Delivery",
		"sClass": "rcol",
		orderable: false,
		"render": function (data) {
			var date = new Date(data);
			var month = date.getMonth() + 1;
			return date.getDate() + "/" + month + "/" + date.getFullYear();
		}
	}

];


fnClientBrief = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex)
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        UIkit.modal("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();
        UIkit.modal("#" + modalSearch).hide();
    },

    create: function () {
        var optionsForm = GetOptionsForm(function () {
            return $("#" + formCreate).parsley().isValid();
        }, function (response, statusText, xhr, $form) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                fnClientBrief.reloadlist();
                UIkit.modal("#" + modalCreate).hide();
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(formCreate, optionsForm);
        setFormAction(formCreate, ajaxUrlCreate);
        UIkit.modal("#" + modalCreate).show();
    },
    update: function (id) {
        ajaxGet(ajaxUrlGet + "/" + id, {}, function (response) {
            response = parseJson(response);

            UIkit.modal("#" + modalUpdate).show();
            setFormAction(formUpdate, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                return $("#" + formUpdate).parsley().isValid();
            }, function (response, statusText, xhr, $form) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    fnClientBrief.reloadlist();
                    UIkit.modal("#" + modalUpdate).hide();
                } else {
                    DangerNotif(response.message);
                }
            });
            InitForm(formUpdate, optionsForm);
            FormLoadByDataUsingName(response.data, formUpdate);
        });
    },
    delete: function (id) {
        var text = t_delete;
        confirmDialog(text, function () {
            ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_deletesuccess);
                    fnClientBrief.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnClientBrief.generatelist();
    initlanguage(lang_pm);
});

