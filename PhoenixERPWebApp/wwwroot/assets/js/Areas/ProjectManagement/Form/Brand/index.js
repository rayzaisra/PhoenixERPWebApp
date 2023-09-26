var ajaxUrl = API + "/ProjectManagement/CompanyBrand";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "CompanyBrandList";
var formSearch = "CompanyBrandFormSearch";
var modalSearch = "CompanyBrandModalSearch";
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
        "sClass": "actioncol3x",
        orderable: false,
        "render": function (data, type, row, meta) {
            retval = '<div class="btn-group">';
            retval += '<a class="m-btn btn btn-sm btn-primary fakelink" href="/Projectmanagement/Brand/update/' + row.id + '"><i class="la la-edit"></i></a>';
            retval += '<a class="m-btn btn btn-sm btn-danger" onClick="fnCompanyBrand.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i></a>';
            retval += '<a class="m-btn btn btn-sm btn-success fakelink" href="/Projectmanagement/Brand/team/' + row.id + '"><i class="fa fa-users"></i></a>';
            retval += '</div>';
            return retval;
        }
    }, {
        "data": "brand_name",
        "title": "name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_name",
        "title": "Company",
        "sClass": "",
        orderable: true
    }, {
        "data": "sub_brand",
        "title": "Sub Brand",
        "sClass": "",
        orderable: true
    } 
];

fnCompanyBrand = {
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
                        fnCompanyBrand.reloadlist();
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
    fnCompanyBrand.generatelist();
    initlanguage(lang_gn);
});
 

