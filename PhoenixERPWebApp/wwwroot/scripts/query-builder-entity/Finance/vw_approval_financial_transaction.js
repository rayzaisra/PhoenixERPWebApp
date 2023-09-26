(function ($) {

    var vw_approval_financial_transaction = {
        vw_approval_financial_transaction: {
            filterSettings: function () {
                var options = [
                    {
                        id: 'ft.transaction_number',
                        label: 'Transaction Number',
                        type: 'string',
                        operators: ['equal', 'not_equal']
                    },
                    {
                        id: 'ft.transaction_datetime',
                        label: 'Transaction Date',
                        type: 'date',
                        validation: {
                            format: 'YYYY/MM/DD'
                        },
                        operators: ['between', 'equal'],
                        plugin: 'datepicker',
                        plugin_config: {
                            format: 'yyyy/mm/dd',
                            todayBtn: 'linked',
                            todayHighlight: true,
                            autoclose: true
                        }
                    }

                ];
                return options;
            }
        }
    };


    $.helper.querybuilder = $.extend({},
        vw_approval_financial_transaction);

}(jQuery));