'use strict';
let loadData = function (folderName) {
    let urlApi = $.helper.resolveApi("~odrive/getroot");

    if (folderName !== null || folderName !== undefined)
        urlApi = $.helper.resolveApi("~odrive/getroot/" + folderName);

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
    table.append('<tr><td colspan="5" class="text-center">there is no data</td></tr>');
};

let bindTable = function (data) {
    console.log(data);

    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        if (typeof (val.file) === 'null' || val.file === null) { // ini folder
            if (typeof (val.folder) === 'null' || val.folder === null) {
                row = row + "<td><i class='fa fa-share-alt' style='color:purple'></i> " + val.name + "</td>";
                row = row + "<td>File Sharing</td>";
            }
            else {
                row = row + "<td><i class='fa fa-folder' style='color:#ffd969'></i> <a href='javascript:lookUpFolder(\"" + val.name + "\")'>" + val.name + "</a></td>";
                row = row + "<td>Folder</td>";
            }
        }
        else {
            row = row + "<td><i class='fa fa-file' style='color:#b54faa'></i> " + val.name + "</td>";
            row = row + "<td>File</td>";
        }

        row = row + "<td>" + moment(val.lastModifiedDateTime).format('MM/DD/YYYY');+ "</td>";
        row = row + "<td>" + val.lastModifiedBy + "</td>";
        if (typeof (val.file) === 'null' || val.file === null) { // ini bisa folder bisa juga sharing
            if (typeof (val.folder) === 'undefined' || val.folder === null) {
                row = row + "<td></td>";
                row = row + "<td></td>";
            }
            else {
                row = row + "<td>" + val.folder.childCount + " item</td>";
                row = row + "<td><button class='btn btn-sm btn-small btn-default' title='open folder' onclick='lookUpFolder(\"" + val.name + "\")'><i class='fa fa-folder-open' style='color:#ffd969'></i></button></td>";
            }
        }
        else { // ini file
            let kb = 0;
            if (val.size < 1024) {
                row = row + "<td>" + val.size + " byte</td>";
                row = row + "<td><button class='btn btn-sm btn-small btn-default' title='download this file' onclick='downloadFile(\"" + val.name + "\")'><i class='fa fa-file-download' style='color:#36a3f7'></i></button></td>";
            }
            else {
                kb = parseFloat(val.size) / 1024;
                row = row + "<td>" + parseFloat(kb).toFixed(2) + " KB</td>";
                row = row + "<td><button class='btn btn-sm btn-small btn-default' title='download this file'  onclick='downloadFile(\"" + val.name + "\")'><i class='fa fa-file-download' style='color:#36a3f7'></i></button></td>";
            }
        }
        row = row + "</tr>";
        table.append(row);
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

let downloadFile = function (filename) {
    let ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    let linked = '';
    if (ds.length === 1) {
        linked = filename;
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                linked = r.name;
            else if (r.id > 1)
                linked = linked + "*" + r.name;
        });
        linked = linked + "*" + filename;
    }

    let urlApi = $.helper.resolveApi("~odrive/download/" + linked);

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
            console.log('success');
            window.open(data.path,"_blank");
            },
        error: function (e,a,b) {
            console.log('error');
            console.log(e);
            console.log(a);
            console.log(b);
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 2e3));
};

let uploadToDrive = function () {
    var files = $("#fu").get(0).files;
    if (files.length === 0) {
        return swal(
            'Information',
            'Ooops, there is no file for upload, please select file first before upload!',
            'warning'
        );
    }
    let ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    let path = '';
    let fullPath = '';

    if (ds.length === 1) {
        path = '';
        fullPath = files[0].name;
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                path = r.name;
            else if (r.id > 1)
                path = path + "/" + r.name;
        });
        fullPath = path + "/" + files[0].name;
    }
    
    var frmData = new FormData();
    //$.each($("#fu").get(0).files, function (i, file) {
    //    frmData.append("File", file);
    //});

    frmData.append("File", files[0]);
    frmData.append("Path", path);
    frmData.append("PathFullWithFileName", fullPath);
    frmData.append("IsBig", false);
    let urlApi = $.helper.resolveApi("~odrive/uploadtodrive");

    $.ajax({
        url: urlApi,
        type: "POST",
        cache: false,
        processData: false,
        contentType: false,
        data: frmData,
        success: function (responseData) {
            bindTable(responseData);
            $('#modalUpload').modal('hide');

        },
        error: function (error) { console.log(error); }
    });


};

let openFormUpload = function () {
    let ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    let linked = '';
    if (ds.length === 1) {
        $('.label-info-root').text('ROOT');
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                linked = r.name;
            else if (r.id > 1)
                linked = linked + " / " + r.name;
        });
        linked = "ROOT / " + linked;
        $('.label-info-root').text(linked);
    }
    $('#fu').val(null);
    $('.label-info-fu').text('');
    $('#modalUpload').modal('show');
};

let uploadMedia = function (files) {
    var sizeinbytes = files[0].size;
    var fSExt = new Array('Bytes', 'KB', 'MB', 'GB');
    var fSize = sizeinbytes;
    var i = 0;
    while (fSize > 900) {
        fSize /= 1024;
        i++;
    }
    var realSize = Math.round(fSize * 100) / 100;
    if (realSize > 4.0 && (fSExt[i] === 'MB' || fSExt[i] === 'GB')) {
        $('.label-info-fu').text('File Size : ' + realSize + ' ' + fSExt[i] + ', cannot upload file more than 4MB');
        $('.label-info-fu').css('color','red');
        $('.btnsave-uploadTodrive').css('display','none');
    }
    else {
        $('.label-info-fu').text('File Size : ' +realSize + ' ' + fSExt[i]);
        $('.label-info-fu').css('color', '#000');
        $('.btnsave-uploadTodrive').css('display', 'block');
    }

};

let createFolder = function () {
    let ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    let path = '';
    let linked = '';

    let folderName = $('.tb-folder-name').val();
    if (ds.length === 1) {
        path = '';
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                path = r.name;
            else if (r.id > 1)
                path = path + "/" + r.name;

            linked = r.link;
        });
    }

    if (folderName.length === 0 || folderName === '') {
        alert("Folder Name cannot empty.");
        return false;
    }

    var model = { Path: path, FolderName: folderName };

    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~odrive/createfolder"),
        data: JSON.stringify(model),
        success: function (r) {
            $('#modalCreateFolder').modal('hide');
            loadData(linked);
        },
        error: function (r) {
            console.log(r);
            swal(
                '' + r.status,
                r.statusText,
                'error'
            );
        }
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
                var dt = sessionStorage.getItem('uniquePage');
                dt = JSON.parse(dt);
                if (dt != null) {
                    if (dt.isRead) {
                        loadData("");
                        var listFolder = [];
                        listFolder.push({ id: 0, name: 'ROOT', link: '' });
                        sessionStorage.setItem('breadcrumpod', JSON.stringify(listFolder));
                    }

                    if (dt.isAdd) {
                        $('.btn-form-upload').on('click', function () {
                            openFormUpload();
                        });

                        $('.btn-form-folder').on('click', function () {
                            $('.tb-folder-name').val('');
                            $('#modalCreateFolder').modal('show');
                        });

                        $('.btnsave-uploadTodrive').on('click', function () {
                            uploadToDrive();
                        });

                        $('.btncreate-folder').on('click', function () {
                            createFolder();
                        });
                    }
                    else {
                        $('.btn-form-upload').hide();
                        $('.btn-form-folder').hide();
                        $('.btnsave-uploadTodrive').hide();
                        $('.btncreate-folder').hide();
                    }

                    $('#fu').change(function () {
                        var files = $("#fu").get(0).files;
                        uploadMedia(files);
                    });

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
