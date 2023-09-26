(function ($) {
    'use strict';
    var asideLeftToggle = new mToggle('m_aside_left_minimize_toggle', {
        target: 'body',
        targetState: 'm-brand--minimize m-aside-left--minimize',
        togglerState: 'm-brand__toggler--active'
    });
    asideLeftToggle.toggleOn();


    var gridUI = $('#grid');
    var gridUIDialog = $('#dialog_grid');
    var ListChecked = [];
    var pageFunction = function () {
        var $legal_entityLookup = $("#legal_entity_id");
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
                serverFiltering: false,
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

            },
            change: function (e) {
                var dataItem = e.sender.dataItem();
                loadCoaBalance(dataItem.id);
            }
        });

        var loadCoaBalance = function (legalEntityId = null) {
            if (legalEntityId == null) return;
            $("#grid").html('');
            
            $.helper.kendoUI.grid($("#grid"), {
                options: {
                    url: $.helper.resolveApi('~/finance/Account/' + legalEntityId + '/coaBalanceKendoGrid'),
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options) {
                            console.log(options);
                            return JSON.stringify(options);
                        }
                    }
                },
                //save: function (e) {
                //    var grid = this;
                //    setTimeout(function () {
                //        grid.refresh();
                //    })
                //},
                navigatable: true,
                //height: 550,
                filterable: {
                    mode: "row",
                    extra: false,
                    operators: {
                        string: {
                            eq: "Is equal to",
                            neq: "Is not equal to",
                            contains:"Contains"
                        }
                    }
                },
                pageable: {
                    refresh: true,
                    pageSizes: [5, 10, 20, 100]
                },
                resizable: true,
                editable: "inline",
                dataSource: {
                    transport: {
                        update: {
                            url: $.helper.resolveApi("~/finance/Account/CoaBalance/SingleUpdate"),
                            type: 'POST',
                            contentType: 'application/json',
                            dataType: 'json'
                        }
                    },
                    group: { field: "is_normal_debit", aggregates: [{ field: "is_normal_debit", aggregate: "count" }] },
                    sort: { field: "account_number", dir: "asc" },
                    pageSize: 10,
                    batch: false,
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        model: {
                            id: "id",
                            fields: {
                                financial_transaction_id: { nullable: false },
                                exchange_rate: { type: "number", validation: { required: true, min: 1 } },
                                debit: { type: "number", validation: { min: 0 } },
                                credit: { type: "number", validation: { min: 0 } },
                                normalized_debit: { type: "number", validation: { min: 0 } },
                                normalized_credit: { type: "number", validation: { min: 0 } }
                            }
                        }
                    },

                },
                schema: {
                    model: {
                        id: "id"
                    }
                },
                marvelCheckboxSelectable: {
                    enable: false,
                    listCheckedTemp: ListChecked
                },
                dataBound: function (e) {
                    $.helper.kendoUI.resizeGrid();
                },
                columns: [
                    {
                        command: ["edit"], title: "&nbsp;", width: "150px",
                        //locked: true
                        //lockable: false
                    },
                    {
                        field: "account_number",
                        title: "Account Code",
                        width: "150px",
                        template: "<b>#= data.account_number #</b><br>#= data.account_name #",
                        editable: function () { return false; },
                        searchable: true
                    },
                    {
                        field: "is_normal_debit",
                        title: "Condition",
                        width: "150px",
                        hidden: true,
                        template: function (data) {
                            if (data.is_normal_debit) {
                                return "<b>Debit</b>";
                            } else
                                return "<b>Credit</b>";
                        },
                        groupHeaderTemplate: "Condition #=value ? 'Debit':'Credit' # - Count: #=count#"
                    },
                    {
                        field: "currency_code",
                        title: "Currency Code",
                        width: "80px",
                        template: "#= data.currency_code #",
                        editor: currencyEditor,
                        filterable: false,
                    },
                    {
                        field: "exchange_rate",
                        title: "Exchange Rate",
                        width: "80px",
                        template: "<b>Rp. </b>#= kendo.toString(data.exchange_rate, 'n') #",
                        filterable: false,
                    },
                    {
                        field: "debit",
                        title: "Debit",
                        width: "100px",
                        headerAttributes: {
                            style: "text-align:right;"
                        },
                        attributes: {
                            style: "text-align:right;"
                        },
                        template: "<b>#= data.currency_symbol # </b>#= kendo.toString(data.debit, 'n') #",
                        groupable: false,
                        editable: function (e) { if (e.is_normal_debit) return true; else return false; },
                        filterable: false,
                    },
                    {
                        field: "credit",
                        title: "Credit",
                        width: "100px",
                        headerAttributes: {
                            style: "text-align:right;"
                        },
                        attributes: {
                            style: "text-align:right;"
                        },
                        template: "<b>#= data.currency_symbol # </b>#= kendo.toString(data.credit, 'n') #",
                        groupable: false,
                        editable: function (e) { if (!e.is_normal_debit) return true; else return false; },
                        filterable: false,
                    },
                    {
                        title: "Debit <b>(N)</b>",
                        width: "100px",
                        headerAttributes: {
                            style: "text-align:right;"
                        },
                        attributes: {
                            style: "text-align:right;"
                        },
                        template: debitTemplate,
                        groupable: false,
                        editable: function (e) { if (!e.is_normal_debit) return true; else return false; },
                        filterable: false,
                    },
                    {
                        title: "Credit <b>(N)</b>",
                        width: "100px",
                        headerAttributes: {
                            style: "text-align:right;"
                        },
                        attributes: {
                            style: "text-align:right;"
                        },
                        template: creditTemplate,
                        groupable: false,
                        editable: function (e) { if (!e.is_normal_debit) return true; else return false; },
                        filterable: false,
                    }

                ]
            });


            //gridUI.data('kendoGrid').dataSource.sync();
        }

        function debitTemplate(dataItem) {
            dataItem.normalized_debit = dataItem.debit * dataItem.exchange_rate;
            return "<b>Rp.</b> " + kendo.toString(dataItem.normalized_debit || 0, 'n');
        }

        function creditTemplate(dataItem) {
            dataItem.normalized_credit = dataItem.credit * dataItem.exchange_rate;
            return "<b>Rp.</b> " + kendo.toString(dataItem.normalized_credit || 0, 'n');
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
            console.log('editor currency');
            console.log(options.model.currency_id);
            console.log(options.model.currency_code);
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
                        console.log('last currency : ' + options.model.currency_id);
                        options.model.currency_id = equipmentModel.id;
                        options.model.currency_symbol = equipmentModel.currency_symbol;
                        options.model.currency_code = equipmentModel.currency_code;
                        console.log('current currency : ' + options.model.currency_id);
                        console.log('equip model id : ' + equipmentModel.id);
                        console.log('CODE : ' + equipmentModel.currency_code);
                        //options.model.set("currency_id", equipmentModel.currency_id);
                        //options.model.set("currency_symbol", equipmentModel.currency_symbol);
                        options.model.set("currency_code", equipmentModel.currency_code);
                    }
                });
        }

        //BEGIN::EVENT BUTTON
        $('#btn-sync_account').click(function () {

        });

        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
                loadCoaBalance();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));