var pageheader = "";

$(function () {
 
    $('#education_level_id').setcombobox({
        data: { 'rows': 100 },
        url: educationLevelURL,
        maxItems: 0,
        searchparam: 'Search_education_name',
        labelField: 'education_name',
        valueField: 'id'
    });

    $('#education_institusion_id').setcombobox({
        data: { 'rows': 100, 'Search_institution_type': 1 },
        url: InstitusionURL,
        maxItems: 0,
        searchparam: 'Search_institution_name',
        labelField: 'institution_name',
        valueField: 'id'
    });

    $('#start_education').datepicker({
        format: " yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    $('#end_education').datepicker({
        format: " yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
});

$('#education_institusion_id').change(function () {
    if ($('#education_institusion_id').val() === '1064750129790792197') {
        $('#institution_other').show();
        var html = '<label for="education_institution_name">Add Option Other Institution Name : *</label>' +
            '<input class="form-control form-control-sm m-input m-input--square" name="education_institusion_id_other" id="education_institusion_id_other" maxlength="50" type="text">';
        $('#institution_other').append(html);
    } else {
        $('#institution_other').empty();
        $('#institution_other').hide();
    }

});
