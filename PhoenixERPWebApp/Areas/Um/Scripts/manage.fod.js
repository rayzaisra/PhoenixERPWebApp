let loadData = function (folderName) {
    let urlApi = $.helper.resolveApi("~mapfod/getdriveitem");
    var key = $('.txt-find-folder').val();
    if (key.length !== 0) {
        urlApi = $.helper.resolveApi("~mapfod/finddriveitem/" + folderName);
    } else {
        if (folderName !== null || folderName !== undefined)
            urlApi = $.helper.resolveApi("~mapfod/getdriveitem/" + folderName);
    }

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
        if (typeof (val.file) === 'undefined') { // ini folder
            if (typeof (val.folder) !== 'undefined') {
                var key = $('.txt-find-folder').val();
                var row = "<tr>";
                if (key.length !== 0) {
                    row = row + "<td><i class='fa fa-folder' style='color:#ffd969'></i> " + val.name + "</td>";
                }
                else row = row + "<td><i class='fa fa-folder' style='color:#ffd969'></i> <a href='javascript:lookUpFolder(\"" + val.name + "\")'>" + val.name + "</a></td>";
                row = row + "<td style='width:225px'>";
                if (key.length !== 0) {
                    row = row + "&nbsp;<button class='btn btn-sm btn-small btn-info' title='mapping folder to group or user' onclick='mapFod(\"" + val.id + "\",\"" + val.name + "\")'><i class='fa fa-users'></i></button>&nbsp;";
                    row = row + "<button class='btn btn-sm btn-small btn-danger' title='remove all mapping group or user in this folder' onclick='removeFod(\"" + val.id + "\",\"" + val.name + "\")'><i class='fa fa-trash'></i></button>&nbsp;";
                }
                else
                {
                    if (dt.isAdd) {
                    row = row + "&nbsp;<button class='btn btn-sm btn-small btn-default' title='open folder' onclick='lookUpFolder(\"" + val.name + "\")'><i class='fa fa-folder-open' style='color:#ffd969'></i></button>&nbsp;";
                    row = row + "<button class='btn btn-sm btn-small btn-info' title='mapping folder to group or user' onclick='mapFod(\"" + val.id + "\",\"" + val.name + "\")'><i class='fa fa-users'></i></button>&nbsp;";
                    }
                    if(dt.isDelete) row = row + "<button class='btn btn-sm btn-small btn-danger' title='remove all mapping group or user in this folder' onclick='removeFod(\"" + val.id + "\",\"" + val.name + "\")'><i class='fa fa-trash'></i></button>&nbsp;";
                }
                row = row + "</td>";
                row = row + "</tr>";
                table.append(row);
            }
        }

    });
};

let lookUpFolder = function (folderName) {
    let ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    let idx = 0;
    let linked = '';
    if (ds.length === 1) {
        ds.push({ id: 1, name: folderName, link: folderName });
        linked = folderName;
    }
    else {
        $.each(ds, function (x, r) {
            idx++;
            if (r.id === 1)
                linked = r.name;
            else if (r.id > 1)
                linked = linked + "*" + r.name;
        });
        linked = linked + "*" + folderName;
        ds.push({ id: idx, name: folderName, link: linked });
    }
    sessionStorage.setItem('breadcrumpod', JSON.stringify(ds));
    buildBreadCrump('add', folderName);
    loadData(linked);
};

let buildBreadCrump = function (type, foldername) {
    let $ul = $('.ul-bredchild');
    if (type === 'add') {
        var li = '<li class="m-nav__separator ' + foldername + '">-</li>';
        li = li + '<li class="m-nav__item ' + foldername + '">';
        li = li + '<a href="javascript:linkBreadCrump(\'' + foldername + '\')" class="m-nav__link">';
        li = li + '<span class="m-nav__link-text">' + foldername+'</span>';
        li = li + '</a></li>';
        $ul.append(li);
    }
};

let linkBreadCrump = function (foldername) {
    let ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    let isbreak = false;
    let newds = [];
    $.each(ds, function (x, r) {
        if (!isbreak) {
            if (foldername === r.name) isbreak = true;
            newds.push({ id: r.id, name: r.name, link: r.link });
        } else {
            $('.' + r.name).remove();
        }
    });

    let idx = 0;
    let linked = '';
    $.each(newds, function (x, r) {
        idx++;
        if (r.id === 1)
            linked = r.name;
        else if (r.id > 1)
            linked = linked + "*" + r.name;
    });
    sessionStorage.setItem('breadcrumpod', JSON.stringify(newds));
    loadData(linked);
};

let mapFod = function (id, name) {
    sessionStorage.setItem('fodname', name);
    sessionStorage.setItem('itemid', id);
    window.location.href = "/um/mapfod/mappingdrive";
}

let removeFod = function (id, name) {
    var stat = confirm('are you sure wanna remove all mapping user and group on this folder ' + name.toUpperCase() +' ?');
    if (stat) {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        }),
            $.ajax({
                type: "GET",
            dataType: 'json',
            url: "~mapfod/removeall/" + id,
            success: function (data) {
                swal(
                    'Information',
                    'Remove all mapping folder ' + name + ' successful.',
                    'success'
                );
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
    }
}

let bindMappingItem = function () {
    var code = sessionStorage.getItem('itemid'); 
    var fodname = sessionStorage.getItem('fodname');
    $('.h-title').text('Folder '+ fodname);
    let urlApi = $.helper.resolveApi("~mapfod/getbyitemid/" + code);
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
                    $('#dt_mapping > tbody').empty();
                    let table = $('#dt_mapping > tbody:last-child');
                    table.append('<tr><td colspan="3" class="text-center">there is no data</td></tr>');
                }
                else {
                    bindTableMapping(data);
                    if ($.fn.dataTable.isDataTable('#dt_mapping') === false) {
                        $('#dt_mapping').dataTable({ pageLength: 10 });
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

let bindTableMapping = function (data) {
    $('#dt_mapping > tbody').empty();
    let tbl = $('#dt_mapping > tbody');

    $.each(data, function (x, r) {
        var row = "<tr>";
        row = row + "<td>" + r.refName + "</td>";
        if (r.refType === 1) row = row + "<td><i class='fa fa-users'></i> " + r.refDesc + "</td>";
        else row = row + "<td><i class='fa fa-user'></i> " + r.refDesc + "</td>";
        row = row + "<td>";
        row = row + "<button class='btn btn-sm btn-small btn-danger' title='remove from mapping this folder' onclick='removeMember(\"" + r.id + "\")'><i class='fa fa-trash'></i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        tbl.append(row);        
    });
};

let removeMember = function (id) {
    var stat = confirm('Are you sure wanna remove this member from mapping folder?');
    if (stat) {
        let urlApi = $.helper.resolveApi("~mapfod/removebymappingid/" + id);
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
                window.location.href = "/um/mapfod/mappingdrive";
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
    }
};

let openPopupListUser = function () {
    var code = sessionStorage.getItem('itemid');
    let urlApi = $.helper.resolveApi("~mapfod/getuserlist/" + code);
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
                $('#dt_listUser>tbody').empty();
                $('#dt_listUser').DataTable().clear().destroy();
                bindListUserGroup(data, 2);
                $('#modalListUser').modal('show');
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
let openPopupListGroup = function () {
    var code = sessionStorage.getItem('itemid'); 
    let urlApi = $.helper.resolveApi("~mapfod/getgrouplist/" + code);
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
                $('#dt_listGroup').DataTable().clear().destroy();
                $('#dt_listGroup>tbody').empty();
                bindListUserGroup(data, 1);
                $('#modalListGroup').modal('show');
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

let bindListUserGroup = function (data, tipe) {
    var tbl = $('#dt_listGroup>tbody');
    if (tipe === 2) {
        tbl = null;
        tbl = $('#dt_listUser>tbody');
    }

    $.each(data, function (x, r) {
        var row = "<tr>";

        if (tipe === 1) row = row + "<td>" + r.groupName;
        else row = row + "<td>" + r.aliasName + "(" + r.email + ")";

        row = row + "</td>";
        row = row + "<td>";
        row = row + "<button class='btn btn-sm btn-small btn-info' title='add to mapping' onclick='addMember(\"" + r.id + "\",\"" + tipe + "\")'><i class='fa fa-plus'> Add</i></button>";
        row = row + "</td>";
        row = row + "</tr>";
        tbl.append(row);
    });

    if (tipe === 1) {
        if ($.fn.dataTable.isDataTable('#dt_listGroup') === false) {
            $('#dt_listGroup').dataTable({ pageLength: 10 });
        } 
    }
    else {
        if ($.fn.dataTable.isDataTable('#dt_listUser') === false) {
            $('#dt_listUser').dataTable({ pageLength: 10 });
        } 
    }
};

let addMember = function (refId, tipe) {
    if (tipe === "1") $('#modalListGroup').modal('hide');
    else  $('#modalListUser').modal('hide');

    var itemCode = sessionStorage.getItem('itemid');
    var model = { RefType: parseInt(tipe), ItemId: itemCode, RefId: refId };
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~/mapfod/AddMember"),
        data: JSON.stringify(model),
        success: function (r) {
            window.location.href = "/um/mapfod/mappingdrive";
        },
        error: function (r) {
            swal(
                '' + r.status,
                r.statusText,
                'error'
            );
        }
    });
};