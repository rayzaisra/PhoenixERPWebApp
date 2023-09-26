//var API_SUFFIX = API.substr(API.length - 1, API.length);
//if (API_SUFFIX === "/")
//	API = API.substr(0, API.length - 1);

var ajaxUrl = API + "Hris/HiringRequest";
var ajaxUrlList = ajaxUrl + "/ListAllWithDetial";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/DeleteWithDetail";

var tableList = "HiringRequestList";
var formSearch = "HiringRequestFormSearch";
var modalSearch = "HiringRequestModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
};

var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    },
    {
        "data": "code",
        "title": "<span data-args='code'>Code</span>",
        "sClass": "actioncol",
        orderable: true,
        "render": function (data, type, row) {
            var datastring = '';

            if (row.statusApproved === null) {
                
                datastring = '<a class="fakelink" href="/Hris/HiringRequest/Create/' + row.id + '">' + data + '</a>';
            } else {
                datastring = '<a class="fakelink" href="/Hris/HiringRequest/Approval/' + row.id + '">' + data + '</a>';
            }
            //datastring = '<a class="fakelink" href="/Hris/HiringRequest/Approval/' + row.id + '">' + data + '</a>';
            return datastring;
        }
    } ,  {
        "data": null,
        "title": "Disivion",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            if (row.division != null)
                return row.division.unit_name;
        }
    }, {
        "data": null,
        "title": "Job Title",
        "sClass": "",
        orderable: false,
        "render": function (data, type, row, meta) {
            return row.jobtitile.title_name;
        }
    }, {
        "data": "employment_status",
        "title": "Employee Status",
        "sClass": "",
        orderable: false,
        "render": function (data, type, row, meta) {
            return data;
        }
    }, {
        "data": "hiring_request_type_id",
        "title": "Category Req",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return data;
        }
    },
    {
        "data": "created_on",
        "title": "Created On",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return moment(data).format('DD-MMM-YYYY HH:mm');
        }
    },
    {
        "data": "statusApproved",
        "title": "Approval Status",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            var datastring = '';
            if (row.status !== 3) {
                if (data === "1") {
                    datastring = "<span class='bluetext'>WAITING APPROVAL</span>";
                } else if (data === "2") {
                    datastring = "<span class='bluetext'>WAITING NEXT FOR APPROVAL</span>";
                } else if (data === "3") {
                    datastring = "<span class='greentext'>APPROVED</span>";
                } else if (data === "4") {
                    datastring = "<span class='redtext'>REJECT</span>";
                } else {
                    datastring = "<span class='bluetext'>OPEN</span>";
                }
            } else {
                datastring = "<span>CLOSED</span>";
            }

            return datastring;
        }
    }, {
        "data": null,
        "title": "Approval By",
        "sClass": "",
        orderable: false,
        "render": function (data, type, row, meta) {

            var datastring = '';
            $.each(row.approval, function (key, value) {
                if (value.statusApproved === 3 || value.statusApproved === "3") {
                    datastring = datastring + value.created_by + '<br>';
                }
            });
         

            return datastring;
        }
    }, {
        "data": null,
        "title": "Approval Date",
        "sClass": "",
        orderable: false,
        "render": function (data, type, row, meta) {
            var datastring = '';
            $.each(row.approval, function (key, value) {
                if (value.statusApproved === 3 || value.statusApproved === "3") {
                    datastring = datastring + moment(value.created_on).format('DD-MMM-YYYY HH:mm') + '<br>';
                }
            });
     
            return datastring;
        }
    }    ,
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            if (row.status === "0") {
                retval += '<span class="dropdown">';
                retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
                retval += '<i class="la la-ellipsis-h"></i>';
                retval += '</a>';
                retval += '<div class="dropdown-menu dropdown-menu-right">';              
                retval += '<a class="dropdown-item" onClick="fnHiringRequest.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
                retval += '</div>';
                retval += '</span>';
            } else {
                retval = '';
            }
            
            return retval;

        }
    }   
];

fnHiringRequest = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex);
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        $("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
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
                ajaxGet(ajaxUrlDelete + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnHiringRequest.reloadlist();
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
    fnHiringRequest.generatelist();
    initlanguage(lang_hr);
   
});
 

