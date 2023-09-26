(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
        };
        pageFunction.process = function () {
            callActionFile({ formId: "form", title: "process", type: "POST", url: param.processUrl });
        };
        pageFunction.save = function () {
            callActionFile({ formId: "form", title: "save", type: "POST", url: param.saveUrl });
        };
        pageFunction.submit = function () {
            callActionFile({ formId: "form", title: "submit", type: "POST", url: param.submitUrl });
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $('#btn-process').click(pageFunction.process);
        $('#btn-save').click(pageFunction.save);
        $('#btn-submit').click(pageFunction.submit);
    });

}(jQuery));