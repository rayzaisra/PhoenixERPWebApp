
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('input[name=id]').val();
        var _formItem = $('#form-item');
        var $itemCategoryLookup = $("#item_category_id");
        var $uomLookup = $("#base_uom");

        //BEGIN::LOOKUP

        $itemCategoryLookup.cmSelect2({
            url: $.helper.resolveApi("~/Core/ItemCategory/lookup"),
            filters: ' r.category_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'category_name',
            }
        });

        $uomLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Uom/lookup"),
            filters: ' r.unit_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'uom_name',
            }
        });

        //END::LOOKUP

        //BEGIN::Form Validation
        _formItem.validate({
            rules: {
                item_name: {
                    required: true
                },
                item_code: {
                    required: true
                },
                base_uom: {
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
                mUtil.scrollTo(_formItem, -200);
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
                        url: $.helper.resolveApi("~/core/Item/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="item_code"]').val(record.item_code);
                                $('input[name="item_name"]').val(record.item_name);

                                $itemCategoryLookup.cmSetLookup(record.item_category_id, record.category_name);
                                $uomLookup.cmSetLookup(record.base_uom, record.unit_name);

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
            var data = _formItem.serializeToJSON();
            if (_formItem.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/Item/save"),
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


