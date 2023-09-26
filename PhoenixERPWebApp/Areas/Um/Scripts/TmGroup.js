let loadData = function () {
    let dataSearch = $('.searchbox').val();
    let urlApi = $.helper.resolveApi("~ManageGroup/Get/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~ManageGroup/Get");

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
            if (data.length === 0) {
                bindEmptyTable();
            }
            else {
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
        }, 2e3));
};

let bindEmptyTable = function () {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="2" class="text-center">there is no data</td></tr>');
};

let bindTable = function (data) {

    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);

    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.groupName + "</td>";
        row = row + "<td>";

        if (dt.isDelete) row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='delete' type='button' onclick='removeData(\"" + val.id +"\")'><i class='fa fa-trash-alt'></i></button>";
        if (dt.isEdit) row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + val.id + "\",\"" + val.groupName +"\")'><i class='fa fa-edit'></i></button>";

        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let selectedAll = function () {
    let cbhead = $('#cbHead').prop('checked');
    let $table = $('#dt_basic');
    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbhead);
};

let saveData = function () {
    if ($('#tbGroupName').val() === '') {
        swal(
            'Information',
            'Group Name cannot empty!',
            'error'
        );
        return false;
    }

    let model = { Id: $('#hidIdGroup').val(), GroupName: $('#tbGroupName').val() };

    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~/ManageGroup/Save"),
        data: JSON.stringify(model),
        success: function (r) {
            $('#hidIdGroup').val('0');
            $('#tbGroupName').val('');
            $('#myModal').modal('hide');
            toastr.success('Data has been success save', "Information");
            window.location.href = "/um/managegroup";
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
    });

};


let editedData = function (id, groupName) {
    $('#hidIdGroup').val(id);
    $('#tbGroupName').val(groupName);    
    $('#myModal').modal('show');
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
             url: $.helper.resolveApi("~ManageGroup/Delete/" + id),
             success: function (data) {
                 window.location.href = "/um/managegroup";
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

let openForm = function () {
    $('#myModal').modal('show');
    $('#hidIdGroup').val('0');
    $('#tbGroupName').val('');    
};

let setAccessPage = function () {
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);
    if (dt !== null) {
        if (dt.isRead) loadData();
        if (dt.isAdd) $('.btn-save').on('click', function () { saveData(); });
        else {
                $('.btn-save').hide();
                $('.btn-formadd').hide();
        }
        $('.cb-head').on('click', function () { selectedAll(); });        
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
