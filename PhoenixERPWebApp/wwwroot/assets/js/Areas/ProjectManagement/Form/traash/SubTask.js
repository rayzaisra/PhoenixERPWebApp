var ajaxUrl = API + "/ProjectManagement/SubTask";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "SubTaskList";
var formCreate = "SubTaskFormCreate";
var formUpdate = "SubTaskFormUpdate";
var formSearch = "SubTaskFormSearch";
var modalCreate = "SubTaskModalCreate";
var modalUpdate = "SubTaskModalUpdate";
var modalSearch = "SubTaskModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnSubTask.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnSubTask.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "attachment",
        "title": "<span class='translate'  data-args='attachment'>Attachment</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "description",
        "title": "<span class='translate'  data-args='description'>Description</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "due_date",
        "title": "<span class='translate'  data-args='due_date'>Due Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "hour",
        "title": "<span class='translate'  data-args='hour'>Hour</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "percentage",
        "title": "<span class='translate'  data-args='percentage'>Percentage</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "start_date",
        "title": "<span class='translate'  data-args='start_date'>Start Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "sub_task",
        "title": "<span class='translate'  data-args='sub_task'>Sub Task</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_id",
        "title": "<span class='translate'  data-args='task_id'>Task Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_status_id",
        "title": "<span class='translate'  data-args='task_status_id'>Task Status Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "user_id",
        "title": "<span class='translate'  data-args='user_id'>User Id</span>",
        "sClass": "",
        orderable: true
    }
];

fnSubTask = {
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
                fnSubTask.reloadlist();
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
                    fnSubTask.reloadlist();
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
                    fnSubTask.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnSubTask.generatelist();
    initlanguage(lang_pm);
});

