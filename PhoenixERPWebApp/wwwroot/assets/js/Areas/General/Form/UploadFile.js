var ajaxUrlOneDrive = API + "General/File/UploadOneDrivePeFolder";
var onedriveform = "fileuploadonedriveform";
var ajaxUrlGetListFile = API + "ProjectManagement/Job/GetListFile";
var ajaxUrlGetFile = API + "General/File/GetFilePe";
var ajaxDeleteFile = API + "ProjectManagement/Job/DeleteFile";

$(function () {
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + onedriveform).valid();
        if (xret) {
            startprocess();
            $('#File_Upload').modal('hide');
        } 
        console.log(xret);
        return xret;
    }, function (response, statusText, xhr, $form) {

        console.log(response);
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            swal({
                title: "Confirmation",
                text: t_useasfile,
                type: "success",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (e) {
                if (e.value) {
                    ajaxGet(ajaxUrlGetListFile + "/" + $('#jobnumberid').val(), {}, function (response) {
                        response = parseJson(response);
                        if (response.success) {
                            var inHtml = "";
                            var loopHtml = "";
                            if (response.rows.length > 0) {
                                $('#table-listfilejob').empty();
                                inHtml = '<table class="table table-striped table-bordered table-hover table-checkable">' +
                                    '<tr><th>File Name</th>' +
                                    '<th>Tags</th>' +
                                    '<th>Upload Date</th>' +
                                    '<th>Action</th>' +
                                    '</tr > ';
                                for (var i = 0; i < response.rows.length; i++) {
                                    var fileName = response.rows[i].file_name;
                                    var filePath = response.rows[i].file_path;
                                    var tags = response.rows[i].tags;
                                    var id = response.rows[i].id;
                                    var uploadDate = moment(response.rows[i].upload_date).format("DD-MM-YYYY");
                                    var link = '<tr><td>' + fileName + '</td><td>' + tags + '</td><td>' + uploadDate + '</td><td>' + '<a href="javascript:;" onclick="DownloadFile(\'' + ajaxUrlGetFile + '\',\'' + id + '\');" >' + filePath + '</td></tr>';
                                    loopHtml = loopHtml + link;

                                }
                                inHtml = inHtml + loopHtml;
                                inHtml = inHtml +
                                    '</table>';
                                $('#table-listfilejob').append(inHtml);
                            }
                            else {
                                $('#table-listfilejob').empty();
                                inHtml = '<table class="table table-striped table-bordered table-hover table-checkable">' +
                                    '<tr><th>File Name</th>' +
                                    '<th>Tags</th>' +
                                    '<th>Upload Date</th>' +
                                    '<th>Action</th>' +
                                    '</tr > ';
                                inHtml = inHtml +
                                    '</table>';
                                $('#table-listfilejob').append(inHtml);
                            }
                        } else {
                            DangerNotif(response.message);
                        }
                    });
                    
                } else {
                    //alert(response.data.id);
                    ajaxGet(ajaxDeleteFile + "/" + response.data.id, {}, function (response) {
                        response = parseJson(response);
                        if (response.success) {
                            SuccessNotif("File not used");
                        }
                    });
                }
                endprocess();
            });

        } else {
            DangerNotif(response.message);
        }
    });

    InitForm(onedriveform, optionsForm);
    setFormAction(onedriveform, ajaxUrlOneDrive);
    initlanguage(lang_gn);


});

$(document).ready(function () {
    $('input[type="file"]').change(function (e) {
        console.log("a");
        $('#lblcustomFile').text(e.target.files[0].name);
    });
});
