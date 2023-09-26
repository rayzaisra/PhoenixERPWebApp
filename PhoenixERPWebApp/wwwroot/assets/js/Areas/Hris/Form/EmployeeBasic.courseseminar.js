var pageheader = "";

$(function () {
   
    $('#course_institute_id').setcombobox({
        data: { 'rows': 100, 'Search_institution_type': 2 },
        url: InstitusionURL,
        maxItems: 0,
        searchparam: 'Search_institution_name',
        labelField: 'institution_name',
        valueField: 'id'
    });

    //$('#expired_certification').datepicker({
    //    format: " yyyy",
    //    viewMode: "years",
    //    minViewMode: "years"
    //});

    //$('#start_training').datepicker({
    //    format: " yyyy",
    //    viewMode: "years",
    //    minViewMode: "years"
    //});

    //$('#end_training').datepicker({
    //    format: " yyyy",
    //    viewMode: "years",
    //    minViewMode: "years"
    //});
});

$('#course_institute_id').change(function () {
    if ($('#course_institute_id').val() === '1064750129790792198') {
        $('#course_institution_other').show();
        var html = '<label for="education_institution_name">Add Option Other Course Institution Name : *</label>' +
            '<input class="form-control form-control-sm m-input m-input--square" name="course_institusion_id_other" id="course_institusion_id_other" maxlength="50" type="text">';
        $('#course_institution_other').append(html);
    } else {
        $('#course_institution_other').empty();
        $('#course_institution_other').hide();
    }

});
