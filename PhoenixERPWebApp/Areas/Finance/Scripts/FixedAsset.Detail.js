(function ($) {
    'use strict';

    var _detailID = $('#detail-id');

    var pageFunction = function () {
        var _formMasterFixedAsset = $('#form-MasterFixedAsset');

        var $businessUnitLookup = $('#division_id');
        var $currencylookup = $('#currency_id');
        var $categorylookup = $('#item_category_id');
        var $companyLookup = $("#company_id");
        var $purchaseOrderLookup = $("#purchase_order_id");
        var $datePick = $("#registration_date");
        var $item = $("#item_id"); 
        var $invoice = $("#invoice_id");
        var $commercial = $("#commercial_depreciation_id"); 
        
        $item.kendoMultiColumnComboBox({
            autoBind: false,
            dataTextField: "item_code",
            dataValueField: "id",
            height: 550,
            columns: [
                { field: "item_code", title: "Item Code", width: 200 },
                { field: "item_name", title: "Item Name", width: 200 }
            ],
            dataSource: {
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
            },
        });

        $datePick.kendoDatePicker({
            format: 'yyyy/MM/dd'
        });

        //Dropdown -- division_id (business Unit)
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
            //width: "100%"
        });

        //Dropdown Invoice
        $commercial.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "commercial_depreciation_code",
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
                        url: $.helper.resolveApi("~/Finance/CommercialDepreciation/KendoLookup"),
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

        //Dropdown Invoice
        $invoice.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "invoice_number",
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
                        url: $.helper.resolveApi("~/Finance/InvoicePayable/KendoLookup"),
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

        //Dropdown Currency
        $currencylookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "currency_code",
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
                        url: $.helper.resolveApi("~/core/Currency/kendolookup"),
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

        //Dropdown category
        $categorylookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "category_name",
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
                        url: $.helper.resolveApi("~/core/ItemCategory/KendoLookup"),
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

        //Dropdown Vendor / Company
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

        //Dropdown PurchaseOrder
        $purchaseOrderLookup.kendoComboBox({
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

        //BEGIN::Form Validation
        _formMasterFixedAsset.validate({
            rules: {
                purchase_order_date: {
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
                mUtil.scrollTo(_formMasterFixedAsset, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation
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
                        url: $.helper.resolveApi("~/finance/FixedAsset/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                console.log(record); 
                                $.helper.kendoUI.dropDownTree.setValue($businessUnitLookup, { id: record.division_id, text: record.unit_name });
                                $.helper.kendoUI.combobox.setValue($currencylookup, { id: record.currency_id, text: record.currency_code });
                                $.helper.kendoUI.combobox.setValue($categorylookup, { id: record.category, text: record.category_name });
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
            if (_formMasterFixedAsset.valid()) {

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                var form = _formMasterFixedAsset.serializeToJSON();
                
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/FixedAsset/save"),
                    data: JSON.stringify(form),
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
        
        return {
            init: function () {
                //initialize
                //loadDetailData();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));