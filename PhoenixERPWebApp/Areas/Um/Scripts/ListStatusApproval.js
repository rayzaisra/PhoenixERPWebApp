let bindUserApproval = function () {
    let dataSearch = $('.textemployee').val();
    let urlApi = $.helper.resolveApi("~ManageUser/GetEmployee/" + dataSearch);
    if (dataSearch.length === 0) urlApi = $.helper.resolveApi("~ManageUser/GetEmployee");

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
                bindTableEmployee(data);
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
        }, 355));
};


$(document).ready(function () {
    //bindUserApproval();    
});
