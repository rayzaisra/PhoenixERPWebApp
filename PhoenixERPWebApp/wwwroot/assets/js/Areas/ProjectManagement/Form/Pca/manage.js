 
var webUrl = WEB + "/ProjectManagement/Pca";
var ajaxUrl = API + "/ProjectManagement/Pca";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlGet = ajaxUrl + "/get"
var ajaxUrlCreate = ajaxUrl + "/manage";
var ajaxUrlsubmittopce = API + "/ProjectManagement/pca/submitpce";
var xform = "PcaFormCreate";
var ajaxUrlListCompany = API + "/ProjectManagement/Pca/companylist";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlcashadvance = API + "/finance/CashAdvance/AddCashAdvanceWithApprovals";
//var ajaxUrlrfq = API + "/finance/RequestForQuotation/saveasdraft";
var ajaxUrlrfq = API + "/ProjectManagement/pca/createRfq";
var ajaxUrlvendorselect = API + "/finance/CashAdvance/AddCashAdvanceWithApprovals";
var ajaxUrlPca = API + "/ProjectManagement/pca/List";
var ajaxUrlListJob = API_PM + "/Job/List";
var ajaxUrlListRateCard = API_PM + "/RateCard/List";
var ajaxUrlListMotherPca = API_PM + "/MotherPca/List";
var ajaxUrlListCurrency = API_GENERAL + "/Currency/List";
var ajaxUrlListType = API + "/ProjectManagement/TypeOfExpense/List";
var ajaxUrlShareServices = API + "ProjectManagement/ShareServices/List"
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var ajaxUrlGetRateCard = API_PM + "/RateCard/Get";
var ajaxUrlGetJob = API_PM + "/Job/GetTaskList";
var ajaxUrlGetTempateApprovalLink = API + "General/TmTemplateApproval/List"; 
var xtaskno = 0;
var xsubtaskno = 0;
var ytaskno = 0;
var ysubtaskno = 0;
var categorylist = [];
var departementlist = [];
var unitlist = [];
var alltask = [];
var jobtask = [];
var ratecardtask = [];
var dropdowntask;
var rfsub = [];
var current_job_id = "";
var current_brand = "";
var tableList = "PcaListApprover";


Date.prototype.sformat = function () {
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();

    return [this.getFullYear(),
    (mm > 9 ? '' : '0') + mm,
    (dd > 9 ? '' : '0') + dd
    ].join('/');
};
fnPca = {
    addnewtask: function () {
        task_name = $("#task_name").val();
        fnPca.addjobtask(task_name, 'newtask');
        $("#task_name").val("");
        dropdowntask.hide();
    },
    removetask: function (task_name) {
        alltask = $.grep(alltask, function (e) {
            return e.task_name !== task_name;
        });
        fnPca.generatetable();

    },
    editsubtask: function (task_name) {
        current_task_name = task_name;
        var foundtask = alltask.find(x => x.task_name === task_name);
        $('#taskTable tbody').empty();
        vstr = "";
        ytaskno = 0;
        treeno = 0;
        subtreeno = 0;
        ysubtaskno = 0;
        $(foundtask.sub_task).each(function (indexsub, itemsub) {
            var vsel = "";
            var vcheck = "";
            if (itemsub.pick == 1) {
                vcheck = "checked";
            }
            if (itemsub.category_id) {
                vsel = '<option value="' + itemsub.category_id + '" selected>' + itemsub.category_name +'</option>';
            }
            vstr += '<tr class="treegrid-' + treeno + '-' + subtreeno + ' treegrid-parent-' + treeno + '" ><td><label><input type="checkbox" name="pick' + ysubtaskno + '" ' + vcheck+'> ' + itemsub.subtask + '</label><input type="hidden" id="subtask' + ysubtaskno + '" value="' + itemsub.subtask + '"></td>';
            vstr += '<td><select required style="width:100%"  class="xcategory form-control form-control-sm m-input m-input--square" id="category' + ysubtaskno + '" >' + vsel+'</select></td>';
            vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="remarks' + ysubtaskno + '" type="text"  value="' + itemsub.remarks + '"></td>';
            vstr += '<td><input required class="xtext form-control-sm m-input m-input--square" id="brand' + ysubtaskno + '" type="text" value="' + itemsub.brand + '" ></td>';
            vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="subbrand' + ysubtaskno + '" type="text"  value="' + itemsub.subbrand + '" ></td>';
            vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square" id="quantity' + ysubtaskno + '" type="text"  value="' + itemsub.quantity + '" ></td>';
            vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square ysubtask' + ytaskno + '" data-id="' + ysubtaskno + '" id="price' + ysubtaskno + '" type="text"  value="' + itemsub.price + '" ></td>';
            vstr += '<td><input readonly class="xcounttask form-control-sm m-input m-input--square " id="total' + ysubtaskno + '"  type="text"  value="' + itemsub.total + '" ></td>';
            vstr += '</tr > ';
            ysubtaskno++;
            subtreeno++
        });
        $('#taskTable tbody').append(vstr);
        $('.xcounttask').mask('000,000,000,000,000', { reverse: true });
        $('.xcategory').select2({
            data: categorylist,

            maximumSelectionLength: 1,
            placeholder: "choose....",
            allowClear: true,
            dropdownCssClass: 'bigselect'
        });
        $('.tree').treegrid({
            expanderExpandedClass: 'fa fa-minus',
            expanderCollapsedClass: 'fa fa-plus'
        });
        hitungtotaltask();
        $("#taskbox").modal('show');
    },
    addjobtask: function (task_name, type) {
        dropdowntask.hide();
        var foundtask = alltask.find(x => x.task_name === task_name);
        if (foundtask) {
            swal("Cancelled", "Found Duplicate Task", "error");
            return false;
        }
        var foundtaskadd = {};
        var taskprice = 0;
        if (type === 'jobtask') {
            foundtaskadd = jobtask.find(x => x.task_name === task_name);

            subtasklist = [];
            taskprice = 0;
            $(foundtaskadd.sub_task).each(function (index, item) {
                data = {
                    'subtask': item.subtask,
                    'category_id': '',
                    'category_name': '',
                    'remarks': '',
                    'brand': '',
                    'subbrand': '',
                    'quantity': 1,
                    'price': 0,
                    'total':0,
                    'pick': 1
                };
                subtasklist.push(data)
            });
            alltask.push({
                'task_name': task_name,
                'departement': '312312312',
                'quantity': 1,
                'unit': '',
                'price': taskprice,
                'ppn': 0,
                'asf': 0,
                'pph': 0,
                'cost': taskprice,
                'percentage': 0,
                'margin': 0,
                'total': taskprice,
                'sub_task': subtasklist
            });

        } else if (type === 'ratecard') {
            foundtaskadd = ratecardtask.find(x => x.task_name === task_name);
 
            subtasklist = [];
            taskprice = 0;
            $(foundtaskadd.sub_task).each(function (index, item) {
                data = {
                    'subtask': item.subtask,
                    'category_id': item.category_id,
                    'category_name': item.category_name,
                    'remarks': item.remarks,
                    'brand': item.brand,
                    'subbrand': item.subbrand,
                    'quantity': item.quantity,
                    'price': item.price,
                    'total': item.total,
                    'pick': 1
                };
                taskprice += item.total;
                subtasklist.push(data)
            });
               alltask.push({
                   'task_name': task_name,
                   'departement': '',
                    'quantity': 1,
                    'unit': '',
                   'price': taskprice,
                   'ppn': 0,
                   'asf': 0,
                   'pph': 0,
                   'cost': taskprice,
                    'percentage': 0,
                    'margin': 0,
                   'total': taskprice,
                    'sub_task': subtasklist
                });
        } else {
            taskprice = 0;
            subtasklist = [];
            alltask.push({
                'task_name': task_name,
                'departement': '',
                'quantity': 1,
                'unit': '',
                'price': taskprice,
                'cost': taskprice,
                'percentage': 0,
                'margin': 0,
                'total': taskprice,
                'sub_task': subtasklist
            });
        }
        
        fnPca.generatetable();
        hitungtotal();

    },
    showtaskbox: function (task_name) {

        var job_id = $("#job_id").val();
        if (job_id === null) {
            swal("Cancelled", "Please Choose Job", "error");
            return false;
        }
        var RateCardId = $("#rate_card_id").val();

        if (RateCardId) {
            ajaxGet(ajaxUrlGetRateCard + '/' + RateCardId, {}, function (response) {

                if (response.data) {
                    var xdata = response.data.detail;
                    rfsub = xdata;
                    $('#RatecardTable tbody').empty();
                    vstr = "";
                    vno = 0;
                    $(xdata).each(function (index, item) {
                        vno++;
                        var taskno = vno;
                        vshow = true;
                        if (task_name) {
                            if (task_name !== item.task_name) vshow = false;
                            $(".showrfqn").show();
                            $(".showrfq").hide();
                        } else {
                            $(".showrfq").show();
                            $(".showrfqn").hide();
                        }
                        if (vshow ) {
                            vstr += '<tr class="treegrid-' + vno + '"><td class="task_name" colspan="7">' + item.task_name + '</td><td>' + item.price + '</td></tr>';
                            $(item.sub_task).each(function (index, itemsub) {
                                vno++;
                                vstr += '<tr class="treegrid-' + vno + ' treegrid-parent-' + taskno + '" ><td><label><input id="subtask" type="checkbox" value="' + itemsub.subtask + '"> ' + itemsub.subtask + '</label></td><td><span class="itemsub_category">' + itemsub.category_name + '</span></td><td>' + itemsub.remarks + '</td><td><span class="itemsub_brand">' + itemsub.brand + '</span></td><td><span class="itemsub_sub_brand">' + itemsub.subbrand + '</td><td class="rcol itemsub_quantity">' + thousandsep(itemsub.quantity) + '</td><td class="rcol itemsub_price">' + thousandsep(itemsub.price) + '</td><td class="rcol itemsub_total">' + thousandsep(itemsub.total) + '</td></tr>';

                            });
                        }
                    });
                    $('#RatecardTable tbody').append(vstr);
                    $('.tree').treegrid({
                        expanderExpandedClass: 'fa fa-minus',
                        expanderCollapsedClass: 'fa fa-plus'
                    });
                }
            });
            $("#ratecardbox").modal('show');
        } else {

            ajaxGet(ajaxUrlGetJob + '/' + job_id, {}, function (response) {

                if (response.rows) {
                    var xdata = response.rows;
                    rfsub = xdata;
                    $('#taskTable tbody').empty();
                    vstr = "";
                    ytaskno = 0;
                    treeno = 0;
                    subtreeno = 0;
                    ysubtaskno = 0;
                    $(xdata).each(function (index, item) {
                        vshow = true;
                        if (task_name) {
                            if (task_name !== item.task_name) vshow = false;
                            $(".showrfqn").show();
                            $(".showrfq").hide();
                        } else {
                            $(".showrfq").show();
                            $(".showrfqn").hide();
                        }
                        if (vshow) {
                            vstr += '<tr class="treegrid-' + treeno + '"><td colspan="7">' + item.task_name + '</td><td><input readonly class="xcounttask form-control-sm m-input m-input--square " id="tasktotal' + ytaskno + '"  type="text" ><input type="hidden" id="task_name' + ytaskno + '" value="' + item.task_name + '"></td></tr>';
                            $(item.sub_task).each(function (indexsub, itemsub) {

                                vstr += '<tr class="treegrid-' + treeno + '-' + subtreeno + ' treegrid-parent-' + treeno + '" ><td><label><input type="checkbox"> ' + itemsub.subtask + '</label><input type="hidden" id="subtask' + ysubtaskno + '" value="' + itemsub.subtask + '"></td>';
                                vstr += '<td><select required style="width:100%"  class="xcategory form-control form-control-sm m-input m-input--square" id="category' + ysubtaskno + '" ></select></td>';
                                vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="remarks' + ysubtaskno + '" type="text" ></td>';
                                vstr += '<td><input required class="xtext form-control-sm m-input m-input--square" id="brand' + ysubtaskno + '" type="text" ></td>';
                                vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="subbrand' + ysubtaskno + '" type="text" ></td>';
                                vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square" id="quantity' + ysubtaskno + '" type="text" ></td>';
                                vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square ysubtask' + ytaskno + '" data-id="' + ysubtaskno + '" id="price' + ysubtaskno + '" type="text" ></td>';
                                vstr += '<td><input readonly class="xcounttask form-control-sm m-input m-input--square " id="total' + ysubtaskno + '"  type="text" ></td>';
                                vstr += '</tr > ';
                                ysubtaskno++;
                                subtreeno++
                            });
                        }
                        treeno++;
                        ytaskno++;

                    });

                    $('#taskTable tbody').append(vstr);
                    $('.xcounttask').mask('000,000,000,000,000', { reverse: true });
                    $('.xcategory').select2({
                        data: categorylist,

                        maximumSelectionLength: 1,
                        placeholder: "choose....",
                        allowClear: true,
                        dropdownCssClass: 'bigselect'
                    })
                    $('.tree').treegrid({
                        expanderExpandedClass: 'fa fa-minus',
                        expanderCollapsedClass: 'fa fa-plus'
                    });
                }
            });
            $("#taskbox").modal('show');

        }





    },
    generatetable: function () {
        $("#" + xform + " #tablePca tbody").empty();
        vstr = '';
        xtaskno=0;
        $(alltask).each(function (index, item) {
            xtaskno++;

            //if (item.departement)

            vstr += '<tr class="tsk' + xtaskno + '">';
            vstr += '<input id="task_id' + xtaskno + '" value="' + item.id + '" type="hidden" >';
            vstr += '<td><select class="departement" id="departement' + xtaskno + '" style="width:100%"></select></td>';
            vstr += '<td ><div class="padd5">' + item.task_name + '<a href="javascript:void(0)" onclick="fnPca.removetask(\'' + item.task_name + '\')" class="pull-right"><i class="fa fa-minus-circle fa-sm"></i></a><a href="javascript:void(0)" onclick="fnPca.editsubtask(\'' + item.task_name + '\')" class="pull-right"><i class="fa fa-play-circle fa-sm"></i></a></div></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="quantity' + xtaskno + '" value="' + item.quantity +'" type="text" ></td>';
            vstr += '<td><select class="unit" id="unit' + xtaskno +'" style="width:100%"></select></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="price' + xtaskno + '" value="' + item.price + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="ppn' + xtaskno + '" value="' + item.ppn + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="asf' + xtaskno + '" value="' + item.asf + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="pph' + xtaskno + '" value="' + item.pph + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="cost' + xtaskno + '" value="' + item.cost + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="percentage' + xtaskno + '" value="' + item.percentage + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="margin' + xtaskno + '" value="' + item.margin + '" type="text" ></td>';
            vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="total' + xtaskno + '" value="' + item.total + '" type="text" ></td>';
            vstr += '</tr>';
            //vstr += '<td> <a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air" onclick="fnPca.removetask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a> <a class="btn btn-outline-success m-btn m-btn--icon btn-sm m-btn--air " onclick="fnPca.addsubtask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-plus"></i></a></td > <td colspan="7"><input class="xtext form-control-sm m-input m-input--square" required id="taskname' + xtaskno + '" required type="text"   ></td>';
            //vstr += '<td ><input  id="taskcount' + xtaskno + '" class="xcount form-control-sm m-input m-input--square"  readonly type="text"   ></td></tr > ';
        });
        $("#" + xform + " #tablePca tbody").append(vstr);
        $('.xcount').mask('000,000,000,000,000', { reverse: true });
        $('.departement').select2({
            data: departementlist,

            maximumSelectionLength: 1,
            placeholder: "choose....",
            allowClear: true,
            dropdownCssClass: 'bigselect'
        });
        $('.unit').select2({
            data: unitlist,

            maximumSelectionLength: 1,
            placeholder: "choose....",
            allowClear: true,
            dropdownCssClass: 'bigselect'
        });
        hitungtotal();
    },
    addtask: function (da) {
        xtaskno++;
        vstr = '<tr class="tsk' + xtaskno + '">';
        vstr +='<td> <a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air" onclick="fnPca.removetask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a> <a class="btn btn-outline-success m-btn m-btn--icon btn-sm m-btn--air " onclick="fnPca.addsubtask(\'' + xtaskno + '\')" href="javascript:void(0)"><i class="fa fa-plus"></i></a></td > <td colspan="7"><input class="xtext form-control-sm m-input m-input--square" required id="taskname' + xtaskno + '" required type="text"   ></td>';
        vstr += '<td ><input  id="taskcount' + xtaskno + '" class="xcount form-control-sm m-input m-input--square"  readonly type="text"   ></td></tr > ';
        $("#" + xform + " #tablePca tbody").append(vstr);
    },
    addsubtask: function (stno) {
        xsubtaskno++;
        vstr = '<tr  class="tsk' + stno + '"><td ><a class="btn btn-outline-danger m-btn m-btn--icon btn-sm m-btn--air " onclick="fnPca.removesubtask(this,\'' + stno + '\')" href="javascript:void(0)"><i class="fa fa-trash"></i></a></td><td class="xrght"><i class="fa fa-arrow-circle-right"></i></td><td><input class="xtext form-control-sm m-input m-input--square " id="subtaskname' + xsubtaskno + '" required type="text" ></td>';
        vstr += '<td><select required style="width:100%"  class="xcategory form-control form-control-sm m-input m-input--square" id="category' + xsubtaskno + '" ></select></td>';
        vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="remarks' + xsubtaskno + '" type="text" ></td>';
        vstr += '<td><input required class="xtext form-control-sm m-input m-input--square" id="brand' + xsubtaskno + '" type="text" ></td>';
        vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="subbrand' + xsubtaskno + '" type="text" ></td>';
        vstr += '<td><input required class="xcount form-control-sm m-input m-input--square" id="quantity' + xsubtaskno + '" type="text" ></td>';
        vstr += '<td ><input class="xcount form-control-sm m-input m-input--square subtask' + stno + '" id="price' + xsubtaskno + '" data-id="' + xsubtaskno + '" required type="text" ></td></tr > ';
        $(vstr).insertAfter($('.tsk' + stno).last());
        $('.xcount').mask('000,000,000,000,000', { reverse: true });
        hitungtotal();
        $('#category' + xsubtaskno).select2({
            data: categorylist,

            maximumSelectionLength: 1,
            placeholder: "choose....",
            allowClear: true,
            dropdownCssClass: 'bigselect'
        })
    },
    removesubtask: function (id,trid) {
        var xcount = $(".tsk" + trid).length;

        if (xcount<= 2) {

            swal("Cancelled", "min 1 sub task!", "error");
        }else{
        $(id).closest("tr").remove();
        }
        hitungtotal()
    },

}
$(document).on('keyup', ".xcount", function () {
    hitungtotal();
});
$(document).on('keyup', ".xcounttask", function () {
    hitungtotaltask();
});

var current_task_name = "";
function savetaskdetil() {
    var foundtask = alltask.find(x => x.task_name === current_task_name);
    subtasklist = [];
    taskprice = 0;
    ytaskno = 0;
    treeno = 0;
    subtreeno = 0;
    ysubtaskno = 0;
    yform = "TaskForm";
    $(foundtask.sub_task).each(function (index, item) {
        var isChecked = $("#" + yform + " #pick" + ysubtaskno).prop('checked');
        var vpick = 0;
        if (isChecked) {
            vpick = 1;
        }
        data = {
            'subtask': item.subtask,
            'category_id': $("#" + yform + " #category" + ysubtaskno).select2('data')[0].id,
            'category_name': $("#" + yform + " #category" + ysubtaskno).select2('data')[0].text,
            'remarks': $("#" + yform + " #remarks" + ysubtaskno).val(),
            'brand': $("#" + yform + " #brand" + ysubtaskno).val(),
            'subbrand': $("#" + yform + " #subbrand" + ysubtaskno).val(),
            'quantity': $("#" + yform + " #quantity" + ysubtaskno).val(),
            'price': $("#" + yform + " #price" + ysubtaskno).val(),
            'total': $("#" + yform + " #total" + ysubtaskno).val(),
            'pick': vpick
        };
        ysubtaskno++;
        subtreeno++
         
        subtasklist.push(data)
    });
    foundtask.sub_task = subtasklist;
    $("#taskbox").modal('hide');
}


function postdata(cdata) {
    //ajaxPost(ajaxUrlCreate, cdata, function (response) {
    //    response = parseJson(response);

    //    if (response.success) {
    //        SuccessNotif(t_createsuccess);
    //        redirecttolink(webUrl)
    //    } else {
    //        DangerNotif(response.message);
    //    } 
    //});

    if ($('#pca_id').val() != "")
    {
        ajaxPost(ajaxUrlUpdate, cdata, function (response) {



            response = parseJson(response);

            if (response.success) {
                SuccessNotif(t_createsuccess);
                redirecttolink(webUrl)
            } else {
                DangerNotif(response.message);
            }

            
        });

    }
    else {

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
}
function getpcadata() {
    var currentdata = {
        'id': $("#pca_id").val(),
        'type_of_expense_id': $("#type_of_expense_id").val(),

        'code': "-",
        'pca_refrence': $("#refrence_id").val(),
        'description': $("#" + xform + " #description").val(),
        'job_id': $("#" + xform + " #job_id").val(),
        'mother_pca_id': $("#" + xform + " #mother_pca_id").val(),
        'rate_card_id': $("#" + xform + " #rate_card_id").val(),
        'shareservices_id': $("#" + xform + " #shareservices_id").val(),
        'mainservice_category_id': $("#" + xform + " #mainservice_category").val(),
        'termofpayment1': getnumber($("#" + xform + " #termofpayment1").val()),
            'termofpayment2': getnumber($("#" + xform + " #termofpayment2").val()),
                'termofpayment3': getnumber($("#" + xform + " #termofpayment3").val()),
        'currency': $("#" + xform + " #currency").val(),
        'subtotal': getnumber($("#" + xform + " #subtotal").val()),
        'other_fee_name': $("#" + xform + " #other_fee_name").val(),
        'other_fee': getnumber( $("#" + xform + " #other_fee").val()),
        'other_fee_percentage': $("#" + xform + " #other_fee_percentage").val(),
        'vat': getnumber($("#" + xform + " #vat").val()),
        'detail': alltask,
        'total': getnumber($("#" + xform + " #total").val())
    }
    
    return currentdata;
    
}
function getdatatask() {
    var detaildata = [];
    var yform = 'TaskForm';
    var xtotal = 0;
 
    for (var ic = 0; ic <= ytaskno; ic++) {
        var xcountx = $("#" + yform + " #tasktotal" + ic).length;

        if (xcountx > 0) {
            vttl = 0;
            var subtasklist = [];
            $(".ysubtask" + ic).each(function (index, item) {
                var xtid = $(this).attr("data-id");
                var qty = getnumber($("#" + yform + " #quantity" + xtid).val())
                var subtask = $("#" + yform + " #subtask" + xtid).val()
                jttl = getnumber($(this).val()) * qty;
                data = {
                    'subtask': subtask,
                    'category_id': $("#" + yform + " #category" + xtid).select2('data')[0].id,
                    'category_name': $("#" + yform + " #category" + xtid).select2('data')[0].text,
                    'remarks': $("#" + yform + " #remarks" + xtid).val(),
                    'brand': $("#" + yform + " #brand" + xtid).val(),
                    'subbrand': $("#" + yform + " #subbrand" + xtid).val(),
                    'quantity': qty,
                    'price': getnumber($(this).val()),
                    'total': jttl
                };
                subtasklist.push(data);

                $("#" + yform + " #total" + xtid).val(thousandsep(jttl))

                vttl += getnumber($(this).val()) * qty;

            });
            xtotal += vttl;
            var task_name = $("#" + yform + " #task_name" + ic).val();
            var departement_id = $("#" + yform + " #departement" + ic).val();
            detaildata.push({
                'task_name': task_name,
                'departement': '',
                'quantity': 1,
                'unit': '',
                'price': vttl,
                'cost': vttl,
                'percentage': vttl,
                'margin': vttl,
                'total': vttl,
                'sub_task': subtasklist
            });

        }
    }
    var currentdata = {
        'code': "-",
        'description': $("#" + xform + " #description").val(),
        'job_id': $("#" + xform + " #job_id").val(),
        'mother_pca_id': $("#" + xform + " #mother_pca_id").val(),
        'rate_card_id': $("#" + xform + " #rate_card_id").val(),
        'currency': $("#" + xform + " #currency").select2('data')[0].id,
        'detail': detaildata,
        'total': xtotal
    }
    return currentdata;
}
 
function getdata() {
    var detaildata = [];
    var xtotal = 0;
    for (var ic = 0; ic <= xtaskno; ic++) {
        
        var xcountx = $("#" + xform+" #taskcount" + ic).length;
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

                var qty = getnumber($("#" + xform +" #quantity" + xtid).val())
                var xprice = getnumber($(this).val());
                var ctotal = xprice * qty;
                vttl += ctotal;

                data = {
                    'subtask': subtask,
                    'category_id': $("#" + xform +" #category" + xtid).select2('data')[0].id,
                    'category_name': $("#" + xform +" #category" + xtid).select2('data')[0].text,
                    'remarks': $("#" + xform +" #remarks" + xtid).val(),
                    'brand': $("#" + xform +" #brand" + xtid).val(),
                    'subbrand': $("#" + xform +" #subbrand" + xtid).val(),
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
                'task_name':task_name,
                'price': vttl,
                'sub_task': subtasklist
            });
            $("#" + xform +" #taskcount" + ic).val(thousandsep(vttl));
        }
    }
    var currentdata = {
        'code': "-",
        'description': $("#" + xform + " #description").val(),
        'business_unit_id': $("#" + xform + " #business_unit_id").select2('data')[0].id,
        'currency': $("#" + xform + " #currency").select2('data')[0].id,
        'name': $("#" + xform +" #name").val(),
        'detail': detaildata,
        'total': xtotal
    }
    return currentdata;
}
function hitungtotaltask() {
    var xtotal = 0;
    var yform = 'TaskForm';

    for (var ic = 0; ic <= ytaskno; ic++) {
        var xcountx = $("#" + yform + " #tasktotal" + ic).length;

        if (xcountx > 0) {
            vttl = 0;
            $(".ysubtask" + ic).each(function (index, item) {
      
                var xtid = $(this).attr("data-id");
                var qty = getnumber($("#" + yform + " #quantity" + xtid).val())
                jttl = getnumber($(this).val()) * qty;
                $("#" + yform + " #total" + xtid).val(thousandsep(jttl))
                 
                vttl += getnumber($(this).val()) * qty;

            });
            xtotal += vttl;
            $("#" + yform + " #tasktotal" + ic).val(thousandsep(vttl));
        }
    }
    $("#" + yform + " #totalAmount").text(thousandsep(xtotal));
    $("#" + yform + " #total").val(thousandsep(xtotal));
}
function hitungtotal() {
    var xtotal = 0;
    xtaskno = 0;
    var dform = "#PcaFormCreate";
    $(alltask).each(function (index, item) {
        xtaskno++;
        alltask[index].Id = $(dform + ' #task_id' + xtaskno).val();
        alltask[index].quantity = getnumber($(dform + ' #quantity' + xtaskno).val());
      
        alltask[index].unit = $(dform + ' #unit' + xtaskno).val();
        alltask[index].price = getnumber($(dform + ' #price' + xtaskno).val());
        alltask[index].ppn = getnumber($(dform + ' #ppn' + xtaskno).val());
        alltask[index].asf = getnumber($(dform + ' #asf' + xtaskno).val());
        alltask[index].pph = getnumber($(dform + ' #pph' + xtaskno).val());
        alltask[index].cost = (alltask[index].quantity * alltask[index].price) + alltask[index].ppn + alltask[index].asf + alltask[index].pph;
        $(dform + ' #cost' + xtaskno).val(thousandsep(alltask[index].cost));
        alltask[index].percentage = getnumber($(dform + ' #percentage' + xtaskno).val());

        alltask[index].margin = (alltask[index].cost * alltask[index].percentage)/100;
        $(dform + ' #margin' + xtaskno).val(thousandsep(alltask[index].margin));

        alltask[index].total = alltask[index].cost + alltask[index].margin;
        $(dform + ' #total' + xtaskno).val(thousandsep(alltask[index].total));
        xtotal += alltask[index].total;
    });

    $(dform + ' #subtotal').val(thousandsep(xtotal));
    var ofp = getnumber($(dform + ' #other_fee_percentage').val());
    var ofpv = xtotal * ofp / 100;
    var vat = 0.1 * xtotal;
    var grandtotal = xtotal + ofpv + vat;
    $(dform + ' #other_fee').val(thousandsep(ofpv));
    $(dform + ' #vat').val(thousandsep(vat));
    $(dform + ' #totalAmount').html(thousandsep(grandtotal));
    $(dform + ' #total').val(thousandsep(grandtotal));
}
var vcall = 0;
function getrecordid() {
    vcall++;
    if (vcall === 2 && RecordID!='') {
        ajaxGet(ajaxUrlGet + "/" + RecordID, {}, function (responsedata) {



            fnPanel.endprocess();
            if (responsedata.data) {
                var edata = responsedata.data;
                FormTextByData(edata, xform);
                alltask = edata.detail;
                
                fnPca.generatetable();


                if (edata.job_id) {
                    $("#" + xform + ' #job_id').append('<option value="' + edata.job_id + '" selected>' + edata.job_name + '</option>');
                }
                if (edata.currency) {
                    $("#" + xform + ' #currency').append('<option value="' + edata.currency + '" selected>' + edata.currency_name + '</option>');
                }
                if (edata.rate_card_id) {
                    $("#" + xform + ' #rate_card_id').append('<option value="' + edata.rate_card_id + '" selected>' + edata.rate_card_name + '</option>');
                }
                if (edata.mother_pca_id) {
                    $("#" + xform + ' #mother_pca_id').append('<option value="' + edata.mother_pca_id + '" selected>' + edata.mother_pca_name + '</option>');
                }
                if (edata.shareservices_id) {
                    $("#" + xform + ' #shareservices_id').append('<option value="' + edata.shareservices_id + '" selected>' + edata.shareservice_name + '</option>')
                }
                if (edata.type_of_expense_id) {
                    $("#" + xform + ' #type_of_expense_id').append('<option value="' + edata.type_of_expense_id + '" selected>' + edata.type_of_expense_name + '</option>')
                }
                if (edata.mainservice_category_id) {
                    $("#" + xform + ' #mainservice_category').append('<option value="' + edata.mainservice_category_id + '" selected>' + edata.mainservice_name + '</option>')
                }
            } else {
                DangerNotif("No Data Found")
                convault(null, WEB + "/projectmanagement/Pca/");
            }
           
        });
    }
}
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
            var data = {
                refId: response.data.id,
                detailLink: "/projectmanagement/pca/Approval/" + response.data.id,
                tName: "pm.pca",
                menuId: "7881f141-06af-4910-a88a-a0c731c67f90",
                IdTemplate: $("#template_approval_id").val()
            };
            saveTemplateApprovalPm(data, webUrl);

            //window.location.href = WEB + "/hris/HiringRequest";
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);


    $('#pca_id').keyup(function () {

     
        if ($(this).val().length == 0) {
           
            $('.approval-disp').hide();
        } else {
          
            $('.approval-disp').show();
        }
    }).keyup();


    $('#pca_id').keyup(function () {


        if ($(this).val().length == 0) {

            //$('.submit-pce').attr('disabled', true);
            $('.submit-pce').hide();
            $('.submit-rfq').hide();
            $('.submit-ca').hide();
        } else {

            //$('.submit-pce').attr('disabled', false);
            $('.submit-pce').show();
            $('.submit-rfq').show();
            $('.submit-ca').show();
        }
    }).keyup();


    ajaxGet(ajaxUrlGetTempateApprovalLink, { Search_MenuName: "Analysis (PCA)", Search_TemplateName: "PCA Budgeted" }, function (response) {

        response = parseJson(response);
        Deleteaja('no_data');
        var count = 1;
        $("#PcaListApprover > tbody").html("");
        $.each(response.rows[0].listTmUserApproval, function (index, value) {
            AddRowTable(count, value.job_title_name, value.employee_full_name);
            count = count + 1;
        });
        $("#template_approval_id").val(response.rows[0].id);

    });


    ajaxGet(ajaxUrlListType, { 'rows': 1000, 'active': true }, function (response) {
        response = parseJson(response);

        var _option = ''
        $.each(response.rows, function (i, value) {
            _option += ('<option value="' + value.id + '"><small>' + value.name + '</small></option>');
        });

        $('select[name=type_of_expense_id]').append(_option);

        console.log(response);
    });

    $('select[name=type_of_expense_id]').change(function () {

        if ($('select[name=type_of_expense_id] option:selected').val() == "1128498563668643849") {
            $('.refrence').show();
        }
        else {
            $('.refrence').hide();
        }

    });

    ajaxGet(ajaxUrlShareServices, { 'rows': 1000, 'active': true }, function (response) {
        response = parseJson(response);

        var _option = ''
        $.each(response.rows, function (i, value) {
            _option += ('<option value="' + value.id + '"><small>' + value.shareservice_name + '</small></option>');
        });

        $('select[name=shareservices_id]').append(_option);

        console.log(response);
    });

    ajaxGet(categoryurl, { 'rows': 1000 }, function (response) {
        categorylist = [{ 'id': null, 'text': "........" }];
        $(response.rows).each(function (index, item) {
            categorylist.push({
                'id': item.account_number,
                'text':item.account_name
            });
            getrecordid()
        });

    });
    ajaxGet(ajaxUrlListOrganization, { 'rows': 1000, Search_business_unit_level: 700 }, function (response) {
        departementlist = [{ 'id': null, 'text': "........" }];
        $(response.rows).each(function (index, item) {
            departementlist.push({
                'id': item.id,
                'text': item.unit_name
            });
            getrecordid()
        });

    });
    unitlist = [{ 'id': 'unit', 'text': 'Unit' },
        { 'id': 'kg', 'text': 'kg' },
        { 'id': 'pcs', 'text': 'pcs' },
        { 'id': 'md', 'text': 'md' },]
    
    var optionsdd = {
        toggle: 'click',
        hoverTimeout: 300,
        skin: 'light',
        height: 'auto',
        maxHeight: false,
        minHeight: false,
        persistent: true,
        mobileOverlay: true
    };
    dropdowntask = new mDropdown('dropdowntask', optionsdd);
    dropdowntask.on('beforeShow', function (e) {
        fnPca.showtaskboxadd
    })
    var optionsForm = GetOptionsForm(function () {

        var xret = $("#" + xform).valid();
        if (xret) {
            data = getpcadata()
            postdata(data);
           
        }
        
        return false;
    }, function (response, statusText, xhr, $form) {
 

        });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreate);
    initlanguage(lang_gn);

    $('#job_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListJob,
        searchparam: 'search',
        labelField: 'job_name',
        valueField: 'id'
    });

    $('#refrence_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlPca,
        searchparam: 'search',
        labelField: 'code',
        valueField: 'id'
    });
    
    $('#mother_pca_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListMotherPca,
        searchparam: 'search',
        labelField: 'code',
        valueField: 'id'
    });
    $('#rate_card_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListRateCard,
        searchparam: 'search',
        labelField: 'code',
        valueField: 'id'
    });
 
    //$('#type_of_expense_id').setcombobox({
    //    data: [{ 'id': 'Normal', 'text': 'Normal' },
    //    { 'id': 'Cross Billing', 'text': 'Cross Billing' },
    //    { 'id': 'Outlett', 'text': 'Outlett' },
    //    { 'id': 'T & E', 'text': 'T & E' }],
    
    //    maximumSelectionLength: 1,

    //    placeholder: "choose....",
    //});
    $('#currency').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListCurrency,
        searchparam: 'search',
        labelField: 'currency_name',
        valueField: 'currency_code'
    });

    $('#job_id').on('select2:select', function (e) {
        var data = e.params.data;
        ajaxGet(ajaxUrlGetJob + '/' + data.id, {}, function (response) {
            jobtask = [];
            alltask = [];
            fnPca.generatetable();
            $('#jobtasklist').empty();
            $('#ratecardtasklist').empty();
            if (response.rows) {
                jobtask = response.rows;
                var xdata = response.rows;
                vstr = "Job Task :<br>";
                 $(jobtask).each(function (index, item) {
                     vstr += '- <a href="javascript:void(0)" onclick="fnPca.addjobtask(\'' + item.task_name + '\',\'jobtask\')">' + item.task_name + '</a><br>';
                });
                $('#jobtasklist').append(vstr);
            }
        });
    });

    $('#rate_card_id').on('select2:select', function (e) {
        var data = e.params.data;
        ajaxGet(ajaxUrlGetRateCard + '/' + data.id, {}, function (response) {
            ratecardtask = [];
            alltask = [];
            fnPca.generatetable();
            $('#jobtasklist').empty();
            $('#ratecardtasklist').empty();
            if (response.data.detail) {
                ratecardtask = response.data.detail;
 
                vstr = "Rate Card Task :<br>";
                $(ratecardtask).each(function (index, item) {
                    vstr += '- <a href="javascript:void(0)" onclick="fnPca.addjobtask(\'' + item.task_name + '\',\'ratecard\')">' + item.task_name + '</a><br>';
                });
                $('#ratecardtasklist').append(vstr);
            }
        });
    });
});

function submitPca(req) {
 

    if (req == "apprv")
    {
      
            
        var data = {
            refId: $("#pca_id").val(),
            detailLink: "/projectmanagement/pca/Approval/" + $("#pca_id").val(),
            tName: "pm.pca",
            menuId: "7881f141-06af-4910-a88a-a0c731c67f90",
            IdTemplate: $("#template_approval_id").val()
        };

        saveTemplateApprovalPm(data, webUrl, function (response) {
            response = parseJson(response);
            //if (response.success) {
            //    SuccessNotif(t_createsuccess);
            //    redirecttolink(webUrlre)
            //} else {
            //    DangerNotif(response.message);
            //}
            console.log(response.message);
        });
        redirecttolink(webUrl);
        
       
    }
}


function saverequest(vreqid, parameter) {
    var datenow = new Date();
    var sfdate = datenow.sformat();
    current_job_id = $("job_id").val();

    var detilx = [];
    if (vreqid == "ca") {

        $.each(rfsub, function (key, item) {
            $.each(item.sub_task, function (keysub, itemsub) {
                dtdetil = {
                    "id": "",
                    "amount": itemsub.quantity,
                    "task_detail": itemsub.sub_task,
                    "task_id": item.task_id,
                    "quantity": "1",
                    "unit_price": itemsub.price
                };
                detilx.push(dtdetil);
            });
        });
 
        datafinance = {
            "cashAdvance": {
                "id": "",
                "is_project_cost": true,
                "ca_number": "",
                "ca_date": sfdate,
                "revision": "0",
                "requested_on": sfdate,
                "requested_by": "189f3ca3-83b0-41aa-8af6-9c0a346d002c",
                "business_unit_id": "0c540a1a-eb30-42b4-b326-3e3ab1b9dc1a",
                "legal_entity_id": "37233a65-4dec-4888-989d-7bead019c991",
                "client_name": "",
                "notes": "",
                "payee_account_number": "",
                "payee_account_name": "",
                "payee_bank_branch": "",
                "ca_period_from": "",
                "ca_period_to": "",
                "cashAdvanceDetails": detilx
            },
            "cashAdvanceDetails": detilx
		};
		if (parameter) {
			if (parameter.location && parameter.url) {
				sessionStorage.setItem("cadetail", JSON.stringify({
					content: datafinance.cashAdvance,
					timestamp: new Date(),
					source: "pca"
				}));
				parameter.location.href = parameter.url;
			}
		} else {
			//no longer in use
			 ajaxPost(ajaxUrlcashadvance, datafinance.cashAdvance, function (response) {
				 swal("Notification", "Data Send to Finance Module!", "success");
			});
		}
       
    } else if (vreqid == "vs") {
        //------------------
    } else {
        var x = 0;
        $.each(rfsub, function (key, item) {
            x++;
            $.each(item.sub_task, function (keysub, itemsub) {
                x++;
                 
                    //dtdetil = {
                    //    "id": "",
                    //    "amount": itemsub.quantity,
                    //    "type_name": "Expense",
                    //    "item_type": "7774a249-0735-ab7f-5b2b-0b96e67ac297",
                    //    "task": $('.task_name').text(),
                    //    "subtask": $('#subtask:checked').val(),
                    //    "item_code": "",
                    //    "item_name": itemsub.sub_task,
                    //    "uom_name": "unit",
                    //    "category_name": "",
                    //    "category_id": "49c31023-856a-4bfe-af70-41589b422aa0",
                    //    "subbrand_name": itemsub.sub_brand,
                    //    "description": "",
                    //    "uom_id": "62919a04-5f2e-44d4-89a6-c831dd3c32e4",
                    //    "quantity": itemsub.quantity,
                    //    "unit_price": itemsub.price_per_quantity
                    //};
                    //detilx.push(dtdetil);
              
                      dtdetil = {
                        "id": "",
                        "amount": itemsub.quantity,
                        "type_name": "Expense",
                        "item_type": "7774a249-0735-ab7f-5b2b-0b96e67ac297",
                        "task": $('.task_name').text(),
                        "subtask": $('tr.treegrid-'+ x +' #subtask').val(),
                        //"item_code": "",
                        //"item_name": itemsub.sub_task,
                        "item_name": $('tr.treegrid-'+ x +' #subtask').val(),
                        "uom_name": "unit",
                        "category_name": $("tr.treegrid-"+ x +" .itemsub_category").text(),
                        "category_id": "49c31023-856a-4bfe-af70-41589b422aa0",
                        "currency_id" : $('#currency').val(),
                        "subbrand_name": itemsub.sub_brand,
                        "description": "",
                        "uom_id": "62919a04-5f2e-44d4-89a6-c831dd3c32e4",
                        "qty": $("tr.treegrid-"+ x +" .itemsub_quantity").text(),
                        "unit_price": $("tr.treegrid-"+ x +" .itemsub_price").text(),
                        "exchange_rate" : "1",

                    };
                    //detilx.push(dtdetil);

                var test = $("tr.treegrid-" + x + " #subtask:checked").val();

                if ( typeof test !== "undefined") {

                    detilx.push(dtdetil);
                }
                //$.each($("#subtask:checked"), function () {
                //    console.log(dtdetil);
                //});



                });
        });
        datafinance = {
            "requestForQuotation": {
                "id": "",
                "request_for_quotation_number": "-",
                "request_for_quotation_date": "01/30/2019",
                //"job_id": current_job_id,
                "job_id": $("#job_id option:selected").val(),
                "prefered_vendor_id": "4c31b7d1-56cb-4226-b416-f57cbba93c59",
                "brand_name": current_brand,
                "business_unit_id": "3c8dc093-20eb-43d3-8ffb-acbd8bba5753",
                "legal_entity_id": "37233a65-4dec-4888-989d-7bead019c991",
                "delivery_place": null,
                "delivery_date": "01/30/2019",
                "closing_date": null,
                "currency_id": "46e4d57e-5ce2-4c31-82be-f81382dc243a",
                "exchange_rate": null,
                "job_pa_id": $("#pca_id").val(),
                "job_pe_id": null,
                "is_prefered_vendor": $('input[name=is_prefered_vendor]:checked').val(),
                "term_of_payment": null,
                "details": detilx
            },
            //"details": detilx
        };

        //console.log(datafinance);
        ajaxPost(ajaxUrlrfq, datafinance.requestForQuotation, function (response) {
            response = parseJson(datafinance);

            swal("Notification", "Data Send to Finance Module!", "success")

            console.log(datafinance);

        });
        //ajaxPost(ajaxUrlrfq, datafinance.requestForQuotation, function (response) {
        //    response = parseJson(datafinance.requestForQuotation);
           
        //    swal("Notification", "Data Send to Finance Module!", "success")

        //    console.log(datafinance.requestForQuotation);

        //});
    }

}
function senttorfq() {
    swal({
        title: "Send Data",
        text: "Send Data to Finance Module",

        timer: 3000
    }).then(function () {
        swal("Success", "Data Have been send", "success");
    });
    
}
function viewpce() {
    Swal.fire({
        title: 'Submit to PCE?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {

            ajaxPost(ajaxUrlsubmittopce + "/" + RecordID, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    swal("Success", "view PCE", "success").then(function () {
                        convault(null, WEB + "/projectmanagement/Pce/" + RecordID);
                    });
                } else {
                    DangerNotif(response.message);
                }

            });

           
        }
    })

}

function randomIntFromInterval(min, max) // min and max included
{
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function AddRowTable(no, title, nama) {
    var acak = randomIntFromInterval(1, 999);
    var markup = "<tr id=" + acak + "><td>" + no + "</td><td>" + title + "</td><td>" + nama + "</td></tr>";
    $("#PcaListApprover tbody").append(markup);
}


function Deleteaja(id) {
    $('table#PcaListApprover tr#' + id + '').remove();
}