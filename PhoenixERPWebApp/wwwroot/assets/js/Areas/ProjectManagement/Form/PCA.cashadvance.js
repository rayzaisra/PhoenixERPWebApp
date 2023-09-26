var ajaxUrlGet = API + "/ProjectManagement/Pca/GetPCAByJobNumber";
var ajaxUrlGetPA = API + "/ProjectManagement/Pca/getPCAByPaNumber";
var ajaxUrlmanagepca = API + "/ProjectManagement/Pca/manage";
var ajaxUrlListjob = API + "/ProjectManagement/Job/List";
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
        if (data.accountmanager) {
            $("#accountmanagerlist").append('<ul>');
            $.each(data.accountmanager, function (key, value) {

                $("#accountmanagerlist").append('<li>' + value + '</li>');
            });
            $("#accountmanagerlist").append('</ul>');
        }
        if (data.pm) {
            $("#pmname").text(data.pm.app_fullname);
        }
   
        
        $("#" + xtable + " tbody").empty();
        var xlayoutdetiltmp = $("#tmpl_taskaccordeondetil").html();
 
        
        if (data.pcatask) {
            detildata = data.pcatask;
 
            generatetable();
            var vstr = "";
            accno = 0;
            $.each(detildata, function (key, item) {
                accno++;
                var xlayoutdetil = xlayoutdetiltmp;

                xlayoutdetil = replaceAll(xlayoutdetil, "{{id}}", item.id);
                xlayoutdetil = replaceAll(xlayoutdetil, "{{unit}}", item.central_resources_name);
                xlayoutdetil = replaceAll(xlayoutdetil, "{{taskname}}", '<div class="tasktitle"><span class="titleno">' + accno + '</span> <span class="titletop">' + item.task_name + '</span><br><span class="titlebottom">' + item.central_resources_name + '</span></div>');
                vstr += xlayoutdetil;

            });
            if (vstr !== "") {
                var xlayout = $("#tmpl_taskaccordeon").html();
                vstr = replaceAll(xlayout, "{{loopdata}}", vstr);
            }
            
            $("#tbltaskdetil").html(vstr);
 
            $.each(detildata, function (key, item) {
                
                if (item.pcaDetil) {
                    vstrsub = "";
                    $.each(item.pcaDetil, function (keysub, itemsub) {
                        xsno++;
                        var xid = xsno;
    
                        vstrsub = '<tr><td>&nbsp;</td>';
                        vstrsub += '<td><input id="task_id_' + xid + '" type="hidden" value="' + item.task_id + '"><input id="task_unit_' + xid + '" type="hidden" value="' + item.central_resources_name + '"><input id="sub_task_id_' + xid + '" type="hidden" value="' + itemsub.sub_task_id + '"><input id="sub_task_' + xid + '" type="hidden" value="' + itemsub.sub_task + '">' + itemsub.sub_task + '</td>';
                        vstrsub += '<td><select class="cbgr" id="category_' + xid +'" style="width:100%"></select></td>';
                        vstrsub += '<td><span class="lbltbl">' + current_brand + '</span></td>';
                        vstrsub += '<td><input class="form-control form-control-sm m-input m-input--square" id="sub_brand_' + xid + '" type="text" value="' + (itemsub.sub_brand !== null ? itemsub.sub_brand:"") + '"></td>';
                        vstrsub += '<td><input class="form-control form-control-sm m-input m-input--square" id="remarks_' + xid + '" type="text" value="' + (itemsub.remarks !== null ? itemsub.remarks : "") + '"></td>';
                        vstrsub += '<td><input class="xcount form-control form-control-sm m-input m-input--square" id="quantity_' + xid + '" type="text" value="' + (itemsub.quantity !== null ? itemsub.quantity : "") + '"></td>';
                        vstrsub += '<td><select class="uoms" id="uom_' + xid +'"></select></td>';
                        vstrsub += '<td><input class="xcount form-control form-control-sm m-input m-input--square" id="price_per_quantity_' + xid + '" type="text" value="' + (itemsub.price_per_quantity !== null ? itemsub.price_per_quantity : "")  + '"></td>';
                        vstrsub += '<td><input id="ppn_' + xid + '" class="xcount form-control form-control-sm m-input m-input--square" value="' + (itemsub.ppn !== null ? itemsub.ppn : "")  + '"></td>' +
                            '<td><input id="asf_' + xid + '" class="xcount form-control form-control-sm m-input m-input--square" value="' + (itemsub.asf !== null ? itemsub.asf : "")  + '"></td>' +
                            '<td><input id="pph_' + xid + '" class="xcount form-control form-control-sm m-input m-input--square" value="' + (itemsub.pph !== null ? itemsub.pph : "")  + '"></td>';
                        vstrsub += '<td><div class="xtotal" id="qptotal_' + xid +'"></div></td>';
                        vstrsub += '</tr>';
                        $("#tabledetilpca" + item.id + ' tbody').append(vstrsub);

                        $('#category_' + xid ).combogrid({
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
                        $('#category_' + xid).combogrid('setValue', itemsub.category);
                        $('#category_' + xid).change();
                        
                    });
                    
                    
 /*
                    $('.category_account').setcombobox({
                        data: { 'rows': 100, 'Search_business_unit_type_id': 'subgroup' },
                        url: categoryurl,
                        searchparam: 'search',
                        labelField: 'account_name',
                        valueField: 'account_number'
                    });
*/
                    

                    $(".uoms").select2({
                        data: uoms,
                        width: "100%",
                        theme: "bootstrap",
                    });
                }
            });
            hitungtotal();
            $(".xcount").mask('000,000,000,000,000', { reverse: true });
            $(".xtotal").mask('000,000,000,000,000', { reverse: true });
        }
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
function getnumber(x) {

    if (x) {

        if (x.length > 0) {
            x = x.replace(/\,/g, '');
            x = x.replace(/\./g, ',');
            
        }
        return parseFloat(x) || 0;
 
    }
    return 0;
}
function hitungtotal() {
    
    var xtasktotalnew = {};
    for (var xid = 1; xid <= xsno; xid++) {
        var obj = $('#quantity_' + xid);
        if (obj.length > 0) {
            var curtabid = $(obj).closest('table').attr("id");
            var xt = 0;

            if (xtasktotalnew[curtabid] === undefined) {
                xtasktotalnew[curtabid] = 0;
            }
    
            $('#qptotal_' + xid).text("0");
            if ($('#' + curtabid + ' #quantity_' + xid).length > 0) {
                var ha = getnumber($('#' + curtabid + ' #quantity_' + xid).val());
                var hb = getnumber($('#' + curtabid + ' #price_per_quantity_' + xid).val());

                var ppn = getnumber($('#' + curtabid + ' #ppn_' + xid).val());

                var asf = getnumber($('#' + curtabid + ' #asf_' + xid).val());
                var pph = getnumber($('#' + curtabid + ' #pph_' + xid).val());

                if ($('#autoppn').is(':checked')) {
                    ppn = ha * hb * 10 / 100;
                    $('#' + curtabid + ' #ppn_' + xid).val(thousandsep(ppn));
                }
                
                xt = ha * hb;
      

                xt = xt+ ppn + asf + pph;
                if (isNaN(xt)) xt = 0;
                if (xtasktotalnew[curtabid] === 0) {
                    xtasktotalnew[curtabid] = getnumber(xt);
                } else {
                    xtasktotalnew[curtabid] += getnumber(xt);
                }

                $('#' + curtabid + ' #qptotal_' + xid).text(thousandsep(xt));
            }

            $('#' + curtabid + ' .tasktotal').text(thousandsep(xtasktotalnew[curtabid]));
        }
        
    }
   
 


}
function hitungtotalx() {
    $.each(detildata, function (key, item) {

        if (item.subtask) {
            var xtasktotal = 0;
            vstrsub = "";
            $.each(item.subtask, function (keysub, itemsub) {
                var xid = itemsub.id;
 
                var xt = 0;
                $('#qptotal_' + xid).text("0");

                    var ha = getnumber($('#existingtaskform #quantity_' + xid).val());
                    var hb = getnumber($('#existingtaskform #price_per_quantity_' + xid).val());
                    var ppn = getnumber($('#existingtaskform #ppn_' + xid).val());
                    var asf = getnumber($('#existingtaskform #asf_' + xid).val());
                    var pph = getnumber($('#existingtaskform #pph_' + xid).val());

                    if ($('#autoppn').is(':checked')) {
                        ppn = ha * hb * 10 / 100;
                        $('#existingtaskform #ppn_' + xid).val(thousandsep(ppn));
                    }
                    xt = ha * hb;

                    xt = parseFloat(xt) + parseFloat(ppn) + parseFloat(asf) + parseFloat(pph);
                    if (isNaN(xt)) xt = 0;
                    if (xtasktotal === 0) {
                        xtasktotal = getnumber(xt);
                    } else {
                        xtasktotal += getnumber(xt);
                    }
                    
                    $('#qptotal_' + xid).text(thousandsep(xt));

                
            });
            $('#tasktotal_' + item.id).text(thousandsep(xtasktotal));
 
        }
    });

 
}
function thousandsep(x) {
    if (x) {
        x = todecimalz(x);
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    }
    return 0;
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

function showrequestbox(x) {
    var vstr = "";
    accno = 0;
    var xlayoutdetilreq = $("#tmpl_taskaccordeonrequest").html();
    $.each(detildata, function (key, item) {
        accno++;
        var xlayoutdetil = xlayoutdetilreq;

        xlayoutdetil = replaceAll(xlayoutdetil, "{{id}}", item.id);
        xlayoutdetil = replaceAll(xlayoutdetil, "{{unit}}", item.central_resources_name);
        xlayoutdetil = replaceAll(xlayoutdetil, "{{taskname}}", '<div class="tasktitle"><span class="titleno">' + accno + '</span> <span class="titletop">' + item.task_name + '</span><br><span class="titlebottom">' + item.central_resources_name + '</span></div>');
        vstr += xlayoutdetil;

    });
    if (vstr !== "") {
        var xlayout = $("#tmpl_taskaccordeon").html();
        vstr = replaceAll(xlayout, "{{loopdata}}", vstr);
    }

    $("#tbltaskdetilrequest").html(vstr);

    $.each(detildata, function (key, item) {

        if (item.pcaDetil) {
            vstrsub = "";
            $.each(item.pcaDetil, function (keysub, itemsub) {
                xsno++;
                var xid = xsno;

                vstrsub = '<tr><td><input type="checkbox" value="x"></td>';
                vstrsub += '<td><input id="task_id_' + xid + '" type="hidden" value="' + item.task_id + '"><input id="task_unit_' + xid + '" type="hidden" value="' + item.central_resources_name + '"><input id="sub_task_id_' + xid + '" type="hidden" value="' + itemsub.sub_task_id + '"><input id="sub_task_' + xid + '" type="hidden" value="' + itemsub.sub_task + '">' + itemsub.sub_task + '</td>';
                vstrsub += '<td>' + itemsub.category + '</td>';
                vstrsub += '<td><span class="lbltbl">' + current_brand + '</span></td>';
                vstrsub += '<td>' + itemsub.sub_brand + '</td>';
                vstrsub += '<td>' + itemsub.remarks + '</td>';
                vstrsub += '<td>' + itemsub.quantity + '</td>';
                vstrsub += '<td>' + itemsub.uom + '</td>';
                vstrsub += '<td>' + itemsub.price_per_quantity + '</td>';
                vstrsub += '<td>' + itemsub.ppn + '</td>' +
                    '<td>' + itemsub.asf + '</td>' +
                    '<td>' + itemsub.pph + '</td>';
                vstrsub += '<td><div class="xtotal" id="qptotal_' + xid + '"></div></td>';
                vstrsub += '</tr>';
                $("#tablerequest" + item.id + ' tbody').append(vstrsub);
  

            });
             
        }
    });
    $("#requestbox").modal('show');
 
}
function shownewtask() {
   
    $('.textbox-text').each(function (i, obj) {
       // $(this).css("width", $(this).parent().width());
    });
    $("#newtask").modal('show');
     $("#existingtaskform")[0].reset();
 }
function generatetable() {

 
    vstr = '';
    var xntab = 0;
    $("#tablepca tbody").empty();
    var current_channel = "";
    var current_task = "";
    detildata = detildata.sort(dynamicSortMultiple("central_resources_name", "task_name"));
    var current_central = "&nbsp;";
    var tno = 0;
    $.each(detildata, function (key, item) {
        itemcentral = "";
        if (current_central !== item.central_resources_name) {
            current_central = item.central_resources_name;
            itemcentral = current_central;
        }
        if (item.status) {
            xntab++;
            vstr = '<tr>' +
                '<td>' + itemcentral + '</td>' +
                '<td>' + item.task_name +
                '</td > ' +
                '<td><div id="cost_' + xntab + '" class="xtotalpca">' + thousandsep(item.total) + '</div></td>' +
                '<td><input type="hidden" value="' + item.task_id + '" name="Pcatask[' + tno + '][task_id]" ><input id="per_' + xntab + '" name="Pcatask[' + tno + '][margin]" class="xcountpca form-control form-control-sm m-input m-input--square"></td>' +
                '<td><div id="margin_' + xntab + '" class="xtotalpca" ></div></td>' +
                '<td><div id="total_' + xntab + '" class="xtotalpca"></div></td>' +
                '</tr>';
            $("#tablepca tbody").append(vstr);

            xprice = getnumber($("#price_" + xntab).val());
            $('#tablepca #per_' + xntab).mask('000', { reverse: true });
            tno++;
        }

    
    });
    
    var satdata = [
        { id: 'pcs', text: 'pcs' },
        { id: 'unit', text: 'unit'},
        { id: 'md', text: 'md' },
        { id: 'hd', text: 'hd' }
    ];

    $(".satuan").select2({ minimumResultsForSearch: -1, data: satdata });
    hitungtotalpca();
}
function todecimalz(value, dp) {
    return +parseFloat(value).toFixed(dp);
}
function hitungtotalpca() {
    var subtotal = 0;
    var frmname = 'PCAForm';
    var vat = 0;
    var xid = 0;
    var grandtotal = 0;
    $.each(detildata, function (key, item) {
        xid++;
        var xcost = getnumber($('#' + frmname+' #cost_' + xid).text());
        var xper = getnumber($('#' + frmname + ' #per_' + xid).val());
        var xmargin = xcost * (xper/100);
        $('#' + frmname + ' #margin_' + xid).text(thousandsep(xmargin));

        var xtotal = xcost + xmargin;
        $('#' + frmname + ' #total_' + xid).text(thousandsep(xtotal));
        subtotal = subtotal + xtotal;
        
    });
    $('#' + frmname + ' #subtotal').text(thousandsep(subtotal));
    var feetotal = getnumber($('#' + frmname + ' #feetotal').val());
    vat = 0.1 * subtotal;
    $('#' + frmname + ' #vat').text(thousandsep(vat));
    grandtotal = subtotal + feetotal + vat;

    $('#' + frmname + ' #grandtotal').text(thousandsep(grandtotal));
    
}
$("#feetotal").mask('000,000,000,000,000', { reverse: true });
$(function () {
    if (RecordID !== "") {
        fnJob.getbyPAnumber();
    }
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
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
    $('#job_detail_id').combogrid({
        panelWidth: 500,
        fitColumns: false,
        delay: 1000,
        dataparam: { 'rows': 100 },
        idField: 'job_number',
        textField: 'job_name',
        url: ajaxUrlListjob,
        searchparam:'search_job_name',
        mode: 'remote',
        method: 'get',
        onSelect: function (row,data) {
            var value = data.id;
            fnJob.getdetil(value);

        },
        columns: [[
            { field: 'job_number', title: 'ID', width: 150 },
            { field: 'job_name', title: 'Name', width: 150 },
            { field: 'description', title: 'description', width: 150}
        ]],
 
    });
    $('#mother_pca').combogrid({
        panelWidth: 500,
        fitColumns: false,
        delay: 1000,
        dataparam: { 'rows': 100 },
        idField: 'job_number',
        textField: 'job_name',
        url: ajaxUrlListjob,
        searchparam: 'search_job_name',
        mode: 'remote',
        method: 'get',
        columns: [[
            { field: 'job_number', title: 'ID', width: 150 },
            { field: 'job_name', title: 'Name', width: 150 },
            { field: 'description', title: 'description', width: 150 }
        ]],

    });
    $('#rate_card_id').combogrid({
        panelWidth: 500,
        fitColumns: false,
        delay: 1000,
        dataparam: { 'rows': 100 },
        idField: 'id',
        textField: 'code',
        url: ajaxUrlListratecard,
        searchparam: 'Search_code',
        mode: 'remote',
        method: 'get',
        columns: [[
            { field: 'id', title: 'ID', width: 150 },
            { field: 'code', title: 'code', width: 150 },
            { field: 'description', title: 'description', width: 150 }
        ]],

    });
});

