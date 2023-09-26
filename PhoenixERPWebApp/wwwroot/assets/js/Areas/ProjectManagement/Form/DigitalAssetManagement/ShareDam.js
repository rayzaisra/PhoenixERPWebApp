var selectedEmpId = [];
var fileCheckbox = [];
var ajaxUrlFileShare = API + "ProjectManagement/Dam/AddShare";
var ajaxUrlFileMoveToMyDam = API + "ProjectManagement/Dam/MoveToMyDam";

function removeA(arr) {
    var what, a = arguments, L = a.length, ax;
    while (L > 1 && arr.length) {
        what = a[--L];
        while ((ax = arr.indexOf(what)) !== -1) {
            arr.splice(ax, 1);
        }
    }
    return arr;
}

let bindDataEmployee = function () {
    let dataSearch = $('.textemployee').val();
    let urlApi = $.helper.resolveApi("~ManageUser/GetEmployeeSortByName/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~ManageUser/GetEmployeeSortByName");

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
        var row = "<tr  id='row-" + val.id +"'>";
        row = row + "<td>" + val.id + "</td>";
        row = row + "<td>" + val.nameEmployee + "</td>";
        row = row + "<td>" + val.email + "</td>";
        row = row + "<td>" + val.gender + "</td>";
        row = row + "<td>";
        row = row + "<button class='m-btn btn btn-info btn-sm m-btn--sm pull-right' id='btn-" + val.id +"'  title='select this employee' type='button' onclick='selectEmployee(\"" + val.id + "\",\"" + val.nameEmployee + "\",\"" + val.email + "\")'><i class='fa fa-eye' id="+val.id+"></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });

    $.each(selectedEmpId, function (index, val) {
        $('#' + val).removeClass("fa fa-eye");
        $('#' + val).addClass("fa fa-check-square");
        $('#' + val).addClass("selectShare");
        $('#btn-' + val).removeClass("btn-info");
        $('#btn-' + val).addClass("btn-success");
        $('#row-' + val).children('td, th').css('background-color', 'yellow');
    });
    
};
let selectEmployee = function (empId, empName, email) {

    if ($('#' + empId).hasClass("fa-eye")) {
        $('#' + empId).removeClass("fa fa-eye");
        $('#' + empId).addClass("fa fa-check-square");
        $('#' + empId).addClass("selectShare");
        $('#btn-' + empId).removeClass("btn-info");
        $('#btn-' + empId).addClass("btn-success");
        selectedEmpId.push(empId);
        $('#row-' + empId).children('td, th').css('background-color', 'yellow');
    } else {
        $('#' + empId).removeClass("fa fa-check-square");
        $('#' + empId).addClass("fa fa-eye");
        $('#' + empId).removeClass("selectShare");
        $('#btn-' + empId).removeClass("btn-success");
        $('#btn-' + empId).addClass("btn-info");
        removeA(selectedEmpId, empId);
        $('#row-' + empId).children('td, th').css('background-color', '');
    }
   
    //$('#tbEmpId').val(empId);
    //$('#tbAliasName').val(empName);
    //$('#tbUserId').val(email);
    //$('#tbEmail').val(email);
    //$('#modalEmployee').modal('hide');
};

function ShareFile() {
    
    $('input[type="checkbox"]:checked').each(function () {
        fileCheckbox.push($(this).attr("value"));
    });

    if (selectedEmpId.length === 0) {
        $('#modalEmployee').modal('hide');
        return DangerNotif("Belum ada user yang dipilih");
    }

    if (fileCheckbox.length === 0 ) {
        $('#modalEmployee').modal('hide');
        return DangerNotif("Belum ada file yang dipilih");
    }

    var model = {
        ShareToEmployeeId: selectedEmpId,
        FileShareId: fileCheckbox
    };
    $('#modalEmployee').modal('hide');
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });

    ajaxPost(ajaxUrlFileShare, model, function (response) {
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(response.message);
            $.each(selectedEmpId, function (index, val) {
                $('#' + val).removeClass("fa fa-check-square");
                $('#' + val).addClass("fa fa-eye");
                $('#' + val).removeClass("selectShare");
                $('#btn-' + val).removeClass("btn-success");
                $('#btn-' + val).addClass("btn-info");
                $('#row-' + val).children('td, th').css('background-color', '');
                selectedEmpId = [];
                fileCheckbox = [];
            });
            mApp.unblockPage();
        } else {
            DangerNotif(response.message);
            $.each(selectedEmpId, function (index, val) {
                $('#' + val).removeClass("fa fa-check-square");
                $('#' + val).addClass("fa fa-eye");
                $('#' + val).removeClass("selectShare");
                $('#btn-' + val).removeClass("btn-success");
                $('#btn-' + val).addClass("btn-info");
                $('#row-' + val).children('td, th').css('background-color', '');
                selectedEmpId = [];
                fileCheckbox = [];
            });
            mApp.unblockPage();
        }
    });

}

function ModalClose() {
    $.each(selectedEmpId, function (index, val) {
        $('#' + val).removeClass("fa fa-check-square");
        $('#' + val).addClass("fa fa-eye");
        $('#' + val).removeClass("selectShare");
        $('#btn-' + val).removeClass("btn-success");
        $('#btn-' + val).addClass("btn-info");
        $('#row-' + val).children('td, th').css('background-color', '');
    });

    selectedEmpId = [];
    fileCheckbox = [];
    $('#modalEmployee').modal('hide');
}
let openDataLookup = function () {
    $('#modalEmployee').modal('show');
    bindDataEmployee();
};

function MoveFileToMyDam() {
    $('input[type="checkbox"]:checked').each(function () {
        fileCheckbox.push($(this).attr("value"));
    });

    if (fileCheckbox.length === 0) {
        return DangerNotif("Belum ada file yang dipilih");
    }

    var model = {
        FileShareId: fileCheckbox
    };

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    
    ajaxPost(ajaxUrlFileMoveToMyDam, model, function (response) {
        response = parseJson(response);
      
        if (response.success) {
            SuccessNotif(response.message);
            fileCheckbox = [];
            $('input[type="checkbox"]:checked').prop('checked', false);
            mApp.unblockPage();
        } else {
            DangerNotif(response.message);
            fileCheckbox = [];
            $('input[type="checkbox"]:checked').prop('checked', false);
            mApp.unblockPage();
        }
    });

}

$(document).ready(function () {

    $('.btnLookupEmp').on('click', function () { openDataLookup(); });
    $('.findEmp').on('click', function () { bindDataEmployee(); });
    $('.btnMoveToMyDam').on('click', function () {
        MoveFileToMyDam();
    });
});
