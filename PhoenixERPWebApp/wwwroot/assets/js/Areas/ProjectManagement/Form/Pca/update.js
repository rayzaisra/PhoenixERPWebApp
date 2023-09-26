
var webUrl = WEB + "/ProjectManagement/Pca";
var ajaxUrl = API + "/ProjectManagement/Pca";
var ajaxUrlGet = ajaxUrl+"/get"
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "PcaFormUpdate";
var ajaxUrlListCompany = API + "/ProjectManagement/Pca/companylist";
var ajaxUrlListJob = API_PM + "/Job/List";
var ajaxUrlListRateCard = API_PM + "/RateCard/List";
var ajaxUrlListMotherPca = API_PM + "/MotherPca/List";
var ajaxUrlListCurrency = API_GENERAL + "/Currency/List";
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var ajaxUrlGetRateCard = API_PM + "/RateCard/Get";
var ajaxUrlGetJob = API_PM + "/Job/GetTaskList";
var xtaskno = 0;
var xsubtaskno = 0;
var ytaskno = 0;
var ysubtaskno = 0;
var categorylist = [];

fnPca = {
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
 
                        fnPca.addtask(item)

                    });




                    if (edata.job_id) {
                        $("#" + xform + ' #job_id').append('<option value="' + edata.job_id + '" selected>' + edata.job_name + '</option>');
                    }
                    if (edata.currency) {
                        $("#" + xform + ' #currency').append('<option value="' + edata.currency + '" selected>' + edata.currency_name + '</option>');
                    }
                } else {
                    DangerNotif("No Data Found")
                    convault(null, WEB + "/projectmanagement/Pca/");
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
        $('#currency').setcombobox({
            data: { 'rows': 100 },
            url: ajaxUrlListCurrency,
            searchparam: 'search',
            labelField: 'currency_name',
            valueField: 'currency_code'
        });
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
    addtask: function (data) {
        xtaskno++;

        vstr = '<tr>';

        vstr += '<td><select required style="width:100%"  class="xcategory form-control form-control-sm m-input m-input--square" id="category' + xtaskno + '" ></select></td>';

        vstr += '<td> <input type="hidden" id="task_name' + xtaskno + '" value="' + data.task_name + '">&nbsp;&nbsp;' + data.task_name + '</td>';
        vstr += '<td><input required class="xtext form-control-sm m-input m-input--square" id="quantity' + xtaskno + '"  value="' + data.quantity + '" type="text" ></td>';
        vstr += '<td><input class="xtext form-control-sm m-input m-input--square" id="unit' + xtaskno + '"  value="' + data.unit + '" type="text" ></td>';
        vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square" id="price' + xtaskno + '"  value="' + data.price + '" type="text" ></td>';
        vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square" id="cost' + xtaskno + '"  value="' + data.cost + '" type="text" ></td>';
        vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square" id="percentage' + xtaskno + '"  value="' + data.percentage + '" type="text" ></td>';
        vstr += '<td><input required class="xcounttask form-control-sm m-input m-input--square ysubtask' + xtaskno + '" data-id="' + xtaskno + '" id="margin' + xtaskno + '" value="' + data.margin + '" type="text" ></td>';
        vstr += '<td><input readonly class="xcounttask form-control-sm m-input m-input--square " id="total' + xtaskno + '" value="' + data.total + '"  type="text" ></td>';
        vstr += '</tr > ';
     
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
$(document).on('keyup', ".xcounttask", function () {
    hitungtotaltask();
});
function savetaskdetil() {

    cdata = getdatatask();
    console.log(cdata);
    postdata(cdata);
}
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
                    'category_id': $("#" + xform + " #category" + xtid).select2('data')[0].id,
                    'category_name': $("#" + xform + " #category" + xtid).select2('data')[0].text,
                    'remarks': $("#" + xform + " #remarks" + xtid).val(),
                    'brand': $("#" + xform + " #brand" + xtid).val(),
                    'subbrand': $("#" + xform + " #subbrand" + xtid).val(),
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
    fnPca.initform();
     
});
