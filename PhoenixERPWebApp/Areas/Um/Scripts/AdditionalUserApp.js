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
        row = row + "<button class='m-btn btn btn-success btn-sm m-btn--sm pull-right' title='select this employee' type='button' onclick='selectEmployee(\"" + val.id + "\",\"" + val.nameEmployee + "\",\"" + val.email + "\")'><i class='fa fa-eye'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};
let selectEmployee = function (empId,empName,email) {
    $('#tbEmpId').val(empId);
    $('#tbAliasName').val(empName);
    $('#tbUserId').val(email);
    $('#tbEmail').val(email);
    $('#modalEmployee').modal('hide');
};

let openDataLookup = function () {
    $('#modalEmployee').modal('show');
    bindDataEmployee();
};

$(document).ready(function () {
    $('.btnLookupEmp').on('click', function () { openDataLookup(); });    
    $('.findEmp').on('click', function () { bindDataEmployee(); });    
    
});
