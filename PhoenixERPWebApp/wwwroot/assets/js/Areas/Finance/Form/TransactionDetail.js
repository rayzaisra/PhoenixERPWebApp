var ajaxUrl = API + "/Finance/TransactionDetail";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "TransactionDetailList";
var formCreate = "TransactionDetailFormCreate";
var formUpdate = "TransactionDetailFormUpdate";
var formSearch = "TransactionDetailFormSearch";
var modalCreate = "TransactionDetailModalCreate";
var modalUpdate = "TransactionDetailModalUpdate";
var modalSearch = "TransactionDetailModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnTransactionDetail.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnTransactionDetail.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "account_id",
        "title": "<span class='translate'  data-args='account_id'>Account Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "affiliation_id",
        "title": "<span class='translate'  data-args='affiliation_id'>Affiliation Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "business_unit_id",
        "title": "<span class='translate'  data-args='business_unit_id'>Business Unit Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_id",
        "title": "<span class='translate'  data-args='company_id'>Company Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "credit",
        "title": "<span class='translate'  data-args='credit'>Credit</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "currency_id",
        "title": "<span class='translate'  data-args='currency_id'>Currency Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "debit",
        "title": "<span class='translate'  data-args='debit'>Debit</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "description",
        "title": "<span class='translate'  data-args='description'>Description</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "legal_entity_id",
        "title": "<span class='translate'  data-args='legal_entity_id'>Legal Entity Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "transaction_id",
        "title": "<span class='translate'  data-args='transaction_id'>Transaction Id</span>",
        "sClass": "",
        orderable: true
    }
];

fnTransactionDetail = {
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
                fnTransactionDetail.reloadlist();
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
                    fnTransactionDetail.reloadlist();
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
                    fnTransactionDetail.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnTransactionDetail.generatelist();
    initlanguage(lang_fn);
});

