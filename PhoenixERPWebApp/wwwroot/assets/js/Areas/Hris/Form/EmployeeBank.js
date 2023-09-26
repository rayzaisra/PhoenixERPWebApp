var ajaxUrl = API + "/Hris/EmployeeBank";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "EmployeeBankList";
var formSearch = "EmployeeBankFormSearch";
var modalSearch = "EmployeeBankModalSearch";
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
        "data": "account_name",
        "title": "account_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "account_no",
        "title": "account_no",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bank_address",
        "title": "bank_address",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bank_city",
        "title": "bank_city",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bank_id",
        "title": "bank_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "branch",
        "title": "branch",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "currency_id",
        "title": "currency_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "employee_basic_info_id",
        "title": "employee_basic_info_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "end_date",
        "title": "end_date",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "start_date",
        "title": "start_date",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "sub_bank_id",
        "title": "sub_bank_id",
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/EmployeeBank/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnEmployeeBank.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnEmployeeBank = {
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
                        fnEmployeeBank.reloadlist();
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
    fnEmployeeBank.generatelist();
    initlanguage(lang_hr);
});
 

