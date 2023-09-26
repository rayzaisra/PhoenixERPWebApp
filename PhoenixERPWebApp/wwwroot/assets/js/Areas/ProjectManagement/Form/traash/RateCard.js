var ajaxUrl = API + "/ProjectManagement/RateCard";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "RateCardList";
var formCreate = "RateCardFormCreate";
var formUpdate = "RateCardFormUpdate";
var formSearch = "RateCardFormSearch";
var modalCreate = "RateCardModalCreate";
var modalUpdate = "RateCardModalUpdate";
var modalSearch = "RateCardModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnRateCard.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnRateCard.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "description",
        "title": "<span class='translate'  data-args='description'>Description</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "flag_pick",
        "title": "<span class='translate'  data-args='flag_pick'>Flag Pick</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_category_pe_id",
        "title": "<span class='translate'  data-args='job_category_pe_id'>Job Category Pe Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_pe_id",
        "title": "<span class='translate'  data-args='job_pe_id'>Job Pe Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "rate_card_code",
        "title": "<span class='translate'  data-args='rate_card_code'>Rate Card Code</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_id",
        "title": "<span class='translate'  data-args='task_id'>Task Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "value",
        "title": "<span class='translate'  data-args='value'>Value</span>",
        "sClass": "",
        orderable: true
    }
];

fnRateCard = {
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
                fnRateCard.reloadlist();
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
                    fnRateCard.reloadlist();
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
                    fnRateCard.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnRateCard.generatelist();
    initlanguage(lang_pm);
});

