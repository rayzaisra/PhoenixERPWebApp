var ajaxUrl = API + "/Hris/Mpp";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/DeleteWithDetail";

var tableList = "MppList";
var formSearch = "MppFormSearch";
var modalSearch = "MppModalSearch";
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
        "title": "<span data-args='Code'>Code</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            if (row.status === "0" || row.status === null) {
                datastring = '<a class="fakelink" href="/Hris/MppDetail/Index/' + row.id + '">' + data + '</a>';
            }
            else {
                datastring = '<a class="fakelink" href="/Hris/MppApprovedStatus/Index/' + row.id + '">' + data + '</a>';
            }
          
            return datastring;
        }
    }  ,{
        "data": "fiscal_year_name",
        "title": "<span data-args='Fiscal Year'>Fiscal Year</span>",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span data-args='Business Unit ID'>Division</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            datastring = row.businessUnit.unit_name;

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<span data-args='Job Title'>Job Title</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            $.each(row.jobTitle, function (key, value) {
                datastring = datastring + value.job_title_name + '<br>';
            });

            return datastring;
        }
    }, {
        "data": "total_budget",
        "title": "<span data-args='Total Budget'>Total Budget</span>",
        "sClass": "text-right",
        orderable: true,
        "render": function (data, type, row) {
            return formatDecimal(data);
        }
    },
    {
        "data": "status",
        "title": "<span data-args='Status'>Status</span>",
        "sClass": "actioncol",
        orderable: true,
        "render": function (data, type, row) {
            var datastring = '';

            if (data === "1") {
                datastring = "<span class='bluetext'>WAITING FOR APPROVAL</span>";
            } else if (data === "2") {
                datastring = "<span class='bluetext'>WAITING FOR NEXT APPROVAL</span>";
            } else if (data === "3") {
                datastring = "<span class='greentext'>APPROVED</span>";
            } else if (data === "4") {
                datastring = "<span class='redtext'>REJECT</span>";
            }else if (data === "0" || data === null){
                datastring = "<span class='bluetext'>OPEN</span>";
            }

            return datastring;
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
    {//class='translate' 
        "data": null,
        "title": "<span data-args='Approved By'>Approved By</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            var datastring = '';
            $.each(row.approvalReal, function (key, value) {
                if (value.statusApproved === 3 || value.statusApproved === "3") {
                    datastring = datastring + value.approved_by + '<br>';
                }
            });

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<span data-args='Approved Date'>Approved Date</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            var datastring = '';
            $.each(row.approvalReal, function (key, value) {
                if (value.statusApproved === 3 || value.statusApproved === "3") {
                    datastring = datastring + moment(value.created_on).format('DD-MMM-YYYY HH:mm') + '<br>';
                }
            });

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<span data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var retval = '';
            if (row.status === "0" || row.status === null) {
                retval = '';
                retval += '<span class="dropdown">';
                retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
                retval += '<i class="la la-ellipsis-h"></i>';
                retval += '</a>';
                retval += '<div class="dropdown-menu dropdown-menu-right">';
                retval += '<a class="fakelink" href="/Hris/MppDetail/Index/' + row.id + '"><i class="la la-edit"></i> Details</a>';
                retval += '<a class="dropdown-item" onClick="fnMpp.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
                retval += '</div>';
                retval += '</span>';
            } else {
                retval = '';
                retval += '<span class="dropdown">';
                retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
                retval += '<i class="la la-ellipsis-h"></i>';
                retval += '</a>';
                retval += '<div class="dropdown-menu dropdown-menu-right">';
                retval += '<a class="fakelink" href="/Hris/MppApprovedStatus/Index/' + row.id + '"><i class="la la-edit"></i> Details</a>';
                //retval += '<a class="dropdown-item" onClick="fnMpp.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
                retval += '</div>';
                retval += '</span>';
            }
            
            return retval;

        }
    }   
];

fnMpp = {
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
                        fnMpp.reloadlist();
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
    fnMpp.generatelist();
    initlanguage(lang_hr);
});

function formatDecimal(total) {
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-" : ' ') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}
 

