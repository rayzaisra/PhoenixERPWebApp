let loadData = function () {
    var dsIdx = sessionStorage.getItem('IdxTemp');
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
            bindDataTemplate(data);
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

let bindDataTemplate = function (data) {
    if (data.isNotifyByEmail === true) $('#cbEmail').removeAttr('disabled');
    if (data.isNotifyByWeb === true) $('#cbWeb').removeAttr('disabled');
    $('#cbWeb').prop('checked', data.isNotifyByWeb);
    $('#cbEmail').prop('checked', data.isNotifyByEmail);
    $('#tbduedatetime').val(data.dueDate);
    $('#tbReminderTIme').val(data.reminder);
    $('#ddlType').val(data.approvalType);
    $('#ddlCategory').val(data.approvalCategory);
    $('#tbTemplateName').val(data.templateName);
    $('#HidId').val(data.id);
    $('#ddlMenu').val(data.menuId).trigger('change');
    $('#ddlJobTitle').val(data.jobTitleId).trigger('change');
    $('#tbMinBudget').val(data.minBudget);
    $('#tbMaxBudget').val(data.maxBudget);

    console.log(data.listUserApprovals);
    //let isReadyLoad = sessionStorage.getItem('firstLoad');
    //if (isReadyLoad === null) {
        //sessionStorage.setItem('firstLoad', JSON.stringify('yes'));
        //sessionStorage.removeItem('dsCurrentUser');
        let dsUser = [];
        $.each(data.listUserApprovals, function (x, r) {
            if (r.isSpecialCase === false) {
                var model = { empId: r.employeeBasicInfoId, empName: r.employeeName, listConditional: [], isConditional: r.isCondition, indexUser: r.indexUser, id: r.id, tmTempApprovalId: data.id };
                dsUser.push(model);
            }
        });

        $.each(dsUser, function (x, r) {
            let dataUser = data.listUserApprovals.filter(x => x.employeeBasicInfoId === r.empId);
            $.each(dataUser[0].listConditionalApprovals, function (b, a) {
                let model = {
                    EmployeeBasicInfoId: a.employeeBasicInfoId,
                    GroupConditionIndex: a.groupConditionIndex,
                    GroupConditionApproval: a.groupConditionApproval,
                    ConditionalApproval: a.conditionalApproval,
                    TemplateUserApprovalId: a.templateUserApprovalId,
                    EmployeeName: a.employeeName
                };
                r.listConditional.push(model);
            });

        });

        bindUserApproval(dsUser);
        sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));

        bindUserApprovalCondition(dsUser);
    //}
    //else {
    //    isReadyLoad = JSON.parse(isReadyLoad);
    //    let dsUser = sessionStorage.getItem('dsCurrentUser');
    //    dsUser = JSON.parse(dsUser);
    //    bindUserApproval(dsUser);
    //    bindUserApprovalCondition(dsUser);
    //}

};

let bindUserApproval = function (dsUser) {
    var table = $('#tableListUser > tbody');
    table.empty();
    $.each(dsUser, function (x, r) {
        var row = "<tr>";
        row = row + "<td style='width:175px'>" + r.empName + "</td>";
        row = row + "<td id='tdConditional-" + r.empId + "'>";
        row = row + "</td>";
        row = row + "<td style='width:145px'>";
        row = row + "<button class='m-btn btn btn-warning btn-sm m-btn--sm pull-right " + r.id + "-clearConditional' title='remove conditional approval' type='button' onclick='clearConditional(\"" + r.id + "\",\"" + r.empId + "\")'><i class='fa fa-eraser'></i></button>&nbsp;";
        row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right " + r.id + "-addConditional' title='use conditional approval'  type='button' onclick='addConditional(\"" + r.id + "\",\"" + r.empId + "\")'><i class='fa fa-plus-circle' style='color:green'></i></button>&nbsp;";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let addConditional = function (id,empid) {
    $('#HidCurrentEmpId').val(empid);
    $('#HidTempApprovalId').val(id);
    addGroupCondition(1);
    $('#modalConditional').modal('show');

};

let addGroupCondition = function (source) {

    let rowCount = $('#tblTempGroup .trSelectUser').length;
    let tbl = $('#tblTempGroup>tbody');
    let row = "";
    let currentEmpId = $('#HidCurrentEmpId').val();


    if (source === 1) { // jika openup nya dari header data...
        tbl.empty();
        let dsCurentUser = sessionStorage.getItem('dsCurrentUser');
        dsCurentUser = JSON.parse(dsCurentUser);

        let dataUser = dsCurentUser.find(x => x.empId === currentEmpId);

        if (dataUser.isConditional === true) {
            let maxIndex = 0;
            let ConditionalApproval = "";
            $.each(dataUser.listConditional, function (x, r) {
                if (parseInt(r.GroupConditionIndex) > parseInt(maxIndex)) maxIndex = r.GroupConditionIndex;
                ConditionalApproval = r.ConditionalApproval;
            });

            if (maxIndex === 0) {
                row = row + "<tr class='trSelectUser trGroupCon-" + maxIndex + "'>";
                row = row + "<td>";
                row = row + '<select class="js-example-basic-multiple grp-' + maxIndex + '" multiple="multiple" style="width:795px"></select>';
                row = row + "</td>";
                row = row + "<td>";
                row = row + "<select id='ddlGroupConditional" + maxIndex + "' class='form-control form-control-sm' style='width:100%'>";
                row = row + "    <option value='AND'>AND</option>";
                row = row + "    <option value='OR'>OR</option>";
                row = row + "</select>";
                row = row + "</td>";
                row = row + "<td>";
                row = row + "<button class='btn btn-sm btn-info' onclick='addGroupCondition(2)'><i class='fa fa-plus-circle'></i></button>";
                if (maxIndex > 0) {
                    row = row + "<button class='btn btn-sm btn-danger' onclick='removeGroupCondition(\"" + maxIndex + "\") '><i class='fa fa-minus-circle'></i></button>";
                }
                row = row + "</td>";
                row = row + "</tr>";
                tbl.append(row);

                var data = sessionStorage.getItem('dsEmployee');
                data = JSON.parse(data);
                let ddlEmployee = $('.grp-' + maxIndex);
                ddlEmployee.empty();
                $.each(data, function (i, item) {
                    $(ddlEmployee).append($('<option>', {
                        value: item.id,
                        text: item.nameEmployee
                    }));
                });
                ddlEmployee.select2({
                    placeholder: "Select a User Approval",
                    allowClear: true
                });

                let itemSelectedEmployee = new Array();
                dataUser.listConditional.sort((a, b) => parseFloat(a.groupConditionIndex) - parseFloat(b.groupConditionIndex));
                let no = 0;
                $.each(dataUser.listConditional, function (x, r) {
                    itemSelectedEmployee[no] = dataUser.listConditional[no].EmployeeBasicInfoId;
                    no++;
                });
                $(".grp-0").val(itemSelectedEmployee).trigger('change');


            }
            else { //jika groupIndex nya lebih dari satu....
                let xx = 0;
                for (var x = 0; x <= maxIndex; x++) {
                    let itemSelectedEmployee = new Array();
                    row = "";
                    if (xx > 0) {
                        row = row + "<tr class='trGroupCon-" + xx + "'>";
                        row = row + "<td colspan='3'>";
                        row = row + "<select id='ddlConditional-" + xx + "' class='form-control form-control-sm ddlConditionGroup' style='width:125px' onchange='changeSelectedCondition(this)'>";
                        row = row + "    <option value='AND'>AND</option>";
                        row = row + "    <option value='OR'>OR</option>";
                        row = row + "</select>";
                        row = row + "</td>";
                        row = row + "</tr>";
                        tbl.append(row);
                        row = "";
                        $('#ddlConditional-' + xx).select2();
                        $('#ddlConditional-' + xx).val(ConditionalApproval).trigger('change');
                    }
                    row = row + "<tr class='trSelectUser trGroupCon-" + xx + "'>";
                    row = row + "<td>";
                    row = row + '<select class="js-example-basic-multiple grp-' + xx + '" multiple="multiple" style="width:795px"></select>';
                    row = row + "</td>";
                    row = row + "<td>";
                    row = row + "<select id='ddlGroupConditional" + xx + "' class='form-control form-control-sm' style='width:100%'>";
                    row = row + "    <option value='AND'>AND</option>";
                    row = row + "    <option value='OR'>OR</option>";
                    row = row + "</select>";
                    row = row + "</td>";
                    row = row + "<td>";
                    row = row + "<button class='btn btn-sm btn-info' onclick='addGroupCondition(2)'><i class='fa fa-plus-circle'></i></button>";
                    if (xx > 0) {
                        row = row + "<button class='btn btn-sm btn-danger' onclick='removeGroupCondition(\"" + xx + "\") '><i class='fa fa-minus-circle'></i></button>";
                    }
                    row = row + "</td>";
                    row = row + "</tr>";
                    tbl.append(row);

                    ///define select2 for user approval..
                    data = sessionStorage.getItem('dsEmployee');
                    data = JSON.parse(data);
                    let ddlEmployee = $('.grp-' + xx);
                    ddlEmployee.empty();
                    $.each(data, function (i, item) {
                        $(ddlEmployee).append($('<option>', {
                            value: item.id,
                            text: item.nameEmployee
                        }));
                    });

                    ddlEmployee.select2({
                        placeholder: "Select a User Approval",
                        allowClear: true
                    });

                    $('#ddlGroupConditional' + xx).select2({ allowClear: true});

                    let listConditional = [];
                    let groupConditional = "";
                    $.each(dataUser.listConditional, function (y, r) {
                        if (r.GroupConditionIndex === x) {
                            listConditional.push(r);
                            groupConditional = r.GroupConditionApproval;
                        }
                    });

                    listConditional.sort((a, b) => parseFloat(a.groupConditionIndex) - parseFloat(b.groupConditionIndex));
                    let no = 0;
                    $.each(listConditional, function (x, r) {
                        itemSelectedEmployee[no] = listConditional[no].EmployeeBasicInfoId;
                        no++;
                    });
                    $(".grp-"+xx).val(itemSelectedEmployee).trigger('change');
                    $('#ddlGroupConditional' + xx).val(groupConditional).trigger('change');
                    xx++;
                }

            }



        }
        else {
            row = row + "<tr class='trSelectUser trGroupCon-0'>";
            row = row + "<td>";
            row = row + '<select class="js-example-basic-multiple grp-0" multiple="multiple" style="width:855px"></select>';
            row = row + "</td>";
            row = row + "<td>";
            row = row + "<select id='ddlGroupConditional0' class='form-control form-control-sm' style='width:100%'>";
            row = row + "    <option value='AND'>AND</option>";
            row = row + "    <option value='OR'>OR</option>";
            row = row + "</select>";
            row = row + "</td>";
            row = row + "<td>";
            row = row + "<button class='btn btn-sm btn-info' onclick='addGroupCondition(2)'><i class='fa fa-plus-circle'></i></button>";
            //if (rowCount > 0) {
            //    row = row + "<button class='btn btn-sm btn-danger' onclick='removeGroupCondition(\"" + rowCount + "\") '><i class='fa fa-minus-circle'></i></button>";
            //}
            row = row + "</td>";
            row = row + "</tr>";
            tbl.append(row);

            $('#ddlGroupConditional0').select2();

            data = sessionStorage.getItem('dsEmployee');
            data = JSON.parse(data);
            let ddlEmployee = $('.grp-0');
            ddlEmployee.empty();
            ddlEmployee.select2({
                placeholder: "Select a User Approval",
            });

            $.each(data, function (i, item) {
                $(ddlEmployee).append($('<option>', {
                    value: item.id,
                    text: item.nameEmployee
                }));
            });

        }

    }
    else { // jika add nya dari table list group user approval....
        if (rowCount > 0) {
            row = row + "<tr class='trGroupCon-" + rowCount + "'>";
            row = row + "<td colspan='3'>";
            row = row + "<select id='ddlConditional-" + rowCount + "' class='form-control form-control-sm ddlConditionGroup' style='width:125px' onchange='changeSelectedCondition(this)'>";
            row = row + "    <option value='AND'>AND</option>";
            row = row + "    <option value='OR'>OR</option>";
            row = row + "</select>";
            row = row + "</td>";
            row = row + "</tr>";
        }
        row = row + "<tr class='trSelectUser trGroupCon-" + rowCount + "'>";
        row = row + "<td>";
        row = row + '<select class="js-example-basic-multiple grp-' + rowCount + '" multiple="multiple" style="width:795px"></select>';
        row = row + "</td>";
        row = row + "<td>";
        row = row + "<select id='ddlGroupConditional" + rowCount + "' class='form-control form-control-sm' style='width:100%'>";
        row = row + "    <option value='AND'>AND</option>";
        row = row + "    <option value='OR'>OR</option>";
        row = row + "</select>";
        row = row + "</td>";
        row = row + "<td>";
        row = row + "<button class='btn btn-sm btn-info' onclick='addGroupCondition(2)'><i class='fa fa-plus-circle'></i></button>";
        if (rowCount > 0) {
            row = row + "<button class='btn btn-sm btn-danger' onclick='removeGroupCondition(\"" + rowCount + "\") '><i class='fa fa-minus-circle'></i></button>";
        }
        row = row + "</td>";
        row = row + "</tr>";
        tbl.append(row);
        $('#ddlConditional-' + rowCount).select2();
        $('#ddlGroupConditional' + rowCount).select2();

        let data = sessionStorage.getItem('dsEmployee');
        data = JSON.parse(data);
        let ddlEmployee = $('.grp-' + rowCount);
        ddlEmployee.empty();
        $.each(data, function (i, item) {
            $(ddlEmployee).append($('<option>', {
                value: item.id,
                text: item.nameEmployee
            }));
        });
        ddlEmployee.select2({
            placeholder: "Select a User Approval",
            allowClear: true
        });

    }

};

let changeSelectedCondition = function (elm) {
    $('.ddlConditionGroup').val(elm.value);
};

let removeGroupCondition = function (rowIndex) {
    $('.trGroupCon-' + rowIndex).remove();
};

let clearConditional = function (id, empId) {
    let ds = sessionStorage.getItem('dsCurrentUser');
    ds = JSON.parse(ds);
    $.each(ds, function (x, xx) {
        if (xx.empId === empId) xx.listConditional = [];
    });

    sessionStorage.setItem('dsCurrentUser', JSON.stringify(ds));
    $('#tdConditional-' + empId).empty();
};

let bindDataEmployee = function () {
    let dsEmpList = sessionStorage.getItem('dsEmployee');
    if (dsEmpList === null) {
        let urlApi = $.helper.resolveApi("~ManageUser/GetEmployee");

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
                    sessionStorage.setItem('dsEmployee', JSON.stringify(data));
                    let ddlEmployee = $('.js-example-basic-multiple');
                    ddlEmployee.empty();
                    $.each(data, function (i, item) {
                        $(ddlEmployee).append($('<option>', {
                            value: item.id,
                            text: item.nameEmployee
                        }));
                    });
                    ddlEmployee.select2({
                        placeholder: "Select a User Approval",
                        allowClear: true
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
    }
    else {
        var dsEmployee = sessionStorage.getItem('dsEmployee');
        dsEmployee = JSON.parse(dsEmployee);
        let ddlEmployee = $('.js-example-basic-multiple');
        ddlEmployee.empty();
        $.each(dsEmployee, function (i, item) {
            $(ddlEmployee).append($('<option>', {
                value: item.id,
                text: item.nameEmployee
            }));
        });
        ddlEmployee.select2({
            placeholder: "Select a User Approval",
            allowClear: true
        });
}
};

let saveGroupConditionalApproval = function () {
    let rowCount = $('#tblTempGroup .trSelectUser').length;
    let isJustONe = false;
    let currentEmpId = $('#HidCurrentEmpId').val();
    let currentEmpIdIsReady = false;

    for (let i = 0; i < rowCount; i++) {
        let listUser = $('.grp-'+i).val();
        if (listUser.length < 2) {
            isJustONe = true;
        }
        $.each(listUser, function (x, r) {
            if (currentEmpId === r) currentEmpIdIsReady = true;
        });
    }

    if (isJustONe) {
        toastr.error('User approval in group conditional approval cannot less than 2, minimum 2 user approval', 'WARNING');
        return false;
    }


    var data = sessionStorage.getItem('dsCurrentUser');
    data = JSON.parse(data);
    let vm = data.find(x => x.empId === currentEmpId);

    if (!currentEmpIdIsReady) {
        toastr.error('User approval with name ' + vm.empName.toUpperCase() + ' must in group conditional approval', 'WARNING');
        return false;
    }
    vm.listConditional = [];
    vm.isConditional = true;


    let conditionApprovalAll = $('#ddlConditional-1').val();
    if (conditionApprovalAll === undefined) conditionApprovalAll = "-";

    for (let i = 0; i < rowCount; i++) {
        listUser = $('.grp-' + i).val();

        for (var a = 0; a < listUser.length; a++) {
            let dsEmp = sessionStorage.getItem('dsEmployee');
            dsEmp = JSON.parse(dsEmp);

            let dataEmp = dsEmp.find(x => x.id === listUser[a].toString());

            let model = {
                Id : "0",
                EmployeeBasicInfoId: dataEmp.id,
                GroupConditionIndex: i,
                GroupConditionApproval: $('#ddlGroupConditional' + i).val(),
                ConditionalApproval: conditionApprovalAll,
                TemplateUserApprovalId: $('#HidTempApprovalId').val(),
                EmployeeName: dataEmp.nameEmployee  
            };

            vm.listConditional.push(model);
        }
    }
    $.each(data, function (x, r) {
        if (r.empid === currentEmpId) {
            r.listConditional = vm.listConditional;
        }
    });
    sessionStorage.setItem('dsCurrentUser', JSON.stringify(data));
    bindUserApprovalCondition(data);
    $('#HidCurrentEmpId').val('');
    $('#modalConditional').modal('hide');

};
let bindUserApprovalCondition = function (data) {


    //each dsCurrentUser ...
    $.each(data, function (x, r) {
        $('#tdConditional-' + r.empId).empty();
        //sortby grou pindex...
        r.listConditional.sort((a, b) => parseFloat(a.groupConditionIndex) - parseFloat(b.groupConditionIndex));

        let groupIndexList = [];
        let condisiApproval = "";
        $.each(r.listConditional, function (y, a) {
            condisiApproval = a.ConditionalApproval;
            let isready = true;
            $.each(groupIndexList, function (f, g) {
                if (g.gi === a.GroupConditionIndex) isready = false;
            });
            if (isready) {
                let vmList = r.listConditional.filter(x => x.GroupConditionIndex === a.GroupConditionIndex);
                groupIndexList.push({ gi: a.GroupConditionIndex, itemGroup: vmList });
            }
        });
        let writeCondition = "";
        let countGi = 1;
        $.each(groupIndexList, function (a, b) {
            writeCondition = "[";
            let countItem = b.itemGroup.length;
            let xx = 1;
            $.each(b.itemGroup, function (r, x) {
                if (xx === countItem)
                    writeCondition = writeCondition + " " + x.EmployeeName;
                else
                    writeCondition = writeCondition + " " + x.EmployeeName + " <b>" + x.GroupConditionApproval + "</b> ";

                xx++;
            });

            if (countGi === groupIndexList.length)
                writeCondition = writeCondition + "]";
            else
                writeCondition = writeCondition + "] <br><b>" + condisiApproval + "</b> <br>";

            $('#tdConditional-' + r.empId).append(writeCondition);
            countGi++;
        });


    });
};

let saveConditionalApproval = function () {
    //cek condional nya ada atau nggak ada...
    let ds = sessionStorage.getItem('dsCurrentUser');
    ds = JSON.parse(ds);

    let isListEmpty = true;
    $.each(ds, function (b, a) {
        if (a.listConditional.length !== 0) isListEmpty = false;
    });
    if (isListEmpty) {
        window.location.href = '/um/templateapproval';
    }

    ///process save conditional...
    let model = [];
    $.each(ds, function (a, b) {

        let vmCondition = [];
        $.each(b.listConditional, function (b, a) {
            let dataCondition = {
                Id :"0",
                EmployeeBasicInfoId: a.EmployeeBasicInfoId,
                GroupConditionIndex: a.GroupConditionIndex,
                GroupConditionApproval: a.GroupConditionApproval,
                ConditionalApproval: a.ConditionalApproval,
                TemplateUserApprovalId: a.TemplateUserApprovalId
            };
            vmCondition.push(dataCondition);
        });

        let vm = {
            Id: b.id,
            EmployeeBasicInfoId: b.empId,
            IndexUser: b.indexUser,
            IsCondition: b.isConditional,
            TmTempApprovalId: $('#HidId').val(),
            ListConditionalApprovals: vmCondition
        };
        model.push(vm);
    });
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
        url: $.helper.resolveApi("~ManageTemplateApproval/SaveConditional"),
        data: JSON.stringify(model),
        success: function (data) {
            sessionStorage.removeItem('firstLoad');
            sessionStorage.removeItem('IdTempApp');
            sessionStorage.removeItem('IdxTemp');
            sessionStorage.removeItem('dsCurrentUser');
            sessionStorage.removeItem('dsEmployee');            
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
                ddl.empty();
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

$(document).ready(function () {
    getJobTitle();
    getMenu();

    bindDataEmployee();
    loadData();
    $('.btnSaveGroupApproval').on('click', function () {
        saveGroupConditionalApproval();
    });
    $('.btnSaveTemp').on('click', function () {
        saveConditionalApproval();
    });

    $('#ddlJobTitle').select2({
        placeholder: "Select a Position",
        allowClear: true
    });
    $('#ddlMenu').select2({
        placeholder: "Select a Menu",
        allowClear: true
    });
});

