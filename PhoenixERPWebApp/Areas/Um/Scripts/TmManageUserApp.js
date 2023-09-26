let loadData = function () {
    //let dataSearch = $('.searchbox').val();
    let dataSearch = "";
    let urlApi = $.helper.resolveApi("~ManageUser/Get/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~ManageUser/Get");

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            if (data.length === 0) {
                bindEmptyTable();
            }
            else {
                sessionStorage.setItem('dtUApp', JSON.stringify(data));
                bindTable(data);
                if ($.fn.dataTable.isDataTable('#dt_basic') === false) {
                    $('#dt_basic').dataTable({ pageLength: 10 });
                }
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
    }, 355));
};

let bindEmptyTable = function () {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="6" class="text-center">there is no data</td></tr>');
};

let bindTable = function (data) {
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);

    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.id + "</td>";
        row = row + "<td>" + val.aliasName + "</td>";
        row = row + "<td>" + val.email + "</td>";
        row = row + "<td>" + val.groupName + "</td>";
        //row = row + "<td>" + val.isMsUser + "</td>";
        row = row + "<td>";

        if (dt.isDelete) row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='delete' type='button' onclick='removeData(\"" + $.trim(val.id) +"\")'><i class='fa fa-trash-alt'></i></button>";
        if (dt.isEdit) row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + $.trim(val.id) + "\")'><i class='fa fa-edit'></i></button>";
        if (dt.isEdit) row = row + "<button class='m-btn btn btn-warning btn-sm m-btn--sm pull-right' title='reset password' type='button' onclick='resetPassword(\"" + $.trim(val.id) + "\",\"" + val.aliasName + "\")'><i class='fa fa-address-card'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let saveData = function () {
    if ($('#tbUserId').val() === '') {
        swal(
            'Information',
            'User Id cannot empty!',
            'error'
        );
        return false;
    }
    if ($('#ddlGroup').val() === '') {
        swal(
            'Information',
            'Please select group before save!',
            'error'
        );
        return false;
    }

    let model = {
        Id: $('#tbUserId').val(),
        AliasName: $('#tbAliasName').val(),
        GroupId : $('#ddlGroup').val(),
        Email : $('#tbEmail').val(),
        EmployeeBasicInfoId: $('#tbEmpId').val(),
        IsMsUser: $('#cbIsMsUser').prop('checked'),
        IsLocked: $('#cbIsLocked').prop('checked'),
        IsUserNeedTimesheet: $('#cbIsUserNeedTimesheed').prop('checked')
    };
   
    
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~/ManageUser/Save"),
        data: JSON.stringify(model),
        beforeSend: function () {
            // Handle the beforeSend event
            $('#modalAdd').modal('hide');
            mApp.blockPage({
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });
        },
        success: function (r) {
            clearInputForm();

            toastr.success('Data has been success save', "Information");
            window.location.href = "/um/manageuser";
        },
        error: function (r) {
            console.log('masuk error');
            console.log(r);
            swal(
                '' + r.status,
                r.statusText,
                'error'
            );
        },
        complete: function () {
            mApp.unblockPage();
        }
    });

};

let editedData = function (empId) {
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: $.helper.resolveApi("~ManageGroup/Get"),
        success: function (data) {
            var ddl = $('#ddlGroup');
            ddl.empty();
            $(ddl).append($('<option>', {
                value: "",
                text: "SELECT GROUP"
            }));
            $.each(data, function (i, item) {
                $(ddl).append($('<option>', {
                    value: item.id,
                    text: item.groupName
                }));
            });

            var listUapp = sessionStorage.getItem('dtUApp');
            var dt = JSON.parse(listUapp);
            var vm = dt.find(c => c.id === empId);

            $('#hidId').val(empId);
            $('#tbUserId').val(empId);
            $('#tbEmail').val(vm.email);
            $('#tbAliasName').val(vm.aliasName);
            $('#tbEmpId').val(vm.employeeBasicInfoId);
            if (vm.isMsUser) { $('#cbIsMsUser').prop('checked', true); } else { $('#cbIsMsUser').prop('checked', false); }
            if (vm.isLocked) { $('#cbIsLocked').prop('checked', true); } else { $('#cbIsLocked').prop('checked', false); }
            if (vm.isUserNeedTimesheet) { $('#cbIsUserNeedTimesheed').prop('checked', true); } else { $('#cbIsUserNeedTimesheed').prop('checked', false); }
            

            $('#ddlGroup').val(vm.groupId);
            $('#ddlGroup').trigger('change');
            $('#modalAdd').modal('show');
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
let openFormApp = function () {
    clearInputForm();
    bindDataGroup();
    $('#modalAdd').modal('show');
};
let clearInputForm = function () {
    $('#hidId').val("0");
    $('#tbUserId').val('');
    $('#tbEmail').val('');
    $('#tbAliasName').val('');
    $('#tbEmpId').val('');
    $('#cbIsMsUser').prop('checked', false);
};
let removeData = function (id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You want delete this record!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
         $.ajax({
             type: "GET",
             dataType: 'json',
             url: $.helper.resolveApi("~ManageUser/Delete/" + id),
             success: function (data) {
                 window.location.href = "/um/manageuser";
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
        }
    });

    
};

let bindDataGroup = function () {
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: $.helper.resolveApi("~ManageGroup/Get"),
        success: function (data) {
            var ddl = $('#ddlGroup');
            ddl.empty();
            $(ddl).append($('<option>', {
                value: "",
                text: "SELECT GROUP"
            }));
            $.each(data, function (i, item) {
                $(ddl).append($('<option>', {
                    value: item.id,
                    text: item.groupName
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
    });
};

let resetPassword = function (id, aliasName) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You want to reset password this user " + aliasName,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Reset it!'
    }).then((result) => {
        if (result.value) {
            let urlApi = $.helper.resolveApi("~ManageUser/ResetPasswordUser/" + id);
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: urlApi,
                beforeSend: function () {
                    // Handle the beforeSend event
                    mApp.blockPage({
                        overlayColor: "#000000",
                        type: "loader",
                        state: "primary",
                        message: "Processing..."
                    });
                },
                success: function (r) {
                    toastr.success('Reset Password has been success', "Information");
                    window.location.href = "/um/manageuser";
                },
                error: function (r) {
                    console.log('masuk error');
                    console.log(r);
                    swal(
                        '' + r.status,
                        r.statusText,
                        'error'
                    );
                },
                complete: function () {
                    mApp.unblockPage();
                }
            });

        }
    });

    //var stat = confirm('are you sure want to reset password this user ' + aliasName);
    //if (stat) {
    //    let urlApi = $.helper.resolveApi("~ManageUser/ResetPasswordUser/" + id);    
    //    mApp.blockPage({
    //        overlayColor: "#000000",
    //        type: "loader",
    //        state: "primary",
    //        message: "Processing..."
    //    }),
    //        $.ajax({
    //        type: "GET",
    //        dataType: 'json',
    //        url: urlApi,
    //        success: function (data) {
    //            alert('Reset Password has been success');
    //            window.location.href = "/um/manageuser";
    //            },
    //            error: function (e, t, s) {
    //                swal(
    //                    'Information',
    //                    'Ooops, something went wrong !',
    //                    'info'
    //                );
    //            }
    //        }).then(setTimeout(function () {
    //            mApp.unblockPage();
    //        }, 355));
    
};

function setAccessPage() {
    var dt = sessionStorage.getItem('uniquePage');

    dt = JSON.parse(dt);
    if (dt.isRead) {
        loadData();
    }
    $('#ddlGroup').select2({
        allowClear: true
    });
    $('.clearEmpId').on('click', function () { $('#tbEmpId').val(''); });
    if (dt.isAdd) $('.btn-save').on('click', function () { saveData(); });    
    else {
        $('.btn-save').hide();
        $('.btn-formAdd').hide();        
    }

};
$(document).ready(function () {
    var pun = $('#hidPageUniqueName').val();
    if (pun === '' || pun === undefined || pun === null) {
        sessionStorage.removeItem('uniquePage');
    }
    else {
        let urlApi = $.helper.resolveApi("~ManageUserAccess/GetMenuAccessByUniqueName/" + pun);
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: urlApi,
            success: function (data) {
                sessionStorage.setItem('uniquePage', JSON.stringify(data));
                setAccessPage();
            },
            error: function (e, t, s) {
                console.error('Error Getting INformation Page Unique Name FOr User Access');
                console.log(e);
                console.log(t);
                console.log(s);
            }
        });
    }
    
});
