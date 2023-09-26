 
var webUrl = WEB + "/Hris/JobTitle";
var ajaxUrl = API + "Hris/BussinesUnitJobLevel";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "JobTitleFormCreate";
var ajaxUrlListJobLevel = API_HRIS + "/JobLevel/List";
var ajaxUrlListJobGrade = API_HRIS + "/JobGrade/List";
var ajaxUrlListJobTitle = API_HRIS + "/JobTitle/List/";

var ajaxUrlListJobLevelGeneral = API_GENERAL + "/BusinessUnit/ListWithParent";
var ajaxUrlBusinessUnitJobLevel = ajaxUrl + "/List";

$(function () {
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                redirecttolink(webUrl);
            } else {
                DangerNotif(response.message);
            }
        });
    $('#job_level_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListJobLevel,
        searchparam: 'Search_job_name',
        labelField: 'job_name',
        valueField: 'id',
        searchField: 'job_name'
    });
    $('#job_grade_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListJobGrade,
        searchparam: 'Search_grade_name',
        labelField: 'grade_name',
        valueField: 'id',
        searchField: 'grade_name'
    });

    $('#job_title_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListJobTitle,
        searchparam: 'Search_title_name',
        labelField: 'title_name',
        valueField: 'id',
        searchField: 'title_name'
    });
 
    $('#parent_id').setcombobox({
        data: { 'rows': 100},
        url: ajaxUrlBusinessUnitJobLevel,
        searchparam: 'search_title_name',
        labelField: 'job_title_name',
        valueField: 'id',
        searchField: 'job_title_name',
        render: function (data) {
            var datanya = '';
            console.log(data);
            if (data.odata) { 
                var Division = '';
                var Departement = '';
                var Subgroup = '';
                var Group = '';
                var UserName = '';
                var Remarks = '';
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

                //if (data.odata.username !== null) {
                //    UserName = data.odata.username;
                //} else {
                //    UserName = ' Vacant ';
                //}

                if (data.odata.remarks === null) {
                    Remarks = ' ';
                } else {
                    Remarks = ' ' + data.odata.remarks;
                }


                if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null && data.odata.businessUnitDivision.name !== null && data.odata.businessUnitDepartement.name !== null) {

                    datanya = Group + " / " + Subgroup + " / " + Division + " / " + Departement + " -- " + data.odata.job_title_name + " -- " + UserName + Remarks;

                } else if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null && data.odata.businessUnitDivision.name !== null) {

                    datanya = Group + " / " + Subgroup + " / " + Division + " -- " + data.odata.job_title_name + " -- " + UserName +  Remarks;

                } else if (data.odata.businessUnitGroup.name !== null && data.odata.businessUnitSubgroup.name !== null) {
                    datanya = Group + " / " + Subgroup + " --  " + data.odata.job_title_name + " -- " + UserName +  Remarks;
                } else {
                    datanya = Group + " --  " + data.odata.job_title_name + " -- " + UserName +  Remarks;
                }
                return datanya;
            }
        }
    });
 
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_hr);


    var id = null;
    var datadef = "token";

    $('#business_unit_group_id').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: 1000 },
        url: ajaxUrlListJobLevelGeneral,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#business_unit_subgroup_id').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlListJobLevelGeneral,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#business_unit_division_id').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlListJobLevelGeneral,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#business_unit_departement_id').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlListJobLevelGeneral,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });

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

        //$('#superior').setcombobox({
        //    data: { 'rows': 100, Search_business_unit_group_id: id},
        //    url: ajaxUrlBusinessUnitJobLevel,
        //    searchparam: 'Search_job_title_name',
        //    labelField: 'job_title_name',
        //    valueField: 'id',
        //    searchField: 'job_title_name'
        //});
    });


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

     

       

        //if ($('#business_unit_group_id').val() !== null || $('#business_unit_group_id').val() !== "") {



        //    $('#superior').setcombobox({
        //        data: { 'rows': 100, Search_business_unit_group_id: $('#business_unit_group_id').val(), Search_business_unit_subgroup_id: id },
        //        url: ajaxUrlBusinessUnitJobLevel,
        //        searchparam: 'Search_job_title_name',
        //        labelField: 'job_title_name',
        //        valueField: 'id',
        //        searchField: 'job_title_name'
        //    });

        //} else {

        //    alert('Please Input stucture organization correctly');
        //    return false;
        //}

      
    });

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


        //if ($('#business_unit_group_id').val() !== null || $('#business_unit_division_id').val() !== null) {

        //    $('#superior').setcombobox({
        //        data: { 'rows': 100, Search_business_unit_group_id: $('#business_unit_group_id').val(), Search_business_unit_subgroup_id: $('#business_unit_subgroup_id').val(), Search_business_unit_division_id:id},
        //        url: ajaxUrlBusinessUnitJobLevel,
        //        searchparam: 'Search_job_title_name',
        //        labelField: 'job_title_name',
        //        valueField: 'id',
        //        searchField: 'job_title_name'
        //    });

        //} else {

        //    alert('Please Input stucture organization correctly');
        //    return false;
        //}
    });

    $('#business_unit_departement_id').on('select2:select', function (e) {
        id = e.params.data.id;

        //if ($('#business_unit_group_id').val() !== null || $('#business_unit_division_id').val() !== null || $('#Search_business_unit_departement_id').val() !== null) {

        //    $('#superior').setcombobox({
        //        data: { 'rows': 100, Search_business_unit_group_id: $('#business_unit_group_id').val(), Search_business_unit_subgroup_id: $('#business_unit_subgroup_id').val(), Search_business_unit_division_id: $('#business_unit_division_id').val(), Search_business_unit_departement_id: id },
        //        url: ajaxUrlBusinessUnitJobLevel,
        //        searchparam: 'Search_job_title_name',
        //        labelField: 'job_title_name',
        //        valueField: 'id',
        //        searchField: 'job_title_name'
        //    });

        //} else {

        //    alert('Please Input stucture organization correctly');
        //    return false;
        //}

        //ajaxGet(ajaxBussinesUnitJobLevel, { Search_business_unit_id: id }, function (response) {
        //    response = parseJson(response);

        //});
    });



});



