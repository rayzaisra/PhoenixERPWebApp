let loadDataTemplate = function () {
    let catId = sessionStorage.getItem('categoryApproval');
    catId = JSON.parse(catId);
    catId = parseInt(catId);
    let urlApi = $.helper.resolveApi("~ManageTransApproval/GetByCategory/" + catId);
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {

            if (data.length === 0) {
                bindEmptyListApproval();
            }
            else {
                bindTableApproval(data);
                sessionStorage.setItem('dsTempApproval', JSON.stringify(data));
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
    });
};
let bindEmptyListApproval = function () {
    $('#dtTempApproval> tbody').empty();
    let table = $('#dtTempApproval > tbody:last-child');
    table.append('<tr><td colspan="8" class="text-center">there is no data</td></tr>');
};
let bindTableApproval = function (data) {
    $('#dtTempApproval> tbody').empty();
    let table = $('#dtTempApproval > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>";
        row = row + "<input type='checkbox' class='cbitem-tempApp' id='cbItem-" + val.id + "' data-code='" + val.id + "' onclick='selectApproval(\"" + val.id + "\" )'>";
        row = row + "</td>";
        row = row + "<td>" + val.templateName + "</td>";
        row = row + "<td>" + val.isNotifyByWeb + "</td>";
        row = row + "<td>" + val.isNotifyByEmail + "</td>";
        row = row + "<td>" + val.dueDate + "<i> - day(s)</i></td>";
        row = row + "<td>" + val.reminder + "<i> - day(s)</i></td>";
        if (val.approvalType === 1) row = row + "<td>SQUANCE</td>";
        else row = row + "<td>PARALLEL</td>";

        if (val.approvalCategory === 1) row = row + "<td>PM</td>";
        else if (val.approvalCategory === 2) row = row + "<td>HRIS</td>";
        else row = row + "<td>FINANCE</td>";

        row = row + "<td>";
        row = row + "<button class='btn btn-default btn-sm pull-right btn-d-" + val.id + " ' title='detail' type='button' onclick='detailApproval(\"" + val.id + "\")'><i class='fa fa-eye'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let selectApproval = function (id) {
    let cbitem = $('#cbItem-' + id);
    let $divTemp = $('.divTempApp');

    //remove session Template Approval...
    //end of remove session template approval...

    if (cbitem.prop('checked') === true) {
        sessionStorage.removeItem('isTempApproval');

        $('.cbitem-tempApp').prop('checked', false);
        $('#cbItem-' + id).prop('checked', true);
        $('.divTempApp > divSelectedTempApp').remove();
        $('.divTempApp > .divSelectedTempApp').remove();
        $('.divTempApp .divSelectedTempApp').remove();

        let dt = sessionStorage.getItem('dsTempApproval');
        dt = JSON.parse(dt);
        let dataApp = dt.find(x => x.id === id);
        var divList = '<div class="row divSelectedTempApp" style="margin-top:15px">';
        divList = divList + "<table class='table table-responsive table-striped- table-bordered table-checkable'>";
        divList = divList + "<thead>";
        divList = divList + "<tr>";
        divList = divList + "<th colspan='2'>#TEMPLATE NAME";
        divList = divList + "</th>";
        divList = divList + "</tr>";
        divList = divList + "</thead>";
        divList = divList + "<tbody>";
        divList = divList + "<tr>";
        divList = divList + "<td>";
        divList = divList + dataApp.templateName;
        divList = divList + "</td>";
        divList = divList + "<td>";
        divList = divList + "<span class='btn btn-sm btn-danger' title='remove template approval' onclick='removeSelectedApproval()'><i class='fa fa-minus-circle'></i></span>";
        divList = divList + "<span class='btn btn-sm btn-default' title='edit template approval' onclick='editSelectedApproval()'><i class='fa fa-edit'></i></span>";
        divList = divList + "</td>";
        divList = divList + "</tr>";
        divList = divList + "</tbody>";
        divList = divList + "</table>";
        divList = divList + "</div>";

        $divTemp.append(divList);
        let vm = { menu: window.location.pathname, dataTempApp: dataApp };
        sessionStorage.setItem('isTempApproval', JSON.stringify(vm));
        sessionStorage.removeItem('dsTempApproval');
        $('#modalListTempApproval').modal('hide');
    } else {
        $('.cbitem-tempApp').prop('checked', false);
        $('.divTempApp > divSelectedTempApp').remove();
        $('.divTempApp > .divSelectedTempApp').remove();
        $('.divTempApp .divSelectedTempApp').remove();
    }
};

let removeSelectedApproval = function () {
    sessionStorage.removeItem('isTempApproval');
    sessionStorage.removeItem('dsTempApproval');
    sessionStorage.removeItem('dsCurrentUser');
    $('.divTempApp > divSelectedTempApp').remove();
    $('.divTempApp > .divSelectedTempApp').remove();
    $('.divTempApp .divSelectedTempApp').remove();
};

let editSelectedApproval = function () {
    let data = sessionStorage.getItem('isTempApproval');
    data = JSON.parse(data);
    bindDataTemplate(data.dataTempApp, true);
    let dsEmp = sessionStorage.getItem('dsEmployee');
    if (dsEmp === null)  bindDataEmployee();

    $('.title-modal-tempApp').text('EDIT TEMPLATE APPROVAL');
    $('.openUserAppForm').css('display', 'block');
    $('.btnsaveConfigureApproval').css('display', 'block');
    $('#modalDetailTempApproval').modal('show');
};

let detailApproval = function (id) {
    loadDetailApproval(id, false);
    $('.openUserAppForm').css('display','none');
    $('.title-modal-tempApp').text('DETAIL TEMPLATE APPROVAL');
    $('.btnsaveConfigureApproval').css('display','none');
    $('#modalDetailTempApproval').modal('show');

};

let loadDetailApproval = function (id,isEdit) {
    let urlApi = $.helper.resolveApi("~ManageTemplateApproval/Get/" + id);
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            bindDataTemplate(data, isEdit);
        },
        error: function (e, t, s) {
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
            console.log(e);
        }
    });
};

let bindDataTemplate = function (data, isEdit) {
    if (data.isNotifyByEmail === true) $('#cbEmail').removeAttr('disabled');
    if (data.isNotifyByWeb === true) $('#cbWeb').removeAttr('disabled');
    $('#cbWeb').prop('checked', data.isNotifyByWeb);
    $('#cbEmail').prop('checked', data.isNotifyByEmail);
    $('#tbduedatetime').val(data.dueDate);
    $('#tbReminderTIme').val(data.reminder);
    $('#ddlType').val(data.approvalType);
    $('#tbTemplateName').val(data.templateName);
    let dsUser = [];
    $.each(data.listUserApprovals, function (x, r) {
        var model = { empId: r.employeeBasicInfoId, empName: r.employeeName, listConditional: [], isCondition: r.isCondition, indexUser: r.indexUser, id: r.id, tmTempApprovalId: data.id };
        dsUser.push(model);
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
    bindUserApproval(dsUser, isEdit);
    sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));

    bindUserApprovalCondition(dsUser, isEdit);

};

let bindUserApproval = function (dsUser, isedit) {
    var table = $('#tableListUser > tbody');
    table.empty();
    if (isedit === undefined) isedit = false;
    $.each(dsUser, function (x, r) {
        var row = "<tr>";
        row = row + "<td style='width:175px'>" + r.empName + "</td>";
        row = row + "<td id='tdConditional-" + r.empId + "'>";
        row = row + "</td>";
        if (isedit) {
            row = row + "<td style='width:165px'>";
            row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='remove user approval'  type='button' onclick='removeUserApproval(\"" + r.empId + "\")'><i class='fa fa-user-minus'></i></button>&nbsp;";
            row = row + "<button class='m-btn btn btn-warning btn-sm m-btn--sm pull-right " + r.id + "-clearConditional' title='remove conditional approval' type='button' onclick='clearConditional(\"" + r.id + "\",\"" + r.empId + "\")'><i class='fa fa-eraser'></i></button>&nbsp;";
            row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right " + r.id + "-addConditional' title='use conditional approval'  type='button' onclick='addConditional(\"" + r.id + "\",\"" + r.empId + "\")'><i class='fa fa-plus-circle' style='color:green'></i></button>&nbsp;";
            row = row + "</td>";
        } else {
            row = row + "<td style='width:145px'>";
            row = row + "</td>";
        }
        row = row + "</tr>";
        table.append(row);
    });
};
let removeUserApproval = function (empId) {
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    var dsUser = sessionStorage.getItem('dsCurrentUser');
    dsUser = JSON.parse(dsUser);

    let newCurrentUser = [];
    $.each(dsUser, function (b, a) {
        if (a.empId !== empId) {
            newCurrentUser.push(a);
        }
    });
    sessionStorage.setItem('dsCurrentUser', JSON.stringify(newCurrentUser));

    //#region remove item in list current approval...
    let dt = sessionStorage.getItem('isTempApproval');
    dt = JSON.parse(dt);

    let newListUserApproval = [];
    $.each(dt.dataTempApp.listUserApprovals, function (b, a) {
        if (a.employeeBasicInfoId !== empId) {
            newListUserApproval.push(a);
        }
    });
    dt.dataTempApp.listUserApprovals = [];
    dt.dataTempApp.listUserApprovals = newListUserApproval;
    sessionStorage.setItem('isTempApproval', JSON.stringify(dt));

    //#endregion

    bindUserApproval(newCurrentUser, true);
    bindUserApprovalCondition(newCurrentUser);
    mApp.unblockPage();

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

let addConditional = function (id, empid) {
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

        if (dataUser.isCondition === true) {
            let maxIndex = 0;
            let ConditionalApproval = "";
            $.each(dataUser.listConditional, function (x, r) {
                if (parseInt(r.GroupConditionIndex) > parseInt(maxIndex)) maxIndex = r.GroupConditionIndex;
                ConditionalApproval = r.ConditionalApproval;
            });

            console.log(maxIndex);

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

                    $('#ddlGroupConditional' + xx).select2({ allowClear: true });

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
                    $(".grp-" + xx).val(itemSelectedEmployee).trigger('change');
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

    let isListApproval = sessionStorage.getItem("isTempApproval");
    isListApproval = JSON.parse(isListApproval);

    $.each(isListApproval.dataTempApp.listUserApprovals, function (x, xx) {
        if (xx.employeeBasicInfoId === empId) xx.listConditionalApprovals = [];
    });

    sessionStorage.setItem('isTempApproval', JSON.stringify(isListApproval));
    $('#tdConditional-' + empId).empty();
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
                Id: "0",
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
            IsCondition: b.isCondition,
            TmTempApprovalId: $('#HidId').val(),
            ListConditionalApprovals: vmCondition
        };
        model.push(vm);
    });
    console.log(model);

    //mApp.blockPage({
    //    overlayColor: "#000000",
    //    type: "loader",
    //    state: "primary",
    //    message: "Processing..."
    //});
    //$.ajax({
    //    type: "POST",
    //    dataType: 'json',
    //    contentType: 'application/json',
    //    url: $.helper.resolveApi("~ManageTemplateApproval/SaveConditional"),
    //    data: JSON.stringify(model),
    //    success: function (data) {
    //        sessionStorage.removeItem('firstLoad');
    //        sessionStorage.removeItem('IdTempApp');
    //        sessionStorage.removeItem('IdxTemp');
    //        sessionStorage.removeItem('dsCurrentUser');
    //        sessionStorage.removeItem('dsEmployee');
    //        window.location.href = '/um/templateapproval';
    //    },
    //    error: function (r) {
    //        console.log('masuk error');
    //        console.log(r);
    //        swal(
    //            '' + r.status,
    //            r.statusText,
    //            'error'
    //        );
    //    }
    //}).then(setTimeout(function () {
    //    mApp.unblockPage();
    //}, 355));
};

let openTemplateApproval = function (categoryId) {
    sessionStorage.removeItem('categoryApproval');
    sessionStorage.setItem('categoryApproval', JSON.stringify(categoryId));
    loadDataTemplate();
    $('#modalListTempApproval').modal('show');
};

let bindListTemplateSelected = function (id) {
    let dataApp = sessionStorage.getItem('isTempApproval');
    dataApp = JSON.parse(dataApp);
    $('.divTempApp > divSelectedTempApp').remove();
    $('.divTempApp > .divSelectedTempApp').remove();
    let $divTemp = $('.divTempApp');
    var divList = '<div class="row divSelectedTempApp" style="margin-top:15px">';
    divList = divList + "<table class='table table-responsive table-striped- table-bordered'>";
    divList = divList + "<thead>";
    divList = divList + "<tr>";
    divList = divList + "<th colspan='2'>#TEMPLATE NAME";
    divList = divList + "</th>";
    divList = divList + "</tr>";
    divList = divList + "</thead>";
    divList = divList + "<tbody>";
    divList = divList + "<tr>";
    divList = divList + "<td>";
    divList = divList + dataApp.dataTempApp.templateName;
    divList = divList + "</td>";
    divList = divList + "<td>";
    divList = divList + "<span class='btn btn-sm btn-danger' title='remove template approval' onclick='removeSelectedApproval()'><i class='fa fa-minus-circle'></i></span>";
    divList = divList + "<span class='btn btn-sm btn-default' title='edit template approval' onclick='editSelectedApproval()'><i class='fa fa-edit'></i></span>";
    divList = divList + "</td>";
    divList = divList + "</tr>";
    divList = divList + "</tbody>";
    divList = divList + "<table>";
    divList = divList + "</div>";
    $divTemp.append(divList);
};

//#region Data Employee
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
    var model = { empId: empId, empName: empName, listConditional: [], isCondition: false, indexUser: idxApp, id: null, tmTempApprovalId: null };
    dsUser.push(model);

    //set is current Temporary approval...
    let tempApp = sessionStorage.getItem('isTempApproval');
    tempApp = JSON.parse(tempApp);
    let vmList = { employeeBasicInfoId: empId, employeeName: empName, id: "0", indexUser: idxApp, isCondition: false, tmTempApprovalId: tempApp.dataTempApp.id, listConditionalApprovals: [] };
    tempApp.dataTempApp.listUserApprovals.push(vmList);
    sessionStorage.setItem('isTempApproval', JSON.stringify(tempApp));
    //end of set is current Temporary approval...

    sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));
    bindUserApproval(dsUser, true);
    bindUserApprovalCondition(dsUser);
    $('#modalEmployee').modal('hide');
};

//#endregion

let saveGroupConditionalApproval = function () {
    let rowCount = $('#tblTempGroup .trSelectUser').length;
    let isJustONe = false;
    let currentEmpId = $('#HidCurrentEmpId').val();
    let currentEmpIdIsReady = false;

    // cek validasi...
    for (let i = 0; i < rowCount; i++) {
        let listUser = $('.grp-' + i).val();
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
    // end off cek validasi....

    var data = sessionStorage.getItem('dsCurrentUser');
    data = JSON.parse(data);
    let vm = data.find(x => x.empId === currentEmpId);

    if (!currentEmpIdIsReady) {
        toastr.error('User approval with name ' + vm.empName.toUpperCase() + ' must in group conditional approval', 'WARNING');
        return false;
    }
    vm.listConditional = [];
    vm.isCondition = true;


    let conditionApprovalAll = $('#ddlConditional-1').val();
    if (conditionApprovalAll === undefined) conditionApprovalAll = "-";

    let listConditionalIsTemp = [];
    for (let i = 0; i < rowCount; i++) {
        listUser = $('.grp-' + i).val();

        for (var a = 0; a < listUser.length; a++) {
            let dsEmp = sessionStorage.getItem('dsEmployee');
            dsEmp = JSON.parse(dsEmp);

            let dataEmp = dsEmp.find(x => x.id === listUser[a].toString());

            let model = {
                Id: "0",
                EmployeeBasicInfoId: dataEmp.id,
                GroupConditionIndex: i,
                GroupConditionApproval: $('#ddlGroupConditional' + i).val(),
                ConditionalApproval: conditionApprovalAll,
                TemplateUserApprovalId: $('#HidTempApprovalId').val(),
                EmployeeName: dataEmp.nameEmployee
            };

            let modelisTempApp = {
                id: "0",
                employeeBasicInfoId: dataEmp.id,
                groupConditionIndex: i,
                groupConditionApproval: $('#ddlGroupConditional' + i).val(),
                conditionalApproval: conditionApprovalAll,
                templateUserApprovalId: $('#HidTempApprovalId').val(),
                employeeName: dataEmp.nameEmployee
            };
            listConditionalIsTemp.push(modelisTempApp);

            vm.listConditional.push(model);
        }
    }
    $.each(data, function (x, r) {
        if (r.empid === currentEmpId) {
            r.listConditional = vm.listConditional;
        }
    });

    let dt = sessionStorage.getItem('isTempApproval');
    dt = JSON.parse(dt);
    $.each(dt.dataTempApp.listUserApprovals, function (a, b) {
        if (b.employeeBasicInfoId === currentEmpId) {
            b.listConditionalApprovals = listConditionalIsTemp;
            b.isCondition = true;
        }
    });

    sessionStorage.setItem('isTempApproval', JSON.stringify(dt));


    sessionStorage.setItem('dsCurrentUser', JSON.stringify(data));
    bindUserApprovalCondition(data);
    $('#HidCurrentEmpId').val('');
    $('#modalConditional').modal('hide');

};

let callSaveTemplate = function () {
    let tempId = makeid();
    let dataVm = { refId: tempId, tName: 'TESTER', detailLink: '/um/managegroup/' + tempId, formReqName: 'Request Manage Group' };
   saveTemplateApproval(dataVm, '/um/managegroup');
};

let saveTemplateApproval = function (data, url) {
    let dataApproval = sessionStorage.getItem("isTempApproval");
    dataApproval = JSON.parse(dataApproval);
    let model = {
        Id: "0",
        RefId: data.refId,
        TemplateName: dataApproval.dataTempApp.templateName,
        IsNotifyByEmail: dataApproval.dataTempApp.isNotifyByEmail,
        IsNotifyByWeb: dataApproval.dataTempApp.isNotifyByWeb,
        DueDate: dataApproval.dataTempApp.dueDate,
        DetailLink: data.detailLink,
        Reminder: dataApproval.dataTempApp.reminder,
        ApprovalCategory: dataApproval.dataTempApp.approvalCategory,
        ApprovalType: dataApproval.dataTempApp.approvalType,
        FormReqName: data.formReqName,
        Tname: data.tName,
        ListUserApprovals: dataApproval.dataTempApp.listUserApprovals
    };
    let urlApi = $.helper.resolveApi("~ManageTransApproval/Save");

    console.log(model);

   $.ajax({
       type: 'POST',
       url: urlApi,
       dataType: 'json',
       contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (result) {
            if (result.success) {
                sessionStorage.removeItem('categoryApproval');
                sessionStorage.removeItem('dsCurrentUser');
                sessionStorage.removeItem('isTempApproval');
                swal('Information', result.message, 'success')
                    .then(function (ok) {
                        if (ok.value) {
                            if (result.url !== undefined) window.location = result.url;
                        }
                    });
            } else {
                swal('Error', result.message, 'error');
            }
            mApp.unblock("#m_blockui_list");
        },
        error: function (e, t, s) {
            //console.log(e)
            console.log(e);
            mApp.unblock("#m_blockui_list");
        }
    });
};

function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (var i = 0; i < 20; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}
let saveConfigurationApproval = function () {
    let dataApproval = sessionStorage.getItem("isTempApproval");
    dataApproval = JSON.parse(dataApproval);
    dataApproval.dataTempApp.templateName = $('#tbTemplateName').val();
    dataApproval.dataTempApp.isNotifyByEmail = $('#cbEmail').prop('checked');
    dataApproval.dataTempApp.isNotifyByWeb = $('#cbWeb').prop('checked');
    dataApproval.dataTempApp.dueDate = parseInt($('#tbduedatetime').val());
    dataApproval.dataTempApp.reminder = parseInt($('#tbReminderTIme').val());
    dataApproval.dataTempApp.approvalType = parseInt($('#ddlType').val());

    sessionStorage.setItem('isTempApproval', JSON.stringify(dataApproval));
    $('#modalDetailTempApproval').modal('hide');

};

$(function () {
    let data = sessionStorage.getItem('isTempApproval');
    data = JSON.parse(data);
    if (data !== null) {
        let thisUrl = window.location.pathname;
        if (thisUrl === data.menu) 
            bindListTemplateSelected();
        else sessionStorage.removeItem('isTempApproval');
    }

    $('.openUserAppForm').on('click', function () {
        $('#modalEmployee').modal('show');
        bindDataEmployee();
    });
    $('.findEmp').on('click', function () {
        bindDataEmployee();
    });

    $('.btn-close-conditional').click(function (e) {
        $("#modalConditional").modal('hide');
        $('body').addClass('modal-open');
    });

    $('.close-modal-emp').click(function (e) {
        $("#modalEmployee").modal('hide');
    });

    $('.btnSaveGroupApproval').on('click', function () {
        saveGroupConditionalApproval();
    });

    $('[data-toggle="collapse"]').on('click', function () {
        $('body').addClass('modal-open');
    });
    $('#ddlType').select2();
    $('.js-example-basic-multiple').select2({
        placeholder: "Select a User Approval",
        allowClear: true
    });
    $('.btnsaveConfigureApproval').on('click', function () {
        saveConfigurationApproval();
    });
    
});
