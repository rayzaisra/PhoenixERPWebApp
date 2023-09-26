var ajaxUrl = API + "/Hris/CandidateRecruitmentComment";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCentral = API + "/Hris/JobVacancy/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetEducationInfo = API + "/Hris/CandidateRecruitmentEducationInfo/Get";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";
var tableList = "EducationInfoList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var ajaxUrlListJobTitle = API + "Hris/JobTitle/List/";
var pageheader = "";
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


$(function () {

    initlanguage(lang_hr);

    var xid = $('#id').val();
    var candidate_recruitment_id = $('#candidate_recruitment_id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();

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
                    $("#applicant_code").val(response2.data.code);} else { DangerNotif("No data candidate found for selected ID.");}
                

            });
            LoadComment(xid);
            
        } else { DangerNotif("No data job vacancy found for selected ID."); }
      


    });
});

function SaveComment() {
    
    ajaxPost(ajaxUrlCreate, { comment_message: $('#comment_message').val(), candidate_recruitment_id: $('#id').val() }, function (response) {
        response = parseJson(response);
        if (response.success) {
            $('comment_message').val('');
            SuccessNotif('Thanks for your comment. Data saved successfully.');
            LoadComment($('#id').val());
            
        } else {
            DangerNotif(response.message);
        }
    });
}

function LoadComment(xid) {
    ajaxGet(ajaxUrlList, { search_candidate_id: xid }, function (response) {
        response3 = parseJson(response);
        //console.log(response2);
        var html = '';
        if (response3.rows) {
            $('#content_comment').empty();
            if (response3.rows.length > 0) {
                for (var i = 0; i < response3.rows.length; i++) {
                    html = '';
                    html = html + '<div class="row">'
                        + '<div class="col-lg-6">'
                        + '<div class="form-group">'
                        + '<label><b><span class="translate" data-args="' + response3.rows[i].name_employee + '">' + response3.rows[i].name_employee + '</span></b> ~ <span>' + moment(response3.rows[i].created_on).format("DD MMMM YYYY hh:mm")+'</span></label>'
                        + '</div>'
                        + '<div class="form-group">'
                        + '<label>' + response3.rows[i].comment_message + '</label>'
                        + '</div>'
                        + ' <hr />'
                        + '</div>'
                        + '</div>';
                    $('#content_comment').append(html);
                }
            } else {
                $('#content_comment').empty();
                html = '';
                html = html + '<div class="row">'
                    + '<div class="col-lg-6">'
                    + '<div class="form-group">'
                    + '<label><b><span class="translate" data-args="No Comments Yet">No Comments Yet</span>  </b></label>'
                    + '</div>'
                    + '<div class="form-group">'
                    + '<label>Be the first to share what you think</label>'
                    + '</div>'
                    + ' <hr />'
                    + '</div>'
                    + '</div>';
                $('#content_comment').append(html);
            }

        } else {
            $('#content_comment').empty();
            html = '';
            html = html + '<div class="row">'
                + '<div class="col-lg-6">'
                + '<div class="form-group">'
                + '<label><b><span class="translate" data-args="No Comments Yet">No Comments Yet</span>  </b></label>'
                + '</div>'
                + '<div class="form-group">'
                + '<label>Be the first to share what you think</label>'
                + '</div>'
                + ' <hr />'
                + '</div>'
                + '</div>';
            $('#content_comment').append(html);
            DangerNotif("No data candidate comment found for selected ID.");
        }
    });
}


 

