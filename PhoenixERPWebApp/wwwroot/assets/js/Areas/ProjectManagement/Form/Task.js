var ajaxUrl = API + "/ProjectManagement/Task";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "TaskList";
var formCreate = "TaskFormCreate";
var formUpdate = "TaskFormUpdate";
var formSearch = "TaskFormSearch";
var modalCreate = "TaskModalCreate";
var modalUpdate = "TaskModalUpdate";
var modalSearch = "TaskModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnTask.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnTask.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "central_resources_id",
        "title": "<span class='translate'  data-args='central_resources_id'>Central Resources Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "deadline_task",
        "title": "<span class='translate'  data-args='deadline_task'>Deadline Task</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "description",
        "title": "<span class='translate'  data-args='description'>Description</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_number",
        "title": "<span class='translate'  data-args='job_number'>Job Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_posting_requisition_id",
        "title": "<span class='translate'  data-args='job_posting_requisition_id'>Job Posting Requisition Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "start_task",
        "title": "<span class='translate'  data-args='start_task'>Start Task</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_name",
        "title": "<span class='translate'  data-args='task_name'>Task Name</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_priority_id",
        "title": "<span class='translate'  data-args='task_priority_id'>Task Priority Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_status_id",
        "title": "<span class='translate'  data-args='task_status_id'>Task Status Id</span>",
        "sClass": "",
        orderable: true
    }
];

fnTask = {
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
                fnTask.reloadlist();
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
                    fnTask.reloadlist();
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
                    fnTask.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnTask.generatelist();
    initlanguage(lang_pm);
});

