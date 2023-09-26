function calculateAmount(dataItem) {
    dataItem.amount = dataItem.qty * dataItem.unit_price;
    return dataItem.amount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0;
}

function itemCodeEditable(dataItem) {
    return dataItem.type_name == "Inventory" ? true : false;
}

function itemUomEditable(dataItem) {
    return dataItem.type_name == "Inventory" ? false : true;
}
function saved(){
    alert("CA Form Successfully Saved !");
}


function ask(){
    if (confirm('Submit Now ?')) {
        alert("CA Form Successfully Submited !");
    }
}
$('#jobname').on('change', function() {
    $("#jobnumber").val(this.value).change();
});

(function ($) {
    'use strict';

    var _detailID = $('#detail-id');
    var gridUI = $('#grid');
    var pageFunction = function () {
        var _formPurchaseOrder = $('#form-PurchaseOrder');
        var $legalEntityLookup = $("#legal_entity_id");
        var $affiliationLookup = $("#affiliation_id");
        var $companyLookup = $("#company_id");
        var $currencyDropdown = $("#currency_id");
        var $approvalLookup = $("#approverId");
        var $paymentTypeLookup = $("#payment_method_id");
        var $invoicingTypeLookup = $("#invoicing_type_id");
        var $purchaseOrderStatusLookup = $("#purchase_order_status_id");
        var $businessUnitLookup = $("#business_unit_id");
        $(".numeric").kendoNumericTextBox();
        $(".numeric").keyup(function() {
            var qty1=$("#qty1").autoNumeric('get');
            var amount1=$("#amount1").autoNumeric('get');
            var result1=qty1*amount1;
            $("#result1").autoNumeric('set',result1);
    
            var qty2=$("#qty2").autoNumeric('get');
            var amount2=$("#amount2").autoNumeric('get');
            var result2=qty2*amount2;
            $("#result2").autoNumeric('set',result2);
        });

        

        $(".percent_width").kendoNumericTextBox({
            format: "p0",
            min: 0,
            step: 0.01,
            factor: 100
        });
        //BEGIN::Form Validation
        _formBudgetManagement.validate({
            rules: {
               
            },
            invalidHandler: function (e, r) {
                mUtil.scrollTo(_formPurchaseOrder, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::Dropdown
        var dgOptions = function () {
            var currencyDropdown = undefined;
            $.ajax({
                url: $.helper.resolveApi("~/core/Currency/lookup?length=300"),
                type: "GET",
                success: function (response) {
                    currencyDropdown = response.items;
                    $.each(currencyDropdown, function (key, data) {
                        $currencyDropdown.append($('<option></option>').attr('value', data.id).text(data.currency_code));
                    });
                },
                async: false
            });

            return currencyDropdown;
        };

        //END:: Dropdown

        //BEGIN::Lookup
        //$companyLookup.cmSelect2({
        //    url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
        //    filters: ' r.company_name LIKE @0 and r.is_vendor = 1',
        //    //paramaters: [],
        //    result: {
        //        id: 'id',
        //        text: 'company_name',
        //        tax_registration_number: 'tax_registration_number'
        //    }
        //}).on('select2:select', function (e) {
        //    var data = e.params.data;
        //    $('input[name="company_name"]').val(data.text);
        //    $('input[name="tax_registration_number"]').val(data.tax_registration_number);
        //    });

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
                filter: { field: "is_vendor", operator: "eq", value: true },
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
                        console.log(mapRequest);
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

        $purchaseOrderStatusLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "status_name",
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
                        url: $.helper.resolveApi("~/Finance/PurchaseOrderStatus/KendoLookup"),
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

        $businessUnitLookup.kendoDropDownTree({
            autoBind: true,
            placeholder: "Choose ..",
            dataTextField: "text",
            dataValueField: "id",
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/core/BusinessUnit/getBusinessUnitTree"),
                        contentType: 'application/json',
                        dataType: 'json',
                        type: 'GET',
                        cache: true
                    },
                    parameterMap: function (data) {
                        return data;
                    }
                },
                schema: {
                    model: {
                        id: "id",
                        children: "items"
                    },
                    data: "data"
                },
            },
            //height: "auto",
            width: "300px"
        });

        $paymentTypeLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "method_name",
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
                        url: $.helper.resolveApi("~/Finance/PaymentMethod/KendoLookup"),
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

        $invoicingTypeLookup.kendoComboBox({
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
                        url: $.helper.resolveApi("~/Finance/InvoicingType/KendoLookup"),
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

        $approvalLookup.select2({
            enable: false,
            width: "auto",
            theme: "bootstrap",
            placeholder: "Assign To",
            ajax: {
                url: $.helper.resolveApi("~/core/User/lookup"),
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var filters = " r.app_fullname LIKE @0 ";
                    var parameters = [];
                    if (params.term === undefined) {
                        parameters.push('%%');
                    }
                    else {
                        parameters.push('%' + params.term + '%');
                    }
                    return {
                        length: 100,
                        filters: "( " + filters + " )",
                        parameters: parameters.toString()  // search term

                    };
                },
                processResults: function (data, page) {
                    var result = { results: [], more: false };
                    if (data && data.items) {
                        result.more = data.totalPages > data.currentPage;
                        $.each(data.items, function () {
                            result.results.push({
                                id: this.id,
                                text: this.app_fullname,
                                username: this.app_username
                            });
                        });
                    };
                    return result;
                },
                cache: true
            },
            escapeMarkup: function (markup) {
                return markup;
            }, // let our custom formatter work
            minimumInputLength: 0,
            templateResult: formatApprovalLookup
            //templateSelection: formatTeamLeaderLookup
        });

        var setLookup_Approver = function (Id) {
            if (Id == '' || Id==null ) return;
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: $.helper.resolveApi("~/core/User/" + Id + "/details"),
                success: function (r) {
                    var record = r.data;
                    console.log(record);

                    $approvalLookup.append($("<option/>", {
                        value: Id,
                        text: record.app_fullname,
                        selected: true
                    }));
                    $approvalLookup.val(Id);
                },
                error: function (e, t, s) {
                    swal(
                        'Information',
                        'Ooops, something went wrong !',
                        'info'
                    );
                }
            });
        };

        function formatApprovalLookup(opt) {
            if (!opt.id) {
                return opt.text;
            }

            var template;
            var stateNo = mUtil.getRandomInt(0, 7);
            var states = [
                'success',
                'brand',
                'danger',
                'accent',
                'warning',
                'metal',
                'primary',
                'info'];
            var state = states[stateNo];

            template = `
                <div class="m-card-user m-card-user--sm">
                    <div class="m-card-user__pic">
                        <div class="m-card-user__no-photo m--bg-fill-` + state + `"><span>` + opt.text.substring(0, 1) + `</span></div>
                    </div>
                    <div class="m-card-user__details">
                        <span class="m-card-user__name">` + opt.text + `</span>
                        <a href="javascript:return;" class="m-card-user__email m-link">` + opt.username + `</a>
                    </div>
                </div>`;
            return template;

        }

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

        $legalEntityLookup.kendoComboBox({
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
        //END: Lookup

        //BEGIN:: Datetimepicker
        //$("#purchase_order_date").datetimepicker({
        //    todayHighlight: !0,
        //    autoclose: !0,
        //    pickerPosition: "bottom-left",
        //    format: "yyyy/mm/dd hh:ii"
        //});
        //$("#delivery_date").datetimepicker({
        //    todayHighlight: !0,
        //    autoclose: !0,
        //    pickerPosition: "bottom-left",
        //    format: "yyyy/mm/dd hh:ii"
        //});
        //END:: Datetimepicker
        var editIndex = undefined;
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
                        url: $.helper.resolveApi("~/finance/PurchaseOrder/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data.purchase_order;
                                var approverId = r.data.approverId;

                                setLookup_Approver(approverId);
                                $('input[name="business_unit_id"]').val(record.business_unit_id);
                                $('input[name="request_id"]').val(record.purchase_request_number);
                                $('input[name="company_reference"]').val(record.company_reference);
                                $('input[name="currency_id"]').val(record.currency_code);
                                $('input[name="description"]').val(record.description);
                                $('input[name="exchange_rate"]').val(record.exchange_rate);
                                if (record.delivery_date != null) $('#delivery_date').datepicker("setDate", moment(record.delivery_date).format("MM/DD/YYYY"));
                                if (record.purchase_order_date != null) $('#purchase_order_date').datepicker("setDate", moment(record.purchase_order_date).format("MM/DD/YYYY"));
                                $('input[name="purchase_order_number"]').val(record.purchase_order_number);

                                $('input[id="discount_percent"]').val(record.discount_percent);
                                $('input[id="amount"]').val(record.amount);
                                $('input[id="pic_name"]').val(record.pic_name);
                                $('input[id="discount_amount"]').val(record.discount_amount);
                                $('input[id="percent_payment_term1"]').val(record.percent_payment_term1);
                                $('input[id="percent_payment_term2"]').val(record.percent_payment_term2);
                                $('input[id="percent_payment_term3"]').val(record.percent_payment_term3);
                                $('input[id="vat_percent"]').val(record.vat_percent);
                                $.helper.kendoUI.combobox.setValue($affiliationLookup, { id: record.affiliation_id, affiliation_name: record.affiliation_name });
                                $.helper.kendoUI.dropDownTree.setValue($businessUnitLookup, { id: record.business_unit_id, text: record.unit_name });
                                $.helper.kendoUI.combobox.setValue($legalEntityLookup, { id: record.legal_entity_id, legal_entity_name: record.legal_entity_name });
                                $.helper.kendoUI.combobox.setValue($paymentTypeLookup, { id: record.payment_method_id, method_name: record.payment_method_name });
                                $.helper.kendoUI.combobox.setValue($invoicingTypeLookup, { id: record.invoicing_type_id, type_name: record.invoicing_type_name });
                                $.helper.kendoUI.combobox.setValue($purchaseOrderStatusLookup, { id: record.purchase_order_status_id, status_name: record.purchase_order_status_name });
                                $.helper.kendoUI.combobox.setValue($companyLookup, { id: record.company_id, company_name: record.current_company_name });

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

        //BEGIN::DataSource
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

        uomDataSource.read();

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
        //END::Datasource

        //BEGIN::Editor
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
                        console.log(equipmentModel);
                        options.model.item_id = equipmentModel.id;
                        options.model.uom_id = equipmentModel.uom_id;
                        options.model.set("item_code", equipmentModel.item_code);
                        options.model.set("item_name", equipmentModel.item_name);
                        options.model.set("uom_name", equipmentModel.uom_name);
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
                    value: options.model.type_id,
                    text: options.model.type_name,
                    dataSource: itemTypeDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        options.model.item_type = equipmentModel.id;
                        options.model.set("type_name", equipmentModel.type_name);
                        if (equipmentModel.type_name != "Inventory") {
                            options.model.item_code = null;
                            options.model.item_name = null;
                            options.model.uom_name = null;
                        }
                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
        }

        function uomEditor(container, options) {
            $('<input required name="' + options.field + '"/>')
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
                        options.model.uom_id = equipmentModel.id;
                        options.model.set("uom_name", equipmentModel.uom_name);
                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '" ></span>');
            tooltipElement.appendTo(container);
        }
        //END::Editor

        //BEGIN::Purchase Order Detail
        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/PurchaseOrderDetail/KendoGrid?purchaseorderid=' + _detailID.val())
            },
            navigatable: true,
            pageable: false,
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
                aggregate: [
                    { field: "amount", aggregate: "sum" },
                ]
            },
            schema: {
                model: {
                    id: "ID",
                    fields: {
                        id: { editable: true, nullable: true },
                        uom_name: { editable: true },
                        item_code: { editable: true }
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
                var data = e.sender.dataSource.view().toJSON();
                var TotalAmount = 0;
                $.each(data, function (i, v) {
                    TotalAmount += v.amount;
                });

                var footerAmount = $('span.amount');

                $(footerAmount).html(TotalAmount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

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
                    ], title: "&nbsp;", width: "100px", title: "Actions",
                    locked: true,
                    lockable: false,
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
                    title: "Item Code",
                    width: "150px",
                    editable: itemCodeEditable,
                    editor: itemEditor
                },
                {
                    field: "item_name",
                    title: "Item Name",
                    width: "150px"
                },
                {
                    field: "item_description",
                    title: "Description",
                    attributes: {
                        style: "text-align:left;"
                    },
                    width: "250px"
                },
                {
                    field: "item_brand",
                    title: "Brand",
                    width: "150px"
                },
                {
                    field: "item_sub_brand",
                    title: "Sub Brand",
                    width: "150px"
                },
                {
                    field: "uom_name",
                    title: "Unit",
                    attributes: {
                        style: "text-align:center;"
                    },
                    width: "100px",
                    editor: uomEditor,
                    editable: itemUomEditable
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
                    field: "unit_price",
                    title: "Price Per Quantity",
                    type: "Number",
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: "#= kendo.toString(data.unit_price, 'n') #",
                    width: "150px"
                },
                {
                    field: "amount",
                    title: "Line Total",
                    type: "Number",
                    editable: function () { return false },
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: "<b>#= calculateAmount(data) #</b>",
                    width: "150px",
                    footerAttributes: {
                        style: "text-align:right;"
                    },
                    footerTemplate: "<span class='amount'></span>"
                },
                {
                    field: "discount_percent",
                    title: "Discount",
                    type: "Number",
                    attributes: {
                        style: "text-align:center;"
                    },
                    width: "75px"
                },
                {
                    field: "tax_percent",
                    title: "PPH",
                    type: "Number",
                    attributes: {
                        style: "text-align:center;"
                    },
                    width: "75px"
                },
                {
                    field: "amount_net",
                    title: "Net",
                    type: "Number",
                    attributes: {
                        style: "text-align:center;"
                    },
                    width: "75px"
                }

            ],
            editable: true
        });

        function reloadGrid() {
            gridUI.data("kendoGrid").dataSource.read();
            gridUI.data('kendoGrid').refresh();
        }

        gridUI.find(".k-grid-toolbar").on("click", ".k-grid-Reload", function (e) {
            e.preventDefault();
            reloadGrid();
        });


        //END::Load Purchase Order Detail

        var formControll = function (hasSubmited) {
            if (!hasSubmited) {
                $('#btn-submit').removeAttr('disabled');
                $('#btn-save').removeAttr('disabled');
                $('#btn-approval').attr('disabled', 'disabled');

            } else {
                $('#btn-submit').attr('disabled', 'disabled');
                $('#btn-save').attr('disabled', 'disabled');
                $('#btn-approval').removeAttr('disabled', '');
            }
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
            if (_formPurchaseOrder.valid()) {
                var purchaseOrder = _formPurchaseOrder.serializeToJSON();
                var detailTransactions = $("#grid").data("kendoGrid");
                var approverId = $('#approverId').val();
                var data = {
                    approverId: approverId,
                    purchase_order: purchaseOrder,
                    purchase_order_details: detailTransactions.dataSource.view().toJSON()
                };

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/PurchaseOrder/savetodraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="purchase_order_number"]').val(r.data.purchase_order_number);
                            toastr.success(r.status.message, "Information");
                            setStatus('draft');
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
            if (_formPurchaseOrder.valid()) {
                var purchaseOrder = _formPurchaseOrder.serializeToJSON();
                var detailTransactions = dgdetail.datagrid('getData');
                var approverId = $('#approverId').val();
                var data = {
                    approverId: approverId,
                    purchase_order: purchaseOrder,
                    purchase_order_details: detailTransactions.rows
                };
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/PurchaseOrder/submited?RecordId=" + _detailID.val()),
                    //data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            //history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            //_detailID.val(r.data.recordID);
                            //$('input[name="id"]').val(r.data.recordID);
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                            formControllSubmited(true);
                            formControll(true);
                            //purchaseOrderDetail.reload();
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
                        url: $.helper.resolveApi("~/Finance/PurchaseOrderDetail/delete"),
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
                loadDetailData(),
                    dgOptions()
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));