var ajaxUrl = API + "Hris/TrainingRequisition";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/GetWithDetial"; 
var ajaxUrlUpdateStatus = ajaxUrl + "/UpdateStatus";
var ajaxUrlProcessPayment = ajaxUrl + "/ProcessPaymentandAdministration";
var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";

var tableList = "TrainingRequisitionApprovalList";
var formSearch = "TrainingRequisitionFormSearch";
var modalSearch = "TrainingRequisitionModalSearch";
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
}
var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }    ,{
        "data": "end_date",
        "title": "end_date",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "preferlocation",
        "title": "preferlocation",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "start_date",
        "title": "start_date",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "trainingname",
        "title": "trainingname",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "training_description",
        "title": "training_description",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/Hris/TrainingRequisition/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnTrainingRequisition.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnTrainingRequisition = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
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
                ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnTrainingRequisition.reloadlist();
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
   // fnTrainingRequisition.generatelist();
    initlanguage(lang_hr);

    var xid = $("#id").val();
  
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
     
            $('#request_date').val(response.data.created_on);
            $('#request_status').val("Process Finance");
            $('#requester').val(response.data.detialTrainingRequester.request_by);
            $('#job_title_id').val(response.data.detialTrainingRequester.job_title_name);
            $('#job_grade_id').val(response.data.detialTrainingRequester.job_grade_name);

            $('#training_name').val(response.data.traning_name);
            $('#training_type').val(response.data.training_type);

            $('#start_date').val(response.data.purpose_start_date);
            $('#end_date').val(response.data.purpose_end_date);

            $('#venue').val(response.data.venue);
            $('#training_cost').val(response.data.training_cost);

            if (parseInt(response.data.training_cost) > 15000000) {
                $("#optyes").prop("checked", true);
            } else {
                $("#optno").prop("checked", true);
            }

            $('#notes').val(response.data.notes);

            $("#dt_basic_job_posting > tbody").html("");
            var participant = 0;
            if (response != null) {
                $.each(response.data.listParticipantTraining, function (index, value) {               
                    AddRowTableParticipant(value.name_employee, value.job_title_name, value.job_grade_name, value.participantBusinessUnit.name);
                    participant = participant + 1;
                });
            }

            $('#total_cost_training').val((response.data.training_cost * participant));
        });
    }


});

function ProcessPayment() {

    swal({
        title: "Confirmation",
        text: "Are you Sure want to Process Payment once this process submited cannot be undo?",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (e) {
        if (e.value) {
            ajaxPost(ajaxUrlProcessPayment, { id: $('#id').val(), status: "2", add_recomendation: $('#add_recomendation').val()}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Payment Process Success");
                    hideAllModal();
                    window.location.href = WEB + "/Hris/TrainingRegistration";
                } else {
                    DangerNotif(response.message);
                }
            });
        }


    });

}

function RejectData() {

    if ($('#reject_reason').val() != null) {
        alert("Please enter reason first!");
        return false;
    } else {
        swal({
            title: "Confirmation",
            text: "Are you Sure want to Reject this data?",
            type: "warning",
            showCancelButton: !0,
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                ajaxPost(ajaxUrlUpdateStatus, { id: $('#id').val(), status: "2" }, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        var data = {
                            Id: '',
                            refId: $('#id').val(),
                            StatusApproved: 4,
                            Remarks: $('#reject_reason').val()
                        };
                        UpdateStatusApprovalHris(data, WEB + "/Hris/TrainingRequisition");
                        SuccessNotif(t_deletesuccess);
                        hideAllModal();
                    } else {
                        DangerNotif(response.message);
                    }
                });
            }


        });

    }

   

}


function AddRowTableParticipant(name, title, grade, division) {
    var acak = randomIntFromIntervalApproval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + name + "</td><td>" + title + "</td><td>" + grade + "</td><td>" + division + "</td></tr>";
    $("#dt_basic_job_posting tbody").append(markup);
}


function DeleteajaParticipan(id) {
    $('table#dt_basic_job_posting tr#' + id + '').remove();
}

function randomIntFromIntervalApproval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

 

