(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
        };
        pageFunction.reject = function () {
            //reject function
        };
        pageFunction.approve = function () {
            //approve function
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $('#btn-reject').click(pageFunction.reject);
        $('#btn-approve').click(pageFunction.approve);

        $("#grid tbody").on("dblclick", "tr", function (e) {

            var rowElement = this;
            var row = $(rowElement);
            var grid = $("#grid").getKendoGrid();
            var item = grid.dataItem(row);
            console.log(item);

            $.ajax({
                type: 'GET',
                url: param.indexScheduleUrl,
                data: {
                    Id: item.Id
                },
                success: function (result) {
                    console.log(result);
                    $('#scheduleModal').html(result);

                    $('#scheduleModal').modal('show');
                    mApp.unblock("#m_blockui_list");
                },
                error: function (e, t, s) {
                    //console.log(e)
                    console.log(e);
                    mApp.unblock("#m_blockui_list");
                }
            }).then(setTimeout(function () {
                mApp.unblock("#m_blockui_list");
            }, 2e3));
        });
    });
}(jQuery));