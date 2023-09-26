var ajaxUrl = API + "ProjectManagement/Job";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "JobList";
var formCreate = "JobFormCreate";
var formUpdate = "JobFormUpdate";
var formSearch = "JobFormSearch";
var modalCreate = "JobModalCreate";
var modalUpdate = "JobModalUpdate";
var modalSearch = "JobModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var search_id = $("#search_id").val();
    var search_val = $("#search_val").val();
    var data = {};
    var user = JSON.parse(sessionStorage.getItem("data"));

    data = {
        'user_id': user.AppUserId,
        'group_id': user.GroupAccess
    };
    if (search_val) {
        data[search_id] = search_val;
        }
 

    return data;
};

var columnsIndex = [
	
    {
        "data": "job_number",
        "title": "Job Number",
        "sClass": "rcol",
        orderable: true,
        render: function (data, type, row, meta) {
            retval = '';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/job/detail/' + row.id + '">'+data+'</a>';
              return retval;
        }
    },
    {
        "data": "job_name",
        "title": "Job Name",
        "sClass": "rcol",
        orderable: true
    },
	{
		"data": "brand_name",
		"title": "Brand",
		"sClass": "lcol",
		orderable: true,
	},
	{
		"data": "company_name",
		"title": "Company Name",
		"sClass": "lcol",
		orderable: true,
	},	
	{
		"data": "campaign_name",
		"title": "Campaign Name",
		"sClass": "rcol",
        orderable: true
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
    },
    {
        "data": "status_name",
        "title": "Status",
        "sClass": "rcol",
        orderable: true
    }
];

fnJob = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'created_on', 'desc');
        setcolvis(tableList, columnsIndex)
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        UIkit.modal("#" + modalSearch).show();
    },
    initsearch: function () {
        TABLES[tableList].ajax.reload();
    },
    create: function () {
        var optionsForm = GetOptionsForm(function () {
            return $("#" + formCreate).parsley().isValid();
        }, function (response, statusText, xhr, $form) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                fnJob.reloadlist();
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
                    fnJob.reloadlist();
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
                    fnJob.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnJob.generatelist();
    initlanguage(lang_pm);
});

