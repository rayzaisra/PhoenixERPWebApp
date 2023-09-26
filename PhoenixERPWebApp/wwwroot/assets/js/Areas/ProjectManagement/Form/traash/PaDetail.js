var ajaxUrl = API + "/ProjectManagement/PaDetail";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "PaDetailList";
var formCreate = "PaDetailFormCreate";
var formUpdate = "PaDetailFormUpdate";
var formSearch = "PaDetailFormSearch";
var modalCreate = "PaDetailModalCreate";
var modalUpdate = "PaDetailModalUpdate";
var modalSearch = "PaDetailModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnPaDetail.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnPaDetail.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "brand",
        "title": "<span class='translate'  data-args='brand'>Brand</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "category",
        "title": "<span class='translate'  data-args='category'>Category</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "channel",
        "title": "<span class='translate'  data-args='channel'>Channel</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "item",
        "title": "<span class='translate'  data-args='item'>Item</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_pa_id",
        "title": "<span class='translate'  data-args='job_pa_id'>Job Pa Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "price_per_quantity",
        "title": "<span class='translate'  data-args='price_per_quantity'>Price Per Quantity</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "quantity",
        "title": "<span class='translate'  data-args='quantity'>Quantity</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "remarks",
        "title": "<span class='translate'  data-args='remarks'>Remarks</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "services",
        "title": "<span class='translate'  data-args='services'>Services</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "sub_brand",
        "title": "<span class='translate'  data-args='sub_brand'>Sub Brand</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "sub_task",
        "title": "<span class='translate'  data-args='sub_task'>Sub Task</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task",
        "title": "<span class='translate'  data-args='task'>Task</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_name",
        "title": "<span class='translate'  data-args='task_name'>Task Name</span>",
        "sClass": "",
        orderable: true
    }
];

fnPaDetail = {
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
                fnPaDetail.reloadlist();
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
                    fnPaDetail.reloadlist();
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
                    fnPaDetail.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnPaDetail.generatelist();
    initlanguage(lang_pm);
});

