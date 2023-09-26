let loadData = function () {
    let dataSearch = $('.searchbox').val();
    let urlApi = $.helper.resolveApi("~ManageGroupAccess/GetListUserHavingGroup");

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
            sessionStorage.setItem('dataGrouping', JSON.stringify(data));
            bindDropdownGroup(data);
            bindListGrouping(data);
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

let bindDropdownGroup = function (data) {
    var ddl = $('#ddlGroup');
    ddl.empty();
    $(ddl).append($('<option>', {
        value: "0",
        text: "ALL"
    }));

    $.each(data, function (i, item) {
        $(ddl).append($('<option>', {
            value: item.id,
            text: item.groupName
        }));
    });
};


let bindListGrouping = function (data) {
    let divTemp = $('#dvLgm');
    divTemp.empty();
    $.each(data, function (x, r) {
        let result = templateTable(r);
        divTemp.append(result);
    });
};

let templateTable = function (data) {
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);

    let tbl = '<table class="table table-bordered">';
        tbl = tbl + '<thead>' ;
        tbl = tbl + '        <tr>' ;
        tbl = tbl + '            <th>';
        if (dt.isAdd) {
            tbl = tbl + '<button class="m-btn btn btn-primary btn-sm m-btn--sm" onclick="addUserToGroup(\'' + data.id + '\',\'' + data.groupName + '\')" title="add new user to this group ' + data.groupName + '">' ;
            tbl = tbl + '<i class="la la-plus"></i>' ;
            tbl = tbl + '</button>';
        }

        tbl = tbl + '                        &nbsp;&nbsp;<i class="fa fa-users" style="color: #47b7bc"></i>&nbsp;&nbsp;<b class="text-uppercase">' + data.groupName + '</b>' ;
        tbl = tbl + '            <span class="pull-right">Total : ' + data.listUsers.length + '</span>' ;
        tbl = tbl + '        </th>' ;
        tbl = tbl + '    </tr>' ;
        tbl = tbl + '                </thead>' ;
        tbl = tbl + '                    <tbody> <tr><td><div style="max-height:200px;overflow:auto">';
    if (data.listUsers.length === 0) {
        tbl = tbl + '<span class="col text-center">there is no user in this group<span>';
    }
    else {
        tbl = tbl + '    <table class="table table-striped table-hover">' +
        '        <thead>' +
        '            <tr>' +
        '                <th>#NO</th>' +
        '                <th>#EMPLOYEE NAME</th>' +
        '                <th>#NICK NAME</th>' +
        '                <th>#EMAIL</th>' +
        '                <th>#JOB TITLE</th>' +
        '                <th>#DIVISION</th>' +
        '                <th>#<i class="fa fa-cogs"></i></th>' +
        '            </tr>' +
        '        </thead>' +
        '        <tbody>';
    let no = 1;
    $.each(data.listUsers, function (x, r) {
        tbl = tbl + '<tr>';
        tbl = tbl + '                <td>' + no + '</td>';
        tbl = tbl +    '                <td>' + r.employeeName+'</td>' ;
        tbl = tbl +    '                <td>' + r.nickName +'</td>' ;
        tbl = tbl +    '                <td>' + r.email +'</td>' ;
        tbl = tbl +    '                <td>' + r.jobTitleName +'</td>' ;
        tbl = tbl +    '                <td>' + r.divisiName +'</td>' ;
        tbl = tbl + '                <td>';
        if(dt.isEdit) tbl = tbl + ' <button class="btn btn-sm m-btn--sm md-btn-icon-default btn-default" onclick="editUserAccess(\'' + r.groupId + '\',\'' + r.userId + '\',\'' + r.employeeName + '\',\'' +data.groupName + '\')"><i class="fa fa-edit"></i></button>' ;
        if(dt.isDelete) tbl = tbl + '<button class="btn btn-sm m-btn--sm md-btn-danger btn-danger"  onclick="deleteUserFromGroup(\'' + r.groupId + '\',\'' + r.userId + '\')"><i class="fa fa-times"></i></button>' ;

        tbl = tbl + '                </td>';
        tbl = tbl +     '            </tr>';
        no++;
    });

        tbl = tbl + '</tbody></table> ';
    }
           '</div></td></tr>'+
           '</tbody> '+
        '</table>';

    return tbl;
};

let addUserToGroup = function (groupId,groupName) {
    sessionStorage.setItem('setToGroup', groupId);
    sessionStorage.setItem('setToGroupName', groupName);
    sessionStorage.removeItem('setUserToGroup');
    sessionStorage.removeItem('dataGrouping');
    
    window.location.href = '/um/mappinggroupuser/adduser';
};
let editUserAccess = function (groupId,userId,empName,groupName) {
    sessionStorage.setItem('setToGroup', groupId);
    sessionStorage.setItem('setUserToGroup', userId);
    sessionStorage.setItem('setToGroupName', groupName);
    sessionStorage.setItem('setToEmpName', empName);
    sessionStorage.removeItem('dataGrouping');
    window.location.href = '/um//manageuseraccess/edituseraccess';
};

let deleteUserFromGroup = function (groupid,userid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't delete this user from group!",
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
             url: $.helper.resolveApi("~ManageUser/DeleteFromGroup/" + groupid + "/" + userid),
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

let selectedChangeGroup = function (groupId) {
    let ds = sessionStorage.getItem('dataGrouping');
    ds = JSON.parse(ds);
    if (ds === null || ds === undefined) ds = [];

    if (groupId === "0") {
        bindListGrouping(ds);
    }
    else {
        let data = ds.find(x => x.id === groupId);
        let newds = [];
        newds.push(data);
        bindListGrouping(newds);
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
                sessionStorage.removeItem('dataGrouping');
                var dt = sessionStorage.getItem('uniquePage');
                dt = JSON.parse(dt);
                if (dt !== null) {
                    if (dt.isRead) {
                        loadData();

                        $('#ddlGroup').select2({
                            allowClear: true,
                            placeholder: 'ALL'
                        });
                        $('#ddlGroup').on('select2:select', function (e) {
                            if (e.params.data.id === undefined)
                                selectedChangeGroup("0");
                            else
                                selectedChangeGroup(e.params.data.id);
                        });
                    }
                }
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
