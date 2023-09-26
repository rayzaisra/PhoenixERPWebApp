var pageheader = "";

$(function () {
    
    $('#family_status_id').setcombobox({
        data: { 'rows': 100 },
        url: familyStatusURL,
        maxItems: 0,
        searchparam: 'Search_family_status_name',
        labelField: 'family_status_name',
        valueField: 'id'
    });

    $('#family_education_level_id').setcombobox({
        data: { 'rows': 100 },
        url: educationLevelURL,
        maxItems: 0,
        searchparam: 'Search_education_name',
        labelField: 'education_name',
        valueField: 'id'
    });

    $('#family_married_status_id').setcombobox({
        data: { 'rows': 100 },
        url: marriedURL,
        maxItems: 0,
        searchparam: 'Search_married_name',
        labelField: 'married_name',
        valueField: 'id'
    });


    $('#family_institution_id').setcombobox({
        data: { 'rows': 100,'Search_institution_type':1},
        url: InstitusionURL,
        maxItems: 0,
        searchparam: 'Search_institution_name',
        labelField: 'institution_name',
        valueField: 'id'

    });

    $('#family_religion_id').setcombobox({
        data: { 'rows': 100 },
        url: religionURL,
        maxItems: 0,
        searchparam: 'Search_religion_name',
        labelField: 'religion_name',
        valueField: 'id'
    });


    
});

$('#family_institution_id').change(function () {
    if ($('#family_institution_id').val() === '1064750129790792197') {
        $('#family_institution_other').show();
        var html = '<label for="education_institution_name">Add Option Other Institution Name : *</label>' +
            '<input class="form-control form-control-sm m-input m-input--square" name="family_education_institusion_id_other" id="family_education_institusion_id_other" maxlength="50" type="text">';
        $('#family_institution_other').append(html);
    } else {
        $('#family_institution_other').empty();
        $('#family_institution_other').hide();
    }

});

