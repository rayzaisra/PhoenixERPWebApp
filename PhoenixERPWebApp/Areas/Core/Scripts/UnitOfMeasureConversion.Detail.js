(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('input[name=id]').val();
        var _formUomConversion = $('#form-UomConversion');
        var $uom1Lookup = $("#uom_1");
        var $uom2Lookup = $("#uom_2");

        //BEGIN::LOOKUP

        $uom1Lookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Uom/lookup"),
            filters: ' r.unit_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'unit_name',
            }
        });

        $uom2Lookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Uom/lookup"),
            filters: ' r.unit_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'unit_name',
            }
        });

        //END::LOOKUP

        //BEGIN::Form Validation
        _formUomConversion.validate({
            rules: {
                uom_1: {
                    required: true
                },
                value_1: {
                    required: true
                },
                uom_2: {
                    required: true
                },
                value_2: {
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
                mUtil.scrollTo(_formUomConversion, -200);
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
                        url: $.helper.resolveApi("~/core/UomConversion/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="value_1"]').val(record.value_1);
                                $('input[name="value_2"]').val(record.value_2);

                                $uom1Lookup.cmSetLookup(record.uom_1, record.unit_name_1);
                                $uom2Lookup.cmSetLookup(record.uom_2, record.unit_name_2);

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

        //BEGIN::EVENT BUTTON
        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formUomConversion.serializeToJSON();
            console.log(data);
            if (_formUomConversion.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/UomConversion/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            toastr.success(r.status.message, "Information");                    
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


