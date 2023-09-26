var webUrl = WEB + "/Hris/CandidateRecruitment";
var ajaxUrl = API + "/Hris/CandidateRecruitment";
var ajaxUrlCreate = ajaxUrl + "/CreateDetil";
var xform = "TrackingNewApplicantFormCreate";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";


$(function () {
    $('#last_education').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlEducationLevel,
        searchparam: 'Search_education_name',
        labelField: 'education_name',
        valueField: 'id',
        searchField: 'education_name'
    });
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            window.location.href = WEB + "/hris/Applicant/TrackingNewApplicant/" + $('#job_vacancy_id').val();
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);

    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx'] });
    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
});
