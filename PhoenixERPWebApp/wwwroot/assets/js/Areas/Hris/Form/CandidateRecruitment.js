var ajaxUrl = API + "/Hris/CandidateRecruitment";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "CandidateRecruitmentList";
var formSearch = "CandidateRecruitmentFormSearch";
var modalSearch = "CandidateRecruitmentModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name != "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
}
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
        "data": "achievement",
        "title": "achievement",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "address",
        "title": "address",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "alcohol_willing",
        "title": "alcohol_willing",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "applicant_progress_id",
        "title": "applicant_progress_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "approver_id",
        "title": "approver_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "birth_place",
        "title": "birth_place",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "certificate",
        "title": "certificate",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "cigaret_willing",
        "title": "cigaret_willing",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "city_id",
        "title": "city_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_last_address",
        "title": "company_last_address",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_last_from",
        "title": "company_last_from",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_last_name",
        "title": "company_last_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_last_phone",
        "title": "company_last_phone",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "company_last_until",
        "title": "company_last_until",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "country",
        "title": "country",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "course_institution",
        "title": "course_institution",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "course_tittle",
        "title": "course_tittle",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "course_years",
        "title": "course_years",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "date_birth",
        "title": "date_birth",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "education_level_id",
        "title": "education_level_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "education_year_from",
        "title": "education_year_from",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "education_year_to",
        "title": "education_year_to",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "email",
        "title": "email",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "file_id",
        "title": "file_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "first_name",
        "title": "first_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "gender",
        "title": "gender",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "identity_card",
        "title": "identity_card",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "institusion",
        "title": "institusion",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "interest",
        "title": "interest",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "job_repository_id",
        "title": "job_repository_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "last_education",
        "title": "last_education",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "last_name",
        "title": "last_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "last_position",
        "title": "last_position",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "looking_job",
        "title": "looking_job",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "major",
        "title": "major",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "married_status_id",
        "title": "married_status_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "mobile_number",
        "title": "mobile_number",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "nationality_id",
        "title": "nationality_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "nick_name",
        "title": "nick_name",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "pay_slip",
        "title": "pay_slip",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "phone_number",
        "title": "phone_number",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "portofolio",
        "title": "portofolio",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "profile_file",
        "title": "profile_file",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "province",
        "title": "province",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "relation_people",
        "title": "relation_people",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "religion_id",
        "title": "religion_id",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "resume_screening",
        "title": "resume_screening",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "source_recruitment",
        "title": "source_recruitment",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "work_number",
        "title": "work_number",
        "sClass": "",
        orderable: true
    }    ,{
        "data": "zip_code",
        "title": "zip_code",
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/CandidateRecruitment/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnCandidateRecruitment.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnCandidateRecruitment = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex)
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        $("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x == "reset") {
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
                        fnCandidateRecruitment.reloadlist();
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
    fnCandidateRecruitment.generatelist();
    initlanguage(lang_hr);
});
 

