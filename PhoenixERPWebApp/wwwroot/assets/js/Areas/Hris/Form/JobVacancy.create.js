var webUrl = WEB + "/Hris/JobVancacy";
var ajaxUrl = API + "Hris/JobVancacy";
var ajaxUrlCreate = API + "Hris/JobVacancy/CreateDetail";
var ajaxUrlListHiringRequest = API + "Hris/HiringRequest/ListAllWithDetial";
var ajaxUrlListHiringCheck = API + "Hris/HiringRequest/GetListHiringMppDetail";
var ajaxUrlMppDetailGet = API + "Hris/Mppdetail/Get";
var ajaxUrlMppDetailGetNew = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrllocationGet = API + "Hris/JobVacancy/GetAllLocation";
var xform = "JobVacancyFormCreate";
var TABLE_NAME_JOB_POSTING = "dt_basic_job_posting";
var JobSouceData = [];
var formdetil = "JobVacancyModalFormDetail";
var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List"; 
 

$(function () {
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        var rowCount = $("#" + TABLE_NAME_JOB_POSTING + " >tbody >tr").length;
        if (xret && rowCount > 0) {
            startprocess();
        } else {
            DangerNotif("Please fill the job vacancy source tab");
            return false;
        }
     
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
            response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            var data = {
                refId: response.data.id,
                detailLink: "/Hris/JobVacancy/Approval/" + response.data.id,
                tName: "hr.job_vacancy",
                menuId: "086cbaa3-41a5-42b2-a830-6075fda0e7dd",
                //IdTemplate: $("#template_approval_id").val(),
                IdTemplate: "",
                NotificationType: 'hris.jobvacancy.need-approval',
                Module : 'HRIS'
            };
            saveTemplateApprovalHris(data, WEB + "/hris/JobVacancy");
           // window.location.href = WEB + "/hris/JobVacancy";
            } else {
                DangerNotif(response.message);
            }
    });

    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);
    $('#code').val("PST" + getDateNow() + randomIntFromInterval(1, 9999999));
   
    $('#hiring_request_id').setcombobox({
        data: { 'rows': 100, 'Search_status': '1' },
        url: ajaxUrlListHiringRequest,
        searchparam: 'Search_code',
        labelField: 'code',
        valueField: 'id',
        searchField: 'code',
        render: function (data) {
            var raw = '';
            response = JSON.stringify(data.odata);
            console.log("Masuk = " + response);
            if (response !== undefined) {
                console.log("Masuk Code = " + data.odata.code);
                if (data.odata.division !== null)
                return '<Span>' + data.odata.code + " - ( " + data.odata.division.unit_name + " / " + data.odata.jobtitile.title_name + " )" + '</Span>';
            }
            //if (typeof(jsvariable)
            //if (typeof(data.odata) !== undefined) {
            //    console.log("Result Combo = " + JSON.stringify(data));
            //    if (JSON.stringify(data) !== null) {
            //        console.log("Masuk = " +  data.odata.code);
            //    }
            //}
            //if (data.odata) {
            //    return '<Span>' + data.odata.code + " - ( " + data.odata.division.unit_name + " / " +data.odata.jobtitile.title_name + " )" +'</Span>';
            //}

        }
    });

    $('#master_location_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrllocationGet,
        searchparam: 'Search_location_name',
        labelField: 'location_name',
        valueField: 'id',
        searchField: 'location_name'
    });

    $('#upload_file').setuploadbox();

    Approval();
   
});

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}
function getDateNow() {
    var d = new Date();
    var strDate = d.getFullYear() + (d.getMonth() + 1) + d.getDate();

    return strDate;
}

function GetDataHiringRequest(id) {
    var tc_table = '';
    
    if (id !== "") {
        ajaxGetJquery(ajaxUrlMppDetailGetNew + "/" + id, {}, function (response) {
            if (response.data === null) {
                DangerNotif("No Data Avaliable");
            } else {
                response = parseJson(response);
                if (response.data.hiring_request_type_id === "Replacement") {
                    $('#head_main_paper').html("<h5>Man Power Replaced</h5>");
                } else {
                    $('#head_main_paper').html("<h5>Man Power Request</h5>");
                }
                $("#budgeted_div").css({ display: "block" });
                $('#vacancy_name').val(response.data.job_title_name + " (" + response.data.job_grade_name + ")");
                $('#requester_id').val(response.data.requester_name);
                $('#job_title_name').val(response.data.job_title_name);
                $('#business_unit_name').val(response.data.business_unit_name);
                if (response.data.hiring_request_type_id === "Replacement") {
                    $('#tff_head').hide();
                    $('#employment_status_head').html("Employee Replaced");
                    $('#mpp_head').html("Budget");
                    tc_table = tc_table + "<tr><td>" + response.data.job_title_name + "</td><td>" + response.data.job_grade_name + "</td><td>" + response.data.replacedEmployee.employee_replaced_name + "</td><td>" + response.data.budget_souce + "</td><td> Rp. " + response.data.mpp_budged + "</td><td>" + moment(response.data.date_fulfilment).format("DD MMMM YYYY") + "</td></tr>";

                } else {
                    $('#tff_head').show();
                    $('#mpp_head').html("Mpp Budget");
                    $('#employment_status_head').html("Employee Status");
                    tc_table = tc_table + "<tr><td>" + response.data.job_title_name + "</td><td>" + response.data.job_grade_name + "</td><td>" + response.data.ttf + "</td><td>" + response.data.employment_status + "</td><td>" + response.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response.data.mpp_budged) + "</td><td>" + moment(response.data.date_fulfilment).format("DD MMMM YYYY") + "</td></tr>";

                }
                $("#dt_basic tbody").html("");
                $("#dt_basic tbody").append(tc_table);

                var markupnodata = "<tr id='no_data'><td colspan='6'>No Data</td></td></tr>";
                $("#" + TABLE_NAME_JOB_POSTING + " tbody").html("");
                $("#" + TABLE_NAME_JOB_POSTING + " tbody").append(markupnodata);
            }

        });
    }
}

function AddRowNoData(tableid) {
    var markup = "<tr id='no_data'><td colspan='6'>No Data</td></td></tr>";
    $("#" + tableid +" tbody").append(markup);

}

function Deleteaja(tableid,id) {
    $("table#" + tableid +" tr#" + id + "").remove();
}

function getDataJobSouce() {
    var tmpData = '';
    JobSouceData = [];
    var rowCount = $("#" + TABLE_NAME_JOB_POSTING +" >tbody >tr").length;
    $("#" + TABLE_NAME_JOB_POSTING +" tr").each(function (row, tr) {
        console.log(row);
        if ($(tr).find('td:eq(0)').text() !== "") {
            tmpData = {
                "posting_code": $(tr).find('td:eq(0)').text(),
                "source": $(tr).find('td:eq(1)').text(),
                "files": $(tr).find('td:eq(2)').text(),
                "url": $(tr).find('td:eq(3)').text(),
                "date_posting": $(tr).find('td:eq(4)').text()
            };
            JobSouceData.push(tmpData);
        }
     
    });
    console.log(JobSouceData);
}

function DeleteLogicJobPosting(id) {
    getDataJobSouce();
    if (JobSouceData.length === 1) {
        AddRowNoData(TABLE_NAME_JOB_POSTING);
    } else {
        Deleteaja(TABLE_NAME_JOB_POSTING,id);
        getDataJobSouce();
        var ttl = 0;
        $('#tmpjobsouce').empty();
        var tmpmpno = 0;
        $.each(JobSouceData, function (index, value) {
            $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][posting_code]" value="' + value.posting_code + '"  >');
            $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][source]" value="' + value.source + '"  >');
            $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][files]" value="' + value.files + '"  >');
            $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][url]" value="' + value.url + '"  >');
            $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][date_posting]" value="' + value.date_posting + '"  >');
            tmpmpno++;
        });
    }
    $("table#" + TABLE_NAME_JOB_POSTING +" tr#" + id ).remove();
}

function AddRowTableJobPosting(posting_code, source, files, url, date_posting) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + posting_code + "</td><td>" + source + "</td><td>" + files + "</td><td>" + url + "</td><td>" + date_posting + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteLogicJobPosting(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#" + TABLE_NAME_JOB_POSTING +" tbody").append(markup);
}

function ClearModal() {

    var optionsForm = GetOptionsForm(function () {
        return $("#" + formdetil).valid();
    });
    InitForm(formdetil, optionsForm);
    $('#jobposting_posting_code').val($('#code').val());
    $('#jobposting_files').setuploadbox();
}

function CheckAndAddRowJobPosting(posting_code, source, files, url, date_posting) {

    if ($('#jobposting_url').val() === "") {
        DangerNotif('Please Choose Job Title');
        return false;
    } else {
        Deleteaja(TABLE_NAME_JOB_POSTING,'no_data');
        getDataJobSouce();
            // if (RealData.length > 0) {
            tmpData = {
                "posting_code": posting_code,
                "source": source,
                "files": files,
                "url": url,
                "date_posting": date_posting
            };
            JobSouceData.push(tmpData);
            $('#tmpjobsouce').empty();
            var tmpmpno = 0;
            $.each(JobSouceData, function (index, value) {
                $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][posting_code]" value="' + value.posting_code + '"  >');
                $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][source]" value="' + value.source + '"  >');
                $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][files]" value="' + value.files + '"  >');
                $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][url]" value="' + value.url + '"  >');
                $('#tmpjobsouce').append('<input type="hidden" name="JobVacancyJobSouceInnerDto[' + tmpmpno + '][date_posting]" value="' + value.date_posting + '"  >');
                tmpmpno++;
            });

            AddRowTableJobPosting(posting_code, source, files, url, date_posting);
            $('#JobVacancyModalDetail').modal('hide');
      
    }
}

function Approval() {
    ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Job Vacancy" }, function (response) {

        response = parseJson(response);
        console.log(response.rows);
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
