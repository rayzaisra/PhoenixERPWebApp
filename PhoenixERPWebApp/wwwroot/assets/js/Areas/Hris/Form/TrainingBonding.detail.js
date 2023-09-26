var ajaxUrlTableList = API + "Hris/EmployeeBasicInfo/List";
var ajaxUrlEmployee = API + "General/BusinessUnit";
var ajaxUrlListEmployee = ajaxUrlEmployee + "/ListWithParent";


var tableList = "EmployeeList";
var formSearch = "EmployeeListFormSearch";
var modalSearch = "EmployeeListModalSearch";
var pageheader = "";
var MppNewRealData = [];

var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    },
    {
        "data": "name_employee",
        "title": "<span data-args='Employee Name'>Employee Name</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            datastring = row.name_employee;

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<span data-args='Job Title'>Job Title</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            datastring = row.job_title;

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<span data-args='Grade'>Grade</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            datastring = row.grade;

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<center><span data-args='Division'>Division</span></center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var datastring = '';
            datastring = row.division;

            return datastring;
        }
    },
    {
        "data": null,
        "title": "<center><span class='translate'  data-args='app-action'>Actions</span></center>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            var retval = '';
            retval += "<center><a href='#' class='btn btn-primary' onClick=\"PushRowtoTableTrainingReq('" + row.id + "' , '" + row.name_employee + "' , '" + row.job_title + "' , '" + row.grade + "' , '" + row.division + "')\">Select</a></center>";
            return retval;

        }
    }
];



$(function () {

    InitTable(tableList, ajaxUrlTableList, null, columnsIndex, 'id', 'asc');
    setcolvis(tableList, columnsIndex);
    initlanguage(lang_hr);
 

   
    var id = null;
    var datadef = "token";

    $('#group').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: 1000 },
        url: ajaxUrlListEmployee,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#subgroup').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlListEmployee,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#division').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlListEmployee,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#departement').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlListEmployee,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });

    $('#group').on('select2:select', function (e) {
        id = e.params.data.id;
        datadef = 900;
        $('#subgroup').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
            url: ajaxUrlListEmployee,
            maxItems: 0,
            searchparam: 'unit_name',
            labelField: 'unit_name',
            valueField: 'id'
        });
    });


    $('#subgroup').on('select2:select', function (e) {
        id = e.params.data.id;
        $('#division').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: 800, Search_parent_unit: id },
            url: ajaxUrlListEmployee,
            maxItems: 0,
            searchparam: 'unit_name',
            labelField: 'unit_name',
            valueField: 'id'
        });
    });

    $('#division').on('select2:select', function (e) {
        id = e.params.data.id;
        $('#departement').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: 700, Search_parent_unit: id },
            url: ajaxUrlListEmployee,
            maxItems: 0,
            searchparam: 'unit_name',
            labelField: 'unit_name',
            valueField: 'id'
        });
    });

    $('#departement').on('select2:select', function (e) {
        id = e.params.data.id;
    });
    

  
});

function PushRowtoTableTrainingReq(id, name_employee, jobtitle, grade, division) {

    var text = "Are You Sure Want To Select This Employee?";
    swal({
        title: "Confirmation",
        text: text,
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (e) {
        if (e.value) {
            CheckAndAddRow(id, name_employee, jobtitle, grade, division);
            //var markup = "<tr id=" + id + "><td>" + name_employee + "</td><td>" + jobtitle + "</td><td>" + grade + "</td><td>" + division + "</td><td><center><a href=\"#\" class=\"btn btn-danger\" onClick=\"DeleteLogic('" + id + "')\">Delete</a></center></td><td style='display: none;'>" + id + "</td><td style='display: none;'></tr>";
            //$("table#dt_basic_job_posting tbody").append(markup);
            //$("#TrainingRequisitionChooseEmployee").modal('hide');
        }
     });
}

function DeleteTableTrainingReq(id) {
    getData();
   // $('table#dt_basic_job_posting tr#' + id + '').remove();
}

function AddRowNoData() {
    var markup = "<tr id='no_data'><td colspan='5'>No Data</td></td></tr>";
    $("table#dt_basic_job_posting tbody").append(markup);

}

function Deleteaja(id) {
    $('table#dt_basic_job_posting tr#' + id + '').remove();
}

function DeleteLogic(id) {
    getData();
    if (MppNewRealData.length === 1) {
        AddRowNoData();
    } else {
        Deleteaja(id);
        getData();
        $('#tmpParticipant').empty();
        var tmpmpno = 0;
        $.each(MppNewRealData, function (index, value) {
            $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][employee_basic_info_id]" value="' + value.id + '"  >');
            $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][name_employee]" value="' + value.name_employee + '"  >');
            $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][job_title_name]" value="' + value.job_title + '"  >');
            $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][job_grade_name]" value="' + value.grade + '"  >');
            $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][division]" value="' + value.division + '"  >');
            tmpmpno++;
        });
    }
    $('table#dt_basic_job_posting tr#' + id + '').remove();
}

function CheckAndAddRow(id, name_employee, job_title, grade, division,mode="withaction") {

            Deleteaja('no_data');
            getData();
            // if (RealData.length > 0) {
            tmpData = {
                "id": id,
                "name_employee": name_employee,
                "job_title": job_title,
                "grade": grade,
                "division": division
            };
            MppNewRealData.push(tmpData);
            $('#tmpParticipant').empty();
            var tmpmpno = 0;
            $.each(MppNewRealData, function (index, value) {
                $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][employee_basic_info_id]" value="' + value.id + '"  >');
                $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][name_employee]" value="' + value.name_employee + '"  >');
                $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][job_title_name]" value="' + value.job_title + '"  >');
                $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][job_grade_name]" value="' + value.grade + '"  >');
                $('#tmpParticipant').append('<input type="hidden" name="listParticipantTraining[' + tmpmpno + '][division]" value="' + value.division + '"  >');
           tmpmpno++;
            });

            if (mode != "withaction") {
                AddRowTableWithoutAction(id, name_employee, job_title, grade, division);
            } else {
                AddRowTable(id, name_employee, job_title, grade, division);
            }
        

         $('#TrainingRequisitionChooseEmployee').modal('hide');
}

function AddRowTable(id, name_employee, jobtitle, grade, division) {
    var markup = "<tr id=" + id + "><td>" + name_employee + "</td><td>" + jobtitle + "</td><td>" + grade + "</td><td>" + division + "</td><td><center><a href=\"#\" class=\"btn btn-danger\" onClick=\"DeleteLogic('" + id + "')\">Delete</a></center></td><td style='display: none;'>" + id + "</td></tr>";
    $("table#dt_basic_job_posting tbody").append(markup);
}

function AddRowTableWithoutAction(id, name_employee, jobtitle, grade, division) {
    var markup = "<tr id=" + id + "><td>" + name_employee + "</td><td>" + jobtitle + "</td><td>" + grade + "</td><td>" + division + "</td><td style='display: none;'><center><a href=\"#\" class=\"btn btn-danger\" onClick=\"DeleteLogic('" + id + "')\">Delete</a></center></td><td style='display: none;'>" + id + "</td></tr>";
    $("table#dt_basic_job_posting tbody").append(markup);
} 

function getData() {
    var TableData = '';
    var tmpData = '';
    MppNewRealData = [];
    var rowCount = $('#dt_basic_job_posting >tbody >tr').length;
    // alert(rowCount);
    $('#dt_basic_job_posting tr').each(function (row, tr) {
        console.log(row);
        if ($(tr).find('td:eq(5)').text() !== "") {
            tmpData = {
                "id": $(tr).find('td:eq(5)').text(),
                "name_employee": $(tr).find('td:eq(0)').text(),
                "job_title": $(tr).find('td:eq(1)').text(),
                "grade": $(tr).find('td:eq(2)').text(),
                "division": $(tr).find('td:eq(3)').text()
            };
            MppNewRealData.push(tmpData);
        }
        //TableData = TableData
        //    + $(tr).find('td:eq(0)').text() + ' '  // jobtitle
        //    + $(tr).find('td:eq(1)').text() + ' '  // ttf
        //    + $(tr).find('td:eq(2)').text() + ' '  // status
        //    + $(tr).find('td:eq(3)').text() + ' '  // mppbudget
        //    + $(tr).find('td:eq(4)').text() + ' '  // grade
        //    + $(tr).find('td:eq(5)').text() + ' '  // grade
        //     + '\n';

    });
    //console.log(MppNewRealData);
    //console.log(TableData);
    //$('#dataDetailMpp').val(JSON.stringify(MppNewRealData));
}

//function DeleteTableEmployeeList(id) {
//    $('table#EmployeeList tr#' + id +'').remove();
//}



