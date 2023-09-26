var ajaxUrl = API + "/Finance/VwBank";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "VwBankList";
var formCreate = "VwBankFormCreate";
var formUpdate = "VwBankFormUpdate";
var formSearch = "VwBankFormSearch";
var modalCreate = "VwBankModalCreate";
var modalUpdate = "VwBankModalUpdate";
var modalSearch = "VwBankModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnVwBank.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnVwBank.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "bank_code",
        "title": "<span class='translate'  data-args='bank_code'>Bank Code</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bank_name",
        "title": "<span class='translate'  data-args='bank_name'>Bank Name</span>",
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
        "data": "swift_code",
        "title": "<span class='translate'  data-args='swift_code'>Swift Code</span>",
        "sClass": "",
        orderable: true
    }
];

fnVwBank = {
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
                fnVwBank.reloadlist();
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
                    fnVwBank.reloadlist();
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
                    fnVwBank.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnVwBank.generatelist();
    initlanguage(lang_fn);
});

