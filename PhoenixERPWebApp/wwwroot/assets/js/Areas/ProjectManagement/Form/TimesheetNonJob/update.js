
var webUrl = WEB + "/ProjectManagement/TimesheetNonJob";
var ajaxUrl = API + "/ProjectManagement/TimesheetNonJob";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlGet = ajaxUrl + "/get";
var xform = "TimesheetNonJobFormUpdate";
var ajaxUrlListCompany = API + "/ProjectManagement/TimesheetNonJob/companylist";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListCurrency = API_GENERAL + "/Currency/List";
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var xtaskno = 0;
var categorylist = [];

fnTimesheetNonJob = {
    initform: function () {

        fnPanel.startprocess();
        fnPanel.startprocess();
        $(".datepicker").setdatepicker();
        ajaxGet(categoryurl, { 'rows': 1000 }, function (response) {
            fnPanel.endprocess();
            categorylist = [{ 'id': null, 'text': "........" }];
            $(response.rows).each(function (index, item) {
                categorylist.push({
                    'id': item.account_number,
                    'text': item.account_name
                });

            });
            ajaxGet(ajaxUrlGet + "/" + RecordID, {}, function (responsedata) {
                fnPanel.endprocess();
                if (responsedata.data) {
                    var edata = responsedata.data;
                    FormTextByData(edata, xform);
                    $(edata.detail).each(function (index, item) {
                        fnTimesheetNonJob.addtask(item)
                    });



 
                    if (edata.business_unit_id) {
                        $("#" + xform+' #business_unit_id').append('<option value="' + edata.business_unit_id + '" selected>' + edata.business_unit_name + '</option>');
                    }
                    if (edata.currency) {
                        $("#" + xform + ' #currency').append('<option value="' + edata.currency + '" selected>' + edata.currency_name + '</option>');
                    }
                } else {
                    DangerNotif("No Data Found")
                    convault(null, WEB + "/projectmanagement/TimesheetNonJob/");
                }
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
        setFormAction(xform, ajaxUrlUpdate);
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
    },
    addtaskbutton: function () {
        fnTimesheetNonJob.addtask();
    },
    addtask: function (xval) {
        var taskdata = {
            'price': "",
            'task_name':""
        };
        if (xval) {
            taskdata = {
                'category_id': xval.category_id !== null ? xval.category_id : "",
                'category_name': xval.category_name !== null ? xval.category_name : "",
                'task_name': xval.task_name !== null ? xval.task_name : ""
            };
        }

        xtaskno++;
 
        vstr = '<tr class="tsk' + xtaskno + '"><td><a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air" onclick="fnTimesheetNonJob.removetask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a></td><td colspan="7"><input class="xtext form-control-sm m-input m-input--square" required id="taskname' + xtaskno + '" required type="text" value="' + taskdata.task_name + '"  ></td>';
        vstr += '<td><select required style="width:100%"  class="xcategory form-control form-control-sm m-input m-input--square" id="category' + xtaskno + '" ></select></td>';
        vstr += '</tr > ';
        $("#" + xform + " #tableTimesheetNonJob tbody").append(vstr);
        $('#category' + xtaskno).select2({
            data: categorylist,
            maximumSelectionLength: 1,
            placeholder: "choose....",
            allowClear: true,
            dropdownCssClass: 'bigselect'
        });
        $('#category' + xtaskno).val(taskdata.category_id).trigger('change');


    },
    
    removetask: function (id) {

        $("tr.tsk" + id).remove();
 
    }
}
$(document).on('keyup', ".xcount", function () {
 
});
function postdata(cdata) {
    ajaxPost(ajaxUrlUpdate, cdata, function (response) {
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_updatesuccess);
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

        var xcountx = $("#" + xform + " #taskname" + ic).length;
        if (xcountx > 0) {
            var task_name = $("#" + xform + " #taskname" + ic).val();

            var foundtask = detaildata.find(x => x.task_name === task_name);
            if (foundtask) {
                swal("Cancelled", "Found Duplicate Task", "error");
                return false;
            }
 
            detaildata.push({
                'task_name': task_name,
                'category_id': $("#" + xform + " #category" + ic).select2('data')[0].id,
                'category_name': $("#" + xform + " #category" + ic).select2('data')[0].text
            });

        }
    }
    var currentdata = {
        'id': $("#" + xform + " #id").val(),
        'description': $("#" + xform + " #description").val(),
        'business_unit_id': $("#" + xform + " #business_unit_id").select2('data')[0].id,
        'name': $("#" + xform + " #name").val(),
        'start_date': $("#" + xform + " #start_date").val(),
        'end_date': $("#" + xform + " #end_date").val(),
        'detail': detaildata,
    }
    return currentdata;
}
 
$(function () {

    fnTimesheetNonJob.initform();

   
    
    
});
