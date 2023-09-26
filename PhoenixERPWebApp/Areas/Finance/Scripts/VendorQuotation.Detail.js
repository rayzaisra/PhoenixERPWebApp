function calculateTotalAmount() {
    var amount = Number($('input[name=amount]').val() || 0);
    var discountAmount = Number($('input[name=discount]').val() || 0);
    t = amount - discountAmount,
        $('#total_final_amount').text(t.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0);
}

//function setAmount(dataItem) {
//    dataItem.amount =  dataItem.qty * (dataItem.unit_price - dataItem.discount_amount);
//}


function getAmount(dataItem) {
    return dataItem.qty * (dataItem.unit_price - dataItem.discount_amount);
}


function calculateVAT(dataItem) {
    dataItem.tax_base = 0.1;
    var amount = getAmount(dataItem);
    var VAT = amount * dataItem.tax_base;
    dataItem.tax_vat = VAT || 0;
    return dataItem.tax_vat.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0;
}

function calculateAmount(dataItem) {
    dataItem.amount = getAmount(dataItem);
    return (dataItem.amount || 0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0;
}


function itemCodeEditable(dataItem) {
    return dataItem.type_name === "Inventory" ? true : false;
}
(function ($) {
    'use strict';


    var _detailID = $('#detail-id');
    var gridUI = $('#grid');

    var pageFunction = function () {
        var _formRequestForQuotation = $('#form-RequestForQuotation');
        var _formRequestForQuotationVendor = $('#form-RequestForQuotationVendor');
        var $companyLookup = $("#company_id");
        var $currencyLookup = $("#currency_id");
        var $rfqLookup = $("#rfq_id");


        //BEGIN::Form Validation
        _formRequestForQuotation.validate({
            rules: {
                purchase_order_date: {
                    required: true
                },
                delivery_date: {
                    required: true
                },
                company_id: {
                    required: true
                },
                business_unit_id: {
                    required: true
                },
                legal_entity_id: {
                    required: true
                },
                affiliation_id: {
                    required: true
                }//,
                //exchange_rate: {
                //    required: true,
                //    min: 1
                //},
                //discount: {
                //    required: true,
                //    min: 0
                //},
                //amount: {
                //    required: true,
                //    min: 0
                //}
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
                mUtil.scrollTo(_formRequestForQuotation, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation


        //BEGIN::Lookup

        $currencyLookup.kendoDropDownList({
            dataTextField: "currency_code",
            dataValueField: "id",
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/Core/Currency/lookup"),
                        contentType: 'application/json',
                        dataType: 'json',
                        type: 'GET',
                        cache: true
                    }
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "items",
                    total: "totalItems"
                },
            },
            change: function (e) {
                var equipmentModel = this.dataItem(e.item);
                try {
                    options.model.currency_id = equipmentModel.id;
                    options.model.set("currency_code", equipmentModel.currency_code);
                } catch (e) {
                    swal(
                        'Information',
                        e.message,
                        'info'
                    );
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
                        url: $.helper.resolveApi("~/Finance/CompanyProfile/KendoLookup"),
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

        $rfqLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "request_for_quotation_number",
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
                        url: $.helper.resolveApi("~/Finance/RequestForQuotation/KendoLookup"),
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
        //END: Lookup

        //BEGIN::Load Datatable
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
                        url: $.helper.resolveApi("~/finance/VendorQuotation/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;

                                $.helper.kendoUI.combobox.setValue($rfqLookup, { id: record.rfq_id, request_for_quotation_number: record.request_for_quotation_number });
                                $.helper.kendoUI.combobox.setValue($companyLookup, { id: record.vendor_id, company_name: record.vendor_name });
                                $currencyLookup.data("kendoDropDownList").value(record.currency_id);

                                if (record.quotation_date !== null) $('#quotation_date').datepicker("setDate", moment(record.quotation_date).format("MM/DD/YYYY"));
                                if (record.expiry_date !== null) $('#expiry_date').datepicker("setDate", moment(record.expiry_date).format("MM/DD/YYYY"));
                                if (record.delivery_date !== null) $('#delivery_date').datepicker("setDate", moment(record.delivery_date).format("MM/DD/YYYY"));


                                $('input[name="delivery_address"]').val(record.delivery_address);
                                $('input[name="term_of_payment"]').val(record.term_of_payment);
                                $('input[name="exchange_rate"]').val(record.exchange_rate);
                                $('input[name="vendor_quotation_number"]').val(record.quotation_number);

                                if (record.job_id !== null || record.job_id !== "") {
                                    $('input[name="rfq_type"]').val("Internal");
                                    $('input[name="reference_number"]').val(record.purchase_request_id);
                                    $('input[name="job_name"]').val("Internal");
                                } else {
                                    $('input[name="rfq_type"]').val("Project");
                                    $('input[name="reference_number"]').val(record.job_number);
                                    $('input[name="job_name"]').val(record.job_name);
                                }
                                $('input[name="vendor_quotation_number"]').val(record.quotation_number);
                                $('input[name="vendor_quotation_number"]').val(record.quotation_number);

                                //$('input[name="amount"]').val(record.amount);
                                $('input[name="discount"]').val(record.discount);
                                debugger;
                                var numb = $('#end_gradtotal').html().match(/\d/g);
                                numb = numb.join("");
                                record.amount_final = (record.amount_final === null) ? numb.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") : 0;

                                $('#total_final_amount').text(record.amount_final);


                                //let urlApi = $.helper.resolveApi("~/finance/VendorQuotation/getlistdetails/?rfq_id=" + record.rfq_id);

                                //$.ajax({
                                //    type: 'GET',
                                //    url: urlApi,
                                //    dataType: 'json',
                                //    contentType: "application/json; charset=utf-8",
                                //    success: function (result) {
                                //        mApp.unblock("#m_blockui_list");
                                //        //window.location.href = url;
                                //    },
                                //    error: function (e, t, s) {
                                //        console.log(e)
                                //        mApp.unblock("#m_blockui_list");
                                //    }
                                //});


                                formControll();
                                if (record.is_locked) {
                                    formControllSubmited(true);
                                    setStatus('submited');
                                } else {
                                    setStatus('draft');
                                }

                            } else {
                                swal(
                                    'Information',
                                    r.status.message,
                                    'info'
                                );
                            }
                        },
                        error: function (e, t, s) {
                            //swal(
                            //    'Information',
                            //    'Ooops, something went wrong !',
                            //    'info'
                            //);
                            console.log(e);
                        }
                    }).then(setTimeout(function () {
                        mApp.unblockPage();
                    }, 2e3));
            } else {
                formControll();
                setStatus('new');
            }

        };
        //END::Load Datatable

        //BEGIN::DataSource
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
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoMultiColumnComboBox({
                    autoBind: false,
                    dataTextField: "item_code",
                    dataValueField: "id",
                    value: options.model.item_id,
                    text: options.model.item_code,
                    height: 550,
                    columns: [
                        { field: "item_code", title: "Item Code", width: 200 },
                        { field: "item_name", title: "Item Name", width: 200 }
                    ],
                    dataSource: itemDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        if (equipmentModel) {
                            options.model.item_id = equipmentModel.id;
                            options.model.uom_id = equipmentModel.uom_id;
                            options.model.uom_name = equipmentModel.uom_name;
                            options.model.set("item_code", equipmentModel.item_code);
                            options.model.set("item_name", equipmentModel.item_name);
                        }
                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
            tooltipElement.appendTo(container);
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
                    text: options.model.item_type_name,
                    dataSource: itemTypeDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        console.log(equipmentModel);
                        console.log(options.model);
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
                            swal(
                                'Information',
                                'Internal Error',
                                'info'
                            );
                        }

                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
        }

        //END::Editor

        //BEGIN::Request For Quotation Detail
        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/VendorQuotationDetail/KendoGrid?vendorquotationid=' + _detailID.val())
            },
            navigatable: true,
            pageable: false,
            //detailTemplate: kendo.template($("#template").html()),
            //detailInit: detailInit,
            //height: 300,
            resizable: true,
            save: function () {
                var grid = this;
                setTimeout(function () {
                    grid.refresh();
                });
            },
            dataSource: {
                group: { field: "type_name", aggregates: [{ field: "type_name", aggregate: "count" }] },
                pageSize: 1000,
                serverPaging: true,
                serverSorting: false,
                batch: true,
                serverOperation: false,
                aggregate: [
                    { field: "amount", aggregate: "sum" }
                ]
            },
            schema: {

                model: {
                    id: "ID",
                    fields: {
                        id: { editable: true, nullable: true },
                        discount_amount: { type: "number", validation: { required: true, min: 0 }, defaultValue: 0 },
                        tax_base: { type: "number", validation: { required: true, min: 0 } },
                        tax_vat: { type: "number", validation: { required: true, min: 0 } }
                    }
                },
                data: "data",
                total: "recordsTotal"

            },

            aggregate: [
                { field: "amount", aggregate: "sum" },
            ],

            marvelCheckboxSelectable: {
                enable: false
            },

            dataBound: function (e) {
                this.expandRow(this.tbody.find("tr.k-master-row").first());

                var data = e.sender.dataSource.data().toJSON();


                var TotalAmount = 0;
                $.each(data, function (i, v) {
                    TotalAmount += v.amount;
                });
                var ASF = 0.1 * TotalAmount;
                var VAT = 0.1 * (TotalAmount + ASF);

                $('#end_subtotal').html(TotalAmount);
                $('#end_asf').html(ASF);
                $('#end_vat').html(VAT);
                var total = (TotalAmount + ASF + VAT).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

                $('#total_final_amount').appendTo( total );
                $('#end_gradtotal').html("<b>" + total + "</b>");
                var valueTOtalnya = parseInt(TotalAmount + ASF + VAT);
                var discountAmount = parseInt($('#discountr').val());
                $('#amountr').val(valueTOtalnya);
                var t1 = valueTOtalnya - discountAmount;

                $('#total_final_amount').text(t1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0);
                $('#total_final_amount').val(t1);
                $('#amount_final').val(t1);
                //calculateTotalAmount();

                var footerAmount = $('span.amount');
                //$(footerAmount).html(TotalAmount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                $.helper.kendoUI.resizeGrid();
            },
            columns: [
                {
                    command: [
                        {
                            name: "rowDelete", text: "<i class='fa 	fa-trash'></i>", click: function (e) {
                                e.preventDefault();
                                var row = e.target.closest('tr');
                                var uid = $(row).data(uid);
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);
                                if (item.id) DeleteData([item.id]);
                                else {
                                    dataSource.cancelChanges(item);
                                }
                            }
                        }
                    ], title: "&nbsp;", width: "200px",
                    //locked: true,
                    //lockable: false,
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
                    title: "Inventory Code",
                    width: "150px",
                    editable: itemCodeEditable,
                    editor: itemEditor
                },
                {
                    field: "item_name",
                    title: "Inventory Name",
                    width: "150px"
                },
                {
                    field: "qty",
                    title: "Qty",
                    type: "Number",
                    attributes: {
                        style: "text-align:center;"
                    },
                    width: "75px"
                },
                {
                    field: "estimated_unit_price",
                    title: "Estimated Price Per Quantity",
                    type: "Number",
                    editable: function () { return false },
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: "Rp. #= kendo.toString(data.estimated_unit_price, 'n') #",
                    width: "150px"
                },
                {
                    field: "unit_price",
                    title: "Final Per Price",
                    type: "Number",
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: function (dataItem) {
                        return "<b>Rp." + (dataItem.unit_price || 0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,') || 0 + "</b>";
                    },
                    width: "150px"
                },
                {
                    field: "discount_amount",
                    title: "Discount Amount",
                    type: "Number",
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: function (dataItem) {
                        return "<b>Rp." + (dataItem.discount_amount || 0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,') || 0 + "</b>";
                    },
                    width: "150px"
                },
                {
                    field: "tax_base",
                    title: "Tax Rate",
                    type: "Number",
                    template: "#= (data.tax_base || 0.1 ) * 100  #<b> %</b>",
                    attributes: {
                        style: "text-align:right;"
                    },
                    width: "75px"
                },
                {
                    field: "tax_vat",
                    title: "VAT",
                    type: "Number",
                    editable: function () { return false },
                    template: "<b>Rp. #= calculateVAT(data) #</b>",
                    attributes: {
                        style: "text-align:right;"
                    },
                    width: "150px"
                },
                {
                    field: "tax_wdh",
                    title: "WDH Tax",
                    type: "Number",
                    editable: function () { return false },
                    attributes: {
                        style: "text-align:right;"
                    },
                    width: "150px"
                },
                {
                    field: "amount",
                    title: "Total Cost",
                    type: "Number",
                    editable: function () { return false },
                    template: "<b>Rp. #= calculateAmount(data) #</b>",
                    footerTemplate: "<span class='amount'></span>",
                    attributes: {
                        style: "text-align:right;"
                    },
                    footerAttributes: {
                        style: "text-align:right;"
                    },
                    width: "150px"
                },
                {
                    field: "task_detail",
                    title: "Notes",
                    attributes: {
                        style: "text-align:left;"
                    },
                    width: "250px"
                },
                {
                    field: "timeline_production",
                    title: "Timeline Production",
                    attributes: {
                        style: "text-align:left;"
                    },
                    width: "250px"
                }
            ],
            editable: {
                mode: "incell",
                createAt: "bottom"
            }

        });

        function detailInit(e) {
            var rowData = e.data;
            var detailRow = e.detailRow;
            kendo.ui.progress(detailRow.find(".k-upload-init-progress"), true);

            $.ajax({
                type: "GET",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/core/FileUpload/entity/record?entityId=51a46d55-6d81-2077-681d-1e1ac7b687de&recordId=" + rowData.rfq_detail_id),
                success: function (r) {
                    var data = (r.status.success ? r.data : []);
                    var template = kendo.template($('#attachment').html());
                    var result = template({
                        items: data
                    });
                    detailRow.find(".file-upload").html(result);
                    kendo.ui.progress(detailRow.find(".k-upload-init-progress"), false);
                    detailRow.find(".k-upload-init-progress").fadeOut();
                    detailRow.find(".file-upload").fadeIn();
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


        //END::Load Request For Quotation Detail

        var formControll = function () {
            $('#btn-save').removeAttr('disabled').show();

            if (_detailID.val() !== '') {
                $('#btn-approval').removeAttr('disabled', '').show();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();

                if ($('input[name="approval_id"]').val() !== '') {
                    $('#btn-approval').attr('disabled', 'disabled').hide();
                    $('#btn-remove_approval').removeAttr('disabled', '').show();

                    $('#btn-submit').removeAttr('disabled', '').show();
                }

            } else {
                $('#btn-approval').attr('disabled', 'disabled').hide();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();
            }
            $('#tb').show();
        };

        var formControllSubmited = function (submited) {
            if (!submited) {
                $('#tb').show();
            }
            else {
                $('#tb').hide();
                $('#btn-save').attr('disabled', 'disabled').hide();
                $('#btn-approval').attr('disabled', 'disabled').hide();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();
            }
        };

        var setStatus = function (state) {
            var output = '';
            switch (state) {
                case 'new':
                    output = `<span class="m-badge m-badge--info m-badge--wide">New</span>`;
                    break;
                case 'draft':
                    output = `<span class="m-badge m-badge--warning m-badge--wide">Draft</span>`;
                    break;
                case 'submited':
                    output = `<span class="m-badge m-badge--danger m-badge--wide">Submited</span>`;
                    break;
                case 'approved':
                    output = `<span class="m-badge m-badge--success m-badge--wide">Approved</span>`;
                    break;
            }
            $('#status').html(output);
        };

        $('#btn-save').click(function () {
            var btn = $(this);
            if (_formRequestForQuotation.valid()) {
                var vendorquotationvendor = [_formRequestForQuotationVendor.serializeToJSON()];
                var requestForQuotation = _formRequestForQuotation.serializeToJSON();
                var numb = $('#end_gradtotal').html().match(/\d/g);
                numb = numb.join("");
                requestForQuotation.amount_final = numb;
                var detailTransactions = $("#grid").data("kendoGrid");
                var approverId = $('#approverId').val();
                var data =
                {
                    VendorQuotation: requestForQuotation,
                    //VendorQuotationVendor: vendorquotationvendor,
                    Details: detailTransactions.dataSource.data().toJSON()
                };
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/VendorQuotation/saveasdraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            console.log(r);
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            reloadGrid();
                            $('input[name="id"]').val(r.data.recordID);
                            toastr.success(r.status.message, "Information");
                            setStatus('draft');

                            window.location.href = $.helper.resolve("~/Finance/VendorQuotation");
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

        $('#btn-submit').click(function () {
            var btn = $(this);
            if (_formRequestForQuotation.valid()) {

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/InternalRfq/submited?RecordId=" + _detailID.val()),
                    //data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            //history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            //_detailID.val(r.data.recordID);
                            //$('input[name="id"]').val(r.data.recordID);
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                            formControllSubmited(true);
                            formControll();
                            reloadGrid();
                            setStatus('submited');
                            toastr.success(r.status.message, "Information");

                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                            btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
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
                });


            }

        });

        $('#btn-remove_approval').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            swal({
                title: "Are you sure reset approval template ?",
                text: "You won't be able to revert this!",
                type: "warning",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (e) {
                if (e.value) {
                    $.ajax({
                        type: "DELETE",
                        dataType: 'json',
                        url: $.helper.resolveApi("~/core/Approval/removeApprovalEntity/" + $('#entity_id').val() + "/?RecordID=" + _detailID.val() + "&ApprovalID=" + $('input[name="approval_id"]').val()),
                        success: function (r) {
                            if (r.status.success) {

                                toastr.success(r.status.message, "Information");
                                $('input[name="approval_id"]').val('');
                                formControll();

                            } else {
                                swal(
                                    'Information',
                                    r.status.message,
                                    'info'
                                );

                            }
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                        },
                        error: function (e, t, s) {
                            swal(
                                'Information',
                                'Ooops, something went wrong !',
                                'info'
                            );
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                        }
                    });
                }
            });

        });

        $('#btn-approval').click(function () {
            var url = $.helper.resolve("~/Core/ApprovalTemplate/SetupApproval/?EntityId=" + $('#entity_id').val() + "&RecordID=" + _detailID.val()
                + "&Title=Internal Request For Quotation&Ref=" + $('input[name="internal_rfq_number"]').val() + "&url=~/Finance/RequestForQuotation/Detail/?id=" + _detailID.val());
            window.location.href = url;
        });

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
                        url: $.helper.resolveApi("~/Finance/VendorQuotationDetail/delete"),
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                }).then(function () {
                                    ListChecked = [];
                                    reloadGrid();
                                });
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
                        mApp.unblock("#m_blockui_list");
                    }, 2e3));
                }


            });
        };

        return {
            init: function () {
                //initialize
                loadDetailData()
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));