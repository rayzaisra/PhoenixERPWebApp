var pageheader = "";


$(function () {

    $('#emergency_family_status_id').setcombobox({
        data: { 'rows': 100 },
        url: familyStatusURL,
        maxItems: 0,
        searchparam: 'Search_family_status_name',
        labelField: 'family_status_name',
        valueField: 'id'
    });
    
});
