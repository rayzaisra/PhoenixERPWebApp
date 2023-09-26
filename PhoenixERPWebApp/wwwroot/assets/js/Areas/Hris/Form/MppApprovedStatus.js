var ajaxUrl = API + "/Hris/Mpp";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/UpdateStatusMpp";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlListGet = ajaxUrl + "/GetWithDetil"; 
var ajaxUrlListOrganization = API_GENERAL + "/BusinessUnit/ListWithParent";
var UrlSessionData = API + "employeeInfo/getByUserLogin";

var tableList = "MppApprovedStatusList";
var formSearch = "MppApprovedStatusFormSearch";
var modalSearch = "MppApprovedStatusModalSearch";
var formApproved = "MppDetailFormApproved";
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
        "data": "created_date",
        "title": "created_date",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "id_mpp",
        "title": "id_mpp",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "reason",
        "title": "reason",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "updated_date",
        "title": "updated_date",
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/MppApprovedStatus/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnMppApprovedStatus.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnMppApprovedStatus = {
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
                        fnMppApprovedStatus.reloadlist();
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
    
    initlanguage(lang_hr);
    var xid = $('#id').val();
    var ajaxUrlGetTrasactionApprovalLink = API + "General/TrTemplateApproval/GetByRefId";
   
    ajaxGet(ajaxUrlListGet + "/" + xid, {}, function (response) {
        response = parseJson(response);
        //console.log(response);
        //$('#division option[value=' + response.data.parent.parent.Id + ']').attr('selected', 'selected');
        var total_budget = 0;
        $('#fiscal_year').val(response.data.fiscal_year);
        $('#status').val(response.data.status);
        $('#code').val(response.data.code);
        $('#b_unit_id').val(response.data.business_unit_id);
        $('#business_unit_id_name').val(response.data.business_unit_name);
        $('#business_unit_id').val(response.data.business_unit_id);
        $('#fiscal_year_name').val(response.data.fiscal_year_name);
        $('#business_unit_id_name').prop('readonly', true);
        //$('#business_unit_id').val(response.data.business_unit_id).trigger('change');
        $.each(response.data.mppDetail, function (index, value) {
            //console.log(value);
            AddRowTable(value.job_title_name, value.ttf, value.employment_status, value.mpp_budged, value.job_grade_id, value.project, value.desc, value.job_tittle_id);
            total_budget = parseInt(total_budget) + parseInt(value.mpp_budged);
        });
        AddRowTotalBudget(total_budget);
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
            ajaxGet(ajaxUrlGetTrasactionApprovalLink + "/" + xid, {}, function (response) {
                response = parseJson(response);
                if (response.data !== null) {
                    DeleteajaApproval('no_data');
                    var datastatus = '';
                    var count = 1;
                    var ctn = '';
                    var ttl = '';
                    var statusApproved = "Current Approval";

                    $("#MppAprovalApproverList > tbody").html("");
                    $.each(response.data.listUserApproval, function (index, value) {

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
                            ctn = '<center>' + moment(value.createdOn).format('DD-MMM-YYYY HH:mm') + '</center>';
                        }

                        if (value.job_title_name === null || value.job_title_name === "") {
                            ttl = "Unset Jobtitle";
                        } else {
                            ttl = value.job_title_name;
                        }
                        AddRowTableApproval(count, ttl, value.employee_full_name, '<center>' + datastatus + '</center >', ctn);
                        count = count + 1;
                    });
                }
            });
        });
    });
});

function AddRowTable(jobtitle, tff, status, mppbudget, grade, project, desc, jobtitleid) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + jobtitle + "</td><td>" + tff + "</td><td>" + status + "</td><td style='text-align=right'>" + formatDecimal(mppbudget) + "</td><td style='display: none;'>" + grade + "</td><td style='display: none;'>" + project + "</td><td style='display: none;'><center><a href='#' class='btn btn-danger' onClick='#'></a></center></td><td style='display: none;'>" + desc + "</td><td style='display: none;'>" + jobtitleid + "</td></tr>";
    $("#MppDetailList tbody").append(markup);
}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function AddRowTotalBudget(total) {
    var markup = "<tr id='total_budget'><td colspan='2'>&nbsp;</td><td>TOTAL BUDGET</td ><td style='text-align:right'>" + formatDecimal(total) + "</td></tr>";
    $("MppDetailList tbody").append(markup);
    $('#total_budget').val(total);
}

function ApprovedData() {
    //alert($('#id').val());
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
            ajaxPost(ajaxUrlUpdate, { id: $('#id').val(), status: 1, fiscal_year: $('#fiscal_year').val(), code: $('#code').val(), business_unit_id: $('#b_unit_id').val(), total_budget: $('#total_budget').val()}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif("Approval Sucess");
                    //window.location.href = WEB + "/hris/Mpp";
                    //var data = {
                    //    Id: '',
                    //    refId: $('#id').val(),
                    //    StatusApproved: 3,
                    //    Remarks: ""
                    //};

                    var data = {
                        id: $('#id').val(),
                        refId: $('#id').val(),
                        detailLink: "/Hris/MppApprovedStatus/Index/" + $('#id').val(),
                        tName: "hr.mpp",
                        menuId: "9c85a265-6e60-41d2-9e1b-792857b381d0",
                        StatusApproved: 3,
                        IdTemplate: $("#template_approval_id").val(),
                        NotificationType: "hris.mpp.need-approval",
                        Module: "HRIS"
                    };
                    UpdateStatusApprovalHris(data,WEB + "/hris/Mpp");
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
           // window.location.href = WEB + "/hris/Mpp";
            var data = {
                Id: '',
                refId: $('#id').val(),
                StatusApproved: 4,
                Remarks: reason1
            };
            UpdateStatusApprovalHris(data, WEB + "/hris/Mpp");
        } else {
            DangerNotif(response.message);
        }
    });

}

function AddRowTableApproval(no, title, nama, status, update_by) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td><td>" + status + "</td><td>" + update_by + "</td></tr>";
    $("#MppAprovalApproverList tbody").append(markup);
}

function DeleteajaApproval(id) {
    $('table#MppAprovalApproverList tr#' + id + '').remove();
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

 

