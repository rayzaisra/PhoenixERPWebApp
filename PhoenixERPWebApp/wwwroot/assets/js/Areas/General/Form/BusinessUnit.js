var ajaxUrl = API + "/General/BusinessUnit";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "BusinessUnitList";
var formSearch = "BusinessUnitFormSearch";
var modalSearch = "BusinessUnitModalSearch";
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
        "data": "business_unit_type_id",
        "title": "business_unit_type_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "default_team",
        "title": "default_team",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "latitude",
        "title": "latitude",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "longitude",
        "title": "longitude",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "organization_id",
        "title": "organization_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "parent_unit",
        "title": "parent_unit",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_address",
        "title": "unit_address",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_code",
        "title": "unit_code",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_description",
        "title": "unit_description",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_email",
        "title": "unit_email",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_fax",
        "title": "unit_fax",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_name",
        "title": "unit_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "unit_phone",
        "title": "unit_phone",
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
            retval += '<a class="dropdown-item fakelink" href="/General/BusinessUnit/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnBusinessUnit.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnBusinessUnit = {
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
                        fnBusinessUnit.reloadlist();
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
    fnBusinessUnit.generatelist();
    initlanguage(lang_gn);
});
 

