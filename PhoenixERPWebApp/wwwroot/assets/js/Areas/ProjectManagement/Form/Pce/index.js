var ajaxUrl = API + "/ProjectManagement/Pce";
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
            retval = '<a class="btn btn-outline-success m-btn m-btn--icon btn-sm m-btn--icon-only m-btn--air fakelink" href="/Projectmanagement/Pce/view/' + row.id + '"><i class="fa fa-eye"></i> </a>';
           
            return retval;
        }
    }, {
        "data": "company_name",
        "title": "Client Name",
        "sClass": "",
        orderable: true
    }, {
        "data": "brand_name",
        "title": "Brand",
        "sClass": "",
        orderable: true
    }, {
        "data": "job_id",
        "title": "Job Number",
        "sClass": "",
        orderable: true
    }, {
        "data": "job_name",
        "title": "Job",
        "sClass": "",
        orderable: true
    }, {
        "data": "job_status",
        "title": "Job status",
        "sClass": "",
        orderable: true
    }, {
        "data": "code",
        "title": "PCE Number",
        "sClass": "",
        orderable: true
    }, {
        "data": "status",
        "title": "PCE Status",
        "sClass": "",
        orderable: true
    }, {
        "data": "created_date",
        "title": "Created",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return moment(data).format('YYYY-MM-DD');;
        }
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


