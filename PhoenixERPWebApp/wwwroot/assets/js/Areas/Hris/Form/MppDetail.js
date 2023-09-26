var ajaxUrl = API + "/Hris/MppDetail";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = API + "/Hris/Mpp" + "/CreateMppWithDetail";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlListOrganization = API + "General/BusinessUnit/ListWithParent";
var ajaxUrlListFiscalYear = API_HRIS + "/MppDetail/GetFiscalYear";
var ajaxUrlListJobTitle = API_HRIS + "/JobTitle/List";
var ajaxUrlListJobGrade = API_HRIS + "/JobGrade/List";
var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List"; 
var saveType = "0";
var ajaxUrlListGet = API + "/HRIS/Mpp" + "/GetWithDetil"; 
var webUrl = WEB + "/Hris/MppDetail";

var tableList = "MppDetailList";
var formCreate = "MppDetailFormCreate";
var formUpdate = "MppDetailFormUpdate";
var formSearch = "MppDetailFormSearch";
var modalCreate = "MppDetailModalCreate";
var modalUpdate = "MppDetailModalUpdate";
var modalSearch = "MppDetailModalSearch";
var MppNewRealData = [];
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
};
var FormModalInner = "MppDetailFormCreateModalInner";
var ttl_budget = 0;
fnMppDetail = {
    search: function () {
        UIkit.modal("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();
        UIkit.modal("#" + modalSearch).hide();
    },
    create: function () {
        console.log($('#'+FormModalInner).length);
        document.getElementById(FormModalInner).reset();
       
        //setFormAction(formCreate, ajaxUrlCreate);
    }
};


$(function () {

    initlanguage(lang_hr);
    var xid = $('#id').val();
    var tbody = $("#MppDetailList tbody");

    if (tbody.children().length === 2) {
        $('#btn-submit').prop("disabled", true);
        $('#btn-submit').css("cursor", "default");
        $('#btn-save').removeAttr('href');
        $('#btn-save').css("pointer-events", "none");
        $('#btn-save').css("cursor", "default");
    }

    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + formCreate).valid();
        if (xret) startprocess();
        return xret;

    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);

        if (response.success) {
            if (saveType === '0') {
                var data = {
                    refId: response.data.id,
                    detailLink: "/Hris/MppApprovedStatus/Index/" + response.data.id,
                    tName: "hr.mpp",
                    menuId: "9c85a265-6e60-41d2-9e1b-792857b381d0",
                    IdTemplate: $("#template_approval_id").val(),
                    NotificationType: "hris.mpp.need-approval",
                    Module: "HRIS"
                };
                saveTemplateApprovalHris(data, webUrl);
            } else {
                window.location.href = WEB + "/hris/Mpp";
            }

            SuccessNotif(t_createsuccess);
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(formCreate, optionsForm);
    setFormAction(formCreate, ajaxUrlCreate);

    $('#business_unit_id').setcombobox({
        data: { 'rows': 1000, 'Search_business_unit_level': 800,'Search_parent_unit_is_not_null' : true },
        url: ajaxUrlListOrganization,
        searchparam: 'Search_unit_name',
        labelField: 'unit_name',
        valueField: 'id',
        searchField: 'unit_name'
    });
   
    $("#business_unit_id").change(function () {
        $("#business_unit_id_name").val($("#business_unit_id option:selected").text());
        //alert("business_unit_id_name = " + $("#business_unit_id_name").val());
        //alert("ajaxUrlGetTempateApprovalLink = " + ajaxUrlGetTempateApprovalLink);

        if ($("#business_unit_id_name").val($("#business_unit_id option:selected").text()) !== null && $("#business_unit_id_name").val($("#business_unit_id option:selected").text()) !== "") { 
            ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "MPP", Search_BusinessUnitName: $("#business_unit_id option:selected").text() }, function (response) {

                response = parseJson(response);
                console.log(response.rows);
                if (response.rows !== null) {
                    
                    //console.log("Urutan = " + JSON.stringify(response));

                    if (response.rows.length > 0) {
                        DeleteajaApproval('no_data');
                        var count = 1;
                        $("#MppListApprover > tbody").html("");
                        $.each(response.rows[0].listTmUserApproval, function (index, value) {
                            AddRowTableApproval(count, response.rows[0].listTmUserApproval[index].job_title_name, value.employee_full_name);
                            count = count + 1;
                        });
                        $("#template_approval_id").val(response.rows[0].id);
                    } else {
                        swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                        setTimeout(
                            function () {
                                //do something special
                                window.location.href = WEB + "/hris/Mpp";
                            },2000);
                        
                    }
                    
                }
           

           });
        }
    });

    $('#division').setcombobox({
        data: { 'rows': 100,'Search_business_unit_type_id':'division'},
        url: ajaxUrlListOrganization,
        searchparam: 'Search_unit_name',
        labelField: 'unit_name',
        valueField: 'id',
        searchField: 'unit_name'
    });

    $('#fiscal_year').setcombobox({
        data: { 'rows': 100, 'Search_Fiscal_Year': $('#fiscal_year').val()  },
        url: ajaxUrlListFiscalYear,
        searchparam: 'Search_Fiscal_Year',
        labelField: 'financialYearName',
        valueField: 'financialYearNumber',
        searchField: 'financialYearName'
    });

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
    budget_souce_change($('#budget_source').val());

    if (xid !== "") {
        ajaxGet(ajaxUrlListGet + "/" + xid, {}, function (response) {

            //console.log("Resul Json Stringfy = " + JSON.stringify(response));
           
            response = parseJson(response);
            var total_budget = 0;
            $('#fiscal_year').val(response.data.fiscal_year);
            //console.log("fiscal_year = " + response.data.fiscal_year);
            $('#fiscal_year').append('<option value="' + response.data.fiscal_year_number + '">' + response.data.fiscal_year_name + '</option>');
            
            $('#status').val(response.data.status);
            $('#code').val(response.data.code);
            $('#b_unit_id').val(response.data.business_unit_id);
            $('#business_unit_id_name').val(response.data.business_unit_name);
            $('#business_unit_id').val(response.data.business_unit_id);
            $('#business_unit_id').append('<option value="' + response.data.business_unit_id + '">' + response.data.business_unit_name + '</option>');
            $('#business_unit_id').val(response.data.business_unit_id).trigger('change');
            $.each(response.data.mppDetail, function (index, value) {
                //console.log("Value Project = " + value.porject_name);
                //Deleteaja('no_data');
                //Deleteaja('total_budget');
                //AddRowTable(value.job_title_name, value.ttf, value.employment_status, value.mpp_budged, value.job_grade_id, value.project, value.desc, value.job_tittle_id);
                //total_budget = parseInt(total_budget) + parseInt(value.mpp_budged);
                $('#job_title').val(value.job_title_name);
                $('#JobGrade').val(value.job_grade_id);
                $('#mppBudget').val(value.mpp_budged);
                $('#budget_source').val(value.budget_souce);
                $('#project_name').val(value.project);
                CheckAndAddRow(value.job_title_name, value.ttf, value.employment_status, value.mpp_budged, value.job_grade_id, value.porject_name, value.description, value.job_tittle_id, value.budget_souce);
               
            });
            $('#fiscal_year').setcombobox({
                data: { 'rows': 100, 'Search_Fiscal_Year': $('#fiscal_year').val() },
                url: ajaxUrlListFiscalYear,
                searchparam: 'Search_Fiscal_Year',
                labelField: 'financialYearName',
                valueField: 'financialYearNumber',
                searchField: 'financialYearName'
            });

            //AddRowTotalBudget(total_budget);
            $('#total_budget').val(total_budget);
           
            //$('#business_unit_id').append('<option value="' + response.data.business_unit_id + '" selected >xxxxxxxxxxxxx</option>');
            //$('#business_unit_id').setcombobox({
            //    data: { 'rows': 100, 'Search_business_unit_type_id': 'departement' },
            //    url: ajaxUrlListOrganization,
            //    searchparam: 'Search_unit_name',
            //    labelField: 'unit_name',
            //    valueField: 'id',
            //    searchField: 'unit_name'
            //});
            //$('#division').setcombobox({
            //    data: { 'rows': 100, 'Search_business_unit_type_id': 'division' },
            //    url: ajaxUrlListOrganization,
            //    searchparam: 'Search_unit_name',
            //    labelField: 'unit_name',
            //    valueField: 'id',
            //    searchField: 'unit_name'
            //});  
            var CurrentEmployeeID = "";
            //ajaxGet(UrlSessionData, {}, function (responseSession) {
            //    responseSession = parseJson(responseSession);
            //    console.log(responseSession);
            //    if (responseSession.data !== null) {
            //        CurrentEmployeeID = responseSession.data.employeeId;
            //    } else {
            //        swal('Information', 'Login User Data Not Valid!', 'info');
            //        setTimeout(
            //            function () {
            //                //do something special
            //                window.location.href = WEB + "/Core/Authentication?status=Logout";
            //            }, 2000);
            //    }
            //    ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + xid, {}, function (response) {
            //        response = parseJson(response);
            //        if (response.data !== null) {
            //            DeleteajaApproval('no_data');
            //            var datastatus = '';
            //            var count = 1;
            //            var ctn = '';
            //            var ttl = '';
            //            var statusApproved = "Current Approval";

            //            $("#MppAprovalApproverList > tbody").html("");
            //            $.each(response.data.listUserApproval, function (index, value) {

            //                if (value.statusApproved === 1) {
            //                    console.log(value.employeeBasicInfoId);
            //                    console.log(CurrentEmployeeID);
            //                    datastatus = "<span class='bluetext'>Need to Approved</span>";
            //                    if (value.employeeBasicInfoId === CurrentEmployeeID) {
            //                        $("#with_approval").css({ display: "block" });
            //                        $("#no_approval").css({ display: "none" });
            //                    } else {
            //                        $("#with_approval").css({ display: "none" });
            //                        $("#no_approval").css({ display: "block" });
            //                    }
            //                } else if (value.statusApproved === 2) {
            //                    datastatus = "<span>Waiting for Approval</span>";
            //                } else if (value.statusApproved === 3) {
            //                    datastatus = "<span class='greentext'>Approved</span>";
            //                } else if (value.statusApproved === 4) {
            //                    datastatus = "<span class='redtext'>Reject</span>";
            //                } else {
            //                    datastatus = "<span class='bluetext'>Draft</span>";
            //                }
            //                if (value.statusApproved !== 3) {
            //                    ctn = '<center> - </center>';
            //                } else {
            //                    ctn = '<center>' + value.createdOn + '</center>';
            //                }

            //                if (value.job_title_name === null || value.job_title_name === "") {
            //                    ttl = "Unset Jobtitle";
            //                } else {
            //                    ttl = value.job_title_name;
            //                }
            //                AddRowTableApproval(count, ttl, value.employee_full_name, '<center>' + datastatus + '</center >', ctn);
            //                count = count + 1;
            //            });
            //        }
            //    });
            //});
        });

    }
});

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

function formatDecimal(total) {
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-" : ' ') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}

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
        $('#tmpmpp').empty();
        var tmpmpno = 0;
        $.each(MppNewRealData, function (index, value) {
            ttl = parseInt(ttl) + parseInt(value.mppbudget);
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle]" value="' + value.jobtitle + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][ttf]" value="' + value.ttf + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][status]" value="' + value.status + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][mppbudget]" value="' + value.mppbudget + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][grade]" value="' + value.grade + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][project]" value="' + value.project + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][desc]" value="' + value.desc + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle_name]" value="' + value.jobtitle_name + '"  >');
            $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][budget_souce]" value="' + value.budget_souce + '"  >');
            tmpmpno++;
        });
        AddRowTotalBudget(ttl);
    }
    $('table#MppDetailList tr#'+id+'').remove();
}

function Deleteaja(id) {
    $('table#MppDetailList tr#' + id + '').remove();
}

function DeleteajaApproval(id) {
    $('table#MppListApprover tr#' + id + '').remove();
}

function AddRowTable(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid, budget_souce) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + jobtitle + "</td><td>" + tff + "</td><td>" + status + "</td><td style='text-align:right'>" + formatDecimal(mppbudget) + "</td><td style='display: none;'>" + grade + "</td><td style='display: none;'>" + project + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteLogic(" + acak + ")'>delete</a></center></td><td style='display: none;'>" + desc + "</td><td style='display: none;'>" + jobtitleid + "</td><td style='display: none;'>" + budget_souce + "</td></tr>";
    $("table#MppDetailList tbody").append(markup);   

    $('#btn-submit').prop("disabled", false);
    $('#btn-submit').css("cursor", "");
    $('#btn-save').attr("href", "#");
    $('#btn-save').css("pointer-events", "");
    $('#btn-save').css("cursor", "");
}

function AddRowTableApproval(count, job_title_name, employee_full_name) {
    var acak = randomIntFromInterval(1, 999);
    if (job_title_name === null) {
        job_title_name = "Undefined Jobtitle";
    }
    var markup = "<tr id=" + acak + "><td>" + count + "</td><td>" + job_title_name + "</td><td>" + employee_full_name + "</td></tr>";
    $("table#MppListApprover tbody").append(markup);
}

function CheckAndAddRow(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid, budget_souce) {

    //alert('ID Value budget_source = ' + $('#budget_source').val());
    //alert('ID Value project_name = ' + $('#project_name').val());

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
        if (budget_souce === "PROJECT" && project === "") {
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
                "jobtitle_name": jobtitle,
                "budget_souce": budget_souce
            };
            MppNewRealData.push(tmpData);
            $('#tmpmpp').empty();
            var tmpmpno = 0;
            $.each(MppNewRealData, function (index, value) {

                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle]" value="' + value.jobtitle + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][ttf]" value="' + value.ttf + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][status]" value="' + value.status + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][mppbudget]" value="' + value.mppbudget + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][grade]" value="' + value.grade + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][project]" value="' + value.project + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][desc]" value="' + value.desc + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][jobtitle_name]" value="' + value.jobtitle_name + '"  >');
                $('#tmpmpp').append('<input type="hidden" name="dataDetailMpp[' + tmpmpno + '][budget_souce]" value="' + value.budget_souce + '"  >');
                tmpmpno++;
            });

            AddRowTable(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid, budget_souce);
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
    $("table#MppDetailList tbody").append(markup);

    $('#btn-submit').prop("disabled", true);
    $('#btn-submit').css("cursor", "default");
    $('#btn-save').removeAttr('href');
    $('#btn-save').css("pointer-events", "none");
    $('#btn-save').css("cursor", "default");

}

function AddRowTotalBudget(total) {
    var markup = "<tr id='total_budget'><td colspan='2'>&nbsp;</td><td>TOTAL BUDGET</td ><td style='text-align:right;'>" +formatDecimal(total)+"</td></tr>";
    $("table#MppDetailList tbody").append(markup);
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
                    "jobtitle_name": $(tr).find('td:eq(0)').text(),
                    "budget_souce": $(tr).find('td:eq(9)').text()
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

function budget_souce_change(id) {
    
    if (id === "CORPORATE") {
        $("#project_name").attr("disabled", true);
    } else {
        $("#project_name").attr("disabled", false);
    }

}

function GetTodayDate() {
    var tdate = new Date();
    var dd = tdate.getDate(); //yields day
    var MM = tdate.getMonth(); //yields month
    var yyyy = tdate.getFullYear(); //yields year
    var currentDate = dd + "-" + (MM + 1) + "-" + yyyy;

    return currentDate;
}

function Save() {
    saveType = '1';
}





