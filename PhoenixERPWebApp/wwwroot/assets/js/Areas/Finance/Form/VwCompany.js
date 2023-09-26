var ajaxUrl = API + "/Finance/VwCompany";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "VwCompanyList";
var formCreate = "VwCompanyFormCreate";
var formUpdate = "VwCompanyFormUpdate";
var formSearch = "VwCompanyFormSearch";
var modalCreate = "VwCompanyModalCreate";
var modalUpdate = "VwCompanyModalUpdate";
var modalSearch = "VwCompanyModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnVwCompany.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnVwCompany.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
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
        "data": "organization_name",
        "title": "<span class='translate'  data-args='organization_name'>Organization Name</span>",
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
        "data": "record_approved_by",
        "title": "<span class='translate'  data-args='record_approved_by'>Record Approved By</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "record_created_by",
        "title": "<span class='translate'  data-args='record_created_by'>Record Created By</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "record_modified_by",
        "title": "<span class='translate'  data-args='record_modified_by'>Record Modified By</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "record_owner",
        "title": "<span class='translate'  data-args='record_owner'>Record Owner</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "record_owning_team",
        "title": "<span class='translate'  data-args='record_owning_team'>Record Owning Team</span>",
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

fnVwCompany = {
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
                fnVwCompany.reloadlist();
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
                    fnVwCompany.reloadlist();
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
                    fnVwCompany.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnVwCompany.generatelist();
    initlanguage(lang_fn);
});

