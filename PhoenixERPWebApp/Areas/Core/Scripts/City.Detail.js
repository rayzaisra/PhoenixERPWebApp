
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formCity = $('#form-city');
        var $countryLookup = $("#country_id");
        //BEGIN::LOOKUP

        $countryLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Country/lookup"),
            filters: ' r.country_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'country_name',
            }
        });

        //END::LOOKUP

        //BEGIN::Form Validation
        _formCity.validate({
            rules: {
                city_code: {
                    required: true
                },
                city_name: {
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
                mUtil.scrollTo(_formCity, -200);
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
                        url: $.helper.resolveApi("~/core/City/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="city_code"]').val(record.city_code);
                                $('input[name="city_name"]').val(record.city_name);

                                $countryLookup.cmSetLookup(record.country_id, record.country_name);

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
            var data = _formCity.serializeToJSON();
            if (_formCity.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/City/save"),
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


