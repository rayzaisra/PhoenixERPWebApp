(function ($) {
    'use strict';

    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formCashAdvance = $('#form-CashAdvanceProject');
        var $businessUnitLookup = $("#business_unit_id");
        var $legalEntityLookup = $("#legal_entity_id");
        var $purchaseOrderLookup = $("#purchase_order_id");
        var $vendorLookup = $("#vendor_id");
        var $bankLookup = $("#payee_bank_id");
        var $accountNumberLookup = $('#payee_account_number');
        var $accountNameLookup = $('#payee_account_name');
        var accountId = $accountNumberLookup.val();

        //BEGIN::Lookup
        $accountNumberLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/Account/lookup"),
            filters: ' r.account_number LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'account_number',
                text: 'account_number',
            }
        }).on('change', function (e) {
            //console.log($countryLookup.val());
            accountId = $accountNumberLookup.val();
            $accountNameLookup.cmSelect2({
                url: $.helper.resolveApi("~/finance/Account/lookup"),
                filters: ' r.account_name LIKE @0 AND r.account_number = @1 ',
                parameters: [accountId],
                result: {
                    id: 'account_name',
                    text: 'account_name'
                }
            });
        });
        $bankLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/Bank/lookup"),
            filters: ' r.bank_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'bank_name',
            }
        });
        $businessUnitLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/BusinessUnit/lookup"),
            filters: ' r.unit_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'unit_name',
            }
        });
        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'legal_entity_name',
            }
        });

        $vendorLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
            filters: ' r.company_name LIKE @0 and r.is_vendor = 1',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'company_name',
            }
        });
        //END: Lookup
        //BEGIN::Form Validation
        _formCashAdvance.validate({
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
                mUtil.scrollTo(_formCashAdvance, -200);
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
            var data = _formCashAdvance.serializeToJSON();
            if (_formCashAdvance.valid()) {
                console.log("masuk Valid");
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/CashAdvance/save"),
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