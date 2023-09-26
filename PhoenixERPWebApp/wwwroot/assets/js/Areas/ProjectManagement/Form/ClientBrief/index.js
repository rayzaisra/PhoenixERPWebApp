var ajaxUrl = API + "/ProjectManagement/ClientBrief";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlCreateJob = API_PM + "/Job/Create";
var ajaxUrlDeleteJob = API_PM + "/Job/Delete";
var ajaxUrlGroupAccess = "/ManageUserAccess/GetMenuAccessByUniqueName";
var tableList = "ClientBriefList";
var formCreate = "ClientBriefFormCreate";
var formUpdate = "ClientBriefFormUpdate";
var formSearch = "ClientBriefFormSearch";
var modalCreate = "ClientBriefModalCreate";
var modalUpdate = "ClientBriefModalUpdate";
var modalSearch = "ClientBriefModalSearch";
var pageheader = "";

var ajaxUrlTeamList = API_GENERAL + "/team/list";
var ajaxUrlJobTypeList = API_PM + "/JobType/List";
var ajaxUrlListLegal = API + "/ProjectManagement/CompanyBrand/LegalList";
var ajaxUrlListOrg = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListMainService = API + "/ProjectManagement/Job/ListMainService";
var ajaxUrlBrandList = API + "/ProjectManagement/Job/BrandListByCompanyId/{id}";
var companyUrl = API_PM + "/CompanyBrand/companylist";
var ajaxUrlListBusinesstype = API_PM + "/BussinessType/list";
var ajaxUrlListcampaigntype = API_PM + "/CampaignType/list";
var ajaxUrlListDepartment = API + "/ProjectManagement/CompanyBrand/DepartementList/{divisionId}";
var isRead = false;
var isAdd = false;
var isEdit = false;
var isDelete = false;

var startJobDate = '';
var deadlineDate = '';
var finaldeliveryDate = '';


var paramsSearchIndex = function () {
    //var searchdata = $("#" + formSearch).serializeArray();

    var search_id = $("#search_id").val();
    var search_val = '';
    if (search_id === 'Search_campaign_name') search_val = $("#search_val").val();
    else search_val = $('#ddl_isdeleted').val();
    var data = {};
    var user = JSON.parse(sessionStorage.getItem("data"));
    data = {
        'user_id': user.AppUserId,
        'group_id': user.GroupAccess
    };
    if (search_val) {
        data[search_id] = search_val;
    }
    /*$(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });*/

    return data;
};



var columnsIndex = [
	{
		"data": "Id",
		"title": "",
        "sClass": "actioncol3x",
		orderable: false,
        render: function (data, type, row, meta) {
 
            retval = '<div class="btn-group">';

            if (isRead) {
                retval += '';
            }
            if (isEdit) {
                retval += '<a class="m-btn btn btn-sm btn-primary fakelink" href="/projectmanagement/clientbrief/update/' + row.id + '"><i class="la la-edit"></i></a>';
                retval += '<a class="m-btn btn btn-sm btn-info fakelink" href="javascript:void(0)" onclick="fnClientBrief.jobAdd(\'' + row.id + '\')"><i class="la la-plus"></i></a>';
            }

            if (isDelete) {
                retval += '<a class="m-btn btn btn-sm btn-danger" onClick="fnClientBrief.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i></a>';
            }


            
           
            //retval += '<a class="m-btn btn btn-sm btn-danger" onClick="fnClientBrief.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i></a>';
            //retval += '<a class="m-btn btn btn-sm btn-success fakelink" href="/projectmanagement/clientbrief/job/' + row.id + '' + row.id + '"><i class="la la-tasks"></i></a>';
            retval += '</div>';
            return retval;

		}
		
	},
	{
		"data": "brand_name",
		"title": "Brand",
		"sClass": "lcol",
		orderable: true,
	},
	
	{
		"data": "campaign_name",
		"title": "Campaign Name",
		"sClass": "rcol",
        orderable: true,
        "render": function (data, type, row) {
            rdata = '';
            rdata += '<a href="' + WEB + '/projectmanagement/clientbrief/update/' + row.id + '?readonly=true" class="fakelink">' + data + '</a>';
            return rdata;
        }

	},
	{
		"data": "job_name",
		"title": "Job Name",
		"sClass": "rcol",
		orderable: false,
		"render": function (data, type, row) {
            rdata = '';
            if (data) {
                //rdata += '<ul>';
                xmn = 0;
                $.each(data, function (key, value) {
                    //rdata += '<li>' + value.app_username + '</li>';
                    if (xmn > 0) rdata += ', ';
                    rdata += value;
                    xmn++;
                });
                //rdata += '</<ul>';
            }
            return rdata;
		}
	},
    {
        "data": "job_number",
        "title": "Job Number",
        "sClass": "rcol",
        orderable: false,
        "render": function (data, type, row) {
            rdata = '';
            if (data) {
                //rdata += '<ul>';
                xmn = 0;
                $.each(data, function (key, value) {
                    //rdata += '<li>' + value.app_username + '</li>';
                    if (xmn > 0) rdata += ', ';
                    rdata += '<a href="' + WEB + '/projectmanagement/job/detail/' + value+'" class="fakelink">'+value+'</a>';
                    xmn++;
                });
                //rdata += '</<ul>';
            }
            return rdata;
        }
    },
	{
		"data": "business_unit_name",
		"title": "Business Unit",
		"sClass": "rcol",
		orderable: false
	},
	{
        "data": "start_campaign",
		"title": "Date Start",
		"sClass": "rcol",
		orderable: true,
		"render": function (data) {
			var date = new Date(data);
			var month = date.getMonth() + 1;
			return date.getDate() + "/" + month + "/" + date.getFullYear();
		}
	},
	{
		"data": "final_delivery",
		"title": "Final Delivery",
		"sClass": "rcol",
		orderable: true,
		"render": function (data) {
			var date = new Date(data);
			var month = date.getMonth() + 1;
			return date.getDate() + "/" + month + "/" + date.getFullYear();
		}
	}

];


fnClientBrief = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'desc');
        setcolvis(tableList, columnsIndex);
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        UIkit.modal("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        console.log(x);
        TABLES[tableList].ajax.reload();
        UIkit.modal("#" + modalSearch).hide();
    },
    create: function () {
        var optionsForm = GetOptionsForm(function () {
            return $("#" + formCreate).parsley().isValid();
        }, function (response, statusText, xhr, $form) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                fnClientBrief.reloadlist();
                UIkit.modal("#" + modalCreate).hide();
                //location.reload(true);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(formCreate, optionsForm);
        setFormAction(formCreate, ajaxUrlCreate);
        UIkit.modal("#" + modalCreate).show();
    },
    update: function (id) {
        ajaxGet(ajaxUrlGet + "/" + id, {}, function (response) {
            response = parseJson(response);

            UIkit.modal("#" + modalUpdate).show();
            setFormAction(formUpdate, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                return $("#" + formUpdate).parsley().isValid();
            }, function (response, statusText, xhr, $form) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    fnClientBrief.reloadlist();
                    UIkit.modal("#" + modalUpdate).hide();
                    //location.reload(true);
                } else {
                    DangerNotif(response.message);
                }
            });
            InitForm(formUpdate, optionsForm);
            FormLoadByDataUsingName(response.data, formUpdate);
        });
    },
    delete: function (id) {
        var text = t_delete;
        confirmDialog(text, function () {
            ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    if (response.message === 'unassigned') {
                        swal.fire({
                            title: "Hapus Job Terlebih Dahulu!",
                            text: "",
                            type: "success",
                            buttonsStyling: false,
                            confirmButtonText: "Confirm me!",
                            confirmButtonClass: "btn btn-brand"
                        });
                    }
                    else {
                        SuccessNotif(t_deletesuccess);
                        fnClientBrief.reloadlist();
                        hideAllModal();
                        //location.reload(true);
                    }
                    //SuccessNotif(t_deletesuccess);
                    //fnClientBrief.reloadlist();
                    //hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    },
    jobAdd: function (RecordId) {
        var formUpdate = "ClientBriefFormUpdate";
        $("#JobCreateModal #client_brief_id").val(RecordId);
        $("#JobCreateForm")[0].reset();
        $("#JobCreateModal #brand_id");
        $("#JobCreateModal").modal('show');
        $('#startjoblabel').hide();
        $('#startjoblabel').html("");
        $('#deadlinejoblabel').hide();
        $('#deadlinejoblabel').html("");
        $('#finaldeliveryjoblabel').hide();
        $('#finaldeliveryjoblabel').html("");
        ajaxGet(ajaxUrlGet + "/" + RecordId, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#JobCreateModal #start_job").val(moment(response.data.start_campaign).format("YYYY-MM-DD"));
                $("#JobCreateModal #deadline").val(moment(response.data.deadline).format("YYYY-MM-DD"));
                $("#JobUpdateModal #final_delivery").val(moment(response.data.final_delivery).format("YYYY-MM-DD"));
                $("#JobCreateModal #business_unit_division_id").val(response.data.business_unit_id);
                startJobDate = response.data.start_campaign;
                deadlineDate = response.data.deadline;
                finaldeliveryDate = response.data.final_delivery;

                if (response.data.business_unit_id) {
                    ajaxUrlListDepartment = ajaxUrlListDepartment.replace("{divisionId}", response.data.business_unit_id.toString());

                    $('#business_unit_id').append('<option value="' + response.data.business_unit_id + '" selected>' + response.data.business_unit_name + '</option>');
                }
                if (response.data.bussiness_type_id) {
                    $('#bussiness_type_id').append('<option value="' + response.data.bussiness_type_id + '" selected>' + response.data.bussiness_type_name + '</option>');
                }

                if (response.data.client_id) {
                    ajaxUrlBrandList = ajaxUrlBrandList.replace("{id}", response.data.client_id);
                    console.log(ajaxUrlBrandList);
                    $('#client_id').append('<option value="' + response.data.client_id + '" selected>' + response.data.company_name + '</option>');
                }

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

                $('#team_id').setcombobox({
                    data: { 'rows': 100 },
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

                $('#main_service_category').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListMainService,
                    searchparam: 'search',
                    labelField: 'name',
                    valueField: 'id'
                });

                $('#campaign_type_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListcampaigntype,
                    searchparam: 'search_campaign_type',
                    labelField: 'campaign_type',
                    valueField: 'id'
                });
            }


        });

    },
    setJobformcreate: function () {
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
                        endprocess();
                        //$("#JobCreateModal").modal('hide');
                        fnClientBrief.reloadlist();
                        SuccessNotif(t_createsuccess);
                        //location.reload(true);
                    } else {
                        endprocess();
                        //$("#JobCreateModal").modal('hide');
                        fnClientBrief.reloadlist();
                        ajaxDelete(ajaxUrlDeleteJob + "/" + response.data.job_number, {}, function (response) {
                            response = parseJson(response);
                            if (!response.success) {
                                DangerNotif(response.message);
                                //location.reload(true);
                            }
                        });
                    }
                });

            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(frmopt, optionsFormCr);
        setFormAction(frmopt, ajaxUrlCreateJob);
    }
};

$(function () {
    //location.reload(true);
    
    initlanguage(lang_pm);
 

    ajaxGet(ajaxUrlGroupAccess + "/" + 'PM.CLIENT_BRIEF', {}, function (response) {
        response = parseJson(response);
        isRead = response.isRead;
        isAdd = response.isAdd;
        isEdit = response.isEdit;
        isDelete = response.isDelete;

        if (isAdd) {
            $('#btn_create_clientbrief').show();
        } else {
            $('#btn_create_clientbrief').hide();
        }
        fnClientBrief.generatelist();
        $('#ddl_isdeleted').hide();
        $('#search_val').show();
        fnClientBrief.setJobformcreate();
        $('#JobCreateForm #btn-savejob').prop("disabled", true);
        $('.datepicker').setdatepicker();
    });
    


});


function funcCondition(obj) {
    if ($(obj).val() === 'Search_is_deleted') {
        $('#ddl_isdeleted').show();
        $('#search_val').hide();
    }
    else {
        $('#ddl_isdeleted').hide();
        $('#search_val').show();
    }
}

function startjobvalidation(formname) {
    
    var startJob = $('#start_job');
    var deadlineJob = $('#' + formname + ' #deadline');
    var finaldeliveryJob = $('#' + formname + ' #final_delivery');

    if (startJob.val() !== "") {

        if (startJob.val() < startJobDate) {
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
            if (startJob.val() > deadlineDate) {
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
        $('#' + formname + ' #btn-savejob').prop("disabled", true);
    }
}

function deadlinejobvalidation(formname) {



    var startJob = $('#start_job');
    var deadlineJob = $('#' + formname + ' #deadline');
    var finaldeliveryJob = $('#' + formname + ' #final_delivery');

    if (deadlineJob.val() <= deadlineDate) {
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

    var deadlineJob = $('#' + formname + ' #deadline');
    var finaldeliveryJob = $('#' + formname + ' #final_delivery');

    if (finaldeliveryJob.val() <= finaldeliveryDate) {
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