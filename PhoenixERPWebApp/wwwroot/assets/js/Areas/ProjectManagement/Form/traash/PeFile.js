var ajaxUrl = API + "/ProjectManagement/PeFile";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "PeFileList";
var formCreate = "PeFileFormCreate";
var formUpdate = "PeFileFormUpdate";
var formSearch = "PeFileFormSearch";
var modalCreate = "PeFileModalCreate";
var modalUpdate = "PeFileModalUpdate";
var modalSearch = "PeFileModalSearch";
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

            return '<a href="javascript:void(0)" onClick="fnPeFile.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnPeFile.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }    ,{
        "data": "file_name",
        "title": "<span class='translate'  data-args='file_name'>File Name</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "file_path",
        "title": "<span class='translate'  data-args='file_path'>File Path</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "file_size",
        "title": "<span class='translate'  data-args='file_size'>File Size</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "file_type",
        "title": "<span class='translate'  data-args='file_type'>File Type</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "flag",
        "title": "<span class='translate'  data-args='flag'>Flag</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "upload_date",
        "title": "<span class='translate'  data-args='upload_date'>Upload Date</span>",
        "sClass": "",
        orderable: true
    }
];

fnPeFile = {
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
                fnPeFile.reloadlist();
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
                    fnPeFile.reloadlist();
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
                    fnPeFile.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
};

$(function () {
    fnPeFile.generatelist();
    initlanguage(lang_pm);
});

