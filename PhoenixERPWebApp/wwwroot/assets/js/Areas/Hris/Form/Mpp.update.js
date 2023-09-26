 
var webUrl = WEB + "/Hris/Mpp";
var ajaxUrl = API + "General/BusinessUnit";
var ajaxUrlUpdate = API_HRIS + "/Mpp/Update";
var xform = "MppFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlListGet = API + "/Hris/Mpp/GetWithDetil";
var ajaxUrlListOrganization = API_GENERAL + "/BusinessUnit/ListWithParent";
var ajaxUrlListJobTitle = API_HRIS + "/JobTitle/List";
var ajaxUrlListJobGrade = API_HRIS + "/JobGrade/List";
var ttl_budget = 0;
var MppNewRealData = [];
$(function () {
    var xid = $("#" + xform +' [name="id"]').val();
    if (xid) {

        $('#job_title').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlListJobTitle,
            searchparam: 'search_title_name',
            labelField: 'title_name',
            valueField: 'id',
            searchField: 'title_name'
        });
        $('#JobGrade').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlListJobGrade,
            searchparam: 'search_grade_name',
            labelField: 'grade_name',
            valueField: 'id',
            searchField: 'grade_name'
        });

    ajaxGet(ajaxUrlListGet + "/" + xid, {}, function (response) {
        response = parseJson(response);

        var optionsForm = GetOptionsForm(function () {
            return $("#" + xform).valid();
        }, function (response, statusText, xhr, $form) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                window.location.href = WEB + "/hris/Mpp";
            } else {

                DangerNotif(response.message);
            }
        });
        InitForm(xform, optionsForm);
        setFormAction(xform, ajaxUrlUpdate);
        //console.log(response);
        //$('#division option[value=' + response.data.parent.parent.Id + ']').attr('selected', 'selected');
        var total_budget = 0;
        $('#fiscal_year').val(response.data.fiscal_year);
        $('#status').val(response.data.status);
        $('#code').val(response.data.code);
        $('#business_unit_id_name').val(response.data.business_unit_name);
        $('#business_unit_id').val(response.data.business_unit_id);
        //$('#business_unit_id').val(response.data.business_unit_id).trigger('change');
        $('#tmpmpp2').empty();
        var tmpmpno = 0;
        $.each(response.data.mppDetail, function (index, value) {
            //console.log(value);
            AddRowTable(value.job_title_name, value.ttf, value.employment_status, value.mpp_budged, value.job_grade_id, value.project, value.desc, value.job_tittle_id);
            total_budget = parseInt(total_budget) + parseInt(value.mpp_budged);
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle]" value="' + value.jobtitle + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][ttf]" value="' + value.ttf + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][status]" value="' + value.status + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][mppbudget]" value="' + value.mppbudget + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][grade]" value="' + value.grade + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][project]" value="' + value.project + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][desc]" value="' + value.desc + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle_name]" value="' + value.jobtitle_name + '"  >');
            tmpmpno++;
        });
        AddRowTotalBudget(total_budget);
        $('#total_budget').val(total_budget);    
    });

    }
    
    
});

function DeleteLogic(id) {
    getData();
    if (MppNewRealData.length === 1) {
        Deleteaja('total_budget');
        AddRowNoData();
        AddRowTotalBudget(0);
    } else {
        Deleteaja(id);
        Deleteaja('total_budget');
        getData();
        var ttl = 0;
        $('#tmpmpp2').empty();
        var tmpmpno = 0;
        $.each(MppNewRealData, function (index, value) {
            ttl = parseInt(ttl) + parseInt(value.mppbudget);
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle]" value="' + value.jobtitle + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][ttf]" value="' + value.ttf + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][status]" value="' + value.status + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][mppbudget]" value="' + value.mppbudget + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][grade]" value="' + value.grade + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][project]" value="' + value.project + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][desc]" value="' + value.desc + '"  >');
            $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle_name]" value="' + value.jobtitle_name + '"  >');
            tmpmpno++;
        });
        AddRowTotalBudget(ttl);
    }
    $('table#MppDetailList tr#' + id + '').remove();
}

function Deleteaja(id) {
    $('table#MppDetailList tr#' + id + '').remove();
}

function AddRowTable(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + jobtitle + "</td><td>" + tff + "</td><td>" + status + "</td><td>" + mppbudget + "</td><td style='display: none;'>" + grade + "</td><td style='display: none;'>" + project + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteLogic(" + acak + ")'>delete</a></center></td><td style='display: none;'>" + desc + "</td><td style='display: none;'>" + jobtitleid + "</td></tr>";
    $("table tbody").append(markup);
}

function CheckAndAddRow(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid) {
    if ($('#job_title').val() === "") {
        DangerNotif('Please Choose Job Title');
        return false;
    } else if ($('#JobGrade').val() === "") {
        DangerNotif('Please Choose Job Grade');
        return false;
    } else if ($('#mppBudget').val() === "") {
        DangerNotif('Please Enter Mpp Budget');
        return false;
    } else {
        if ($('#budget_source').val() === "PROJECT" && $('#project_name').val() === "") {
            DangerNotif('Please enter project name when budget Souce set to project');
            return false;
        } else {
            Deleteaja('no_data');
            Deleteaja('total_budget');
            getData();
            // if (RealData.length > 0) {
            tmpData = {
                "jobtitle": jobtitleid,
                "ttf": tff,
                "status": status,
                "mppbudget": mppbudget,
                "grade": grade,
                "project": project,
                "desc": desc,
                "jobtitle_name": jobtitle
            };
            MppNewRealData.push(tmpData);
            $('#tmpmpp2').empty();
            var tmpmpno = 0;
            $.each(MppNewRealData, function (index, value) {

                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle]" value="' + value.jobtitle + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][ttf]" value="' + value.ttf + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][status]" value="' + value.status + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][mppbudget]" value="' + value.mppbudget + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][grade]" value="' + value.grade + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][project]" value="' + value.project + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][desc]" value="' + value.desc + '"  >');
                $('#tmpmpp2').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle_name]" value="' + value.jobtitle_name + '"  >');
                tmpmpno++;
            });

            AddRowTable(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid);
            // } else {
            //     AddRowTable(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid);
            //}
            ttl_budget = parseInt(ttl_budget) + parseInt(mppbudget);
            AddRowTotalBudget(ttl_budget);
            // $('#dataDetailMpp').val(JSON.stringify(MppNewRealData));
            $('#MppDetailFormCreateModal').modal('hide');
        }
    }
}

function AddRowNoData() {
    var markup = "<tr id='no_data'><td colspan='5'>No Data</td></td></tr>";
    $("table tbody").append(markup);

}
function AddRowTotalBudget(total) {
    var markup = "<tr id='total_budget'><td colspan='3'>&nbsp;</td><td>TOTAL BUDGET</td ><td>" + total + "</td></tr>";
    $("table tbody").append(markup);
    $('#total_budget').val(total);
}
function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}


function getData() {
    var TableData = '';
    var tmpData = '';
    MppNewRealData = [];
    var rowCount = $('#MppDetailList >tbody >tr').length;
    // alert(rowCount);
    $('#MppDetailList tr').each(function (row, tr) {
        console.log(row);
        if ($(tr).find('td:eq(8)').text() !== "") {
            tmpData = {
                "jobtitle": $(tr).find('td:eq(8)').text(),
                "ttf": $(tr).find('td:eq(1)').text(),
                "status": $(tr).find('td:eq(2)').text(),
                "mppbudget": $(tr).find('td:eq(3)').text(),
                "grade": $(tr).find('td:eq(4)').text(),
                "project": $(tr).find('td:eq(5)').text(),
                "desc": $(tr).find('td:eq(7)').text(),
                "jobtitle_name": $(tr).find('td:eq(0)').text()
            };
            MppNewRealData.push(tmpData);
        }
        //TableData = TableData
        //    + $(tr).find('td:eq(0)').text() + ' '  // jobtitle
        //    + $(tr).find('td:eq(1)').text() + ' '  // ttf
        //    + $(tr).find('td:eq(2)').text() + ' '  // status
        //    + $(tr).find('td:eq(3)').text() + ' '  // mppbudget
        //    + $(tr).find('td:eq(4)').text() + ' '  // grade
        //    + $(tr).find('td:eq(5)').text() + ' '  // project
        //    + $(tr).find('td:eq(7)').text() + ' '  // desc
        //    + $(tr).find('td:eq(8)').text() + ' '  // jobtitle_name
        //    + $(tr).find('td:eq(6)').text() + ' '  // Task
        //     + '\n';

    });
    console.log(MppNewRealData);
    $('#dataDetailMpp').val(JSON.stringify(MppNewRealData));
}


  
