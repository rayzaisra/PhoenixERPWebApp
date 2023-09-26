
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('input[name=id]').val();
        var _formAccount = $('#form-account');
        var _formCostSharing = $('#form-costsharingallocation');
        var $accountGroupLookup = $("#account_group_id");
        var $accountParentLookup = $("#account_parent_id");
        var $accountLevel = $("#account_level");
        var $currency = $("#currency_id");
        var $legalEntity = $("#legal_entity_id");
        var legalEntityId = $legalEntity.val();

        //BEGIN::LOOKUP

        $accountGroupLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/AccountGroup/lookup"),
            filters: ' r.account_group_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'account_group_name'
            }
        });

        $accountParentLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/Account/lookup"),
            filters: ' r.account_number LIKE @0 OR r.account_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'account_name'
            }
        });

        $currency.cmSelect2({
            url: $.helper.resolveApi("~/core/currency/lookup"),
            filters: ' r.currency_code LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'currency_code'
            }
        });

        $accountLevel.cmSelect2({
            url: $.helper.resolveApi("~/finance/Account/LevelsLookup"),
            result: {
                id: 'id',
                text: 'text'
            }
        });

        $legalEntity.cmSelect2({
            url: $.helper.resolveApi("~/Core/LegalEntity/Lookup"),
            filters: ' r.legal_entity_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        }).on('change', function (e) {
            legalEntityId = $legalEntity.val();

            $accountParentLookup.cmSelect2({
                url: $.helper.resolveApi("~/finance/Account/lookup"),
                filters: ' ( r.account_name LIKE @0 OR r.account_number LIKE @0 ) AND r.legal_entity_id = @1 ',
                parameters: [legalEntityId],
                result: {
                    id: 'id',
                    text: 'account_name'
                }
            });
        });

        //END::LOOKUP

        var counter = {};

        //Begin:: Function Add Form
        function addForm(data, callBack) {
            if (!counter[data[2]]) {
                counter[data[2]] = 0;
            }

            var num = 0;
            var count = $('div.' + data[2] + '[data-idx="' + num + '"]').length;
            while (count > 0) {
                num++;
                count = $('div.' + data[2] + '[data-idx="' + num + '"]').length;
            }

            var dataRow = $(data[1]).html();
            var row = document.createElement("div");

            dataRow = dataRow.replace(/\%index\%/gi, num);

            $(row).attr({
                "class": "col-lg-12 row " + data[2],
                "data-idx": num
            }).html(dataRow);

            $(data[0]).append(row);

            $('div.' + data[2] + '[data-idx="' + num + '"] button.btn-remove-row').off('click').on('click', function () {
                var idx = $(this).attr('data-index');

                if ($('div.' + data[2]).length > 1) {
                    $('div.' + data[2] + '[data-idx="' + idx + '"]').remove();
                    counter[data[2]] = $('div.' + data[2]).length;
                }
            });

            // bwt generate select2 dan plugin yg lainnya
            if (callBack) {
                callBack(num, data);
            }

            counter[data[2]] = $('div.' + data[2]).length;
        }
        //End:: Function Add Form



        //BEGIN::Form Validation

        _formCostSharing.validate({
            rules: {
                "cost_sharing_allocation.start_date": {
                    required: true
                },
                "cost_sharing_allocation.end_date": {
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
                mUtil.scrollTo(_formCostSharing, -200);
            },
            submitHandler: function (e) { }
        });

        _formAccount.validate({
            rules: {
                account_group_id: {
                    required: true
                },
                account_name: {
                    required: true
                },
                account_number: {
                    required: true
                },
                legal_entity_id: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                var errors = r.numberOfInvalids();
                if (errors) {
                    //var message = errors === 1
                    //    ? 'Please correct the following error:\n'
                    //    : 'Please correct the following ' + errors + ' errors.\n';
                    //var errors = "";
                    //if (r.errorList.length > 0) {
                    //    for (var x = 0; x < r.errorList.length; x++) {
                    //        console.warn(r.errorList[x]);
                    //        errors += "\n\u25CF " + r.errorList[x].message;
                    //    }
                    //}
                    //alert(message + errors);
                }
                mUtil.scrollTo(_formAccount, -200);
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
                        url: $.helper.resolveApi("~/finance/Account/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;

                                $('input[name="account_name"]').val(record.account_name);
                                $('input[name="account_number"]').val(record.account_number);
                                $('input[name="account_level"]').val(record.account_level);

                                $('#tabs_account_setup > .cost-sharing').show();

                                $accountGroupLookup.cmSetLookup(record.account_group_id, record.account_group_name);
                                $accountParentLookup.cmSetLookup(record.account_parent_id, record.account_parent_name);
                                $accountLevel.cmSetLookup(record.account_level, record.account_level);
                                $currency.cmSetLookup(record.currency_id, record.currency_code);
                                $legalEntity.cmSetLookup(record.legal_entity_id, record.legal_entity_name);

                                //LoadAllocationDetail
                                loadAllocationDetail(_detailID);

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
            else {
                $('.no-record').removeClass("collapse");
                $('#tabs_account_setup > .cost-sharing').hide();
            }
        };

        //BEGIN:: Init Allocation Detail
        var loadAllocationDetail = function (accountId) {
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: $.helper.resolveApi("~/finance/Account/" + accountId + "/sharingalloaction"),
                success: function (r) {
                    if (r.status.success && r.data) {

                        var recordCostSharing = r.data.cost_sharing_allocation;
                        var recordCostSharingDetail = r.data.cost_sharing_allocation_detail;

                        //Begin::set form Value cost sharing allocation
                        $('input[name="cost_sharing_allocation.start_date"]').val(recordCostSharing.start_date);
                        $('input[name="cost_sharing_allocation.end_date"]').val(recordCostSharing.end_date);
                        //End::form Value cost sharing allocation

                        $.each(recordCostSharingDetail, function (i, v) {
                            addForm(["#allocation_sharing_detail", ".template-allocation_detail", "counter-allocation_detail"], function (idx, data) {

                                $('input[name="cost_sharing_allocation_detail.cost_sharing_allocation_id[' + i + ']"]')
                                    .val(v.cost_sharing_allocation_id);

                                $('input[name="cost_sharing_allocation_detail.cost_sharing_allocation_percent[' + i + ']"]')
                                    .val(v.cost_sharing_allocation_percent)

                                $('select.business_unit-lookup[name="cost_sharing_allocation_detail.business_unit_id[' + i + ']')
                                    .cmSetLookup(v.business_unit_id, v.unit_name);

                            });
                        });

                        if (recordCostSharing.is_active) {
                            $('.no-record').addClass("collapse");
                            $('#enable_cost_sharing').prop('checked', false);
                            $('#enable_cost_sharing').trigger('click');
                            $('#enable_cost_sharing').attr('disabled', '');
                        }
                        //BEGIN::CONTROL FORM
                        $('#btn-remove_cost_sharing').show();
                        //END::CONTROL FORM


                    } else {
                        $('.no-record').removeClass("collapse");
                        initAllocationDetail();
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
        };

        var initAllocationDetail = function () {

            addForm(["#allocation_sharing_detail", ".template-allocation_detail", "counter-allocation_detail"], function (idx, data) {
                // select2 Business Unit lookup
                $('div.' + data[2] + '[data-idx="' + idx + '"] select.business_unit-lookup[name="cost_sharing_allocation_detail.business_unit_id[' + idx + ']"]').cmSelect2({
                    url: $.helper.resolveApi("~/core/BusinessUnit/lookup"),
                    filters: ' r.unit_name LIKE @0 ',
                    //paramaters: [],
                    result: {
                        id: 'id',
                        text: 'unit_name'
                    }
                });
            });
        };

        //END:: Init Allocation Detail
        //END::Load Datatable

        //BEGIN::EVENT BUTTON

        $('#btn-remove_cost_sharing').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            $.ajax({
                type: "DELETE",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/finance/Account/removeallocationsharing?accountid=" + _detailID),
                success: function (r) {
                    console.log(r);
                    if (r && r.status.success) {
                        toastr.success("Allocation Sharing has been remove", "Information");

                        //BEGIN::CONTROL FORM
                        $('#enable_cost_sharing').prop('checked', false);
                        _formCostSharing.hide();
                        $('#enable_cost_sharing').removeAttr('disabled', '');
                        $('.no-record').removeClass("collapse");
                        //END::CONTROL FORM
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

        });

        $('#btn-create_cost_sharing').click(function () {
            var btn = $(this);
            var data = $("form#form-costsharingallocation").cmserializeToJSON();

            if (_formCostSharing.valid()) {
                console.log(data);
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/Account/createcostsharing?accountid=" + $('input[name=id]').val()),
                    data: data,
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            toastr.success(r.status.message, "Information");
                            //BEGIN::CONTROL FORM
                            $('#enable_cost_sharing').attr('disabled', '');
                            $('#btn-remove_cost_sharing').show();
                            $('.no-record').addClass("collapse");
                            //END::CONTROL FORM
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

        $('#btn-add-allocation').click(function () {
            initAllocationDetail();
        });

        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formAccount.serializeToJSON();
            if (_formAccount.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/Account/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            _detailID = r.data.recordID;
                            toastr.success(r.status.message, "Information");
                            $('#tabs_account_setup > .cost-sharing').show();
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
                    $("#allocation_sharing_range").datepicker({
                        rtl: mUtil.isRTL(),
                        todayHighlight: !0,
                        templates: t
                    });
                }
            }
        }();

        //END::Date Picker

        //BEGIN::CONTROL FORM
        $('#enable_cost_sharing').change(function () {
            if (this.checked) {
                _formCostSharing.show();
                datePicker.init();
            } else { _formCostSharing.hide(); }

        });

        //END::CONTROL FORM

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


