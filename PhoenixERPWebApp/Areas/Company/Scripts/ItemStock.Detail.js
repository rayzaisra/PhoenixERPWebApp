(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('input[name=id]').val();
        var _formItemStock = $('#form-itemstock');
        var $itemLookup = $("#item_id");
        var $warehouseLookup = $("#warehouse_id");
        var $unitLookup = $("#uom_id");

        //Begin:: Document Ready
        $itemLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Item/lookup"),
            filters: ' r.item_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'item_name',
            }
        });

        $warehouseLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Warehouse/lookup"),
            filters: ' r.warehouse_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'warehouse_name',
            }
        });

        $unitLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Uom/lookup"),
            filters: ' r.unit_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'unit_name',
            }
        });


        //End:: Document Ready

        /* Create and log the result. */
        //BEGIN::Form Validation
        _formItemStock.validate({
            rules: {
                item_id: {
                    required: true
                },
                warehouse_id: {
                    required: true
                },
                uom_id: {
                    required: true
                },
                qty: {
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
                mUtil.scrollTo(_formItemStock, -200);
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
                        url: $.helper.resolveApi("~/core/ItemStock/" + _detailID + "/details"),
                        success: function (r) {
                            console.log(r);
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="qty"]').val(record.qty);
                                console.warn(record)
                                $itemLookup.cmSetLookup(record.item_id, record.item_name);
                                $warehouseLookup.cmSetLookup(record.warehouse_id, record.warehouse_name);
                                $unitLookup.cmSetLookup(record.uom_id, record.unit_name);
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
                                'Ooops, Something when wrong !',
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
            var data = _formItemStock.serializeToJSON();
            if (_formItemStock.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/ItemStock/save"),
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


