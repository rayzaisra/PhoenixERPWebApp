var ajaxUrl = API + "/ProjectManagement/RateCard";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "RateCardList";
var formSearch = "RateCardFormSearch";
var modalSearch = "RateCardModalSearch";
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
            retval = '<a class="btn btn-outline-success m-btn m-btn--icon btn-sm m-btn--icon-only m-btn--air fakelink" href="/Projectmanagement/RateCard/update/' + row.id + '"><i class="la la-edit"></i> </a>';
            retval += ' <a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--icon-only m-btn--air" onClick="fnRateCard.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> </a>';
            return retval;
        }
    }, {
        "data": "code",
        "title": "code",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "name",
        "title": "name",
        "sClass": "",
        orderable: true
    }, {
        "data": "total",
        "title": "total",
        "sClass": "",
        orderable: true
    }, {
        "data": "revision",
        "title": "revision",
        "sClass": "",
        orderable: true
    } 
];

fnRateCard = {
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
                        fnRateCard.reloadlist();
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
    fnRateCard.generatelist();
    initlanguage(lang_gn);
});
 

