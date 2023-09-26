var ajaxUrl = API + "/Hris/CandidateRecruitmentInterview";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCentral = API + "/Hris/JobVacancy/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlListLocation = API + "Hris/MasterLocation/List";
var ajaxUrlListLocationGet = API + "Hris/MasterLocation/Get";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";
var tableList = "InterviewList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";


var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !=="__RequestVerificationToken") {
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
    } , {
        "data": null,
        "title": "Date Interview",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return moment(row.date_interview).format("DD MMMM YYYY");
        }
    } 
    , {
        "data": null,
        "title": "Time",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var tfh, tfm, tth, ttm;
            if (parseInt(row.time_from_hour) < 10) {
                tfh = "0" + row.time_from_hour;
            } else {
                tfh = row.time_from_hour;
            }

            if (parseInt(row.time_from_minutes) < 10) {
                tfm = "0" + row.time_from_minutes;
            } else {
                tfm = row.time_from_minutes;
            }

            if (parseInt(row.time_to_hour) < 10) {
                tth = "0" + row.time_to_hour;
            } else {
                tth = row.time_to_hour;
            }

            if (parseInt(row.time_to_minutes) < 10) {
                ttm = "0" + row.time_to_minutes;
            } else {
                ttm = row.time_to_minutes;
            }

            return tfh + ":" + tfm + " - " + tth + ":" + ttm;
        }
    }, {
        "data": null,
        "title": "Subject",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return row.subject;
        }
    },
    {
        "data": null,
        "title": "<center><span class='translate'  data-args='app-action'>Actions</span></center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            //retval = '';
            //retval += '<center><a class="btn btn-secondary" href="/Hris/Applicant/TrackingNewApplicantUpdate/' + row.id + '/' + $('#id').val() + '"><i class="la la-edit"></i> Details</a>&nbsp';
            //retval += '<a class="btn btn-secondary" onClick="fnApplicantProgress.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a></center>';
            //return retval;
            retval = '';

            retval += '<center><a class="btn btn-secondary" href="/Hris/ApplicantDetail/InterviewUpdate/' + row.id + '/' + $('#job_vacancy_id').val() + '/' + $('#id').val() + '"><i class="la la-edit"></i> Details</a>&nbsp';
            retval += '<a class="btn btn-secondary" onClick="fnApplicantProgress.delete(\'' + row.id + '\')" href="javascript:void(0)"></i> Delete</a></center>';


            return retval;
        }
    }   
];

fnApplicantProgress = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, { page: 1, rows: 10, sortBy: 'id', sortDir: 'asc', Search_candidate_recruitment_id: $('#id').val() }, columnsIndex, 'id', 'asc');
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
                ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
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
    var xform = "InterviewFormCreate";
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            window.location.href = WEB + "/hris/ApplicantDetail/Interview/" + $('#id').val() + "/" + $('#job_vacancy_id').val();
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);

    var xid = $('#id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();
    //$('#cv').setuploadbox({});
    //$('#photo').setuploadbox({});
    //$('#portfolio').setuploadbox({});
   

    ajaxGet(ajaxUrlGetCentral + "/" + job_vacancy_id, {}, function (resp) {
        response = parseJson(resp);
        //console.log(response);
        if (response.data) {
            ajaxGet(ajaxUrlGetCandidateReq + "/" + xid, {}, function (resp2) {
                response2 = parseJson(resp2);
                //console.log(response2);

                var dataraw = '';
                if (response2.data) {
                    $("#vacancy_name").val(response.data.vacancy_name);
                    $("#posting_code").val(response.data.code);

                    var dataraw = '';
                    if (response.data.posting_status === "0") {
                        dataraw = "DRAFT";
                    } else if (response.data.posting_status === "1") {
                        dataraw = "APPROVED";
                    } else if (response.data.posting_status === "2") {
                        dataraw = "REJECT";
                    } else if (response.data.posting_status === "3") {
                        dataraw = "UNPOSTING";
                    } else if (response.data.posting_status === "4") {
                        dataraw = "POSTED";
                    } else {
                        dataraw = "CLOSED";
                    }
                    $("#posting_status").val(dataraw);

                    if (response2.data.applicant_progress_id.trim() === "0") {
                        dataraw = "New Applicant";
                    } else if (response2.data.applicant_progress_id.trim() === "1") {
                        dataraw = "Interview";
                    } else if (response2.data.applicant_progress_id.trim() === "2") {
                        dataraw = "Job Offered";
                    } else if (response2.data.applicant_progress_id.trim() === "3") {
                        dataraw = "Approval";
                    } else if (response2.data.applicant_progress_id.trim() === "4") {
                        dataraw = "Accepted";
                    } else if (response2.data.applicant_progress_id.trim() === "5") {
                        dataraw = "KIV / Tallent Pool";
                    } else if (response2.data.applicant_move_stage_id.trim() === "7") {
                        dataraw2 = "Talent Pool";
                    } else {
                        dataraw = "Rejected";
                    }

                    var dataraw2 = '';
                    if (response2.data.applicant_move_stage_id === null) {
                        dataraw2 = "Screening / New Applicant";
                    } else {
                        if (response2.data.applicant_move_stage_id.trim() === "1") {
                            dataraw2 = "Screening / New Applicant";
                        } else if (response2.data.applicant_move_stage_id.trim() === "2") {
                            dataraw2 = "Invite 1st Interview";
                        } else if (response2.data.applicant_move_stage_id.trim() === "3") {
                            dataraw2 = "1st Interview Accepted";
                        } else if (response2.data.applicant_move_stage_id.trim() === "4") {
                            dataraw2 = "Job Offering";
                        } else if (response2.data.applicant_move_stage_id.trim() === "5") {
                            dataraw2 = "Waiting Approval";
                        } else if (response2.data.applicant_move_stage_id.trim() === "6") {
                            dataraw2 = "Accepted";
                        } else if (response2.data.applicant_move_stage_id.trim() === "7") {
                            dataraw2 = "Talent Pool";
                        } else {
                            dataraw2 = "Rejected";
                        }
                    }

                    $("#recruitment_status").val(dataraw2);
                    $("#applicant_progress_id").val(response2.data.applicant_progress_id);
                    $("#applicant_move_stage_id").val(response2.data.applicant_move_stage_id);
                    $("#applicant_code").val(response2.data.code);
                    $("#applicant_name").val(response2.data.first_name + " " + response2.data.last_name);
                    if (response2.data.cv_file === "") {
                        $('#divcvplace .input-group').remove();
                        $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                    } else {
                        $('#divcvplace .input-group').remove();
                        $('#cv').val(response2.data.cv_file);
                        $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                    }

                    if (response2.data.photo_file === "") {
                        $('#divphotoplace .input-group').remove();
                        $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
                    } else {
                        $('#divphotoplace .input-group').remove();
                        $('#photo').val(response2.data.photo_file);
                        $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
                    }

                    if (response2.data.portofolio_file === "") {
                        $('#divportfolioplace .input-group').remove();
                        $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                    } else {
                        $('#divportfolioplace .input-group').remove();
                        $('#portfolio').val(response2.data.portofolio_file);
                        $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                    }
                    if ($("#action_plan").val() === "1st Interview") {
                        $("#subject").val("Invite 1st Interview");
                    } else {
                        $("#subject").val("Invite Nextstage Interview");
                    }

                    $('#master_location_id').setcombobox({
                        data: { 'rows': 100 },
                        url: ajaxUrlListLocation,
                        maxItems: 0,
                        searchparam: 'location_name',
                        labelField: 'location_name',
                        valueField: 'id'
                    });
                } else { DangerNotif("No data candidate found for selected ID."); }
                
            });

        } else { DangerNotif("No data job vacancy found for selected ID."); }
       
    });
});

function changeinterview(subject) {
    if (subject === "1st Interview") {
        $("#subject").val("Invite 1st Interview");
    } else {
        $("#subject").val("Invite Nextstage Interview");
    }
}
function submitEducationDate() {
    $('#year').val($('#year_from').val() + " - " + $('#year_to').val());
    
    
    ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: 1, applicant_move_stage_id: 2 }, function (response) {
        response = parseJson(response);
        if (response.success) {
            $('#InterviewFormCreate').submit();
        } else {
            DangerNotif(response.message);
        }
    });
  
}

function ApprovedData() {
    var text = "Do you want to Approved this Candidate?";
    swal({
        title: "Confirmation",
        text: text,
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (e) {
        if (e.value) {
            ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: 4,applicant_move_stage_id: 6 }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Approval Sucess");
                    window.location.href = WEB + "/hris/Applicant/TrackingNewApplicant/" + $('#job_vacancy_id').val();
                } else {
                    DangerNotif(response.message);
                }
            });
        }
    });

}


function showModalNew() {

    showModalAjaxGet("test", WEB_HRIS + '/AplicantDetail/InterviewCreate/' + id, {}, function () {
     
    
        $('.dtpicker').datetimepicker({ autoclose: true });
        var optionsForm = GetOptionsForm(function () {
            var xret = $("#" + xtaskform).valid();

            return xret;
        }, function (response, statusText, xhr, $form) {

            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_updatesuccess);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xtaskform, optionsForm);
        setFormAction(xtaskform, ajaxUrlassigntask);


    }, 'taskmakerbox');
}



