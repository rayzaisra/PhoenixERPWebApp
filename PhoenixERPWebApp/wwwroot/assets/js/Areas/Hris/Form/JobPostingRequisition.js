var ajaxUrl = API + "Hris/HiringRequest";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = API + "Hris/CandidateRecruitment/CreateDetil";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/GetListHiringMppWithCode";
var ajaxUrlGetVacancy = API + "Hris/JobVacancy/GetDetialWithIDHiring";
var ajaxUrlGetVacancyName = API + "Hris/JobVacancy/GetVancacyNameByCode";
var ajaxUrlListFinancialCode = API + "Hris/JobVacancy/ListPostingCode";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";
var tableList = "JobPostingRequisitionList";
var formSearch = "JobPostingRequisitionFormSearch";
var modalSearch = "JobPostingRequisitionModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
};

var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }    ,{
        "data": "business_unit_id",
        "title": "business_unit_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "candidate_age_range_end",
        "title": "candidate_age_range_end",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "candidate_age_range_start",
        "title": "candidate_age_range_start",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "candidate_experience",
        "title": "candidate_experience",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "candidate_qualification",
        "title": "candidate_qualification",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "central_resource_id",
        "title": "central_resource_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "client_brief_id",
        "title": "client_brief_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "code",
        "title": "code",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_post_description",
        "title": "job_post_description",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_title",
        "title": "job_title",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "notes",
        "title": "notes",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "number_of_position",
        "title": "number_of_position",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "reg_code",
        "title": "reg_code",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "salary_end",
        "title": "salary_end",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "salary_start",
        "title": "salary_start",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "status",
        "title": "status",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/Hris/JobPostingRequisition/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnJobPostingRequisition.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnJobPostingRequisition = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex);
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        $("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();
        $("#" + modalSearch).modal('hide');
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
                        fnJobPostingRequisition.reloadlist();
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
    //fnJobPostingRequisition.generatelist();
    var xid = $('#job_vacancy_id').val();

    $('#job_vacancy_code').setcombobox({
        data: {
            'rows': 100
        },
        url: ajaxUrlListFinancialCode,
        searchparam: '',
        labelField: 'vacancy_name',
        valueField: 'code',
        searchField: 'code',

    });

    $('#job_vacancy_code').on('change', function () {
        var code = this.value;
        if (code != '') {
            ajaxGet(ajaxUrlGetVacancyName + "/" + code, {}, function (response) {
                response = parseJson(response);
                console.log(response)
                $('#name_vancancy').val(response.data.vacancy_name);
                $('#label_job').text('');
                $('#label_job').text(response.data.countHIringRequest);
                //console.log("Hasil = " + response.data.vacancy_name);
            });
        }
      
        //console.log(response);
        //if (response.data !== null) {
        //    ajaxGet(ajaxUrlGetVacancy + "/" + response.data.id, {}, function (response2) {
        //        response2 = parseJson(response2);
        //        if (response.data !== null) {
        //            if (response2.data.posting_status === "5" || response2.data.posting_status === 5){
        //                DangerNotif('Job Vacancy Already Closed');
        //                return false;

        //            }else {
        //                SuccessNotif("Request Code Found");
        //                $('#label_job').html('<strong>' + response.data.business_unit_name + ' / ' + response.data.job_title_name + ' (' + response.data.job_grade_name + ')</strong>');
        //                window.location.href = WEB + "/hris/JobPosting/Career/" + response2.data.id;
        //            }
        //             } else {
        //            DangerNotif('Posting Code Not Available');
        //            return false;
        //        }
        //    });

        //} else {
        //    DangerNotif('Posting Code Not Available');
        //    return false;
        //}
    });

    $('#last_education').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlEducationLevel,
        searchparam: 'Search_education_name',
        labelField: 'education_name',
        valueField: 'id',
        searchField: 'education_name'
    });

    var xform = "CareerFormCreate";
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            window.location.href = WEB + "/hris/JobPosting/Submit/" + response.data.code;
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);

    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});

  

    //if (xid !== "") {



    //}
    
});

function CheckJob() {

    var code = $('#job_vacancy_code').val();

    //ajaxGet(ajaxUrlGet + "/" + code, {}, function (response) {
    //    response = parseJson(response);
    //    //console.log(response);
    //    if (response.data !== null) {
    //        ajaxGet(ajaxUrlGetVacancy + "/" + response.data.id, {}, function (response2) {
    //            response2 = parseJson(response2);
    //            if (response.data !== null) {
    //                if (response2.data.posting_status === "5" || response2.data.posting_status === 5){
    //                    DangerNotif('Job Vacancy Already Closed');
    //                    return false;

    //                }else {
    //                    SuccessNotif("Request Code Found");
    //                    $('#label_job').html('<strong>' + response.data.business_unit_name + ' / ' + response.data.job_title_name + ' (' + response.data.job_grade_name + ')</strong>');
    //                    window.location.href = WEB + "/hris/JobPosting/Career/" + response2.data.id;
    //                }
    //                 } else {
    //                DangerNotif('Posting Code Not Available');
    //                return false;
    //            }
    //        });
              
    //    } else {
    //        DangerNotif('Posting Code Not Available');
    //        return false;
    //    }
        
    //});

    ajaxGet(ajaxUrlGetVacancy + "/" + code, {}, function (response2) {
        response2 = parseJson(response2);
        if (response2.data !== null) {
            if (response2.data.posting_status === "5" || response2.data.posting_status === 5) {
                DangerNotif('Job Vacancy Already Closed');
                return false;

            } else {
                SuccessNotif("Request Code Found");
                //$('#label_job').html('<strong>' + response.data.business_unit_name + ' / ' + response.data.job_title_name + ' (' + response.data.job_grade_name + ')</strong>');
                //window.location.href = WEB + "/hris/JobPosting/Career/" + response2.data.id;
                window.location.href = WEB + "/Hris/Applicant/TrackingNewApplicant/" + response2.data.id;

            }
        } else {
            DangerNotif('Posting Code Not Available');
            return false;
        }
    });

}
 

