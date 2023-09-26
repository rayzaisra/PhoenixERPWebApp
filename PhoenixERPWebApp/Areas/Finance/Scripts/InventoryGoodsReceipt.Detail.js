
function itemCodeEditable(dataItem) {
    
    return (dataItem.type_name == "Inventory" || dataItem.item_id == null) ? true : false;
}

function itemUomEditable(dataItem) {
    return dataItem.type_name == "Inventory" ? false : true;
}

function backToPageBefore() {
    window.history.back();
}

function CountRemainingQty(e) {
    var grid = $('#gridDetail').data("kendoGrid");
    grid.table.on("click", "tr > td", function (e) {
        if (grid.cellIndex(this) == 6 || grid.cellIndex(this) == 5) {
            
            e.preventDefault();
            e.stopPropagation();
            var dataItem = grid.dataSource.get();
            var po_qty = dataItem.purchased_qty;
            var rcv_qty = dataItem.received_qty;
            var remain_qty = (po_qty - rcv_qty < 0) ? 0 : po_qty - rcv_qty;
            dataItem.set('qty_remaining', remain_qty);
            dataItem.set('qty_count_in', rcv_qty);
        }
    });
}

(function ($) {
    'use strict';

    var asideLeftToggle = new mToggle('m_aside_left_minimize_toggle', {
        target: 'body',
        targetState: 'm-brand--minimize m-aside-left--minimize',
        togglerState: 'm-brand__toggler--active'
    });
    asideLeftToggle.toggleOn();
    var gridUI = $('#gridDetail');
    var pageFunction = function () {
        var _detailID = $('#detail-id');
        var _formGoodReceipt = $('#form-goodreceipt');
        var $po_number = $('#po_number');
        var $purchaseOrderStatusLookup = $("#purchase_order_status_id");
        var $receiptByLookup = $("#receipt_by");
        var $type = $("#invoicing_type_id");
        var $companyLookup = $("#company_id");
        var $datePicker = $('#goods_receipt_datetime');

        $datePicker.kendoDatePicker({
            format: 'yyyy/MM/dd'
        });

        //BEGIN::LOOKUP
        $purchaseOrderStatusLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "status_name",
            dataValueField: "id",
            filter: "contains",
            autoBind: true,
            delay: 1000,
            dataSource: {
                type: "json",
                pageSize: 100,
                serverPaging: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/finance/PurchaseOrderStatus/KendoLookup"),
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

        $receiptByLookup.kendoMultiSelect({
            placeholder: "Choose ..",
            dataTextField: "app_fullname",
            dataValueField: "id",
            filter: "contains",
            autoBind: false,
            footerTemplate: 'Total #: instance.dataSource.total() # items found',
            itemTemplate: '<span class="k-state-default" style="background-image: url(\'../content/web/Customers/#:data.id#.jpg\')"></span>' +
                '<span class="k-state-default"><h3>#: data.app_fullname #</h3><p>#: data.app_fullname #</p></span>',
            tagTemplate: '<span class="selected-value" style="background-image: url(\'../content/web/Customers/#:data.id#.jpg\')"></span><span>#:data.app_fullname#</span>',
            dataBound: function (e) {
                var newArr = [];
                for (var i = 0; i < e.sender.dataSource._data.length; i++) {
                    newArr.push({ id: e.sender.dataSource._data[i].id, text: e.sender.dataSource._data[i].app_fullname });
                }
            },
            delay: 1000,
            dataSource: {
                type: "json",
                pageSize: 100,
                serverPaging: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/core/User/lookup"),
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

        $type.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "type_name",
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
                        url: $.helper.resolveApi("~/finance/InvoicingType/lookup"),
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

        $companyLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "company_name",
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
                        url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
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

        function detailInit(e) {
            
            var rowData = e.data;
            var detailRow = e.detailRow;

            kendo.ui.progress(detailRow.find(".k-upload-init-progress"), true);

            $.ajax({
                type: "GET",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/core/FileUpload/entity/record?entityId=7d9759ba-4f4b-6484-134d-469d20003f47&recordId=" + rowData.inventory_goods_receipt_detail_id),
                success: function (r) {
                    var data = (r.status.success ? r.data : []);
                    detailRow.find(".file-upload").kendoUpload({
                        template: $("#fileTemplate").html(),
                        async: {
                            saveUrl: $.helper.resolveApi("~/core/FileUpload/entity?entityId=51a46d55-6d81-2077-681d-1e1ac7b687de&recordId=" + rowData.inventory_goods_receipt_detail_id + "&tags="),
                            removeUrl: "remove",
                            autoUpload: true,
                        },
                        multiple: true,
                        files: (r.status.success ? r.data : []),
                        select: function onSelect(e) {
                            if (e.files.length > 1) {
                                alert("Please select max 1 files.");
                                e.preventDefault();
                            }
                        },
                        complete: function (e) {
                            console.log(e);
                        },
                        success: function (e) {
                            console.log('success');
                            console.log(e);
                            if (e.response.status.success) {
                                detailRow.find(".link_" + e.files[0].uid).attr('href', 'api/core/FileUpload?id=' + e.response.data.id);
                                var urlDownload = "";
                                urlDownload = $.helper.resolveApi("~/core/FileUpload?id=" + e.response.data.id + "&x-token=" + $.helper.getTokenData().Token);
                                detailRow.find(".link_" + e.files[0].uid + ":first").attr('href', urlDownload);

                            }
                        },
                        upload: function (e) {
                            var xhr = e.XMLHttpRequest;
                            //xhr.withCredentials = true;
                            if (xhr) {
                                xhr.addEventListener("readystatechange", function onReady(e) {
                                    console.log('readystatechange');
                                    if (xhr.readyState == 1 /* OPENED */) {
                                        xhr.setRequestHeader("Authorization", "Bearer " + $.helper.getTokenData().Token);
                                        xhr.removeEventListener("readystatechange", onReady);
                                    }
                                });
                            }
                        }
                    });


                    kendo.ui.progress(detailRow.find(".k-upload-init-progress"), false);
                    detailRow.find(".k-upload-init-progress").fadeOut();
                    detailRow.find(".file-upload").fadeIn();

                    if (data.length > 0) {
                        detailRow.find('.link-download').each(function () {
                            var id = $(this).data('id');
                            var urlDownload = "";
                            urlDownload = $.helper.resolveApi("~/core/FileUpload?id=" + id + "&x-token=" + $.helper.getTokenData().Token);
                            $(this).attr('href', urlDownload);
                        })
                    }
                }
            });
        }
        
        var loadDetailTransaction = function () {
            var poID = location.search.match(/poID=([^&]*)/);
            var poIDs = (poID == null) ? '' : poID[1];

            if (poIDs != '') {
                $.helper.kendoUI.grid($("#gridDetail"), {
                    options: {
                        url: $.helper.resolveApi('~/finance/PurchaseOrderDetail/KendoGridFromGoodsReceipt?purchaseorderid=' + poIDs)
                    },
                    navigatable: true,
                    pageable: false,
                    detailTemplate: kendo.template($("#template").html()),
                    detailInit: detailInit,
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
                                    item_code: { editable: true, validation: { required: true } },
                                    type_name: { editable: false },
                                    item_name: { type: "string", editable: false, validation: { required: true } },
                                    delivery_time: { type: "date", editable: true },
                                    qty: { type: "number", editable: false, validation: { required: true } },
                                    received_qty: { type: "number", defaultValue: 0, editable: true },
                                    qty_remaining: { type: "number", defaultValue: 0, editable: false },
                                    qty_count_in: { type: "number", defaultValue: 0, editable: false },
                                    remark: { type: "string", defaultValue: 0, editable: true },
                                    inventory_goods_receipt_id: { type: "number", editable: true }
                                }
                            },
                            data: "data",
                            total: "recordsTotal"
                        }
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
                        enable: false
                    },
                    dataBound: function (e) {
                        this.expandRow(this.tbody.find("tr.k-master-row").first());

                        var data = e.sender.dataSource.view().toJSON();
                        var TotalAmount = 0;
                        $.each(data, function (i, v) {
                            TotalAmount += v.amount;
                        });
                        var footerAmount = $('span.amount');
                        $(footerAmount).html(TotalAmount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        $.helper.kendoUI.resizeGrid();
                    },
                    columns: [
                        {
                            command: [
                                {
                                    name: "delete", text: "<i class='fa fa-trash'></i>"
                                }
                            ], title: "&nbsp;", width: "100px", title: "Actions",
                            groupable: true,
                            sortable: false
                        },
                        {
                            template: "<b>#= data.type_name ? type_name : ''#</b>",
                            field: "type_name",
                            title: "Type",
                            width: "200px",
                            editor: itemTypeEditor
                        },
                        {
                            field: "item_code",
                            title: "Code",
                            width: "150px",
                            editable: itemCodeEditable
                        },
                        {
                            template: "<b>#= data.type_name ? type_name : ''#</b>",
                            field: "item_type_name",
                            title: "Type",
                            width: "200px",
                            editor: itemTypeEditor
                        },
                        {
                            field: "item_name",
                            title: "Item Name",
                            width: "250px"
                        },
                        {
                            field: "delivery_time",
                            title: "Delivery Time",
                            width: "150px",
                            format: "{0: yyyy/MM/dd}",
                            editor: deliveryTimeEditor
                        },
                        {
                            field: "qty",
                            title: "Qty PO",
                            width: "120px"
                        },
                        {
                            field: "received_qty",
                            title: "Qty Received",
                            width: "150px",
                            editor: itemNumericTextBoxEditor
                        },
                        {
                            template: "<span class='remainingSpan'>#= (total_received_qty < 1) ? qty-received_qty : qty-total_received_qty #</span>",
                            field: "qty_remaining",
                            title: "Qty Remaining",
                            width: "150px"
                        },
                        {
                            template: "<span class='totalSpan'>#= received_qty || 0 #</span>",
                            field: "qty_count_in",
                            title: "Qty Count In",
                            width: "150px"
                        },
                        //{
                        //    field: "attachment",
                        //    title: "Attachment",
                        //    width: "150px" 
                        //},
                        {
                            field: "remark",
                            title: "Remark",
                            width: "150px"
                        }
                    ],
                    editable: {
                        mode: "incell",
                        createAt: "bottom"
                    }
                });

            } else {
                $.helper.kendoUI.grid($("#gridDetail"), {
                    options: {
                        url: $.helper.resolveApi('~/finance/InventoryGoodsReceiptDetail/KendoGrid?InventoryGoodsReceiptId=' + _detailID.val())
                    },
                    navigatable: true,
                    pageable: false,
                    detailTemplate: kendo.template($("#template").html()),
                    detailInit: detailInit,
                    //height: 300,
                    resizable: true,
                    save: function () {
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
                                    qty_remaining: { type: "number", editable: false },
                                    qty_count_in: { type: "number", editable: false  },
                                    attachment: { type: "string", defaultValue: 0 },
                                    remark: { type: "string", defaultValue: 0 }
                                }
                            },
                            data: "data",
                            total: "recordsTotal"
                        }
                    },
                    schema: {
                        model: {
                            id: "id"
                        },
                        data: "data",
                        total: "recordsTotal"
                    },

                    marvelCheckboxSelectable: {
                        enable: false
                    },

                    dataBound: function (e) {
                        this.expandRow(this.tbody.find("tr.k-master-row").first());

                        var data = e.sender.dataSource.view().toJSON();
                        var TotalAmount = 0;
                        $.each(data, function (i, v) {
                            TotalAmount += v.amount;
                        });
                        var footerAmount = $('span.amount');
                        $(footerAmount).html(TotalAmount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        $.helper.kendoUI.resizeGrid();
                    },
                    //toolbar: ["create", "cancel"],
                    columns: [
                        {
                            field: "code",
                            title: "Code",
                            width: "150px"
                        },
                        {
                            template: "<b>#= data.item_type_name ? item_type_name : ''#</b>",
                            field: "item_type_name",
                            title: "Type",
                            width: "200px",
                            editor: itemTypeEditor
                        },
                        {
                            field: "item_name",
                            title: "Item Name",
                            width: "250px"
                        },
                        {
                            field: "delivery_time",
                            title: "Delivery Time",
                            width: "150px",
                            format: "{0: yyyy/MM/dd}",
                            editor: deliveryTimeEditor
                        },
                        {
                            field: "purchased_qty",
                            title: "Qty PO",
                            width: "120px"
                        },
                        {
                            field: "received_qty",
                            title: "Qty Received",
                            width: "150px"
                        },
                        {
                            template: "<span class='remainingSpan'>#= purchased_qty-received_qty #</span>",
                            field: "qty_remaining",
                            title: "Qty Remaining",
                            width: "150px",
                            editor: itemEditorDisable
                        },
                        {
                            template: "<span class='totalSpan'>#= received_qty #</span>",
                            title: "Qty Count In",
                            width: "150px",
                            editor: itemEditorDisable
                        },
                        {
                            field: "attachment",
                            title: "Attachment",
                            width: "150px"
                        },
                        {
                            field: "remark",
                            title: "Remark",
                            width: "150px"
                        }
                    ],
                    editable: false
                });
            }
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

        function itemEditorDisable(container, options) {
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

            var inputName = (options.field == 'type_name') ? 'item_type_name' : options.field;

            $('<input required name="' + inputName + '" width="250px"/>')
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
            if (typeof (container[0].nextSibling.childNodes[0]) !== 'undefined') {
                $("[name='" + inputName + "']").data('kendoComboBox').enable(false);
            }
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
        }

        function itemNumericTextBoxEditor(container, options) {
            var remaining_qty = Number(options.model.qty) - Number(options.model.total_received_qty);
            
            $('<input required name="' + options.field + '" width="250px"/>')
                .appendTo(container)
                .kendoNumericTextBox({
                    max: Number(remaining_qty)
                });

            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
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
            
            var poID = location.search.match(/poID=([^&]*)/);
            var poIDs = (poID == null) ? '' : poID[1];
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
                        url: $.helper.resolveApi("~/Finance/InventoryGoodsReceipt/" + _detailID.val() + "/details"),
                    success: function (r) {
                        
                            if (r.status.success && r.data) {
                                var record = r.data;

                                $('#gr_number').val(record.goods_receipt_number);
                                $('#gr_number').prop('disabled', true);
                                $('#delivery_order_number').val(record.delivery_order_number);
                                $('#delivery_place').val(record.delivery_place);
                                $('#delivery_term').val(record.delivery_term);

                                $purchaseOrderStatusLookup.data('kendoComboBox').value(record.purchase_order_status_id);
                                $purchaseOrderStatusLookup.data('kendoComboBox').trigger("change");
                                $purchaseOrderStatusLookup.data('kendoComboBox').enable(false);
                                
                                $.helper.kendoUI.combobox.setValue($po_number, { id: record.id, purchase_order_number: record.purchase_order_number });
                                $po_number.data('kendoComboBox').enable(false);

                                //$.helper.kendoUI.combobox.setValue($companyLookup, { status_name: record.vendor_name });                                

                                $companyLookup.data('kendoComboBox').text(record.vendor_name);
                                $companyLookup.data('kendoComboBox').trigger("change");
                                $companyLookup.data('kendoComboBox').enable(false);

                                $datePicker.data("kendoDatePicker").value(record.create_on);                                

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

            if (poIDs != '') {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/Finance/PurchaseOrder/" + poIDs + "/details"),
                    success: function (r) {
                        if (r.status.success && r.data) {
                            
                            var record = r.data.purchase_order;
                            if (record.invoicing_type_id == null) {
                                swal(
                                    'Information',
                                    'Invoicing type cannot empty, please complete your Purchase Order transaction.',
                                    'info'
                                ).then(() => {
                                    $('#btn-save').attr('disabled', true);
                                    $('#btn-submit').attr('disabled', true);
                                });
                                return;
                            }

                            $.helper.kendoUI.combobox.setValue($po_number, { id: record.id, purchase_order_number: record.purchase_order_number });
                            $po_number.data('kendoComboBox').enable(false);
                            $.helper.kendoUI.combobox.setValue($companyLookup, { id: record.vendor_id, company_name: record.vendor_name });
                            $companyLookup.data('kendoComboBox').enable(false);
                            $.helper.kendoUI.combobox.setValue($type, { id: record.invoicing_type_id, type_name: record.invoicing_type_name });
                            $type.data('kendoComboBox').enable(false);
                            
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

                var poID = location.search.match(/poID=([^&]*)/);
                var poIDs = (poID == null) ? '' : poID[1];

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                var header = _formGoodReceipt.serializeToJSON();
                var transactionDetailKendo = $("#gridDetail").data("kendoGrid");

                header.vendor_name = $companyLookup.data('kendoComboBox').text();
                header.purchase_order_id = poIDs;     

                var invoicing_type = $type.data('kendoComboBox').text();
                var detail = transactionDetailKendo.dataSource.data().toJSON();
                var msg = '';
                for (var i = 0; i < detail.length; i++) {

                    if ((detail[i].item_code == null && detail[i].received_qty != null) ||
                        (detail[i].item_code == '' && detail[i].received_qty != null)) {
                        msg = 'Please insert <b>item code</b>.';
                    }

                    if (msg != '') {
                        toastr.warning(msg, "Warning");
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                        return;
                    }
                    
                    var type_name = detail[i].type_name;
                    detail[i].purchased_qty = (poIDs == '') ? detail[i].purchased_qty : detail[i].qty;
                    detail[i].total_received_qty = (poIDs == '') ? detail[i].purchased_qty : detail[i].received_qty;
                    delete detail[i].qty;
                    if (poIDs == '') {
                        delete detail[i].type_name;
                        detail[i].item_type_name = type_name;
                    } else {
                        var po_detail_id = detail[i].id;
                        var goods_receipt_detail_id = detail[i].inventory_goods_receipt_detail_id;
                        detail[i].purchase_order_detail_id = po_detail_id;
                        detail[i].id = goods_receipt_detail_id;
                        detail[i].item_type_id = detail[i].item_type;
                        detail[i].item_type_name = detail[i].type_name; 
                    }

                    if (invoicing_type == 'SINGLE' && detail[i].purchased_qty != detail[i].total_received_qty) {
                        swal(
                            'Information',
                            'Purchase order with invoicing type "single" can only be sent once.',
                            'info'
                        );
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                        return;
                    }
                }

                var data = {
                    good_receipt: header,
                    good_receipt_details: detail
                };
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/InventoryGoodsReceipt/saveasdraft"),
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
                        url: $.helper.resolveApi("~/Finance/InventoryGoodsReceiptDetail/delete"),
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
