(function ($) {
    'use strict';

    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formCurrencyExchange = $('#form-CurrencyExchange');
        var $sourceCurrencyLookup = $("#source_currency");
        var $targetCurrencyLookup = $("#target_currency");  

        //BEGIN::Lookup

        $sourceCurrencyLookup.cmSelect2({
            url: $.helper.resolveApi("~/Core/Currency/lookup"),
            filters: ' r.currency_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'currency_name'
            }
        });

        $targetCurrencyLookup.cmSelect2({
            url: $.helper.resolveApi("~/Core/Currency/lookup"),
            filters: ' r.currency_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'currency_name'
            }
        });

        //END: Lookup

        //BEGIN::Form Validation
        _formCurrencyExchange.validate({
            rules: {
                source_currency: {
                    required: true
                },
                target_currency: {
                    required: true
                },
                exchange_rate: {
                    required: true
                },
                start_date: {
                    required: true
                },
                end_date: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                var errors = r.numberOfInvalids();
                if (errors) {
                    var message = errors === 1
                        ? 'Please correct the following error:\n'
                        : 'Please correct the following ' + errors + ' errors.\n';

                    if (r.errorList.length > 0) {
                        for (var x = 0; x < r.errorList.length; x++) {
                            console.warn(r.errorList[x]);
                            errors += "\n\u25CF " + r.errorList[x].message;
                        }
                    }
                    alert(message + errors);
                }
                mUtil.scrollTo(_formCurrencyExchange, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::Load Datatable
        var loadDetailData = function () {

            if (_detailID !== '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Processing..."
                }),
                    $.ajax({
                        type: "GET",
                        dataType: 'json',
                        url: $.helper.resolveApi("~/Finance/CurrencyExchange/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="exchange_rate"]').val(record.exchange_rate);
                                $('input[name="start_date"]').val(record.start_date);
                                $('input[name="end_date"]').val(record.end_date);

                                $sourceCurrencyLookup.cmSetLookup(record.source_currency, record.source_currency_name);
                                $targetCurrencyLookup.cmSetLookup(record.target_currency, record.target_currency_name);

                            } else {
                                swal(
                                    'Information',
                                    r.status.message,
                                    'info'
                                );
                            }
                        },
                        error: function (e, t, s) {
                            swal(
                                'Information',
                                'Ooops, something went wrong !',
                                'info'
                            );
                        }
                    }).then(setTimeout(function () {
                        mApp.unblockPage();
                    }, 2e3));

            }

        };
        //END::Load Datatable

        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formCurrencyExchange.serializeToJSON();
            if (_formCurrencyExchange.valid()) {
                console.log("masuk Valid");
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/CurrencyExchange/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            toastr.success(r.status.message, "Information");
                            console.log(r.data);
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
                    },
                    error: function (r) {
                        swal(
                            '' + r.status,
                            r.statusText,
                            'error'
                        );
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    }
                }).then(function () { btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); });

            }
        });


        //BEGIN::EVENT BUTTON
        //do stuff ...
        //END::EVENT BUTTON


        //BEGIN::EVENT BUTTON

        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
                loadDetailData();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));