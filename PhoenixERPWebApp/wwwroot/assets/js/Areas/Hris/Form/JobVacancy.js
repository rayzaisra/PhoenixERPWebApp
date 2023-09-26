var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/DeleteDetil";

var tableList = "JobVacancyList";
var formSearch = "JobVacancyFormSearch";
var modalSearch = "JobVacancyModalSearch";
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
    }, {
        "data": null,
        "title": "Posting Code",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var datasting = '';
            if (row.posting_status === "0") {
                datasting = '<a href="/Hris/JobVacancy/Approval/' + row.id + '">' + "<center>" + row.code + "</center>" + '</a>';
            } else {
                datasting = '<a href="/Hris/JobVacancy/Update/' + row.id + '">' + "<center>" + row.code + "</center>" + '</a>';
            }
            return datasting;
        }
    } ,{
        "data": "request_code",
        "title": "REQ ID",
        "sClass": "",
        orderable: true
    }  , {
        "data": null,
        "title": "Vacancy Name",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
          
            dataraw = row.vacancyTitleName.job_title_name + " (" + row.vacancyGradeName.job_grade_name + ")";


            return dataraw;
        }
    }, {
        "data": null,
        "title": "Division",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            if (row.businessUnit !== null) 
                dataraw = row.businessUnit.unit_name;
            //console.log("Bussiness = " + row.businessUnit.unit_name);
            //console.log(JSON.stringify(row));
           // dataraw = row.businessUnit.parent.unit_name;
            return dataraw;
        }
    }, {
        "data": null,
        "title": "Job Title",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';

            dataraw = row.jobTitleRequester.job_title_name;


            return dataraw;
        }
    },{
        "data": "created_on",
        "title": "Created Date",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {

            return moment(row.created_on).format("DD MMMM YYYY HH:mm");
        }
    }, {
        "data": null,
        "title": "Status",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';

            if (row.posting_status === "0") {
                dataraw = "DRAFT";
            } else if (row.posting_status === "1") {
                dataraw = "APPROVED";
            } else if (row.posting_status === "2") {
                dataraw = "REJECT";
            } else if (row.posting_status === "3") {
                dataraw = "UNPOSTING";
            } else if (row.posting_status === "4") {
                dataraw = "POSTED";
            } else {
                dataraw = "CLOSED";
            }
            return dataraw;
        }
    }, {
        "data": null,
        "title": "Applicant",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = 0;

            $.each(row.applicant, function (index, value) {
                dataraw = dataraw + 1;
            });
            var datastring = '';
            datastring = '<a href="/Hris/Applicant/TrackingNewApplicant/' + row.id + '">' + "<center>" + dataraw + "</center>" + '</a>';
            return datastring;
        }
    } ,
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/JobVacancy/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            //retval += '<a class="dropdown-item" onClick="fnJobInterview.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnJobInterview = {
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
                        fnJobInterview.reloadlist();
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
    fnJobInterview.generatelist();
    initlanguage(lang_hr);
});
 

