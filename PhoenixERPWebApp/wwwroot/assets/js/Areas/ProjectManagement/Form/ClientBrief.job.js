var ajaxUrl = API_PM + "/ClientBrief";
var ajaxUrlCampaign = API_PM + "/Campaign";
var ajaxUrlList = ajaxUrl + "/Search";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = API_PM + "/Job/Update";
var ajaxUrlCreate = API_PM + "/Job/Create";
var ajaxUrlDelete = API_PM + "/Job/Delete";
var formUpdate = "JobUpdateForm";
var formCreate = "JobCreateForm";
var formList = "ClientBriefFormList";
var contentno = 0;
var currentjoblist = [];
fnclientbriefjob = {
    init: function () {
        if (RecordId) {
            ajaxGet(ajaxUrlGet + "/" + RecordId, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    var xdata = response.data;
                    var optionsFormCr = GetOptionsForm(function () {
                        return $("#" + formCreate).valid();
                    }, function (response, statusText, xhr, $form) {
                        response = parseJson(response);
                        if (response.success) {
                            SuccessNotif(t_createsuccess);
                        } else {
                            DangerNotif(response.message);
                        }
                    });
                    InitForm(formCreate, optionsFormCr);
                    setFormAction(formCreate, ajaxUrlCreate);

                    var optionsFormUp = GetOptionsForm(function () {
                        return $("#" + formUpdate).valid();
                    }, function (response, statusText, xhr, $form) {
                        response = parseJson(response);
                        if (response.success) {
                            SuccessNotif(t_createsuccess);
                        } else {
                            DangerNotif(response.message);
                        }
                    });
                    InitForm(formUpdate, optionsFormUp);
                    setFormAction(formUpdate, ajaxUrlUpdate);


                    FormLoadByDataUsingName(xdata, formList);
                    $("#accountmanagerlist").empty();
                    if (xdata.account_management) {
                        $.each(xdata.account_management, function (key, item) {

                            $("#accountmanagerlist").append('<li>' + item.app_username + '</li>');
                        });
                    }
                    if (xdata.external_brand_id) {
                        $('#external_brand_id').append('<option value="' + xdata.external_brand_id + '" selected>' + xdata.brand_name + '</option>');
                    }
                    if (xdata.business_unit_id) {
                        $('#business_unit_id').append('<option value="' + xdata.business_unit_id + '" selected>' + xdata.business_unit_name + '</option>');
                    }
                    if (xdata.bussiness_type_id) {
                        $('#bussiness_type_id').append('<option value="' + xdata.bussiness_type_id + '" selected>' + xdata.bussiness_type_name + '</option>');
                    }

                    if (xdata.campaign_type_id) {
                        $('#campaign_type_id').append('<option value="' + xdata.campaign_type_id + '" selected>' + xdata.campaign_type_name + '</option>');
                    }
                    if (xdata.account_management) {
                        $.each(xdata.account_management, function (key, item) {
                            $('#account_management_id').append('<option value="' + item.user_id + '" selected>' + item.app_fullname + '</option>');
                        });
                    }
                    $("#JobList tbody").empty();
                    if (xdata.job_detil) {
                        vstr = '';
                        vstrno = 0;
                        currentjoblist = xdata.job_detil;
                        $.each(xdata.job_detil, function (key, itemjob) {
                            vstrno++;
                            vstr += '<tr>';
                            vstr += '<td><a href="javascript:void(0)" onclick="fnclientbriefjob.edit(\'' + itemjob.id + '\')"><i class="fa fa-edit"></i></a>&nbsp;<a  href="javascript:void(0)" onclick="fnclientbriefjob.delete(\'' + itemjob.id + '\')"><i class="fa fa-trash"></i></a></td>';
                            vstr += '<td><a href="' + WEB + '/projectmanagement/job/detail/' + itemjob.id+'" class="fakelink">' + itemjob.job_number + '</a></td>';
                            vstr += '<td>' + itemjob.job_name + '</td>';
                            vstr += '<td>' + (itemjob.job_description !== null ? itemjob.job_description : "&nbsp;") + '</td>';
                            vstr += '<td>' + (itemjob.deadline !== null ? itemjob.deadline : "&nbsp;") + '</td>';
                            vstr += '<td>' + (itemjob.final_delivery !== null ? itemjob.final_delivery : "&nbsp;") + '</td>';
                            vstr += '<td>' + (itemjob.job_status_name !== null ? itemjob.job_status_name : "&nbsp;") + '</td>';
                            vstr += '</tr>';

                        });
                        $("#JobList tbody").append(vstr);
                    }
                    var xlayoutdetiltmp = $("#tmpl_taskaccordeondetil").html();
                    if (xdata.content) {
                        $.each(xdata.content, function (key, item) {
                            var divstr = '<div class="newcontentclass col-lg-6">' +
                                '<div class="heading cibripanel">' +
                                '<div class="input-group"><input id="contentid' + contentno + '" value="' + item.id + '" name="content[' + contentno + '][id]" type="hidden"><input required type="text" class="contenttitle form-control form-control-sm" placeholder="Title...." value="' + item.name + '" name="content[' + contentno + '][name]" /><div class="input-group-append"><button onclick="fnclientbriefupdate.removecontent(this)" class="btn m-btn--pill m-btn--air btn-outline-danger btn-sm btn-sx" type="button"><i class="fa fa-trash"></i></button></div></div>' +
                                '<div class="upload-btn-wrapper">' +


                                '</div>' +
                                '</div>' +
                                '<div class="panel-content">' +
                                '<textarea rows="5" class="form-control form-control-sm" placeholder="write here" required name="content[' + contentno + '][description]">' + item.description + '</textarea>' +
                                '<input type="text" id="contentfile' + contentno + '" value="' + item.file + '" name="content[' + contentno + '][file]" />' +
                                '</div>' +
                                '<br></div>';
                            $('#contentref').append(divstr);
                            $('#contentfile' + contentno).setuploadbox();
                            contentno++;
                        });
                    }






                }


            });
        }
    },
    add: function () {
        $("#JobCreateModal #client_brief_id").val(RecordId);
        $("#JobCreateForm")[0].reset();
        $("#JobCreateModal").modal('show');
    },
    edit: function (id) {
        $("#JobUpdateForm")[0].reset();
        $.each(currentjoblist, function (key, itemjob) {
            if (itemjob.id == id) {
                $("#JobUpdateModal #id").val(id);
                $("#JobUpdateModal #job_number").val(itemjob.job_number);
                $("#JobUpdateModal #job_name").val(itemjob.job_name);
                $("#JobUpdateModal #job_description").val(itemjob.job_description);
                $("#JobUpdateModal #start_job").val(itemjob.start_job);
                $("#JobUpdateModal #deadline").val(itemjob.deadline);
                $("#JobUpdateModal #final_delivery").val(itemjob.final_delivery);
            }
        });
        
        $("#JobUpdateModal").modal('show');
    },
    delete: function (id) {
        var text = t_delete;
        swal({
            title: "Confirmation",
            text: t_delete,
            type: "warning",
            showCancelButton: !0,
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnclientbriefjob.init();
                        hideAllModal();
                    } else {
                        DangerNotif(response.message);
                    }
                });
            }


        });

    }
};
$(function () {
    $('.dtpicker').setdatetimepicker();
    fnclientbriefjob.init();
});
