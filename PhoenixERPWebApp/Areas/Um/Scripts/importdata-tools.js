let bindListOfTable = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/GetListTable");
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
                var ddl = $('#ddlTable');
               ddl.empty();
               $(ddl).append($('<option>', {
                    value: "0",
                    text: "CHOISE TABLE"
                }));
 
               $.each(data, function (i, item) {
                    $(ddl).append($('<option>', {
                        value: item.tableSchema,
                        text: item.tableName
                    }));
                });
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

let downloadTemplate = function () {
    var tbl = $('#ddlTable').val();
    if (tbl === "0") {
        alert('please defince selection table before download template.');
        return false;
    }

    var ddl = $('#ddlTable').select2('data');


    let urlApi = $.helper.resolveApi("~notifycenter/GetTemplateImportData/" + tbl + "/" + ddl[0].text);
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
            window.open(data.path,"_blank");
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


let importData = function () {
    var tbl = $('#ddlTable').val();
    if (tbl === "0") {
        alert('please defince selection table before import data.');
        return false;
    }

    var files = $("#fu").get(0).files;
    if (files.length === 0) {
        alert('Ooops, there is no file for upload import data, please select file first before import data!');
        return false;
    }

    var frmData = new FormData();
    frmData.append("File", files[0]);
    frmData.append("SchemaName", tbl);
    frmData.append("TableName", $('#ddlTable').select2('data')[0].text);
    let urlApi = $.helper.resolveApi("~notifycenter/importdata");

    $.ajax({
        url: urlApi,
        type: "POST",
        cache: false,
        processData: false,
        contentType: false,
        data: frmData,
        success: function (responseData) {
            alert('Import data success');
            window.location.href = "/um/ImportDataTools";
        },
        error: function (error) { console.log(error); }
    });



};

$(function () {
    $('#ddlTable').select2({ allowClear : true });
    bindListOfTable();
    $('.btn-download-tools').on('click', function () { downloadTemplate();});
    $('.btn-import-tools').on('click', function () { importData();});

});