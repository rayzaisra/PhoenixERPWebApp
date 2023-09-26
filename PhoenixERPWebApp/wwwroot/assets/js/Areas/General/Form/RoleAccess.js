var ajaxUrl = API + "/General/RoleAccess";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "RoleAccessList";
var formSearch = "RoleAccessFormSearch";
var modalSearch = "RoleAccessModalSearch";
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
        "data": "access_activate",
        "title": "access_activate",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_append",
        "title": "access_append",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_approve",
        "title": "access_approve",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_create",
        "title": "access_create",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_delete",
        "title": "access_delete",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_level",
        "title": "access_level",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_lock",
        "title": "access_lock",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_read",
        "title": "access_read",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_share",
        "title": "access_share",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "access_update",
        "title": "access_update",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "application_entity_id",
        "title": "application_entity_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "application_role_id",
        "title": "application_role_id",
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
            retval += '<a class="dropdown-item fakelink" href="/General/RoleAccess/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnRoleAccess.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnRoleAccess = {
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
                        fnRoleAccess.reloadlist();
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
    fnRoleAccess.generatelist();
    initlanguage(lang_gn);
});
 

