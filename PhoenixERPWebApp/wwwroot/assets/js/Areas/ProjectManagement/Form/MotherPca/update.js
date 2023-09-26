
var webUrl = WEB + "/ProjectManagement/MotherPca";
var ajaxUrl = API + "/ProjectManagement/MotherPca";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlGet = ajaxUrl + "/get";
var xform = "MotherPcaFormUpdate";
var ajaxUrlListCompany = API + "/ProjectManagement/MotherPca/companylist";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListCurrency = API_GENERAL + "/Currency/List";
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var xtaskno = 0;
var xsubtaskno = 0;
var categorylist = [];

fnMotherPca = {
    initform: function () {
        fnPanel.startprocess();
        fnPanel.startprocess();
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
                        fnMotherPca.addtask(item)
                        $(item.sub_task).each(function (index, itemsub) {
                            fnMotherPca.addsubtask(xtaskno, itemsub)
                        });
                    });



 
                    if (edata.business_unit_id) {
                        $("#" + xform+' #business_unit_id').append('<option value="' + edata.business_unit_id + '" selected>' + edata.business_unit_name + '</option>');
                    }
                    if (edata.currency) {
                        $("#" + xform + ' #currency').append('<option value="' + edata.currency + '" selected>' + edata.currency_name + '</option>');
                    }
                } else {
                    DangerNotif("No Data Found")
                    convault(null, WEB + "/projectmanagement/MotherPca/");
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
        fnMotherPca.addtask();
        fnMotherPca.addsubtask(xtaskno);
    },
    addtask: function (xval) {
        var taskdata = {
            'price': "",
            'task_name':""
        };
        if (xval) {
            taskdata = {
                'price': xval.price !== null ? xval.price : 0,
                'task_name': xval.task_name !== null ? xval.task_name : ""
            };
        }

        xtaskno++;
        vstr = '<tr class="tsk' + xtaskno + '"><td><a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air" onclick="fnMotherPca.removetask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a><a class="btn btn-outline-success m-btn m-btn--icon btn-sm m-btn--air " onclick="fnMotherPca.addsubtask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-plus"></i></a></td><td colspan="3"><input class="xtext form-control-sm m-input m-input--square" required id="taskname' + xtaskno + '" required type="text" value="' + taskdata.task_name + '"  ></td>';
        vstr += '<td ><input  id="taskcount' + xtaskno + '" class="xcount form-control-sm m-input m-input--square"  readonly type="text"  value="' + taskdata.price + '" ></td></tr > ';
        $("#" + xform + " #tableMotherPca tbody").append(vstr);
        


    },
    addsubtask: function (stno, xval) {
        var subtaskdata = {
            'brand': "",
            'category_id': "",
            "category_name": "",
            "price": "",
            "quantity": "",
            "remarks": "",
            "subbrand": "",
            "subtask": "",
            "total": "",
        };
        if (xval) {
            subtaskdata = xval;
        }
        xsubtaskno++;
        vstr = '<tr  class="tsk' + stno + '"><td ><a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air " onclick="fnMotherPca.removesubtask(this,\'' + stno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a></td><td class="xrght"><i class="fa fa-arrow-circle-right"></i></td><td><input class="xtext form-control-sm m-input m-input--square " id="subtaskname' + xsubtaskno + '" value="' + subtaskdata.subtask + '" required type="text" ></td>';

        vstr += '<td><input  value="' + subtaskdata.quantity + '" required class="xcount form-control-sm m-input m-input--square" id="quantity' + xsubtaskno + '" type="text" ></td>';
        vstr += '<td ><input  value="' + subtaskdata.price + '" class="xcount form-control-sm m-input m-input--square subtask' + stno + '" id="price' + xsubtaskno + '" data-id="' + xsubtaskno + '" required type="text" ></td></tr > ';
        $(vstr).insertAfter($('.tsk' + stno).last());
        $('.xcount').mask('000,000,000,000,000', { reverse: true });
        hitungtotal();

    },
    removesubtask: function (id, trid) {
        var xcount = $(".tsk" + trid).length;

        if (xcount <= 2) {

            swal("Cancelled", "min 1 sub task!", "error");
        } else {
            $(id).closest("tr").remove();
        }
        hitungtotal()
    },
    removetask: function (id) {

        $("tr.tsk" + id).remove();
        hitungtotal()
    }
}
$(document).on('keyup', ".xcount", function () {
    hitungtotal();
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

        var xcountx = $("#" + xform + " #taskcount" + ic).length;
        if (xcountx > 0) {
            var task_name = $("#" + xform + " #taskname" + ic).val();
            var subtasklist = [];
            var foundtask = detaildata.find(x => x.task_name === task_name);
            if (foundtask) {
                swal("Cancelled", "Found Duplicate Task", "error");
                return false;
            }
            vttl = 0;
            var xcondition = false;
            $(".subtask" + ic).each(function (index, item) {
                var xtid = $(this).attr("data-id");
                var subtask = $("#" + xform + " #subtaskname" + xtid).val();
                var foundsubtask = subtasklist.find(x => x.subtask === subtask);
                if (foundsubtask) {

                    xcondition = true;
                    return false;
                }

                var qty = getnumber($("#" + xform + " #quantity" + xtid).val())
                var xprice = getnumber($(this).val());
                var ctotal = xprice * qty;
                vttl += ctotal;

                data = {
                    'subtask': subtask,
                    'quantity': qty,
                    'price': xprice,
                    'total': ctotal
                };
                subtasklist.push(data);
            });
            if (xcondition) {
                swal("Cancelled", "Found Duplicate Sub Task in same task", "error");
                return false;
            }
            xtotal += vttl;
            detaildata.push({
                'task_name': task_name,
                'price': vttl,
                'sub_task': subtasklist
            });
            $("#" + xform + " #taskcount" + ic).val(thousandsep(vttl));
        }
    }
    var currentdata = {
        'id': $("#" + xform + " #id").val(),
        'code': "-",
        'description': $("#" + xform + " #description").val(),
        'business_unit_id': $("#" + xform + " #business_unit_id").select2('data')[0].id,
        'currency': $("#" + xform + " #currency").select2('data')[0].id,
        'name': $("#" + xform + " #name").val(),
        'detail': detaildata,
        'total': xtotal
    }
    return currentdata;
}
function hitungtotal() {
    var xtotal = 0;
    for (var ic = 0; ic <= xtaskno; ic++) {
        var xcountx = $("#" + xform + " #taskcount" + ic).length;
        if (xcountx > 0) {
            vttl = 0;
            $(".subtask" + ic).each(function (index, item) {
                var xtid = $(this).attr("data-id")
                var qty = getnumber($("#" + xform + " #quantity" + xtid).val())
                vttl += getnumber($(this).val()) * qty;

            });
            xtotal += vttl;
            $("#" + xform + " #taskcount" + ic).val(thousandsep(vttl));
        }
    }
    $("#" + xform + " #totalAmount").text(thousandsep(xtotal));
    $("#" + xform + " #total").val(thousandsep(xtotal));
}
$(function () {

    fnMotherPca.initform();

   
    
    
});
