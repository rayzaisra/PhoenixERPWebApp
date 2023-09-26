var ajaxUrl = API + "/Hris/CandidateRecruitment";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = API + "Hris/CandidateRecruitment/DeleteDetil";
var ajaxUrlGet = API + "/Hris/JobVacancy/GetWithDetial";
var tableList = "ApplicantProgressList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var ajaxUrlListJobTitle = API + "Hris/JobTitle/List/";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = { Search_status: 1 };
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
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
    }  , {
        "data": null,
        "title": "Code",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var datastring = '';
        
            datastring = '<a  href="/Hris/ApplicantDetail/PersonalInfo/' + row.id + '/' + $('#id').val() +'">' + row.code + '</a>';

            return datastring;
        }
    }   ,{
        "data": null,
        "title": "Name",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            dataraw = row.first_name + " " + row.last_name;
            return dataraw;
        }
    }, {
        "data": null,
        "title": "Source",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            
            return row.source;
        }
    }, {
        "data": null,
        "title": "<center>Education</center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            dataraw = row.last_education;
            return "<center>" + dataraw +"</center>";
        }
    }, {
        "data": null,
        "title": "<center>Year Experience</center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = '';
            dataraw = row.year_experience;
            return "<center>" + dataraw +"</center>";
        }
    },
    {
        "data": null,
        "title": "<center><span class='translate'  data-args='app-action'>Actions</span></center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<center><a class="btn btn-secondary" href="/Hris/Applicant/TrackingNewApplicantUpdate/' + row.id + '/' + $('#id').val() +'"><i class="la la-edit"></i> Details</a>&nbsp';
            retval += '<a class="btn btn-secondary" onClick="fnApplicantProgress.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a></center>';
            return retval;

        }
    }   
];

fnApplicantProgress = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, { Search_applicant_progress_id: 0, Search_job_vacancy_id:$('#id').val() }, columnsIndex, 'id', 'asc');
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
                ajaxPost(ajaxUrlDelete, { id: id, job_vacancy_id:$('#id').val()}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnApplicantProgress.reloadlist();
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
    fnApplicantProgress.generatelist();
    initlanguage(lang_hr);

    var xid = $('#id').val();

    if (xid != '') {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            console.log(response);
            $('#code').val(response.data.code);
            $('#request_code').val(response.data.request_code);
            $('#post_date').val(response.data.created_on);
            $('#vacancy_name').val(response.data.vacancy_name);


            $('#vacancy_name').append("<option value=" + $('#vacancy_name').val() + ">" + response.data.vacancy_name + "</option>");
            $('#vacancy_name').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlListJobTitle,
                searchparam: 'Search_title_name',
                labelField: 'title_name',
                valueField: 'id',
                searchField: 'title_name'
            });
        });
    }
 
});


 

