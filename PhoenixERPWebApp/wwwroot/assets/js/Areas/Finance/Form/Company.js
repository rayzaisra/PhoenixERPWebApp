var ajaxUrl = API + "/Finance/Company";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "CompanyList";
var formCreate = "CompanyFormCreate";
var formUpdate = "CompanyFormUpdate";
var formSearch = "CompanyFormSearch";
var modalCreate = "CompanyModalCreate";
var modalUpdate = "CompanyModalUpdate";
var modalSearch = "CompanyModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnCompany.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnCompany.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "company_name",
        "title": "<span class='translate'  data-args='company_name'>Company Name</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "email",
        "title": "<span class='translate'  data-args='email'>Email</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "facebook",
        "title": "<span class='translate'  data-args='facebook'>Facebook</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "fax",
        "title": "<span class='translate'  data-args='fax'>Fax</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "instagram",
        "title": "<span class='translate'  data-args='instagram'>Instagram</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "is_client",
        "title": "<span class='translate'  data-args='is_client'>Is Client</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "is_vendor",
        "title": "<span class='translate'  data-args='is_vendor'>Is Vendor</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "mobile_phone",
        "title": "<span class='translate'  data-args='mobile_phone'>Mobile Phone</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "organization_id",
        "title": "<span class='translate'  data-args='organization_id'>Organization Id</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "payment_term_day",
        "title": "<span class='translate'  data-args='payment_term_day'>Payment Term Day</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "phone",
        "title": "<span class='translate'  data-args='phone'>Phone</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "registration_code",
        "title": "<span class='translate'  data-args='registration_code'>Registration Code</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "shipping_term_day",
        "title": "<span class='translate'  data-args='shipping_term_day'>Shipping Term Day</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "tax_number",
        "title": "<span class='translate'  data-args='tax_number'>Tax Number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "twitter",
        "title": "<span class='translate'  data-args='twitter'>Twitter</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "website",
        "title": "<span class='translate'  data-args='website'>Website</span>",
        "sClass": "",
        orderable: true
    }
];

fnCompany = {
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
                fnCompany.reloadlist();
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
                    fnCompany.reloadlist();
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
                    fnCompany.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnCompany.generatelist();
    initlanguage(lang_fn);
});

