var webUrl = WEB + "/Hris/EmployeeBasic";
var API_FIX = API.substr(API.length - 1, API.length);
//console.log('api' + API_FIX);
if (API_FIX == "/")
	API_FIX = API.substr(0, API.length - 1);
else
	API_FIX = API;

//console.log('api FIX ' + API_FIX);

var ajaxUrl = API_FIX + "/Hris/EmployeeBasicInfo";
var ajaxUrlUpdate = ajaxUrl + "/UpdateNewEmployeeBasic";
var xform = "EmployeeBasicInfoFormUpdate";
var ajaxUrlGet = ajaxUrl + "/GetWithDetail";
var ajaxUrlBusinessUnitJobLevel = API_FIX + "/Hris/BussinesUnitJobLevel/List";

var ajaxUrlLegalEntity = API_FIX + "/General/LegalEntity/List";
var ajaxUrllocationGet = API_FIX + "/Hris/JobVacancy/GetAllLocation";
var ajaxUrlEmploymentStatus = API_FIX + "/Hris/EmployementStatus/List";
var ajaxUrlGetBusinessUnitJobLevelWithID = API_FIX + "/Hris/BussinesUnitJobLevel/GetDetail";
var ajaxUrlListJobLevelGeneral = API_GENERAL + "/BusinessUnit/ListWithParent";

var ajaxUrlListJobLevel = API_HRIS + "/JobLevel/List";
var ajaxUrlListJobGrade = API_HRIS + "/JobGrade/List";
var ajaxUrlListJobTitle = API_HRIS + "/JobTitle/List/";

var emergencyUrl = API_FIX + "/Hris/EmployeeEmergencyContact";
var emergencyUrlList = emergencyUrl + "/List";
var emergencyUrlGet = emergencyUrl + "/Get";
var emergencyUrlUpdate = emergencyUrl + "/Update";
var emergencyUrlCreate = emergencyUrl + "/Create";
var emergencyUrlDelete = emergencyUrl + "/Delete";

var courseUrl = API_FIX + "/Hris/EmployeeTraining";
var courseUrlList = courseUrl + "/List";
var courseUrlGet = courseUrl + "/Get";
var courseUrlUpdate = courseUrl + "/Update";
var courseUrlCreate = courseUrl + "/Create";
var courseUrlDelete = courseUrl + "/Delete";

var educationUrl = API_FIX + "/Hris/Education";
var educationUrlList = educationUrl + "/List";
var educationUrlGet = educationUrl + "/Get";
var educationUrlUpdate = educationUrl + "/Update";
var educationUrlCreate = educationUrl + "/Create";
var educationUrlDelete = educationUrl + "/Delete";

var historyUrl = API_FIX + "/Hris/EmployeeExperience";
var historyUrlList = historyUrl + "/List";
var historyUrlGet = historyUrl + "/Get";
var historyUrlUpdate = historyUrl + "/Update";
var historyUrlCreate = historyUrl + "/Create";
var historyUrlDelete = historyUrl + "/Delete";

var nationalityURL = API_FIX + "/Hris/Nationality/List";
var educationLevelURL = API_FIX + "/Hris/EducationLevel/List";
var religionURL = API_FIX + "/Hris/Religion/List";
var marriedstatusURL = API_FIX + "/Hris/MarriedStatus/List";
var familyStatusURL = API_FIX + "/Hris/FamilyStatus/List";
var marriedURL = API_FIX + "/Hris/MarriedStatus/List";
var InstitusionURL = API_FIX + "/Hris/Institution/List";
var EmployeestatusURL = API_FIX + "/Hris/StatusEmployee/List";
var BranchBankURL = API_FIX + "/Hris/BankBranch/List";
var PtkpSettingUrl = API_FIX + "/Hris/PTKPSetting/List";

var familyUrl = API_FIX + "/Hris/Family";
var familyUrlList = familyUrl + "/List";
var familyUrlGet = familyUrl + "/Get";
var familyUrlUpdate = familyUrl + "/Update";
var familyUrlCreate = familyUrl + "/Create";
var familyUrlDelete = familyUrl + "/Delete";

var tableFamily = "FamilyList";
var tableEmergency = "EmergencyContactList";
var tableHistory = "HistoryList";
var tableCourse = "CourseList";
var tableEducation = "EducationList";

$(function () {
    // initiate Year Picker
    var xid = $("#" + xform + ' [name="id"]').val();

    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            setFormAction(xform, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                var xret = $("#" + xform).valid();
                if (xret) startprocess();
                return xret;
            }, function (response, statusText, xhr, $form) {
                endprocess();
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    redirecttolink(webUrl);
                } else {
                    DangerNotif(response.message);
                }
            });
            InitForm(xform, optionsForm);
            setFormAction(xform, ajaxUrlUpdate);
            initlanguage(lang_hr);
            FormLoadByDataUsingName(response.data, xform);

            $('#photo').setuploadbox();
            $('#filemaster_id').setuploadbox();

            if (response.data.legal_entity_id != null) {
                $('#legal_entity_id').append("<option value=" + response.data.legal_entity_id + ">" + response.data.legal_entity_name + "</option>");
                $("#legal_entity_id").val([response.data.legal_entity_id]).trigger('change');
            }
            $('#legal_entity_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlLegalEntity,
                searchparam: 'Search_legal_entity_name',
                labelField: 'legal_entity_name',
                valueField: 'id',
                searchField: 'legal_entity_name'
            });

            if (response.data.location_id != null) {
                $('#location_id').append("<option value=" + response.data.location_id + ">" + response.data.location_name + "</option>");
                $("#location_id").val([response.data.location_id]).trigger('change');
            }
            $('#location_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrllocationGet,
                searchparam: 'Search_location_name',
                labelField: 'location_name',
                valueField: 'id',
                searchField: 'location_name'
            });
            if (response.data.nationality_id != null) {
                $('#nationality_id').append("<option value=" + response.data.nationality_id + ">" + response.data.nationality_name + "</option>");
                $("#nationality_id").val([response.data.nationality_id]).trigger('change');
            }
            $('#nationality_id').setcombobox({
                data: { 'rows': 100 },
                url: nationalityURL,
                maxItems: 0,
                searchparam: 'Search_nationality_name',
                labelField: 'nationality_name',
                valueField: 'id'
            });
            if (response.data.last_education != null) {
                $('#last_education').append("<option value=" + response.data.last_education + ">" + response.data.last_education_name + "</option>");
                $("#last_education").val([response.data.last_education]).trigger('change');
            }
            $('#last_education').setcombobox({
                data: { 'rows': 100 },
                url: educationLevelURL,
                maxItems: 0,
                searchparam: 'Search_education_name',
                labelField: 'education_name',
                valueField: 'id'
            });
            if (response.data.religion_id != null) {
                $('#religion_id').append("<option value=" + response.data.religion_id + ">" + response.data.religion_name + "</option>");
                $("#religion_id").val([response.data.religion_id]).trigger('change');
            }
            $('#religion_id').setcombobox({
                data: { 'rows': 100 },
                url: religionURL,
                maxItems: 0,
                searchparam: 'Search_religion_name',
                labelField: 'religion_name',
                valueField: 'id'
            });
            if (response.data.married_status_id != null) {
                $('#married_status_id').append("<option value=" + response.data.married_status_id + ">" + response.data.married_status_name + "</option>");
                $("#married_status_id").val([response.data.married_status_id]).trigger('change');
            }
            $('#married_status_id').setcombobox({
                data: { 'rows': 100 },
                url: marriedstatusURL,
                maxItems: 0,
                searchparam: 'Search_married_name',
                labelField: 'married_name',
                valueField: 'id'
            });

            if (response.data.employement_status_id != null) {
                $('#employement_status_id').append("<option value=" + response.data.employement_status_id + ">" + response.data.employement_status_name + "</option>");
                $("#employement_status_id").val([response.data.employement_status_id]).trigger('change');
            }
            $('#employement_status_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlEmploymentStatus,
                maxItems: 0,
                searchparam: 'Search_employment_status_name',
                labelField: 'employment_status_name',
                valueField: 'id'
            });			
            
            $('#education_institusion_id').setcombobox({
            	data: { 'rows': 100 },
            	url: InstitusionURL,
            	maxItems: 0,
            	searchparam: 'Search_institution_name',
            	labelField: 'institution_name',
            	valueField: 'id'
            });
			
            if (response.data.ptkp_setting_id != null) {
            	$('#ptkp_setting_id').append("<option value=" + response.data.ptkp_setting_id + ">" + response.data.ptkp_setting_name + "</option>");
            	$("#ptkp_setting_id").val([response.data.ptkp_setting_id]);
            }
            
            $('#ptkp_setting_id').setcombobox({
            	data: { 'rows': 100 },
            	url: PtkpSettingUrl,
            	maxItems: 0,
            	searchparam: 'Search_name',
            	labelField: 'name',
            	valueField: 'id'
            });
            
            if (response.data.bank_id != null) {
            	$("#bank_id").val([response.data.bank_id]);
            }

            if (response.data.listFamily.length > 0) {
                DeleteAja('no_data', 'FamilyList');
                $('#tmpfamilyinfo').empty();
                var tmpfamilyinfono = 0;
                $.each(response.data.listFamily, function (index, value) {
                	$('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][Id]" value="' + value.Id + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][name]" value="' + value.name + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][phone_no]" value="' + value.phone_no + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][birthday]" value="' + value.birthday + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][other_no]" value="' + value.other_no + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_education_level_id]" value="' + value.family_education_level_id + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][place_birth]" value="' + value.place_birth + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_status_id]" value="' + value.family_status_id + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_status_name]" value="' + value.family_status_name + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][religion_id]" value="' + value.religion_id + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][gender]" value="' + value.gender + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][address]" value="' + value.address + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][institution_id]" value="' + value.institution_id + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][description]" value="' + value.description + '"  >');
                    $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][married_status_id]" value="' + value.married_status_id + '"  >');                   

                    AddRowTableFamilyInfo(value.name, value.phone_no, value.birthday, value.other_no, value.family_education_level_id, value.place_birth, value.family_status_id, value.family_status_name, value.religion_id, value.gender, value.address, value.institution_id, value.description, value.married_status_id);
                    tmpfamilyinfono++;
                });
            }

            if (response.data.listEmergency.length > 0) {
                DeleteAja('no_data', 'EmergencyContactList');
                $('#tmpemergencycontact').empty();
                var tmpemergencycontactno = 0;
                $.each(response.data.listEmergency, function (index, value) {
                    $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][family_status_id]" value="' + value.family_status_id + '"  >');
                    $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][relation_name]" value="' + value.relation_name + '"  >');
                    $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][phone_no]" value="' + value.phone_no + '"  >');
                    $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][address]" value="' + value.address + '"  >');
                    $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][full_name]" value="' + value.full_name + '"  >');
                    $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][other_no]" value="' + value.other_no + '"  >');
                   
                    AddRowTableEmergencyContact(value.family_status_id, value.relation_name, value.phone_no, value.address, value.full_name, value.other_no);
                    tmpemergencycontactno++;
                });
            }

            if (response.data.listEducation.length > 0) {
                DeleteAja('no_data', 'EducationList');
                $('#tmpeducationinfo').empty();
                var tmpeducationinfono = 0;
                $.each(response.data.listEducation, function (index, value) {
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][education_level_id]" value="' + value.education_level_id + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][education_level_name]" value="' + value.education_level_name + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][institution_id]" value="' + value.institution_id + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][institution_name]" value="' + value.institution_name + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][start_education]" value="' + value.start_education + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][end_education]" value="' + value.end_education + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][majors]" value="' + value.majors + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][qualification]" value="' + value.qualification + '"  >');
                    $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][address]" value="' + value.address + '"  >');
                    

                    AddRowTableEducationInfo(value.education_level_id, value.education_level_name, value.institution_id, value.institution_name, value.start_education, value.end_education, value.majors, value.qualification, value.address);
                    tmpeducationinfono++;
               });
            }

            if (response.data.listExperience.length > 0) {
                DeleteAja('no_data', 'EmployementHistoryList');
                $('#tmpemploymenthistory').empty();
                var tmpbasicinfoemploymenthistoryno = 0;
                $.each(response.data.listExperience, function (index, value) {
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][company_name]" value="' + value.company_name + '"  >');
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][phone]" value="' + value.phone + '"  >');
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][remark]" value="' + value.remark + '"  >');
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][address]" value="' + value.address + '"  >');
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][start_date]" value="' + value.start_date + '"  >');
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][end_date]" value="' + value.end_date + '"  >');
                    $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][position]" value="' + value.position + '"  >');


                    AddRowTableEmploymentHistory(value.company_name, value.phone, value.remark, value.address, value.start_date, value.end_date, value.position);
                    tmpbasicinfoemploymenthistoryno++;
                });
            }

            if (response.data.listTraining.length > 0) {
                DeleteAja('no_data', 'CourseList');
                $('#tmpcourseseminar').empty();
                var tmpbasicinfocourseseminarno = 0;
                $.each(response.data.listTraining, function (index, value) {
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][name_training]" value="' + value.name_training + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][institution_id]" value="' + value.institution_id + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][address]" value="' + value.address + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][start_training]" value="' + value.start_training + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][end_training]" value="' + value.end_training + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][no_certification]" value="' + value.no_certification + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][expired_certificate]" value="' + value.expired_certificate + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][filemaster_id]" value="' + value.filemaster_id + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][description]" value="' + value.description + '"  >');
                        $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][institution_name]" value="' + value.institution_name + '"  >');


                        AddRowTableCourseSeminar(value.name_training, value.institution_id, value.institution_name, value.address, value.start_training, value.end_training, value.no_certification, value.expired_certificate, value.filemaster_i, value.description);
                        tmpbasicinfocourseseminarno++;
                });
            }

            if (response.data.bussinesUnitJobLevel != null) {
                $('#job_title_id').val(response.data.bussinesUnitJobLevel.job_title_id);

                $('#job_grade_id').append("<option value=" + response.data.bussinesUnitJobLevel.job_grade_id + ">" + response.data.bussinesUnitJobLevel.job_grade_name + "</option>");
                $("#job_grade_id").val([response.data.bussinesUnitJobLevel.job_grade_id]).trigger('change');
                $('#job_grade_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlListJobGrade,
                    searchparam: 'Search_grade_name',
                    labelField: 'grade_name',
                    valueField: 'id',
                    searchField: 'grade_name'
                });

                if (response.data.bussinesUnitJobLevel.business_unit_division_id != null) {
                    $('#business_unit_division_id').append("<option value=" + response.data.bussinesUnitJobLevel.business_unit_division_id + ">" + response.data.bussinesUnitJobLevel.businessUnitDivision.name + "</option>");
                    $("#business_unit_division_id").val([response.data.bussinesUnitJobLevel.business_unit_division_id]).trigger('change');
                }
                $('#business_unit_division_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 800, Search_parent_unit: response.data.bussinesUnitJobLevel.business_unit_subgroup_id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });
                if (response.data.bussinesUnitJobLevel.business_unit_departement_id != null) {
                    $('#business_unit_departement_id').append("<option value=" + response.data.bussinesUnitJobLevel.business_unit_departement_id + ">" + response.data.bussinesUnitJobLevel.businessUnitDepartement.name + "</option>");
                    $("#business_unit_departement_id").val([response.data.bussinesUnitJobLevel.business_unit_departement_id]).trigger('change');
                }
                $('#business_unit_departement_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 700, Search_parent_unit: response.data.bussinesUnitJobLevel.business_unit_division_id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });


                if (response.data.bussinesUnitJobLevel.parent_id != null) {

                    var Division = '';
                    var Departement = '';
                    var Subgroup = '';
                    var Group = '';

                    if (response.data.bussinesUnitJobLevel.businessUnitDivision.name != "") {
                        Division = response.data.bussinesUnitJobLevel.businessUnitDivision.name;
                    } else {
                        Division = '';
                    }

                    if (response.data.bussinesUnitJobLevel.businessUnitDepartement.name != "") {
                        Departement = response.data.bussinesUnitJobLevel.businessUnitDepartement.name;
                    } else {
                        Departement = '';
                    }

                    if (response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != "") {
                        Subgroup = response.data.bussinesUnitJobLevel.businessUnitSubgroup.name;
                    } else {
                        Subgroup = '';
                    }

                    if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != "") {
                        Group = response.data.bussinesUnitJobLevel.businessUnitGroup.name;
                    } else {
                        Group = '';
                    }


                    if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != null && response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != null && response.data.bussinesUnitJobLevel.businessUnitDivision.name != null && response.data.bussinesUnitJobLevel.businessUnitDepartement.name != null) {

                        datanya2 = Group + " / " + Subgroup + " / " + Division + " / " + Departement + " -- " + response.data.bussinesUnitJobLevel.parentBusinessUnit.name;

                    } else if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != null && response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != null && response.data.bussinesUnitJobLevel.businessUnitDivision.name != null) {

                        datanya2 = Group + " / " + Subgroup + " / " + Division + " -- " + response.data.bussinesUnitJobLevel.parentBusinessUnit.name;

                    } else if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != null && response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != null) {
                        datanya2 = Group + " / " + Subgroup + " --  " + response.data.bussinesUnitJobLevel.parentBusinessUnit.name;
                    } else {
                        datanya2 = Group + " --  " + response.data.bussinesUnitJobLevel.parentBusinessUnit.name;
                    }


                    $('#parent_id').append("<option value=" + response.data.bussinesUnitJobLevel.parentBusinessUnit.id + ">" + datanya2 + "</option>");
                    $("#parent_id").val([response.data.bussinesUnitJobLevel.parentBusinessUnit.id]).trigger('change');
                }

                $('#parent_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlBusinessUnitJobLevel,
                    searchparam: 'Search_job_title_name',
                    labelField: 'job_title_name',
                    valueField: 'parent_id',
                    searchField: 'job_title_name',
                    render: function (data) {
                        var datanya = '';
                        if (data.odata) {
                            var Division = '';
                            var Departement = '';
                            var Subgroup = '';
                            var Group = '';

                            if (data.odata.businessUnitDivision.name != "") {
                                Division = data.odata.businessUnitDivision.name;
                            } else {
                                Division = '';
                            }

                            if (data.odata.businessUnitDepartement.name != "") {
                                Departement = data.odata.businessUnitDepartement.name;
                            } else {
                                Departement = '';
                            }

                            if (data.odata.businessUnitSubgroup.name != "") {
                                Subgroup = data.odata.businessUnitSubgroup.name;
                            } else {
                                Subgroup = '';
                            }

                            if (data.odata.businessUnitGroup.name != "") {
                                Group = data.odata.businessUnitGroup.name;
                            } else {
                                Group = '';
                            }


                            if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null && data.odata.businessUnitDepartement.name != null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " => " + Departement + " -- (" + data.odata.job_title_name + ")";

                            } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " -- (" + data.odata.job_title_name + ")";

                            } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null) {
                                datanya = Group + " => " + Subgroup + " --  (" + data.odata.job_title_name + ")";
                            } else {
                                datanya = Group + " --  (" + data.odata.job_title_name + ")";
                            }
                            return datanya;
                        }
                    }
                });


                if (response.data.bussinesUnitJobLevel.id != null) {

                    var Division2 = '';
                    var Departement2 = '';
                    var Subgroup2 = '';
                    var Group2 = '';

                    if (response.data.bussinesUnitJobLevel.businessUnitDivision.name != "") {
                        Division2 = response.data.bussinesUnitJobLevel.businessUnitDivision.name;
                    } else {
                        Division2 = '';
                    }

                    if (response.data.bussinesUnitJobLevel.businessUnitDepartement.name != "") {
                        Departement2 = response.data.bussinesUnitJobLevel.businessUnitDepartement.name;
                    } else {
                        Departement2 = '';
                    }

                    if (response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != "") {
                        Subgroup2 = response.data.bussinesUnitJobLevel.businessUnitSubgroup.name;
                    } else {
                        Subgroup2 = '';
                    }

                    if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != "") {
                        Group2 = response.data.bussinesUnitJobLevel.businessUnitGroup.name;
                    } else {
                        Group2 = '';
                    }


                    if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != null && response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != null && response.data.bussinesUnitJobLevel.businessUnitDivision.name != null && response.data.bussinesUnitJobLevel.businessUnitDepartement.name != null) {

                        datanya = Group2 + " / " + Subgroup2 + " / " + Division2 + " / " + Departement2 + " -- " + response.data.bussinesUnitJobLevel.job_title_name;

                    } else if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != null && response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != null && response.data.bussinesUnitJobLevel.businessUnitDivision.name != null) {

                        datanya = Group2 + " / " + Subgroup2 + " / " + Division2 + " -- " + response.data.bussinesUnitJobLevel.job_title_name;

                    } else if (response.data.bussinesUnitJobLevel.businessUnitGroup.name != null && response.data.bussinesUnitJobLevel.businessUnitSubgroup.name != null) {
                        datanya = Group2 + " / " + Subgroup2 + " --  " + response.data.bussinesUnitJobLevel.job_title_name;
                    } else {
                        datanya = Group2 + " --  " + response.data.bussinesUnitJobLevel.job_title_name;
                    }


                    $('#business_unit_job_level_id').append("<option value=" + response.data.bussinesUnitJobLevel.id + ">" + datanya + "</option>");
                    $("#business_unit_job_level_id").val([response.data.bussinesUnitJobLevel.id]).trigger('change');
                }
            }
            

            $('#business_unit_job_level_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlBusinessUnitJobLevel,
                searchparam: 'Search_job_title_name',
                labelField: 'job_title_name',
                valueField: 'id',
                searchField: 'job_title_name',
                render: function (data) {
                    var datanya = '';
                    if (data.odata) {
                        var Division = '';
                        var Departement = '';
                        var Subgroup = '';
                        var Group = '';

                        if (data.odata.businessUnitDivision.name != "") {
                            Division = data.odata.businessUnitDivision.name;
                        } else {
                            Division = '';
                        }

                        if (data.odata.businessUnitDepartement.name != "") {
                            Departement = data.odata.businessUnitDepartement.name;
                        } else {
                            Departement = '';
                        }

                        if (data.odata.businessUnitSubgroup.name != "") {
                            Subgroup = data.odata.businessUnitSubgroup.name;
                        } else {
                            Subgroup = '';
                        }

                        if (data.odata.businessUnitGroup.name != "") {
                            Group = data.odata.businessUnitGroup.name;
                        } else {
                            Group = '';
                        }


                        if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null && data.odata.businessUnitDepartement.name != null) {

                            datanya = Group + " => " + Subgroup + " => " + Division + " => " + Departement + " -- (" + data.odata.job_title_name + ")";

                        } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null) {

                            datanya = Group + " => " + Subgroup + " => " + Division + " -- (" + data.odata.job_title_name + ")";

                        } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null) {
                            datanya = Group + " => " + Subgroup + " --  (" + data.odata.job_title_name + ")";
                        } else {
                            datanya = Group + " --  (" + data.odata.job_title_name + ")";
                        }
                        return datanya;
                    }
                }
            });
            


            $('#business_unit_job_level_id').on('select2:select', function (e) {
                var id2 = e.params.data.odata.id;

                if (id2 != null) {
                    ajaxGet(ajaxUrlGetBusinessUnitJobLevelWithID + "/" + id2, {}, function (response2) {
                        response2 = parseJson(response2);


                        $('#job_title_id').val(response2.data.job_title_id);

                        $('#job_grade_id').append("<option value=" + response2.data.job_grade_id + ">" + response2.data.job_grade_name + "</option>");
                        $("#job_grade_id").val([response2.data.job_grade_id]).trigger('change');
                        $('#job_grade_id').setcombobox({
                            data: { 'rows': 100 },
                            url: ajaxUrlListJobGrade,
                            searchparam: 'Search_grade_name',
                            labelField: 'grade_name',
                            valueField: 'id',
                            searchField: 'grade_name'
                        });

                        if (response2.data.business_unit_division_id != null) {
                            $('#business_unit_division_id').append("<option value=" + response2.data.business_unit_division_id + ">" + response2.data.businessUnitDivision.name + "</option>");
                            $("#business_unit_division_id").val([response2.data.business_unit_division_id]).trigger('change');
                        }
                        $('#business_unit_division_id').setcombobox({
                            data: { 'rows': 100, Search_business_unit_level: 800, Search_parent_unit: response2.data.business_unit_subgroup_id },
                            url: ajaxUrlListJobLevelGeneral,
                            maxItems: 0,
                            searchparam: 'unit_name',
                            labelField: 'unit_name',
                            valueField: 'id'
                        });
                        if (response2.data.business_unit_departement_id != null) {
                            $('#business_unit_departement_id').append("<option value=" + response2.data.business_unit_departement_id + ">" + response2.data.businessUnitDepartement.name + "</option>");
                            $("#business_unit_departement_id").val([response2.data.business_unit_departement_id]).trigger('change');
                        }
                        $('#business_unit_departement_id').setcombobox({
                            data: { 'rows': 100, Search_business_unit_level: 700, Search_parent_unit: response2.data.business_unit_division_id },
                            url: ajaxUrlListJobLevelGeneral,
                            maxItems: 0,
                            searchparam: 'unit_name',
                            labelField: 'unit_name',
                            valueField: 'id'
                        });


                        if (response2.data.parentBusinessUnit.id != null) {

                            var Division = '';
                            var Departement = '';
                            var Subgroup = '';
                            var Group = '';

                            if (response2.data.businessUnitDivision.name != "") {
                                Division = response2.data.businessUnitDivision.name;
                            } else {
                                Division = '';
                            }

                            if (response2.data.businessUnitDepartement.name != "") {
                                Departement = response2.data.businessUnitDepartement.name;
                            } else {
                                Departement = '';
                            }

                            if (response2.data.businessUnitSubgroup.name != "") {
                                Subgroup = response2.data.businessUnitSubgroup.name;
                            } else {
                                Subgroup = '';
                            }

                            if (response2.data.businessUnitGroup.name != "") {
                                Group = response2.data.businessUnitGroup.name;
                            } else {
                                Group = '';
                            }


                            if (response2.data.businessUnitGroup.name != null && response2.data.businessUnitSubgroup.name != null && response2.data.businessUnitDivision.name != null && response2.data.businessUnitDepartement.name != null) {

                                datanya2 = Group + " / " + Subgroup + " / " + Division + " / " + Departement + " -- " + response2.data.parentBusinessUnit.name;

                            } else if (response2.data.businessUnitGroup.name != null && response2.data.businessUnitSubgroup.name != null && response2.data.businessUnitDivision.name != null) {

                                datanya2 = Group + " / " + Subgroup + " / " + Division + " -- " + response2.data.parentBusinessUnit.name;

                            } else if (response.data.businessUnitGroup.name != null && response2.data.businessUnitSubgroup.name != null) {
                                datanya2 = Group + " / " + Subgroup + " --  " + response2.data.parentBusinessUnit.name;
                            } else {
                                datanya2 = Group + " --  " + response2.data.parentBusinessUnit.name;
                            }


                            $('#parent_id').append("<option value=" + response2.data.parentBusinessUnit.id + ">" + datanya2 + "</option>");
                            $("#parent_id").val([response2.data.parentBusinessUnit.id]).trigger('change');
                        }

                        $('#parent_id').setcombobox({
                            data: { 'rows': 100 },
                            url: ajaxUrlBusinessUnitJobLevel,
                            searchparam: 'Search_job_title_name',
                            labelField: 'job_title_name',
                            valueField: 'parent_id',
                            searchField: 'job_title_name',
                            render: function (data) {
                                var datanya = '';
                                if (data.odata) {
                                    var Division = '';
                                    var Departement = '';
                                    var Subgroup = '';
                                    var Group = '';

                                    if (data.odata.businessUnitDivision.name != "") {
                                        Division = data.odata.businessUnitDivision.name;
                                    } else {
                                        Division = '';
                                    }

                                    if (data.odata.businessUnitDepartement.name != "") {
                                        Departement = data.odata.businessUnitDepartement.name;
                                    } else {
                                        Departement = '';
                                    }

                                    if (data.odata.businessUnitSubgroup.name != "") {
                                        Subgroup = data.odata.businessUnitSubgroup.name;
                                    } else {
                                        Subgroup = '';
                                    }

                                    if (data.odata.businessUnitGroup.name != "") {
                                        Group = data.odata.businessUnitGroup.name;
                                    } else {
                                        Group = '';
                                    }


                                    if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null && data.odata.businessUnitDepartement.name != null) {

                                        datanya = Group + " => " + Subgroup + " => " + Division + " => " + Departement + " -- (" + data.odata.job_title_name + ")";

                                    } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null) {

                                        datanya = Group + " => " + Subgroup + " => " + Division + " -- (" + data.odata.job_title_name + ")";

                                    } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null) {
                                        datanya = Group + " => " + Subgroup + " --  (" + data.odata.job_title_name + ")";
                                    } else {
                                        datanya = Group + " --  (" + data.odata.job_title_name + ")";
                                    }
                                    return datanya;
                                }
                            }
                        });

                    });
                } else {
                    DangerNotif("No Data!");
                    return false;
                }

            });

       


        });
    } else {
        DangerNotif("No Data");
        return false;
    }

});

function clearModal(formname) {
    $("#" + formname)[0].reset();
}


function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function DeleteAja(id, table) {
    $('table#' + table + ' tr#' + id + '').remove();
}

function AddRowNoData(colspan, table) {
    var markup = "<tr id='no_data'><td colspan='" + colspan + "'>No Data</td></td></tr>";
    $('table#' + table + ' tbody').append(markup);

}

var BasicInfofamilyData = [];
var BasicInfoEmergencyContact = [];
var BasicInfoEducationInfo = [];
var BasicInfoEmploymentHistory = [];
var BasicInfoCourseSeminar = [];

//-------------------------region family info--------------------------------------

function AddRowTableFamilyInfo(family_name, family_phone_no, family_birthday, family_other_no, family_education_level_id, family_place_birth, family_status_id, family_status_name, religion_id, gender, address, institution_id, description, married_status_id) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + family_name + "</td><td>" + family_phone_no + "</td><td style='display: none;'>" + family_birthday + "</td><td style='display: none;'>" + family_other_no + "</td><td style='display: none;'>" + family_education_level_id + "</td><td style='display: none;'>" + family_place_birth + "</td><td style='display: none;'>" + family_status_id + "</td><td>" + family_status_name + "</td><td style='display: none;'>" + religion_id + "</td><td>" + gender + "</td><td>" + address + "</td><td style='display: none;'>" + institution_id + "</td><td style='display: none;'>" + description + "</td><td style='display: none;'>" + married_status_id + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteFamilyInfo(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#FamilyList tbody").append(markup);
}

function DeleteFamilyInfo(id) {
    getDataFamilyInfo();
    if (BasicInfofamilyData.length === 1) {
        AddRowNoData('6', 'FamilyList');
    } else {
        DeleteAja(id, 'FamilyList');
        getDataFamilyInfo();
        var ttl = 0;
        $('#tmpfamilyinfo').empty();
        var tmpfamilyinfono = 0;
        $.each(BasicInfofamilyData, function (index, value) {
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][name]" value="' + value.name + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][phone_no]" value="' + value.phone_no + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][birthday]" value="' + value.birthday + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][other_no]" value="' + value.other_no + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_education_level_id]" value="' + value.family_education_level_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][place_birth]" value="' + value.place_birth + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_status_id]" value="' + value.family_status_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_status_name]" value="' + value.family_status_name + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][religion_id]" value="' + value.religion_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][gender]" value="' + value.gender + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][address]" value="' + value.address + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][institution_id]" value="' + value.institution_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][description]" value="' + value.description + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][married_status_id]" value="' + value.married_status_id + '"  >');
            tmpfamilyinfono++;
        });
    }
    $('table#FamilyList tr#' + id + '').remove();
}

function getDataFamilyInfo() {
    var TableData = '';
    var tmpData = '';
    BasicInfofamilyData = [];
    var rowCount = $('#FamilyList >tbody >tr').length;
    // alert(rowCount);
    $('#FamilyList tr').each(function (row, tr) {
        //console.log(row);
        if ($(tr).find('td:eq(1)').text() !== "") {
            tmpData = {
                "name": $(tr).find('td:eq(0)').text(),
                "phone_no": $(tr).find('td:eq(1)').text(),
                "birthday": $(tr).find('td:eq(2)').text(),
                "other_no": $(tr).find('td:eq(3)').text(),
                "family_education_level_id": $(tr).find('td:eq(4)').text(),
                "place_birth": $(tr).find('td:eq(5)').text(),
                "family_status_id": $(tr).find('td:eq(6)').text(),
                "family_status_name": $(tr).find('td:eq(7)').text(),
                "religion_id": $(tr).find('td:eq(8)').text(),
                "gender": $(tr).find('td:eq(9)').text(),
                "address": $(tr).find('td:eq(10)').text(),
                "institution_id": $(tr).find('td:eq(11)').text(),
                "description": $(tr).find('td:eq(12)').text(),
                "married_status_id": $(tr).find('td:eq(13)').text()
            };
            BasicInfofamilyData.push(tmpData);
        }
    });
    //console.log(BasicInfofamilyData);
    $('#tmpfamilyinfo').val(JSON.stringify(BasicInfofamilyData));
}

function CheckAndAddRowFamilyInfo() {

    var family_name = $('#family_name').val();
    var family_phone_no = $('#family_phone_no').val();
    var family_birthday = $('#family_birthday').val();
    var family_other_no = $('#family_other_no').val();
    var family_education_level_id = $('#family_education_level_id').val();
    var family_place_birth = $('#family_place_birth').val();
    var family_status_id = $('#family_status_id').val();
    var family_status_name = $('#family_status_id').find('option:selected').text();
    var religion_id = $('#family_religion_id').val();
    var gender = $("input:checked").val();
    var address = $('#family_address').val();
    var institution_id = $('#family_institution_id').val();
    var description = $('#family_description').val();
    var married_status_id = $('#family_married_status_id').val();

    if ($('#family_name').val() === "") {
        alert('Please Enter Family Name');
        return false;
    } else if ($('#family_status_id').val() === "") {
        alert('Please Enter Family Status');
        return false;
    } else if ($('#family_address').val() === "") {
        alert('Please Enter Family Address');
        return false;
    } else {
        DeleteAja('no_data', 'FamilyList');
        getDataFamilyInfo();
        tmpData = {
            "name": family_name,
            "phone_no": family_phone_no,
            "birthday": family_birthday,
            "other_no": family_other_no,
            "family_education_level_id": family_education_level_id,
            "place_birth": family_place_birth,
            "family_status_id": family_status_id,
            "family_status_name": family_status_name,
            "religion_id": religion_id,
            "gender": gender,
            "address": address,
            "institution_id": institution_id,
            "description": description,
            "married_status_id": married_status_id
        };
        BasicInfofamilyData.push(tmpData);
        $('#tmpfamilyinfo').empty();
        var tmpfamilyinfono = 0;
        $.each(BasicInfofamilyData, function (index, value) {

            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][name]" value="' + value.name + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][phone_no]" value="' + value.phone_no + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][birthday]" value="' + value.birthday + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][other_no]" value="' + value.other_no + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_education_level_id]" value="' + value.family_education_level_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][place_birth]" value="' + value.place_birth + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_status_id]" value="' + value.family_status_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][family_status_name]" value="' + value.family_status_name + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][religion_id]" value="' + value.religion_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][gender]" value="' + value.gender + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][address]" value="' + value.address + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][institution_id]" value="' + value.institution_id + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][description]" value="' + value.description + '"  >');
            $('#tmpfamilyinfo').append('<input type="hidden" name="dataListEmployeBasicFamily[' + tmpfamilyinfono + '][married_status_id]" value="' + value.married_status_id + '"  >');
            tmpfamilyinfono++;
        });

        AddRowTableFamilyInfo(family_name, family_phone_no, family_birthday, family_other_no, family_education_level_id, family_place_birth, family_status_id, family_status_name, religion_id, gender, address, institution_id, description, married_status_id);

        $('#EmployeeBasicfamilyModal').modal('hide');

    }
}
//------------------------- end region family info--------------------------------------




//-------------------------region emergency contact--------------------------------------

function AddRowTableEmergencyContact(family_status_id, relation_name, phone_no, address, full_name, other_no) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td style='display: none;'>" + family_status_id + "</td><td>" + relation_name + "</td><td>" + phone_no + "</td><td>" + address + "</td><td>" + full_name + "</td><td>" + other_no + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteFamilyEmergencyContact(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#EmergencyContactList tbody").append(markup);
}

function DeleteFamilyEmergencyContact(id) {
    getDataFamilyEmergencyInfo();
    if (BasicInfoEmergencyContact.length === 1) {
        AddRowNoData('6', 'EmergencyContactList');
    } else {
        DeleteAja(id, 'EmergencyContactList');
        getDataFamilyEmergencyInfo();
        $('#tmpemergencycontact').empty();
        var tmpemergencycontactno = 0;
        $.each(BasicInfoEmergencyContact, function (index, value) {
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][family_status_id]" value="' + value.family_status_id + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][relation_name]" value="' + value.relation_name + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][phone_no]" value="' + value.phone_no + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][address]" value="' + value.address + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][full_name]" value="' + value.full_name + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][other_no]" value="' + value.other_no + '"  >');
            tmpemergencycontactno++;
        });
    }
    $('table#EmergencyContactList tr#' + id + '').remove();
}

function getDataFamilyEmergencyInfo() {
    var tmpData = '';
    BasicInfoEmergencyContact = [];
    var rowCount = $('#EmergencyContactList >tbody >tr').length;
    // alert(rowCount);
    $('#EmergencyContactList tr').each(function (row, tr) {
        //console.log(row);
        if ($(tr).find('td:eq(0)').text() !== "") {
            tmpData = {
                "family_status_id": $(tr).find('td:eq(0)').text(),
                "relation_name": $(tr).find('td:eq(1)').text(),
                "phone_no": $(tr).find('td:eq(2)').text(),
                "address": $(tr).find('td:eq(3)').text(),
                "full_name": $(tr).find('td:eq(4)').text(),
            };
            BasicInfoEmergencyContact.push(tmpData);
        }
    });
    //console.log(BasicInfoEmergencyContact);
    $('#tmpemergencycontact').val(JSON.stringify(BasicInfoEmergencyContact));
}

function CheckAndAddRowEmergencyContact() {

    var emergency_family_status_id = $('#emergency_family_status_id').val();
    var relation_name = $('#emergency_family_status_id').find('option:selected').text();
    var emergency_phone_no = $('#emergency_phone_no').val();
    var emergency_address = $('#emergency_address').val();
    var emergency_name = $('#emergency_name').val();
    var emergency_other_no = $('#emergency_other_no').val();

    if ($('#emergency_phone_no').val() === "") {
        alert('Please Enter Emergency Phone');
        return false;
    } else if ($('#emergency_address').val() === "") {
        alert('Please Enter Emergency Address');
        return false;
    } else {
        DeleteAja('no_data', 'EmergencyContactList');
        getDataFamilyEmergencyInfo();
        tmpData = {
            "family_status_id": emergency_family_status_id,
            "relation_name": relation_name,
            "phone_no": emergency_phone_no,
            "address": emergency_address,
            "full_name": emergency_name,
            "other_no": emergency_other_no
        };
        BasicInfoEmergencyContact.push(tmpData);
        $('#tmpemergencycontact').empty();
        var tmpemergencycontactno = 0;
        $.each(BasicInfoEmergencyContact, function (index, value) {
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][family_status_id]" value="' + value.family_status_id + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][relation_name]" value="' + value.relation_name + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][phone_no]" value="' + value.phone_no + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][address]" value="' + value.address + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][full_name]" value="' + value.full_name + '"  >');
            $('#tmpemergencycontact').append('<input type="hidden" name="dataListEmployeBasicEmegencyContact[' + tmpemergencycontactno + '][other_no]" value="' + value.other_no + '"  >');
            tmpemergencycontactno++;
        });

        AddRowTableEmergencyContact(emergency_family_status_id, relation_name, emergency_phone_no, emergency_address, emergency_name, emergency_other_no);

        $('#EmployeeBasicEmergencyContactModal').modal('hide');

    }
}
//------------------------- end region emergency contact--------------------------------------



//-------------------------region Education info--------------------------------------

function AddRowTableEducationInfo(education_level_id, education_level_name, education_institusion_id, education_institusion_name, start_education, end_education, education_major, qualification, education_location) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td style='display: none;'>" + education_level_id + "</td><td>" + education_level_name + "</td><td style='display: none;'>" + education_institusion_id + "</td><td>" + education_institusion_name + "</td><td style='display: none;'>" + start_education + "</td><td style='display: none;'>" + end_education + "</td><td>" + education_major + "</td><td style='display: none;'>" + qualification + "</td><td>" + education_location + "</td><td>" + start_education + " - " + end_education + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteEducationInfo(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#EducationList tbody").append(markup);
}

function DeleteEducationInfo(id) {
    getDataEducationInfo();
    if (BasicInfoEducationInfo.length === 1) {
        AddRowNoData('6', 'EducationList');
    } else {
        DeleteAja(id, 'EducationList');
        getDataEducationInfo();
        $('#tmpeducationinfo').empty();
        var tmpeducationinfono = 0;
        $.each(BasicInfoEducationInfo, function (index, value) {
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][education_level_id]" value="' + value.education_level_id + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][education_level_name]" value="' + value.education_level_name + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][institution_id]" value="' + value.institution_id + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][institution_name]" value="' + value.institution_name + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][start_education]" value="' + value.start_education + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][end_education]" value="' + value.end_education + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][majors]" value="' + value.majors + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][qualification]" value="' + value.qualification + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][address]" value="' + value.address + '"  >');
            tmpeducationinfono++;
        });
    }
    $('table#EducationList tr#' + id + '').remove();
}

function getDataEducationInfo() {
    var tmpData = '';
    BasicInfoEducationInfo = [];
    var rowCount = $('#EducationList >tbody >tr').length;
    // alert(rowCount);
    $('#EducationList tr').each(function (row, tr) {
        //console.log(row);
        if ($(tr).find('td:eq(0)').text() !== "") {
            tmpData = {
                "education_level_id": $(tr).find('td:eq(0)').text(),
                "education_level_name": $(tr).find('td:eq(1)').text(),
                "institution_id": $(tr).find('td:eq(2)').text(),
                "institution_name": $(tr).find('td:eq(3)').text(),
                "start_education": $(tr).find('td:eq(4)').text(),
                "end_education": $(tr).find('td:eq(5)').text(),
                "majors": $(tr).find('td:eq(6)').text(),
                "qualification": $(tr).find('td:eq(7)').text(),
                "address": $(tr).find('td:eq(8)').text()
            };
            BasicInfoEducationInfo.push(tmpData);
        }
    });
    //console.log(BasicInfoEducationInfo);
    $('#tmpeducationinfo').val(JSON.stringify(BasicInfoEducationInfo));
}

function CheckAndAddRowEducationInfo() {

    var education_level_id = $('#education_level_id').val();
    var education_level_name = $('#education_level_id').find('option:selected').text();
    var start_education = $('#start_education').val();
    var end_education = $('#end_education').val();
    var qualification = $('#qualification').val();
    var education_major = $('#education_major').val();
    var education_institusion_id = $('#education_institusion_id').val();
    var education_institusion_name = $('#education_institusion_id').find('option:selected').text();
    var education_location = $('#education_location').val();

    if ($('#education_major').val() === "") {
    	alert('Please Enter Education Major');
    	return false;
    } else if (education_level_id === "") {
    	alert('Please Enter Last Education');
    	return false;
    } else if (start_education === "") {
    	alert('Please Enter Start Year Education');
    	return false;
    } else if (end_education === "") {
    	alert('Please Enter End Year Education');
    	return false;
    } else if (education_institusion_name === "") {
    	alert('Please Enter Institision Name');
    	return false;
    } else if (education_location === "") {
    	alert('Please Enter Education Location (Address)');
    	return false;
    } else {
        DeleteAja('no_data', 'EducationList');
        getDataEducationInfo();
        tmpData = {
            "education_level_id": education_level_id,
            "education_level_name": education_level_name,
            "institution_id": education_institusion_id,
            "institution_name": education_institusion_name,
            "start_education": start_education,
            "end_education": end_education,
            "majors": education_major,
            "qualification": qualification,
            "address": education_location
        };
        BasicInfoEducationInfo.push(tmpData);
        $('#tmpeducationinfo').empty();
        var tmpeducationinfono = 0;
        $.each(BasicInfoEducationInfo, function (index, value) {
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][education_level_id]" value="' + value.education_level_id + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][education_level_name]" value="' + value.education_level_name + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][institution_id]" value="' + value.institution_id + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][institution_name]" value="' + value.institution_name + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][start_education]" value="' + value.start_education + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][end_education]" value="' + value.end_education + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][majors]" value="' + value.majors + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][qualification]" value="' + value.qualification + '"  >');
            $('#tmpeducationinfo').append('<input type="hidden" name="dataListEmployeBasicEducationInfo[' + tmpeducationinfono + '][address]" value="' + value.address + '"  >');
            tmpeducationinfono++;
        });

        AddRowTableEducationInfo(education_level_id, education_level_name, education_institusion_id, education_institusion_name, start_education, end_education, education_major, qualification, education_location);

        $('#EmployeeBasicEducationInfoModal').modal('hide');

    }
}
//------------------------- end region education info-------



//-------------------------employment history--------------------------------------

function AddRowTableEmploymentHistory(company_name, history_phone, history_remark, history_address, history_start, history_end, history_position) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + company_name + "</td><td>" + history_phone + "</td><td>" + history_start + "-" + history_end + "</td><td>" + history_address + "</td><td>" + history_position + "</td><td style='display: none;'>" + history_remark + "</td><td style='display: none;'>" + history_start + "</td><td style='display: none;'>" + history_end + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteEmploymentHistory(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#EmployementHistoryList tbody").append(markup);
}

function DeleteEmploymentHistory(id) {
    getDataEmploymentHistory();
    if (BasicInfoEmploymentHistory.length === 1) {
        AddRowNoData('6', 'EmployementHistoryList');
    } else {
        DeleteAja(id, 'EmployementHistoryList');
        getDataEmploymentHistory();
        $('#tmpemploymenthistory').empty();
        var tmpbasicinfoemploymenthistoryno = 0;
        $.each(BasicInfoEmploymentHistory, function (index, value) {
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][company_name]" value="' + value.company_name + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][phone]" value="' + value.phone + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][remark]" value="' + value.remark + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][address]" value="' + value.address + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][start_date]" value="' + value.start_date + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][end_date]" value="' + value.end_date + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][position]" value="' + value.position + '"  >');
            tmpbasicinfoemploymenthistoryno++;
        });
    }
    $('table#EmployementHistoryList tr#' + id + '').remove();
}

function getDataEmploymentHistory() {
    var tmpData = '';
    BasicInfoEmploymentHistory = [];
    var rowCount = $('#EmployementHistoryList >tbody >tr').length;
    // alert(rowCount);
    $('#EmployementHistoryList tr').each(function (row, tr) {
        //console.log(row);
        if ($(tr).find('td:eq(0)').text() !== "") {
            tmpData = {
                "company_name": $(tr).find('td:eq(0)').text(),
                "phone": $(tr).find('td:eq(1)').text(),
                "remark": $(tr).find('td:eq(5)').text(),
                "address": $(tr).find('td:eq(3)').text(),
                "start_date": $(tr).find('td:eq(6)').text(),
                "end_date": $(tr).find('td:eq(7)').text(),
                "position": $(tr).find('td:eq(4)').text()
            };
            BasicInfoEmploymentHistory.push(tmpData);
        }
    });
    if (BasicInfoEmploymentHistory.length === 1) {
    	$('#tmpemploymenthistory').val("");
    } else {
    	$('#tmpemploymenthistory').val(JSON.stringify(BasicInfoEmploymentHistory));
    }
    //console.log(BasicInfoEducationInfo);
    
}

function CheckAndAddRowEmploymentHistory() {

    var company_name = $('#company_name').val();
    var history_start = $('#history_start').val();
    var history_end = $('#history_end').val();
    var history_remark = $('#history_remark').val();
    var history_phone = $('#history_phone').val();
    var history_position = $('#history_position').val();
    var history_address = $('#history_address').val();

    if ($('#company_name').val() === "") {
    	alert('Please Enter history of employement Company Name');
    	return false;
    } else if (history_start === "") {
    	alert('Please Enter Start Year');
    	return false;
    } else if (history_end === "") {
    	alert('Please Enter End Year');
    	return false;
    } else if (history_position === "") {
    	alert('Please Enter history of employement position');
    	return false;
    } else if (history_address === "") {
    	alert('Please Enter history of employement address');
    	return false;
    } else {
        DeleteAja('no_data', 'EmployementHistoryList');
        getDataEmploymentHistory();
        tmpData = {
            "company_name": company_name,
            "phone": history_phone,
            "remark": history_remark,
            "address": history_address,
            "start_date": history_start,
            "end_date": history_end,
            "position": history_position
        };
        BasicInfoEmploymentHistory.push(tmpData);
        $('#tmpemploymenthistory').empty();
        var tmpbasicinfoemploymenthistoryno = 0;
        $.each(BasicInfoEmploymentHistory, function (index, value) {
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][company_name]" value="' + value.company_name + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][phone]" value="' + value.phone + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][remark]" value="' + value.remark + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][address]" value="' + value.address + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][start_date]" value="' + value.start_date + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][end_date]" value="' + value.end_date + '"  >');
            $('#tmpemploymenthistory').append('<input type="hidden" name="dataListEmployeBasicEmploymentHistory[' + tmpbasicinfoemploymenthistoryno + '][position]" value="' + value.position + '"  >');
            tmpbasicinfoemploymenthistoryno++;
        });

        AddRowTableEmploymentHistory(company_name, history_phone, history_remark, history_address, history_start, history_end, history_position);

        $('#EmployeeBasicEmploymentHistoryModal').modal('hide');

    }
}
//------------------------- end employment history----------------



//-------------------------course seminar--------------------------------------

function AddRowTableCourseSeminar(course_name, course_institute_id, course_institute_name, course_address, start_training, end_training, no_certification, expired_certification, filemaster_id, course_description) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + course_name + "</td><td style='display: none;'>" + course_institute_id + "</td><td>" + course_institute_name + "</td><td>" + start_training + "-" + end_training + "</td><td>" + course_address + "</td><td>" + no_certification + "</td><td style='display: none;'>" + expired_certification + "</td><td style='display: none;'>" + filemaster_id + "</td><td style='display: none;'>" + start_training + "</td><td style='display: none;'>" + end_training + "</td><td style='display: none;'>" + course_description + "</td><td><center><a href='#' class='btn btn-danger' onClick='DeleteCourseSeminar(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#CourseList tbody").append(markup);
}

function DeleteCourseSeminar(id) {
    getDataCourseSeminar();
    if (getDataCourseSeminar.length === 1) {
        AddRowNoData('5', 'CourseList');
    } else {
        DeleteAja(id, 'CourseList');
        getDataCourseSeminar();
        $('#tmpemploymenthistory').empty();
        var tmpbasicinfocourseseminarno = 0;
        $.each(BasicInfoCourseSeminar, function (index, value) {
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][name_training]" value="' + value.name_training + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][institution_id]" value="' + value.institution_id + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][address]" value="' + value.address + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][start_training]" value="' + value.start_training + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][end_training]" value="' + value.end_training + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][no_certification]" value="' + value.no_certification + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][expired_certificate]" value="' + value.expired_certificate + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][filemaster_id]" value="' + value.filemaster_id + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][description]" value="' + value.description + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][institution_name]" value="' + value.institution_name + '"  >');

            tmpbasicinfocourseseminarno++;
        });
    }
    $('table#CourseList tr#' + id + '').remove();
}

function getDataCourseSeminar() {
    var tmpData = '';
    BasicInfoCourseSeminar = [];
    var rowCount = $('#CourseList >tbody >tr').length;
    // alert(rowCount);
    $('#CourseList tr').each(function (row, tr) {
        //console.log(row);
        if ($(tr).find('td:eq(0)').text() !== "") {
            tmpData = {
                "name_training": $(tr).find('td:eq(0)').text(),
                "institution_id": $(tr).find('td:eq(1)').text(),
                "address": $(tr).find('td:eq(4)').text(),
                "start_training": $(tr).find('td:eq(8)').text(),
                "end_training": $(tr).find('td:eq(9)').text(),
                "no_certification": $(tr).find('td:eq(5)').text(),
                "expired_certificate": $(tr).find('td:eq(6)').text(),
                "filemaster_id": $(tr).find('td:eq(7)').text(),
                "description": $(tr).find('td:eq(10)').text(),
                "institution_name": c$(tr).find('td:eq(2)').text()
            };
            BasicInfoCourseSeminar.push(tmpData);
        }
    });
    //console.log(BasicInfoEducationInfo);
    $('#tmpcourseseminar').val(JSON.stringify(BasicInfoCourseSeminar));
}

function CheckAndAddRowCourseSeminar() {

    var course_name = $('#course_name').val();
    var course_address = $('#course_address').val();
    var no_certification = $('#no_certification').val();
    var expired_certification = $('#expired_certification').val();
    var filemaster_id = $('#filemaster_id').val();
    var course_institute_id = $('#course_institute_id').val();
    var course_institute_name = $('#course_institute_id').find('option:selected').text();
    var start_training = $('#start_training').val();
    var end_training = $('#end_training').val();
    var course_description = $('#course_description').val();

    if (course_name === "") {
    	alert('Please Enter Course Name');
    	return false;
    } else if (course_address === "") {
    	alert('Please Enter Course Address');
    	return false;
    } else if (start_training === "") {
    	alert('Please Enter Start Training');
    	return false;
    } else if (end_training === "") {
    	alert('Please Enter End Training');
    	return false;
    } else if (course_institute_id === "") {
    	alert('Please Choose Institution name');
    	return false;
    } else if (course_description === "") {
    	alert('Please Enter Course Description');
    	return false;
    } else if (no_certification === "") {
    	alert('Please Enter Cerifitaion Course No');
    	return false;
    } else {
        DeleteAja('no_data', 'CourseList');
        getDataCourseSeminar();
        tmpData = {
            "name_training": course_name,
            "institution_id": course_institute_id,
            "address": course_address,
            "start_training": start_training,
            "end_training": end_training,
            "no_certification": no_certification,
            "expired_certificate": expired_certification,
            "filemaster_id": filemaster_id,
            "description": course_description,
            "institution_name": course_institute_name
        };
        BasicInfoCourseSeminar.push(tmpData);
        $('#tmpcourseseminar').empty();
        var tmpbasicinfocourseseminarno = 0;
        $.each(BasicInfoCourseSeminar, function (index, value) {
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][name_training]" value="' + value.name_training + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][institution_id]" value="' + value.institution_id + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][address]" value="' + value.address + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][start_training]" value="' + value.start_training + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][end_training]" value="' + value.end_training + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][no_certification]" value="' + value.no_certification + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][expired_certificate]" value="' + value.expired_certificate + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][filemaster_id]" value="' + value.filemaster_id + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][description]" value="' + value.description + '"  >');
            $('#tmpcourseseminar').append('<input type="hidden" name="dataListEmployeBasicCourseSeminar[' + tmpbasicinfocourseseminarno + '][institution_name]" value="' + value.institution_name + '"  >');
            tmpbasicinfocourseseminarno++;
        });

        AddRowTableCourseSeminar(course_name, course_institute_id, course_institute_name, course_address, start_training, end_training, no_certification, expired_certification, filemaster_id, course_description);

        $('#EmployeeBasicCourseSeminarModal').modal('hide');

    }
}
//-------------------------course seminar----------------



