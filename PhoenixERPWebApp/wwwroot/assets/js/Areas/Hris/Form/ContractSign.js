var ajaxUrl = API + "Hris/CandidateRecruitment";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGetVacancy = API + "Hris/JobVacancy/GetWithDetial";
var tableList = "ApplicantProgressList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var pageheader = "";

fnApplicantProgress = {
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
    initlanguage(lang_hr);
    
    $('#candicate_recruitment_id').setcombobox({
        data: { 'rows': 1000, Search_applicant_progress_id: 4, Search_applicant_move_stage_id: 6 },
        url: ajaxUrlList,
        searchparam: 'Search_title_name',
        labelField: 'first_name',
        valueField: 'job_vacancy_id',
        searchField: 'first_name',
        render: function (data) {
            var datanya = '';
            if (data.odata) {
                var dataraw = '';
                //console.log(data.odata);
                dataraw = data.odata.code + " - " + data.odata.first_name + " " + data.odata.last_name;
                return dataraw;
            }
        }
    });
    $('#upload_sign_contract').setuploadbox({});

    $('#candicate_recruitment_id').on('select2:select', function (e) {
        var id = e.params.data.odata.id;
        var code_data = e.params.data.odata.code;
        var applicant_progress_id = e.params.data.odata.applicant_progress_id;
        var applicant_move_stage_id = e.params.data.odata.applicant_move_stage_id;

        if (id !== null) {
            ajaxGet(ajaxUrlGetVacancy + "/" + e.params.data.id, {}, function (response) {
                response = parseJson(response);
                console.log(response);
                if (response.data) {
                    $('#code').val(code_data);
                    $('#id').val(id);
                    $('#posting_code').val(response.data.code);
                    $('#souce').val(response.data.jobVacancyJobSource.source);
                    $('#vacancy_name').val(response.data.vacancy_name);
                    $('#contractType').val(response.data.contracttype);
                    $('#contractSignDate').val(response.data.contractSignDate);
                    var dataraw = '';
                    if (applicant_progress_id.trim() === "0") {
                        dataraw = "New Applicant";
                    } else if (applicant_progress_id.trim() === "1") {
                        dataraw = "Interview";
                    } else if (applicant_progress_id.trim() === "2") {
                        dataraw = "Job Offered";
                    } else if (applicant_progress_id.trim() === "3") {
                        dataraw = "Approval";
                    } else if (applicant_progress_id.trim() === "4") {
                        dataraw = "Accepted";
                    } else if (applicant_progress_id.trim() === "5") {
                        dataraw = "KIV / Tallent Pool";
                    } else {
                        dataraw = "Rejected";
                    }


                    var dataraw2 = '';
                    if (applicant_move_stage_id.trim() === "1") {
                        dataraw2 = "Screening / New Applicant";
                    } else if (applicant_move_stage_id.trim() === "2") {
                        dataraw2 = "Invite 1st Interview";
                    } else if (applicant_move_stage_id.trim() === "3") {
                        dataraw2 = "1st Interview Accepted";
                    } else if (applicant_move_stage_id.trim() === "4") {
                        dataraw2 = "Job Offering";
                    } else if (applicant_move_stage_id.trim() === "5") {
                        dataraw2 = "Approval";
                    } else if (applicant_move_stage_id.trim() === "6") {
                        dataraw2 = "Accepted";
                    } else {
                        dataraw2 = "Rejected";
                    }

                    $('#status').val(dataraw2);
                } else { DangerNotif("No data candidate found for selected ID."); }
            });

        }
    });
});

function RejectData() {
    var text = "Do you want to Move this Candidate?";
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
            ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: 6, applicant_move_stage_id: 7 }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    if (candidate_recruitment_id === "5" || candidate_recruitment_id === 5) {
                        var data = {
                            refId: $('#id').val(),
                            detailLink: "/hris/ApplicantDetail/ApplicantMoveStage/" + $('#id').val() + "/" + $('#job_vacancy_id').val(),
                            tName: "hr.job_vacancy",
                            menuId: "086cbaa3-41a5-42b2-a830-6075fda0e7dd",
                            IdTemplate: $("#template_approval_id").val()
                        };
                        SuccessNotif("Change Status Sucess");
                        saveTemplateApprovalHris(data, WEB + "/hris/ApplicantDetail/ApplicantMoveStage/" + $('#id').val() + "/" + $('#job_vacancy_id').val());


                    } else {
                        SuccessNotif("Change Status Sucess");
                        window.location.href = WEB + "/hris/ApplicantDetail/ApplicantMoveStage/" + $('#id').val() + "/" + $('#job_vacancy_id').val();

                    }


                } else {
                    DangerNotif(response.message);
                }
            });
        }
    });

}


function SubmitContractSign() {
    swal({
        title: "Confirmation",
        text: "Are you sure to contract this candidate ?",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (e) {
        if (e.value) {
            if ($('#upload_sign_contract').val().trim() !== "") {  
                ajaxPost(API + "Hris/CandidateRecruitment/UpdateCustomWithContract", { id: $('#id').val(), applicant_progress_id: 4, applicant_move_stage_id: 7, remarks: $('textarea#remakrs').val(), filemaster_id: $('#upload_sign_contract').val(), start_date: $('#start_date').val(), end_date: $('#end_date').val(), thp: $('#thp').val(), ptpp: $('#ptpp').val(), contracttype: $('#contractType').val(), contractSignDate: $('#contractSignDate').val() }, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif("Approval Sucess");
                        window.location.href = WEB + "/hris/ContractSign/";
                    } else {
                        DangerNotif(response.message);
                    }
                });
            } else {
                DangerNotif("Please complete form first");
                return false;
            }
        }
    });
}

$(".allownumericwithdecimal").keypress(function (e) {
    if (e.which === 46) {
        if ($(this).val().indexOf('.') !== -1) {
            return false;
        }
    }

    if (e.which !== 8 && e.which !== 0 && e.which !== 46 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
 

