var ajaxUrl = API + "/ProjectManagement/Pca";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "PCAList";
var formCreate = "PCAFormCreate";
var formUpdate = "PCAFormUpdate";
var formSearch = "PCAFormSearch";
var modalCreate = "PCAModalCreate";
var modalUpdate = "PCAModalUpdate";
var modalSearch = "PCAModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
};
var columnsIndex = [
    {
        "data": "Id",
        "title": "",
        "sClass": "ecol x20",
        orderable: false,
        render: function (data, type, row, meta) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/pca/manage/' + row.pa_number + '"><i class="la la-edit"></i> Edit</a>';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/pca/cashadvance/' + row.pa_number + '"><i class="la la-edit"></i> Cash Advance</a>';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/pca/cashadvance/' + row.pa_number + '"><i class="la la-edit"></i> Cash Advance</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;
        }

    },
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
        "data": 'company_name',
        "title": "<span class='translate'  data-args='Client'>Client</span>",
        "sClass": "actioncol",
        orderable: true
    }    ,{
        "data": "job_name",
        "title": "<span class='translate'  data-args='Job Name'>Job Name</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_detail_id",
        "title": "<span class='translate'  data-args='job_number'>job_number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "mother_pca",
        "title": "<span class='translate'  data-args='mother_pca'>mother_pca</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pa_number",
        "title": "<span class='translate'  data-args='pca_number'>pca_number</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": null,
        "title": "<span class='translate'  data-args='PO Number'>PO Number</span>",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return "";
        }
    }    ,{
        "data": null,
        "title": "<span class='translate'  data-args='CA Number'>CA Number</span>",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return "";
        }
    }    ,{
        "data": null,
        "title": "<span class='translate'  data-args='proc number'>proc number</span>",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return "";
        }
    }  
];

fnPCA = {
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
                fnPCA.reloadlist();
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
                    fnPCA.reloadlist();
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
                    fnPCA.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnPCA.generatelist();
    initlanguage(lang_pm);
});

