var ajaxUrl = API + "/ProjectManagement/Pca";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "PcaList";
var formSearch = "PcaFormSearch";
var modalSearch = "PcaModalSearch";
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
            retval = '<a class="btn btn-outline-success m-btn m-btn--icon btn-sm m-btn--icon-only m-btn--air fakelink" href="/Projectmanagement/Pca/manage/' + row.id + '"><i class="la la-edit"></i> </a>';
            retval += ' <a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--icon-only m-btn--air" onClick="fnPca.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> </a>';
            return retval;
        }
    }, {
        "data": "code",
        "title": "code",
        "sClass": "actioncol",
        orderable: true,
          "render": function (data, type, row) {
            var datastring = '';
            if (row.statusApproved == "1" || row.statusApproved == "2") {
                datastring = '<a class="fakelink" href="/projectmanagement/pca/Approval/' + row.id + '">' + data + '</a>';
            } else {
                datastring = data;
            }

            return datastring;
        }

    }    ,{
        "data": "name",
        "title": "name",
        "sClass": "",
        orderable: true
    }, {
        "data": "job_name",
        "title": "job name",
        "sClass": "",
        orderable: true
    }, {
        "data": "company_name",
        "title": "Client",
        "sClass": "",
        orderable: true
    }, {
        "data": "brand_name",
        "title": "brand",
        "sClass": "",
        orderable: true
    }, {
        "data": "total",
        "title": "total",
        "sClass": "",
        orderable: true
    }, {
        "data": "status",
        "title": "status",
        "sClass": ""
        //orderable: true
    },  {
        "data": "revision",
        "title": "revision",
        "sClass": "",
        orderable: true
    } 
];

fnPca = {
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
                        fnPca.reloadlist();
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
    fnPca.generatelist();
    initlanguage(lang_gn);
});
 

