 
var webUrl = WEB + "/ProjectManagement/Pca";
var ajaxUrl = API + "/ProjectManagement/Pca";
var ajaxUrlCreate = ajaxUrl + "/Create";
var xform = "PcaFormCreate";
var ajaxUrlListCompany = API + "/ProjectManagement/Pca/companylist";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListJob = API_PM + "/Job/List";
var ajaxUrlListRateCard = API_PM + "/RateCard/List";
var ajaxUrlListMotherPca = API_PM + "/MotherPca/List";
var ajaxUrlListCurrency= API_GENERAL + "/Currency/List";
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var ajaxUrlGetRateCard = API_PM + "/RateCard/Get";
var ajaxUrlGetJob = API_PM + "/Job/GetTaskList";
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
var dropdowntask
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
    showtaskbox: function () {
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
                    $('#RatecardTable tbody').empty();
                    vstr = "";
                    vno = 0;
                    $(xdata).each(function (index, item) {
                        vno++;
                        var taskno = vno;
                        vstr += '<tr class="treegrid-' + vno + '"><td colspan="7">' + item.task_name + '</td><td>' + item.price + '</td></tr>';
                        $(item.sub_task).each(function (index, itemsub) {
                            vno++;
                            vstr += '<tr class="treegrid-' + vno + ' treegrid-parent-' + taskno + '" ><td><label><input type="checkbox"> ' + itemsub.subtask + '</label></td><td>' + itemsub.category_name + '</td><td>' + itemsub.remarks + '</td><td>' + itemsub.brand + '</td><td>' + itemsub.subbrand + '</td><td class="rcol">' + thousandsep(itemsub.quantity) + '</td><td class="rcol">' + thousandsep(itemsub.price) + '</td><td class="rcol">' + thousandsep(itemsub.total) + '</td></tr>';

                        });
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
                    $('#taskTable tbody').empty();
                    vstr = "";
                    ytaskno = 0;
                    treeno = 0;
                    subtreeno = 0;
                    ysubtaskno = 0;
                    $(xdata).each(function (index, item) {
                        console.log(item.sub_task);
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
            vstr += '<tr class="tsk' + xtaskno + '">';
            vstr += '<td><select class="departement" id="departement' + xtaskno +'" style="width:100%"></select></td>';
            vstr += '<td ><div class="padd5">' + item.task_name + '<a href="javascript:void(0)" onclick="fnPca.removetask(\'' + item.task_name + '\')" class="pull-right"><i class="fa fa-minus-circle fa-sm"></i></a><a href="javascript:void(0)" onclick="fnPca.subtask(\'' + item.task_name +'\')" class="pull-right"><i class="fa fa-play-circle fa-sm"></i></a></div></td>';
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
function savetaskdetil() {

    cdata = getpcadata();
 
    postdata(cdata);
}
function postdata(cdata) {
    ajaxPost(ajaxUrlCreate, cdata, function (response) {
        response = parseJson(response);
        //test
        console.log(response.rows);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            redirecttolink(webUrl)
        } else {
            DangerNotif(response.message);
        }
        
    });
}
function getpcadata() {
    var currentdata = {
        'code': "-",
        'description': $("#" + xform + " #description").val(),
        'job_id': $("#" + xform + " #job_id").val(),
        'mother_pca_id': $("#" + xform + " #mother_pca_id").val(),
        'rate_card_id': $("#" + xform + " #rate_card_id").val(),
        'currency': $("#" + xform + " #currency").val(),
        'subtotal': $("#" + xform + " #subtotal").val(),
        'other_fee_name': $("#" + xform + " #other_fee_name").val(),
        'other_fee': $("#" + xform + " #other_fee").val(),
        'other_fee_percentage': $("#" + xform + " #other_fee_percentage").val(),
        'vat': $("#" + xform + " #vat").val(),
        'detail': alltask,
        'total': $("#" + xform + " #total").val()
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
        alltask[index].quantity = getnumber($(dform + ' #quantity' + xtaskno).val());
        alltask[index].departement = $(dform + ' #departement' + xtaskno).val();
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
$(function () {

    ajaxGet(categoryurl, { 'rows': 1000 }, function (response) {
        categorylist = [{ 'id': null, 'text': "........" }];
        $(response.rows).each(function (index, item) {
            categorylist.push({
                'id': item.account_number,
                'text':item.account_name
            });

        });

    });
    ajaxGet(ajaxUrlListOrganization, { 'rows': 1000, Search_business_unit_level: 700 }, function (response) {
        departementlist = [{ 'id': null, 'text': "........" }];
        $(response.rows).each(function (index, item) {
            departementlist.push({
                'id': item.id,
                'text': item.unit_name
            });

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
            savetaskdetil();
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
    $('#type').select2({
        data: [{ 'id': 'Normal', 'text': 'Normal' },
            { 'id': 'Cross Billing', 'text': 'Cross Billing' },
            { 'id': 'Outlett', 'text': 'Outlett' },
            { 'id': 'T & E', 'text': 'T & E' }],
        maximumSelectionLength: 1,
        allowClear: true,
        dropdownCssClass: 'bigselect'
    });
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
