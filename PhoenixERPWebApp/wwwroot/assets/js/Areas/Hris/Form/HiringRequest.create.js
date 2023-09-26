//var API_SUFFIX = API.substr(API.length - 1, API.length);
//if (API_SUFFIX === "/")
//	API = API.substr(0, API.length - 1);

//var WEB_SUFFIX = WEB.substr(WEB.length - 1, WEB.length);
//if (WEB_SUFFIX === "/")
//	WEB = WEB.substr(0, WEB.length - 1);

var webUrl = WEB + "/Hris/HiringRequest";
var ajaxUrl = API + "Hris/HiringRequest";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "form-vendor";
var UrlGetRequestData = API + "Hris/HiringRequest/GetDataRequester";
var ajaxUrlListJobTitle = API + "Hris/JobTitle/ListTitleInsideBu/";
var ajaxUrlListJobTitleAll = API + "Hris/JobTitle/List/";
var ajaxUrlListHiringCheck = API + "Hris/HiringRequest/GetListHiringMppDetail";
var ajaxUrlMppDetailGet = API + "Hris/Mppdetail/Get";
var ajaxUrlHiringRequestDetailGet = API + "Hris/EmployeeBasicInfo/GetWithDetail";
var ajaxUrlGetGradeData = API + "Hris/HiringRequest/GetGradeDataInBusinessUnit";
var VAR_BUGETED = "Hiring Request Budgeted Nava";
var VAR_UNBUGEDTED = "Hiring Request Unbudgeted Nava";
var VAR_REPLACEMENT = "Replacement";
var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List";
var tableList = "HiringRequestListApprover";
var UrlSessionData = API + "employeeInfo/getByUserLogin";
var ajaxUrlGetEmployeBasicWithDevId = API + "Hris/EmployeeBasicInfo/GetWithDivisionId";
var saveType = "0";
var ajaxUrlListGet = ajaxUrl + "/GetWithDetil"; 
var ajaxUrlListGetBusinessUnit = API + "General/BusinessUnit/rootparent"; 
var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";

$(function () {
    initlanguage(lang_hr);
    $('.dtpicker').datetimepicker();
    var xid = $('#id').val();
    ajaxGet(UrlSessionData, {}, function (responseSession) {
        responseSession = parseJson(responseSession);
        //$("#BusinessUnitId").val(responseSession.data.businessUnitId);
        $("#BusinessUnitId").val(responseSession.data.divisionId);
        $("#EmployeeId").val(responseSession.data.employeeId);
        $("#requester_employee_basic_info_id").val(responseSession.data.employeeId);

        if (responseSession.data !== null) {
            ajaxGet(UrlGetRequestData + "/" + responseSession.data.employeeId, {}, function (resp) {
                if (resp.data === null) {
                    DangerNotif("No Data Avaliable");
                } else {
                    parseJson(resp);
                    console.log("Response = " + JSON.stringify(resp));
                    if (xid === "") {
                        $('#code').val("REQ" + getDateNow() + randomIntFromInterval(1, 9999999));
                        $('#requester').val(resp.data.requester_name);
                        $('#job_title').val(resp.data.job_title_name);
                        $('#basic_info_grade_tittle').val(resp.data.job_title_name);
                        $('#division').val(resp.data.business_unit_name);

                        if ($('#BusinessUnitId').val() === "be2050a0-7402-4e2d-b4fb-2e4c4040e9be") {
                            $('#select_job_title').setcombobox({
                                data: { 'rows': 100},
                                url: ajaxUrlListJobTitleAll,
                                searchparam: 'Search_title_name',
                                labelField: 'title_name',
                                valueField: 'id',
                                searchField: 'title_name'
                            });
                        } else {
                            $('#select_job_title').setcombobox({
                                data: { 'rows': 100, Search_business_unit_id: $('#BusinessUnitId').val() },
                                url: ajaxUrlListJobTitle,
                                searchparam: 'Search_title_name',
                                labelField: 'title_name',
                                valueField: 'id',
                                searchField: 'title_name'
                            });
                        }
                    }
                    else {
                        
                        ajaxGet(ajaxUrlListGet + "/" + xid, {}, function (response) {
                            response = parseJson(response);
                          
                            $('#code').val(response.data.code);
                            $('#requester_name').val(response.data.requester_name);
                            $('#requester_employee_basic_info_id').val(response.data.requester_employee_basic_info_id);
                            $('#job_title_name').val(response.data.jobtitile.title_name);
                            $('#job_title_id').val(response.data.jobtitile.id);
                            $('#job_title_requester').val(response.data.job_title_requester);
                            $('#job_title_name').val(response.data.jobtitile.title_name);
                            $('#select_job_title').val(response.data.jobtitile.id).trigger('change');
                            $('#select_job_title').append('<option value="' + response.data.jobtitile.id + '">' + response.data.jobtitile.title_name + '</option>');
                            $('#requester').val(resp.data.requester_name);
                            $('#job_title').val(resp.data.job_title_name);
                            $('#basic_info_grade_tittle').val(resp.data.job_title_name);
                            $('#division').val(resp.data.business_unit_name);
                            $("#mpp_id").val(response.data.mpp_id);
                            if (response.data.hiring_request_type_id === "Budgeted") {
                                $('#hiring_request_type_id').val(VAR_BUGETED);
                                $('#inner_label_budgeted').html('<strong>Budgeted</strong>');

                                ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Hiring Request", Search_BusinessUnitId: $("#BusinessUnitId").val(), Search_Budgeted: VAR_BUGETED }, function (response) {
                                    response = parseJson(response);
                                    console.log(response);
                                    if (response.rows.length > 0) {
                                        Deleteaja('no_data');
                                        var count = 1;
                                        //$("#HiringRequestListApprover > tbody").html("");
                                        //$.each(response.rows[0].listTmUserApproval, function (index, value) {
                                        //    AddRowTable(count, value.job_title_name, value.employee_full_name);
                                        //    count = count + 1;
                                        //});
                                        $("#template_approval_id").val(response.rows[0].id);
                                    } else {
                                        swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                                        setTimeout(
                                            function () {
                                                //do something special
                                                window.location.href = WEB + "/hris/Mpp";
                                            }, 2000);
                                    }

                                });
                            } else if (response.data.hiring_request_type_id === "Unbudgeted") {
                                $('#hiring_request_type_id').val(VAR_UNBUGEDTED);
                                $('#inner_label_budgeted').html('<strong>Unbudgeted</strong>');

                                ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Hiring Request", Search_BusinessUnitId: $("#BusinessUnitId").val(), Search_Budgeted: VAR_UNBUGEDTED }, function (response) {
                                    response = parseJson(response);
                                    console.log(response);
                                    if (response.rows.length > 0) {
                                        Deleteaja('no_data');
                                        //var count = 1;
                                        //$("#HiringRequestListApprover > tbody").html("");
                                        //$.each(response.rows[0].listTmUserApproval, function (index, value) {
                                        //    AddRowTable(count, value.job_title_name, value.employee_full_name);
                                        //    count = count + 1;
                                        //});
                                        $("#template_approval_id").val(response.rows[0].id);
                                    } else {
                                        swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                                        setTimeout(
                                            function () {
                                                //do something special
                                                window.location.href = WEB + "/hris/Mpp";
                                            }, 2000);
                                    }

                                });
                            } else {
                                $('#inner_label_budgeted').html('<strong>Replacement</strong>');
                                $('#tff_head').hide();
                                $('#mppbudget_head').html("Budget");
                                $('#employeestatus_head').html("Employee Replaced");
                            }

                            if (response.data.hiring_request_type_id === "Replacement") {
                                $('#hiring_request_type_id').val(VAR_REPLACEMENT);
                                $('#checkbox_replacement').prop("checked", true);
                                budgted_table = "<tr id=" + response.data.id + "><td>" + response.data.jobtitile.title_name + "</td><td>" + response.data.jobGrade.grade_name + "</td><td>" + response.data.replacedEmployee.employee_replaced_name + "</td><td>" + response.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response.data.mpp_budged) + "</td><td style=\"text-align:center;vertical-align:middle\">" + response.data.date_fulfilment.split('T')[0] + "</td></tr>";
                            } else {
                                budgted_table = "<tr id=" + response.data.id + "><td>" + response.data.jobtitile.title_name + "</td><td>" + response.data.jobGrade.grade_name + "</td><td>" + response.data.ttf + "</td><td>" + response.data.employment_status + "</td><td>" + response.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response.data.mpp_budged) + "</td><td style=\"text-align:center;vertical-align:middle\">" + response.data.date_fulfilment.split('T')[0] + "</td></tr>";
                            }

                            $("#dt_basic tbody").append(budgted_table);
                            ajaxGet(ajaxUrlListGetBusinessUnit + "/" + response.data.division.id, {}, function (response) {
                                response = parseJson(response);
                                //console.log(response);
                                $('#division_id').val(response.data.parent.id);
                                $('#division_name').val(response.data.parent.unit_name);
                                $('#business_unit_id').val(response.data.id);
                                $('#business_unit_name').val(response.data.unit_name);
                            });
                            ajaxGet(UrlSessionData, {}, function (responseSession) {
                                responseSession = parseJson(responseSession);
                                console.log(responseSession);
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

                            });

                            if (response.data.status !== "1" || response.data.status !== "2") {
                                $('#code').prop("readonly", true);
                                $('#requester_name').prop("readonly", true);

                                $('#job_title_requester').prop("readonly", true);
                                $('#division_name').prop("readonly", true);
                                $('#business_unit_name').prop("readonly", true);
                                $('#job_title_name').prop("readonly", true);
                                $('#requester_name').prop("readonly", true);
                            }

                            //process();

                            $('#open_div').show();

                            $('#tmpmppDetail').empty();
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][jobtitle]" value="' + response.data.jobtitile.id + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][ttf]" value="' + response.data.ttf + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][status]" value="' + response.data.employment_status + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][mppbudget]" value="' + response.data.mpp_budged + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][grade]" value="' + response.data.jobGrade.id + '"  >');
                            //$('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][project]" value="' + response.data.budget_souce  + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][desc]" value="' + response.data.detail_candidate_qualification + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][descresponsibilites]" value="' + response.data.detail_candidate_responsibilites + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][jobtitle_name]" value="' + response.data.jobtitile.title_name + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][budget_souce]" value="' + response.data.budget_souce + '"  >');
                            $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][dff]" value="' + response.data.date_fulfilment.split('T')[0] + '"  >');
                            if (response.data.dataDetailMpp !== null) {
                                $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][Id]" value="' + response.data.dataDetailMpp[0].id + '"  >');
                            }
                            
     
                            $('#btn-submit').prop("disabled", false);
                            $('#btn-submit').css("cursor", "");
                            $('#btn-save').attr("href", "#");
                            $('#btn-save').css("pointer-events", "");
                            $('#btn-save').css("cursor", "");

                        });
                    }
                }
            });
        } else {
            swal('Information', 'Login User Data Not Valid!', 'info');
            setTimeout(
                function () {
                    //do something special
                    window.location.href = WEB + "/Core/Authentication?status=Logout";
                }, 2000);
        }

        if ($('#tmpmppDetail').html() === "") {
            $('#btn-submit').prop("disabled", true);
            $('#btn-submit').css("cursor", "default");
            $('#btn-save').removeAttr('href');
            $('#btn-save').css("pointer-events", "none");
            $('#btn-save').css("cursor", "default");
        }

        

    });

    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            if (saveType === '0') {
                var data = {
                    refId: response.data.id,
                    detailLink: "/Hris/HiringRequest/Approval/" + response.data.id,
                    tName: "hr.HiringRequest",
                    menuId: "4ed5d07d-ff65-415b-873e-429b4eadfa3a",
                    IdTemplate: $("#template_approval_id").val(),
                    NotificationType: "hris.hiringrequest.need-approval",
                    Module:"HRIS"
                };
                saveTemplateApprovalHris(data, webUrl);
            }
            else {
                 window.location = WEB + "/hris/HiringRequest";
            }
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);

    var rep_table = "";
    $('#checkbox_replacement').click(function () {
        if ($(this).is(':checked')) {
            $("#label_replacement").css({ display: "block" });
            $("#budgeted_div").css({ display: "none" });
            $("#label_budgeted").css({ display: "block" });
            $('#inner_label_budgeted').html('<strong>Replacement</strong>');
            $('#this_title_is').html('&nbsp');
            $("#replacement_div").css({ display: "block" });
            $('#hiring_request_type_id').val(VAR_REPLACEMENT);
            var emps = "";
            ajaxGet(ajaxUrlGetEmployeBasicWithDevId + "/" + $("#BusinessUnitId").val(), {}, function (response) {
                response = parseJson(response);
                console.log(response);
                $("#table_replacement > tbody").html("");
                rep_table = "";
                $.each(response.rows, function (index, value) {
                    // console.log(value);
                    if (value.employee_status === null || value.employee_status === "") {
                        emps = "Unset Status Employee";
                    } else {
                        emps = value.employee_status;
                    }
                    rep_table = rep_table + "<tr id=" + value.id + "><td>" + value.name_employee + "</td><td>" + value.job_title + "</td><td>" + value.grade + "</td><td>" + emps + "</td><td style=\"text-align:center;vertical-align:middle\"><label class=\"m-radio m-radio--state-success\"><input  type=\"radio\" name=\"radioReplacement-" + index +"\"data-toggle=\"modal\" data-target=\"#HiringRequestModalDetailReplacement\" onclick=\"clearModalDetialReplacement('" + value.id + "','" + index + "')\"><span></span></label></td></tr>";
                    //console.log(budgted_table);
                });
                $("#table_replacement tbody").append(rep_table);
                ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Hiring Request", Search_BusinessUnitId: $("#BusinessUnitId").val(), Search_Budgeted: VAR_BUGETED }, function (response) {
                    console.log("Result Response = " + response);
                    response = parseJson(response);
                    console.log("Result Response After Parse = " + response);
                    if (response.rows.length > 0) {
                        Deleteaja('no_data');
                        //var count = 1;
                        //$("#HiringRequestListApprover > tbody").html("");
                        //$.each(response.rows[0].listTmUserApproval, function (index, value) {
                        //    AddRowTable(count, value.job_title_name, value.employee_full_name);
                        //    count = count + 1;
                        //});
                        $("#template_approval_id").val(response.rows[0].id);
                    } else {
                        //swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                        //setTimeout(
                        //    function () {
                        //        //do something special
                        //        window.location.href = WEB + "/hris/Mpp";
                        //    }, 2000);
                    }

                });
            });
        } else {
            $("#budgeted_div").css({ display: "block" });
            $("#label_replacement").css({ display: "block" });
            $("#label_budgeted").css({ display: "block" });
            $('#inner_label_budgeted').html('<strong>Budgeted</strong>');
            $('#this_title_is').html('This Title is ');
            $("#replacement_div").css({ display: "none" });
            $('#hiring_request_type_id').val(VAR_BUGETED);

        }
    });

});

function Save() {
    saveType = '1';
    $('#status').val('0');
}

function process() {
    var budgted_table = "";
    $("#budgeted_div").css({ display: "none" });
    $("#unbudgeted_div").css({ display: "none" });
    $("#label_budgeted").css({ display: "none" });
    $("#table_budeted tbody").empty();
    $('#job_title_unbudgeted').empty();
 
    ajaxPost(ajaxUrlListHiringCheck, { business_unit_id: $('#BusinessUnitId').val(), job_tittle_id: $('#select_job_title').val() }, function (response) {
        response = parseJson(response);
        //console.log("Hasil Response = " + response.success);
        console.log("Hasil Response recordsTotal = " + response.recordsTotal);

        if (response.success) {
            budgted_table = '';
            if (response.recordsTotal > 0) {
                $("#budgeted_div").css({ display: "block" });
                $("#label_replacement").css({ display: "block" });
                $("#label_budgeted").css({ display: "block" });
                $('#inner_label_budgeted').html('<strong>Budgeted</strong>');
                $.each(response.rows, function (index, value) {
                    budgted_table = budgted_table + "<tr id=" + value.id + "><td>" + value.job_title_name + "</td><td>" + value.job_grade_name + "</td><td>" + value.ttf + "</td><td>" + value.employment_status + "</td><td>" + value.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(value.mpp_budged) + "</td><td data-field-id=\"radioBudget-" + index +"\" style=\"text-align:center;vertical-align:middle\"><label class=\"m-radio m-radio--state-success\"><input  type=\"radio\" data-toggle=\"modal\" data-target=\"#HiringRequestModalDetail\" onclick=\"clearModalDetialBudgeted(this,'" + value.id + "','" + index + "')\" id=\"radioBudget-" + index +"\" name='radioBudget'><span></span></label></td></tr>";
                    //console.log(budgted_table);
                });
                $("#table_budeted tbody").append(budgted_table);
                $('#hiring_request_type_id').val(VAR_BUGETED);
                ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Hiring Request", Search_BusinessUnitId: $("#BusinessUnitId").val(), Search_Budgeted: VAR_BUGETED }, function (response) {
                    response = parseJson(response);
                    console.log(response);
                    if (response.rows.length > 0) {
                        Deleteaja('no_data');
                        //var count = 1;
                        //$("#HiringRequestListApprover > tbody").html("");
                        //$.each(response.rows[0].listTmUserApproval, function (index, value) {
                        //    AddRowTable(count, value.job_title_name, value.employee_full_name);
                        //    count = count + 1;
                        //});
                        $("#template_approval_id").val(response.rows[0].id);
                    } else {
                        swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                        setTimeout(
                            function () {
                                //do something special
                                window.location.href = WEB + "/hris/Mpp";
                            }, 2000);
                    }

                });
                // HrHiringRequestCreate.generatelist();
                $('#open_div').empty();
                $('#open_div').hide();
            } else {
                $("#label_replacement").css({ display: "none" });
                $("#unbudgeted_div").css({ display: "block" });
                $("#label_budgeted").css({ display: "block" });
                $('#inner_label_budgeted').html('<strong>Unbudgeted</strong>');
                $('#hiring_request_type_id').val(VAR_UNBUGEDTED);
                $('#job_title_unbudgeted').val($('#select_job_title').val()).trigger('change');
                $('#open_div').empty();
                $('#open_div').hide();
                //if ($("#BusinessUnitId").val() == "3062e5d2-96d5-4344-a5c9-9b6d2eca24bd") {
                //    ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Hiring Request", Search_BusinessUnitId: $("#BusinessUnitId").val(), Search_TemplateName: "Hiring Request Unbudgeted IRIS A / BRANDCOM" }, function (response) {
                //        response = parseJson(response);
                //        console.log(response);
                //        if (response.rows.length > 0) {
                //            Deleteaja('no_data');
                //            var count = 1;
                //            $("#HiringRequestListApprover > tbody").html("");
                //            $.each(response.rows[0].listTmUserApproval, function (index, value) {
                //                AddRowTable(count, value.job_title_name, value.employee_full_name);
                //                count = count + 1;
                //            });
                //            $("#template_approval_id").val(response.rows[0].id);
                //        } else {
                //            swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                //            setTimeout(
                //                function () {
                //                    //do something special
                //                    window.location.href = WEB + "/hris/Mpp";
                //                }, 2000);
                //        }

                //    });
                //} else {

                ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Hiring Request", Search_BusinessUnitId: $("#BusinessUnitId").val(), Search_Budgeted: VAR_UNBUGEDTED }, function (response) {
                    response = parseJson(response);
                    console.log(response);
                    if (response.rows.length > 0) {
                        Deleteaja('no_data');
                        //var count = 1;
                        //$("#HiringRequestListApprover > tbody").html("");
                        //$.each(response.rows[0].listTmUserApproval, function (index, value) {
                        //    AddRowTable(count, value.job_title_name, value.employee_full_name);
                        //    count = count + 1;
                        //});
                        $("#template_approval_id").val(response.rows[0].id);
                    } else {
                        swal('Information', 'Please Create Template Approval for this Division First !', 'info');
                        setTimeout(
                            function () {
                                //do something special
                                window.location.href = WEB + "/hris/Mpp";
                            }, 2000);
                    }

                });

                //}

                $('#job_title_unbudgeted').append("<option value=" + $('#select_job_title').val() + ">" + $("#select_job_title option:selected").text() + "</option>");
                $('#job_title_unbudgeted').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListJobTitle,
                    searchparam: 'Search_title_name',
                    labelField: 'title_name',
                    valueField: 'id',
                    searchField: 'title_name'
                });

            }
        } else {
            DangerNotif(response.message);
        }
    });


}

function clearModalDetialBudgeted(obj,id, index) {

    //$('#table_budeted > tbody').find('tr').each(function () {
    //    var radio = $(this).find('td[data-field-id="radioBudget-' + index + '"]').is(":checked");
       
    //    alert("Value radio = " + radio);

    //});
   
    var xformdetil = "HiringRequestFormDetail";
    $('#indexBudget').val(index);
    ajaxGetJquery(ajaxUrlMppDetailGet + "/" + id, {}, function (response) {
        if (response.data === null) {
            DangerNotif("No Data Avaliable");
        } else {
            parseJson(response);
            console.log(response.data);
            var optionsForm = GetOptionsForm(function () {
                var xret = $("#" + xform).valid();
                if (xret) startprocess();
                return xret;
            });
            InitForm(xformdetil, optionsForm);
            $("#time_to_fullfill").val(response.data.ttf);
            $("#employment_status").val(response.data.employment_status);
            $("#job_title_nm").val(response.data.job_title_name);
            $("#budget_source").val(response.data.budget_souce);
            $("#Job_Grade").val(response.data.job_grade_name);
            $("#job_grade_idu").val(response.data.job_grade_id);
            $("#mppBudget").val(response.data.mpp_budged);
            $("#detail_spesification").val(response.data.desc);
            $("#job_title_id").val(response.data.job_tittle_id);
            $("#project_name").val(response.data.porject_name);
            $("#mpp_id").val(response.data.mpp_id);
            $("#Id").val(id);

            if ($('#tmpmppDetailAdded').html() !== "") {
                if ($("input[name='dataDetailMppAdded[" + index + "][dff]']").val() !== "") {
                    $('#dff').val($("input[name='dataDetailMppAdded[" + index + "][dff]']").val());
                    $('#detail_spesification').val($("input[name='dataDetailMppAdded[" + index + "][desc]']").val());
                    $('#detail_requirement').val($("input[name='dataDetailMppAdded[" + index + "][descresponsibilites]']").val());
                }
            }
        }
    });
}

function clearModalDetialReplacement(id, index) {
    var xformdetil = "HiringRequestFormDetailReplacement";
    $('#indexReplacement').val(index);
    ajaxGetJquery(ajaxUrlHiringRequestDetailGet + "/" + id, {}, function (response) {
        if (response.data === null) {
            DangerNotif("No Data Avaliable");
        } else {
            parseJson(response);
            console.log(response.data);
            var optionsForm = GetOptionsForm(function () {
                var xret = $("#" + xform).valid();
                if (xret) startprocess();
                return xret;
            });
            InitForm(xformdetil, optionsForm);
            if (response.data.employement_status_name === null || response.data.employement_status_name === "") {
                $("#employment_statusr").val("Unset Status Employee");
            }
            $("#employment_statusr").val(response.data.employement_status_name);
            $("#job_title_nmr").val(response.data.bussinesUnitJobLevel.job_title_name);
            $("#Job_Grader").val(response.data.bussinesUnitJobLevel.job_grade_name);
            $("#job_grade_idur").val(response.data.bussinesUnitJobLevel.job_grade_id);
            $("#job_title_idr").val(response.data.bussinesUnitJobLevel.job_title_id);

            $("#employee_name_replace").val(response.data.name_employee);
            $("#employee_id_replace").val(response.data.id);

            $("#mppBudgetr").val("90000000");
            $("#porject_namer").val("");

            if ($('#tmpmppDetailAdded').html() !== "") {
                if ($("input[name='dataDetailMppAdded[" + index + "][dff]']").val() !== "") {
                    $('#porject_namer').val($("input[name='dataDetailMppAdded[" + index + "][project]']").val());
                    $('#budget_sourcer').val($("input[name='dataDetailMppAdded[" + index + "][status]']").val());
                    $('#mppBudgetr').val($("input[name='dataDetailMppAdded[" + index + "][mppbudget]']").val());
                    $('#dffr').val($("input[name='dataDetailMppAdded[" + index + "][dff]']").val());
                    $('#detail_spesificationr').val($("input[name='dataDetailMppAdded[" + index + "][desc]']").val());
                }
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

function formatDecimal(total) {
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-" : ' ') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function getDateNow() {
    var d = new Date();
    var strDate = d.getFullYear() + (d.getMonth() + 1) + d.getDate();

    return strDate;
}

function SaveDetilReplacement() {
    var index = $('#indexReplacement').val();
    var indexZero = '0';

    if ($('#dffr').val() !== "" && $('#detail_requirementr').val() !== "") {
        $('#tmpmppDetail').empty();
        $('#tmpmppDetailAdded').empty();
        $('input[type=radio]').prop("checked", false);

        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][jobtitle]" value="' + $('#job_title_idr').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][status]" value="' + $('#employment_statusr').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][mppbudget]" value="' + $('#mppBudgetr').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][grade]" value="' + $('#job_grade_idur').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][project]" value="' + $('#porject_namer').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][desc]" value="' + $('#detail_spesificationr').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][jobtitle_name]" value="' + $('#job_title_nmr').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][budget_souce]" value="' + $('#budget_sourcer').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][dff]" value="' + $('#dffr').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][Id]" value="' + $('#Idr').val() + '"  >');

        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][jobtitle]" value="' + $('#job_title_idr').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][status]" value="' + $('#employment_statusr').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][mppbudget]" value="' + $('#mppBudgetr').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][grade]" value="' + $('#job_grade_idur').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][project]" value="' + $('#porject_namer').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][desc]" value="' + $('#detail_spesificationr').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][jobtitle_name]" value="' + $('#job_title_nmr').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][budget_souce]" value="' + $('#budget_sourcer').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][dff]" value="' + $('#dffr').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][Id]" value="' + $('#Idr').val() + '"  >');


        $('#replacement_employee_basic_info_idr').val($('#employee_id_replace').val());
        $('#HiringRequestModalDetailReplacement').modal('toggle');

        $('input[name=radioReplacement-' + index + ']').prop("checked", true);
        //console.log($('#tmpmppDetail').val());
        SuccessNotif(t_createsuccess);

        $('#btn-submit').prop("disabled", false);
        $('#btn-submit').css("cursor", "");
        $('#btn-save').attr("href", "#");
        $('#btn-save').css("pointer-events", "");
        $('#btn-save').css("cursor", "");

    } else {
        DangerNotif('Please Insert Date of fullfillment or detail requirement first!');
        return false;
    }
}

function SaveDetilBudget() {
    var index = $('#indexBudget').val();
    var indexZero = '0';
    if ($('#dff').val() !== "" && $('#detail_requirement').val() !== "") {
        $('#tmpmppDetail').empty();
        $('#tmpmppDetailAdded').empty();
        $('input[type=radio]').prop("checked", false);
      
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][jobtitle]" value="' + $('#job_title_id').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][ttf]" value="' + $('#time_to_fullfill').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][status]" value="' + $('#employment_status').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][mppbudget]" value="' + $('#mppBudget').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][grade]" value="' + $('#job_grade_idu').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][project]" value="' + $('#project_name').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][desc]" value="' + $('#detail_spesification').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][descresponsibilites]" value="' + $('#detail_requirement').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][jobtitle_name]" value="' + $('#job_title_nm').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][budget_souce]" value="' + $('#budget_source').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][dff]" value="' + $('#dff').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[' + indexZero + '][Id]" value="' + $('#Id').val() + '"  >');

        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][jobtitle]" value="' + $('#job_title_id').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][ttf]" value="' + $('#time_to_fullfill').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][status]" value="' + $('#employment_status').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][mppbudget]" value="' + $('#mppBudget').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][grade]" value="' + $('#job_grade_idu').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][project]" value="' + $('#project_name').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][desc]" value="' + $('#detail_spesification').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][descresponsibilites]" value="' + $('#detail_requirement').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][jobtitle_name]" value="' + $('#job_title_nm').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][budget_souce]" value="' + $('#budget_source').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][dff]" value="' + $('#dff').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="dataDetailMppAdded[' + index + '][Id]" value="' + $('#Id').val() + '"  >');

        $('#HiringRequestModalDetail').modal('toggle');
       // $('input[name=radioBudget-' + index + ']').prop("checked", true);
        $('#radioBudget_' + index).prop("checked", true);
        
        //console.log($('#tmpmppDetail').val());
        SuccessNotif(t_createsuccess);

        $('#btn-submit').prop("disabled", false);
        $('#btn-submit').css("cursor", "");
        $('#btn-save').attr("href", "#");
        $('#btn-save').css("pointer-events", "");
        $('#btn-save').css("cursor", "");
    } else {
        DangerNotif('Please Insert Date of fullfillment or detail requirement first!');
        return false;
    }
}

function RemoveDetilBudget() {
    var index = $('#indexBudget').val();
    //$('input[name="dataDetailMpp[' + index + '][jobtitle]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][ttf]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][status]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][mppbudget]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][grade]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][project]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][desc]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][descresponsibilites]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][jobtitle_name]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][budget_souce]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][dff]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][Id]"]').remove();
    $('#tmpmppDetail').empty();
    if ($('#tmpmppDetail').html() === "") {
        $('#btn-submit').prop("disabled", true);
        $('#btn-submit').css("cursor", "default");
        $('#btn-save').removeAttr('href');
        $('#btn-save').css("pointer-events", "none");
        $('#btn-save').css("cursor", "default");
    }

    $('#dff').val("");
    $('#detail_spesification').val("");
    $('#detail_requirement').val("");

    $('input[name=radioBudget-' + index + ']').prop("checked", false);
    $('#HiringRequestModalDetail').modal('toggle');
    SuccessNotif("Remove Data Succesfully");
}

function RemoveDetilReplacement() {
    var index = $('#indexReplacement').val();
    //$('input[name="dataDetailMpp[' + index + '][jobtitle]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][ttf]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][status]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][mppbudget]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][grade]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][project]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][desc]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][descresponsibilites]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][jobtitle_name]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][budget_souce]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][dff]"]').remove();
    //$('input[name="dataDetailMpp[' + index + '][Id]"]').remove();
    $('#tmpmppDetail').empty();
    if ($('#tmpmppDetail').html() === "") {
        $('#btn-submit').prop("disabled", true);
        $('#btn-submit').css("cursor", "default");
        $('#btn-save').removeAttr('href');
        $('#btn-save').css("pointer-events", "none");
        $('#btn-save').css("cursor", "default");
    }

    $('#porject_namer').val("");
    $('#budget_sourcer').val("");
    $('#mppBudgetr').val("");
    $('#dffr').val("");
    $('#detail_spesificationr').val("");

    $('input[name=radioReplacement-' + index + ']').prop("checked", false);
    $('#HiringRequestModalDetailReplacement').modal('toggle');
    SuccessNotif("Remove Data Succesfully");
}

function SaveDetilUnBudget() {
    if ($('#dff_ub').val() !== "" && $('#detail_requirement_ub').val() !== "") {
        $('#tmpmppDetail').empty();
        $('#tmpmppDetailAdded').empty();
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][jobtitle]" value="' + $('#job_title_id_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][ttf]" value="' + $('#time_to_fullfill_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][status]" value="' + $('#employment_status_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][mppbudget]" value="' + $('#mppBudget_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][grade]" value="' + $('#job_grade_id_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][project]" value="' + $('#project_name_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][desc]" value="' + $('#detail_spesification_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][descresponsibilites]" value="' + $('#detail_requirement_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][jobtitle_name]" value="' + $('#job_title_name_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][budget_souce]" value="' + $('#budget_source_ub').val() + '"  >');
        $('#tmpmppDetail').append('<input type="hidden" name="dataDetailMpp[0][dff]" value="' + $('#dff_ub').val() + '"  >');
        $('#tmpmppDetailAdded').append('<input type="hidden" name="JobGradeName" value="' + $('#Job_Grade_ub').val() + '"  >');
        $('#HiringRequestModalDetailUnbugeted').modal('toggle');
        $('#btn-submit').prop("disabled", false);
        $('#btn-submit').css("cursor", "");
        $('#btn-save').attr("href", "#");
        $('#btn-save').css("pointer-events", "");
        $('#btn-save').css("cursor", "");
        SuccessNotif(t_createsuccess);
    } else {
        DangerNotif('Please Insert Date of fullfillment or detail requirement first!');
        return false;
    }
}

function setDataUnbudeted() {
    var xformdetil = "HiringRequestFormDetailUnbugeted";
    ajaxPost(ajaxUrlGetGradeData, { business_unit_id: $('#BusinessUnitId').val(), job_tittle_id: $('#select_job_title').val() }, function (response) {
        if (response.data === null) {
            DangerNotif("No Position Avalaible, Please Create Position First");
            return false;
        } else {
            $('#HiringRequestModalDetailUnbugeted').modal('show');
            parseJson(response);
            var optionsForm = GetOptionsForm(function () {
                var xret = $("#" + xform).valid();
                if (xret) startprocess();
                return xret;
            });
            InitForm(xformdetil, optionsForm);

            if ($('#tmpmppDetail').html() !== "") {
                $('#job_title_id_ub').val($("input[name='dataDetailMpp[0][jobtitle]']").val());
                $('#time_to_fullfill_ub').val($("input[name='dataDetailMpp[0][ttf]").val());
                $('#budget_source_ub').val($("input[name='dataDetailMpp[0][status]']").val());
                $('#mppBudget_ub').val($("input[name='dataDetailMpp[0][mppbudget]']").val());
                $('#job_grade_id_ub').val($("input[name='dataDetailMpp[0][grade]']").val());
                $('#project_name_ub').val($("input[name='dataDetailMpp[0][project]").val());
                $('#detail_spesification_ub').val($("input[name='dataDetailMpp[0][desc]']").val());
                $('#detail_requirement_ub').val($("input[name='dataDetailMpp[0][descresponsibilites]']").val());
                $('#job_title_name_ub').val($("input[name='dataDetailMpp[0][jobtitle_name]']").val());
                $('#budget_source_ub').val($("input[name='dataDetailMpp[0][budget_souce]").val());
                $('#dff_ub').val($("input[name='dataDetailMpp[0][dff]']").val());
                $('#Job_Grade_ub').val($("input[name='JobGradeName").val());
            } else {
                $('#job_title_id_ub').val($('#select_job_title').val());
                $('#job_title_name_ub').val($("#select_job_title option:selected").text());
                $('#Job_Grade_ub').val(response.data.job_grade_name);
                $('#job_grade_id_ub').val(response.data.job_grade_id);
            }


        }

    });

}

function AddRowTable(no, title, nama) {
    var acak = randomIntFromInterval(1, 999);
    if (title === null) {
        title = "Undefined Jobtitle";
    }
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td></tr>";
    $("#HiringRequestListApprover tbody").append(markup);
}

function Deleteaja(id) {
    $('table#HiringRequestListApprover tr#' + id + '').remove();
}

