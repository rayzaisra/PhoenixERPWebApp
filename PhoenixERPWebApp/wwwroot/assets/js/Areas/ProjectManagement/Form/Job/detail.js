var ajaxUrlGet = API + "ProjectManagement/Job/get";
var ajaxUrlGetCentral = API + "General/CentralResource/List";
var ajaxUrlGetComment = API + "ProjectManagement/JobComment/GetComments";
var ajaxUrlPostComment = API + "ProjectManagement/JobComment/CreateComments";
var ajaxUrlGetFolder = API + "ProjectManagement/Job/GetFolderPath";
var ajaxUrlGetListFile = API + "ProjectManagement/Job/GetListFile";
var ajaxUrlGetFile = API + "General/File/GetFilePe";
var xform = "JobDetailForm";   
var folderPath = "";

fnJob = {
    setupjobform: function (data) {
        var optionsForm = GetOptionsForm(function () {
            var xret = $("#" + xform).valid();
            if (xret) startprocess();
            return xret;
        }, function (response, statusText, xhr, $form) {
            endprocess();
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_updatesuccess);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xform, optionsForm);
        //$("#clientbrieflink").attr("href", WEB +"/ProjectManagement/clientbrief/job/"+data.client_brief_id)
        $("#clientbrieflink").attr("href", "/ProjectManagement/clientbrief/update/" + data.client_brief_id);
        //$("#centralresourcelist").empty();
        //if (data.centralresource) {
        //    $.each(data.centralresource, function (key, value) {
        //        $("#centralresourcelist").append('<li>' + value.name + '</li>');
        //    });
        //}

        $("#damlink").attr("href", "/ProjectManagement/DigitalAssetManagement/Detail/" + data.job_number);
        $("#affiliation").empty();
        if (data.affliation_name) {
            $("#affiliation").text(data.affliation_name);
        }


        $("#bussinessunit").empty();
        if (data.business_unit_name) {
            $("#bussinessunit").text(data.business_unit_name);
        }
        //division

        $("#departement").empty();
        if (data.departement_name) {
            $("#departement").text(data.departement_name);
        }

        $("#legalentity").empty();
        if (data.legalEntityName) {
            $("#legalentity").text(data.legalEntityName);
        }

       
        if (data.pca) {
            var pca = data.pca;
            vstr = '';

            var url = "'" + WEB + "/pm/Pca/AddEdit?Id=" + pca.id + "'";
            //vstr = '<a href="' + WEB + '/projectmanagement/pca/manage/' + pca.id + '" class="fakelink">' + pca.code + ' (' + thousandsep(pca.total) + ')</a>'
            //vstr = '<a href="' + WEB + '/pm/Pca/AddEdit?Id=' + pca.id + '" class="fakelink">' + pca.code + ' (' + thousandsep(pca.total) + ')</a>'
            vstr = '<a href="JavaScript:void(0);" class="fakelink" onclick = "directLink(' + url + ')">' + pca.code + ' (' + thousandsep(pca.total) + ')</a>'
            $("#pcaview").html(vstr)
        }
        else {
            var url = "'" + WEB + "/pm/pca/addedit" + "'";
            vstr = '<a href="JavaScript:void(0);" class="fakelink" onclick = "directLink(' + url + ')"> Create PCA </a>'
            $("#pcaview").html(vstr)
        }


        if (data.pce) {
            var pce = data.pce;
            vstr = '';
            var url = "'" + WEB + "/pm/Pce/AddEdit?Id=" + pce.id + "'";
            //vstr = '<a href="' + WEB + '/pm/Pce/AddEdit?Id=' + pce.id + '" class="fakelink">' + pce.code + ' (' + thousandsep(pce.total) + ')</a>'
            vstr = '<a href="JavaScript:void(0);" class="fakelink" onclick = "directLink(' + url + ')">' + pce.code + ' (' + thousandsep(pce.total) + ')</a>'
            $("#pceview").html(vstr)
        }
        else {
            var url = "'"+ WEB + "/pm/pce/addedit" +"'";
            vstr = '<a href="JavaScript:void(0);" class="fakelink" onclick = "directLink(' + url + ')">Create PCE </a>'
            $("#pceview").html(vstr)
        }

        var urlx = "'" + WEB + "/finance/JobCard/Detail?jobId=" + $('#jobnumberid').val() + "'";
        $('#job_card').html('<a href="JavaScript:void(0);" class="fakelink" onclick = "directLink(' + urlx + ')">view job card</a>');

        if (data.teamMember) {
            generateuserlist(data.teamMember);
        }
        initlanguage(lang_pm);
        FormTextByData(data, xform);
    },
    centralchange: function () {
        var crid = $(".cr_id");
        $("#jobtasksrc").empty();
        $('input[type=checkbox].cr_id').each(function () {
            if (this.checked) {
                settask($(this).attr("data-name"));
            }
        });
    },
    setupjobComment: function (xid) {
        ajaxGet(ajaxUrlGetComment + "/" + xid, {}, function (resp) {
            generateCommentUser(resp.rows);
        });

        $('#input-comment').click(function (e) {
            e.preventDefault();
            addComment();
            /* Obat buat click link ketika add comment, ajaxnya freeze  */
            $('#area-comment').load($(this).attr('href'), function () {
                $('#input-comment').click(function (e) {
                    e.preventDefault();
                });
            });

        });
    }
    
};
function generateuserlist(data) {
    $("#TeamusersList tbody").empty();

    vstr = '';
    $.each(data, function (key, row) {
        vstr += '<tr>';

        vstr += '<td>' + row.username + '</td>';
        vstr += '<td>' + row.email + '</td>';
        vstr += '<td>' + row.act_as_name + '</td>';

        vstr += '</tr>';
    });
    $("#TeamusersList tbody").append(vstr);
}

function generateCommentUser(data) {
    var container = $('.comment-container');
    container.empty();

    var vstring = '';
    $.each(data, function (index, value) {
        var classTime = 'time-right';
        var classText = '';
        var classMessage = '';
        if ((index + 1) % 2 === 1) { classTime = 'time-left'; classText = 'text-right'; classMessage = 'text-right'; }
        if (index === 3) { container.css('overflow-y', 'scroll'); container.css('border-radius', '10px'); container.css('height', '235px'); }
        vstring += "<div class = 'container'>";
        vstring += "<span class='" + classTime + "'>";
        vstring += value.hour_comment_date;
        vstring += "</span>";
        vstring += "<p class='m-messenger__message-username username " + classText + "'>";
        vstring += value.user_name;
        vstring += "</p>";
        vstring += "<p class='" + classMessage + "'>";
        vstring += value.comment_message;
        vstring += "</p>";
        vstring += "</span>";
        vstring += "</div>";
    });
    container.append(vstring);
}

function directLink(url) {
    window.open(url, '_blank');
}

function addComment() {
    var model = JSON.parse(sessionStorage.getItem("data"));
    var xid = $("#" + xform + ' [name="id"]').val();
    ajaxPost(ajaxUrlPostComment, { job_number: xid, comment_message: $('#txtAreaComment').val(), user_id: model.EmailUser, user_name: model.FullName },
        function (response) {
            if (response.success) {
                generateCommentUser(response.rows);
            }
        });
}

$(function () {
    var xid = $("#" + xform + ' [name="id"]').val();
    
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#jobdetailinit").hide();
                $("#jobdetailcontent").show();
                var dateStartJob = new Date(response.data.start_job);
                response.data.start_job = moment(dateStartJob).format("DD-MM-YYYY");

                var dateFinale = new Date(response.data.final_delivery);
                response.data.final_delivery = moment(dateFinale).format("DD-MM-YYYY");
               
                fnJob.setupjobform(response.data);
                fnJob.setupjobComment(xid);
            } else {
                show404();
            }
        });
    }

    ajaxGet(ajaxUrlGetFolder + "/" + $('#jobnumberid').val(), {}, function (response) {
        response = parseJson(response);
        if (response.success) {
            folderPath = response.data.folder_path;
        } else {
            DangerNotif(response.message);
        }
    });

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
                    var fileName = response.rows[i].file_name.split('.').shift();
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
});

$('#input-upload').on('click', function () {
    showModalAjaxGet("File Upload", WEB_GENERAL + '/fileupload', {}, function () {
        $('#uploadid').val($('#jobnumberid').val());
        $('#folderpath').val(folderPath);
        $('#folderpaths').val(folderPath);
    });
});

