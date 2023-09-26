var ajaxUrl = API + "/Hris/SalaryComponent";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "SalaryComponentList";
var formCreate = "SalaryComponentFormCreate";
var formUpdate = "SalaryComponentFormUpdate";
var formSearch = "SalaryComponentFormSearch";
var modalCreate = "SalaryComponentModalCreate";
var modalUpdate = "SalaryComponentModalUpdate";
var modalSearch = "SalaryComponentModalSearch";
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
        "title": "No",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }
    , {
        "data": "code",
        "title": "<span class='translate'  data-args='Code'>Code</span>",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "salary_component",
        "title": "<span class='translate'  data-args='Salary Component'>Salary Component</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "description",
    "title": "<span class='translate'  data-args='Description'>Description</span>",
    "sClass": "",
    orderable: true
    }  ,
    {
        "data": "created_on",
        "title": "<span class='translate'  data-args='Created Date'>Created Date</span>",
        "sClass": "",
        orderable: false,
        "render": function (data, type, row) {
            return moment(row.created_on).locale('id').format('LL');
        }
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='Actions'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/Hris/SalaryComponent/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnSalaryComponent.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }     
];

fnSalaryComponent = {
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
                        fnSalaryComponent.reloadlist();
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
    fnSalaryComponent.generatelist();
    initlanguage(lang_hr);
});

