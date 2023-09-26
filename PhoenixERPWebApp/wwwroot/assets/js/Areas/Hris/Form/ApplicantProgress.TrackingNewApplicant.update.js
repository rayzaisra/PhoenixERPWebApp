var webUrl = WEB + "/Hris/CandidateRecruitment";
var ajaxUrl = API + "/Hris/CandidateRecruitment";
var ajaxUrlCreate = ajaxUrl + "/Update";
var ajaxUrlUpdate = API + "/Hris/CandidateRecruitment/UpdateCustom";
var ajaxUrlGet = ajaxUrl + "/Get";
var xform = "TrackingNewApplicantFormUpdate";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";



$(function () {
    var xid = $('#id').val();
    var vacancy_id = $('#job_vacancy_id').val();

   
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

    //$('#cv').setuploadbox({});
    //$('#photo').setuploadbox({});
    //$('#portfolio').setuploadbox({});

    if (xid != null) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            //console.log("Hasil = " + JSON.stringify(response));
            if (response.data) {
                $('#first_name').val(response.data.first_name);
                $('#last_name').val(response.data.last_name);
                $('#nick_name').val(response.data.nick_name);
                $('#code').val(response.data.code);
                $('#applicant_progress_id').val(response.data.applicant_progress_id);
                var $radiosgender = $('input:radio[name=gender]');
                var cekgender = response.data.gender;
                if (cekgender === "male") {
                    $radiosgender.filter('[value=male]').prop('checked', true);
                } else {
                    $radiosgender.filter('[value=female]').prop('checked', true);
                }
                $('#birth_place').val(response.data.birth_place);
                $('#address').val(response.data.address);
                $('#email').val(response.data.email);
                var dateBirth = moment(response.data.date_birth).format("MM/DD/YYYY");
                $('#date_birth').val(dateBirth);
                //alert(response.data.last_education);
                if (response.data.last_education) {
                    ajaxGet(ajaxUrlEducationLevel, { Search_education_id: response.data.last_education }, function (resp) {
                       //resp = parseJson(resp);
                       //console.log("Hasil resp = " + JSON.stringify(resp));
                        var educatioName = '';

                        $.each(resp.rows, function (index, value) {
                            if (value.id == response.data.last_education) {
                                educatioName = value.education_name;
                            }
                           // console.log("value = " + value.id + " name = " + value.education_name);
                        });

                        $('#last_education').append('<option value="' + response.data.last_education + '" selected>' + educatioName + '</option>');
                     
                    });

                }

                $('#year_experience').val(response.data.year_experience);
                $('#phone_number').val(response.data.phone_number);
                $('#source').val(response.data.source);
                if (response.data.cv_file === "") {
                    $('#divcvplace .input-group').remove();
                    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                } else {
                    $('#divcvplace .input-group').remove();
                    $('#cv').val(response.data.cv_file);
                    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                }

                if (response.data.photo_file === "") {
                    $('#divphotoplace .input-group').remove();
                    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
                } else {
                    $('#divphotoplace .input-group').remove();
                    $('#photo').val(response.data.photo_file);
                    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
                }

                if (response.data.portofolio_file === "") {
                    $('#divportfolioplace .input-group').remove();
                    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                } else {
                    $('#divportfolioplace .input-group').remove();
                    $('#portfolio').val(response.data.portofolio_file);
                    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                }
                var $radioscigarete = $('input:radio[name=out_of_town_willing]');
                var cekcigarete = response.data.out_of_town_willing;
                if (cekcigarete === true) {
                    $radioscigarete.filter('[value=true]').prop('checked', true);
                } else {
                    $radioscigarete.filter('[value=false]').prop('checked', true);
                }
                var $radiosalcohol = $('input:radio[name=alcohol_willing]');
                var cekalcohol = response.data.alcohol_willing;
                if (cekalcohol === true) {
                    $radiosalcohol.filter('[value=true]').prop('checked', true);
                } else {
                    $radiosalcohol.filter('[value=false]').prop('checked', true);
                }
            } else {
                DangerNotif("No data job vacancy found for selected ID.");
            }

        });
    }

   
});
