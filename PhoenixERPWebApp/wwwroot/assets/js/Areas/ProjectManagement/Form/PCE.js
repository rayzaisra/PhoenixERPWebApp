var ajaxUrl = API + "/ProjectManagement/JobPe";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "JobPeList";
var formCreate = "JobPeFormCreate";
var formUpdate = "JobPeFormUpdate";
var formSearch = "JobPeFormSearch";
var modalCreate = "JobPeModalCreate";
var modalUpdate = "JobPeModalUpdate";
var modalSearch = "JobPeModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnJobPe.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnJobPe.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "approval_id",
        "title": "<span class='translate'  data-args='approval_id'>Approval Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "ca_number",
        "title": "<span class='translate'  data-args='ca_number'>Ca Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "child_pce_number",
        "title": "<span class='translate'  data-args='child_pce_number'>Child Pce Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "detail",
        "title": "<span class='translate'  data-args='detail'>Detail</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "due_date",
        "title": "<span class='translate'  data-args='due_date'>Due Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "file_id",
        "title": "<span class='translate'  data-args='file_id'>File Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_detail_id",
        "title": "<span class='translate'  data-args='job_detail_id'>Job Detail Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_pa_id",
        "title": "<span class='translate'  data-args='job_pa_id'>Job Pa Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "margin",
        "title": "<span class='translate'  data-args='margin'>Margin</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pe_cost",
        "title": "<span class='translate'  data-args='pe_cost'>Pe Cost</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pe_date",
        "title": "<span class='translate'  data-args='pe_date'>Pe Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pe_line_item",
        "title": "<span class='translate'  data-args='pe_line_item'>Pe Line Item</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pe_number",
        "title": "<span class='translate'  data-args='pe_number'>Pe Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "proc_number",
        "title": "<span class='translate'  data-args='proc_number'>Proc Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "submitted_by",
        "title": "<span class='translate'  data-args='submitted_by'>Submitted By</span>",
        "sClass": "",
        orderable: true
    }
];

fnJobPe = {
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
                fnJobPe.reloadlist();
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
                    fnJobPe.reloadlist();
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
                    fnJobPe.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnJobPe.generatelist();
    initlanguage(lang_pm);
});

