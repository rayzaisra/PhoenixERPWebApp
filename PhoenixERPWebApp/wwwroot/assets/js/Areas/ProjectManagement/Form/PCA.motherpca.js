var ajaxUrlGet = API + "/ProjectManagement/Pca/Motherpcabycbno";
var ajaxUrlGetPA = API + "/ProjectManagement/Pca/MotherPCAbyMPCAno";
var ajaxUrlmanagepca = API + "/ProjectManagement/Pca/Managemotherpca";
var ajaxUrlListcb = API + "/ProjectManagement/Clientbrief/List";
var ajaxUrlListratecard = API + "/ProjectManagement/RateCardCode/List";
var ajaxUrlcreatetask = API + "/ProjectManagement/task/create";
var ajaxUrlcreatesubtask = API + "/ProjectManagement/pca/Managesubtask";
var ajaxUrlListOrganization = API_GENERAL + "/CentralResource/List";
var categoryurl = API + "/ProjectManagement/Pca/ListAccount";
var brandURL = API_PM + "/CompanyBrand/list";
var xform = "PCAForm";
var xtable = "tabledetilpca";
var detildata = [];
var categorylist = [];
var current_job_id = "";
var current_brand = "";
var accno = 0;
var uoms = [
    { "id": "pcs", "text": "pcs" },
    { "id": "unit", "text": "unit" },
    { "id": "md", "text": "md" },
    { "id": "hp", "text": "hp" }
];
function format(item) { return item.name; };
fnJob = {
    getbyPAnumber: function (value) {
        fnPanel.startprocess();
        ajaxGet(ajaxUrlGetPA + "/" + RecordID, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#showdetil").show();
                fnJob.setupjobform(response.data);
            }
            fnPanel.endprocess();
        });
    },
    getdetil: function (value) {
        fnPanel.startprocess();
        ajaxGet(ajaxUrlGet + "/" + value, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#showdetil").show();
                fnJob.setupjobform(response.data);
            }
            fnPanel.endprocess();
        });
    },
    setupjobform: function (data) {
        
        current_job_id = data.id;
        current_brand = data.brand_name;
        generatetable();
        setFormAction(xform, ajaxUrlmanagepca);
        FormTextByData(data, xform);
        $('#job_detail_id').combogrid('setValue', data.job_detail_id);
        $("#job_detail_id").focus();
        if (data.pm_id) {
            $('#user_id').append('<option value="' + data.pm_id + '" selected>' + data.pm + '</option>')
        }
       
        $("#accountmanagerlist").empty();
        if (data.account_management) {
            $.each(data.account_management, function (key, item) {

                $("#accountmanagerlist").append('<li>' + item.app_username + '</li>');
            });
        }
        if (data.detail !== null) {
            detildata = JSON.parse(data.detail);
            console.log(detildata);
            generatetable();
 //-------------
        } else {
            if (data.job_detil !== null) {
                $.each(data.job_detil, function (key, item) {
                    var datajob = {
                        'id':item.id,
                        'job_number': item.job_number,
                        'job_name': item.job_name,
                        'cost': 0,
                        'task':null
                    };
                    detildata.push(datajob);
                });
                generatetable();
            }
        }
        if (data.pm) {
            $("#pmname").text(data.pm.app_fullname);
        }
   
        
        $("#" + xtable + " tbody").empty();
        var xlayoutdetiltmp = $("#tmpl_taskaccordeondetil").html();
 
        
    }

};
function createtask() {
    var task_unit = $('#task_unit').select2('data');
 
    var task_unit_id = task_unit[0] !== undefined ? task_unit[0].id : "";
    var task_unit_text = task_unit[0] !== undefined ? task_unit[0].text : "";
    var data = {};
    data['central_resources_id'] = task_unit_id;
    data['task_name'] = $('#task_name').val();
    data['task_priority_id'] ='low';
    data['job_number'] = current_job_id;
    ajaxPost(ajaxUrlcreatetask, data, function (response) {
        response = parseJson(response);
        if (response.success) {
            var item = response.data;
            var xlayoutdetil = $("#tmpl_taskaccordeondetil").html();
            accno++;

            xlayoutdetil = replaceAll(xlayoutdetil, "{{id}}", item.id);
            xlayoutdetil = replaceAll(xlayoutdetil, "{{taskname}}", '<div class="tasktitle"><span class="titleno">' + accno + '</span> <span class="titletop">' + item.task_name + '</span><br><span class="titlebottom">' + task_unit_text + '</span></div>');
            $("#tbltaskdetil #taskaccordeonitem").append(xlayoutdetil);
            var retdata = response.data;
            retdata.central_resources_name = task_unit_text;
            retdata.subtask = [];
            detildata.push(retdata);
        }


    });
}
 
$(document).on('keyup', ".xcount", function () {
    hitungtotal();
});
$(document).on('keyup', ".xcountpca", function () {
    hitungtotalpca();
});
$(document).on('click', '.xcheck', function () {
    hitungtotal();
});
function toogledetil(e) {
    if ($("#tbltaskdetil").css('display') === 'none') {
        $('#showdetil').text("Task");
        $("#tablepca").hide();
        $("#tbltaskdetil").show();
    } else {
        $("#tablepca").show();
        $('#showdetil').text("Detil");
        $("#tbltaskdetil").hide();
    }
}
var xsno = 0;
function remnewsubtask(id) {

    $('table tbody tr.ntsk_'+id).remove();
  
}
function addnewsubtask(obj) {
    var curtabid = $(obj).closest('table').attr("id");
    var task_id = $(obj).closest('table').attr("tbl-id");
    var task_unit = $(obj).closest('table').attr("tbl-unit");
    xsno++;
    vstrsub = "";
   
    var xid = xsno;

    vstrsub += '<tr class="ntsk_' + xid + '">';
    vstrsub += '<td class="centericon"><a href="javascript:void(0)" onclick="remnewsubtask(\'' + xid + '\')"><i class="fa fa-minus-circle m--font-info"></i></a></td>';
    vstrsub += '<td><input id="task_id_' + xid + '" type="hidden" value="' + task_id + '"><input id="task_unit_' + xid + '" type="hidden" value="' + task_unit + '"><input id="sub_task_id_' + xid + '" type="hidden" value=""><input class="form-control form-control-sm m-input m-input--square" required id="sub_task_' + xid + '" type="text"></td>';
        vstrsub += '<td><select  id="category_' + xid + '" style="width:100%" ></select></td>';
    vstrsub += '<td><span class="lbltbl">' + current_brand+ '</span></td>';
    vstrsub += '<td><input class="form-control form-control-sm m-input m-input--square" id="sub_brand_' + xid + '" type="text"></td>';
        vstrsub += '<td><input class="form-control form-control-sm m-input m-input--square" id="remarks_' + xid + '" type="text"></td>';
    vstrsub += '<td><input class="xcount form-control form-control-sm m-input m-input--square" id="quantity_' + xid + '" type="text"></td>';
    vstrsub += '<td><select class="uoms" id="uom_' + xid + '"></select></td>';
    vstrsub += '<td><input class="xcount form-control form-control-sm m-input m-input--square" id="price_per_quantity_' + xid + '" type="text"></td>';
    vstrsub += '<td><input id="ppn_' + xid + '" class="xcount form-control form-control-sm m-input m-input--square"></td>' +
        '<td><input id="asf_' + xid + '" class="xcount form-control form-control-sm m-input m-input--square"></td>' +
        '<td><input id="pph_' + xid + '" class="xcount form-control form-control-sm m-input m-input--square"></td>';
    vstrsub += '<td><div class="xtotal" id="qptotal_' + xid + '"></div></td>';
        vstrsub += '</tr>';
 
 
    $("#"+curtabid+" tbody").append(vstrsub);
    $('#category_' + xid).combogrid({
        panelWidth: 450,
        idField: 'account_number',
        textField: 'account_name',
        columns: [[
            { field: 'account_number', title: 'Code', width: 120 },
            { field: 'account_name', title: 'Name', width: 160 }
        ]]
    });
    var cbc = $('#category_' + xid).combogrid('grid');
    cbc.datagrid('loadData', categorylist);
    $("#uom_"+xid).select2({
        data: uoms,
        width: "100%",
        theme: "bootstrap",
    });
    $('#price_per_quantity_' + xid ).mask('000,000,000,000,000', { reverse: true });
    $('#quantity_' + xid).mask('000,000,000,000,000', { reverse: true });
    $('#ppn_' + xid).mask('000,000,000,000,000', { reverse: true });
    $('#asf_' + xid).mask('000,000,000,000,000', { reverse: true });
    $('#pph_' + xid).mask('000,000,000,000,000', { reverse: true });
    
}

 

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

$("#existingtaskform").submit(function (e) {
    e.preventDefault();
    savetaskfromexisting();
    return false;
});
function savetaskfromexisting() {
    var curtabid = "existingtaskform";
    var xtasktotalnew = 0;
    var subtaskdata = [];
    for (var xid = 1; xid <= xsno; xid++) {



        var xt = 0;
        $('#qptotal_' + xid).text("0");
        if ($('#' + curtabid + ' #quantity_' + xid).length > 0) {
            var ha = getnumber($('#' + curtabid + ' #quantity_' + xid).val());
            var hb = getnumber($('#' + curtabid + ' #price_per_quantity_' + xid).val());
            var uom = $('#' + curtabid + ' #uom_' + xid).select2('data');
            var ppn = getnumber($('#' + curtabid + ' #ppn_' + xid).val());

            var asf = getnumber($('#' + curtabid + ' #asf_' + xid).val());
            var pph = getnumber($('#' + curtabid + ' #pph_' + xid).val());

            if ($('#autoppn').is(':checked')) {
                ppn = ha * hb * 10 / 100;
                $('#' + curtabid + ' #ppn_' + xid).val(thousandsep(ppn));
            }
            xt = ha * hb;
            subtaskdata.push({
                'task_id': $('#' + curtabid + ' #task_id_' + xid).val(),
                'sub_task_id': $('#' + curtabid + ' #sub_task_id_' + xid).val(),
                'sub_task': $('#' + curtabid + ' #sub_task_' + xid).val(),
                'category': $('#' + curtabid + ' #category_' + xid).combogrid('getValue'),
                'category_name': $('#' + curtabid + ' #category_' + xid).combogrid('getText'),
                'brand': current_brand,
                'sub_brand': $('#' + curtabid + ' #sub_brand_' + xid).val(),
                'remarks': $('#' + curtabid + ' #remarks_' + xid).val(),
                'quantity': ha,
                'uom': uom[0].text,
                'price_per_quantity': hb,
                'ppn': ppn,
                'asf': asf,
                'pph': pph,
                'task': ''
            });
        }


    }
    ajaxPost(ajaxUrlcreatesubtask, subtaskdata, function (response) {
        response = parseJson(response);
        if (response.success) {
            var value = $('#job_detail_id').combogrid('getValue');
            fnJob.getdetil(value);
            $("#newtask").modal('hide');
        }


    });



}
  
var xtaskid = 0;
function removetask(x) {
    xt = $(x).closest('tr');
    xt.remove();
    hitungtotalpca();
}
function addtask(x, y) {
    var xbtn = $(x);
   xbtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
    xtaskid++;
    xt = $(x).closest('tr');
    vstr = '<tr>' +
        '<td><input required  id="taskname_' + xtaskid +'" value="" class="form-control form-control-sm m-input m-input--square"></td>' +
        '<td><input required id="taskcost_' + xtaskid+'"  data-job="'+y+'"  value="" class="xcountpca taskcostinput form-control form-control-sm m-input m-input--square"></td>' +
        '<td><button class="btn btn-sm btn-block btn-danger" onclick="removetask(this)" type="button">remove</button> </td>' +
        '</tr>';
    $(vstr).insertBefore(xt)
    $('#tablepca #task_' + xtaskid).mask('000,000,000,000,000', { reverse: true });
    xbtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
}
function generatetable() {

 
    vstr = '';
    var xntab = 0;
    $("#tablepca tbody").empty();
    var current_channel = "";
    var current_task = "";

    var current_central = "&nbsp;";
    var tno = 0;
    $.each(detildata, function (key, item) {
 
            xntab++;
            vstr = '<tr>' +
                '<td><strong>' + item.job_name + '</strong></td>' +
                '<td>&nbsp;</td>' +
                '<td><input readonly job-id="' + item.id + '" job-name="' + item.job_name + '" id="cost_' + item.id + '"   value="' + thousandsep(item.total) + '" class="xcountpca jobcostinput form-control form-control-sm m-input m-input--square"></td>' +

            '</tr>';
        if (item.task) {
            $.each(item.task, function (keytask, itemtask) {
                xtaskid++;
                vstr += '<tr>' +
                    '<td><input required  id="taskname_' + xtaskid + '" value="' + itemtask.task_name + '" class="form-control form-control-sm m-input m-input--square"></td>' +
                    '<td><input required id="taskcost_' + xtaskid + '"  data-job="' + item.id + '"  value="' + itemtask.task_cost + '" class="xcountpca taskcostinput form-control form-control-sm m-input m-input--square"></td>' +
                    '<td><button class="btn btn-sm btn-block btn-danger" onclick="removetask(this)" type="button">remove</button> </td>' +
                    '</tr>';
            })
        }
            vstr += '<tr>' +
                '<td colspan="3"><button class="btn btn-sm btn-block btn-primary" onclick="addtask(this,\''+item.id+'\')" type="button">Add Task</button> </td>' +
            '</tr>';
 
            $("#tablepca tbody").append(vstr);

            xprice = getnumber($("#price_" + xntab).val());
        $('#tablepca #cost_' + item.id ).mask('000,000,000,000,000', { reverse: true });
            tno++;
 

    
    });
    
    var satdata = [
        { id: 'pcs', text: 'pcs' },
        { id: 'unit', text: 'unit'},
        { id: 'md', text: 'md' },
        { id: 'hd', text: 'hd' }
    ];

    hitungtotalpca();
}
function todecimalz(value, dp) {
    return +parseFloat(value).toFixed(dp);
}
function jobchangeval(id, cost) {
    for (var i in detildata) {
        if (detildata[i].id === id) {
            detildata[i].cost = cost;
            break;
        }
    }
}
function jobchangetask(id, task) {
    for (var i in detildata) {
        if (detildata[i].id === id) {
            detildata[i].task = task;
            break;
        }
    }
}
function isidetil() {
    var xtask = [];
    for (xx = 0; xx <= xtaskid; xx++) {
        var exdata = $("#taskname_" + xx);
        var excost = $("#taskcost_" + xx);
        if (exdata !== undefined) {
            var jobid = excost.attr("data-job");

            dtask = {
                'job_id': jobid,
                'task_name': exdata.val(),
                'task_cost': getnumber(excost.val())
            };
            if (xtask[jobid] !== undefined) {
              
                xtask[jobid].push(dtask);
            } else {
              
                xtask[jobid] = [];
                xtask[jobid].push(dtask);
            }
            console.log(dtask);
        }
    }
    $('.jobcostinput').each(function (i, obj) {
        var xcost = getnumber($(obj).val());
        var jobid = $(obj).attr("job-id");
        jobchangeval(jobid, xcost);
        if (xtask[jobid] !== undefined) {
            jobchangetask(jobid, xtask[jobid]);
        }
            
    });
    
    $("#detail").val(JSON.stringify(detildata));
}
function hitungtotalpca() {
    var subtotal = 0;
    var total = getnumber($('#total').val());
    var frmname = 'PCAForm';
    var vat = 0;
    var xid = 0;
    var grandtotal = 0;
    var jcinput = $(".jobcostinput");
    
    $('.jobcostinput').each(function (i, obj) {
        $(obj).val(0);
    })
    $('.taskcostinput').each(function (i, obj) {
        var jobid = $(obj).attr('data-job');
        var xcost = getnumber($(obj).val());
        var dtotal = getnumber($("#cost_" + jobid).val());
        var xdtotal = dtotal + xcost;
 
        $("#cost_" + jobid).val(thousandsep(xdtotal));

    });
    $('.jobcostinput').each(function (i, obj) {
        var xcost = getnumber($(obj).val());


        grandtotal = xcost + grandtotal;
    });
    if (total < grandtotal) {
        DangerNotif('OverBudget\n ( max: '+total +')');
    }
     $('#' + frmname + ' #grandtotal').text(thousandsep(grandtotal));
    
}
 
$(function () {
    //$('#total').mask('000,000,000,000,000', { reverse: true });
    if (RecordID !== "") {
        fnJob.getbyPAnumber();
    }
    var optionsForm = GetOptionsForm(function () {
        isidetil();
        var grnd = getnumber($('#grandtotal').text());
        var ttl = getnumber($('#total').val());
        var xret = $("#" + xform).valid();
        if (grnd !== ttl) {
            DangerNotif("value invalid");
            xret = false;
        }
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_updatesuccess);
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    $('#task_unit').setcombobox({
        data: { 'rows': 100, 'Search_business_unit_type_id':'departement' },
        url: ajaxUrlListOrganization,
        searchparam: 'search',
        labelField: 'name',
        valueField: 'id'
    });
    $("#showdetil").hide();
    /*
    $('#job_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListjob,
        searchparam: 'search',
        labelField: 'job_number',
        valueField: 'job_number',
    });
    
    $("#job_id").on("select2:unselecting", function (e) {
        $("#showdetil").hide();
    });
    $('#job_id').on("select2:selecting", function (e) {
        var value = e.params.args.data.id;
        ajaxGet(ajaxUrlGet + "/" + value, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#showdetil").show();
                fnJob.setupjobform(response.data);
            }


        });
    });
    */
    $.extend($.fn.combogrid.defaults, {
        loader: function (param, success, error) {
            var opts = $(this).datagrid('options');
            if (!opts.url) return false;
 
            var xdata = {};
            if (opts.dataparam) {
                xdata = opts.dataparam;
            }
            xdata[opts.searchparam] = param.q;
            $.ajax({
                type: opts.method,
                url: opts.url,
                data: xdata,
                dataType: 'json',
                success: function (data) {
                    success(data);
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        }
    });
    ajaxGet(categoryurl, { 'rows': 1000, 'Search_business_unit_type_id': 'subgroup' }, function (response) {
        categorylist = response.rows;
    });
    $('#client_brief_id').combogrid({
        panelWidth: 500,
        fitColumns: false,
        delay: 1000,
        dataparam: { 'rows': 100 },
        idField: 'id',
        textField: 'campaign_name',
        url: ajaxUrlListcb,
        searchparam:'Search_campaign_name',
        mode: 'remote',
        method: 'get',
        onSelect: function (row,data) {
            var value = data.id;
            fnJob.getdetil(value);

        },
        columns: [[
            { field: 'campaign_name', title: 'Name', width: 150 },
            { field: 'job_name', title: 'Name', width: 150 },
            { field: 'description', title: 'description', width: 150}
        ]],
 
    });
    
});

