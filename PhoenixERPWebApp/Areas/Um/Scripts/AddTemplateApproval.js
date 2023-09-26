let bindDataEmployee = function () {
    let dataSearch = $('.textemployee').val();
    let urlApi = $.helper.resolveApi("~ManageUser/GetEmployee/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~ManageUser/GetEmployee");

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: urlApi,
            success: function (data) {
                bindTableEmployee(data);
            },
            error: function (e, t, s) {
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
                console.log(e);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 355));
};
let bindTableEmployee = function (data) {
    $('#tblEmp> tbody').empty();
    let table = $('#tblEmp > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.id + "</td>";
        row = row + "<td>" + val.nameEmployee + "</td>";
        row = row + "<td>" + val.email + "</td>";
        row = row + "<td>" + val.gender + "</td>";
        row = row + "<td>";
        row = row + "<button class='m-btn btn btn-success btn-sm m-btn--sm pull-right' title='select this employee' type='button' onclick='selectEmployee(\"" + val.id + "\",\"" + val.nameEmployee + "\")'><i class='fa fa-eye'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};
let selectEmployee = function (empId, empName) {
    if ($('#HidTypeUserApp').val() === 'CURRENT') {
        var dsUser = sessionStorage.getItem('dsCurrentUser');
        if (dsUser === null) dsUser = [];
        else dsUser = JSON.parse(dsUser);

        var idxApp = 0;
        $.each(dsUser, function (x, r) {
            idxApp = parseInt(r.indexUser) + 1;
        });

        //cek user is ready in list or not...
        let empIsReady = false;
        if (dsUser.length > 0) {
            $.each(dsUser, function (x, r) {
                if (r.empId === empId) {
                    empIsReady = true;
                }
            });
        }
        if (empIsReady === true) {
            toastr.options = {
                "positionClass": "toast-bottom-right",
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr.warning('employee name ' + empName + ', already in current list approval ', "Information");
            return false;
        }
        if (idxApp === 0) idxApp = 1;
        var model = { empId: empId, empName: empName, listConditional: [], isConditional: false, indexUser: idxApp };
        dsUser.push(model);
        sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));
    }
    else if ($('#HidTypeUserApp').val() === 'SC') {
        let dsUser = sessionStorage.getItem('dsSCUser');
        if (dsUser === null) dsUser = [];
        else dsUser = JSON.parse(dsUser);

        let idxApp = 0;
        $.each(dsUser, function (x, r) {
            idxApp = parseInt(r.indexUser) + 100;
        });

        //cek user is ready in list or not...
        let empIsReady = false;
        if (dsUser.length > 0) {
            $.each(dsUser, function (x, r) {
                if (r.empId === empId) {
                    empIsReady = true;
                }
            });
        }
        if (empIsReady === true) {
            toastr.options = {
                "positionClass": "toast-bottom-right",
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr.warning('employee name ' + empName + ', already in current special case approval ', "Information");
            return false;
        }
        if (idxApp === 0) idxApp = 100;
        let model = { empId: empId, empName: empName, indexUser: idxApp };
        dsUser.push(model);
        sessionStorage.setItem('dsSCUser', JSON.stringify(dsUser));
    }

    bindUserApproval();
    bindUserSpecialCase();
    $('#modalEmployee').modal('hide');
};

let openDataLookup = function (type) {
    $('#modalEmployee').modal('show');
    $('#HidTypeUserApp').val(type);
    bindDataEmployee();
};
let bindUserApproval = function () {
    var dsUser = sessionStorage.getItem('dsCurrentUser');
    if (dsUser === null) dsUser = [];
    else dsUser = JSON.parse(dsUser);

    var table = $('#tableListUser > tbody');
    table.empty();
    $.each(dsUser, function (x, r) {
        var row = "<tr>";
        row = row + "<td>" + r.empName + "</td>";
        row = row + "<td style='width:225px'>";
        row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='remove this user from approval' type='button' onclick='removeUserFromApproval(\"" + r.empId + "\")'><i class='fa fa-user-minus'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};
let bindUserSpecialCase = function () {
    var dsUser = sessionStorage.getItem('dsSCUser');
    if (dsUser === null) dsUser = [];
    else dsUser = JSON.parse(dsUser);

    var table = $('#tblUserSpecialCase > tbody');
    table.empty();
    $.each(dsUser, function (x, r) {
        var row = "<tr>";
        row = row + "<td>" + r.empName + "</td>";
        row = row + "<td style='width:225px'>";
        row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='remove this user from approval' type='button' onclick='removeUserFromSpecialCase(\"" + r.empId + "\")'><i class='fa fa-user-minus'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};


let saveData = function () {

    var isAllDiv = false;
    var ForDivisionSelect = "";
    var isSpecialCase = false;
    if ($("input[name='IsAllDivision']").is(':checked')) {
        //alert('all div');
        isAllDiv = true;
    } else {
        var optional = $("#ddlDivision").kendoMultiSelect({
            autoClose: false
        }).data("kendoMultiSelect");

        ForDivisionSelect = optional.value();
        if (ForDivisionSelect === "" || ForDivisionSelect === null) {
            ForDivisionSelect = "0";
        }
    }

    if ($("input[name='IsSpecialCase']").is(':checked')) {
        //alert('all div');
        isSpecialCase = true;
    }

    let templateName = $('#tbTemplateName').val();
    if (templateName.length === 0 || templateName === '') {
        toastr.warning('Template Name cannot empty', 'WARNING');
        return false;
    }

    let dsUser = sessionStorage.getItem('dsCurrentUser');


    if (dsUser === null) dsUser = [];
    else dsUser = JSON.parse(dsUser);
    if (dsUser.length === 0 && isSpecialCase === false) {
        toastr.warning('User Approval cannot empty, please add User Approval', 'WARNING');
        return false;
    }

    let webNotif = $('#cbWeb').prop('checked');
    let emailNotif = $('#cbEmail').prop('checked');
    let dueDateTime = $('#tbduedatetime').val();
    let reminderTime = $('#tbReminderTIme').val();
    let approvalType = $('#ddlType').val();
    let approvalCategory = $('#ddlCategory').val();

    let vmListUserApp = [];
    $.each(dsUser, function (zz, r) {
        let vmUser = {
            Id: '0',
            EmployeeBasicInfoId: r.empId,
            IndexUser: r.indexUser,
            IsCondition: r.isConditional,
            TmTempApprovalId: $('#HidId').val(),
            ListConditionalApprovals: [],
            IsSpecialCase: false
        };
        vmListUserApp.push(vmUser);
    });

    // adding user special case in last
    //dsUser = sessionStorage.getItem('dsSCUser');
    //if (dsUser === null) dsUser = [];
    //else dsUser = JSON.parse(dsUser);
    //$.each(dsUser, function (zz, r) {
    //    let vmUser = {
    //        Id: '0',
    //        EmployeeBasicInfoId: r.empId,
    //        IndexUser: r.indexUser,
    //        IsCondition: false,
    //        TmTempApprovalId: $('#HidId').val(),
    //        ListConditionalApprovals: [],
    //        IsSpecialCase: true
    //    };
    //    vmListUserApp.push(vmUser);
    //});

    var model = {
        Id: $('#HidId').val(),
        TemplateName: templateName,
        IsNotifyByEmail: emailNotif,
        IsNotifyByWeb: webNotif,
        DueDate: dueDateTime,
        Reminder: reminderTime,
        ApprovalCategory: approvalCategory,
        ApprovalType: approvalType,
        ListUserApprovals: vmListUserApp,
        //Additional....
        JobTitleId: $('#ddlJobTitle').val(),
        //ForDivisionId: $('#ddlDivision').val(),
        ForDivisionId: ForDivisionSelect,
        MenuId: $('#ddlMenu').val(),
        MinBudget: $('#tbMinBudget').val(),
        MaxBudget: $('#tbMaxBudget').val(),
        IsAllDivision: isAllDiv,
        IsSpecialCase: isSpecialCase
    };
    console.log(model);

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~ManageTemplateApproval/Save"),
        data: JSON.stringify(model),
        success: function (data) {
            console.log(data);

            //saveTemplateApprovalHris();
            Swal.fire(
                'Good job!',
                'Data has been saved!',
                'success'
            );
            sessionStorage.removeItem('firstLoad');
            sessionStorage.removeItem('dsCurrentUser');
            sessionStorage.removeItem('IdTempApp');
            $.each(data.idTemp, function (index, value) {
                sessionStorage.setItem('IdxTemp', JSON.stringify(value));
            });
            //sessionStorage.setItem('IdxTemp', JSON.stringify(data.data.idTemp));
            //window.location.href = '/um/templateapproval/AdditionalTemplate';
            window.location.href = '/um/templateapproval';

        },
        error: function (r) {
            console.log('masuk error');
            console.log(r);
            swal(
                '' + r.status,
                r.statusText,
                'error'
            );
        }
    }).then(setTimeout(function () {
        mApp.unblockPage();
    }, 355));

};

let removeUserFromApproval = function (empId) {
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    var dsUser = sessionStorage.getItem('dsCurrentUser');
    if (dsUser === null) dsUser = [];
    else dsUser = JSON.parse(dsUser);

    let vm = dsUser.find(x => x.empId === empId);
    dsUser.splice(vm, 1);

    sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));
    bindUserApproval();
    mApp.unblockPage();

};

let removeUserFromSpecialCase = function (empId) {
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    var dsUser = sessionStorage.getItem('dsSCUser');
    if (dsUser === null) dsUser = [];
    else dsUser = JSON.parse(dsUser);

    let vm = dsUser.find(x => x.empId === empId);
    dsUser.splice(vm, 1);

    sessionStorage.setItem('dsSCUser', JSON.stringify(dsUser));
    bindUserSpecialCase();
    mApp.unblockPage();

};


let bindEditedData = function (tempId) {
    var dsIdx = sessionStorage.getItem('IdTempApp');
    dsIdx = JSON.parse(dsIdx);
    let urlApi = $.helper.resolveApi("~ManageTemplateApproval/Get/" + dsIdx);

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: urlApi,
            success: function (data) {

                //$('#ddlMenu').val(data.menuId).trigger('change');
                console.log('data Menu Id = ' + data.menuId);
                console.log('data  = ' + JSON.stringify(data));

                let dsUser = [];
                $.each(data.listUserApprovals, function (x, r) {
                    if (r.isSpecialCase === false) {
                        var model = { empId: r.employeeBasicInfoId, empName: r.employeeName, listConditional: [], isConditional: r.isCondition, indexUser: r.indexUser };
                        dsUser.push(model);
                    }
                });
                sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));

                // reproduce user special case
                let dsUsersc = [];
                $.each(data.listUserApprovals, function (x, r) {
                    if (r.isSpecialCase === true) {
                        var model = { empId: r.employeeBasicInfoId, empName: r.employeeName, indexUser: r.indexUser };
                        dsUsersc.push(model);
                    }
                });
                sessionStorage.setItem('dsSCUser', JSON.stringify(dsUsersc));


                $('#HidId').val(dsIdx);
                if (data.isNotifyByEmail === true) $('#cbEmail').removeAttr('disabled');
                if (data.isNotifyByWeb === true) $('#cbWeb').removeAttr('disabled');
                $('#cbWeb').prop('checked', data.isNotifyByWeb);
                $('#cbEmail').prop('checked', data.isNotifyByEmail);
                $('#tbduedatetime').val(data.dueDate);
                $('#tbReminderTIme').val(data.reminder);
                $('#ddlType').val(data.approvalType).trigger('change');
                $('#ddlCategory').val(data.approvalCategory).trigger('change');
               
               
                $('#ddlJobTitle').val(data.jobTitleId).trigger('change');
                $('#ddlDivision').val(data.forDivisionId).trigger('change');
                $('#tbTemplateName').val(data.templateName);
                $('#tbMinBudget').val(data.minBudget);
                $('#tbMaxBudget').val(data.maxBudget);
                if (data.isAllDivision === true) {
                    $("input[name='IsAllDivision']").prop('checked', data.isAllDivision);
                    $('#ddlDivisionTr').hide();
                } else {
                    $('#ddlDivisionTr').show();
                }

                if (data.isSpecialCase === true) {
                    $("input[name='IsSpecialCase']").prop('checked', data.isSpecialCase);
                }
                bindUserApproval();
                bindUserSpecialCase();

                if ($('#ddlMenu').find("option[value='" + data.menuName + "']").length) {
                    $('#ddlMenu').val(data.id).trigger('change');
                } else {
                    //Create a DOM Option and pre-select by default
                    var newOption = new Option(data.menuName, data.menuId, true, true);
                    $("#ddlMenu").append(newOption).trigger('change');
                   
                } 

            },
            error: function (e, t, s) {
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
                console.log(e);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 2e3));


};

let bindNewData = function () {
    $('#tbduedatetime').val('1');
    $('#tbReminderTIme').val('1');

    bindUserApproval();
};

//get JOb TItle 
let getJobTitle = function () {
    let urlApi = $.helper.resolveApi("~/ManageTemplateApproval/GetJobTitle");

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: urlApi,
            success: function (data) {
                let ddl = $('#ddlJobTitle');
                ddl.empty();


                $(ddl).append($('<option>', {
                    value: "0",
                    text: "SELECT POSITION"
                }));

                $.each(data, function (i, item) {
                    $(ddl).append($('<option>', {
                        value: item.id,
                        text: item.titleName
                    }));
                });

            },
            error: function (e, t, s) {
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
                console.log(e);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 355));
};

let getMenu = function () {
    let urlApi = $.helper.resolveApi("~/ManageMenu/Get");

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: urlApi,
            success: function (data) {
                let ddl = $('#ddlMenu');
                //ddl.empty();
                $(ddl).append($('<option>', {
                    value: "0",
                    text: "SELECT MENU"
                }));

                $.each(data, function (i, item) {
                    $(ddl).append($('<option>', {
                        value: item.id,
                        text: item.menuName
                    }));
                });

            },
            error: function (e, t, s) {
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
                console.log(e);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 355));
};

let bindDivision = function () {
    let urlApi = $.helper.resolveApi("~/ManageTemplateApproval/GetDivision");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            let ddl = $('#ddlDivision');
            ddl.empty();

            $(ddl).append($('<option>', {
                value: "0",
                text: "SELECT DIVISION"
            }));

            $.each(data, function (i, item) {
                $(ddl).append($('<option>', {
                    value: item.id,
                    text: item.unitName
                }));
            });

        },
        error: function (e, t, s) {
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
            console.log(e);
        }
    })
};

$("#IsAllDivision").click(function () {
    if (this.checked) {
        $('#ddlDivisionTr').hide();
    } else {
        $('#ddlDivisionTr').show();
    }
});

$(document).ready(function () {
    $('#ddlType').select2();
    $('#ddlCategory').select2();
    $('.open-listemp').on('click', function () { openDataLookup('CURRENT'); });
    $('.open-listempSC').on('click', function () { openDataLookup('SC'); });
    $('.findEmp').on('click', function () { bindDataEmployee(); });
    $('.btnSaveTemp').on('click', function () { saveData(); });
    getJobTitle();
    getMenu();
    bindDivision();

    let dataId = sessionStorage.getItem('IdTempApp');
    if (dataId === null) bindNewData();
    else {
        dataId = JSON.parse(dataId);
        bindEditedData(dataId);
    }
    $('#ddlJobTitle').select2({
        placeholder: "Select a Position",
        allowClear: true
    });
    $('#ddlDivision').select2({
        placeholder: "Select a Division",
        allowClear: true
    });
    $('#ddlMenu').select2({
        placeholder: "Select a Menu",
        allowClear: true
    });
    if ($("input[name='IsAllDivision']").is(':checked')) {
        $('#ddlDivisionTr').hide();
    } else {
        $('#ddlDivisionTr').show();
    }
});
