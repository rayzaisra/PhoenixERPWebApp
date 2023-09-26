var webUrl = WEB + "/Hris/TrainingRequisition";
var ajaxUrl = API + "Hris/TrainingRequisition";
var ajaxUrlCreate = ajaxUrl + "/CreateWithDetail";
var xform = "TrainingRequisitionFormCreate";
var ajaxUrlTrainingList = API + "Hris/Training/List";

var ajaxUrlTableList = API + "Hris/EmployeeBasicInfo/List";
var ajaxUrlEmployee = API + "General/BusinessUnit";
var ajaxUrlListEmployee = ajaxUrlEmployee + "/ListWithParent";


var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List";


var tableList = "EmployeeList";
var formSearch = "EmployeeListFormSearch";
var modalSearch = "EmployeeListModalSearch";
var pageheader = "";




$(function () {
    
  
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                var data = {
                    refId: response.data.id,
                    detailLink: "/Hris/TrainingRequisition/Approval/" + response.data.id,
                    tName: "hr.training_requisition",
                    menuId: "93a774ff-75b1-4e7d-a6d1-00524c1fb0c9",
                    IdTemplate: $("#template_approval_id").val()
                };
                saveTemplateApprovalHris(data, webUrl);
                //redirecttolink(webUrl);
            } else {
                DangerNotif(response.message);
            }
        });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);

    $('#download_file').setuploadbox({});

    ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Training Request", Search_TemplateName: "Training Request IRIS / BRANDCOM" }, function (response) {

        response = parseJson(response);
        DeleteajaApproval('no_data');
        var count = 1;
        var ttl = '';
        $("#TrainingListApprover > tbody").html("");
        $.each(response.rows[0].listTmUserApproval, function (index, value) {
            if (value.job_title_name === null || value.job_title_name === "") {
                ttl = "Unset Jobtitle";
            } else {
                ttl = value.job_title_name;
            }
            AddRowTableApproval(count, ttl, value.employee_full_name);
            count = count + 1;
        });
        $("#template_approval_id").val(response.rows[0].id);

    });

    
  
});

function AddRowTableApproval(no, title, nama) {
    var acak = randomIntFromIntervalApproval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td></tr>";
    $("#TrainingAprovalApproverList tbody").append(markup);
}


function DeleteajaApproval(id) {
    $('table#TrainingAprovalApproverList tr#' + id + '').remove();
}

function randomIntFromIntervalApproval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}


