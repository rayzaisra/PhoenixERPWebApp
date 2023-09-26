let bindMenuByUser = function (userid) {
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
        type: "GET",
        dataType: 'json',
        url: $.helper.resolveApi("~ManageUserAccess/Get/" + userid + "/" + $('#ddlGroup').val()),
            success: function (data) {
                sessionStorage.setItem('dtGAM', JSON.stringify(data));
                generateTableByGrouping(data);
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

let bindTable = function (data, dtcode) {
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

    $('#dtd-' + dtcode + '> tbody').empty();
    let table = $('#dtd-' + dtcode + '> tbody:last-child');
    $.each(data, function (r, val) {
        let isParent = false;

        $.each(uniqueMenuIds, function (x, xx) {
            if (val.menuId === xx) isParent = true;
        });

        var row = "";
        //row = "<tr>";
        if (isParent) {
            row = "<tr style='display:none'>";
        } else {
            row = "<tr>";
        }
        row = row + "<td>";
        row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-" + val.menuId + "' >";
        row = row + "<input type='checkbox' value='' class='m-group-checkable cb-item cbitem' id='cbItem-" + val.menuId + "' data-code='" + val.menuId + "' onclick='selectedItem(\"" + val.menuId + "\",\"" + dtcode + "\")'/>";
        row = row + "<span></span>";
        row = row + "</label>";
        row = row + "</td>";
        row = row + "<td>" + val.menuName + "</td>";

        row = row + "<td>";
        if (!isParent) {

            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-read-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemread mn-" + val.menuId + "' id='cbItem-read-" + val.menuId + "' onclick='selectedItemRead(\"" + val.menuId + "\" ,\"" + dtcode + "\")'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "<td>";

        if (!isParent) {
            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-add-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemadd mn-" + val.menuId + "' id='cbItem-add-" + val.menuId + "' onclick='selectedItemAdd(\"" + val.menuId + "\" ,\"" + dtcode + "\")'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "<td>";

        if (!isParent) {
            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-edit-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemedit mn-" + val.menuId + "' id='cbItem-edit-" + val.menuId + "' onclick='selectedItemEdit(\"" + val.menuId + "\" ,\"" + dtcode + "\")'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "<td>";
        if (!isParent) {

            row = row + "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-item-delete-" + val.menuId + "'><input type='checkbox' value='' class='m-group-checkable cb-item cbitemdelete mn-" + val.menuId + "' id='cbItem-delete-" + val.menuId + "' onclick='selectedItemDelete(\"" + val.menuId + "\" ,\"" + dtcode + "\")'>";
            row = row + "<span></span>";
            row = row + "</label>";
        }
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let selectedItem = function (menuId, tcode) {
    let cb = $('#cbItem-' + menuId).prop('checked');
    let $table = $('#dtd-' + tcode);
    var $cbitem = $table.find('.mn-' + menuId);
    $cbitem.prop('checked', cb);

    //cek parent menu....
    if (cb === true) {
        checkParentIsSelected(menuId, tcode);
    }
    else {
        checkParentUnSelected(menuId, tcode);
        checkChildParentUnSelected(menuId, tcode);
    }
};
let selectedItemRead = function (menuId, tcode) {
    let cbread = $('#cbItem-read-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId, tcode);
    }

};
let selectedItemAdd = function (menuId, tcode) {
    let cbread = $('#cbItem-add-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        $('#cbItem-read-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId, tcode);
    }
};
let selectedItemEdit = function (menuId, tcode) {
    let cbread = $('#cbItem-edit-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        $('#cbItem-read-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId, tcode);
    }
};
let selectedItemDelete = function (menuId, tcode) {
    let cbread = $('#cbItem-delete-' + menuId).prop('checked');
    if (cbread === true) {
        $('#cbItem-' + menuId).prop('checked', true);
        $('#cbItem-read-' + menuId).prop('checked', true);
        checkParentIsSelected(menuId, tcode);
    }
};

let checkParentIsSelected = function (menuid, tcode) {
    let dsMenu = sessionStorage.getItem('dtGAM');
    let dt = JSON.parse(dsMenu);

    let dataMenuGroup = dt.find(x => x.menuId === tcode);
    let dataMenu = dataMenuGroup.listMenuGroup.find(x => x.menuId === menuid);
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
    getParentMenu(dataMenuGroup.listMenuGroup);

    for (let i in tempMenuId) {
        $('#cbItem-' + tempMenuId[i]).prop('checked', true);
        let $table = $('#dtd-' + tcode);
        var $cbitem = $table.find('.mn-' + tempMenuId[i]);
        $cbitem.prop('checked', true);

    }

};

let checkChildParentUnSelected = function (menuid, tcode) {
    let dsMenu = sessionStorage.getItem('dtGAM');
    let dt = JSON.parse(dsMenu);

    let dataMenuGroup = dt.find(x => x.menuId === tcode);
    let datamenu = dataMenuGroup.listMenuGroup.find(x => x.menuId === menuid);
    if (datamenu === undefined) return false;

    let listChildMenu = [];
    $.each(dataMenuGroup.listMenuGroup, function (r, x) {
        if (x.parentId === datamenu.parentId) listChildMenu.push(x);
    });
    let $table = $('#dtd-' + tcode);
    let ischecked = false;

    $.each(listChildMenu, function (x, r) {
        var $cbitem = $table.find('#cbItem-' + r.menuId);
        if ($cbitem.prop('checked') === true) ischecked = true;
    });

    if (!ischecked) {
        var $cb = $table.find('#cbItem-' + datamenu.parentId);
        $cb.prop('checked', false);
        checkParentUnSelected(datamenu.parentId, tcode);
    }
};

let checkParentUnSelected = function (menuid, tcode) {
    let dsMenu = sessionStorage.getItem('dtGAM');
    let dt = JSON.parse(dsMenu);

    let dataMenuGroup = dt.find(x => x.menuId === tcode);
    let datamenu = dataMenuGroup.listMenuGroup.find(x => x.menuId === menuid);
    if (datamenu === undefined) return false;

    let listChildMenu = [];
    $.each(dataMenuGroup.listMenuGroup, function (r, x) {
        if (x.parentId === datamenu.menuId) listChildMenu.push(x);
    });
    let $table = $('#dtd-' + tcode);
    let ischecked = false;

    $.each(listChildMenu, function (x, r) {
        var $cb = $table.find('#cbItem-' + r.menuId);
        $cb.prop('checked', false);
        var $cbitem = $table.find('.mn-' + r.menuId);
        $cbitem.prop('checked', false);
        checkParentUnSelected(r.menuId, tcode);
    });

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
        }, 350));
};

let bindUserByGroup = function (groupId) {
    $('#dt_basic> tbody').empty();
    let $table = $('#dt_basic');
    var $tdCheckbox = $table.find('thead input:checkbox');
    $tdCheckbox.prop('checked', false);

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: $.helper.resolveApi("~ManageUser/GetListUserByGroupId/" + groupId),
            success: function (data) {
                var ddl = $('#ddlUser');
                ddl.empty();
                $(ddl).append($('<option>', {
                    value: "",
                    text: "SELECT USER APP"
                }));

                $.each(data, function (i, item) {
                    $(ddl).append($('<option>', {
                        value: item.id,
                        text: item.aliasName
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
        }, 350));

};

let selectedAll = function (tcode) {

    let cbhead = $('#cbSelectALl-' + tcode).prop('checked');
    let $table = $('#dtd-' + tcode);

    var $thCheckbox = $table.find('thead input:checkbox'); // checboxes inside table body   
    $.each($thCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $thCheckbox.prop('checked', cbhead);


    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbhead);

};
let readAll = function (tcode) {
    let cbreadAll = $('#cbReadAll-' + tcode).prop('checked');
    let $table = $('#dtd-' + tcode);
    var $tdCheckbox = $table.find('.cbitemread');
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbreadAll);

};
let addAll = function (tcode) {
    let cbreadAll = $('#cbAddAll-' + tcode).prop('checked');
    let $table = $('#dtd-' + tcode);
    var $tdCheckbox = $table.find('.cbitemadd');
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbreadAll);
};
let editAll = function (tcode) {
    let cbreadAll = $('#cbEditAll-' + tcode).prop('checked');
    let $table = $('#dtd-' + tcode);
    var $tdCheckbox = $table.find('.cbitemedit');
    $.each($tdCheckbox, function (i, d) {
        $(this).trigger("click");
    });
    $tdCheckbox.prop('checked', cbreadAll);

};
let deleteAll = function (tcode) {
    let cbreadAll = $('#cbDeleteAll-' + tcode).prop('checked');
    let $table = $('#dtd-' + tcode);
    var $tdCheckbox = $table.find('.cbitemdelete');
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
    if ($('#ddlGroup').val() === '') {
        swal(
            'Information',
            'User App not selected!',
            'warning'
        );
        return false;
    }
    let dsMenu = sessionStorage.getItem('dtGAM');
    let dt = JSON.parse(dsMenu);
    let listmodel = [];
    let rootParentList = [];
    $.each(dt, function (xx, rr) {
        let $table = $('#dtd-' + rr.menuId);
        var $theCheckbox = $table.find('.cbitem');

        $.each($theCheckbox, function (r, x) {
            if ($(x).prop('checked') === true) {
                rootParentList.push(rr.menuId);
                let menuCode = $(x).attr('data-code');
                var isRead = $('#cbItem-read-' + menuCode).prop('checked');
                var isAdd = $('#cbItem-add-' + menuCode).prop('checked');
                var isEdit = $('#cbItem-edit-' + menuCode).prop('checked');
                var isDelete = $('#cbItem-delete-' + menuCode).prop('checked');
                if (isRead === undefined) isRead = true;
                if (isAdd === undefined) isAdd = true;
                if (isEdit === undefined) isEdit = true;
                if (isDelete === undefined) isDelete = true;
                listmodel.push({
                    MenuId: menuCode,
                    GroupId: $('#ddlGroup').val(),
                    UserId: $('#ddlUser').val(),
                    IsRead: isRead,
                    IsAdd: isAdd,
                    IsEdit: isEdit,
                    IsDelete: isDelete
                });
            }
        });

    });

    var uniqueNames = [];
    $.each(rootParentList, function (i, el) {
        if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
    });

    $.each(uniqueNames, function (m, n) {
        listmodel.push({
            MenuId: n,
            GroupId: $('#ddlGroup').val(),
            UserId: $('#ddlUser').val(),
            IsRead: true,
            IsAdd: true,
            IsEdit: true,
            IsDelete: true
        });
    });

    if (listmodel.length === 0) {
        swal(
            'Information',
            'There is not selected menu!',
            'warning'
        );
        return false;
    }

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
        url: $.helper.resolveApi("~/ManageUserAccess/Save"),
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
                bindGroup();
                $('#ddlGroup').select2({
                    allowClear: true
                });
                $('#ddlUser').select2({
                    allowClear: true
                });
                $('#ddlUser').on('select2:select', function (e) {
                    bindMenuByUser(e.params.data.id);
                });
                $('#ddlGroup').on('select2:select', function (e) {
                    bindUserByGroup(e.params.data.id);
                });
                $('.btn-save-data').on('click', function () { saveData(); });
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

let generateTableByGrouping = function (data) {
    $('#dvLgm').empty();
    $.each(data, function (x, r) {
        generateTemplateTable(r.groupMenuName, r.menuId);
        bindTable(r.listMenuGroup, r.menuId);
        onrowdatabound(r.listMenuGroup);
    });
};

let generateTemplateTable = function (menuname, tcode) {

    function templateTable() {
        var tbl = '<table class="table table-striped- table-bordered table-hover table-checkable" id="dtd-' + tcode + '">' +
            '<thead>' +
            '<tr>' +
            '<th>' +
            '<label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-head-' + tcode + '" >' +
            '<input type="checkbox" value="" class="m-group-checkable cb-head" name="cbSelectALl" id="cbSelectALl-' + tcode + '">' +
            '<span></span>' +
            '</label>' +
            '</th>' +
            '<th>#MENU NAME</th>' +
            '<th>' +
            'READ ALL<br />' +
            '<label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-readall-' + tcode + '">' +
            '<input type="checkbox" value="" class="m-group-checkable cb-head" name="cbReadAll" id="cbReadAll-' + tcode + '">' +
            '<span></span>' +
            '</label>' +
            '</th>' +
            '<th>' +
            'ADD ALL<br />' +
            '<label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-addall-' + tcode + '">' +
            '<input type="checkbox" value="" class="m-group-checkable cb-head" name="cbAddAll" id="cbAddAll-' + tcode + '">' +
            '<span></span>' +
            '</label>' +
            '</th>' +
            '<th>' +
            'EDIT ALL<br />' +
            '<label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-editall-' + tcode + '">' +
            '<input type="checkbox" value="" class="m-group-checkable cb-head" name="cbEditAll" id="cbEditAll-' + tcode + '">' +
            '<span></span>' +
            '</label>' +
            '</th>' +
            '<th>' +
            'DELETE ALL<br />' +
            '<label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand lb-deleteall-' + tcode + '">' +
            '<input type="checkbox" value="" class="m-group-checkable cb-head" name="cbDeleteAll" id="cbDeleteAll-' + tcode + '">' +
            '<span></span>' +
            '</label>' +
            '</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody></tbody>' +
            '</table>';
        return tbl;
    }

    var tblHeader = '<table class="table table-striped- table-bordered table-hover table-checkable" id="dth-' + tcode + '">' +
        '<thead>' +
        '<tr>' +
        '<th> <i class="fa fa-plus-circle" onclick="expandcollapseDetail(\'trT' + tcode + '\')" id="iplustrT' + tcode + '" style="cursor:pointer;font-size:16px"></i>' +
        '&nbsp;&nbsp;<i class="fa fa-tags" style="color: #47b7bc"></i>&nbsp;&nbsp;<b>' + menuname + '</b>' +
        '</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '<tr>' +
        '<td><div  id="trT' + tcode + '" style="position: relative; display: none; height:400px;overflow-x:auto">' + templateTable() + '</div>' +
        '</td>' +
        '</tr>' +
        '</tbody > ' +
        '</table>';


    $('#dvLgm').append(tblHeader);

    $('.lb-head-' + tcode).on('click', function () { selectedAll(tcode); });
    $('.lb-readall-' + tcode).on('click', function () { readAll(tcode); });
    $('.lb-addall-' + tcode).on('click', function () { addAll(tcode); });
    $('.lb-editall-' + tcode).on('click', function () { editAll(tcode); });
    $('.lb-deleteall-' + tcode).on('click', function () { deleteAll(tcode); });


};

let expandcollapseDetail = function (divname) {
    var div = document.getElementById(divname);
    if (div.style.display === "none") {
        div.style.display = "block";
        $('#iplus' + divname).removeClass("fa-plus-circle");
        $('#iplus' + divname).addClass("fa-minus-circle");
    } else {
        div.style.display = "none";
        $('#iplus' + divname).removeClass("fa-minus-circle");
        $('#iplus' + divname).addClass("fa-plus-circle");
    }
};
