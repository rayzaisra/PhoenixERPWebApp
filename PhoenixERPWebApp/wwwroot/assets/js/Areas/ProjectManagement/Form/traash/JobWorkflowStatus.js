var ajaxUrl = API + "/ProjectManagement/JobWorkflowStatus";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "JobWorkflowStatusList";
var formCreate = "JobWorkflowStatusFormCreate";
var formUpdate = "JobWorkflowStatusFormUpdate";
var formSearch = "JobWorkflowStatusFormSearch";
var modalCreate = "JobWorkflowStatusModalCreate";
var modalUpdate = "JobWorkflowStatusModalUpdate";
var modalSearch = "JobWorkflowStatusModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name != "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
}
var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            return '<a href="javascript:void(0)" onClick="fnJobWorkflowStatus.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnJobWorkflowStatus.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "current_status_created_by",
        "title": "<span class='translate'  data-args='current_status_created_by'>Current Status Created By</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "current_status_created_date",
        "title": "<span class='translate'  data-args='current_status_created_date'>Current Status Created Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "current_status_id",
        "title": "<span class='translate'  data-args='current_status_id'>Current Status Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_number",
        "title": "<span class='translate'  data-args='job_number'>Job Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "last_status_created_by",
        "title": "<span class='translate'  data-args='last_status_created_by'>Last Status Created By</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "last_status_created_date",
        "title": "<span class='translate'  data-args='last_status_created_date'>Last Status Created Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "last_status_id",
        "title": "<span class='translate'  data-args='last_status_id'>Last Status Id</span>",
        "sClass": "",
        orderable: true
    }
];

fnJobWorkflowStatus = {
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
        if (x == "reset") {
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
                fnJobWorkflowStatus.reloadlist();
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
                    fnJobWorkflowStatus.reloadlist();
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
                    fnJobWorkflowStatus.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnJobWorkflowStatus.generatelist();
    initlanguage(lang_pm);
});

