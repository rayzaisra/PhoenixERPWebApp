let bindMenuByGroup = function (groupId) {
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
        type: "GET",
        dataType: 'json',
        url: $.helper.resolveApi("~ManageGroupAccess/Get/" + groupId),
        success: function (data) {
            sessionStorage.setItem('dtGAM', JSON.stringify(data));
            bindTable(data);
            onrowdatabound(data);
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
        }, 500));
};


let bindTable = function (data) {
    let tempMenuId = [];
    let getParentMenu = function (dt) {
        for (var i in dt) {
            for (var m in data) {
                if (dt[i].menuId === data[m].parentId) {
                    tempMenuId.push(dt[i].menuId);
                }
            }
        }
    };
    getParentMenu(data);
    var uniqueMenuIds = [];
    $.each(tempMenuId, function (i, el) {
        if ($.inArray(el, uniqueMenuIds) === -1) uniqueMenuIds.push(el);
    });

    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        let isParent = false;

        $.each(uniqueMenuIds, function (x, xx) {
            if (val.menuId === xx) isParent = true;
        });

        var row = "";
        if (isParent) {
            row = "<tr style='display:none'>";
        } else {
            row = "<tr>";
        }
        row = row + "<td><label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-" + val.menuId + "' >";
        row = row + "<input type='checkbox' value='' class='m-group-checkable cb-item cbitem' id='cbItem-" + val.menuId + "' data-code='" + val.menuId + "' onclick='selectedItem(\"" + val.menuId + "\" )'>";
        row = row + "<span></span>";
        row = row + "</label>";
        row = row + "</td>";
        row = row + "<td>" + val.menuName + "</td>";
        row = row + "<td>";
        if (!isParent) {
            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-read-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemread mn-" + val.menuId + "' id='cbItem-read-" + val.menuId + "' onclick='selectedItemRead(\"" + val.menuId + "\" )'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "<td>";
        if (!isParent) {
            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-add-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemadd mn-" + val.menuId + "' id='cbItem-add-" + val.menuId + "' onclick='selectedItemAdd(\"" + val.menuId + "\" )'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "<td>";
        if (!isParent) {
            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-edit-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemedit mn-" + val.menuId + "' id='cbItem-edit-" + val.menuId + "' onclick='selectedItemEdit(\"" + val.menuId + "\" )'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "<td>";
        if (!isParent) {
            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-delete-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemdelete mn-" + val.menuId + "' id='cbItem-delete-" + val.menuId + "' onclick='selectedItemDelete(\"" + val.menuId + "\" )'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};
let selectedItem = function (menuId) {
    let cb = $('#cbItem-' + menuId).prop('checked');
    let $table = $('#dt_basic');
    var $cbitem = $table.find('.mn-' + menuId); 
    $cbitem.prop('checked', cb);

    //cek parent menu....
    if (cb === true) {
        checkParentIsSelected(menuId);
    }
};
let selectedItemRead = function (menuId) {
    let cbread = $('#cbItem-read-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId);
    }

};
let selectedItemAdd = function (menuId) {
    let cbread = $('#cbItem-add-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        $('#cbItem-read-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId);
    }
};
let selectedItemEdit = function (menuId) {
    let cbread = $('#cbItem-edit-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        $('#cbItem-read-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId);
    }
};
let selectedItemDelete = function (menuId) {
    let cbread = $('#cbItem-delete-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        $('#cbItem-read-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId);
    }
};

let checkParentIsSelected = function (menuid) {
    let dsMenu = sessionStorage.getItem('dtGAM');
    let dt = JSON.parse(dsMenu);

    let dataMenu = dt.find(x => x.menuId === menuid);
    if (dataMenu.parentId === "0") {
        return;
    }

    let tempMenuId = [];
    let tempParentId = dataMenu.parentId;

    let getParentMenu = function (dt) {
        for (var i in dt) {
            if (dt[i].menuId === tempParentId) {
                tempMenuId.push(dt[i].menuId);
                tempParentId = dt[i].parentId;

                if (dt[i].parentId !== "0") getParentMenu(dt);
            }
        }
    };
    getParentMenu(dt);
    for (let i in tempMenuId) {
        $('#cbItem-' + tempMenuId[i]).prop('checked', true);
        let $table = $('#dt_basic');
        var $cbitem = $table.find('.mn-' + tempMenuId[i]);
        $cbitem.prop('checked', true);

    }
};

function getNestedChildren(arr, parentId) {
    var out = [];
    for (var i in arr) {
        if (arr[i].parentId === parentId) {
            var childMenu = getNestedChildren(arr, arr[i].menuId);
            if (childMenu.length) {
                arr[i].childMenu = childMenu;
            }
            out.push(arr[i]);
        }
    }
    return out;
}

let selectedAll = function () {
    let cbhead = $('#cbSelectALl').prop('checked');
    let $table = $('#dt_basic');

    var $thCheckbox = $table.find('thead input:checkbox'); // checboxes inside table body   
    $.each($thCheckbox , function (i, d) {
        $(this).trigger("click");
    });
    $thCheckbox.prop('checked', cbhead);


    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbhead);
};


let bindGroup = function () {
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
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
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 2e3));
};

let readAll = function () {
    let cbreadAll = $('#cbReadAll').prop('checked');
    let $table = $('#dt_basic');
    var $tdCheckbox = $table.find('.cbitemread'); 
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbreadAll);

};
let addAll = function () {
    let cbreadAll = $('#cbAddAll').prop('checked');
    let $table = $('#dt_basic');
    var $tdCheckbox = $table.find('.cbitemadd');
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbreadAll);
};
let editAll = function () {
    let cbreadAll = $('#cbEditAll').prop('checked');
    let $table = $('#dt_basic');
    var $tdCheckbox = $table.find('.cbitemedit');
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbreadAll);

};

let saveData = function () {
    if ($('#ddlGroup').val() === '') {
        swal(
            'Information',
            'Group not selected!',
            'warning'
        );
        return false;
    }
    let $table = $('#dt_basic');
    var $theCheckbox = $table.find('.cbitem');
    let listmodel= [];
    $.each($theCheckbox, function (r, x) {
        if ($(x).prop('checked') === true) {
            let menuCode = $(x).attr('data-code');
            listmodel.push({
                MenuId: menuCode,
                GroupId: $('#ddlGroup').val(),
                IsRead: $('#cbItem-read-' + menuCode).prop('checked'),
                IsAdd: $('#cbItem-add-' + menuCode).prop('checked'),
                IsEdit: $('#cbItem-edit-' + menuCode).prop('checked'),
                IsDelete: $('#cbItem-delete-' + menuCode).prop('checked'),
            });
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
        url: $.helper.resolveApi("~/ManageGroupAccess/Save"),
        data: JSON.stringify(listmodel),
        success: function (r) {
            toastr.success('Data has been success save', "Information");
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


let onrowdatabound = function (data) {
    $.each(data, function (x, r) {
        $('#cbItem-' + r.menuId).prop('checked', r.isAdd);
        $('#cbItem-read-' + r.menuId).prop('checked', r.isRead);
        $('#cbItem-add-' + r.menuId).prop('checked', r.isAdd);
        $('#cbItem-edit-' + r.menuId).prop('checked', r.isEdit);
        $('#cbItem-delete-' + r.menuId).prop('checked', r.isDelete);
    });
};

$(document).ready(function () {
    bindGroup();
    $('#ddlGroup').select2({
        allowClear : true
    });
    $('#ddlGroup').on('select2:select', function (e) {
        bindMenuByGroup(e.params.data.id);
    });
    $('.lb-head').on('click', function () { selectedAll(); });
    $('.lb-readall').on('click', function () { readAll(); });
    $('.lb-addall').on('click', function () { addAll(); });
    $('.lb-editall').on('click', function () { editAll(); });
    $('.lb-deleteall').on('click', function () { deleteAll(); });
    $('.btn-save-data').on('click', function () { saveData(); });
    
});
