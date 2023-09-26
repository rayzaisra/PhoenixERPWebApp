$(function () {
    var dt = sessionStorage.getItem('uniquePage');
    dt = JSON.parse(dt);
    if (dt !== null) {
        if(dt.isEdit) binData();
    }
});
        
let binData = function () {
    let urlApi = $.helper.resolveApi("~ManageMenu/GetHierachyMenu");
    
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            newGenerateTableMenu(data);
            },
            error: function (e, t, s) {
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
                console.log(e);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 2e3));
};			

let newGenerateTableMenu = function(data){
    var tbl = $('#example>tbody');
    for (let i = 0; i < data.length; ++i) {
        let row = '';
        row = row + '<tr class="'+data[i].id+'-'+i+'">';
        if(data[i].children.length > 0) row = row + '<td><i class="fa fa-plus-circle collapse-index" onclick="expandcollapseDetail(\'trT' + data[i].id + '\')" id="iplustrT' + data[i].id + '"></i> '+ data[i].menuName +'</td>';
        else row = row + '<td><span style="margin-left:23px">'+ data[i].menuName +'</span></td>';

        row = row + '<td style="width:75px;text-align:center">';
        row = row + '<button type="button" class="btn btn-sm btn-info btn-indexer" title="move to up this index menu" onclick="reworkIndex(\''+data[i].id +'\', \'' +data[i].menuIndex +'\', \'U\',\'P\')"><i class="fa fa-arrow-circle-up"></i></button>&nbsp;';
        row = row + '<button type="button" class="btn btn-sm btn-indexer" title="move to down this index menu"  onclick="reworkIndex(\''+data[i].id +'\', \'' +data[i].menuIndex +'\', \'D\',\'P\')"><i class="fa fa-arrow-circle-down"></i></button>';
        row = row + '</td>';
        row = row + '</tr>';

        if(data[i].children.length > 0)
        {
            row = row + generateHirarchieMenu(data[i],i);
        }

        tbl.append(row);
    }
    $('#example').dataTable({});
};

let generateHirarchieMenu = function(data,index){
    var row = '';
    row = row + '<tr style="border:0" class="'+data.id+'-'+index+'">';
    row = row + '<td colspan="2" class="trT'+data.id+'" style="display: none;border:0;">' + generateTableSubMenu(data.children) ;
    row = row + '</td>';
    row = row + '</tr>';
    return row;
}

let generateTableSubMenu = function(data){
    var table = '<table class="table dTable" style="margin:0;padding:0"><thead></thead>';
    table = table + '<tbody>';
    for (let a = 0; a < data.length; a++) {
        const mn = data[a];
        table = table + '<tr  class="'+mn.id+'-'+a+'">';
        if(mn.children.length > 0) table = table + '<td><i class="fa fa-plus-circle collapse-index" onclick="expandcollapseDetail(\'trT' + mn.id + '\')" id="iplustrT' + mn.id + '"></i> '+ mn.menuName +'</td>';
        else table = table + '<td><span style="margin-left:23px">'+ mn.menuName +'</span></td>';

        table = table + '<td>'+mn.menuLink+'</td>';
        table = table + '<td style="text-align:center;width:75px">';
        table = table + '<button type="button" class="btn btn-sm btn-info btn-indexer" title="move to up this index menu"  onclick="reworkIndex(\''+mn.id +'\', \'' +mn.menuIndex +'\', \'U\',\''+mn.parentId +'\')"><i class="fa fa-arrow-circle-up"></i></button>&nbsp;';
        table = table + '<button type="button" class="btn btn-sm btn-indexer" title="move to down this index menu"  onclick="reworkIndex(\''+mn.id +'\', \'' +mn.menuIndex +'\', \'D\',\''+mn.parentId +'\')"><i class="fa fa-arrow-circle-down"></i></button>';
        table = table + '</td>';
        table = table + '</tr>';
        if(mn.children.length > 0)
        {
            table = table + '<tr style="border:0" class="'+mn.id+'-'+a+'">';
            table = table + '<td colspan="3" class="trT'+mn.id+'" style="display: none;">' + generateTableSubMenu(mn.children) ;
            table = table + '</td>';
            table = table + '</tr>';
        }
    }
    table = table + '</tbody>';
    table = table + '</table>';
    return table;
};

let reworkIndex = function(code,idx,tipe,parentId){
    let urlApi = $.helper.resolveApi("~ManageMenu/updateindex");
    var model = {Id : code,MenuIndex : idx,Type : tipe, ParentId : parentId};
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: urlApi,
        data: JSON.stringify(model),
        success: function (data) {
            if(parentId === 'P') window.location.href = "/um/managemenu/manageindex";
            else {
                reworkTableChild(data, parentId);
            }
        },
        error: function (r) {
            console.log(r);
        }
    });
};

let reworkTableChild = function(data,parentId){
    console.log(data);
    $(".trT" + parentId)[0].innerHTML = "";
    var row = generateTableSubMenu(data);
    $(".trT" + parentId)[0].innerHTML = row;
};

let expandcollapseDetail = function (divname) {
    if ($('.'+ divname).css('display') === "none") {
        $('.'+ divname).show(1000);
        $('.'+ divname).css('display','table-cell');
        $('#iplus' + divname).removeClass("fa-plus-circle");
        $('#iplus' + divname).addClass("fa-minus-circle");
    } 
    else {
        $('.'+ divname).css('display','none');
        $('#iplus' + divname).removeClass("fa-minus-circle");
        $('#iplus' + divname).addClass("fa-plus-circle");
    }
};	