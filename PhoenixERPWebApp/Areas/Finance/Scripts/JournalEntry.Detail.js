function calculateDebit(dataItem) {
    dataItem.normalized_debit = dataItem.debit * dataItem.exchange_rate;
    return dataItem.normalized_debit || 0;
}
function calculateCredit(dataItem) {
    dataItem.normalized_credit = dataItem.credit * dataItem.exchange_rate;
    return dataItem.normalized_credit || 0;
}
(function ($) {
    'use strict';

    var _detailID = $('#detail-id');

    var gridUI = $('#grid');
    var legalEntityId = $('#legal_entity_id').val();

    var pageFunction = function () {

        var _formFinancialTransaction = $('#form-financial_transaction');
        $('input[type=radio][name="is_internal"]').change(function () {
            if (this.value == "true") {
                $companyLookup.attr('disabled', 'disabled');
                $companyLookup.val('').trigger('change');
                $('input[name="company_name"]').val('');
            }
            else if (this.value == "false") {
                $companyLookup.removeAttr('disabled');
            }
        });


        var accountDataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: $.helper.resolveApi("~/finance/Account/lookup?length=1000"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'GET',
                    cache: true
                },
                parameterMap: function (data) {
                    var mapRequest = data;
                    var extractOption = {
                        filters: ' ( r.legal_entity_id = @0 AND r.account_level = 5 ) ',
                        parameters: legalEntityId
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

        var companyDataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'GET',
                    cache: true
                },
                parameterMap: function (data) {
                    var mapRequest = data;
                    var extractOption = {
                        filters: " ( r.company_name like @0 or r.registration_code like @0  )",
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
        companyDataSource.read();

        var businessUnitDataSource = new kendo.data.HierarchicalDataSource({
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
        });
        businessUnitDataSource.read();

        var affiliationDataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: $.helper.resolveApi("~/core/Affiliation/lookup"),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'GET',
                    cache: true
                },
                parameterMap: function (data) {
                    var mapRequest = data;
                    var extractOption = {
                        filters: " ( r.affiliation_name like @0 )",
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
        affiliationDataSource.read();

        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/FinancialTransactionDetail/KendoGrid?financialtransactionid=' + _detailID.val())
            },
            navigatable: true,
            pageable: false,
            //height: 300,
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
                    { field: "normalized_debit", aggregate: "sum" },
                    { field: "normalized_credit", aggregate: "sum" }
                ],
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            id: { editable: true, nullable: true },
                            //account_name: { editable: true, validation: { required: true }  }
                            //is_internal: { type: "bool",validation: { required: true } },
                            //debit: { type: "number", validation: { required: true, min: 0 } },
                            //credit: { type: "number", validation: { required: true, min: 0 } }
                            //normalized_debit: { type: "number", validation: { required: true, min: 0 } },
                            //normalized_credit: { type: "number", validation: { required: true, min: 0 } }
                        }
                    },
                    data: "data",
                    total: "recordsTotal"
                },
            },
            aggregate: [
                { field: "normalized_debit", aggregate: "sum" },
                { field: "normalized_credit", aggregate: "sum" }
            ],
            marvelCheckboxSelectable: {
                enable: false
            },
            dataBound: function (e) {
                var data = e.sender.dataSource.view().toJSON();
                var TotalDebit = 0;
                var TotalCredit = 0;
                $.each(data, function (i, v) {
                    TotalDebit += v.normalized_debit || 0;
                    TotalCredit += v.normalized_credit || 0;
                });

                var footerDebit = $('span.totalDebit');
                var footerCredit = $('span.totalCredit');


                $(footerDebit).html(TotalDebit);
                $(footerCredit).html(TotalCredit);

                $.helper.kendoUI.resizeGrid();
            },
            toolbar: ["create", "cancel", "Reload"],
            columns: [
                {
                    template: "<b>#= data.account_name ? account_name : ''#</b>",
                    field: "account_name",
                    title: "Account",
                    width: "250px",
                    locked: true,
                    lockable: false,
                    groupable: false,
                    sortable: false,
                    editor: accountEditor
                },
                {
                    field: "debit",
                    title: "Debit",
                    attributes: {
                        style: "text-align:right;"
                    },
                    width: "150px"
                },
                {
                    field: "credit",
                    title: "Credit",
                    attributes: {
                        style: "text-align:right;"
                    },
                    width: "150px",
                },
                {
                    field: "currency_code",
                    title: "Currency",
                    width: "100px",
                    editor: currencyEditor
                },
                {
                    field: "exchange_rate",
                    title: "Exchange Rate",
                    attributes: {
                        style: "text-align:right;"
                    },
                    width: "120px"
                },
                {
                    field: "normalized_debit",
                    title: "Debit(N)",
                    editable: function () { return false },
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: "<b>#= calculateDebit(data) #</b>",
                    footerTemplate: "<span class='totalDebit'></span>",
                    width: "150px",
                },
                {
                    field: "normalized_credit",
                    title: "Credit(N)",
                    editable: function () { return false },
                    attributes: {
                        style: "text-align:right;"
                    },
                    aggregates: ["sum"],
                    footerTemplate: "<span class='totalCredit'></span>",
                    template: "<b>#= calculateCredit(data) #</b>",
                    width: "150px",
                },
                {
                    field: "is_internal",
                    title: "Internal",
                    width: "80px",
                    attributes: {
                        style: "text-align:center;align:center"
                    },
                    template: function (e) {
                        if (e.is_internal) {
                            e.company_id = null;
                            e.company_name = null;
                            return "Y";
                        } else {
                            return "N";
                        }
                    },
                    editor: function (container, options) {
                        $('<input type="checkbox" style="align:center" name="' + options.field + '"/>').appendTo(container);

                    }
                },
                {
                    field: "company_name",
                    title: "Company",
                    width: "250px",
                    editor: companyEditor
                },
                {
                    field: "unit_name",
                    title: "Bussiness Unit",
                    width: "250px",
                    editor: businessUnitEditor
                },
                {
                    field: "affiliation_name",
                    title: "Affiliation",
                    width: "250px",
                    editor: affiliationEditor
                },
                {
                    command: [
                        {
                            name: "Undo", click: function (e) {
                                e.preventDefault();
                                var row = e.target.closest('tr')
                                var uid = $(row).data(uid)
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);

                                dataSource.cancelChanges(item);
                                this.refresh()
                            }
                        },
                        {
                            name: "rowDelete", text: "Delete", click: function (e) {
                                e.preventDefault();
                                var row = e.target.closest('tr')
                                var uid = $(row).data(uid);
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);
                                if (item.id) DeleteData([item.id]);
                                else {
                                    dataSource.cancelChanges(item);
                                }
                            }
                        }
                    ], title: "&nbsp;", width: "200px"
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


        function accountEditor(container, options) {
            $('<input name="' + options.field + '"/>')
                .appendTo(container)
                .kendoMultiColumnComboBox({
                    autoBind: false,
                    dataTextField: "account_name",
                    dataValueField: "account_name",
                    value: options.model.account_id,
                    text: options.model.account_name,
                    height: 550,
                    columns: [
                        { field: "account_number", title: "Account", width: 200 },
                        { field: "account_name", title: "Name", width: 200 }
                    ],
                    dataSource: accountDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        console.log(equipmentModel);
                        options.model.account_id = equipmentModel.id;
                        options.model.set("account_name", equipmentModel.account_name);
                    }
                });
        }
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
                        options.model.currency_id = equipmentModel.id;
                        options.model.set("currency_code", equipmentModel.currency_code);
                    }
                });
        }
        function companyEditor(container, options) {
            $('<input name="' + options.field + '"/>')
                .appendTo(container)
                .kendoComboBox({
                    autoBind: false,
                    dataTextField: "company_name",
                    dataValueField: "company_name",
                    value: options.model.company_id,
                    text: options.model.company_name,
                    dataSource: companyDataSource,
                    change: function (e) {
                        try {
                            var equipmentModel = this.dataItem(e.item);
                            options.model.company_id = equipmentModel.id;
                            options.model.set("company_name", equipmentModel.company_name);
                        } catch (e) { }

                    }
                });
        }
        function businessUnitEditor(container, options) {
            businessUnitDataSource.read();
            $('<input name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownTree({
                    autoBind: true,
                    placeholder: "Select ...",
                    dataTextField: "text",
                    dataValueField: "id",
                    value: options.model.business_unit_id || null,
                    text: options.model.unit_name || null,
                    dataSource: businessUnitDataSource,
                    height: "auto",
                    treeview: {
                        select: function (e) {
                            var dataItem = e.sender.dataItem(e.node);
                            if (options.model.business_unit_id !== dataItem.id) {
                                //options.model.business_unit_id = dataItem.id;
                                //options.model.unit_name = dataItem.text;
                                options.model.set("business_unit_id", dataItem.id);
                                options.model.set("unit_name", dataItem.text);

                            }
                        }
                    }
                });
        }
        function affiliationEditor(container, options) {
            $('<input name="' + options.field + '"/>')
                .appendTo(container)
                .kendoComboBox({
                    autoBind: false,
                    dataTextField: "affiliation_name",
                    dataValueField: "affiliation_name",
                    value: options.model.affiliation_id,
                    text: options.model.affiliation_name,
                    dataSource: affiliationDataSource,
                    change: function (e) {
                        var equipmentModel = this.dataItem(e.item);
                        options.model.affiliation_id = equipmentModel.id;
                        options.model.set("affiliation_name", equipmentModel.affiliation_name);
                    }
                });
        }

        //BEGIN::Form Validation
        _formFinancialTransaction.validate({
            rules: {
                transaction_datetime: {
                    required: true
                },
                self_reference: {
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
                mUtil.scrollTo(_formFinancialTransaction, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::LOOKUP
        var $transactionType = $('#transaction_type_id');
        var $companyLookup = $("#company_id");
        var $legalEntityLookup = $("#legal_entity_id");
        var $affiliateLookup = $("#affiliation_id");

        //$transactionType.select2({ width: '100%' });

        $transactionType.cmSelect2({
            url: $.helper.resolveApi("~/finance/FinancialTransactionType/lookup"),
            filters: ' r.transaction_type like @0 ',
            result: {
                id: 'id',
                text: 'transaction_type'
            }
        });

        $companyLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
            filters: ' r.company_name like @0 or r.registration_code like @0  ',
            result: {
                id: 'id',
                text: 'company_name'
            },
            options: {
                //allowClear: true
            }
        }).on('select2:select', function (e) {
            var data = e.params.data;
            console.log(data);
            $('input[name="company_name"]').val(data.text);
        });

        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name like @0 ',
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        }).on('change', function () {
            legalEntityId = $legalEntityLookup.val();
            accountDataSource.read();
        });

        $affiliateLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Affiliation/lookup"),
            filters: ' r.affiliation_name like @0 ',
            result: {
                id: 'id',
                text: 'affiliation_name'
            }
        });

        //END::LOOKUP

        $("#transaction_datetime").datepicker({
            todayHighlight: !0,
            //autoclose: !0,
            orientation: "bottom-left",
            format: "mm/dd/yyyy"
        });

        //BEGIN::Load Data
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
                        url: $.helper.resolveApi("~/Finance/FinancialTransaction/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                console.log(record);
                                //return;
                                $('input[name="approval_id"]').val(record.approval_id);
                                $('input[name="transaction_number"]').val(record.transaction_number);
                                //$('#transaction_datetime').val();
                                $('#transaction_datetime').datepicker("setDate", moment(record.transaction_datetime).format("DD/MM/YYYY"));
                                $('input[name="reference"]').val(record.reference);
                                $('input[name="description"]').val(record.description);

                                $companyLookup.cmSetLookup(record.company_id, record.company_name);

                                $('input[name="business_unit_id"]').val(record.business_unit_id);
                                $('input[id="business_unit_id"]').val(record.unit_name);
                                $("input[name=is_internal][value='" + record.is_internal + "']").trigger('click');
                                legalEntityId = record.legal_entity_id;
                                $legalEntityLookup.cmSetLookup(record.legal_entity_id, record.legal_entity_name);
                                if (legalEntityId !== '') $legalEntityLookup.attr('disabled', 'disabled');


                                $affiliateLookup.cmSetLookup(record.affiliation_id, record.affiliation_name);
                                $transactionType.cmSetLookup(record.transaction_type_id, record.transaction_type);

                                /*
                                 * check submited
                                 * submited = is_locked eq true
     
                                */
                                formControll();
                                if (record.is_locked) {
                                    formControllSubmited(true);
                                    $('.adjustment').show();
                                    setStatus('submited');
                                } else {
                                    setStatus('draft');
                                    $('.adjustment').hide();
                                }
                                //transactionDetails.reload();

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

            } else {
                formControll();
                setStatus('new');
            }

        };


        //END::Load Data
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

                    $.ajax({
                        type: "DELETE",
                        dataType: 'json',
                        contentType: 'application/json',
                        url: $.helper.resolveApi("~/Finance/FinancialTransactionDetail/delete"),
                        data: JSON.stringify(ArrID),
                        success: function (r) {

                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                }).then(function () {
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

                    }, 2e3));
                }
            });
        };

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
            if (submited) {
                gridUI.data("kendoGrid").setOptions({
                    editable: false
                });
                gridUI.find('div.k-grid-toolbar').remove();
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

        function removeit() {
            //if (editIndex === undefined) { return }

            //var row = dgdetail.datagrid('getSelected');

            //if (row && row.id) {
            //    console.log(row);
            //    var ids = [];
            //    ids.push(row.id);
            //    DeleteData(ids);
            //} else {
            //    dgdetail.datagrid('cancelEdit', editIndex)
            //        .datagrid('deleteRow', editIndex);
            //    editIndex = undefined;
            //}
        }

        $('#btn-save').click(function () {
            var btn = $(this);
            if (_formFinancialTransaction.valid()) {
                var financialTransaction = _formFinancialTransaction.serializeToJSON();
                var transactionDetailKendo = $("#grid").data("kendoGrid");
                var data = {
                    transaction: financialTransaction,
                    details: transactionDetailKendo.dataSource.view().toJSON()
                };
                var grid = gridUI.getKendoGrid();
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/FinancialTransaction/savetodraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="transaction_number"]').val(r.data.transaction_number);
                            reloadGrid();
                            setStatus('draft');
                            formControll();
                            toastr.success(r.status.message, "Information");
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
            if ($('input[name="approval_id"]').val() !== '') {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/finance/FinancialTransaction/submited?RecordId=" + _detailID.val()),
                    success: function (r) {
                        if (r && r.status.success) {
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                            formControllSubmited(true);
                            setStatus('submited');
                            $('.adjustment').show();
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

        $('#btn-adjustment').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            window.location.href = $.helper.resolve('~/Finance/AdjustmentJournalEntry/Detail/?selfReference=' + $('input[name="transaction_number"]').val());
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

        $('#btn-choose_business_unit').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            $('.modal-container').load($.helper.resolve("~/Core/BusinessUnit/Lookup/"), function (result) {
                var $modal = $('#m_modal_business_unit');

                $modal.modal({ show: true });
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                var $tree = $('#tree_businessunit');
                $modal.find('#btn-addselected').click(function () {
                    var selected = $tree.jstree("get_selected", true)[0];

                    $('input[name="business_unit_id"]').val(selected.id);
                    $('input[id="business_unit_id"]').val(selected.text);
                    $modal.modal('toggle');
                });


                //$("#m_modal_business_unit").on('hidden.bs.modal', function () {

                //});
            });
        });

        $('#btn-approval').click(function () {
            var url = $.helper.resolve("~/Core/ApprovalTemplate/SetupApproval/?EntityId=" + $('#entity_id').val() + "&RecordID=" + _detailID.val()
                + "&Title=Journal Entry&Ref=" + $('input[name="transaction_number"]').val() + "&url=~/Finance/JournalEntry/Detail/?id=" + _detailID.val());
            window.location.href = url;
        });

        return {
            init: function () {
                //initialize
                loadDetailData();
                //transactionDetails.init();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));

