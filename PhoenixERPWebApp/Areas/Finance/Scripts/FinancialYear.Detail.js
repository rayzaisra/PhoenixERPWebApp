(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formFinancialYear = $('#form-FinancialYear');
        var $legalEntityLookup = $("#legal_entity_id");

        //BEGIN::Form Validation
        _formFinancialYear.validate({
            rules: {
                financial_year_name: {
                    required: true
                },
                legal_entity_id: {
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
                mUtil.scrollTo(_formFinancialYear, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation        

        //BEGIN::Lookup
        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/Core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        });
        //END: Lookup

        $('#start_date').datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $('#end_date').datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

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
                        url: $.helper.resolveApi("~/Finance/FinancialYear/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;

                                $('input[name="financial_year_name"]').val(record.financial_year_name);
                                $('input[name="start_date"]').val(('' + record.start_date).substring(0, 10));
                                $('input[name="end_date"]').val(('' + record.end_date).substring(0, 10));

                                $legalEntityLookup.cmSetLookup(record.legal_entity_id, record.legal_entity_name);

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
            var data = _formFinancialYear.serializeToJSON();
            console.log(data);
            console.log("masuk data");
            if (_formFinancialYear.valid()) {
                console.log("masuk Valid");
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/FinancialYear/save"),
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