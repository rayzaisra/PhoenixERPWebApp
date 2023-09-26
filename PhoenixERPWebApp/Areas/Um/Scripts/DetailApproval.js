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
    $('#tbMenuName').val(data.menuName);
    //$('#lblJobtitleName').text = data.jobTitleName;
    $('#lblJobtitleName').html = data.jobTitleName;

    $('#tbmin').text = data.minBudget;
    $('#tbmax').html = data.maxBudget;

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

    let dsUsersc = [];
    $.each(data.listUserApprovals, function (x, r) {
        if (r.isSpecialCase === true) {
            var model = { empId: r.employeeBasicInfoId, empName: r.employeeName, indexUser: r.indexUser, id: r.id, tmTempApprovalId: data.id };
            dsUsersc.push(model);
        }
    });

    bindUserApproval(dsUser);
    bindSpecialCaseApproval(dsUsersc);
    sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));

    bindUserApprovalCondition(dsUser);

};

let bindUserApproval = function (dsUser) {
    var table = $('#tableListUser > tbody');
    table.empty();
    $.each(dsUser, function (x, r) {
        var row = "<tr>";
        row = row + "<td style='width:175px'>" + r.empName + "</td>";
        row = row + "<td id='tdConditional-" + r.empId + "'>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let bindSpecialCaseApproval = function (dsUser) {
    var table = $('#tblSc> tbody');
    table.empty();
    if (dsUser.length === 0) {
        var row = "<tr>";
        row = row + "<td class='text-center'>there is no data for user special case approval</td>";
        row = row + "</tr>";
        table.append(row);
    }
    else {
        $.each(dsUser, function (x, r) {
            var row = "<tr>";
            row = row + "<td>" + r.empName + "</td>";
            row = row + "</tr>";
            table.append(row);
        });
    }
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

$(document).ready(function () {
    //bindDataEmployee();
    loadData();

});

