let loadData = function () {
    let dataSearch = $('.searchbox').val();
    let urlApi = $.helper.resolveApi("~ManageMenu/Get/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~ManageMenu/Get");

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
            bindParentMenu(data);
            if (data.length === 0) {
                bindEmptyTable();

            }
            else {
                bindTable(data);
                sessionStorage.setItem('dtMenu', JSON.stringify(data));
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
        }, 2e3));
};

let bindParentMenu = function (data) {
    var ddl = $('#ddlParentMenuId');
    ddl.empty();
    $(ddl).append($('<option>', {
        value: "0",
        text: "ROOT"
    }));

    $.each(data, function (i, item) {
        $(ddl).append($('<option>', {
            value: item.id,
            text: item.parentName +' => '+item.menuName
        }));
    });
};

let bindEmptyTable = function () {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="5" class="text-center">there is no data</td></tr>');
};

let bindTable = function (data) {
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.menuName + "</td>";
        row = row + "<td>" + val.menuLink + "</td>";
        row = row + "<td>" + val.menuUnique + "</td>";
        row = row + "<td>" + val.parentName + "</td>";
        row = row + "<td style='width:125px'>";
        if (dt.isAdd) row = row + "<button class='m-btn btn btn-info btn-sm m-btn--sm pull-right' title='manage approval' type='button' onclick='directToPageApproval(\"" + val.id + "\",\"" + val.menuName + "\")'><i class='fa fa-id-card'></i></button>";
        if (dt.isDelete) row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='delete' type='button' onclick='removeData(\"" + val.id +"\")'><i class='fa fa-trash-alt'></i></button>";
        if(dt.isEdit) row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + val.id + "\")'><i class='fa fa-edit'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let directToPageApproval = function (menuId, menuName) {
    var vm = { menuId: menuId, menuName: menuName };
    sessionStorage.setItem('dataMenuApproval', JSON.stringify(vm));
    window.location.href = "/um/templateapproval/";
};

let saveData = function () {
    let mn = $('#tbMenuName').val();
    if (mn === '') {
        swal('WARNING', 'Group Name cannot empty!','error');
        return false;
    }
    let ml = $('#tbMenuLink').val();
    if (ml === '') {
        swal('WARNING', 'Menu Link cannot empty!', 'error');
        return false;
    }
    let mu = $('#tbMenuUnique').val();
    if (mu === '') {
        swal('WARNING', 'Menu Unique cannot empty!', 'error');
        return false;
    }
    let ddlParent = $('#ddlParentMenuId').val();

    let model = {
        Id: $('#hidId').val(),
        MenuName: mn,
        MenuLink: ml,
        MenuUnique: mu,
        ParentId: ddlParent
    };
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~ManageMenu/Save"),
        data: JSON.stringify(model),
        success: function (r) {
            $('#hidId').val('0');
            $('#tbMenuName').val('');
            $('#tbMenuLink').val('');
            $('#tbMenuUnique').val('');    
            $('#myModal').modal('hide');
            toastr.success('Data has been success save', "Information");            
            window.location.href = "/um/managemenu";

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
        }, 2e3));

};

let editedData = function (id) {
    let urlApi = $.helper.resolveApi("~ManageMenu/Get");
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
            success: function (dataMenu) {
                bindParentMenu(dataMenu);
                let data = sessionStorage.getItem('dtMenu');
                let dt = JSON.parse(data);
                let dm = dt.find(x => x.id === id);

                $('#hidId').val(id);
                $('#tbMenuName').val(dm.menuName);
                $('#tbMenuLink').val(dm.menuLink);
                $('#tbMenuUnique').val(dm.menuUnique);
                //$('#ddlParentMenuId').select2('data', { id: id, text: dm.parentName + ' => ' + dm.menuName });
                $('#ddlParentMenuId').val(id);
                $('#ddlParentMenuId').select2().trigger('change');
                $('#myModal').modal('show');
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

let removeData = function (id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
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
             url: $.helper.resolveApi("~ManageMenu/Delete/" + id),
             success: function (data) {
                 toastr.success('Data has been success remove', "Information");          
                 window.location.href = "/um/managemenu";
             },
             error: function (e) {
                 console.log(e);
                 //swal(
                 //    'Information',
                 //    'Ooops, something went wrong !',
                 //    'info'
                 //);
             }
         });
        }
    });
    
};

let openForm = function () {
    $('#myModal').modal('show');
    $('#hidId').val('0');
    $('#tbMenuName').val('');    
    $('#tbMenuLink').val('');    
    $('#tbMenuUnique').val('');    
    $('#ddlParentMenuId').select2('data', { id: '0', text: 'ROOT'});    
};

let setAccessPage = function () {
    var dt = sessionStorage.getItem('uniquePage');
    if (dt !== undefined || dt !== null) {
        dt = JSON.parse(dt);
        
        if (dt !== null) {
            if (dt.isRead) loadData();
            if (dt.isAdd) $('.btn-save').on('click', function () { saveData(); });
            else {
                $('.btn-save').hide();
                $('.btn-formadd').hide();
                $('.btn-manage-index').hide();                
            }
            $("#ddlParentMenuId").select2({
                allowClear: true
            });
            $('.find-data').on('click', function () {
                loadData();
            });
        }
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
