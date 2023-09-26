let loadData = function () {
    let keyvalue = $('#tbcari').val();
    let urlApi = $.helper.resolveApi("~ManageGroupAccess/GetListUserNotHavingGroup");
    if (keyvalue.length !== 0) 
        urlApi = $.helper.resolveApi("~ManageGroupAccess/GetListUserNotHavingGroup/"+ keyvalue);

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
                bindTable(data);
                $('#dtd').dataTable({ pageLength: 10 });
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

let bindTable = function (data) {
    let tbl = $('#dtd>tbody');
    tbl.empty();
    let no = 1;
    $.each(data, function (x, r) {
        let row = '<tr>' +
            '<td>'+  no +'</td>' +
            '<td>' + r.employeeName +'</td>' +
            '<td>' + r.nickName +'</td>' +
            '<td>' + r.email +'</td>' +
            '<td>' + r.jobTitleName +'</td>' +
            '<td>' + r.divisiName +'</td>' +
            '<td>' +
            '    <label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-' + r.employeeId + '">' +
            '        <input type="checkbox" data-code="' + r.employeeId + '"  data-name="' + r.employeeName + '"  data-email="' + r.email + '"  data-alias="' + r.nickName + '" class="m-group-checkable cbitem"><span></span>' +
            '                            </label>' +
            '                        </td>' +
                '                    </tr>';
        no++;
        tbl.append(row);
    });

};

let findData = function () {
    let key = $('#tbcari').val();
    key = key.toLowerCase();
    var data = [];
    var table = $("#dtd>tbody");
    table.find('tr').each(function (rowIndex, r) {
        var cols = [];
        let isada = false;
        $(this).find('td').each(function (colIndex, c) {
            c = c.textContent.toLowerCase();
            var n = c.includes(key);
            if (n) isada = true;
            //cols.push(c.textContent);
        });
        if (isada) {
            $(this).show();
        } else {
            $(this).hide();
        }
        //data.push(cols);
    });
};

let saveData = function () {
    let $table = $('#dtd');
    var $theCheckbox = $table.find('.cbitem');
    let groupId = sessionStorage.getItem('setToGroup');
    let vm = [];
    $.each($theCheckbox, function (r, x) {
        if ($(x).prop('checked') === true) {
            let empId = $(x).attr('data-code');
            let empName = $(x).attr('data-name');
            let email = $(x).attr('data-email');
            let nickName = $(x).attr('data-alias');
            if (nickName.length === 0 || nickName === '')  nickName = empName;

            let model = {
                Id: email,
                AliasName: nickName,
                GroupId: groupId,
                Email: email,
                EmployeeBasicInfoId: empId,
                IsMsUser : false
            };
            vm.push(model);
        }
    });

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
        url: $.helper.resolveApi("~/ManageUser/BulkSave"),
            data: JSON.stringify(vm),
            success: function (r) {
                toastr.success('Data has been success save', "Information");
                window.location.href = "/um/mappinggroupuser";
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
        }, 12e3));

};

$(document).ready(function () {
    let groupName = sessionStorage.getItem('setToGroupName');
    $('#hgroupname').text(groupName);

    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);
    if (dt !== null) {
        loadData();

        if (dt.isAdd) {
            $('.btn-save').on('click', function () { saveData(); });
            $('.btn-back').on('click', function () {
                sessionStorage.removeItem('setToGroup');
                sessionStorage.removeItem('setToGroupName');
                window.location.href = "/um/mappinggroupuser";
            });
            $('#basic-addon2').on('click', function () { findData(); });

            $('#tbcari').keyup(function (e) {
                if (e.which === 8 && this.value === false) {
                    return false;
                } else {
                    findData();
                }
            });
        }
    }

});
