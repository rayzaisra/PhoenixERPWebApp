(function ($) {
    'use strict';

    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formWarehouse = $('#form-Warehouse');
        var $countryLookup = $("#warehouse-country_id");        
        var $cityLookup = $("#warehouse-city_id");
        var countryId = $countryLookup.val();

        //BEGIN::LOOKUP

        $countryLookup.cmSelect2({
            url: $.helper.resolveApi("~/Core/Country/lookup"),
            filters: ' r.country_name LIKE @0 ',
            //parameters: [],
            result: {
                id: 'id',
                text: 'country_name'
            }
        }).on('change', function (e) {
            //console.log($countryLookup.val());
            countryId = $countryLookup.val();

            $cityLookup.cmSelect2({
                url: $.helper.resolveApi("~/Core/City/lookup"),
                filters: ' r.city_name LIKE @0 AND r.country_id = @1 ',
                parameters: [countryId],
                result: {
                    id: 'id',
                    text: 'city_name'
                }
            });
        });

        //END::LOOKUP

        //BEGIN::Form Validation

        _formWarehouse.validate({
            rules: {
                warehouse_name: {
                    required: true
                },
                address: {
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
                mUtil.scrollTo(_formWarehouse, -200);
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
                        url: $.helper.resolveApi("~/Core/Warehouse/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;

                                $('input[name="warehouse_name"]').val(record.warehouse_name);
                                $('input[name="address"]').val(record.address);
                                $('input[name="zip_code"]').val(record.zip_code);
                                $('input[name="phone"]').val(record.phone);
                                $('input[name="fax"]').val(record.fax);

                                $countryLookup.cmSetLookup(record.country_id, record.country_name);
                                $cityLookup.cmSetLookup(record.city_id, record.city_name);

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
            var data = _formWarehouse.serializeToJSON();
            console.log(data);
            console.log("masuk data");
            if (_formWarehouse.valid()) {
                console.log("masuk Valid");
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Core/Warehouse/save"),
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