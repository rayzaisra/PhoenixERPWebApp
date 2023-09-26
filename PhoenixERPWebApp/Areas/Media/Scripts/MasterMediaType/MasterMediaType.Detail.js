(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
        };
        pageFunction.save = function () {
            callActionFile({ formId: "form", title: "save", type: "POST", url: param.saveUrl });
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $('#btn-save').click(pageFunction.save);
    });
}(jQuery));