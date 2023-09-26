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

let drawPivotUI = function (sc, tbl) {
    if (sc === "0") return false;
    let urlApi = $.helper.resolveApi("~notifycenter/GetDataRows/" + sc + "/" + tbl);
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
            console.log(data);
           $("#output").pivotUI(
            data.dataTables,
            {
                rows: data.listOfColumns,
                cols: data.listOfColumns
            }
        );


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

$(function () {
    $('#ddlTable').select2({ allowClear : true });
    $('#ddlTable').on('select2:select', function (e) {
        drawPivotUI(e.params.data.id, e.params.data.text);
    });
    bindListOfTable();
});