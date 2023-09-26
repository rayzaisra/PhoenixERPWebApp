var webUrl = WEB + "/Hris/JobVacancy";
var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlUpdate = API + "/Hris/CandidateRecruitment/UpdateCustom";
var xform = "JobVacancyFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List"; 
var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";
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
};


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

    var xid = $('#id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();
    //$('#cv').setuploadbox({});
    //$('#photo').setuploadbox({});
    //$('#portfolio').setuploadbox({});
    if (xid !== "" && job_vacancy_id !== "") {
        ajaxGet(ajaxUrlGet + "/" + job_vacancy_id, {}, function (resp) {
            response = parseJson(resp);

            if (response.data) {
                
                ajaxGet(ajaxUrlGetCandidateReq + "/" + xid, {}, function (resp2) {
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


                    response2 = parseJson(resp2);
                    //console.log(response2);

                    // console.log("hasil Data Response2 = " + response2.data.groupId);

                    if (response2.data.groupId === '1be4026b-523e-46f0-9f59-b045ec6ee7a2') {
                        $('#btnSumbitMoveStage').prop('disabled', false);
                    } else {
                        $('#btnSumbitMoveStage').prop('disabled', true);
                    }


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
                        if (response2.data.applicant_move_stage_id === null) {
                            dataraw2 = "Screening / New Applicant";
                            $("#candidate_recruitment_id").val("1");
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
                            $("#candidate_recruitment_id").val(response2.data.applicant_move_stage_id);
                        }

                        var intId = parseInt($("#candidate_recruitment_id").val());
                        var nextId = intId + 1;

                        if (nextId < 6) {
                            $('#optval' + intId.toString()).show();
                            $('#optval' + nextId.toString()).show();
                        }

                        var applicant_progress_id = response2.data.applicant_progress_id;

                        $("#applicant_progress_id").val(applicant_progress_id);

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
                        var dateBirth = moment(response.data.date_birth).format("YYYY-MM-DD");
                        $('#date_birth').val(dateBirth);

                        if (response2.data.last_education) {
                            ajaxGet(ajaxUrlEducationLevel, { Search_education_id: response2.data.last_education }, function (resp) {
                                resp = parseJson(resp);
                                $('#last_education').append('<option value="' + response2.data.last_education + '" selected>' + resp.rows[0].education_name + '</option>');
                            });

                        }

                        $('#year_experience').val(response2.data.year_experience);
                        $('#phone_number').val(response2.data.phone_number);
                        $('#source').val(response2.data.source);
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

function SubmitData() {
    var applicant_progress_id_current = "0";
    var candidate_recruitment_id = $('#candidate_recruitment_id').val();

    if (candidate_recruitment_id === "1") {
        applicant_progress_id_current = "0";
    } else if (candidate_recruitment_id === "2" || candidate_recruitment_id === "3") {
        applicant_progress_id_current = "1";
    } else if (candidate_recruitment_id === "4") {
        applicant_progress_id_current = "2";
    } else if (candidate_recruitment_id === "6") {
        applicant_progress_id_current = "3";
    } else if (candidate_recruitment_id === "5") {
        applicant_progress_id_current = "3";
    } else {
        applicant_progress_id_current = $("#applicant_progress_id").val();
    }

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
            ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: applicant_progress_id_current, applicant_move_stage_id: candidate_recruitment_id }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    if (candidate_recruitment_id === "5" || candidate_recruitment_id === 5) {
                        var data = {
                            Id: $('#id').val(),
                            //refId: $('#job_vacancy_id').val(),
                            refId: $('#id').val(),
                            detailLink: "/hris/Applicant/TrackingApproval/" + $('#id').val() + "/" + $('#job_vacancy_id').val(),
                            tName: "Job Vacancy Global",
                            menuId: "086cbaa3-41a5-42b2-a830-6075fda0e7dd",
                            IdTemplate: 'e58c8bfb-4b02-48de-852c-85629289dc72',
                            NotificationType: 'hris.applicant.need-approval',
                            Module : 'HRIS'
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

function ShowData(id) {
    

    if (id === "5" || id === 5) {
        $('#approval_page_applicant').css({ display: "block" });
        //ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Job Vacancy", Search_TemplateName: "Job Vacancy IRIS / BRANDCOM" }, function (response) {
        //    response = parseJson(response);
        //    DeleteajaVacancy('no_data');
        //    var count = 1;
        //    $("#JobVacancyListApprover > tbody").html("");
        //    var ttl = "";
        //    $.each(response.rows[0].listTmUserApproval, function (index, value) {
        //        if (value.job_title_name === null || value.job_title_name === "") {
        //            ttl = "Unset Jobtitle";
        //        } else {
        //            ttl = value.job_title_name;
        //        }
        //        AddRowTableVacancy(count, ttl, value.employee_full_name);
        //        count = count + 1;
        //    });
        //    $("#template_approval_id").val(response.rows[0].id);
        //});
        
        ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + $('#job_vacancy_id').val(), {}, function (response2) {
            response2 = parseJson(response2);
            if (response2.data) {
                Deleteaja('no_data');
                var datastatus = '';
                var count = 1;
                var ctn = '';
                var ttl = '';
                $("#JobVacancyAprovalApproverList > tbody").html("");
                $.each(response2.data.listUserApproval, function (index, value) {
                    if (value.statusApproved === 1) {
                        datastatus = "<span class='bluetext'>Need to Approved</span>";
                    } else if (value.statusApproved === 2) {
                        datastatus = "<span>Waiting for Approval</span>";
                    } else if (value.statusApproved === 3) {
                        datastatus = "<span class='greentext'>Approved</span>";
                    } else if (value.statusApproved === 4) {
                        datastatus = "<span class='redtext'>Reject</span>";
                    } else {
                        datastatus = "<span class='bluetext'>Draft</span>";
                    }
                    if (value.statusApproved !== 3) {
                        ctn = '<center> - </center>';
                    } else {
                        ctn = '<center>' + value.createdOn + '</center>';
                    }

                    if (value.job_title_name === null || value.job_title_name === "") {
                        ttl = "Unset Jobtitle";
                    } else {
                        ttl = value.job_title_name;
                    }
                    AddRowTable(count, ttl, value.employee_full_name, '<center>' + datastatus + '</center >', ctn);
                    count = count + 1;
                });} else { DangerNotif("No data list approval for selected ID.");}
        });
    } else {
        $('#approval_page_applicant').css({ display: "none" });
    }

}


function AddRowTable(no, title, nama, status, update_by) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td><td>" ;
    $("#JobVacancyAprovalApproverList tbody").append(markup);
}


function DeleteajaVacancy(id) {
    $('table#JobVacancyListApprover tr#' + id + '').remove();
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function Deleteaja(id) {
    $('table#JobVacancyAprovalApproverList tr#' + id + '').remove();
}

 

