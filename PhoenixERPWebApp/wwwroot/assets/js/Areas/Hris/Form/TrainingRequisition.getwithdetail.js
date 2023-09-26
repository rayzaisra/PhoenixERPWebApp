var ajaxUrl = API + "Hris/TrainingRequisition";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/GetWithDetial"; 
var ajaxUrlUpdateStatus = ajaxUrl + "/UpdateStatus";
var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";

var tableList = "TrainingRequisitionApprovalList";
var formSearch = "TrainingRequisitionFormSearch";
var modalSearch = "TrainingRequisitionModalSearch";
var pageheader = "";



$(function () {
   // fnTrainingRequisition.generatelist();
    initlanguage(lang_hr);

    var xid = $("#id").val();
  
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            $('#training_id').val(response.data.training_id);
            $('#training_name').val(response.data.traning_name);
            $('#training_type').val(response.data.training_type);
            $('#training_cost_value').val("Rp. " + response.data.training_cost);
            $('#training_cost').val(response.data.training_cost);
            $('#purpose_start_date').val(response.data.purpose_start_date);
            $('#purpose_end_date').val(response.data.purpose_end_date);
            $('#venue').val(response.data.venue);
            $('#notes').val(response.data.notes);


            if (parseInt(response.data.training_cost) > 15000000) {
                $("#optyes").prop("checked", true);
            } else {
                $("#optno").prop("checked", true);
            }

            $("#dt_basic_job_posting > tbody").html("");
            $.each(response.data.listParticipantTraining, function (index, value) {

                AddRowTableParticipant(value.name_employee, value.job_title_name, value.job_grade_name, value.participantBusinessUnit.name);
            });
        });
    }


});



function AddRowTableParticipant(name, title, grade, division) {
    var acak = randomIntFromIntervalApproval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + name + "</td><td>" + title + "</td><td>" + grade + "</td><td>" + division + "</td></tr>";
    $("#dt_basic_job_posting tbody").append(markup);
}


function DeleteajaParticipant(id) {
    $('table#dt_basic_job_posting tr#' + id + '').remove();
}

function randomIntFromIntervalApproval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function SendtoFinance() {
    var text = "Do you want to Send this Data to Finance?";
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
            ajaxPost(ajaxUrlUpdateStatus, { id: $('#id').val(), status: 1 }, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Send Data to Finance Sucess");
                    window.location.href = WEB + "/Hris/TrainingRequisition";
                } else {
                    DangerNotif(response.message);
                }
            });
        }
    });

}

 

