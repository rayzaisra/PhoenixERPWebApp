var ajaxUrl = API + "Hris/HiringRequest";
var ajaxUrlList = ajaxUrl + "/ListAllWithDetial";
var ajaxUrlUpdate = ajaxUrl + "/UpdateStatusHiringRequest";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlListGet = ajaxUrl + "/GetWithDetil"; 
var ajaxUrlListGetBusinessUnit = API + "General/BusinessUnit/rootparent"; 
var UrlSessionData = API + "employeeInfo/getByUserLogin";

var tableList = "HiringRequestList";
var formSearch = "HiringRequestFormSearch";
var modalSearch = "HiringRequestModalSearch";
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

var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";
$(function () {
    var xid = $('#id').val();
    var budgted_table = '';
    ajaxGet(ajaxUrlListGet + "/" + xid, {}, function (response) {
        response = parseJson(response);
        //console.log(response); 
        $('#code').val(response.data.code);
        $('#requester_name').val(response.data.requester_name);
        $('#requester_employee_basic_info_id').val(response.data.requester_employee_basic_info_id);
        $('#job_title_name').val(response.data.jobtitile.title_name);
        $('#job_title_id').val(response.data.jobtitile.id);
        $('#job_title_requester').val(response.data.job_title_requester);
        $('#job_title_name').val(response.data.jobtitile.title_name);

        if ((response.data.hiring_request_type_id.toLowerCase().indexOf("unbudgeted") >= 0)) {
            $('#inner_label_budgeted').html('<strong>Unbudgeted</strong>');
        } else if ((response.data.hiring_request_type_id.toLowerCase().indexOf("budgeted") >= 0)) {
            $('#inner_label_budgeted').html('<strong>Budgeted</strong>');
        } else {
            $('#inner_label_budgeted').html('<strong>Replacement</strong>');
            $('#tff_head').hide();
            $('#mppbudget_head').html("Budget");
            $('#employeestatus_head').html("Employee Replaced");
        }


        
        if (response.data.hiring_request_type_id === "Replacement") {
            budgted_table = "<tr id=" + response.data.id + "><td>" + response.data.jobtitile.title_name + "</td><td>" + response.data.jobGrade.grade_name + "</td><td>" + response.data.replacedEmployee.employee_replaced_name + "</td><td>" + response.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response.data.mpp_budged) + "</td><td style=\"text-align:center;vertical-align:middle\">" + response.data.date_fulfilment.split('T')[0] + "</td></tr>";
        } else {
            budgted_table = "<tr id=" + response.data.id + "><td>" + response.data.jobtitile.title_name + "</td><td>" + response.data.jobGrade.grade_name + "</td><td>" + response.data.ttf + "</td><td>" + response.data.employment_status + "</td><td>" + response.data.budget_souce + "</td><td style=\"text-align:right;vertical-align:middle\"> Rp. " + thousandsep(response.data.mpp_budged) + "</td><td style=\"text-align:center;vertical-align:middle\">" + response.data.date_fulfilment.split('T')[0] + "</td></tr>";
        }
        
        $("#dt_basic tbody").append(budgted_table);
        ajaxGet(ajaxUrlListGetBusinessUnit + "/" + response.data.division.id, {}, function (response) {
            response = parseJson(response);
            console.log("Responese Result = " + JSON.stringify(response));
            $('#division_id').val(response.data.id);
            $('#division_name').val(response.data.unit_name);
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
            ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + xid, {}, function (response2) {
                response2 = parseJson(response2);
                Deleteaja('no_data');
                var datastatus = '';
                var count = 1;
                var ctn = '';
                var ttl = '';
                $("#HiringRequestAprovalApproverList > tbody").html("");
                if (response2.data !== null) {
                    if (response2.data.listUserApproval !== null) {
                        $.each(response2.data.listUserApproval, function (index, value) {
                            if (value.statusApproved === 1) {
                                console.log(value.employeeBasicInfoId);
                                console.log(CurrentEmployeeID);
                                datastatus = "<span class='bluetext'>Need to Approved</span>";
                                if (value.employeeBasicInfoId === CurrentEmployeeID) {
                                    $("#with_approval").css({ display: "block" });
                                    $("#no_approval").css({ display: "none" });
                                } else {
                                    $("#with_approval").css({ display: "none" });
                                    $("#no_approval").css({ display: "block" });
                                }
                                datastatus = "<span class='bluetext'>Need to Approved</span>";
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
                                ctn = '<center>' + moment(value.createdOn).format("YYYY-MM-DD") + '</center>';
                            }

                            if (value.job_title_name === null || value.job_title_name === "") {
                                ttl = "Unset Jobtitle";
                            } else {
                                ttl = value.job_title_name;
                            }
                            AddRowTable(count, ttl, value.employee_full_name, '<center>' + datastatus + '</center >', ctn);
                            count = count + 1;
                        });
                    }
                }
            });
        });

        if (response.data.status !== "1" || response.data.status !== "2") {
            $('#code').prop("disabled", true);
            $('#requester_name').prop("disabled", true);

            $('#job_title_requester').prop("disabled", true);
            $('#division_name').prop("disabled", true);
            $('#business_unit_name').prop("disabled", true);
            $('#job_title_name').prop("disabled", true);
            $('#requester_name').prop("disabled", true);

        }
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
            ajaxPost(ajaxUrlUpdate, { id: $('#id').val(), status: 1}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Approval Sucess");
                    var data = {
                        Id: '',
                        refId: $('#id').val(),
                        StatusApproved: 3,
                        Remarks: ""
                    };
                    UpdateStatusApprovalHris(data, WEB + "/Hris/HiringRequest");
                    //window.location.href = WEB + "/hris/HiringRequest";
                } else {
                    DangerNotif(response.message);
                }
            });
        }
    });

}

function RejectData() {
    $('#MppApprovedStatusModalSearch').modal('toggle');

    var reason1 = $('#reject_reason').val();
    ajaxPost(ajaxUrlUpdate, { id: $('#id').val(), status: 2, fiscal_year: $('#fiscal_year').val(), code: $('#code').val(), business_unit_id: $('#b_unit_id').val(), total_budget: $('#total_budget').val(), reason: reason1 }, function (response) {
        response = parseJson(response);
        if (response.success) {
            SuccessNotif("Reject Sucess");
            var data = {
                Id: '',
                refId: $('#id').val(),
                StatusApproved: 4,
                Remarks: reason1
            };
            UpdateStatusApprovalHris(data, WEB + "/Hris/HiringRequest");
            //window.location.href = WEB + "/hris/HiringRequest";
        } else {
            DangerNotif(response.message);
        }
    });

}

function AddRowTable(no, title, nama,status,update_by) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td><td>" + status + "</td><td>" + update_by + "</td></tr>";
    $("#HiringRequestAprovalApproverList tbody").append(markup);
}


function Deleteaja(id) {
    $('table#HiringRequestAprovalApproverList tr#' + id + '').remove();
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

 

