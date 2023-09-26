(function ($) {
    'use strict';

    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
            var receiveDate = $('#ReceiveDate').data('kendoDatePicker');
            var taxDate = $('#TaxDate').data('kendoDatePicker');
            var date = $('#Date').data('kendoDatePicker');
            var periode = $('#Period').data('kendoDatePicker');

            if (new Date(date.value()).valueOf() === param.minDate.valueOf()) {
                date.value(new Date());
            }
            if (new Date(taxDate.value()).valueOf() === param.minDate.valueOf()) {
                taxDate.value(new Date());
            }
            if (new Date(receiveDate.value()).valueOf() === param.minDate.valueOf()) {
                receiveDate.value(new Date());
            }
            if (new Date(periode.value()).valueOf() === param.minDate.valueOf()) {
                periode.value(new Date());
            }
        };
        pageFunction.openForm = function () {
            $('#myModal').modal('show');
        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $("#btn-attachment").click(pageFunction.openForm);
    });
}(jQuery));