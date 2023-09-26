var webUrl = WEB + "/Hris/EmployeeBasicInfo";
var ajaxUrl = API + "Hris/EmployeeBasicInfo";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "EmployeeBasicInfoFormUpdate";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlBusinessUnitJobLevel = API + "Hris/BussinesUnitJobLevel/List";

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
            retval = '';
            //retval += '<span class="dropdown">';
            //retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            //retval += '<i class="la la-ellipsis-h"></i>';
            //retval += '</a>';
            //retval += '<div class="dropdown-menu dropdown-menu-right">';
            //retval += '<a class="dropdown-item family" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            //retval += '<a class="dropdown-item family" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            //retval += '</div>';
            //retval += '</span>';
            retval += '<a class="family" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a> | <a class="family" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            return retval;
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
        "data": "relation_name",
        "title": "<span class='translate'  data-args='Relation'>Relation</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "full_name",
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
            retval = '';
            //retval += '<span class="dropdown">';
            //retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            //retval += '<i class="la la-ellipsis-h"></i>';
            //retval += '</a>';
            //retval += '<div class="dropdown-menu dropdown-menu-right">';
            //retval += '<a class="dropdown-item emergency" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            //retval += '<a class="dropdown-item emergency" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            //retval += '</div>';
            //retval += '</span>';
            retval += '<a class="emergency" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a> | <a class="emergency" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            return retval;
            //return '<a href="javascript:void(0)" onClick="fnEmployee.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">View</i></a>';
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
        "data": "phone",
        "title": "<span class='translate'  data-args='Company Phone'>Company Phone</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": null,
        "title": "<span class='translate'  data-args='Year'>Year</span>",
        "sClass": "",
        orderable: true,
        "render": function (data, type, row, meta) {
            return data.start_date + " - " + data.end_date;
        }
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
            retval = '';
            //retval += '<span class="dropdown">';
            //retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            //retval += '<i class="la la-ellipsis-h"></i>';
            //retval += '</a>';
            //retval += '<div class="dropdown-menu dropdown-menu-right">';
            //retval += '<a class="dropdown-item history" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            //retval += '<a class="dropdown-item history" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            //retval += '</div>';
            //retval += '</span>';
            //return retval;
            retval += '<a class="history" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a> | <a class="history" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            return retval;
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
            //retval += '<span class="dropdown">';
            //retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            //retval += '<i class="la la-ellipsis-h"></i>';
            //retval += '</a>';
            //retval += '<div class="dropdown-menu dropdown-menu-right">';
            //retval += '<a class="dropdown-item course" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            //retval += '<a class="dropdown-item course" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            //retval += '</div>';
            //retval += '</span>';
            retval += '<a class="course" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a> | <a class="course" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            return retval;
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
        "data": "education_level_name",
        "title": "<span class='translate'  data-args='Last Education'>Last Education</span>",
        "sClass": "",
        orderable: true
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
            return data.start_education + " - " + data.end_education;
        }
    }, {
        "data": "address",
        "title": "<span class='translate'  data-args='Location'>Location</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "qualification",
        "title": "<span class='translate' data-args='Qualification Attained'>Qualification Attained</span>",
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
            //retval += '<span class="dropdown">';
            //retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            //retval += '<i class="la la-ellipsis-h"></i>';
            //retval += '</a>';
            //retval += '<div class="dropdown-menu dropdown-menu-right">';
            //retval += '<a class="dropdown-item education" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a>';
            //retval += '<a class="dropdown-item education" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            //retval += '</div>';
            //retval += '</span>';
            retval += '<a class="education" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralUpdate(this)" href="javascript:void(0)"><i class="la la-edit"></i> Edit</a> | <a class="education" data-id="' + row.id + '" onClick="fnEmployeeBasicInfo.GeneralDelete(this)" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            return retval;
        }
    }
];

fnEmployeeBasicInfo = {
    reloadlist: function (e) {
        TABLES[e].ajax.reload(null, false);
    },
    EmployeeBasicInfoID: 0,
    ListFamz: new Array(),
    ListEdu: new Array(),
    ListCourse: new Array(),
    GeneralUpdate: function (e) {

        var url = '';
        switch (e.classList[1]) {
            case "family":
                url = familyUrlGet;
                break;
            case "emergency":
                url = emergencyUrlGet;
                break;
            case "history":
                url = historyUrlGet;
                break;
            case "course":
                url = courseUrlGet;
                break;
            case "education":
                url = educationUrlGet;
                break;
        }
        ajaxGet(url + "/" + e.dataset.id, {}, function (response) {
            response = parseJson(response);

            switch (e.classList[1]) {
                case "family":
                    fnEmployeeBasicInfo.OpenFamilyModal(response);
                    break;
                case "emergency":
                    fnEmployeeBasicInfo.OpenEmergencyModal(response);
                    break;
                case "history":
                    fnEmployeeBasicInfo.OpenHistoryModal(response);
                    break;
                case "course":
                    fnEmployeeBasicInfo.OpenCourseModal(response);
                    break;
                case "education":
                    fnEmployeeBasicInfo.OpenEducationModal(response);
                    break;
            }
        });
    },
    GeneralDelete: function (e) {
        //debugger;
        var url = '';
        var id = e.dataset.id;
        switch (e.classList[1]) {
            case "family":
                url = familyUrlDelete;
                break;
            case "emergency":
                url = emergencyUrlDelete;
                break;
            case "history":
                url = historyUrlDelete;
                break;
            case "course":
                url = courseUrlDelete;
                break;
            case "education":
                url = educationUrlDelete;
                break;
        }

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
                ajaxDelete(url + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnEmployeeBasicInfo.reloadlist(tableFamily);
                        fnEmployeeBasicInfo.reloadlist(tableEmergency);
                        fnEmployeeBasicInfo.reloadlist(tableHistory);
                        fnEmployeeBasicInfo.reloadlist(tableCourse);
                        fnEmployeeBasicInfo.reloadlist(tableEducation);
                    } else {
                        DangerNotif(response.message);
                    }
                });
            }
        });
    },
    OpenFamilyModal: function (response) {

        $('#familyModal').modal('show');

        $('#familyModal').on('shown.bs.modal', function (e) {
            $("#family_birthday").datepicker({
                format: "dd/mm/yyyy"
            });

            // Get data from API to set Dropdown
            if ($('#family_religion_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#family_religion_id').val(data.religion_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Religion.");
                    }
                });
            }

            if ($('#family_status_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#family_status_id').val(data.family_status_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Family Status.");
                    }
                });
            }

            if ($('#family_married_status_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#family_married_status_id').val(data.married_status_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Maried Status.");
                    }
                });
            }

            if ($('#family_institution_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#family_institution_id').val(data.institution_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Institusion.");
                    }
                });
            }

            if ($('#family_education_level_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#family_education_level_id').val(data.name);
                        };
                    } else {
                        console.log("Error when load Education Level.");
                    }
                });
            }

            if (typeof response != 'undefined') {
                var data = response.data;
                var birth = moment(data.birthday).format("DD/MM/YYYY");
                var gender = data.gender;
                // Set Input
                $('#family_id').val(data.id);
                $('#family_name').val(data.name);
                $('#family_birthday').val(birth);
                $('[value="' + gender + '"]').attr('checked', true); //radio
                $('#family_phone_no').val(data.phone_no);
                $('#family_other_no').val(data.other_no);
                $('#family_place_birth').val(data.place_birth);
                $('#family_address').val(data.address);
                $('#family_description').val(data.description);
            }
        })
    },
    OpenEmergencyModal: function (response) {

        $('#emergencyModal').modal('show');

        $('#emergencyModal').on('shown.bs.modal', function (e) {

            // Get data from API to set Dropdown
            if ($('#emergency_family_status_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#emergency_family_status_id').val(data.family_status_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Family Status.");
                    }
                });
            }

            if (typeof response != 'undefined') {
                let data = response.data;

                // Set Input
                $('#emergency_id').val(data.id);
                $('#emergency_phone_no').val(data.phone_no);
                $('#emergency_address').val(data.address);
                $('#emergency_name').val(data.full_name);
                $('#emergency_other_no').val(data.other_no);
                if ($('#emergency_family_status_id option').length > 0) {
                    $('#emergency_family_status_id').val(data.family_status_id);
                }
            }
        })
    },
    OpenEducationModal: function (response) {

        $('#educationModal').modal('show');

        $('#educationModal').on('shown.bs.modal', function (e) {

            // Get data from API to set Dropdown
            if ($('#education_level_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#education_level_id').val(data.name);
                        };
                    } else {
                        console.log("Error when load Education Level.");
                    }
                });
            }

            if ($('#education_institution_id option').length == 0) {
                let marriedUrl = API + "/Hris/Institution/List";
                let params = {};
                params["sortBy"] = "institution_name";
                params["sortDir"] = "asc";
                params["rows"] = 25;
                params["page"] = 1;

                ajaxGet(marriedUrl, params, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        let select = $("#education_institution_id");

                        $.each(response.rows, function (a, b) {
                            select.append($("<option/>").attr("value", b.id).text(b.institution_name));
                        });
                        if (typeof data != 'undefined') {
                            $('#education_institution_id').val(data.institution_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Institusion.");
                    }
                });
            }

            if (typeof response != 'undefined') {
                let data = response.data;

                // Set Input
                $('#education_id').val(data.id);
                $('#education_location').val(data.address);
                $('#history_start').val(data.start_education);
                $('#history_end').val(data.start_education);
                $('#qualification').val(data.qualification);
                if ($('#education_institution_id option').length > 0) {
                    $('#education_institution_id').val(data.institution_id);
                }
                if ($('#education_level_id option').length > 0) {
                    $('#education_level_id').val(data.education_status_id);
                }
            }
        })
    },
    OpenHistoryModal: function (response) {

        $('#historyModal').modal('show');

        $('#historyModal').on('shown.bs.modal', function (e) {

            // Get data from API to set Dropdown
            if ($('#education_level_id option').length == 0) {
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
                        if (typeof data != 'undefined') {
                            $('#education_level_id').val(data.name);
                        };
                    } else {
                        console.log("Error when load Education Level.");
                    }
                });
            }

            if ($('#education_institution_id option').length == 0) {
                let marriedUrl = API + "/Hris/Institution/List";
                let params = {};
                params["sortBy"] = "institution_name";
                params["sortDir"] = "asc";
                params["rows"] = 25;
                params["page"] = 1;

                ajaxGet(marriedUrl, params, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        let select = $("#education_institution_id");

                        $.each(response.rows, function (a, b) {
                            select.append($("<option/>").attr("value", b.id).text(b.institution_name));
                        });
                        if (typeof data != 'undefined') {
                            $('#education_institution_id').val(data.institution_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Institusion.");
                    }
                });
            }

            if (typeof response != 'undefined') {
                let data = response.data;

                // Set Input
                $('#education_id').val(data.id);
                $('#education_institution_id').val(data.phone_no);
                $('#education_location').val(data.address);
                $('#start_education').val(data.start_education);
                $('#end_education').val(data.start_education);
                $('#qualification').val(data.qualification);
                if ($('#education_institution_id option').length > 0) {
                    $('#education_institution_id').val(data.institution_id);
                }
                if ($('#education_level_id option').length > 0) {
                    $('#education_level_id').val(data.education_status_id);
                }
            }
        })
    },
    OpenCourseModal: function (response) {

        $('#courseModal').modal('show');

        $('#courseModal').on('shown.bs.modal', function (e) {

            // Get data from API to set Dropdown
            if ($('#course_institute_id option').length == 0) {
                let marriedUrl = API + "/Hris/Institution/List";
                let params = {};
                params["sortBy"] = "institution_name";
                params["sortDir"] = "asc";
                params["rows"] = 25;
                params["page"] = 1;

                ajaxGet(marriedUrl, params, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        let select = $("#course_institute_id");

                        $.each(response.rows, function (a, b) {
                            select.append($("<option/>").attr("value", b.id).text(b.institution_name));
                        });
                        if (typeof data != 'undefined') {
                            $('#course_institute_id').val(data.institution_id); //dropdown
                        };
                    } else {
                        console.log("Error when load Institusion.");
                    }
                });
            }

            if (typeof response != 'undefined') {
                let data = response.data;

                // Set Input
                $('#education_id').val(data.id);
                $('#course_institute_id').val(data.phone_no);
                $('#course_name').val(data.address);
                $('#course_address').val(data.start_education);
                $('#no_certification').val(data.start_education);
                $('#expired_certification').val(data.qualification);
                $('#start_training').val(data.qualification);
                $('#end_training').val(data.qualification);
                $('#course_description').val(data.qualification);
                $('#course_cert').val();// upload file
                if ($('#education_institution_id option').length > 0) {
                    $('#education_institution_id').val(data.institution_id);
                }
                if ($('#education_level_id option').length > 0) {
                    $('#education_level_id').val(data.education_status_id);
                }
            }
        })
    },
    addHistory: function () {
        var history = {};
        var url = '';

        history["id"] = $("#history_id").val();
        history["company_name"] = $("#company_name").val();
        history["phone"] = $("#history_phone").val();
        history["address"] = $("#history_address").val();
        history["remark"] = $("#history_remark").val();
        history["start_date"] = $("#history_start").val();
        history["end_date"] = $("#history_end").val();
        history["position"] = $("#history_position").val();
        history["employee_basic_info_id"] = $("input[name='id']").val();

        url = (history.id == null || history.id == '') ? historyUrlCreate : historyUrlUpdate;

        ajaxPost(url, history, function (response) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(response.Message, "sucess");
                fnEmployeeBasicInfo.reloadlist(tableHistory);
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
            } else {
                DangerNotif(response.message);
            }
        });
    },
    addCourse: function () {

        let course = {};
        course["name_training"] = $("#course_name").val();
        course["institution_id"] = $("#course_institute_id").val();
        course["address"] = $("#course_address").val();
        course["start_training"] = $("#start_training").val();
        course["end_training"] = $("#end_training").val();
        course["no_certification"] = $("#no_certification").val();
        course["expired_certificate"] = $("#expired_certificate").val();
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
        var emergency = {};
        var url = '';

        emergency["id"] = $("#emergency_id").val();
        emergency["employee_basic_info_id"] = $("input[name='id']").val();
        emergency["family_status_id"] = $("#emergency_family_status_id").val();
        emergency["full_name"] = $("#emergency_name").val();
        emergency["phone_no"] = $("#emergency_phone_no").val();
        emergency["other_no"] = $("#emergency_other_no").val();
        emergency["address"] = $("#emergency_address").val();

        url = (emergency.id == null || emergency.id == '') ? emergencyUrlCreate : emergencyUrlUpdate;

        ajaxPost(url, emergency, function (response) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(response.Message, "sucess");
                fnEmployeeBasicInfo.reloadlist(tableEmergency);
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
            } else {
                DangerNotif(response.message);
            }
        });
    },
    addEducation: function () {
        var edu = {};
        var url = "";

        edu["id"] = $("#education_id").val();
        edu["employee_basic_info_id"] = $("input[name='id']").val();
        edu["education_level_id"] = $("#education_level_id").val();
        edu["institution_id"] = $("#education_institution_id").val();
        edu["address"] = $("#education_location").val();
        edu["majors"] = $("#education_major").val();
        edu["qualification"] = $("#qualification").val();
        edu["start_education"] = $("#start_education").val();
        edu["end_education"] = $("#end_education").val();

        url = (edu.id == null || edu.id == '') ? educationUrlCreate : educationUrlUpdate;

        ajaxPost(url, edu, function (response) {

            response = parseJson(response);
            if (response.success) {
                SuccessNotif(response.Message, "sucess");
                fnEmployeeBasicInfo.reloadlist(tableEducation);
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
            } else {
                DangerNotif(response.message);
            }
        });
    },
    addFamily: function () {
        let fam = {};
        var birthdate = moment($('#family_birthday').val(), 'DD/MM/YYYY');
        var url = '';

        fam["employee_basic_info_id"] = $("input[name='id']").val();
        fam["id"] = $("#family_id").val();
        fam["name"] = $("#family_name").val();
        fam["family_status_id"] = $("#family_status_id").val();
        fam["married_status_id"] = $("#family_married_status_id").val();
        fam["institution_id"] = $("#family_institution_id").val();
        fam["education_id"] = $("#family_education_id").val();
        fam["religion_id"] = $("#family_religion_id").val();
        fam["description"] = $("#family_description").val();
        fam["phone_no"] = $("#family_phone_no").val();
        fam["other_no"] = $("#family_other_no").val();
        fam["birthday"] = birthdate.format('YYYY-MM-DD');
        fam["address"] = $("#family_address").val();
        fam["gender"] = document.querySelector('input[name="family_gender"]:checked').value;
        fam["place_birth"] = $("#family_place_birth").val();

        url = (fam.id == null || fam.id == '') ? familyUrlCreate : familyUrlUpdate;

        ajaxPost(url, fam, function (response) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(response.Message, "sucess");
                fnEmployeeBasicInfo.reloadlist(tableFamily);
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
            } else {
                DangerNotif(response.message);
            }
        });
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
            } else {
                DangerNotif(response.message);
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
            } else {
                DangerNotif(response.message);
            }
        });
    },
    getEducationList: function () {

        if ($('#last_education option').length == 0) {
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
        paramSearch['Search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableCourse, courseUrlList, paramSearch, columnsCourse, 'id', 'asc');
        setcolvis(tableCourse, columnsHistory);
    },
    generateHistoryList: function () {
        let paramSearch = {};
        paramSearch['Search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableHistory, historyUrlList, paramSearch, columnsHistory, 'id', 'asc');
        setcolvis(tableHistory, columnsHistory);
    },
    generateFamilyList: function () {
        let paramSearch = {};
        paramSearch['Search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableFamily, familyUrlList, paramSearch, columnsFamily, 'id', 'asc');
        setcolvis(tableFamily, columnsFamily);
    },
    generateEmergencyList: function () {
        let paramSearch = {};
        paramSearch['Search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableEmergency, emergencyUrlList, paramSearch, columnsEmergency, 'id', 'asc');
        setcolvis(tableEmergency, columnsEmergency);
    },
    generateEducationList: function () {

        let paramSearch = {};
        paramSearch['Search_employee_basic_info_id'] = fnEmployeeBasicInfo.EmployeeBasicInfoID;
        InitTable(tableEducation, educationUrlList, paramSearch, columnsEducation, 'id', 'asc');
        setcolvis(tableEducation, columnsEducation);
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

    var xid = $("#" + xform + ' [name="id"]').val();
    fnEmployeeBasicInfo.EmployeeBasicInfoID = xid;

    // Load Grid
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
            $('[value="' + response.data.gender + '"]').attr('checked', true); //radio
            var birth = moment(response.data.birthday).format("DD/MM/YYYY");
            $('#date_birth').val(birth);


            $('#job_title_id').setcombobox({
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

                            datanya = Group + " / " + Subgroup + " / " + Division + " / " + Departement + " -- " + data.odata.job_title_name;

                        } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null && data.odata.businessUnitDivision.name != null) {

                            datanya = Group + " / " + Subgroup + " / " + Division + " -- " + data.odata.job_title_name;

                        } else if (data.odata.businessUnitGroup.name != null && data.odata.businessUnitSubgroup.name != null) {
                            datanya = Group + " / " + Subgroup + " --  " + data.odata.job_title_name;
                        } else {
                            datanya = Group + " --  " + data.odata.job_title_name
                        }
                        return datanya;
                    }
                }
            });
        });
    }

});

function UpdateData() {



}

