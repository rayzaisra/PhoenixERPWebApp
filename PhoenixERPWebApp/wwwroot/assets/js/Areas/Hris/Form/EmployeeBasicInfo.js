var ajaxUrl = API + "/Hris/EmployeeBasicInfo";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var tableList = "EmployeeBasicInfoList";

var formCreate = "EmployeeBasicInfoFormCreate";
var formUpdate = "EmployeeBasicInfoFormUpdate";
var formSearch = "EmployeeBasicInfoFormSearch";
var modalCreate = "EmployeeBasicInfoModalCreate";
var modalUpdate = "EmployeeBasicInfoModalUpdate";
var modalSearch = "EmployeeBasicInfoModalSearch";
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
    }, {
        "data": "name_employee",
        "title": "<span class='translate'  data-args='name_employee'>Employee Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "division",
        "title": "<span class='translate'  data-args='Division'>Division</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "job_title",
        "title": "<span class='translate'  data-args='job_title'>Job Title Requested</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "grade",
        "title": "<span class='translate'  data-args='Grade'>Grade</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "employee_status",
        "title": "<span class='translate'  data-args='Employee Status'>Employee Status</span>",
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/EmployeeBasicInfo/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnEmployeeBasicInfo.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;
        }
    }
];

fnEmployeeBasicInfo = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex)
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        // UIkit.modal("#" + modalSearch).show();
    },
    delete: function (id) {
        debugger;
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
                        fnEmployeeBasicInfo.reloadlist();
                    } else {
                        DangerNotif(response.message);
                    }
                });
            }


        });

    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();

        $('#' + modalSearch + '').modal('hide');
        // UIkit.modal("#" + modalSearch).hide();
    }
};

$(function () {
    // Load Grid
    fnEmployeeBasicInfo.generatelist();
    initlanguage(lang_hr);
});

