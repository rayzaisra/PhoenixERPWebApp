let loadData = function () {
    let dataSearch = $('.searchbox').val();
    let urlApi = $.helper.resolveApi("~auditdata/Get/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~auditdata/Get");

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
            if (data.length === 0) {
                bindEmptyTable();
            }
            else {
                bindTable(data);
            }
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

let bindEmptyTable = function () {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="2" class="text-center">there is no data</td></tr>');
};

let bindTable = function (data) {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.actionDescription+ "</td>";
        row = row + "<td>" + val.tablE_NAME+ "</td>";
        row = row + "<td>" + val.fielD_NAME+ "</td>";
        row = row + "<td>" + val.olD_VALUE+ "</td>";
        row = row + "<td>" + val.new_VALUE + "</td>";
        row = row + "<td>" + val.dateToString + "</td>";
        row = row + "<td>" + val.updatE_BY+ "</td>";
        row = row + "<td>" + val.dB_USER_NAME + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

$(document).ready(function () {
    loadData();
});
