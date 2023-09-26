var ajaxUrl = API + "/ProjectManagement/MasterSearchContent";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "MasterSearchContentList";
var formCreate = "MasterSearchContentFormCreate";
var formUpdate = "MasterSearchContentFormUpdate";
var formSearch = "MasterSearchContentFormSearch";
var modalCreate = "MasterSearchContentModalCreate";
var modalUpdate = "MasterSearchContentModalUpdate";
var modalSearch = "MasterSearchContentModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnMasterSearchContent.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnMasterSearchContent.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "created_date",
        "title": "<span class='translate'  data-args='created_date'>Created Date</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "isdeleted",
        "title": "<span class='translate'  data-args='isdeleted'>Isdeleted</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "screen",
        "title": "<span class='translate'  data-args='screen'>Screen</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "search_content",
        "title": "<span class='translate'  data-args='search_content'>Search Content</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "updated_by",
        "title": "<span class='translate'  data-args='updated_by'>Updated By</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "updated_date",
        "title": "<span class='translate'  data-args='updated_date'>Updated Date</span>",
        "sClass": "",
        orderable: true
    }
];

fnMasterSearchContent = {
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
                fnMasterSearchContent.reloadlist();
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
                    fnMasterSearchContent.reloadlist();
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
                    fnMasterSearchContent.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnMasterSearchContent.generatelist();
    initlanguage(lang_pm);
});

