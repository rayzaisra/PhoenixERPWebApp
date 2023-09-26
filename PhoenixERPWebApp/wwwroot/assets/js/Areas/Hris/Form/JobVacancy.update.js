 var webUrl = WEB + "/Hris/JobVacancy";
var ajaxUrl = API + "/Hris/JobVacancy";
var ajaxUrlUpdate = ajaxUrl + "/UpdatePostingStatus";
var xform = "JobVacancyFormUpdate";
var ajaxUrlGet = ajaxUrl + "/GetWithDetial"; 
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrllocationGet = API + "Hris/JobVacancy/GetAllLocation";
var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List"; 
var ajaxUrlGetApprovalUserClose = API + "ManageTransApproval/GetListApprovalByRefId";

$(function () {
    var xid = $('#id').val();
    $('#master_location_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrllocationGet,
        searchparam: 'Search_location_name',
        labelField: 'location_name',
        valueField: 'id',
        searchField: 'location_name'
    });

    ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
        response = parseJson(response);
        $('#code').val(response.data.code);
        $('#vacancy_name').val(response.data.vacancy_name);
        $('#closing_date').val(moment(response.data.closing_date).format("YYYY-MM-DD"));
        response.data.master_location_id = response.data.master_location_id || '';
        response.data.location = response.data.location || '';
        if (response.data.master_location_id !== "") {
            $('#master_location_id').append('<option value="' + response.data.master_location_id + '" selected>' + response.data.master_location_name + '</option>');
        } 

        //$('#posting_status option[value="' + response.data.posting_status + '"]').prop('selected', true);

        if (response.data.posting_status === 0) {
            $('#posting_status').val('Draft');
        } else if (response.data.posting_status === 3) {
            $('#posting_status').val('Post/Unpublished');
        } else if (response.data.posting_status === 4) {
            $('#posting_status').val('Post/Published');
        } else if (response.data.posting_status === 4) {
            $('#posting_status').val('Closed');
        }

        ajaxGet(ajaxUrlGetHiringRequest + "/" + response.data.hiring_request_id, {}, function (response) {
            response2 = parseJson(response);

            $('#hiring_request_id').val(response2.data.id);
            $('#hiring_request_code').val(response2.data.code);
            $('#hiring_request_id').val(response2.data.id);
            $('#hiring_request_code').val(response2.data.code);
            $('#requester_id').val(response2.data.requester_employee_basic_info_id);
            $('#requester_name').val(response2.data.requester_name);
            $('#job_title_name').val(response.data.job_title_name + "(" + response.data.job_grade_name + ")");
            $('#job_title_id').val(response2.data.job_title_id);
            $('#business_unit_id').val(response2.data.business_unit_id);
            $('#business_unit_name').val(response2.data.business_unit_name);

            budgted_table = "<tr id=" + response2.data.id + "><td>" + response2.data.job_title_name + "</td><td>" + response2.data.job_grade_name + "</td><td>" + response2.data.ttf + "</td><td>" + response2.data.employment_status + "</td><td>" + response2.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response2.data.mpp_budged) + "</td><td>" + moment(response2.data.date_fulfilment).format("DD MMMM YYYY HH:mm") + "</td></tr>";
            $("#dt_basic tbody").append(budgted_table);
        });

        if (response.data.posting_status !== '0') {
            $('#code').prop('disabled', true);
            $('#vacancy_name').prop('disabled', true);
            $('#closing_date').prop('disabled', true);
            $('#master_location_id').prop('disabled', true);
            $('#hiring_request_id').prop('disabled', true);
            $('#hiring_request_code').prop('disabled', true);
            $('#requester_id').prop('disabled', true);
            $('#posting_status').prop('disabled', true);
            $('#job_title_id').prop('disabled', true);
            $('#business_unit_id').prop('disabled', true);
            $('#requester_name').prop('disabled', true);
            $('#job_title_name').prop('disabled', true);
            $('#business_unit_name').prop('disabled', true);
            $('#btn-save').hide();

            Approval('1',xid);
        } else {
            Approval('0',null);
        }
    });
    

});


function UpdateData() {
    var text = "Do you want to Update this Data";
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
            ajaxPost(ajaxUrlUpdate, { id: $('#id').val(), posting_status: $('#posting_status').val() }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Update Sucess");
                    window.location.href = WEB + "/hris/JobVacancy";
                } else {
                    DangerNotif(response.message);
                }
            });
        }
    });

}
  
function Approval(isClose,xid=null) {
    if (isClose === '0') {
        ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Job Vacancy" }, function (response) {
            response = parseJson(response);

            if (response.rows !== null) {
                if (response.rows.length > 0) {
                    DeleteajaApproval('no_data');
                    var count = 1;
                    $("#VacancyApprover > tbody").html("");
                    $.each(response.rows[0].listTmUserApproval, function (index, value) {
                        AddRowTableApproval(count, value.job_title_name, value.employee_full_name);
                        count = count + 1;
                    });
                    $("#template_approval_id").val(response.rows[0].id);
                } else {
                    swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                    setTimeout(
                        function () {
                            //do something special
                            window.location.href = WEB + "/Hris/JobVacancy/";
                        }, 2000);

                }

            }
        });
    } else {
        ajaxGet(ajaxUrlGetApprovalUserClose + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.length > 0) {
                DeleteajaApproval('no_data');
                var count = 1;
                $("#VacancyApprover > tbody").html("");
                $.each(response, function (index, value) {
                    AddRowTableApproval(count, value.jobTitleName, value.employeeName);
                    count = count + 1;
                });
                $("#template_approval_id").val(response.id);
            } else {
                swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                setTimeout(
                    function () {
                        //do something special
                        window.location.href = WEB + "/Hris/JobVacancy/";
                    }, 2000);

            }
        });
    }
   

}

function AddRowTableApproval(count, job_title_name, employee_full_name) {
    var acak = randomIntFromInterval(1, 999);
    if (job_title_name === null) {
        job_title_name = "Undefined Jobtitle";
    }
    var markup = "<tr id=" + acak + "><td>" + count + "</td><td>" + job_title_name + "</td><td>" + employee_full_name + "</td></tr>";
    $("table#VacancyApprover tbody").append(markup);
}


function DeleteajaApproval(id) {
    $('table#VacancyApprover tr#' + id + '').remove();
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}
