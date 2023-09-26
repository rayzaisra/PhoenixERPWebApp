var webUrl = WEB + "/Hris/JobVacancy";
var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlUpdate = API + "/Hris/CandidateRecruitment/UpdateCustom";
var xform = "JobVacancyFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";
var UrlSessionData = API + "employeeInfo/getByUserLogin";
var tableList = "ApplicantProgressList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var ajaxUrlListJobTitle = API + "Hris/JobTitle/List/";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";
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
        var xid = $('#id').val();
        var job_vacancy_id = $('#job_vacancy_id').val();
        InitTable(tableList, ajaxUrlList, { Search_applicant_progress_id: 0, Search_job_vacancy_id: job_vacancy_id }, columnsIndex, 'id', 'asc');
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
        initlanguage(lang_hr);
        $("#reject_reason").prop('disabled', true);
        //$('#cv').setuploadbox({});
        //$('#photo').setuploadbox({});
        //$('#portfolio').setuploadbox({});

        ajaxGet(ajaxUrlGet + "/" + job_vacancy_id, {}, function (response) {

            response = parseJson(response);
            //console.log(response);
            if (response.data) {
                
                ajaxGet(ajaxUrlGetCandidateReq + "/" + xid, {}, function (resp) {
                    response2 = parseJson(resp);
                    //console.log(response2);
                    if (response2.data) {
                        
                        var CurrentEmployeeID = "";

                        ajaxGet(UrlSessionData, {}, function (responseSession) {
                            responseSession = parseJson(responseSession);
                            
                            if (responseSession.data !== null) {
                                CurrentEmployeeID = responseSession.data.employeeId;
                            } else {
                                swal('Information', 'Login User Data Not Valid!', 'info');
                                setTimeout(
                                    function () {
                                        //do something special
                                        window.location.href = WEB + "/Core/Authentication?status=Logout";
                                    }, 2000);
                            }
                            ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + xid, {}, function (resp2) {
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
                                }

                                var intId = parseInt($("#candidate_recruitment_id").val());
                                var nextId = intId + 1;

                                if (nextId < 6) {
                                    $('#optval' + intId.toString()).show();
                                    $('#optval' + nextId.toString()).show();
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

                                response4 = parseJson(resp2);
                                Deleteaja('no_data');
                                var datastatus = '';
                                var count = 1;
                                var ctn = '';
                                var ttl = '';
                                $("#JobVacancyAprovalApproverList > tbody").html("");
                                if (response4.data !== null) {
                                    $.each(response4.data.listUserApproval, function (index, value) {

                                        if (value.statusApproved === 1) {
                                            if (value.employeeBasicInfoId === CurrentEmployeeID) {
                                                $("#with_approval").css({ display: "block" });
                                                $("#no_approval").css({ display: "none" });
                                            } else {
                                                $("#with_approval").css({ display: "none" });
                                                $("#no_approval").css({ display: "block" });
                                            }
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
                                    });
                                } else {
                                    var markup = "<tr id='no_data'><td colspan='5'>No Data</td></tr>";
                                    $("#JobVacancyAprovalApproverList tbody").append(markup);
                                }


                            });
                        });

                    } else { DangerNotif("No data candidate found for selected ID."); }

                });
            } else { DangerNotif("No data job vacancy found for selected ID."); }



        });
    }
    
   
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



function ApprovedData() {
    var text = "Do you want to Approved this Data";
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

            var data = {
                //Id: '',
                //refId: $('#id').val(),
                Id: $('#id').val(),
                refId: $('#id').val(),
                StatusApproved: 3,
                Remarks: ""
            };
           
            ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: 4, applicant_move_stage_id: 6 }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    UpdateStatusApprovalHris(data, WEB + "/hris/ApplicantDetail/PersonalInfo/" + $('#id').val() + "/" + $('#job_vacancy_id').val());
                } else {
                    DangerNotif(response.message);
                }
            });

        }
    });

}

function RejectData() {
    $('#MppApprovedStatusModalSearch').modal('toggle');

    var reason1 = $('#reject_reason').val();

    var data = {
        Id: '',
        refId: $('#id').val(),
        StatusApproved: 4,
        Remarks: reason1
    };

    ajaxPost(API + "/Hris/CandidateRecruitment/UpdateCustom", { id: $('#id').val(), applicant_progress_id: 6, applicant_move_stage_id: 8 }, function (response) {
        response = parseJson(response);
        if (response.success) {
            UpdateStatusApprovalHris(data, WEB + "/hris/ApplicantDetail/PersonalInfo/" + $('#id').val() + "/" + $('#job_vacancy_id').val());
        } else {
            DangerNotif(response.message);
        }
    });
}

function AddRowTable(no, title, nama, status, update_by) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td><td>" + status + "</td><td>" + update_by + "</td></tr>";
    $("#JobVacancyAprovalApproverList tbody").append(markup);
}


function Deleteaja(id) {
    $('table#JobVacancyAprovalApproverList tr#' + id + '').remove();
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}


 

