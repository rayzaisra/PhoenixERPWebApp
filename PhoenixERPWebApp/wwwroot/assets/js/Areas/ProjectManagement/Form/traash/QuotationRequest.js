var ajaxUrl = API + "/ProjectManagement/QuotationRequest";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "QuotationRequestList";
var formCreate = "QuotationRequestFormCreate";
var formUpdate = "QuotationRequestFormUpdate";
var formSearch = "QuotationRequestFormSearch";
var modalCreate = "QuotationRequestModalCreate";
var modalUpdate = "QuotationRequestModalUpdate";
var modalSearch = "QuotationRequestModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnQuotationRequest.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnQuotationRequest.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "closing_date",
        "title": "<span class='translate'  data-args='closing_date'>Closing Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_id",
        "title": "<span class='translate'  data-args='company_id'>Company Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "days",
        "title": "<span class='translate'  data-args='days'>Days</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "delivery_date",
        "title": "<span class='translate'  data-args='delivery_date'>Delivery Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "delivery_place",
        "title": "<span class='translate'  data-args='delivery_place'>Delivery Place</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "fq_date",
        "title": "<span class='translate'  data-args='fq_date'>Fq Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "payment_id",
        "title": "<span class='translate'  data-args='payment_id'>Payment Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "remarks",
        "title": "<span class='translate'  data-args='remarks'>Remarks</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "shift",
        "title": "<span class='translate'  data-args='shift'>Shift</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "task_id",
        "title": "<span class='translate'  data-args='task_id'>Task Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "transaction_type_id",
        "title": "<span class='translate'  data-args='transaction_type_id'>Transaction Type Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "weeks",
        "title": "<span class='translate'  data-args='weeks'>Weeks</span>",
        "sClass": "",
        orderable: true
    }
];

fnQuotationRequest = {
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
                fnQuotationRequest.reloadlist();
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
                    fnQuotationRequest.reloadlist();
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
                    fnQuotationRequest.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnQuotationRequest.generatelist();
    initlanguage(lang_pm);
});

