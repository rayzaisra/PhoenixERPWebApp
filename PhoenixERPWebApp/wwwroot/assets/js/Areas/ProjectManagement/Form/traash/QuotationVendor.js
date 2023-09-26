var ajaxUrl = API + "/ProjectManagement/QuotationVendor";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "QuotationVendorList";
var formCreate = "QuotationVendorFormCreate";
var formUpdate = "QuotationVendorFormUpdate";
var formSearch = "QuotationVendorFormSearch";
var modalCreate = "QuotationVendorModalCreate";
var modalUpdate = "QuotationVendorModalUpdate";
var modalSearch = "QuotationVendorModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnQuotationVendor.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnQuotationVendor.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "delivery_place",
        "title": "<span class='translate'  data-args='delivery_place'>Delivery Place</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "delivery_time",
        "title": "<span class='translate'  data-args='delivery_time'>Delivery Time</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "downpayment",
        "title": "<span class='translate'  data-args='downpayment'>Downpayment</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "expiry_date",
        "title": "<span class='translate'  data-args='expiry_date'>Expiry Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "external_id",
        "title": "<span class='translate'  data-args='external_id'>External Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "lead_time",
        "title": "<span class='translate'  data-args='lead_time'>Lead Time</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "noted",
        "title": "<span class='translate'  data-args='noted'>Noted</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "payment_id",
        "title": "<span class='translate'  data-args='payment_id'>Payment Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pe_bidding_id",
        "title": "<span class='translate'  data-args='pe_bidding_id'>Pe Bidding Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "price",
        "title": "<span class='translate'  data-args='price'>Price</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "qty",
        "title": "<span class='translate'  data-args='qty'>Qty</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "quotation_date",
        "title": "<span class='translate'  data-args='quotation_date'>Quotation Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "rfq_number",
        "title": "<span class='translate'  data-args='rfq_number'>Rfq Number</span>",
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
    }
];

fnQuotationVendor = {
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
                fnQuotationVendor.reloadlist();
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
                    fnQuotationVendor.reloadlist();
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
                    fnQuotationVendor.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnQuotationVendor.generatelist();
    initlanguage(lang_pm);
});

