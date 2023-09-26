(function ($) {
    'use strict';

    var asideLeftToggle = new mToggle('m_aside_left_minimize_toggle', {
        target: 'body',
        targetState: 'm-brand--minimize m-aside-left--minimize',
        togglerState: 'm-brand__toggler--active'
    });
    asideLeftToggle.toggleOn();
    var ListChecked = [];
    var gridUI = $('#gridDetail');
    var pageFunction = function () {
        var _detailID = $('#detail-id');
        var _formGoodReceipt = $('#form-goodreceipt');
        var $po_number = $('#po_number');
        var $affiliationLookup = $('#affiliation_id');
        var $legal_entityLookup = $("#legal_entity_id");

        //BEGIN::LOOKUP
        $affiliationLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "affiliation_name",
            dataValueField: "id",
            filter: "contains",
            autoBind: false,
            delay: 1000,
            dataSource: {
                type: "json",
                pageSize: 100,
                serverPaging: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/core/Affiliation/KendoLookup"),
                        contentType: 'application/json',
                        dataType: 'json',
                        type: 'POST',
                        cache: true
                    },
                    parameterMap: function (data, operation) {
                        var mapRequest = data;
                        return JSON.stringify(mapRequest);
                    }
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "items"
                }

            }
        });

        $legal_entityLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "legal_entity_name",
            dataValueField: "id",
            filter: "contains",
            autoBind: false,
            delay: 1000,
            dataSource: {
                type: "json",
                pageSize: 100,
                serverPaging: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/core/LegalEntity/KendoLookup"),
                        contentType: 'application/json',
                        dataType: 'json',
                        type: 'POST',
                        cache: true
                    },
                    parameterMap: function (data, operation) {
                        var mapRequest = data;
                        return JSON.stringify(mapRequest);
                    }
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "items"
                }

            }
        });

        //$type.kendoComboBox({
        //    placeholder: "Choose ..",
        //    dataTextField: "text",
        //    dataValueField: "value",
        //    delay: 1000,
        //    dataSource: [
        //        { text: "Multiple", value: true },
        //        { text: "Single", value: false }
        //    ],
        //});

        $po_number.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "purchase_order_number",
            dataValueField: "id",
            filter: "contains",
            autoBind: false,
            delay: 1000,
            dataSource: {
                type: "json",
                pageSize: 100,
                serverPaging: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/finance/PurchaseOrder/lookup"),
                        contentType: 'application/json',
                        dataType: 'json',
                        type: 'GET',
                        cache: true
                    },
                    parameterMap: function (data, operation) {
                        var mapRequest = data;
                        return JSON.stringify(mapRequest);
                    }
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "items"
                }
            }
        });

        //END::LOOKUP

        //BEGIN::Form Validation
        _formGoodReceipt.validate({
            rules: {
                request_datetime: {
                    required: true
                },
                legal_entity_id: {
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
                }
                mUtil.scrollTo(_formGoodReceipt, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation


        var loadDetailTransaction = function () {

            $.helper.kendoUI.grid($("#gridDetail"), {
                options: {
                    url: $.helper.resolveApi('~/finance/InventoryGoodsRequestDetail/KendoGrid?InventoryGoodsRequestId=' + _detailID.val())
                },
                navigatable: true,
                pageable: false,
                resizable: true,
                save: function (e) {
                    var grid = this;
                    setTimeout(function () {
                        grid.refresh();
                    })
                },
                dataSource: {
                    pageSize: 1000,
                    serverPaging: true,
                    serverSorting: false,
                    batch: true,
                    serverOperation: false,
                    aggregate: [
                        { field: "amount", aggregate: "sum" }
                    ],
                    schema: {
                        model: {
                            id: "id",
                            fields: {
                                id: { editable: true, nullable: true },
                                code: { editable: true, validation: { required: true } },
                                item_type_name: { editable: true },
                                item_name: { type: "string", editable: true, validation: { required: true } },
                                delivery_time: { type: "date", editable: true },
                                purchased_qty: { type: "number", editable: true, validation: { required: true } },
                                received_qty: { type: "number", defaultValue: 0, validation: { required: true, min: 1 } },
                                qty_remaining: { type: "number", defaultValue: 0 },
                                qty_count_in: { type: "number", defaultValue: 0 },
                                attachment: { type: "string", defaultValue: 0 },
                                remark: { type: "string", defaultValue: 0 }
                            }
                        },
                        data: "data",
                        total: "recordsTotal"
                    },
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "data",
                    total: "recordsTotal"
                },
                aggregate: [
                    { field: "amount", aggregate: "sum" }
                ],
                marvelCheckboxSelectable: {
                    enable: true,
                    listCheckedTemp: ListChecked
                },
                change: function (e) {
                    //validator.hideMessages();
                    //viewModel.set("selected", model);
                },
                dataBound: function (e) {

                    var data = e.sender.dataSource.view().toJSON();
                    var TotalAmount = 0;
                    $.each(data, function (i, v) {
                        TotalAmount += v.amount || 0;
                    });

                    var footerAmount = $('span.totalAmount');
                    $(footerAmount).html("Rp. " + TotalAmount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    $.helper.kendoUI.resizeGrid();

                },
                toolbar: ["create", "cancel"],
                columns: [
                    {
                        field: "category",
                        title: "Category",
                        width: "100px"
                    },
                    {
                        field: "brand",
                        title: "Brand",
                        width: "100px"
                    },
                    {
                        field: "sub_brand",
                        title: "Sub Brand",
                        width: "100px"
                    },
                    {
                        field: "task_detail",
                        title: "Task Detail",
                        width: "100px"
                    },
                    {
                        field: "description",
                        title: "Description",
                        width: "100px"
                    },
                    {
                        field: "qty_request",
                        title: "Qty Request",
                        width: "100px"
                    },
                    {
                        field: "qty_submitted",
                        title: "Qty Submitted",
                        width: "100px"
                    },
                    {
                        field: "remaining",
                        title: "Remaining",
                        width: "100px"
                    },
                    {
                        field: "attachment",
                        title: "Attachment",
                        width: "100px"
                    },
                ],
                editable: {
                    mode: "incell",
                    createAt: "bottom"
                }
            });
        }

        var itemTypeDataSource = new kendo.data.DataSource({
            type: "json",
            pageSize: 100,
            serverPaging: true,
            serverFiltering: true,
            transport: {
                read: {
                    url: $.helper.resolveApi("~/Core/ItemType/KendoLookup"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'POST',
                    cache: true
                },
                parameterMap: function (data) {
                    var mapRequest = data;
                    return JSON.stringify(mapRequest);
                }
            },
            schema: {
                model: {
                    id: "id"
                },
                data: "items"
            },
        });
        itemTypeDataSource.read();

        var itemDataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: $.helper.resolveApi("~/core/Item/lookup"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'GET',
                    cache: true
                },
                parameterMap: function (data) {
                    var mapRequest = data;
                    var extractOption = {
                        filters: " ( r.item_code like @0 )",
                        parameters: '%%'
                    };
                    mapRequest = $.extend(true, mapRequest, extractOption);
                    return mapRequest;

                }
            },
            schema: {
                model: {
                    id: "id"
                },
                data: "items",
                total: "totalItems"
            },
        });

        itemDataSource.read();

        function itemEditor(container, options) {

            var remaining_qty = Number(options.model.purchased_qty) - Number(options.model.received_qty);
            options.model.qty_remaining = remaining_qty;
            $('<input name="' + options.field + '" value="' + remaining_qty + '" disabled/>');
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
            tooltipElement.appendTo(container);
        }

        function deliveryTimeEditor(container, options) {

            var dateString = kendo.toString(options.model.OrderDate, "yyyy/MM/dd");

            var $input = $('<input class="marvel-datepicker" name="' + options.field + '" id="' + options.field + '" value="' + dateString + '" />').appendTo(container);
            $input.kendoDatePicker({
                format: 'yyyy/MM/dd'
            });

        }

        function itemTypeEditor(container, options) {

            $('<input required name="' + options.field + '" width="250px"/>')
                .appendTo(container)
                .kendoComboBox({
                    placeholder: "Choose ..",
                    filter: "contains",
                    autoBind: false,
                    dataTextField: "type_name",
                    dataValueField: "type_name",
                    value: options.model.item_type_id,
                    text: options.model.type_name,
                    dataSource: itemTypeDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        try {
                            options.model.item_type_id = equipmentModel.id;
                            options.model.set("type_name", equipmentModel.type_name);

                            //reset all row related
                            options.model.item_id = null;
                            options.model.item_code = null;
                            options.model.item_name = null;
                            options.model.uom_id = null;
                            options.model.uom_name = null;
                        } catch (e) {

                        }

                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
        }

        var currencyDataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: $.helper.resolveApi("~/Core/Currency/lookup"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'GET',
                    cache: true
                },
                parameterMap: function (data) {
                    var mapRequest = data;
                    var extractOption = {
                        filters: " (r.currency_name like @0 or r.currency_code like @0 )",
                        parameters: '%%'
                    };
                    mapRequest = $.extend(true, mapRequest, extractOption);
                    return mapRequest;

                }
            },
            schema: {
                model: {
                    id: "id"
                },
                data: "items",
                total: "totalItems"
            },
        });
        currencyDataSource.read();
        function currencyEditor(container, options) {
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoComboBox({
                    autoBind: false,
                    dataTextField: "currency_code",
                    dataValueField: "currency_code",
                    value: options.model.currency_id,
                    text: options.model.currency_code,
                    dataSource: currencyDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        try {
                            options.model.currency_id = equipmentModel.id;
                            options.model.set("currency_code", equipmentModel.currency_code);
                        } catch (e) {

                        }

                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
        }
        var uomDataSource = new kendo.data.DataSource({
            type: "json",
            pageSize: 100,
            serverPaging: true,
            serverFiltering: true,
            transport: {
                read: {
                    url: $.helper.resolveApi("~/core/Uom/KendoLookup"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'POST'
                },
                parameterMap: function (data, operation) {
                    var mapRequest = data;
                    return JSON.stringify(mapRequest);
                }
            },
            schema: {
                model: {
                    id: "id"
                },
                data: "items"
            }
        });
        //uomDataSource.read();
        function uomEditor(container, options) {
            $('<input name="' + options.field + '"/>')
                .appendTo(container)
                .kendoComboBox({
                    autoBind: false,
                    filter: "contains",
                    dataTextField: "uom_name",
                    dataValueField: "uom_name",
                    value: options.model.uom_id,
                    text: options.model.uom_name,
                    dataSource: uomDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        try {
                            options.model.uom_id = equipmentModel.id;
                            options.model.set("uom_name", equipmentModel.uom_name);
                        } catch (e) {

                        }

                    }
                });
        }

        function reloadGrid() {
            gridUI.data("kendoGrid").dataSource.read();
            gridUI.data('kendoGrid').refresh();
        }

        gridUI.find(".k-grid-toolbar").on("click", ".k-grid-Reload", function (e) {
            e.preventDefault();
            reloadGrid();
        });


        //BEGIN::Load Detail
        var loadDetailData = function () {

            if (_detailID.val() !== '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Processing..."
                }),
                    $.ajax({
                        type: "GET",
                        dataType: 'json',
                        url: $.helper.resolveApi("~/Finance/InventoryGoodsRequest/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {

                                var record = r.data;
                                $('#gr_number').val(record.goods_receipt_number);
                                $('#gr_number').prop('disabled', true);
                                $('#delivery_order_number').val(record.delivery_order_number);
                                $('#delivery_place').val(record.delivery_place);
                                $('#delivery_term').val(record.delivery_term);

                                $.helper.kendoUI.combobox.setValue($po_number, { id: record.purchase_order_id, affiliation_name: record.purchase_order_number });
                                $.helper.kendoUI.combobox.setValue($receiptByLookup, { id: record.purchase_order_id, affiliation_name: record.receiver_name });

                                $('input[name="request_datetime"]').val(moment(record.request_datetime).format('MM/DD/YYYY'));
                                $('#remarks').val(record.remarks);
                                $('#request_number').text(record.request_number);
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
        //END::Load Detail

        //BEGIN::EVENT BUTTON
        $('#btn-save').click(function (e) {
            var btn = $(this);
            if (_formGoodReceipt.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                var header = _formGoodReceipt.serializeToJSON();

                var transactionDetailKendo = $("#gridDetail").data("kendoGrid");
                var data = {
                    good_receipt: header,
                    good_receipt_details: transactionDetailKendo.dataSource.view().toJSON()
                };
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/InventoryGoodsRequest/saveasdraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            _detailID.val(r.data.recordID);
                            toastr.success(r.status.message, "Information");
                            loadDetailData();
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    },
                    error: function (r) {
                        swal(
                            '' + r.status,
                            r.statusText,
                            'error'
                        );
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    }
                });

            }
        });
        //END::EVENT BUTTON

        var DeleteData = function (ArrID) {
            swal({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (e) {
                if (e.value) {
                    mApp.block("#m_blockui_list", {
                        overlayColor: "#000000",
                        type: "loader",
                        state: "primary",
                        message: "Processing..."
                    });

                    $.ajax({
                        type: "DELETE",
                        dataType: 'json',
                        contentType: 'application/json',
                        url: $.helper.resolveApi("~/Finance/InventoryGoodsRequestDetail/delete"),
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                });
                            } else {
                                swal(
                                    'Information',
                                    r.status.message,
                                    'info'
                                );
                            }
                            reloadGrid();
                        },
                        error: function (e, t, s) {
                            swal(
                                'Information',
                                'Ooops, something went wrong !',
                                'info'
                            );
                        }
                    }).then(setTimeout(function () {
                        mApp.unblock("#m_blockui_list");
                    }, 2e3));
                }


            });
        };

        return {
            init: function () {
                //initialize
                loadDetailData();
                loadDetailTransaction();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));
