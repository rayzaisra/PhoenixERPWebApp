var ajaxUrlGet = API + "ProjectManagement/Job/get";
var ajaxUrlGetFolder = API + "ProjectManagement/Job/GetFolderPath";

'use strict';
function loadData(folderName) {

    var linkDefault = "PROJECTMANAGEMENT*" + $('#job_name').text() + " - " + $('#job_number').text();
    var urlApi = "";
    if (folderName === '' || folderName === undefined)
        urlApi = $.helper.resolveApi("~odrive/getrootpe/" + linkDefault);
    else
        urlApi = $.helper.resolveApi("~odrive/getrootpe/" + linkDefault + "*" + folderName);

    urlApi = urlApi + "::" + $('#job_number').text();
    //var model = { WebUrl: urlApi, Id: $('#job_number').text() };
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    $.ajax({
        //type: "POST",
        //dataType: 'json',
        //contentType: 'application/json',
        //url: $.helper.resolveApi("~odrive/getrootpe"),
        //data: JSON.stringify(model),
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
            mApp.unblockPage();
        },
        error: function (e, t, s) {
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
            console.log(e);
            mApp.unblockPage();
        }
    });
};


function bindEmptyTable() {
    $('#dt_basic> tbody').empty();
    var table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="5" class="text-center">there is no data</td></tr>');
};

function bindTable(data) {
   

    $('#dt_basic> tbody').empty();
    var table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var fullpath = "PROJECTMANAGEMENT*" + $('#job_name').text() + " - " + $('#job_number').text() + "*";
        
        var type = "";
        var row = "<tr>";
        if (val.file === null) { // ini folder
            if (val.folder === null) {
                row = row + "<td><i class='fa fa-share-alt' style='color:purple'></i> " + val.name + "</td>";
                row = row + "<td>File Sharing</td>";
                type = "File Sharing";
            }
            else {
                row = row + "<td><i class='fa fa-folder' style='color:#ffd969'></i> <a href='javascript:lookUpFolder(\"" + val.name + "\")'>" + val.name + "</a></td>";
                row = row + "<td>Folder</td>";
                type = "Folder";
            }
        }
        else {
            row = row + "<td><i class='fa fa-file' style='color:#b54faa'></i> " + val.name + "</td>";
            row = row + "<td>File</td>";
            type = "File";
        }

        row = row + "<td>" + moment(val.lastModifiedDateTime).format('MM/DD/YYYY'); + "</td>";
        row = row + "<td>" + val.lastModifiedBy + "</td>";

        if (val.file === null) { // ini bisa folder bisa juga sharing
            if (val.folder === null) {
                row = row + "<td></td>";
                row = row + "<td></td>";
                row = row + "<td></td>";
            }
            else {
                row = row + "<td>" + val.folder.childCount + " item</td>";
                row = row + "<td>" + '<input type="checkbox" name="' + $('#job_number').text() + '-' + type + '-' + val.name + '" value="' + $('#job_number').text() + '::' + type + '::' + fullpath + val.name + '"/>' + "</td>";
                row = row + "<td><button class='btn btn-sm btn-small btn-default' title='open folder' onclick='lookUpFolder(\"" + val.name + "\")'><i class='fa fa-folder-open' style='color:#ffd969'></i></button>  <button class='btn btn-sm btn-small btn-default' title='delete folder' onclick='deleteFile(\"" + val.name + "\")'><i class='fa fa-trash' style='color:#ffd969'></i></button></td>";
            }
        }
        else { // ini file
            var kb = 0;
            if (val.size < 1024) {
                row = row + "<td>" + val.size + " byte</td>";
                row = row + "<td>" + '<input type="checkbox" name="' + $('#job_number').text() + '-' + type + '-' + val.name + '" value="' + $('#job_number').text() + '::' + type + '::' + fullpath + val.name + '"/>' + "</td>";
                row = row + "<td><button class='btn btn-sm btn-small btn-default' title='download this file' onclick='downloadFile(\"" + val.name + "\")'><i class='fa fa-file-download' style='color:#36a3f7'></i></button>  <button class='btn btn-sm btn-small btn-default' title='delete this file' onclick='deleteFile(\"" + val.name + "\")'><i class='fa fa-trash' style='color:#36a3f7'></i></button></td>";
            }
            else {
                kb = parseFloat(val.size) / 1024;
                row = row + "<td>" + parseFloat(kb).toFixed(2) + " KB</td>";
                row = row + "<td>" + '<input type="checkbox" name="' + $('#job_number').text() + '-' + type + '-' + val.name + '" value="' + $('#job_number').text() + '::' + type + '::' + fullpath + val.name + '"/>' + "</td>";
                row = row + "<td><button class='btn btn-sm btn-small btn-default' title='download this file'  onclick='downloadFile(\"" + val.name + "\")'><i class='fa fa-file-download' style='color:#36a3f7'></i></button>  <button class='btn btn-sm btn-small btn-default' title='delete this file' onclick='deleteFile(\"" + val.name + "\")'><i class='fa fa-trash' style='color:#36a3f7'></i></button></td>";
            }
        }
        row = row + "</tr>";
        table.append(row);
    });
};

function lookUpFolder(folderName) {
    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var idx = 0;
    var linked = '';
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

function buildBreadCrump(type, foldername) {
    var $ul = $('.ul-bredchild');
    if (type === 'add') {
        var li = '<li class="m-nav__separator ' + foldername.replace(/ /g, '') + '">-</li>';
        li = li + '<li class="m-nav__item ' + foldername.replace(/ /g, '') + '" >';
        li = li + '<a href="javascript:linkBreadCrump(\'' + foldername + '\')" class="m-nav__link">';
        li = li + '<span class="m-nav__link-text">' + foldername + '</span>';
        li = li + '</a></li>';
        $ul.append(li);
    }
};

function linkBreadCrump(foldername) {
    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var isbreak = false;
    var newds = [];

    $.each(ds, function (x, r) {
        if (!isbreak) {
            if (foldername === r.name) isbreak = true;
            newds.push({ id: r.id, name: r.name, link: r.link });
        } else {
            $('.' + r.name.replace(/ /g, '')).remove();
        }
    });

    var idx = 0;
    var linked = '';
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

function downloadFile(filename) {
    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var linked = "PROJECTMANAGEMENT*" + $('#job_name').text() + " - " + $('#job_number').text();
    if (ds.length === 1) {
        linked = linked + "*" + filename;
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                linked = linked + "*" + r.name;
            else if (r.id > 1)
                linked = linked + "*" + r.name;
        });
        linked = linked + "*" + filename;
    }

    var urlApi = $.helper.resolveApi("~odrive/downloadpe/" + linked);

    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    });
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data, statusText, xhr) {
         
            var disposition = xhr.getResponseHeader('Content-Disposition');
            var type = xhr.getResponseHeader('Content-Type');

            var blob = base64ToBlob(data.file, type, 512);
            var filename = data.fileName;

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                window.navigator.msSaveBlob(blob, filename);

            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename !== '') {
                    // use HTML5 a[download] attribute to specify filename
                    var a = document.createElement("a");
                    // safari doesn't support this yet
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;

                    } else {

                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {

                    window.location = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
            }
            mApp.unblockPage();
        },
        error: function (e, a, b) {
            console.log('error');
            console.log(e);
            console.log(a);
            console.log(b);
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
            mApp.unblockPage();
        }
    });
};

function deleteFile(filename) {
    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var linkedDefault = "PROJECTMANAGEMENT*" + $('#job_name').text() + " - " + $('#job_number').text();
    var linked = '';
    var folderName = '';
    var path = '';
    if (ds.length === 1) {
        linked = linkedDefault + "*" + filename;
        path = linked.replace("*" + filename, "");
        folderName = '';
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1) {
                linked = linkedDefault + "*" + r.name;

            }
            else if (r.id > 1) {
                linked = linkedDefault + "*" + r.name;

            }
        });
        linked = linked + "*" + filename;
        path = linked.replace("*" + filename, "");
        folderName = path.replace(linkedDefault, '');
        //alert(folderName);
    }

    var model = { FolderName: linkedDefault, Path: linked };
    swal({
        title: "Confirmation",
        text: "Do you want to delete this data?",
        type: "success",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (e) {
        if (e.value) {

            $.ajax({
                type: "POST",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~odrive/deletePe"),
                data: JSON.stringify(model),
                beforeSend: function () {
                    startprocess();
                },
                success: function (r) {
                    endprocess();
                    loadData(folderName);
                    swal('Data has been deleted');
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
        }
    });
};

function uploadToDrive() {
    var files = $("#fu").get(0).files;
    if (files.length === 0) {
        return swal(
            'Information',
            'Ooops, there is no file for upload, please select file first before upload!',
            'warning'
        );
    }

    if ($('#title_file_name').val() === "") {
        return swal(
            'Information',
            'Ooops, please fill the title of file first!',
            'warning'
        );
    }


    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var path = "PROJECTMANAGEMENT/" + $('#job_name').text() + " - " + $('#job_number').text();
    var link = '';
    var fullPath = '';
    var fileExtension = files[0].name.substring(files[0].name.lastIndexOf('.') + 1); 
    var aliasFileNameWithExtension = $('#title_file_name').val() +'.' +fileExtension;
    if (ds.length === 1) {
        fullPath = path + "/" + aliasFileNameWithExtension;

    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                path = path + "/" + r.name;
            else if (r.id > 1)
                path = path + "/" + r.name;
        });
        fullPath = path + "/" + aliasFileNameWithExtension;

    }

    var frmData = new FormData();
    //$.each($("#fu").get(0).files, function (i, file) {
    //    frmData.append("File", file);
    //});

    frmData.append("File", files[0]);
    frmData.append("Path", path);
    frmData.append("PathFullWithFileName", fullPath);
    frmData.append("IsBig", false);
    frmData.append("JobId", $('#job_number').text());
    frmData.append("FileAliasName", $('#title_file_name').val());
    var urlApi = $.helper.resolveApi("~odrive/uploadtodrivePe");
    //startprocess();
    $.ajax({
        url: urlApi,
        type: "POST",
        cache: false,
        processData: false,
        contentType: false,
        data: frmData,
        beforeSend: function () {
            startprocess();
        },
        success: function (responseData) {
            //loadData();
            bindTable(responseData);
            $('#modalUpload').modal('hide');
            endprocess();
        },
        error: function (error) { console.log(error); }
    });


};

function openFormUpload() {
    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var linked = "/PROJECTMANAGEMENT/" + $('#job_name').text() + " - " + $('#job_number').text() + "/";
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

function uploadMedia(files) {
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
        $('.label-info-fu').css('color', 'red');
        $('.btnsave-uploadTodrive').css('display', 'none');
    }
    else {
        $('.label-info-fu').text('File Size : ' + realSize + ' ' + fSExt[i]);
        $('.label-info-fu').css('color', '#000');
        $('.btnsave-uploadTodrive').css('display', 'block');
    }

};

function createFolder() {
    var ds = sessionStorage.getItem('breadcrumpod');
    ds = JSON.parse(ds);
    var path = "PROJECTMANAGEMENT/" + $('#job_name').text() + " - " + $('#job_number').text();
    var linked = '';

    var folderName = $('.tb-folder-name').val();
    if (ds.length === 1) {
        path = path;
    }
    else {
        $.each(ds, function (x, r) {
            if (r.id === 1)
                path = path + "/" + r.name;
            else if (r.id > 1)
                path = path + "/" + r.name;

            linked = r.link;
        });
    }

    if (folderName.length === 0 || folderName === '') {
        DangerNotif("Folder Name cannot empty.");
        return false;
    }

    var model = { Path: path, FolderName: folderName, Id: $('#job_number').text() };
    

    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~odrive/createfolder"),
        data: JSON.stringify(model),
        beforeSend: function () {
            startprocess();
        },
        success: function (r) {
            endprocess();
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

function base64ToBlob(base64, mimetype, slicesize) {
    if (!window.atob || !window.Uint8Array) {
        // The current browser doesn't have the atob function. Cannot continue
        return null;
    }
    mimetype = mimetype || '';
    slicesize = slicesize || 512;
    var bytechars = atob(base64);
    var bytearrays = [];
    for (var offset = 0; offset < bytechars.length; offset += slicesize) {
        var slice = bytechars.slice(offset, offset + slicesize);
        var bytenums = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            bytenums[i] = slice.charCodeAt(i);
        }
        var bytearray = new Uint8Array(bytenums);
        bytearrays[bytearrays.length] = bytearray;
    }
    return new Blob(bytearrays, { type: 'application/x-unknown' });
};


$(function() {
    var xid = $('#jobnumberid').val();
    var jobname = "";
    var jobnumber = "";
    var jobstatus = "";
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
               
                var dateStartJob = new Date(response.data.start_job);
                response.data.start_job = moment(dateStartJob).format("DD-MM-YYYY");

                var dateFinale = new Date(response.data.final_delivery);
                response.data.final_delivery = moment(dateFinale).format("DD-MM-YYYY");
                jobname = response.data.job_name;
                jobnumber = response.data.job_number;
                $('#job_name').text(response.data.job_name);
                $('#brand_name').text(response.data.brand_name);
                $('#job_number').text(response.data.job_number);
                $('#campaign_name').text(response.data.campaign_name);
                $('#departement_name').text(response.data.departement_name);
                jobstatus = response.data.job_status_id;
                if (jobstatus !== "delivered") {
                    $('.btnMoveToMyDam').hide();
                } else {
                    $('.btnMoveToMyDam').show();
                }
                $('#job_status').text(response.data.status_name);
                $("#job_number").attr("href", "/ProjectManagement/job/Detail/" + response.data.job_number);
                loadData('');
                var listFolder = [];
                listFolder.push({ id: 0, name: 'ROOT', link: '' });
                sessionStorage.setItem('breadcrumpod', JSON.stringify(listFolder));

            } else {
                show404();
            }
        });

        ajaxGet(ajaxUrlGetFolder + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                var folderPath = response.data.folder_path;
                //location.reload();
                loadData('');
            } else {
                DangerNotif(response.message);
            }
        });

    }

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
    $('#fu').change(function () {
        var files = $("#fu").get(0).files;
        uploadMedia(files);
    });
});
