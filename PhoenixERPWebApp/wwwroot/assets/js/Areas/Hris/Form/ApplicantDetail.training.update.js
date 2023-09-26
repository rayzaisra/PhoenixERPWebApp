var ajaxUrl = API + "/Hris/CandidateRecruitmentTraining";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCentral = API + "/Hris/JobVacancy/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetTrainingInfo = API + "/Hris/CandidateRecruitmentTraining/Get";
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
}


$(function () {

    initlanguage(lang_hr);

    var xid = $('#id').val();
    var candidate_recruitment_id = $('#candidate_recruitment_id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();

    var xform = "TrainingFormUpdate";
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            window.location.href = WEB + "/hris/ApplicantDetail/Training/" + candidate_recruitment_id + "/" + job_vacancy_id;
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlUpdate);

    ajaxGet(ajaxUrlGetCentral + "/" + job_vacancy_id, {}, function (response) {
        response = parseJson(response);
        //console.log(response);
        if (response.data) {
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
            ajaxGet(ajaxUrlGetCandidateReq + "/" + candidate_recruitment_id, {}, function (response) {
                response2 = parseJson(response);
                //console.log(response2);

                var dataraw = '';
                if (response2.data) {
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
                    } else {
                        dataraw2 = "Rejected";
                    }

                    $("#recruitment_status").val(dataraw2);
                    $("#applicant_code").val(response2.data.code);
                } else {
                    DangerNotif("No data candidate found for selected ID.");
                }

            });
            ajaxGet(ajaxUrlGetTrainingInfo + "/" + xid, {}, function (response) {
                response2 = parseJson(response);
                //console.log(response2);
                if (response2.data) {
                    $("#training_name").val(response2.data.training_name);
                    $("#score").val(response2.data.score);
                    $("#status").val(response2.data.status);
                    $("#candidate_recruitment_id").val(response2.data.candidate_recruitment_id);
                    $("#id").val(response2.data.id);
                } else {
                    DangerNotif("No data candidate training found for selected ID.");
                }
            });

        } else {
            DangerNotif("No data job vacancy found for selected ID.");
        }

    });
});

function SubmitEditDataEducation() {
    $('#TrainingFormUpdate').submit();
}


 

