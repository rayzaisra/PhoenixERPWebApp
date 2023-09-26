var ajaxUrl = API_PM + "/ClientBrief";
var ajaxUrlCampaign = API_PM + "/Campaign";
var ajaxUrlList = ajaxUrl + "/Search";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlCreateCampaign = ajaxUrlCampaign + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var brandURL = API_PM + "/CompanyBrand/list";
var ajaxUrlListCampaign = ajaxUrlCampaign + "/list";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/list";
var ajaxUrlListOrg = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListClientBriefContentRef = API_PM + "/ClientBriefContentRef/list";
var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
var ajaxUrlJobTypeList = API_PM + "/JobType/List";
var ajaxUrlListBusinesstype = API_PM + "/BussinessType/list";
var ajaxUrlListcampaigntype = API_PM + "/CampaignType/list";
var ajaxUrlJobList = API_PM + "/Job/ListClientBrief";
var ajaxUrlUpdateJob = API_PM + "/Job/Update";
var ajaxUrlCreateJob = API_PM + "/Job/Create";
var ajaxUrlDeleteJob = API_PM + "/Job/Delete";
var ajaxUrlTeamList = API_GENERAL + "/team/list";
var companyUrl = API_PM + "/CompanyBrand/companylist";
var ajaxUrlListLegal = API + "/ProjectManagement/CompanyBrand/LegalList";
var ajaxUrlListDepartment = API + "/ProjectManagement/CompanyBrand/DepartementList/{divisionId}";
var ajaxUrlListMainService = API + "/ProjectManagement/Job/ListMainService";
var ajaxUrlBrandList = API + "/ProjectManagement/Job/BrandListByCompanyId/{id}";
var ajaxUrlCreateOneDrive = API + "/odrive/createfolder";
var formUpdate = "ClientBriefFormUpdate";
var formJob = "ClientBriefFormList";
var currentjoblist = new Array();
var contentno = 0;
var columnsIndex = [

    {
        "data": null,
        "title": "#",
        "sClass": "rcol",
        orderable: true,
        render: function (data, type, row, meta) {
            var itemjob = row;
            var me = getUrlVars()["readonly"];
            if (me !== undefined) {
                retval = "";
            } else {
                currentjoblist.push(itemjob);
                retval = '<div class="btn-group"><a class="m-btn btn btn-sm btn-primary" href="javascript:void(0)" onclick="fnclientbriefjob.edit(\'' + itemjob.id + '\')"><i class="la la-edit"></i></a>&nbsp;<a  class="m-btn btn btn-sm btn-danger"href="javascript:void(0)" onclick="fnclientbriefjob.delete(\'' + itemjob.id + '\')"><i class="la la-trash"></i></a></div>';
            }

            return retval;
        }
    },
    {
        "data": "job_number",
        "title": "Code",
        "sClass": "lcol",
        orderable: true,
        render: function (data, type, row, meta) {
            var me = getUrlVars()["readonly"];
            if (me !== undefined) {
                retval = '';
            } else {
                retval = '<a href="' + WEB + '/projectmanagement/job/detail/' + data + '" class="fakelink">' + data + '</a></td>';
            }

            return retval;
        }
    },
    {
        "data": "job_name",
        "title": "Name",
        "sClass": "lcol",
        orderable: true
    },
    {
        "data": "deadline",
        "title": "Deadline",
        "sClass": "rcol",
        orderable: false,
        "render": function (data) {
            var date = new Date(data);
            return moment(date).format("DD-MM-YYYY");
        }
    },
    {
        "data": "final_delivery",
        "title": "Final Delivery",
        "sClass": "rcol",
        orderable: false,
        "render": function (data) {
            var date = new Date(data);
            return moment(date).format("DD-MM-YYYY");
        }
    },
    {
        "data": "status_name",
        "title": "Status",
        "sClass": "rcol",
        orderable: false
    }
];

fnclientbriefjob = {
    reloadlist: function () {
        TABLES['JobList'].ajax.reload(null, false);
    },
    generatelist: function () {
        currentjoblist = [];
        InitTable('JobList', ajaxUrlJobList, { 'Search_client_brief_id': RecordId }, columnsIndex, 'job_number', 'desc');
    },
    setformcreate: function () {
        var frmopt = "JobCreateForm";
        var optionsFormCr = GetOptionsForm(function () {
            var xret = $("#" + frmopt).valid();
            if (xret) {
                startprocess();
                $("#JobCreateModal").modal('hide');
            } 
            return xret;
        }, function (response, statusText, xhr, $form) {
                
                response = parseJson(response);
                if (response.success) {
                    endprocess();
                    swal({
                        title: "Confirmation",
                        text: "Do you want to create this job?",
                        type: "success",
                        showCancelButton: !0,
                        confirmButtonText: "Yes",
                        cancelButtonText: "No",
                        reverseButtons: !0,
                        closeOnConfirm: false,
                        closeOnCancel: false
                    }).then(function (e) {
                        if (e.value) {
                            //$("#JobCreateModal").modal('hide');
                            fnclientbriefjob.reloadlist();
                            SuccessNotif(t_createsuccess);
                        } else {
                            //$("#JobCreateModal").modal('hide');
                            ajaxDelete(ajaxUrlDeleteJob + "/" + response.data.job_number, {}, function (response) {
                                response = parseJson(response);
                                if (!response.success) {
                                    DangerNotif(response.message);
                                } 
                            });
                        }
                    });

                } else {
                    endprocess();
                    DangerNotif(response.message);
                }
        });
        InitForm(frmopt, optionsFormCr);
        setFormAction(frmopt, ajaxUrlCreateJob);
    },

    setformupdate: function () {
        var frmopt = "JobUpdateForm";
        var optionsFormCr = GetOptionsForm(function () {
            var xret = $("#" + frmopt).valid();
            if (xret) {
                $("#JobUpdateModal").modal('hide');
                startprocess();
            } 
            return xret;

        }, function (response, statusText, xhr, $form) {
            
            response = parseJson(response);
            if (response.success) {
                endprocess();

                SuccessNotif(t_createsuccess);
                fnclientbriefjob.reloadlist();
            } else {
                endprocess();
                DangerNotif(response.message);
            }
        });
        InitForm(frmopt, optionsFormCr);
        setFormAction(frmopt, ajaxUrlUpdateJob);
    },
    add: function () {
        $("#JobCreateModal #client_brief_id").val(RecordId);
        $("#JobCreateModal #business_unit_division_id").val($('#business_unit_id').val());
        $("#JobCreateForm")[0].reset();
        $("#JobCreateModal #start_job").val($("#" + formUpdate + " #start_campaign").val());
        $("#JobCreateModal #deadline").val($("#" + formUpdate + " #deadline").val());
        $("#JobUpdateModal #final_delivery").val($("#" + formUpdate + " #final_delivery").val());
        $("#JobCreateModal #brand_id");


        $("#JobCreateModal").modal('show');

    },
    edit: function (id) {

        $('#jjob_type').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlJobTypeList,
            searchparam: 'search',
            labelField: 'name',
            valueField: 'id'
        });

        $('#tteam_id').setcombobox({
            data: { 'rows': 100},
            url: ajaxUrlTeamList,
            searchparam: 'search',
            labelField: 'team_name',
            valueField: 'id'
        });


        $('#llegal_entity_id').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlListLegal,
            searchparam: 'search',
            labelField: 'name',
            valueField: 'id'
        });

        $('#bbusiness_unit_departement_id').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: 700 },
            url: ajaxUrlListOrg,
            searchparam: 'search',
            labelField: 'unit_name',
            valueField: 'id'
        });

        $('#mmain_service_category').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlListMainService,
            searchparam: 'search',
            labelField: 'name',
            valueField: 'id'
        });

        $('#bbrand_id').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlBrandList,
            searchparam: 'search',
            labelField: 'brand_name',
            valueField: 'id'
        });

        $("#JobUpdateForm")[0].reset();

        $.each(currentjoblist, function (key, itemjob) {
            if (itemjob.id === id) {
                $("#JobUpdateModal #id").val(id);
                $("#JobCreateModal #business_unit_division_id").val($('#business_unit_id').val());
                $("#JobUpdateModal #job_number").val(itemjob.job_number);
                $("#JobUpdateModal #job_name").val(itemjob.job_name);
                if (itemjob.job_type) {

                    $('#jjob_type').append('<option value="' + itemjob.job_type + '" selected>' + itemjob.job_type_name + '</option>');
                }
                if (itemjob.team_id) {

                    $('#tteam_id').append('<option value="' + itemjob.team_id + '" selected>' + itemjob.team_name + '</option>');
                }
                if (itemjob.legal_entity_id) {
                    //
                    $('#llegal_entity_id').append('<option value="' + itemjob.legal_entity_id + '" selected>' + itemjob.legal_entity_name + '</option>');
                }
                if (itemjob.business_unit_departement_id) {

                    $('#bbusiness_unit_departement_id').append('<option value="' + itemjob.business_unit_departement_id + '" selected>' + itemjob.business_unit_departement_name + '</option>');
                }
                if (itemjob.main_service_category) {

                    $('#mmain_service_category').append('<option value="' + itemjob.main_service_category + '" selected>' + itemjob.main_service_category_name + '</option>');
                }
                
                if (itemjob.brand_id) {

                    $('#bbrand_id').append('<option value="' + itemjob.brand_id + '" selected>' + itemjob.brand_name + '</option>');
                }

                $("#JobUpdateModal #job_description").val(itemjob.job_description);
                var date = new Date(itemjob.start_job);
                $("#JobUpdateModal #start_job").val(moment(date).format("YYYY-MM-DD"));
                date = new Date(itemjob.deadline);
                $("#JobUpdateModal #deadline").val(moment(date).format("YYYY-MM-DD"));
                date = new Date(itemjob.final_delivery);
                $("#JobUpdateModal #final_delivery").val(moment(date).format("YYYY-MM-DD"));

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
                ajaxDelete(ajaxUrlDeleteJob + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        if (response.message === 'unassigned') {
                            swal.fire({
                                title: "Data Tidak Bisa Di Hapus!",
                                text: "",
                                type: "success",
                                buttonsStyling: false,
                                confirmButtonText: "Confirm me!",
                                confirmButtonClass: "btn btn-brand"
                            });
                        }
                        else {
                            SuccessNotif(t_deletesuccess);
                            fnclientbriefjob.reloadlist();
                        }
                      

                    } else {
                        DangerNotif(response.message);
                    }
                });
            }


        });

    }
};

fnclientbriefupdate = {

    removecontent: function (x) {
        stexist = $("div.newcontentclass").length;
        if (stexist > 1) {
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
                    $(x).parents('div.newcontentclass').remove();
                }


            });

        } else {
            DangerNotif("minimum 1 Content");
        }
    }, addcontent: function (name) {
        var cancreate = true;
        $("input[name='name[]']").each(function () {
            var value = $(this).val();
            if (value) {
                if (value.trim() === "") {
                    cancreate = false;
                    return false;
                }
            } else {
                cancreate = false;
                return false;
            }
        });
        if (cancreate) {

            var divstr = '<div class="newcontentclass col-lg-6">' +
                '<div class="heading cibripanel">' +
                '<div class="input-group"><input required type="text" class="contenttitle form-control form-control-sm" placeholder="Title...." value="' + name + '" name="content[' + contentno + '][name]" /><div class="input-group-append"><button onclick="fnclientbriefupdate.removecontent(this)" class="btn m-btn--pill m-btn--air btn-outline-danger btn-sm btn-sx" type="button"><i class="fa fa-trash"></i></button></div></div>' +
                '<div class="upload-btn-wrapper">' +


                '</div>' +
                '</div>' +
                '<div class="panel-content">' +
                '<textarea rows="5" class="form-control form-control-sm" placeholder="write here" required name="content[' + contentno + '][description]"></textarea>' +
                '<input type="text" value="" id="contentfile' + contentno + '" name="content[' + contentno + '][file]" />' +
                '</div>' +
                '<br></div>';
            $('#contentref').append(divstr);
            $('#contentfile' + contentno).setuploadbox();
            contentno++;
        } else {
            DangerNotif("Please write the Content Name");
        }
    }
};

$(function () {
    var last30Days = new Date();
    last30Days.setDate(last30Days.getDate() - 30);
    $('#JobCreateForm #btn-savejob').prop("disabled", true);
    if (RecordId) {
      
        ajaxGet(ajaxUrlGet + "/" + RecordId, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                var xdata = response.data;
                var optionsForm = GetOptionsForm(function () {
                    var xret = $("#" + formUpdate).valid();
                    if (xret) startprocess();
                    return xret;

                }, function (response, statusText, xhr, $form) {
                    endprocess();
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_createsuccess);
                    } else {
                        DangerNotif(response.message);
                    }
                });

                InitForm(formUpdate, optionsForm);
                setFormAction(formUpdate, ajaxUrlUpdate);
                FormLoadByDataUsingName(xdata, formUpdate);
                fnclientbriefjob.generatelist();

                //if (xdata.external_brand_id) {
                //    $('#external_brand_id').append('<option value="' + xdata.external_brand_id + '" selected>' + xdata.brand_name + '</option>');
                //}
                $('#campaign_name2').html(xdata.campaign_name);

                if (xdata.business_unit_id) {
                    ajaxUrlListDepartment = ajaxUrlListDepartment.replace("{divisionId}", xdata.business_unit_id.toString());
                  
                    $('#business_unit_id').append('<option value="' + xdata.business_unit_id + '" selected>' + xdata.business_unit_name + '</option>');
                }
                if (xdata.bussiness_type_id) {
                    $('#bussiness_type_id').append('<option value="' + xdata.bussiness_type_id + '" selected>' + xdata.bussiness_type_name + '</option>');
                }

                if (xdata.client_id) {
                    ajaxUrlBrandList = ajaxUrlBrandList.replace("{id}", xdata.client_id);
                    console.log(ajaxUrlBrandList);
                    $('#client_id').append('<option value="' + xdata.client_id + '" selected>' + xdata.company_name + '</option>');

                }

                $("#campaign_id").html(xdata.id);

                $("#id").html(xdata.id);
                if (xdata.campaign_type_id) {
                    $('#campaign_type_id').append('<option value="' + xdata.campaign_type_id + '" selected>' + xdata.campaign_type_name + '</option>');
                }

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

               

                //$('.datepicker').setdatepicker();

                $('.datepicker').datepicker({
                    autoclose: true,
                    todayHighlight: true,
                    startDate: last30Days,
                    format:"yyyy-mm-dd"
                });

                //$('#external_brand_id').setcombobox({
                //    data: { 'rows': 100 },
                //    url: brandURL,
                //    searchparam: 'search',
                //    labelField: 'brand_name',
                //    valueField: 'id',
                //    render: function (data) {

                //        if (data.odata) {
                //            return '<div class="option">' +
                //                '<span class="title">' +
                //                '<span class="name">' + data.odata.brand_name + '</span>' +
                //                '</span>' +
                //                '<span class="description">' + data.odata.company_name + '</span>' +
                //                '</div>';
                //        }

                //    },
                //});

                $('#team_id').setcombobox({
                    data: { 'rows': 100},
                    url: ajaxUrlTeamList,
                    searchparam: 'search',
                    labelField: 'team_name',
                    valueField: 'id'
                });


                $('#job_type').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlJobTypeList,
                    searchparam: 'search',
                    labelField: 'name',
                    valueField: 'id'
                });

                $('#legal_entity_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListLegal,
                    searchparam: 'search',
                    labelField: 'name',
                    valueField: 'id',
                });

                $('#business_unit_departement_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 700 },
                    url: ajaxUrlListOrg,
                    searchparam: 'search',
                    labelField: 'unit_name',
                    valueField: 'id'
                });

                //$('#business_unit_departement_id').setcombobox({
                //    data: { 'rows': 100, Search_business_unit_level: 800 },
                //    url: ajaxUrlListOrg,
                //    searchparam: 'search',
                //    labelField: 'name',
                //    valueField: 'id'
                //});

                $('#main_service_category').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListMainService,
                    searchparam: 'search',
                    labelField: 'name',
                    valueField: 'id'
                });

                //$('#brand_id').setcombobox({
                //    data: { 'rows': 100 },
                //    url: ajaxUrlBrandList,
                //    searchparam: 'search',
                //    labelField: 'brand_name',
                //    valueField: 'id'
                //});

                $('#brand_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlBrandList,
                    searchparam: 'search',
                    labelField: 'brand_name',
                    valueField: 'id'
                    /*render: function (data) {

                        if (data.odata) {
                            return '<div class="option">' +
                                '<span class="title">' +
                                '<span class="name">' + data.odata.brand_name + '</span>' +
                                '</span>' +
                                '<span class="description">' + data.odata.company_name + '</span>' +
                                '</div>';
                        }

                    },*/
                });

                //$('#brand_id').setcombobox({
                //    data: { 'rows': 100 },
                //    url: ajaxUrlBrandList,
                //    searchparam: 'search',
                //    labelField: 'brand_name',
                //    valueField: 'id'
                //});

                $('#client_id').setcombobox({
                    data: { 'rows': 100 },
                    url: companyUrl,
                    searchparam: 'search',
                    labelField: 'company_name',
                    valueField: 'id',
                    render: function (data) {

                        if (data.odata) {
                            return '<div class="option">' +
                                '<span class="title">' +
                                '<span class="name">' + data.odata.company_name + '</span>' +
                                '</span>' +
                                '<span class="description">' + data.odata.company_name + '</span>' +
                                '</div>';
                        }

                    },
                });

                //$('#business_unit_id').setcombobox({
                //    data: { 'rows': 100, 'Search_business_unit_type_id': 'subgroup' },
                //    url: ajaxUrlListOrganization,
                //    searchparam: 'search',
                //    labelField: 'unit_name',
                //    valueField: 'id'
                //});


                $('#business_unit_id').setcombobox({
                    data: { 'rows': 100, Seach_is_finance: true },
                    url: ajaxUrlListOrg,
                    searchparam: 'search',
                    labelField: 'unit_name',
                    valueField: 'id'
                });

                $('#bussiness_type_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListBusinesstype,
                    searchparam: 'search_propose_type',
                    labelField: 'propose_type',
                    valueField: 'id',
                });
                $('#campaign_type_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListcampaigntype,
                    searchparam: 'search_campaign_type',
                    labelField: 'campaign_type',
                    valueField: 'id'
                });
                fnclientbriefjob.setformcreate();
                fnclientbriefjob.setformupdate();

                var me = getUrlVars()["readonly"];
                if (me !== undefined) {
                   

                    $('#ClientBriefFormUpdate input').attr('readonly', 'readonly');
                    $('#ClientBriefFormUpdate input[type="text"], textarea').attr('readonly', 'readonly');
                    $('#ClientBriefFormUpdate button').attr("disabled", true);
                    //$('#ClientBriefFormUpdate a').removeAttr('href');
                    $('#ClientBriefFormUpdate .input-group-append').empty();
                    $("#ClientBriefFormUpdate select").select2({ disabled: 'readonly' });

                    $('#JobUpdateForm input').attr('readonly', 'readonly');
                    $('#JobUpdateForm input[type="text"], textarea').attr('readonly', 'readonly');
                    $('#JobUpdateForm button').attr("disabled", true);
                    $("#JobUpdateForm select").select2({ disabled: 'readonly' });

                    $('#ClientBriefJob button').attr("disabled", true);
                    //$('#ClientBriefJob a').removeAttr('href');

                    // $('#JobList >tbody.a').removeAttr('onclick').click(function () { return false; });


                }
      

            } else {
                ///--------------if not found
               
            }
            
           
        });
    } 

    

 
});

$('#customFile').on('change', function () {
    //get the file name
    var fileName = $(this).val();
    console.log(fileName);
    //replace the "Choose a file" label
    $(this).next('.custom-file-label').html(fileName);
});

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function startcampaignvalidation() {
    var startCampaign = $('#start_campaign').val();
    var deadline = $('#deadline').val();
    var finalDelivery = $('#final_delivery').val();

    if (startCampaign !== "") {
        $('#deadline').prop("disabled", false);
        if (startCampaign !== "" && deadline !== "" && finalDelivery !== "" ) {
            if (deadline > startCampaign && finalDelivery >= deadline) {
                $('#deadlinelabel').hide();
                $('#deadlinelabel').html("");
                $('#finaldeliverylabel').hide();
                $('#finaldeliverylabel').html("");
                $('#deadline').prop("disabled", false);
                $('#final_delivery').prop("disabled", false);
                $('#btn-save').prop("disabled", false);
            } else {
                if (startCampaign >= deadline) {
                    $('#deadline').prop("disabled", true);
                    $('#final_delivery').prop("disabled", true);
                    $('#deadlinelabel').show();
                    $('#deadlinelabel').css("color", "red");
                    $('#deadlinelabel').html("Start campaign date is over than Deadline date ");
                }
                $('#btn-save').prop("disabled", true);
            }
        }


    } else {
        $('#deadline').prop("disabled", true);
        //$('#deadline').val("");
        $('#final_delivery').prop("disabled", true);
        //$('#final_delivery').val("");
        $('#btn-save').prop("disabled", true);
    }
}

function deadlinevalidation() {
    var startCampaign = $('#start_campaign').val();
    var deadline = $('#deadline').val();
    var finalDelivery = $('#final_delivery').val();

    if (deadline <= startCampaign) {
        if (deadline !== "") {
            $('#deadlinelabel').show();
            $('#deadlinelabel').css("color", "red");
            $('#deadlinelabel').html("Deadline date is older than start campaign date");
            $('#final_delivery').prop("disabled", true);
        } else {
            $('#deadlinelabel').hide();
            $('#deadlinelabel').html("");
        }
       
        //$('#final_delivery').val("");
        $('#btn-save').prop("disabled", true);
    } else {
        $('#deadlinelabel').hide();
        $('#deadlinelabel').html("");
        $('#final_delivery').prop("disabled", false);
        if (startCampaign !== "" && deadline !== "" && finalDelivery !== "") {
            if (deadline > startCampaign && finalDelivery >= deadline) {
                $('#btn-save').prop("disabled", false);
            } else {
                $('#btn-save').prop("disabled", true);
            }
        }
    }

}

function finaldeliveryvalidation() {
    var startCampaign = $('#start_campaign').val();
    var deadline = $('#deadline').val();
    var finalDelivery = $('#final_delivery').val();

    if (finalDelivery < deadline) {
        if (finalDelivery !== "") {
            $('#finaldeliverylabel').show();
            $('#finaldeliverylabel').css("color", "red");
            $('#finaldeliverylabel').html("Final delivery date is older than deadline date");
        } else {
            $('#finaldeliverylabel').hide();
            $('#finaldeliverylabel').html("");
        }
        $('#btn-save').prop("disabled", true);
    } else {
        $('#finaldeliverylabel').hide();
        $('#finaldeliverylabel').html("");
        $('#btn-save').prop("disabled", false);
    }
}

function startjobvalidation(formname) {
    var startCampaign = $('#ClientBriefFormUpdate #start_campaign');
    var startJob = $('#start_job');
    var deadlineJob = $('#' + formname+' #deadline');
    var finaldeliveryJob = $('#' + formname+' #final_delivery');

    if (startJob.val() !== "") {
        
        if (startJob.val() < startCampaign.val()) {
            $('#startjoblabel').show();
            $('#startjoblabel').css("color", "red");
            $('#startjoblabel').html("Start job date is older than start campaign date");
            $('#deadlinejoblabel').hide();
            $('#deadlinejoblabel').html("");
            $('#finaldeliveryjoblabel').hide();
            $('#finaldeliveryjoblabel').html("");
            deadlineJob.prop("disabled", true);
            finaldeliveryJob.prop("disabled", true);
        } else {
            if (startJob.val() > deadlineJob.val()) {
                $('#startjoblabel').show();
                $('#startjoblabel').css("color", "red");
                $('#startjoblabel').html("Start job date is newer than deadline job date");
                $('#deadlinejoblabel').hide();
                $('#deadlinejoblabel').html("");
                $('#finaldeliveryjoblabel').hide();
                $('#finaldeliveryjoblabel').html("");
                deadlineJob.prop("disabled", true);
                finaldeliveryJob.prop("disabled", true);
            } else {
                $('#startjoblabel').hide();
                $('#startjoblabel').html("");
                $('#deadlinejoblabel').hide();
                $('#deadlinejoblabel').html("");
                $('#finaldeliveryjoblabel').hide();
                $('#finaldeliveryjoblabel').html("");
                deadlineJob.prop("disabled", false);
                finaldeliveryJob.prop("disabled", false);
            }
        }
    } else {
        $('#startjoblabel').hide();
        $('#startjoblabel').html("");
        $('#deadlinejoblabel').hide();
        $('#deadlinejoblabel').html("");
        $('#finaldeliveryjoblabel').hide();
        $('#finaldeliveryjoblabel').html("");
        deadlineJob.prop("disabled", true);
        finaldeliveryJob.prop("disabled", true);
        $('#' + formname+' #btn-savejob').prop("disabled", true);
    }
}

function deadlinejobvalidation(formname) {
    var startCampaign = $('#ClientBriefFormUpdate #start_campaign');
    var deadline = $('#ClientBriefFormUpdate #deadline');
    var finaldelivery = $('#ClientBriefFormUpdate #final_delivery');
    var startJob = $('#start_job');
    var deadlineJob = $('#' + formname + ' #deadline');
    var finaldeliveryJob = $('#' + formname + ' #final_delivery');

    if (deadlineJob.val() <= deadline.val()) {
        if (deadlineJob.val() <= startJob.val()) {
            if (deadlineJob.val() !== "") {
                $('#deadlinejoblabel').show();
                $('#deadlinejoblabel').css("color", "red");
                $('#deadlinejoblabel').html("Deadline job date is older than start job date");
                
            } else {
                $('#deadlinejoblabel').hide();
                $('#deadlinejoblabel').html("");
               
            }
            finaldeliveryJob.prop("disabled", true);
            $('#' + formname + ' #btn-savejob').prop("disabled", true);
        } else {
            $('#deadlinejoblabel').hide();
            $('#deadlinejoblabel').html("");
            finaldeliveryJob.prop("disabled", false);
            
            if (startJob.val() !== "" && deadlineJob.val() !== "" && finaldeliveryJob.val() !== "") {
                if (deadlineJob.val() > startJob.val() && finaldeliveryJob.val() >= deadlineJob.val()) {
                    $('#' + formname + ' #btn-savejob').prop("disabled", false);
                } else {
                    $('#' + formname + ' #btn-savejob').prop("disabled", true);
                }
            }
        }

    } else {
        $('#startjoblabel').hide();
        $('#startjoblabel').html("");
        $('#finaldeliveryjoblabel').hide();
        $('#finaldeliveryjoblabel').html("");
        $('#deadlinejoblabel').show();
        $('#deadlinejoblabel').css("color", "red");
        $('#deadlinejoblabel').html("Deadline job date is newer than deadline campaign date");
        finaldeliveryJob.prop("disabled", true);
        $('#' + formname + ' #btn-savejob').prop("disabled", true);
    }
}

function finaldeliveryjobvalidation(formname) {
    var startCampaign = $('#ClientBriefFormUpdate #start_campaign');
    var deadline = $('#ClientBriefFormUpdate #deadline');
    var finaldelivery = $('#ClientBriefFormUpdate #final_delivery');
    var startJob = $('#start_job');
    var deadlineJob = $('#' + formname + ' #deadline');
    var finaldeliveryJob = $('#' + formname + ' #final_delivery');

    if (finaldeliveryJob.val() <= finaldelivery.val()) {
        if (finaldeliveryJob.val() < deadlineJob.val()) {
            if (finaldeliveryJob.val() !== "") {
                $('#finaldeliveryjoblabel').show();
                $('#finaldeliveryjoblabel').css("color", "red");
                $('#finaldeliveryjoblabel').html("Final delivery job date is older than deadline job date");
            } else {
                $('#finaldeliveryjoblabel').hide();
                $('#finaldeliveryjoblabel').html("");
            }
            $('#' + formname + ' #btn-savejob').prop("disabled", true);
        } else {
            $('#finaldeliveryjoblabel').hide();
            $('#finaldeliveryjoblabel').html("");
            $('#' + formname + ' #btn-savejob').prop("disabled", false);
        }
    } else {
        $('#startjoblabel').hide();
        $('#startjoblabel').html("");
        $('#deadlinejoblabel').hide();
        $('#deadlinejoblabel').html("");
        $('#finaldeliveryjoblabel').show();
        $('#finaldeliveryjoblabel').css("color", "red");
        $('#finaldeliveryjoblabel').html("Final delivery job date is newer than final delivery campaign date");
        $('#' + formname + ' #btn-savejob').prop("disabled", true);
    }
}