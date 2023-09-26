var ajaxUrl = API + "/General/AuditTrail";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "AuditTrailList";
var formSearch = "AuditTrailFormSearch";
var modalSearch = "AuditTrailModalSearch";
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
        "data": "application_entity_id",
        "title": "application_entity_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "application_user_id",
        "title": "application_user_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "app_fullname",
        "title": "app_fullname",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "app_username",
        "title": "app_username",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "new_record",
        "title": "new_record",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "new_view",
        "title": "new_view",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "old_record",
        "title": "old_record",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "old_view",
        "title": "old_view",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "organization_id",
        "title": "organization_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "organization_name",
        "title": "organization_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "record_id",
        "title": "record_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "user_action",
        "title": "user_action",
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
            retval += '<a class="dropdown-item fakelink" href="/General/AuditTrail/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnAuditTrail.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnAuditTrail = {
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
                        fnAuditTrail.reloadlist();
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
    fnAuditTrail.generatelist();
    initlanguage(lang_gn);
});
 

