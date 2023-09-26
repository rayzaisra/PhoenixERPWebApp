var webUrl = WEB + "/Hris/EmployeeBasicInfo";
var ajaxUrl = API + "/Hris/EmployeeBasicInfo";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "EmployeeBasicInfoFormCreate";
var formCreate = "EmployeeBasicInfoFormCreate";
var formUpdate = "EmployeeBasicInfoFormUpdate";
var formSearch = "EmployeeBasicInfoFormSearch";

var emergencyUrl = API + "/Hris/EmployeeEmergencyContact";
var emergencyUrlList = emergencyUrl + "/List";
var emergencyUrlGet = emergencyUrl + "/Get";
var emergencyUrlUpdate = emergencyUrl + "/Update";
var emergencyUrlCreate = emergencyUrl + "/Create";
var emergencyUrlDelete = emergencyUrl + "/Delete";

var courseUrl = API + "/Hris/EmployeeTraining";
var courseUrlList = courseUrl + "/List";
var courseUrlGet = courseUrl + "/Get";
var courseUrlUpdate = courseUrl + "/Update";
var courseUrlCreate = courseUrl + "/Create";
var courseUrlDelete = courseUrl + "/Delete";

var educationUrl = API + "/Hris/Education";
var educationUrlList = educationUrl + "/List";
var educationUrlGet = educationUrl + "/Get";
var educationUrlUpdate = educationUrl + "/Update";
var educationUrlCreate = educationUrl + "/Create";
var educationUrlDelete = educationUrl + "/Delete";

var historyUrl = API + "/Hris/EmployeeExperience";
var historyUrlList = historyUrl + "/List";
var historyUrlGet = historyUrl + "/Get";
var historyUrlUpdate = historyUrl + "/Update";
var historyUrlCreate = historyUrl + "/Create";
var historyUrlDelete = historyUrl + "/Delete";

var familyUrl = API + "/Hris/Family";
var familyUrlList = familyUrl + "/List";
var familyUrlGet = familyUrl + "/Get";
var familyUrlUpdate = familyUrl + "/Update";
var familyUrlCreate = familyUrl + "/Create";
var familyUrlDelete = familyUrl + "/Delete";

var tableFamily = "FamilyList";
var columnsFamily = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "name",
        "title": "<span class='translate'  data-args='Name'>Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "family_status_name",
        "title": "<span class='translate'  data-args='Family Status'>Family Status</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "address",
        "title": "<span class='translate'  data-args='Address'>Address</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "phone_no",
        "title": "<span class='translate'  data-args='Phone No'>Phone No</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "gender",
        "title": "<span class='translate'  data-args='gender'>gender</span>",
        "sClass": "",
        orderable: true
    },
    {
        "data": "action",
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            return '<a href="javascript:void(0)" onClick="fnEmployee.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">View</i></a>';
        }
    }
];

var tableEmergency = "EmergencyContactList";
var columnsEmergency = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "family_status_name",
        "title": "<span class='translate'  data-args='Relation'>Relation</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "name",
        "title": "<span class='translate'  data-args='Full Name'>Full Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "phone_no",
        "title": "<span class='translate'  data-args='Phone No.'>Phone No.</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "other_no",
        "title": "<span class='translate'  data-args='Other No.'>Other No.</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "address",
        "title": "<span class='translate'  data-args='Address'>Address</span>",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            return '<a href="javascript:void(0)" onClick="fnEmployee.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">View</i></a>';
        }
    }
];

var tableHistory = "HistoryList";
var columnsHistory = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "company_name",
        "title": "<span class='translate'  data-args='Company Name'>Company Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "address",
        "title": "<span class='translate'  data-args='Address'>Address</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "company_phone",
        "title": "<span class='translate'  data-args='Company Phone'>Company Phone</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "year",
        "title": "<span class='translate'  data-args='Year'>Year</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "position",
        "title": "<span class='translate'  data-args='Position'>Position</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "remark",
        "title": "<span class='translate'  data-args='Remark'>Remark</span>",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            return '<a href="javascript:void(0)" onClick="fnEmployee.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">View</i></a>';
        }
    }
];
var tableCourse = "CourseList";
var columnsCourse = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "institution_name",
        "title": "<span class='translate'  data-args='Institution Name'>Institution Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": null,
        "title": "<span class='translate'  data-args='Year'>Year</span>",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return data.start_training + " - " + data.end_training;
        }
    }, {
        "data": "file_upload",
        "title": "<span class='translate'  data-args='Certificate (Scan)'>Certificate (Scan)</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "description",
        "title": "<span class='translate'  data-args='Remark'>Remark</span>",
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
            retval += '<a class="dropdown-item course" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            retval += '<a class="dropdown-item course" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;
            //return '<a href="javascript:void(0)" onClick="fnEmployee.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">View</i></a>';
        }
    }
];
var tableEducation = "EducationList";
var columnsEducation = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "institution_name",
        "title": "<span class='translate'  data-args='Institution Name'>Institution Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "expired_certificate",
        "title": "<span class='translate'  data-args='Year'>Year</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "file_upload",
        "title": "<span class='translate'  data-args='Certificate (Scan)'>Certificate (Scan)</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "description",
        "title": "<span class='translate'  data-args='Remark'>Remark</span>",
        "sClass": "",
        orderable: true
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {

            return '<a href="javascript:void(0)" onClick="fnEmployee.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">View</i></a>';
        }
    }
];

fnEmployeeBasicInfo = {
    EmployeeBasicInfoID: 0,
    ListFamz: new Array(),
    ListEdu: new Array(),
    ListCourse: new Array(),
    ListHistory: new Array(),
    ListEmergency: new Array(),
    addHistory: function () {
        // Disabled ordering
        let tableOptions = {
            "searching": false,
            "paging": false,
            "ordering": false,
            "info": false
        };
        TABLES[tableHistory].clear().destroy();
        $('#' + tableHistory + '').DataTable(tableOptions);

        let history = {};
        history["company_name"] = $("#company_name").val();
        history["phone"] = $("#history_phone").val();
        history["address"] = $("#history_address").val();
        history["remark"] = $("#history_remark").val();
        history["start_date"] = $("#history_start").val();
        history["end_date"] = $("#history_end").val();
        history["position"] = $("#history_position").val();

        fnEmployeeBasicInfo.ListHistory.push(history);

        // Remove No data record 
        $(".odd").remove();
        let no = $('#' + tableHistory + ' >tbody >tr').length;

        let newRow = "<tr id='emergency" + no + "'><td>" + Number(no + 1) + "</td><td>" + history.company_name + "</td><td>" + history.address + "</td><td>" + history.phone + "</td><td>" + history.start_date + ' - ' + history.end_date + "</td><td>" + history.position + "</td><td>" + history.remark + "</td><td><button onclick='fnEmployeeBasicInfo.deleteRow(this)'>Delete</button></td></tr>";
        $('#' + tableHistory + '').append(newRow);

        $('#historyModal').modal('hide');
        $('#historyModal').on('hidden.bs.modal', function (e) {
            $(this)
                .find("input,textarea,select")
                .val('')
                .end()
                .find("input[type=checkbox], input[type=radio]")
                .prop("checked", "")
                .end();
        });
    },
    addCourse: function () {
        // Disabled ordering
        let tableOptions = {
            "searching": false,
            "paging": false,
            "ordering": false,
            "info": false
        };
        TABLES[tableCourse].clear().destroy();
        $('#' + tableCourse + '').DataTable(tableOptions);

        let course = {};
        course["name_training"] = $("#course_name").val();
        course["institusion_id"] = $("#course_institute_id").val();
        course["address"] = $("#course_address").val();
        course["start_training"] = $("#start_training").val();
        course["end_training"] = $("#end_training").val();
        course["no_certification"] = $("#no_certification").val();
        course["expired_certificate"] = $("#expired_certification").val();
        course["description"] = $("#course_description").val();

        fnEmployeeBasicInfo.ListCourse.push(course);

        // Remove No data record 
        $(".odd").remove();
        let no = $('#' + tableCourse + ' >tbody >tr').length;

        let newRow = "<tr id='emergency" + no + "'><td>" + Number(no + 1) + "</td><td>" + $("#course_institute_id option:selected").text() + "</td><td>" + course["start_training"] + "</td><td>" + course["file_upload"] + "</td><td>" + course["description"] + "</td><td><button onclick='fnEmployeeBasicInfo.deleteRow(this)'>Delete</button></td></tr>";
        $('#' + tableCourse + '').append(newRow);

        $('#courseModal').modal('hide');
        $('#courseModal').on('hidden.bs.modal', function (e) {
            $(this)
                .find("input,textarea,select")
                .val('')
                .end()
                .find("input[type=checkbox], input[type=radio]")
                .prop("checked", "")
                .end();
        });
    },
    addEmergency: function () {
        // Disabled ordering
        let tableOptions = {
            "searching": false,
            "paging": false,
            "ordering": false,
            "info": false
        };
        TABLES[tableEmergency].clear().destroy();
        $('#' + tableEmergency + '').DataTable(tableOptions);

        let emergency = {};
        emergency["family_status_id"] = $("#emergency_family_status_id").val();
        emergency["full_name"] = $("#emergency_name").val();
        emergency["phone_no"] = $("#emergency_phone_no").val();
        emergency["other_no"] = $("#emergency_other_no").val();
        emergency["address"] = $("#emergency_address").val();

        fnEmployeeBasicInfo.ListEmergency.push(emergency);

        // Remove No data record 
        $(".odd").remove();
        let no = $('#' + tableEmergency + ' >tbody >tr').length;

        let newRow = "<tr id='" + emergency + no + "'><td>" + Number(no + 1) + "</td><td>" + $("#emergency_family_status_id option:selected").text() + "</td><td>" + emergency["full_name"] + "</td><td>" + emergency["other_no"] + "</td><td>" + emergency["phone_no"] + "</td><td>" + emergency["address"] + "</td><td><button onclick='fnEmployeeBasicInfo.deleteRow(this)'>Delete</button></td></tr>";
        $('#' + tableEmergency + '').append(newRow);

        $('#emergencyModal').modal('hide');
        $('#emergencyModal').on('hidden.bs.modal', function (e) {
            $(this)
                .find("input,textarea,select")
                .val('')
                .end()
                .find("input[type=checkbox], input[type=radio]")
                .prop("checked", "")
                .end();
        });
    },
    addEducation: function () {
        // Disabled ordering
        let tableOptions = {
            "searching": false,
            "paging": false,
            "ordering": false,
            "info": false
        };
        TABLES[tableEducation].clear().destroy();
        $('#' + tableEducation + '').DataTable(tableOptions);

        let edu = {};
        edu["education_level_id"] = $("#education_level_id").val();
        edu["institution_id"] = $("#education_institution_name").val();
        edu["majors"] = $("#education_major").val();
        edu["qualification"] = $("#qualification").val();
        edu["address"] = $("#education_location").val();
        edu["start_education"] = $("#start_education").val();
        edu["end_education"] = $("#end_education").val();

        fnEmployeeBasicInfo.ListEdu.push(edu);

        // Remove No data record 
        $(".odd").remove();
        let no = $('#' + tableEducation + ' >tbody >tr').length;

        let newRow = "<tr id='education" + no + "'><td>" + Number(no + 1) + "</td><td>" + edu.institution_id + "</td><td>" + edu.start_education + " - " + edu.end_education + "</td><td>" + null + "</td><td>" + edu.address + "</td><td><button onclick='fnEmployeeBasicInfo.deleteRow(this)'>Delete</button></td></tr>";
        $('#' + tableEducation + '').append(newRow);

        $('#educationModal').modal('hide');
        $('#educationModal').on('hidden.bs.modal', function (e) {
            $(this)
                .find("input,textarea,select")
                .val('')
                .end()
                .find("input[type=checkbox], input[type=radio]")
                .prop("checked", "")
                .end();
        });
    },
    addFamily: function () {
        // Disabled ordering
        let tableOptions = {
            "searching": false,
            "paging": false,
            "ordering": false,
            "info": false
        };

        TABLES[tableFamily].clear().destroy();
        $('#' + tableFamily + '').DataTable(tableOptions);

        let fam = {};
        fam["name"] = $("#family_name").val();
        fam["family_status_id"] = $("#family_status_id").val();
        fam["married_status_id"] = $("#family_married_status_id").val();
        fam["institution_id"] = $("#family_institution_id").val();
        fam["education_id"] = $("#family_education_id").val();
        fam["religion_id"] = $("#family_religion_id").val();
        fam["description"] = $("#family_description").val();
        fam["phone_no"] = $("#family_phone_no").val();
        fam["other_no"] = $("#family_other_no").val();
        fam["birthday"] = $("#family_birthday").val();
        fam["address"] = $("#family_address").val();
        fam["gender"] = document.querySelector('input[name="family_gender"]:checked').value;
        fam["place_birth"] = $("#family_place_birth").val();

        fnEmployeeBasicInfo.ListFamz.push(fam);

        // Remove No data record 
        $(".odd").remove();
        let no = $('#' + tableFamily + ' >tbody >tr').length;

        let newRow = "<tr id='family" + no + "'><td>" + Number(no + 1) + "</td><td>" + fam["name"] + "</td><td>" + $("#family_status_id option:selected").text() + "</td><td>" + fam["address"] + "</td><td>" + fam["phone_no"] + "</td><td>" + fam["gender"] + "</td><td><button name='family" + no + "' onclick='fnEmployeeBasicInfo.deleteRow(this)'>Delete</button></td></tr>";
        $('#' + tableFamily + '').append(newRow);

        $('#familyModal').modal('hide');
        $('#familyModal').on('hidden.bs.modal', function (e) {
            $(this)
                .find("input,textarea,select")
                .val('')
                .end()
                .find("input[type=checkbox], input[type=radio]")
                .prop("checked", "")
                .end();
        });
    },
    deleteRow: function (e) {

        if (e.name.indexOf("family") !== -1) {
            $('#' + e.name + '').remove();
            let index = e.name.replace(/[^0-9]/g, '');
            fnEmployeeBasicInfo.ListFamz.splice(Number(index - 1), 1);
        }
    },
    getMaritalStatusList: function () {
        let marriedUrl = API + "/Hris/MarriedStatus/List";
        let params = {};
        params["sortBy"] = "married_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(marriedUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#married_status_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.married_name));
                });
            } 
        });
    },
    getJobTitleList: function () {

        let marriedUrl = API + "/Hris/JobTitle/List";
        let params = {};
        params["sortBy"] = "id";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(marriedUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#job_title_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.title_name));
                });
            } 
        });
    },
    getJobGradeList: function () {

        let marriedUrl = API + "/Hris/JobGrade/List";
        let params = {};
        params["sortBy"] = "id";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(marriedUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#job_grade_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.grade_name));
                });
            } 
        });
    },
    getEmployementStatusList: function () {

        let marriedUrl = API + "/Hris/EmployementStatus/List";
        let params = {};
        params["sortBy"] = "id";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(marriedUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#employement_status_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.employment_status_name));
                });
            } 
        });
    },
    getFamilyStatusList: function () {
        let familyStatusUrl = API + "/Hris/FamilyStatus/List";
        let params = {};
        params["sortBy"] = "family_status_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(familyStatusUrl, params, function (response) {
            response = parseJson(response);

            if (response.success) {
                let select = $("#family_status_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.family_status_name));
                });
            }
        });
    },
    getReligionList: function () {
        let religionUrl = API + "/Hris/Religion/List";
        let params = {};
        params["sortBy"] = "religion_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(religionUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#religion_id");
                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.religion_name));
                });
            }
        });
    },
    getNationalityList: function () {
        let nationalityUrl = API + "/Hris/Nationality/List";
        let params = {};
        params["sortBy"] = "nationality_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(nationalityUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {

                let select = $("#nationality_id");
                $.each(response.rows, function (a, b) {

                    select.append($("<option/>").attr("value", b.id).text(b.nationality_name));
                });
            } 
        });
    },
    getEducationList: function () {

        if ($('#last_education option').length === 0) {
            let educationUrl = API + "/Hris/EducationLevel/List";
            let params = {};
            params["sortBy"] = "education_name";
            params["sortDir"] = "asc";
            params["rows"] = 25;
            params["page"] = 1;

            ajaxGet(educationUrl, params, function (response) {
                response = parseJson(response);

                if (response.success) {
                    let select = $("#last_education");

                    $.each(response.rows, function (a, b) {
                        select.append($("<option/>").attr("value", b.id).text(b.education_name));
                    });
                }
            });
        }
    },
    generateCourseList: function () {
        let paramSearch = {};
        paramSearch['search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableCourse, courseUrlList, paramSearch, columnsCourse, 'id', 'asc');
        setcolvis(tableCourse, columnsHistory);
    },
    generateHistoryList: function () {
        let paramSearch = {};
        paramSearch['search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableHistory, historyUrlList, paramSearch, columnsHistory, 'id', 'asc');
        setcolvis(tableHistory, columnsHistory);
    },
    generateFamilyList: function () {

        let paramSearch = {};
        paramSearch['search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableFamily, familyUrlList, paramSearch, columnsFamily, 'id', 'asc');
        setcolvis(tableFamily, columnsFamily);
    },
    generateEmergencyList: function () {
        let paramSearch = {};
        paramSearch['search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableEmergency, emergencyUrlList, paramSearch, columnsEmergency, 'id', 'asc');
        setcolvis(tableEmergency, columnsEmergency);
    },
    generateEducationList: function () {
        let paramSearch = {};
        paramSearch['search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableEducation, educationUrlList, paramSearch, columnsEducation, 'id', 'asc');
        setcolvis(tableEducation, columnsEducation);
    },
    create: function () {

        var msg = '';
        msg = ($('#job_title_id').val() === '') ? 'Job Title Cannot empty' : '';
        msg = ($('#job_grade_id').val() === '') ? msg + 'Job Title Cannot empty' : '';
        msg = ($('#business_unit_id').val() === '') ? msg + 'Bussiness Unit Cannot empty' : '';
        msg = ($('#employement_status_id').val() === '') ? msg + 'Employee status Cannot empty' : '';
        if (msg !== '') {
            DangerNotif(msg);
            return;
        } else {
            const forms = document.querySelectorAll("[id='" + formCreate + "']");//('form');
            const form = forms[0];

            let data = {};
            data['name_employee'] = $('#first_name').val() + ' ' + $('#last_name').val();
            data['date_birth'] = moment($('#date_birth').val()).format("YYYY-MM-DD");
            data['npwp_expiry'] = moment($('#npwp_expiry').val()).format("YYYY-MM-DD");
            data.listExperience = fnEmployeeBasicInfo.ListHistory;
            data.listTraining = fnEmployeeBasicInfo.ListCourse;
            data.listFamily = fnEmployeeBasicInfo.ListFamz;
            data.listEmergency = fnEmployeeBasicInfo.ListEmergency;
            data.listEducation = fnEmployeeBasicInfo.ListEdu;
            [...form.elements].forEach((input) => {
                if (input.name !== '' && input.name.toLowerCase().indexOf('token') === -1) {
                    data[input.name] = (input.name === 'gender') ? document.querySelector('input[name="gender"]:checked').value : input.value;
                }
            });
            console.log("data employee " + data);
            //ajaxPost(ajaxUrlCreate, data, function (response) {
            //    response = parseJson(response);
            //    if (response.success) {
            //        SuccessNotif(response.message, "sucess");
            //        window.location.href = webUrl;
            //    } else {
            //        DangerNotif(response.message);
            //    }
            //});
        }
    },
    update: function (id) {
        ajaxGet(ajaxUrlGet + "/" + id, {}, function (response) {
            response = parseJson(response);

            setFormAction(formUpdate, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                return $("#" + formUpdate).parsley().isValid();
            }, function (response, statusText, xhr, $form) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    fnEmployeeBasicInfo.reloadlist();
                    $('#' + modalUpdate + '').modal('hide');
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
                    SuccessNotif(t_deletesuccess);
                    fnEmployeeBasicInfo.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }
}

$(function () {
    // initiate Year Picker
    $("#history_start").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
    $("#history_end").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
    $("#start_education").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
    $("#end_education").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
    $("#start_training").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
    $("#end_training").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    // Init grid
    fnEmployeeBasicInfo.generateEducationList();
    fnEmployeeBasicInfo.generateEmergencyList();
    fnEmployeeBasicInfo.generateHistoryList();
    fnEmployeeBasicInfo.generateFamilyList();
    fnEmployeeBasicInfo.generateCourseList();

    // Load Dropdown
    fnEmployeeBasicInfo.getNationalityList();
    fnEmployeeBasicInfo.getReligionList();
    fnEmployeeBasicInfo.getMaritalStatusList();
    fnEmployeeBasicInfo.getFamilyStatusList();
    fnEmployeeBasicInfo.getEducationList();
    fnEmployeeBasicInfo.getJobTitleList();
    fnEmployeeBasicInfo.getJobGradeList();
    fnEmployeeBasicInfo.getEmployementStatusList();

    $('#photo').setuploadbox();
    $('#course_cert').setuploadbox();

    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            redirecttolink(webUrl)
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);
});

$('#familyModal').on('shown.bs.modal', function (e) {
    if ($('#family_religion_id option').length === 0) {
        let religionUrl = API + "/Hris/Religion/List";
        let params = {};
        params["sortBy"] = "religion_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(religionUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let Familyselect = $("#family_religion_id");
                $.each(response.rows, function (a, b) {
                    Familyselect.append($("<option/>").attr("value", b.id).text(b.religion_name));
                });
            }
        });
    }

    if ($('#family_status_id option').length === 0) {
        let familyStatusUrl = API + "/Hris/FamilyStatus/List";
        let params = {};
        params["sortBy"] = "family_status_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(familyStatusUrl, params, function (response) {
            response = parseJson(response);

            if (response.success) {
                let select = $("#family_status_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.family_status_name));
                });
            }
        });
    }

    if ($('#family_married_status_id option').length === 0) {
        let marriedUrl = API + "/Hris/MarriedStatus/List";
        let params = {};
        params["sortBy"] = "married_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(marriedUrl, params, function (response) {

            response = parseJson(response);
            if (response.success) {
                let select = $("#family_married_status_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.married_name));
                });
            }
        });
    }

    if ($('#family_institution_id option').length === 0) {
        let marriedUrl = API + "/Hris/Institution/List";
        let params = {};
        params["sortBy"] = "institution_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(marriedUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#family_institution_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.institution_name));
                });
            }
        });
    }

    if ($('#family_education_level_id option').length === 0) {
        let educationUrl = API + "/Hris/EducationLevel/List";
        let params = {};
        params["sortBy"] = "education_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(educationUrl, params, function (response) {
            response = parseJson(response);
            if (response.success) {
                let select = $("#family_education_level_id");
                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.education_name));
                });
            }
        });
    }
});

$('#educationModal').on('shown.bs.modal', function (e) {
    if ($('#education_level_id option').length === 0) {
        let educationUrl = API + "/Hris/EducationLevel/List";
        let params = {};
        params["sortBy"] = "education_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(educationUrl, params, function (response) {
            response = parseJson(response);

            if (response.success) {
                let select = $("#education_level_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.education_name));
                });
            }
        });
    }

    $('#start_education').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
        }
    });

    $('#end_education').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
        }
    });
});

$('#emergencyModal').on('shown.bs.modal', function (e) {
    if ($('#emergency_family_status_id option').length === 0) {
        let familyStatusUrl = API + "/Hris/FamilyStatus/List";
        let params = {};
        params["sortBy"] = "family_status_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(familyStatusUrl, params, function (response) {
            response = parseJson(response);

            if (response.success) {
                let select = $("#emergency_family_status_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.family_status_name));
                });
            }
        });
    }

})

$('#courseModal').on('shown.bs.modal', function (e) {
    if ($('#course_institute_id option').length === 0) {
        let institutionUrl = API + "/Hris/Institution/List";
        let params = {};
        params["sortBy"] = "institution_name";
        params["sortDir"] = "asc";
        params["rows"] = 25;
        params["page"] = 1;

        ajaxGet(institutionUrl, params, function (response) {
            response = parseJson(response);

            if (response.success) {
                let select = $("#course_institute_id");

                $.each(response.rows, function (a, b) {
                    select.append($("<option/>").attr("value", b.id).text(b.institution_name));
                });
            }
        });
    }

})