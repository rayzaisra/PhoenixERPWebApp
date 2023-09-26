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
            var ajaxUrlListClientBriefContentRef = API_PM + "/ClientBriefContentRef/list";
            var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
             var ajaxUrlListBusinesstype = API_PM + "/BussinessType/list";
            var ajaxUrlListcampaigntype = API_PM + "/CampaignType/list";
            var formCreate = "ClientBriefFormCreate";
            var contentno = 0;
            fnclientbriefcreate = {
                addjob: function (x) {
                    //dval=$("#jobcontent div:last-child > input[name='job_name[]").val();
                    dval = "";
                    var newjob = '<div class="input-group newjobclass"><input  type="text" value="' + dval + '" required name="job_name[]" class="form-control form-control-sm m-input">' +
                        '<div class="input-group-append">' +
                        '<button onclick="fnclientbriefcreate.addjob(this)" class="btn btn-success btn-sm" type="button"><i class="la la-plus"></i></button>' +
                        '</div><div class="input-group-append">' +
                        '<button onclick="fnclientbriefcreate.removejob(this)" class="btn btn-danger btn-sm" type="button"><i class="la la-minus"></i></button>' +
                        '</div></div>';
                    var cancreate = true;
                    
                    cancreate = true;//--let it bee blank
                    if (cancreate) {
                        if (x) {
                            $(x).parents('div.newjobclass').after(newjob);
                        } else {
                            $('#jobcontent').append(newjob);
                        }
                        
                        //$("#jobcontent div.newjobclass:last-child > input[name='job_name[]").val("");
 
                        
                    }
                },
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
                        DangerNotif("minimum 1 Content")
                    }                    

                },
                removejob: function (x) {
                    stexist = $("div.newjobclass").length;
                    if (stexist > 1) {
                        $(x).parents('div.newjobclass').remove();
                    } else {
                        DangerNotif("minimum 1 Job");
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
                            '<div class="input-group"><input required type="text" class="contenttitle form-control form-control-sm" placeholder="Title...." value="' + name + '" name="content[' + contentno + '][name]" /><div class="input-group-append"><button onclick="fnclientbriefcreate.removecontent(this)" class="btn m-btn--pill m-btn--air btn-outline-danger btn-sm btn-sx" type="button"><i class="fa fa-trash"></i></button></div></div>' +
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
            }
$(function () {
    fnclientbriefcreate.addjob();
    $('.dtpicker').setdatetimepicker();
                var optionsForm = GetOptionsForm(function () {
                    return $("#" + formCreate).valid();
                }, function (response, statusText, xhr, $form) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_createsuccess);
 
                    } else {
						
                        DangerNotif(response.message);
                    }
                });
                InitForm(formCreate, optionsForm);
                setFormAction(formCreate, ajaxUrlCreate);
                 
                $('#external_brand_id').setcombobox({
                    data: { 'rows': 100 },
                    url: brandURL,
                    searchparam: 'search',
                    labelField: 'brand_name',
                    valueField: 'id',
                    render: function (data) {
          
                        if (data.odata) {
                            return '<div class="option">' +
                                '<span class="title">' +
                                '<span class="name">' + data.odata.brand_name + '</span>' +
                                '</span>' +
                                '<span class="description">' + data.odata.company_name + '</span>' +
                                '</div>';
                        }
                            
                    },
                });
               
                 

                $('#business_unit_id').setcombobox({
                    data: { 'rows': 100, 'Search_business_unit_type_id': 'subgroup' },
                    url: ajaxUrlListOrganization,
                    searchparam: 'search',
                    labelField: 'unit_name',
                    valueField: 'id'
                });
                $('#account_management_id').setcombobox({
                    data: { 'rows': 100},
                    url: ajaxUrlListAccountManagement,
                    maxItems:3,
                    searchparam: 'search_app_username',
                    labelField: 'app_fullname',
                    valueField: 'user_id',
                    render: function (data) {
                        if (data.odata) {
                            var stateNo = mUtil.getRandomInt(0, 7);
                            var states = [
                                'success',
                                'brand',
                                'danger',
                                'accent',
                                'warning',
                                'metal',
                                'primary',
                                'info'];
                            var state = states[stateNo];
                            return '<div class="m-card-user m-card-user--sm">' +
                                '<div class="m-card-user__pic">' +
                                '<div class="m-card-user__no-photo m--bg-fill-' + state + '"><span>' + data.odata.app_fullname.substring(0, 1) + '</span></div>' +
                                '</div>' +
                                '<div class="m-card-user__details">' +
                                '<span class="m-card-user__name">' + data.odata.app_fullname + '</span>' +
                                '<a href="javascript:return;" class="m-card-user__email m-link">' + data.odata.app_username + '</a>' +
                                '</div></div>';
                        }
                    }
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
        valueField: 'id',
    });
       
    
                ajaxGet(ajaxUrlListClientBriefContentRef, { page: 100, sortBy:'seq'}, function (res) {

                    if (res.success) {
                        $.each(res.rows, function (key, value) {
                            fnclientbriefcreate.addcontent(value.name);

                        });
                        
                    } else {
                        DangerNotif(res.Message)
                    }
    });

});
