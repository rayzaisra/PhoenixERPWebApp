let loadData = function () {
    let urlApi = $.helper.resolveApi("~ManageTemplateApproval/Get");
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
        }, 4e3));
};


let bindEmptyTable = function () {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="8" class="text-center">there is no data</td></tr>');
};

let bindTable = function (data) {
    var dt = sessionStorage.getItem('uniquePage');

        dt = JSON.parse(dt);
    
   
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.menuName + "</td>";
        row = row + "<td>" + val.templateName + "</td>";
        row = row + "<td>" + val.forDivisionName + "</td>";
        row = row + "<td>" + val.isNotifyByWeb + "</td>";
        row = row + "<td>" + val.isNotifyByEmail + "</td>";
        row = row + "<td>" + val.dueDate + "<i> - day(s)</i></td>";
        row = row + "<td>" + val.reminder + "<i> - day(s)</i></td>";
        if (val.approvalType === 1) row = row + "<td>SQUANCE</td>";
        else row = row + "<td>PARALLEL</td>";

        if (val.approvalCategory === 1) row = row + "<td>PM</td>";
        else if (val.approvalCategory === 2) row = row + "<td>HRIS</td>";
        else row = row + "<td>FINANCE</td>";

        //row = row + "<td>" + val.minBudget + "</td>";
        //row = row + "<td>" + val.maxBudget + "</td>";

        row = row + "<td>";
        if(dt.isDelete) row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='delete' type='button' onclick='removeData(\"" + val.id +"\")'><i class='fa fa-trash-alt'></i></button>";
        if(dt.isEdit) row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + val.id + "\")'><i class='fa fa-edit'></i></button>";
        if(dt.isEdit) row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='detail' type='button' onclick='detailData(\"" + val.id + "\")'><i class='fa fa-eye'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};


let editedData = function (id) {
    sessionStorage.setItem('IdTempApp', JSON.stringify(id));
    window.location.href = "/um/TemplateApproval/AdditionalTemplate";
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
             url: $.helper.resolveApi("~ManageTemplateApproval/Delete/" + id),
             success: function (data) {
                 toastr.success('Data has been success remove', "Information");          
                 loadData();
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

let detailData = function (id) {
    sessionStorage.setItem('IdxTemp', JSON.stringify(id));
    window.location.href = '/um/templateapproval/detailconditional';
};

$(document).ready(function () {
    
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);
    loadData();
    if (dt.isRead) {

        sessionStorage.removeItem('IdxTemp');
        sessionStorage.removeItem('IdTempApp');
        sessionStorage.removeItem('dsCurrentUser');
    }
    if (dt.isAdd === false) {
        $('.btnadd-template').hide();
    }
    
   

});
