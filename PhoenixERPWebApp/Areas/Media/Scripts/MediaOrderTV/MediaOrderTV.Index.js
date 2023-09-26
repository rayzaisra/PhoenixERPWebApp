(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
        };
        pageFunction.editGridRow = function (e) {
            var grid = $("#grid").data("kendoGrid");
            var dataItem = grid.dataItem($(this).closest("tr"));
            window.location.href = param.editUrl + dataItem.Id;
        };
        pageFunction.deleteGridRow = function (e) {
            var data = $(this).closest("tr");
            deleteRow(data, param.deleteUrl);
            counter = 1;
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $("#grid").on("click", ".edit", pageFunction.editGridRow);
        $("#grid").on("click", ".delete", pageFunction.deleteGridRow);
    });
}(jQuery));