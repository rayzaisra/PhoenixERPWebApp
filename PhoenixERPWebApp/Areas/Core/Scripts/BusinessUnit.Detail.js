
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formBusinessUnit = $('#form-businessunit');
        var $BusinessUnitTypeLookup = $("#business_unit_type_id");
        var $ParentUnitId = $('input[name=parent_unit]');
        //BEGIN::LOOKUP
        $BusinessUnitTypeLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/BusinessUnitType/lookup/?parentunitid=" + $ParentUnitId.val() ),
            filters: ' r.business_unit_type_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'business_unit_type_name',
            }
        });

        //END::LOOKUP


        //BEGIN::Form Validation

        _formBusinessUnit.validate({
            rules: {
                unit_name: {
                    required: true
                },
                unit_email: {
                    required: true,
                    email:true
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
                mUtil.scrollTo(_formBusinessUnit, -200);
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
                        url: $.helper.resolveApi("~/core/BusinessUnit/" + _detailID + "/details"),
                        success: function (r) {
                            console.log(r);
                            if (r.status.success && r.data) {
                                var record = r.data;
                                console.log(record);
                                $('input[name="unit_name"]').val(record.unit_name);
                                $('input[name="unit_phone"]').val(record.unit_phone);

                                $BusinessUnitTypeLookup.cmSetLookup(record.business_unit_type_id, record.business_unit_type_name);

                                $('input[name="unit_fax"]').val(record.unit_fax);
                                $('input[name="unit_email"]').val(record.unit_email);
                                $('#unit_address').text(record.unit_address);
                                
                                    

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
            var data = _formBusinessUnit.serializeToJSON();
            if (_formBusinessUnit.valid()) {
                console.log("masuk Valid");
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/BusinessUnit/save"),
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


