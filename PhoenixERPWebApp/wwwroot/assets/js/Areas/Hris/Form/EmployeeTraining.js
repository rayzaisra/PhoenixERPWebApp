var ajaxUrl = API + "/Hris/EmployeeTraining";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "EmployeeTrainingList";
var formSearch = "EmployeeTrainingFormSearch";
var modalSearch = "EmployeeTrainingModalSearch";
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
    }    ,{
        "data": "address",
        "title": "address",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "description",
        "title": "description",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "employee_basic_info_id",
        "title": "employee_basic_info_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "end_training",
        "title": "end_training",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "expired_certificate",
        "title": "expired_certificate",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "file_upload",
        "title": "file_upload",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "institusion_id",
        "title": "institusion_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "name_training",
        "title": "name_training",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "no_certification",
        "title": "no_certification",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "start_training",
        "title": "start_training",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/Hris/EmployeeTraining/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnEmployeeTraining.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnEmployeeTraining = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex)
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        $("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x == "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();
        $("#" + modalSearch).modal('hide');
    },
    delete: function (id) {
        var text = t_delete;
        swal({
            title: "Confirmation",
            text: t_delete,
            type: "warning",
            showCancelButton: !0,
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnEmployeeTraining.reloadlist();
                        hideAllModal();
                    } else {
                        DangerNotif(response.message);
                    }
                });
            }


        });

    }
};

$(function () {
    fnEmployeeTraining.generatelist();
    initlanguage(lang_hr);
});
 

