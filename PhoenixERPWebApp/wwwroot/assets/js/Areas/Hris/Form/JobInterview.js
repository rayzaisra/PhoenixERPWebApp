var ajaxUrl = API + "/Hris/JobInterview";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "JobInterviewList";
var formSearch = "JobInterviewFormSearch";
var modalSearch = "JobInterviewModalSearch";
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
        "data": "from_email",
        "title": "from_email",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "interviewer",
        "title": "interviewer",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "interview_date",
        "title": "interview_date",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "interview_time",
        "title": "interview_time",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_interview_description",
        "title": "job_interview_description",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_posting_requisition_id",
        "title": "job_posting_requisition_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "latitude",
        "title": "latitude",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "longitude",
        "title": "longitude",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "notes",
        "title": "notes",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "place_of_interview",
        "title": "place_of_interview",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "to_email",
        "title": "to_email",
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/JobInterview/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnJobInterview.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnJobInterview = {
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
 

