
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('input[name=id]').val();
        var _formCostSharingAllocation = $('#form-costsharingallocation');

       
        //BEGIN::Form Validation

        _formCostSharingAllocation.validate({
            rules: {
                cost_sharing_allocation_name: {
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
                mUtil.scrollTo(_formCostSharingAllocation, -200);
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
                        url: $.helper.resolveApi("~/finance/CostSharingAllocation/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="cost_sharing_allocation_name"]').val(record.cost_sharing_allocation_name);
                                var startDateFormat = moment(record.start_date, "YYYY-MM-DD").format("MM/DD/YYYY");
                                var endDateFormat = moment(record.end_date, "YYYY-MM-DD").format("MM/DD/YYYY");

                                $('input[name="start_date"]').val(startDateFormat);
                                $('input[name="end_date"]').val(endDateFormat);


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

        //BEGIN::Date Picker
        var datePicker = function () {
            var t;
            t = mUtil.isRTL() ? {
                leftArrow: '<i class="la la-angle-right"></i>',
                rightArrow: '<i class="la la-angle-left"></i>'
            } : {
                    leftArrow: '<i class="la la-angle-left"></i>',
                    rightArrow: '<i class="la la-angle-right"></i>'
                };

            return {
                init: function () {
                    $("#m_datepicker_5").datepicker({
                        rtl: mUtil.isRTL(),
                        todayHighlight: !0,
                        templates: t
                    });
                }
            }
        }();

        //END::Date Picker


        //BEGIN::EVENT BUTTON

        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formCostSharingAllocation.serializeToJSON();
            if (_formCostSharingAllocation.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/CostSharingAllocation/save"),
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
                datePicker.init();
                loadDetailData();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


