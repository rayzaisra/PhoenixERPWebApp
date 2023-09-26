var webUrl = WEB + "/Hris/JobVacancy";
var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlUpdate = API + "/Hris/CandidateRecruitment/UpdateCustom";
var xform = "JobVacancyFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";
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
    var xid = $('#id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();
    if (xid !== "" && job_vacancy_id !== "") {
    $('#last_education').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlEducationLevel,
        searchparam: 'Search_education_name',
        labelField: 'education_name',
        valueField: 'id',
        searchField: 'education_name'
    });

    ajaxPost(API + "/Hris/CandidateRecruitment/CheckApprovalCustom", { id: $('#id').val() }, function (response) {
        response = parseJson(response);
        console.log(response);
    });

    initlanguage(lang_hr);

    //$('#cv').setuploadbox({});
    //$('#photo').setuploadbox({});
    //$('#portfolio').setuploadbox({});
 

        ajaxGet(ajaxUrlGet + "/" + job_vacancy_id, {}, function (resp) {
            response = parseJson(resp);
            //console.log(response);
            if (response.data) {
                ajaxGet(ajaxUrlGetCandidateReq + "/" + xid, {}, function (resp2) {
                    response2 = parseJson(resp2);
                    console.log(response2);

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
                        }
                        else {
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
                        $("#applicant_code").val(response2.data.code);

                        $('#first_name').val(response2.data.first_name);
                        $('#last_name').val(response2.data.last_name);
                        $('#nick_name').val(response2.data.nick_name);
                        $('#code').val(response2.data.code);
                        $('#applicant_progress_id').val(response2.data.applicant_progress_id);
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
                        var dateBirth = moment(response.data.date_birth).format("YYYY-MM-DD");
                        $('#date_birth').val(dateBirth);

                        if (response2.data.last_education) {
                            ajaxGet(ajaxUrlEducationLevel, { Search_education_id: response2.data.last_education }, function (resp) {
                                var educatioName = '';
                                $.each(resp.rows, function (index, value) {
                                    if (value.id == response2.data.last_education) {
                                        educatioName = value.education_name;
                                    }
                                });

                                $('#last_education').append('<option value="' + response2.data.last_education + '" selected>' + educatioName + '</option>');


                                //resp = parseJson(resp);
                                //$('#last_education').append('<option value="' + response2.data.last_education + '" selected>' + resp.rows[0].education_name + '</option>');
                            });

                        }

                        $('#year_experience').val(response2.data.year_experience);
                        $('#phone_number').val(response2.data.phone_number);
                        $('#source').val(response2.data.source);
                        var $radioscigarete = $('input:radio[name=out_of_town_willing]');
                        var cekcigarete = response2.data.out_of_town_willing;
                        if (cekcigarete === true) {
                            $radioscigarete.filter('[value=true]').prop('checked', true);
                        } else {
                            $radioscigarete.filter('[value=false]').prop('checked', true);
                        }
                        var $radiosalcohol = $('input:radio[name=alcohol_willing]');
                        var cekalcohol = response2.data.alcohol_willing;
                        if (cekalcohol === true) {
                            $radiosalcohol.filter('[value=true]').prop('checked', true);
                        } else {
                            $radiosalcohol.filter('[value=false]').prop('checked', true);
                        }
                    } else { DangerNotif("No data candidate found for selected ID."); }

                });
            } else { DangerNotif("No data job vacancy found for selected ID."); }


        });
    }
    
});

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
            ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: 4, applicant_move_stage_id:6}, function (response) {
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


 

