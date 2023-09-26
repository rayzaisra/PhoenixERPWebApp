(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
        };
        pageFunction.editGridRow = function (e) {
            var item = dataItem(e);
            window.location.href = param.editUrl + item.Id;
        };
        pageFunction.deleteGridRow = function (e) {
            deleteRow(e, param.deleteUrl);
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));