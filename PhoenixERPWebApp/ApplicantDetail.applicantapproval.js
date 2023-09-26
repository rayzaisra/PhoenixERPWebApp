var webUrl = WEB + "/Hris/JobVacancy";
var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlUpdate = API + "/Hris/CandidateRecruitment/UpdateCustom";
var xform = "JobVacancyFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";

var tableList = "ApplicantProgressList";
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


fnApplicantProgress = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, { Search_applicant_progress_id: 0, Search_job_vacancy_id:$('#id').val() }, columnsIndex, 'id', 'asc');
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
                ajaxPost(ajaxUrlDelete, { id: id, job_vacancy_id:$('#id').val()}, function (response) {
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

    initlanguage(lang_hr);
    $("#reject_reason").prop('disabled', true);
    var xid = $('#id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();
    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});

    ajaxGet(ajaxUrlGet + "/" + job_vacancy_id, {}, function (response) {
        response = parseJson(response);
        //console.log(response);
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
        ajaxGet(ajaxUrlGetCandidateReq + "/" + xid, {}, function (response) {
            response2 = parseJson(response);
            //console.log(response2);

            var dataraw = '';
           

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
            } else if (response2.data.applicant_move_stage_id.trim() === "7") {
                dataraw2 = "Talent Pool";
            } else {
                dataraw2 = "Rejected";
            }

            $("#recruitment_status").val(dataraw2);
            $("#applicant_code").val(response2.data.code);

            $('#first_name').val(response2.data.first_name);
            $('#last_name').val(response2.data.last_name);
            $('#nick_name').val(response2.data.nick_name);
            $('#code').val(response2.data.code);
            $('#applicant_progress_id').val(response2.data.applicant_progress_id);
            var $radiosgender = $('input:radio[name=gender]');
            var cekgender = response2.data.gender;
            if (cekgender === "male") {
                $radiosgender.filter('[value=male]').prop('checked', true);
            } else {
                $radiosgender.filter('[value=female]').prop('checked', true);
            }
            $('#birth_place').val(response2.data.birth_place);
            $('#address').val(response2.data.address);
            $('#email').val(response2.data.email);
            $('#date_birth').val(response2.data.date_birth);
            $('#education_level_id').val(response2.data.education_level_id);
            $('#year_experience').val(response2.data.year_experience);
            $('#phone_number').val(response2.data.phone_number);
            $('#source').val(response2.data.source);
            var $radioscigarete = $('input:radio[name=cigaret_willing]');
            var cekcigarete = response2.data.cigaret_willing;
            if (cekcigarete === true) {
                $radioscigarete.filter('[value=true]').prop('checked', true);
            } else {
                $radiosgender.filter('[value=false]').prop('checked', true);
            }
            var $radiosalcohol = $('input:radio[name=alcohol_willing]');
            var cekalcohol = response2.data.alcohol_willing;
            if (cekalcohol === true) {
                $radiosalcohol.filter('[value=true]').prop('checked', true);
            } else {
                $radiosalcohol.filter('[value=false]').prop('checked', true);
            }
        });


    });
   
});

function CheckRadioButton() {
    if ($("#reject_type_kiv").is(":checked")) {
        $("#reject_reason").val('');
        $("#reject_reason").prop('disabled', true);
        $('#candidate_recruitment_id').val("7");
        $('#applicant_progress_id').val("5");
    }
     if ($("#reject_type_reject").is(":checked")) {
         $("#reject_reason").prop('disabled', false);
         $('#candidate_recruitment_id').val("7");
         $('#applicant_progress_id').val("6");
    }

}

function RejectData() {
    if ($('#candidate_recruitment_id').val() === "" || $('#candidate_recruitment_id').val() === null) {
        alert("Please Choose Reject Type");
        return false;    
    } else {
       
        if ($("#reject_type_reject").is(":checked")) {

            if ($('#reject_reason').val() === "" || $('#reject_reason').val() === null) {
                alert("Please Enter Reject Reason");
                return false;
            }

        }

        var text = "Do you want to Process this Candidate?";
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
                ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: $('#applicant_progress_id').val(), applicant_move_stage_id: $('#candidate_recruitment_id').val() }, function (response) {
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
    

}


 

