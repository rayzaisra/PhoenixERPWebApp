 
var webUrl = WEB + "/Hris/JobTitle";
var ajaxUrl = API + "Hris/BussinesUnitJobLevel";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "JobTitleFormUpdate";
var ajaxUrlGet = ajaxUrl + "/GetDetail"; 

var ajaxUrlListJobLevel = API_HRIS + "/JobLevel/List";
var ajaxUrlListJobGrade = API_HRIS + "/JobGrade/List";
var ajaxUrlListJobTitle = API_HRIS + "/JobTitle/List/";

var ajaxUrlListJobLevelGeneral = API_GENERAL + "/BusinessUnit/ListWithParent";
var ajaxUrlBusinessUnitJobLevel = ajaxUrl + "/List";



$(function () {
    var xid = $("#" + xform +' [name="id"]').val();
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

            $('#job_level_id').append("<option value=" + response.data.job_level_id + ">" + response.data.job_level_name + "</option>");
            $('#job_level_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlListJobLevel,
                searchparam: 'Search_job_name',
                labelField: 'job_name',
                valueField: 'id',
                searchField: 'job_name'
            });
            $('#job_grade_id').append("<option value=" + response.data.job_grade_id + ">" + response.data.job_grade_name + "</option>");
            $('#job_grade_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlListJobGrade,
                searchparam: 'Search_grade_name',
                labelField: 'grade_name',
                valueField: 'id',
                searchField: 'grade_name'
            });
            $('#job_title_id').append("<option value=" + response.data.job_title_id + ">" + response.data.job_title_name + "</option>");
            $('#job_title_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlListJobTitle,
                searchparam: 'Search_title_name',
                labelField: 'title_name',
                valueField: 'id',
                searchField: 'title_name'
            });



            var id = null;
            var datadef = "token";
            $('#business_unit_group_id').append("<option value=" + response.data.business_unit_group_id + ">" + response.data.businessUnitGroup.name + "</option>");
            $('#business_unit_group_id').setcombobox({
                data: { 'rows': 100, Search_business_unit_level: 1000 },
                url: ajaxUrlListJobLevelGeneral,
                maxItems: 0,
                searchparam: 'unit_name',
                labelField: 'unit_name',
                valueField: 'id'
            });
            if (response.data.business_unit_subgroup_id !== null) {
                $('#business_unit_subgroup_id').append("<option value=" + response.data.business_unit_subgroup_id + ">" + response.data.businessUnitSubgroup.name + "</option>");

            }
            $('#business_unit_subgroup_id').setcombobox({
                data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
                url: ajaxUrlListJobLevelGeneral,
                maxItems: 0,
                searchparam: 'unit_name',
                labelField: 'unit_name',
                valueField: 'id'
            });
            if (response.data.business_unit_division_id !== null) {
                $('#business_unit_division_id').append("<option value=" + response.data.business_unit_division_id + ">" + response.data.businessUnitDivision.name + "</option>");

            }
             $('#business_unit_division_id').setcombobox({
                data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
                url: ajaxUrlListJobLevelGeneral,
                maxItems: 0,
                searchparam: 'unit_name',
                labelField: 'unit_name',
                valueField: 'id'
            });
            if (response.data.business_unit_departement_id !== null) {
                $('#business_unit_departement_id').append("<option value=" + response.data.business_unit_departement_id + ">" + response.data.businessUnitDepartement.name + "</option>");

            }
            $('#business_unit_departement_id').setcombobox({
                data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
                url: ajaxUrlListJobLevelGeneral,
                maxItems: 0,
                searchparam: 'unit_name',
                labelField: 'unit_name',
                valueField: 'id'
            });

            if ($('#business_unit_group_id').val() !== null) {
                id = $('#business_unit_group_id').val();
                datadef = 900;
                $('#business_unit_subgroup_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });
            }

            $('#business_unit_group_id').on('select2:select', function (e) {
                id = e.params.data.id;
                datadef = 900;
                $('#business_unit_subgroup_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });

            });

            if ($('#business_unit_subgroup_id').val() !== null) {
                id = $('#business_unit_subgroup_id').val();
                $('#business_unit_division_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 800, Search_parent_unit: id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });
            }

            $('#business_unit_subgroup_id').on('select2:select', function (e) {
                id = e.params.data.id;
                $('#business_unit_division_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 800, Search_parent_unit: id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });


            });

            if ($('#business_unit_division_id').val() !== null) {
                id = $('#business_unit_division_id').val();
                $('#business_unit_departement_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 700, Search_parent_unit: id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });
            }

            $('#business_unit_division_id').on('select2:select', function (e) {
                id = e.params.data.id;
                $('#business_unit_departement_id').setcombobox({
                    data: { 'rows': 100, Search_business_unit_level: 700, Search_parent_unit: id },
                    url: ajaxUrlListJobLevelGeneral,
                    maxItems: 0,
                    searchparam: 'unit_name',
                    labelField: 'unit_name',
                    valueField: 'id'
                });

            });

            $('#business_unit_departement_id').on('select2:select', function (e) {
                id = e.params.data.id;

            });

            var Division = '';
            var Departement = '';
            var Subgroup = '';
            var Group = '';
            var Username = '';
            var Remarks = '';
            if (response.data.parent_id !== null) {

                if (response.data.parentBusinessUnit.id !== null) {
                    if (response.data.parentBusinessUnit.business_unit_division_id !== null) {
                        Division = response.data.parentBusinessUnit.business_unit_division_id.name;
                    } else {
                        Division = '';
                    }

                    if (response.data.parentBusinessUnit.business_unit_departement_id !== null) {
                        Departement = response.data.parentBusinessUnit.business_unit_departement_id.name;
                    } else {
                        Departement = '';
                    }

                    if (response.data.parentBusinessUnit.business_unit_subgroup_id !== null) {
                        Subgroup = response.data.parentBusinessUnit.business_unit_subgroup_id.name;
                    } else {
                        Subgroup = '';
                    }

                    if (response.data.parentBusinessUnit.business_unit_group_id !== null) {
                        Group = response.data.parentBusinessUnit.business_unit_group_id.name;
                    } else {
                        Group = '';
                    }

                    //if (response.data.parentBusinessUnit.username === null) {
                    //    Username = " Vacant ";
                    //} else {
                    //    Username = response.data.parentBusinessUnit.username;
                    //}

                    if (response.data.parentBusinessUnit.remarks === null ) {
                        Remarks = " ";
                    } else {
                        Remarks = " " + response.data.parentBusinessUnit.remarks;
                    }
                }

                if (response.data.parentBusinessUnit.business_unit_group_id.name !== null && response.data.parentBusinessUnit.business_unit_subgroup_id.name !== null && response.data.parentBusinessUnit.business_unit_division_id.name !== null && response.data.parentBusinessUnit.business_unit_departement_id.name !== null) {

                    datanya = Group + " / " + Subgroup + " / " + Division + " / " + Departement + " -- " + response.data.parentBusinessUnit.name + " -- " + Username + Remarks;

                } else if (response.data.parentBusinessUnit.business_unit_group_id.name !== null && response.data.parentBusinessUnit.business_unit_subgroup_id.name !== null && response.data.parentBusinessUnit.business_unit_division_id.name !== null) {

                    datanya = Group + " / " + Subgroup + " / " + Division + " -- " + response.data.parentBusinessUnit.name + " -- " + Username + Remarks;

                } else if (response.data.parentBusinessUnit.business_unit_group_id.name !== null && response.data.parentBusinessUnit.business_unit_subgroup_id.name !== null) {
                    datanya = Group + " / " + Subgroup + " --  " + response.data.parentBusinessUnit.name + " -- " + Username + Remarks; 
                } else {
                    datanya = Group + " --  " + response.data.parentBusinessUnit.name + " -- " + Username + Remarks;
                }

                $('#parent_id').append("<option value=" + response.data.parentBusinessUnit.id + ">" + datanya + "</option>");


            }

            $('#parent_id').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlBusinessUnitJobLevel,
                searchparam: 'search_title_name',
                labelField: 'job_title_name',
                valueField: 'id',
                searchField: 'job_title_name',
                render: function (data) {
                    var datanya = '';
                    console.log(data.odata);
                    if (data.odata) {
                        var Division2 = '';
                        var Departement2 = '';
                        var Subgroup2 = '';
                        var Group2 = '';
                        var UserName2 = '';
                        var Remarks2 = '';
 

                        if (data.odata.businessUnitDivision.name !== "") {
                            Division2 = data.odata.businessUnitDivision.name;
                        } else {
                            Division2 = '';
                        }

                        if (data.odata.businessUnitDepartement.name !== "") {
                            Departement2 = data.odata.businessUnitDepartement.name;
                        } else {
                            Departement2 = '';
                        }

                        if (data.odata.businessUnitSubgroup.name !== "") {
                            Subgroup2 = data.odata.businessUnitSubgroup.name;
                        } else {
                            Subgroup2 = '';
                        }

                        if (data.odata.businessUnitGroup.name !== "") {
                            Group2 = data.odata.businessUnitGroup.name;
                        } else {
                            Group2 = '';
                        }

                        //if (data.odata.businessUnitGroup.username !== null ) {
                        //    UserName2 = data.odata.businessUnitGroup.username;
                        //} else {
                        //    UserName2 = ' Vacant ';
                        //}

                        if (data.odata.remarks === null) {
                            Remarks2 = ' ';
                        } else {
                            Remarks2 = ' ' + data.odata.remarks;
                        }



                        if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null && data.odata.businessUnitDivision.name !== null && data.odata.businessUnitDepartement.name !== null) {

                            datanya = Group2 + " / " + Subgroup2 + " / " + Division2 + " / " + Departement2 + " -- " + data.odata.job_title_name + " -- " + UserName2 + Remarks2;

                        } else if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null && data.odata.businessUnitDivision.name !== null) {

                            datanya = Group2 + " / " + Subgroup2 + " / " + Division2 + " -- " + data.odata.job_title_name + " -- " + UserName2 +  Remarks2;

                        } else if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null) {
                            datanya = Group2 + " / " + Subgroup2 + " --  " + data.odata.job_title_name + " -- " + UserName2 +  Remarks2;
                        } else {
                            datanya = Group2 + " --  " + data.odata.job_title_name + " -- " + UserName2 +  Remarks2;
                        }
                        return datanya;
                    }
                }
            });

        });
    }
    


    
    
});
  
