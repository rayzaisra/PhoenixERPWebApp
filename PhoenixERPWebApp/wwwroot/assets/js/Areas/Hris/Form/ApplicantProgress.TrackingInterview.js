var ajaxUrl = API + "/Hris/CandidateRecruitment";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = API + "Hris/CandidateRecruitment/DeleteDetil";
var ajaxUrlGet = API + "/Hris/JobVacancy/GetWithDetial";
var tableList = "ApplicantProgressList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var ajaxUrlListJobTitle = API + "Hris/JobTitle/List/";
var pageheader = "";
var ajaxUrlEmployeeList = API + "/Hris/EmployeeBasicInfo/List";


var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = { Search_status: 1 };
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
    }, {
        "data": null,
        "title": "Code",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var datastring = '';

            datastring = '<a  href="/Hris/ApplicantDetail/PersonalInfo/' + row.id + '/' + $('#id').val() +'">' + row.code + '</a>';

            return datastring;
        }
    }, {
        "data": null,
        "title": "Name",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            dataraw = row.first_name + " " + row.last_name;
            return dataraw;
        }
    }, {
        "data": null,
        "title": "Source",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';

            return row.source;
        }
    }, {
        "data": null,
        "title": "<center>Education</center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            dataraw = row.last_education_name;
            return "<center>" + dataraw + "</center>";
        }
    }, {
        "data": null,
        "title": "<center>Year Experience</center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            dataraw = row.year_experience;
            return "<center>" + dataraw + "</center>";
        }
    },
    {
        "data": null,
        "title": "<center>Applicant Progress</center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            if (row.applicant_move_stage_id !== null && row.applicant_progress_id !== null) {
                if (row.applicant_progress_id.trim() === "0" && row.applicant_move_stage_id.trim() === "1") {
                    dataraw = "New Applicant";
                } else if (row.applicant_progress_id.trim() === "1" && row.applicant_move_stage_id.trim() === "2") {
                    dataraw = "Invite 1st Interview";
                } else if (row.applicant_progress_id.trim() === "1" && row.applicant_move_stage_id.trim() === "3") {
                    dataraw = "1st Interview Accepted";
                } else if (row.applicant_progress_id.trim() === "2" && row.applicant_move_stage_id.trim() === "4") {
                    dataraw = "Job Offering";
                } else if (row.applicant_progress_id.trim() === "3" && row.applicant_move_stage_id.trim() === "5") {
                    dataraw = "Waiting Approval";
                } else if (row.applicant_progress_id.trim() === "4" && row.applicant_move_stage_id.trim() === "6") {
                    dataraw = "Accepted";
                } else if (row.applicant_progress_id.trim() === "5" && row.applicant_move_stage_id.trim() === "7") {
                    dataraw = "Talent Pool";
                } else {
                    dataraw = "Rejected";
                }}
            
            //dataraw = row.applicant_progress_id;
            return "<center>" + dataraw + "</center>";
        }
    },
    {
        "data": null,
        "title": "<center><span class='translate'  data-args='app-action'>Actions</span></center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<center><a class="btn btn-secondary" href="/Hris/Applicant/TrackingNewApplicantUpdate/' + row.id + '/' + $('#id').val() + '"><i class="la la-edit"></i> Details</a>&nbsp';
            retval += '<a class="btn btn-secondary" onClick="fnApplicantProgress.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a></center>';
            return retval;

        }
    }
];

fnApplicantProgress = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, { Search_applicant_progress_id: 1, Search_job_vacancy_id: $('#id').val() }, columnsIndex, 'id', 'asc');
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
                ajaxPost(ajaxUrlDelete, { id: id, job_vacancy_id: $('#id').val() }, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnApplicantProgress.reloadlist();
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
    fnApplicantProgress.generatelist();
    initlanguage(lang_hr);

    var xid = $('#id').val();
    var xidsec = $('#idsec').val();
    if (xidsec !== "") {
        xid = xidsec;
    }
    if (xid != '') {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
          
            if (response.data) {
                $('#code').val(response.data.code);
                $('#request_code').val(response.data.request_code);
                var created_on = moment(response.data.created_on).format("DD MMMM YYYY");
                $('#post_date').val(created_on);
                $('#vacancy_name').val(response.data.vacancy_name);


                $('#vacancy_name').append("<option value=" + $('#vacancy_name').val() + ">" + response.data.vacancy_name + "</option>");
                $('#vacancy_name').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListJobTitle,
                    searchparam: 'Search_title_name',
                    labelField: 'title_name',
                    valueField: 'id',
                    searchField: 'title_name'
                });

                $('#employee_basic_info_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlEmployeeList,
                    maxItems: 0,
                    searchparam: 'Search_name',
                    labelField: 'name_employee',
                    valueField: 'id'
                });
            } else { DangerNotif("No data job vacancy found for selected ID."); }

        });
    }
    
    
});




