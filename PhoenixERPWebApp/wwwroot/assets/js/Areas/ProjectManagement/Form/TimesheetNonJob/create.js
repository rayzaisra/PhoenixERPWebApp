 
var webUrl = WEB + "/ProjectManagement/TimesheetNonJob";
var ajaxUrl = API + "/ProjectManagement/TimesheetNonJob";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "TimesheetNonJobFormCreate";
var ajaxUrlListCompany = API + "/ProjectManagement/TimesheetNonJob/companylist";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListCurrency= API_GENERAL + "/Currency/List";
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var xtaskno = 0;
var xsubtaskno = 0;
var categorylist = [];

fnTimesheetNonJob = {
    addtask: function () {
        xtaskno++;
        vstr = '<tr class="tsk' + xtaskno + '"><td><a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air" onclick="fnTimesheetNonJob.removetask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a></td><td  ><input class="xtext form-control-sm m-input m-input--square" required id="taskname' + xtaskno + '" required type="text"   ></td>';
        vstr += '<td><select required style="width:100%"  class="xcategory form-control form-control-sm m-input m-input--square" id="category' + xtaskno + '" ></select></td>';
        vstr += '</tr > ';
        $("#" + xform + " #tableTimesheetNonJob tbody").append(vstr);
 

        $('#category' + xtaskno).select2({
            data: categorylist,

            maximumSelectionLength: 1,
            placeholder: "choose....",
            allowClear: true,
            dropdownCssClass: 'bigselect'
        })
    },
   
    removetask: function (id) {

        $("tr.tsk" + id).remove();
 
    }
}
 
function postdata(cdata) {
    ajaxPost(ajaxUrlCreate, cdata, function (response) {
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            redirecttolink(webUrl)
        } else {
            DangerNotif(response.message);
        }

    });
}
function getdata() {
    var detaildata = [];
    var xtotal = 0;
    for (var ic = 0; ic <= xtaskno; ic++) {
        
        var xcountx = $("#" + xform +" #taskname" + ic).length;
        if (xcountx > 0) {
            var task_name = $("#" + xform + " #taskname" + ic).val();
 
            var foundtask = detaildata.find(x => x.task_name === task_name);
            if (foundtask) {
                swal("Cancelled", "Found Duplicate Task", "error");
                return false;
            }
             detaildata.push({
                'task_name':task_name,
                 'category_id': $("#" + xform + " #category" + ic).select2('data')[0].id,
                 'category_name': $("#" + xform + " #category" + ic).select2('data')[0].text,
            });
  
        }
    }
    var currentdata = {
        'description': $("#" + xform + " #description").val(),
        'business_unit_id': $("#" + xform + " #business_unit_id").select2('data')[0].id,
        'name': $("#" + xform + " #name").val(),
        'start_date': $("#" + xform + " #start_date").val(),
        'end_date': $("#" + xform + " #end_date").val(),
        'detail': detaildata
    }
    return currentdata;
}

$(function () {
    $(".datepicker").setdatepicker();
    ajaxGet(categoryurl, { 'rows': 1000 }, function (response) {
        categorylist = [{ 'id': null, 'text': "........" }];
        $(response.rows).each(function (index, item) {
            categorylist.push({
                'id': item.account_number,
                'text':item.account_name
            });

        });

    });
    var optionsForm = GetOptionsForm(function () {

        var xret = $("#" + xform).valid();
        var cdata = getdata();
        if (cdata) {
            postdata(cdata);
        }
        
        return false;
    }, function (response, statusText, xhr, $form) {
 

        });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_gn);
    $('#business_unit_id').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: 900 },
        url: ajaxUrlListOrganization,
        searchparam: 'search',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#currency').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListCurrency,
        searchparam: 'search',
        labelField: 'currency_name',
        valueField: 'currency_code'
    });      
});
