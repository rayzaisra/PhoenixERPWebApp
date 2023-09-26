let loadData = function () {
    let dataSearch = $('.searchbox').val();
    let urlApi = $.helper.resolveApi("~groupchat/Get/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~groupchat/Get");

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

let loadDataMember = function () {
    let urlApi = $.helper.resolveApi("~ManageUser/get");
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
            
            let $dll = $('#ddlMember');
            $dll.append("<option value='0'>SELECT MEMBER</option>");
            $.each(data, function (x, r) {
                $dll.append("<option value='" + r.id + "'>" + r.aliasName + "(" + r.id + ")</option>");
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
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 2e3));
};

let addMemberToGroup = function () {

    let dataDdl = $('#ddlMember').select2('data');
    var idx = dataDdl[0].id;
    var txt = dataDdl[0].text;

    let $tbl = $('.tbl-list-member>tbody');
    var tempClass = idx.replace('@','-');
    tempClass = tempClass.replace('.', '-');
    tempClass = tempClass.replace('.', '-');
    tempClass = tempClass.replace('.', '-');

    let isreadyRow = $tbl.find('.'+tempClass);
    if (isreadyRow.length > 0) {
        swal(
            'Information',
            'Ooops, member '+ txt +' already in list!',
            'info'
        );
        return false;
    }
    bindTableListMember(tempClass, idx, txt);
};

let bindTableListMember = function (tempClass, idx, txt) {
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);
    var row = "<tr class='" + tempClass + "'>";
    row = row + "<td><input type='hidden' value='" + idx + "' class='HidIdMember'/> " + txt + "</td>";
    row = row + "<td>";
    if(dt.isDelete) row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='delete' type='button' onclick='removeMember(\"" + tempClass + "\")'><i class='fa fa-trash-alt'></i></button>";
    row = row + "</td>";
    row = row + "</tr>";
    let $tbl = $('.tbl-list-member>tbody');
    $tbl.append(row);
};
let removeMember = function (idx) {
    $('.' + idx).remove();
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
        if (dt.isEdit) row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + val.id + "\")'><i class='fa fa-edit'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};


let saveData = function () {
    if ($('.in-groupname').val() === '' || $('.in-groupname').val().length === 0) {
        swal(
            'Information',
            'Group Chat Name cannot empty!',
            'error'
        );
        return false;
    }
    //validate member....
    let countMember = $('.tbl-list-member>tbody>tr');
    if (countMember.length === 0) {
        swal(
            'Information',
            'There is no member in list!',
            'error'
        );
        return false;
    }
    var listMember = [];
    var dataColls = $('.tbl-list-member>tbody').find('.HidIdMember');
    $.each(dataColls, function (x, r) {
        var vm = { GroupId: "0", MemberCode: $(r).val(), MemberName: '' };
        listMember.push(vm);
    });

    let model = { Id: $('#HidIdGroup').val(), GroupName: $('.in-groupname').val(), Members: listMember };

    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~/groupchat/Save"),
        data: JSON.stringify(model),
        success: function (r) {
            window.location.href = "/um/groupchatting";
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

let editedData = function (id) {
    window.location.href = "/um/groupchatting/add/" + id;
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
             url: $.helper.resolveApi("~groupchat/Delete/" + id),
             success: function (data) {
                 loadData();
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

let bindEditData = function (id) {
    let urlApi = $.helper.resolveApi("~groupchat/GetById/" + id);

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
            $('.in-groupname').val(data.groupName);
            $.each(data.members, function (x, r) {
                var tempClass = r.memberCode.replace('@', '-');
                tempClass = tempClass.replace('.', '-');
                tempClass = tempClass.replace('.', '-');
                tempClass = tempClass.replace('.', '-');
                bindTableListMember(tempClass,r.memberCode,r.memberName);
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
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 2e3));
};

function generateGuid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4();
}