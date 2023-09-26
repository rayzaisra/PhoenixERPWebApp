(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
        };
        pageFunction.process = function () {
            callActionFile({ formId: "form", title: "save", type: "POST", url: param.processUrl });
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $('#btn-process').click(pageFunction.process);
    });
    
}(jQuery));