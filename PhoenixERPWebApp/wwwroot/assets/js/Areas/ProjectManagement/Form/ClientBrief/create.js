var ajaxUrl = API_PM + "/ClientBrief";
var ajaxUrlCampaign = API_PM + "/Campaign";
var ajaxUrlList = ajaxUrl + "/Search";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlCreateCampaign = ajaxUrlCampaign + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
//var brandURL = API_PM + "/CompanyBrand/list";
var companyUrl = API_PM + "/CompanyBrand/companylist";
var ajaxUrlListCampaign = ajaxUrlCampaign + "/list";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListClientBriefContentRef = API_PM + "/ClientBriefContentRef/list";
var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
var ajaxUrlListBusinesstype = API_PM + "/BussinessType/list";
var ajaxUrlListcampaigntype = API_PM + "/CampaignType/list";
var formCreate = "ClientBriefFormCreate";
var contentno = 0;
var ajaxUrlCheckId = ajaxUrl + "/CheckUserId";

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
            DangerNotif("minimum 1 Content");
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
};

$(function () {
    $('#btn-save').prop("disabled", true);
    $('#deadline').prop("disabled", true);
    $('#final_delivery').prop("disabled", true);

    var dateToday = new Date();
    dateToday.setDate(dateToday.getDate());
    var last30Days = new Date();
    last30Days.setDate(last30Days.getDate() - 30);

    $('.datepicker').datepicker({
        autoclose: true,
        todayHighlight: true,
        startDate: last30Days,
        format:"yyyy-mm-dd"
    });

    //$('.datepicker').setdatepicker();

    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + formCreate).valid();
        if (xret) startprocess();
        return xret;

    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            var xretid = response.data.id;
            convault(null, WEB + "/projectmanagement/clientbrief/update/" + xretid);
            SuccessNotif(t_createsuccess);

        } else {

            DangerNotif(response.message);
        }
    });
    InitForm(formCreate, optionsForm);
    setFormAction(formCreate, ajaxUrlCreate);
                 
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

        }
    });
               
    $('#business_unit_id').setcombobox({
        data: { 'rows': 100, Seach_is_finance: true },
        url: ajaxUrlListOrganization,
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
        valueField: 'id',
    });
       
    ajaxGet(ajaxUrlListClientBriefContentRef, { page: 100, sortBy: 'seq' }, function (res) {
        if (res.success) {
            $.each(res.rows, function (key, value) {
                fnclientbriefcreate.addcontent(value.name);
            });
        } else {
            DangerNotif(res.message);
        }
    });

    ajaxGet(ajaxUrlCheckId, null, function (res) {
        if (res.success) {
            SuccessNotif(res.message);
            //$('#btn-save').prop("disabled", false);
        } else {
            //DangerNotif(res.message);
            swal('Warning', res.message, 'warning');
            $('#ClientBriefFormCreate input').attr('readonly', 'readonly');
            $('#ClientBriefFormCreate input').prop('disabled', true);
            $('#ClientBriefFormCreate input[type="text"], textarea').attr('readonly', 'readonly');
            $('#ClientBriefFormCreate button').attr("disabled", true);
            $('#ClientBriefFormCreate .input-group-append').empty();
            $("#ClientBriefFormCreate select").select2({ disabled: 'readonly' });
            $('#JobUpdateForm input').attr('readonly', 'readonly');
            $('#JobUpdateForm input[type="text"], textarea').attr('readonly', 'readonly');
            $('#JobUpdateForm button').attr("disabled", true);
            $("#JobUpdateForm select").select2({ disabled: 'readonly' });
            $('#ClientBriefJob button').attr("disabled", true);
            $('#contentref').hide();
            //$('#btn-save').prop("disabled", true);
        }
    });

});

function startcampaignvalidation() {
    var startCampaign = $('#start_campaign').val();
    var deadline = $('#deadline').val();
    var finalDelivery = $('#final_delivery').val();

    if (startCampaign !== "") {
        $('#deadline').prop("disabled", false);
        if (startCampaign !== "" && deadline !== "" && finalDelivery !== "") {
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