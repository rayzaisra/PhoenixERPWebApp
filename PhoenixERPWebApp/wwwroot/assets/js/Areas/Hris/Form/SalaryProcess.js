var ajaxUrl = API + "/Hris/SalaryProcess";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "SalaryProcessList";
var formSearch = "SalaryProcessFormSearch";
var modalSearch = "SalaryProcessModalSearch";
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
        "data": "basic_salary",
        "title": "basic_salary",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bonus",
        "title": "bonus",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bruto",
        "title": "bruto",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "bruto_ofyears",
        "title": "bruto_ofyears",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "employee_basic_info_id",
        "title": "employee_basic_info_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "employement_status_id",
        "title": "employement_status_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "loan",
        "title": "loan",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "location",
        "title": "location",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "month",
        "title": "month",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "netto",
        "title": "netto",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "netto_ofyears",
        "title": "netto_ofyears",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pkp_ofyears",
        "title": "pkp_ofyears",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "position_cost",
        "title": "position_cost",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "ptkp_ofmonths",
        "title": "ptkp_ofmonths",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "ptkp_ofyears",
        "title": "ptkp_ofyears",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "status_employee_id",
        "title": "status_employee_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "tax_no_npwp",
        "title": "tax_no_npwp",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "tax_ofmonths",
        "title": "tax_ofmonths",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "tax_ofyears",
        "title": "tax_ofyears",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "transport",
        "title": "transport",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "year",
        "title": "year",
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/SalaryProcess/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnSalaryProcess.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnSalaryProcess = {
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
                        fnSalaryProcess.reloadlist();
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
    fnSalaryProcess.generatelist();
    initlanguage(lang_hr);
});
 

