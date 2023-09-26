
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('input[name=id]').val();
        var _formBusinessUnitType = $('#form-BusinessUnitType');

        //BEGIN::Form Validation

        _formBusinessUnitType.validate({
            rules: {
                business_unit_type_name: {
                    required: true
                },
                business_unit_level: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                //var errors = r.numberOfInvalids();
                //if (errors) {
                //    var message = errors === 1
                //        ? 'Please correct the following error:\n'
                //        : 'Please correct the following ' + errors + ' errors.\n';
                //    var errors = "";
                //    if (r.errorList.length > 0) {
                //        for (var x = 0; x < r.errorList.length; x++) {
                //            console.warn(r.errorList[x]);
                //            errors += "\n\u25CF " + r.errorList[x].message;
                //        }
                //    }
                //    alert(message + errors);
                //}
                mUtil.scrollTo(_formBusinessUnitType, -200);
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
                        url: $.helper.resolveApi("~/Core/BusinessUnitType/" + _detailID + "/details"),
                        success: function (r) {
                            console.log(r);
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="business_unit_type_name"]').val(record.business_unit_type_name);
                                $('input[name="business_unit_level"]').val(record.business_unit_level);

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
            var data = _formBusinessUnitType.serializeToJSON();
            if (_formBusinessUnitType.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Core/BusinessUnitType/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
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