var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/UpdatePostingStatus";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/GetWithDetial";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";
var UrlSessionData = API + "employeeInfo/getByUserLogin";


$(function () {
    initlanguage(lang_hr);
    var xid = $('#id').val();
    ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
        response = parseJson(response);
        $('#code').val(response.data.code);
        $('#vacancy_name').val(response.data.vacancy_name);
        $('#master_location_id').val(response.data.master_location_id);
        $('#master_location_name').val(response.data.master_location_name);
        $('#posting_status').val(response.data.posting_status);
        $('#closing_date').val(moment(response.data.closing_date).format("YYYY-MM-DD"));
        var dataraw = '';
        if (response.data.posting_status === "0") {
            dataraw = "Draft";
        } else if (response.data.posting_status === "1") {
            dataraw = "Approved";
        } else if (response.data.posting_status === "2") {
            dataraw = "Reject";
        } else if (response.data.posting_status === "3") {
            dataraw = "Post/Unpublished";
        } else if (response.data.posting_status === "4") {
            dataraw = "Post/Published";
        } else {
            dataraw = "Closed";
        }
        $('#posting_status_name').val(dataraw);
        ajaxGet(ajaxUrlGetHiringRequest + "/" + response.data.hiring_request_id, {}, function (response) {
            response2 = parseJson(response);
            $('#hiring_request_id').val(response2.data.id);
            $('#hiring_request_code').val(response2.data.code);
            $('#hiring_request_id').val(response2.data.id);
            $('#hiring_request_code').val(response2.data.code);
            $('#requester_id').val(response2.data.requester_employee_basic_info_id);
            $('#requester_name').val(response2.data.requester_name);
            $('#job_title_name').val(response.data.job_title_name + "(" + response.data.job_grade_name+")");
            $('#job_title_id').val(response2.data.job_title_id);
            $('#business_unit_id').val(response2.data.business_unit_id);
            $('#business_unit_name').val(response2.data.business_unit_name);
            $('#departement_id').val(response2.data.departement_id);
            $('#departement_name').val(response2.data.departement_name);

            budgted_table = "<tr id=" + response2.data.id + "><td>" + response2.data.job_title_name + "</td><td>" + response2.data.job_grade_name + "</td><td>" + response2.data.ttf + "</td><td>" + response2.data.employment_status + "</td><td>" + response2.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response2.data.mpp_budged) + "</td><td >" + moment(response2.data.date_fulfilment).format("DD MMMM YYYY HH:mm") + "</td></tr>";
            $("#dt_basic tbody").append(budgted_table);

           
        });

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
            ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + xid, {}, function (response) {
                response = parseJson(response);
                if (response.data !== null) {
                    DeleteajaApproval('no_data');
                    var datastatus = '';
                    var count = 1;
                    var ctn = '';
                    var ttl = '';
                    var statusApproved = "Current Approval";

                    $("#VacancyApproverList > tbody").html("");
                    $.each(response.data.listUserApproval, function (index, value) {

                        if (value.statusApproved === 1) {
                            datastatus = "<span class='bluetext'>Need to Approved</span>";
                            if (value.employeeBasicInfoId === CurrentEmployeeID) {
                                $("#with_approval").css({ display: "block" });
                                $("#no_approval").css({ display: "none" });
                            } else {
                                $("#with_approval").css({ display: "none" });
                                $("#no_approval").css({ display: "block" });
                            }
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
                        AddRowTableApproval(count, ttl, value.employee_full_name, '<center>' + datastatus + '</center >', ctn);
                        count = count + 1;
                    });
                }
            });
        });
    });
});

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
            ajaxPost(ajaxUrlUpdate, { id: $('#id').val(), posting_status: 3 }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Approval Sucess");
                    window.location.href = WEB + "/hris/JobVacancy";
                } else {
                    DangerNotif(response.message);
                }
            });
        }
    });

}

function RejectData() {
    $('#JobVacancyApprovedModalSearch').modal('toggle');

    var reason1 = $('#reject_reason').val();
    ajaxPost(ajaxUrlUpdate, { id: $('#id').val(), posting_status: 2}, function (response) {
        response = parseJson(response);
        if (response.success) {
            SuccessNotif("Reject Sucess");
            window.location.href = WEB + "/hris/JobVacancy";
        } else {
            DangerNotif(response.message);
        }
    });

}

 
function DeleteajaApproval(id) {
    $('table#VacancyApproverList tr#' + id + '').remove();
}

function AddRowTableApproval(no, title, nama, status, update_by) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td><td>" + status + "</td><td>" + update_by + "</td></tr>";
    $("#VacancyApproverList tbody").append(markup);
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}
