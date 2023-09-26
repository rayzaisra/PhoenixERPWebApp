var ajaxUrl = API + "/General/ApplicationUser";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "ApplicationUserList";
var formSearch = "ApplicationUserFormSearch";
var modalSearch = "ApplicationUserModalSearch";
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
        "data": "access_token",
        "title": "access_token",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "app_fullname",
        "title": "app_fullname",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "app_password",
        "title": "app_password",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "app_username",
        "title": "app_username",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "business_unit_id",
        "title": "business_unit_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "email",
        "title": "email",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "is_system_administrator",
        "title": "is_system_administrator",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "manager_id",
        "title": "manager_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "mime_type",
        "title": "mime_type",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "organization_id",
        "title": "organization_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "phone",
        "title": "phone",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "photo_base64",
        "title": "photo_base64",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "primary_team",
        "title": "primary_team",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "thumbnail_base64",
        "title": "thumbnail_base64",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "token_expiry",
        "title": "token_expiry",
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
            retval += '<a class="dropdown-item fakelink" href="/General/ApplicationUser/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnApplicationUser.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnApplicationUser = {
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
                        fnApplicationUser.reloadlist();
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
    fnApplicationUser.generatelist();
    initlanguage(lang_gn);
});
 

