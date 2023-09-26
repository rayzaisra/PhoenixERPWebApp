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

        var model = { empId: empId, empName: empName, listConditional: [], isConditional: false, indexUser: idxApp };
        dsUser.push(model);
        sessionStorage.setItem('dsCurrentUser', JSON.stringify(dsUser));
        bindUserApproval();
    }
    else if ($('#HidTypeUserApp').val() === 'ADD-GROUP-CONDITIONAL') {
        $('#HidCurrentEmpId').val();

    }
    $('#modalEmployee').modal('hide');
};

let rebuildConditional = function (listcon) {
    listcon.sort((a, b) => parseFloat(a.groupConditionIndex) - parseFloat(b.groupConditionIndex));

    // find and grouping by group index
    let listGroupIndex = [];
    $.each(listcon, function (a, b) {
        if (listGroupIndex.length === 0) listGroupIndex.push({ id: b.groupConditionIndex , empList : []});
        else {
            let isready = false;
            $.each(listGroupIndex, function (z, x) {
                if (b.groupConditionIndex === x.id) {
                    isready = true;
                }
            });
            if (!isready) {
                listGroupIndex.push({ id: b.groupConditionIndex, empList: []});
            }
        }
    });

    let listShow = [];
    $.each(listGroupIndex, function (x, r) {
        var mapList = listcon.find(x => x.groupConditionIndex === r.id);

        var showText = "";
        let appCon = "";
        $.each(mapList, function (x, r) {
            if (appCon.length === 0)
                appCon = r.GroupConditionApproval;

            showText = r.empName + " " + r.groupConditionApproval;
        });
        listShow.push({ rawHtml: "(" + showText + ")" });
    });

};

let addConditional = function (empId, empName) {
    $('#HidTypeUserApp').val('CONDITIONAL');
    $('#modalConditional').modal('show');
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
        row = row + "<td style='width:175px'>" + r.empName + "</td>";

        if (r.isConditional === true) {
            row = row + "<td>";
            row = row + "</td> ";
        } else {
            row = row + "<td>";
            row = row + "<div style='background: #e7f4ff; padding: 5px; margin: 5px' class='panel'> ";
            row = row + "<span class='badge-icon'>" + r.empName + "</span>";
            row = row + "<span class='btn btn-sm btn-default' onclick='editGroupApproval()'><i class='fa fa-edit'></i></span> ";
            row = row + "<span class='btn btn-sm btn-default' onclick='addUserGroupApproval(\"" + r.empId + "\",1)'><i class='fa fa-user-plus'></i></span> ";
            row = row + "</div> ";
           row = row + "</td>";
        }
        row = row + "<td style='width:145px'>";
        row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='remove this user from approval' type='button' onclick='removeUserFromApproval(\"" + r.empId + "\")'><i class='fa fa-user-minus'></i></button>&nbsp;";

        if (r.isConditional === true) {
            row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='use conditional approval' type='button' onclick='addConditional(\"" + r.empId + "\",\"" + r.empName + "\")'><i class='fa fa-check-circle' style='color: green'></i></button>&nbsp;";
            row = row + "<button class='m-btn btn btn-warning btn-sm m-btn--sm pull-right' title='remove conditional approval' type='button' onclick='clearConditional(\"" + r.empId + "\")'><i class='fa fa-eraser'></i></button>&nbsp;";
            row = row + "<button class='m-btn btn btn-info btn-sm m-btn--sm pull-right' title='edit conditional approval' type='button' onclick='editConditional(\"" + r.empId + "\")'><i class='fa fa-edit'></i></button>&nbsp;";
        }
        else row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='use conditional approval' type='button' onclick='addConditional(\"" + r.empId + "\",\"" + r.empName + "\")'><i class='fa fa-check-circle'></i></button>&nbsp;";

        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let addUserGroupApproval = function (currentEmpId, isFirst) {
    $('#modalEmployee').modal('show');
    $('#HidTypeUserApp').val('ADD-GROUP-CONDITIONAL');
    $('#HidCurrentEmpId').val(currentEmpId);
    bindDataEmployee();
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

$(document).ready(function () {
    //bindUserApproval();
    //$('.open-listemp').on('click', function () { openDataLookup('CURRENT'); });
    //$('.findEmp').on('click', function () { bindDataEmployee(); });        
    //$('.findEmpConditional').on('click', function () { openDataLookup('CONDITIONAL'); });        
    
});
