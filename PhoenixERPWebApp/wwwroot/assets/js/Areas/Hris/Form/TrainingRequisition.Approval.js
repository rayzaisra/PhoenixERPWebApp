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

            if (response.data.status == "0") {
                $('#request_status').val("Waiting Approval");
            } else {
                $('#request_status').val("Approved");
            }

            $('#requester').val(response.data.detialTrainingRequester.request_by);
            $('#job_title_id').val(response.data.detialTrainingRequester.job_title_name);
            $('#job_grade_id').val(response.data.detialTrainingRequester.job_grade_name);

            $('#training_name').val(response.data.traning_name);
            $('#training_type').val(response.data.training_type);

            $('#start_date').val(response.data.purpose_start_date);
            $('#end_date').val(response.data.purpose_end_date);

            $('#venue').val(response.data.venue);
            $('#training_cost').val(response.data.training_cost);
            $('#notes').val(response.data.notes);

            if (response != null) {
                Deleteaja('no_data');
                $.each(response.data.listParticipantTraining, function (index, value) {               
                    CheckAndAddRow(value.employee_basic_info_id, value.name_employee, value.job_title_name, value.job_grade_name, value.participantBusinessUnit.name, "withoutaction");
                });
            }

            ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + xid, {}, function (response2) {
                response2 = parseJson(response2);
                DeleteajaApproval('no_data');
                var datastatus = '';
                var count = 1;
                var ctn = '';
                var ttl = '';
                $("#TrainingAprovalApproverList > tbody").html("");
                $.each(response2.data.listUserApproval, function (index, value) {
                    if (value.statusApproved == 1) {
                        datastatus = "<span class='bluetext'>Need to Approved</span>";
                    } else if (value.statusApproved == 2) {
                        datastatus = "<span>Waiting for Approval</span>";
                    } else if (value.statusApproved == 3) {
                        datastatus = "<span class='greentext'>Approved</span>";
                    } else if (value.statusApproved == 4) {
                        datastatus = "<span class='redtext'>Reject</span>";
                    } else {
                        datastatus = "<span class='bluetext'>Draft</span>";
                    }
                    if (value.statusApproved != 3) {
                        ctn = '<center> - </center>';
                    } else {
                        ctn = '<center>' + value.createdOn + '</center>';
                    }

                    if (value.job_title_name == null || value.job_title_name == "") {
                        ttl = "Unset Jobtitle";
                    } else {
                        ttl = value.job_title_name;
                    }
                    AddRowTableApproval(count, ttl, value.employee_full_name, '<center>' + datastatus + '</center >', ctn);
                    count = count + 1;
                });

            });

        });
    }


});

function ApprovedData() {

    swal({
        title: "Confirmation",
        text: "Are you Sure want to Approved this data?",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (e) {
        if (e.value) {
            ajaxPost(ajaxUrlUpdateStatus , {id:$('#id').val(),status:"0"}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    var data = {
                        Id: '',
                        refId: $('#id').val(),
                        StatusApproved: 3,
                        Remarks: ""
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


function AddRowTableApproval(no, title, nama, status, update_by) {
    var acak = randomIntFromIntervalApproval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td><td>" + status + "</td><td>" + update_by + "</td></tr>";
    $("#TrainingAprovalApproverList tbody").append(markup);
}


function DeleteajaApproval(id) {
    $('table#TrainingAprovalApproverList tr#' + id + '').remove();
}

function randomIntFromIntervalApproval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

 

