var ajaxUrl = API + "/Hris/CandidateRecruitmentEmploymentHistory";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCentral = API + "/Hris/JobVacancy/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetEducationInfo = API + "/Hris/CandidateRecruitmentEmploymentHistory/Get";
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

    var xform = "EmploymentHistoryFormUpdate";
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            window.location.href = WEB + "/hris/ApplicantDetail/EmploymentHistory/" + candidate_recruitment_id + "/" + job_vacancy_id;
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

                    $("#recruitment_status").val(dataraw);
                    $("#applicant_code").val(response2.data.code);
                } else {
                    DangerNotif("No data candidate found for selected ID.");
                }
               

            });

            ajaxGet(ajaxUrlGetEducationInfo + "/" + xid, {}, function (response) {
                response2 = parseJson(response);
                //console.log(response2);
                $("#company_address").val(response2.data.company_address);
                $("#company_name").val(response2.data.company_name);
                $("#company_phone").val(response2.data.company_phone);
                $("#position").val(response2.data.position);
                $("#year_from").val(response2.data.year_from);
                $("#year_to").val(response2.data.year_to);
                $("#main_responsibilities").val(response2.data.main_responsibilities);
                $("#achievements").val(response2.data.achievements);
                $("#candidate_recruitment_id").val(response2.data.candidate_recruitment_id);


            });
        } else {
            DangerNotif("No data job vacancy found for selected ID.");
        }
        


    });
});

function SubmitEditDataEducation() {
    $('#EmploymentHistoryFormUpdate').submit();
}


 

