var ajaxUrl = API + "General/File/Upload";
var ajaxUrlOneDrive = API + "General/File/UploadOneDrive";

var ajaxUrlGetFile = API + "General/File/Getfile"; 
var uploadform = "damuploadform";
var onedriveform = "damonedriveform";
 

$(function () {
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + onedriveform).valid();
        if (xret) {
            startprocess();
        } 
        $('#File_Upload').modal('hide');
        return xret;
    }, function (response, statusText, xhr, $form) {
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

                        //$("#lblx" + currentuploadbox).html('<a href="' + ajaxUrlGetFile + '/' + response.data.id+'" target="_blank">' + response.data.name + '</a>');
                        $("#lblx" + currentuploadbox).html('<a href="javascript:;" onclick="DownloadFile(\'' + ajaxUrlGetFile + '\',\'' + response.data.id + '\');" >' + response.data.name + '</a>');
                        $('#' + currentuploadbox).val(response.data.id);

                    }
                });

            } else {
                DangerNotif(response.message);
            }
            $('#File_Upload').modal('hide');
            endprocess();
        });
    
    //InitForm(uploadform, optionsForm);
    //setFormAction(uploadform, ajaxUrl);
    
    InitForm(onedriveform, optionsForm);
    setFormAction(onedriveform, ajaxUrlOneDrive);

    initlanguage(lang_gn);
});

$(document).ready(function () {
    $('input[type="file"]').change(function (e) {
        $('#lblcustomFile').text(e.target.files[0].name);
        var ext = $(this).val().split('.').pop().toLowerCase();
        
        var customFileType = $('#customFileType').val();

        if (customFileType != '') {
            var arrayCustomFileType = customFileType.split(',');

            if ($.inArray(ext, arrayCustomFileType) == -1) {
                alert('invalid extension!');
            }
        }
       
    });
});
