var ajaxUrl = API + "Hris/CandidateRecruitment";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = API + "Hris/EmployeeBasicInfo/CreateEmployeeFromCandidate";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGetVacancy = API + "Hris/JobVacancy/GetWithDetial";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrllocationGet = API + "Hris/JobVacancy/GetAllLocation";
var ajaxUrllistCategory = API + "Hris/EmployeeChecklistCategory/List";
var ajaxUrlGetCategory = API + "Hris/EmployeeChecklistCategory/Get";
var ajaxUrlBusinessUnitJobLevel = API + "Hris/BussinesUnitJobLevel/List";
var ajaxUrlGetBusinessUnitJobLevel = API + "Hris/BussinesUnitJobLevel/GetDetail";
var ajaxUrlLegalEntity = API + "General/LegalEntity/List";
var ajaxUrlReligion = API + "Hris/Religion/List";
var ajaxUrlNationality = API + "Hris/Nationality/List";
var ajaxUrlCandidateRecruitmentEmploymentHistory = API + "Hris/CandidateRecruitmentEmploymentHistory/GetListWithIDCandidate";
var ajaxUrlCandidateRecruitmentEducationInfo = API + "Hris/CandidateRecruitmentEducationInfo/GetListWithIDCandidate";
var ajaxUrlEducationLevel = API + "Hris/EducationLevel/List";
var tableList = "ApplicantProgressList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var pageheader = "";
var NewEmployeeChecklist = [];

fnApplicantProgress = {
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
    $('#last_education').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlEducationLevel,
        searchparam: 'Search_education_name',
        labelField: 'education_name',
        valueField: 'id',
        searchField: 'education_name'
    });

    $('#religion_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlReligion,
        searchparam: 'Search_religion_name',
        labelField: 'religion_name',
        valueField: 'id',
        searchField: 'religion_name'
    });

    $('#nationality_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlNationality,
        searchparam: 'Search_nationality_name',
        labelField: 'nationality_name',
        valueField: 'id',
        searchField: 'nationality_name'
    });

    initlanguage(lang_hr);
    var formCreate = "NewEmployeeFormNewApplicant";
    var optionsForm = GetOptionsForm(function () {
        startprocess();
        return $("#" + formCreate).valid();
    }, function (response, statusText, xhr, $form) {
            endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            //window.location.href = WEB + "/Hris/employeebasicinfo";
            window.location.href = "/Hrisnew/employeebasic";
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(formCreate, optionsForm);
    setFormAction(formCreate, ajaxUrlCreate);

    $('#candicate_recruitment_name').setcombobox({
        data: { 'rows': 1000, Search_applicant_progress_id: 4, Search_applicant_move_stage_id: 7 },
        url: ajaxUrlList,
        searchparam: 'Search_title_name',
        labelField: 'first_name',
        valueField: 'job_vacancy_id',
        searchField: 'first_name',
        render: function (data) {
            var datanya = '';
            if (data.odata) {
                var dataraw = '';
                //console.log(data.odata);
                dataraw = data.odata.code + " - " + data.odata.first_name + " " + data.odata.last_name;
                return dataraw;
            }
        }
    });

    $('#checklist_category').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrllistCategory,
        searchparam: 'Search_checklist_category',
        labelField: 'checklist_category',
        valueField: 'id',
        searchField: 'checklist_category',
        maxItems:0
    });

    $('#upload_sign_contract').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});

    $('#candicate_recruitment_name').on('select2:select', function (e) {
        //console.log(e.params.data.odata);
        var id = e.params.data.odata.id;
        $('#candicate_recruitment_id').val(id);
        $('#id').val(id);
        //var code_data = e.params.data.odata.code;
        //var applicant_progress_id = e.params.data.odata.applicant_progress_id;
        //var applicant_move_stage_id = e.params.data.odata.applicant_move_stage_id;

        if (id !== null) {
            ajaxGet(ajaxUrlGetCandidateReq + "/" + id, {}, function (response2) {
                response2 = parseJson(response2);
                $('#apply_date').val(moment(response2.data.apply_date).format("YYYY-MM-DD"));
                $('#interview_date').val(moment(response2.data.interview_date).format("YYYY-MM-DD"));
                $('#contract_number').val(response2.data.contract_number);
                $('#contract_date').val(moment(response2.data.contract_date).format("YYYY-MM-DD"));
                $('#start_date').val(moment(response2.data.contract_date).format("YYYY-MM-DD"));

                if (response2.data.contractSignDate === null || response2.data.contractSignDate === "") {
                    $('#contractSignDate').val('');
                } else {
                    $('#contractSignDate').val(moment(response2.data.contractSignDate).format("YYYY-MM-DD"));
                }
               
                $('#first_name').val(response2.data.first_name);
                $('#last_name').val(response2.data.last_name);
                $('#nick_name').val(response2.data.nick_name);
                $('#code').val(response2.data.code);
                $('#applicant_progress_id').val(response2.data.applicant_progress_id);
                var $radiosgender = $('input:radio[name=gender]');
                var cekgender = response2.data.gender;
                if (cekgender === "male") {
                    $radiosgender.filter('[value=male]').prop('checked', true);
                } else {
                    $radiosgender.filter('[value=female]').prop('checked', true);
                }
                $('#birth_place').val(response2.data.birth_place);
                $('#address').val(response2.data.address);
                $('#email').val(response2.data.email);
                $('#comment').val(response2.data.comment);
                var dateBirth = moment(response2.data.date_birth).format("YYYY-MM-DD");
                //var dateBirth = response2.data.date_birth;
                $('#date_birth').val(dateBirth);
                //$('#last_education').val(response2.data.education_level_id);
                if (response2.data.last_education) {
                        ajaxGet(ajaxUrlEducationLevel, { Search_education_id: response2.data.last_education }, function (resp) {
                            resp = parseJson(resp);
                            $('#last_education').append('<option value="' + response2.data.last_education + '" selected>' + resp.rows[0].education_name + '</option>');
                        });

                    }
                $('#year_experience').val(response2.data.year_experience);
                $('#phone_number').val(response2.data.phone_number);
                $('#source').val(response2.data.source);
                if (response2.data.source === "") {
                    $('#source').val(Others);
                }
               
                var $radioscigarete = $('input:radio[name=cigaret_willing]');
                var cekcigarete = response2.data.cigaret_willing;
                if (cekcigarete === true) {
                    $radioscigarete.filter('[value=true]').prop('checked', true);
                } else {
                    $radiosgender.filter('[value=false]').prop('checked', true);
                }
                var $radiosalcohol = $('input:radio[name=alcohol_willing]');
                var cekalcohol = response2.data.alcohol_willing;
                if (cekalcohol === true) {
                    $radiosalcohol.filter('[value=true]').prop('checked', true);
                } else {
                    $radiosalcohol.filter('[value=false]').prop('checked', true);
                }

                var $radiosout = $('input:radio[name=out_of_town_willing]');
                var cekout = response2.data.out_of_town_willing;
                if (cekout === true) {
                    $radiosout.filter('[value=true]').prop('checked', true);
                } else {
                    $radiosout.filter('[value=false]').prop('checked', true);
                }

                if (response2.data.cv_file === "") {
                    $('#divcvplace .input-group').remove();
                    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                } else {
                    $('#divcvplace .input-group').remove();
                    $('#cv').val(response2.data.cv_file);
                    $('#cv').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                }

                if (response2.data.photo_file === "") {
                    $('#divphotoplace .input-group').remove();
                    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
                } else {
                    $('#divphotoplace .input-group').remove();
                    $('#photo').val(response2.data.photo_file);
                    $('#photo').setuploadbox({ fileType: ['png', 'jpg', 'jpeg']});
                }

                if (response2.data.portofolio_file === "") {
                    $('#divportfolioplace .input-group').remove();
                    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                } else {
                    $('#divportfolioplace .input-group').remove();
                    $('#portfolio').val(response2.data.portofolio_file);
                    $('#portfolio').setuploadbox({ fileType: ['png', 'jpg', 'jpeg', 'pdf', 'doc', 'docx']});
                }
                

                $('#location_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrllocationGet,
                    searchparam: 'Search_location_name',
                    labelField: 'location_name',
                    valueField: 'id',
                    searchField: 'location_name'
                });

                $('#religion_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlReligion,
                    searchparam: 'Search_religion_name',
                    labelField: 'religion_name',
                    valueField: 'id',
                    searchField: 'religion_name'
                });

                $('#nationality_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlNationality,
                    searchparam: 'Search_nationality_name',
                    labelField: 'nationality_name',
                    valueField: 'id',
                    searchField: 'nationality_name'
                });

                $('#legal_entity_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlLegalEntity,
                    searchparam: 'Search_legal_entity_name',
                    labelField: 'legal_entity_name',
                    valueField: 'id',
                    searchField: 'legal_entity_name'
                });


                if (response2.data.businessJobLevelId !== "") {
                    ajaxGet(ajaxUrlGetBusinessUnitJobLevel + "/" + response2.data.businessJobLevelId, {}, function (response4) {
                        response4 = parseJson(response4);
                        $('#business_unit_job_level_id').append("<option value=" + response2.data.businessJobLevelId + ">" + response4.data.job_title_name + "</option>");
                        ajaxGet(ajaxUrlGetBusinessUnitJobLevel + "/" + response4.data.parent_id, {}, function (response5) {
                            response5 = parseJson(response5);
                            var Division = '';
                            var Departement = '';
                            var Subgroup = '';
                            var Group = '';

                            if (response5.data.businessUnitDivision.name !== "") {
                                Division = response5.data.businessUnitDivision.name;
                            } else {
                                Division = '';
                            }

                            if (response5.data.businessUnitDepartement.name !== "") {
                                Departement = response5.data.businessUnitDepartement.name;
                            } else {
                                Departement = '';
                            }

                            if (response5.data.businessUnitSubgroup.name !== "") {
                                Subgroup = response5.data.businessUnitSubgroup.name;
                            } else {
                                Subgroup = '';
                            }

                            if (response5.data.businessUnitGroup.name !== "") {
                                Group = response5.data.businessUnitGroup.name;
                            } else {
                                Group = '';
                            }

                            if (response5.data.businessUnitGroup.name !== null && response5.data.businessUnitSubgroup.name !== null && response5.data.businessUnitDivision.name !== null && response5.data.businessUnitDepartement.name !== null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " => " + Departement + " -- (" + response5.data.job_title_name + ")";

                            } else if (response5.data.businessUnitGroup.name !== null && response5.data.businessUnitSubgroup.name !== null && response5.data.businessUnitDivision.name !== null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " -- (" + response5.data.job_title_name + ")";

                            } else if (response5.data.businessUnitGroup.name !== null && response5.data.businessUnitSubgroup.name !== null) {
                                datanya = Group + " => " + Subgroup + " --  (" + response5.data.job_title_name + ")";
                            } else {
                                datanya = Group + " --  (" + response5.data.job_title_name + ")";
                            }


                            $('#parent_name').val(datanya);
                            $('#business_unit_division_id').val(response5.data.businessUnitDivision.id);
                            $('#business_unit_division_name').val(response5.data.businessUnitDivision.name);
                            $('#business_unit_departement_id').val(response5.data.businessUnitDepartement.id);
                            $('#business_unit_departement_name').val(response5.data.businessUnitDepartement.name);
                        });
                    });
                }
                $('#business_unit_job_level_id').setcombobox({
                    data: { 'rows': 100 },
                    url: ajaxUrlBusinessUnitJobLevel,
                    searchparam: 'search_title_name',
                    labelField: 'job_title_name',
                    valueField: 'id',
                    render: function (data) {
                        var datanya = '';
                        if (data.odata) {
                            var Division = '';
                            var Departement = '';
                            var Subgroup = '';
                            var Group = '';

                            if (data.odata.businessUnitDivision.name !== "") {
                                Division = data.odata.businessUnitDivision.name;
                            } else {
                                Division = '';
                            }

                            if (data.odata.businessUnitDepartement.name !== "") {
                                Departement = data.odata.businessUnitDepartement.name;
                            } else {
                                Departement = '';
                            }

                            if (data.odata.businessUnitSubgroup.name !== "") {
                                Subgroup = data.odata.businessUnitSubgroup.name;
                            } else {
                                Subgroup = '';
                            }

                            if (data.odata.businessUnitGroup.name !== "") {
                                Group = data.odata.businessUnitGroup.name;
                            } else {
                                Group = '';
                            }


                            if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null && data.odata.businessUnitDivision.name !== null && data.odata.businessUnitDepartement.name !== null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " => " + Departement + " -- (" + data.odata.job_title_name + ")";

                            } else if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null && data.odata.businessUnitDivision.name !== null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " -- (" + data.odata.job_title_name + ")";

                            } else if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null) {
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
            DangerNotif("No Data Candidate");
            return false;
        }
    });

    $('#business_unit_job_level_id').on('select2:select', function (e) {
  
        var id2 = e.params.data.odata.parent_id;
        if (id2 !== null) {
            ajaxGet(ajaxUrlGetBusinessUnitJobLevel + "/" + id2, {}, function (response3) {
                response3 = parseJson(response3);

                            var Division = '';
                            var Departement = '';
                            var Subgroup = '';
                            var Group = '';

                            if (response3.data.businessUnitDivision.name !== "") {
                                Division = response3.data.businessUnitDivision.name;
                            } else {
                                Division = '';
                            }

                            if (response3.data.businessUnitDepartement.name !== "") {
                                Departement = response3.data.businessUnitDepartement.name;
                            } else {
                                Departement = '';
                            }

                            if (response3.data.businessUnitSubgroup.name !== "") {
                                Subgroup = response3.data.businessUnitSubgroup.name;
                            } else {
                                Subgroup = '';
                            }

                            if (response3.data.businessUnitGroup.name !== "") {
                                Group = response3.data.businessUnitGroup.name;
                            } else {
                                Group = '';
                            }

                            if (response3.data.businessUnitGroup.name !== null && response3.data.businessUnitSubgroup.name !== null && response3.data.businessUnitDivision.name !== null && response3.data.businessUnitDepartement.name !== null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " => " + Departement + " -- (" + response3.data.job_title_name + ")";

                            } else if (response3.data.businessUnitGroup.name !== null && response3.data.businessUnitSubgroup.name !== null && response3.data.businessUnitDivision.name !== null) {

                                datanya = Group + " => " + Subgroup + " => " + Division + " -- (" + response3.data.job_title_name + ")";

                            } else if (response3.data.businessUnitGroup.name !== null && response3.data.businessUnitSubgroup.name !== null) {
                                datanya = Group + " => " + Subgroup + " --  (" + response3.data.job_title_name + ")";
                            } else {
                                datanya = Group + " --  (" + response3.data.job_title_name + ")";
                            }
                           
                       
                $('#parent_id').val(id2);
                $('#parent_name').val(datanya);

            });
            $('#business_unit_division_id').val(e.params.data.odata.businessUnitDivision.id);
            $('#business_unit_division_name').val(e.params.data.odata.businessUnitDivision.name);
            $('#business_unit_departement_id').val(e.params.data.odata.businessUnitDepartement.id);
            $('#business_unit_departement_name').val(e.params.data.odata.businessUnitDepartement.name);
        }
       
    });
});

function DeleteLogic(id) {
    getData();
    if (NewEmployeeChecklist.length === 1) {
        AddRowNoData();
    } else {
        Deleteaja(id);
        getData();
        var ttl = 0;
        $('#tmpNewEmployeeChecklist').empty();
        var tmpmpno = 0;
        $.each(NewEmployeeChecklist, function (index, value) {
            $('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][checklist_category]" value="' + value.checklist_category + '"  >');
            $('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][checklistitem]" value="' + value.checklistitem + '"  >');
            $('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][description]" value="' + value.description + '"  >');
            //$('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][whentoalert]" value="' + value.whentoalert + '"  >');
              tmpmpno++;
        });

    }
    $('table#dt_employee_checklist tr#' + id + '').remove();
}

function Deleteaja(id) {
    $('table#dt_employee_checklist tr#' + id + '').remove();
}

function AddRowNoData() {
    var markup = "<tr id='no_data'><td colspan='5'>No Data</td></td></tr>";
    $("table#dt_employee_checklist tbody").append(markup);

}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function AddRowTable(checklistcategoryid,checklistcategoryname,checklistitem, description, whentoalert) {
    var acak = randomIntFromInterval(1, 9999);
    var markup = "<tr id=" + acak + "><td style='display: none;'>" + checklistcategoryid + "</td><td>" + checklistcategoryname + "</td><td>" + checklistitem + "</td><td>" + description + "</td>" + "<td><center><a href='#' class='btn btn-danger' onClick='DeleteLogic(" + acak + ")'>Delete</a></center></td></tr>";
    $("table#dt_employee_checklist tbody").append(markup);
}
function clearModal() {

    var formCreateDetail = "NewEmployeeChecklistModalFormDetail";
    var optionsForm = GetOptionsForm(function () {
        return $("#" + formCreateDetail).valid();
    });
    InitForm(formCreateDetail, optionsForm);
    $('#checklist_category').val(null).trigger('change');
}
function CheckAndAddRow(checklistcategoryid,checklistcategoryname,checklistitem, description) {
    var checklistName = "";
    $("#checklist_category option:selected").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text();
            checklistName += selText + ";";
        }
    });
    if ($('#checklist_item').val() === "") {
        DangerNotif('Please Enter Checklist item');
        return false;
    } else if ($('#desc').val() === "") {
        DangerNotif('Please Enter Description');
        return false;
    } else { 
            Deleteaja('no_data');
            getData();
            // if (RealData.length > 0) {
        tmpData = {
                "checklist_category": checklistcategoryid,
                "checklistitem": checklistitem,
            "description": description
            //,"whentoalert": whentoalert
            };
            NewEmployeeChecklist.push(tmpData);
            $('#tmpNewEmployeeChecklist').empty();
            var tmpmpno = 0;
            $.each(NewEmployeeChecklist, function (index, value) {
                $('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][checklist_category]" value="' + value.checklist_category + '"  >');
                $('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][checklistitem]" value="' + value.checklistitem + '"  >');
                $('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][description]" value="' + value.description + '"  >');
                //$('#tmpNewEmployeeChecklist').append('<input type="hidden" name="listDataNewEmployeeChecklist[' + tmpmpno + '][whentoalert]" value="' + value.whentoalert + '"  >');
                tmpmpno++;
            });

        AddRowTable(checklistcategoryid, checklistName,checklistitem, description);
            $('#NewEmployeeChecklistModalDetail').modal('hide');
      
    }
}

function getData() {
    var TableData = '';
    var tmpData = '';
    NewEmployeeChecklist = [];
    var rowCount = $('#dt_employee_checklist >tbody >tr').length;
    // alert(rowCount);
    $('#dt_employee_checklist tr').each(function (row, tr) {
        //console.log(row);
        if ($(tr).find('td:eq(0)').text() !== "") {
            tmpData = {
                "checklist_category": $(tr).find('td:eq(0)').text(),
                "checklistitem": $(tr).find('td:eq(2)').text(),
                "description": $(tr).find('td:eq(3)').text(),
                "whentoalert": $(tr).find('td:eq(4)').text()
            };
            NewEmployeeChecklist.push(tmpData);
        }
        //TableData = TableData
        //    + $(tr).find('td:eq(0)').text() + ' '  // jobtitle
        //    + $(tr).find('td:eq(1)').text() + ' '  // ttf
        //    + $(tr).find('td:eq(2)').text() + ' '  // status
        //    + $(tr).find('td:eq(3)').text() + ' '  // mppbudget
        //    + $(tr).find('td:eq(4)').text() + ' '  // grade
        //    + $(tr).find('td:eq(5)').text() + ' '  // project
        //    + $(tr).find('td:eq(7)').text() + ' '  // desc
        //    + $(tr).find('td:eq(8)').text() + ' '  // jobtitle_name
        //    + $(tr).find('td:eq(6)').text() + ' '  // Task
        //     + '\n';

    });
    //console.log(NewEmployeeChecklist);
    $('#dataDetailMpp').val(JSON.stringify(NewEmployeeChecklist));
}

 
$('#start_date').on('change', function () {
    var startDate = new Date($('#start_date').val());
    var contractDate = new Date($('#contract_date').val());
    if (startDate < contractDate) {
        DangerNotif("Start Date must be higher than contract date!!!");
        $('#start_date').val(contractDate);
    }
});


//$('#date_birth').on('change', function () {
//    $('#date_birth').val(moment($('#date_birth').val()).format("YYYY-MM-DD"));
//});