(function ($) {
    'use strict';

    var _detailID = $('#detail-id');
    
    var dgdetail = $('#dgdetail');
    var legalEntityId = $('#legal_entity_id').val();
    var financial_transaction_id = $('#financial_transaction_id');
    var pageFunction = function () {

        var _formBankPayment = $('#form-BankPayment');
    
        //BEGIN::Form Validation
        _formBankPayment.validate({
            rules: {
                requested_on: {
                    required: true
                },
                currency_id: {
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
                },
                payment_amount: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                mUtil.scrollTo(_formBankPayment, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::LOOKUP
        var $transactionType = $('#transaction_type_id');
        var $company = $("#paid_to_vendor_id");
        var $legalEntity = $("#legal_entity_id");
        var $affiliate = $("#affiliation_id");
        var $bankPaymentMethod = $("#bank_payment_method_id");
        var $bankPaymentType = $("#bank_payment_type_id");
        var $currency = $("#currency_id");
        var $account = $("#source_account_id");
        var $voucherLookup = $("#voucher_id");

        //$transactionType.select2({ width: '100%' });


        $transactionType.cmSelect2({
            url: $.helper.resolveApi("~/finance/FinancialTransactionType/lookup"),
            filters: ' r.transaction_type like @0 ',
            result: {
                id: 'id',
                text: 'transaction_type'
            }
        });

        $company.cmSelect2({
            url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
            filters: ' ( r.company_name like @0 or r.registration_code like @0 ) and r.is_vendor= 1',
            result: {
                id: 'id',
                text: 'company_name'
            }
        });


        $voucherLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/Voucher/lookup"),
            filters: ' r.voucher_name like @0 ',
            result: {
                id: 'id',
                text: 'voucher_name'
            }
        });

        $legalEntity.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name like @0 ',
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        }).on('change', function () {
            legalEntityId = $legalEntity.val();

            $account.cmSelect2({
                url: $.helper.resolveApi("~/Finance/Account/lookup"),
                filters: ' ( r.account_name like @0 OR r.account_number like @0 ) ' +
                    ' AND r.legal_entity_id = @1 AND r.account_level = 5 ',
                parameters: [legalEntityId],
                result: {
                    id: 'id',
                    text: 'account_name'
                }
            });

            transactionDetails.reload();
        });

        $affiliate.cmSelect2({
            url: $.helper.resolveApi("~/core/Affiliation/lookup"),
            filters: ' r.affiliation_name like @0 ',
            result: {
                id: 'id',
                text: 'affiliation_name'
            }
        });

        $bankPaymentMethod.cmSelect2({
            url: $.helper.resolveApi("~/Finance/BankPaymentMethod/lookup"),
            filters: ' r.method_name like @0 ',
            result: {
                id: 'id',
                text: 'method_name'
            }
        });

        $bankPaymentType.cmSelect2({
            url: $.helper.resolveApi("~/Finance/BankPaymentType/lookup"),
            filters: ' r.payment_type like @0 ',
            result: {
                id: 'id',
                text: 'payment_type'
            }
        });

        $currency.cmSelect2({
            url: $.helper.resolveApi("~/Core/Currency/lookup"),
            filters: ' ( r.currency_name like @0 OR r.currency_code like @0 ) ',
            result: {
                id: 'id',
                text: 'currency_code'
            }
        });

        $account.cmSelect2({
            url: $.helper.resolveApi("~/Finance/Account/lookup"),
            filters: " ( r.account_name like @0 OR r.account_number like @0 ) AND r.account_group_id = '65c9dc25-0b84-4d60-a541-35c06e4153fa' AND r.account_level = 5 ",
            result: {
                id: 'id',
                text: 'account_name'
            }
        });

        //END::LOOKUP

        $("#requested_on").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $("#due_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        var editIndex = undefined;
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
                        url: $.helper.resolveApi("~/Finance/BankPayment/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                
                                
                                $('input[name="requested_on"]').val(record.requested_on);
                                $('input[name="due_date"]').val(record.due_date);
                                $company.cmSetLookup(record.paid_to_vendor_id, record.company_name);
                                $('input[name="payment_number"]').val(record.payment_number);
                                $('input[name="payment_amount"]').val(record.payment_amount);
                                $('input[name="remarks"]').val(record.remarks);
                                
                                $('input[name="exchange_rate"]').val(record.exchange_rate);
                                $('input[name="reference_number"]').val(record.reference_number);
                                $('input[name="payment_charges"]').val(record.payment_charges);
                                $('input[name="business_unit_id"]').val(record.business_unit_id);
                                console.log('disini');
                                console.log(record);
                                $('input[id="business_unit_id"]').val(record.business_unit_name);
                                financial_transaction_id = record.financial_transaction_id;
                                $legalEntity.cmSetLookup(record.legal_entity_id, record.legal_entity_name);
                                $affiliate.cmSetLookup(record.affiliation_id, record.affiliation_name);
                                $transactionType.cmSetLookup(record.transaction_type_id, record.transaction_type);
                                $bankPaymentMethod.cmSetLookup(record.bank_payment_method_id, record.bank_payment_method);
                                $bankPaymentType.cmSetLookup(record.bank_payment_type_id, record.bank_payment_type);
                                $currency.cmSetLookup(record.currency_id, record.currency_code);
                                $account.cmSetLookup(record.source_account_id, record.account_name);

                                financialTransactionDetails(record.financial_transaction_id);
                                /*
                                 * check submited
                                 * submited = is_locked eq true
    
                                */
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

            } else {
                formControll();
                setStatus('new');
            }

        };

        function financialTransactionDetails(financialTransactionId) {
            if (financialTransactionId !== '') {
                console.log('masuk');
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/Finance/FinancialTransaction/" + financialTransactionId + "/details"),
                    success: function (r) {
                        if (r.status.success && r.data) {
                            var record = r.data;
                            console.log(record.approval_id);
                            $('input[name="approval_id"]').val(record.approval_id);
                            formControll();
                        }
                    }
                })
            }
        }

        //BEGIN::Load Financial Transaction Detail
        var transactionDetails = function () {
            var Geturl = function () {
                return $.helper.resolveApi('~/finance/BankPayment/transactiondetail/dgtable?bankpaymentid=' + _detailID.val());
            };

            //async=false
            var dgOptions = function () {
                var accountDataLookup = undefined;
                $.ajax({
                    url: $.helper.resolveApi("~/finance/Account/lookup?length=300"),
                    data: {
                        filters: ' ( r.legal_entity_id = @0 AND r.account_level = 5 ) ',
                        parameters: legalEntityId
                    },
                    type: "GET",
                    success: function (response) {
                        accountDataLookup = response;
                    },
                    async: false
                });

                var options = {
                    toolbar: '#tb',
                    iconCls: 'icon-edit',
                    singleSelect: true,
                    pagination: false,
                    clientPaging: false,
                    remoteFilter: true,
                    showFooter: true,
                    rownumbers: true,
                    onClickCell: onClickCell,
                    pageSize: null,
                    onEndEdit: onEndEdit,
                    url: Geturl(),
                    idField: 'id',
                    generalFilterColumns: [
                        {
                            name: 'account_id',
                            filterable: true
                        },
                        {
                            name: 'company_name',
                            filterable: true
                        }
                    ],
                    generalSearch: $('#dt_basic_eui-search'),
                    //orders: [{ sortName: 'account_number', sortOrder: 'asc' }],
                    frozenColumns: [[
                        {
                            field: 'account_id', title: 'Account', align: 'center', width: '50%',
                            formatter: function (value, row) {
                                return row.account_name;
                            },
                            editor: {
                                type: 'combogrid',
                                options: {
                                    panelWidth: 500,
                                    idField: 'id',
                                    textField: 'account_name',
                                    fitColumns: true,
                                    filter: function (q, row) {
                                        //this will check in two columns...
                                        return (row['account_name'].indexOf(q) !== -1 || row['account_number'].indexOf(q) !== -1);
                                    },
                                    columns: [[
                                        { field: 'account_name', title: 'Account Name', width: 200 },
                                        { field: 'account_number', title: 'Number', width: 100 }
                                    ]],
                                    data: accountDataLookup.items,
                                    required: true
                                }
                            }
                        }
                    ]],
                    columns: [[
                        {
                            field: 'id', title: 'ID', width: 0, align: 'center',
                            formatter: function (value, row) {
                                return row.id;
                            }
                        },
                        { field: 'debit', title: 'Debit', width: '25%', align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } },
                        { field: 'credit', title: 'Credit', width: '25%', align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } }
                    ]],
                    renderRow: function (target, fields, frozen, rowIndex, rowData) {
                        console.log('render row');
                    },
                    onLoadSuccess: function (data) {
                        $(this).datagrid('hideColumn', 'id');
                        footerCalculate();
                    }
                };
                return options;
            };

            return {
                init: function () {
                    dgdetail.cmDataGrid(dgOptions());
                },
                reload: function () {
                    dgdetail.cmDataGrid(dgOptions());
                }
            };
        }();
        //END::Load Financial Transaction Detail


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
                    dgdetail.datagrid('loading');
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
                                    //dgdetail.datagrid('cancelEdit', editIndex)
                                    //    .datagrid('deleteRow', editIndex);
                                    //editIndex = undefined;
                                    dgdetail.datagrid('reload');
                                    editIndex = undefined;
                                    footerCalculate();
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
                        dgdetail.datagrid('loaded');
                    }, 2e3));
                }
            });
        };


        var formControll = function () {
            $('#btn-save').removeAttr('disabled').show();
            if (_detailID.val() !== '') {
                console.log('idfound');
                $('#btn-approval').removeAttr('disabled', '').show();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();

                if ($('input[name="approval_id"]').val() !== '') {
                    console.log('approval id not found');
                    $('#btn-approval').attr('disabled', 'disabled').hide();
                    $('#btn-remove_approval').removeAttr('disabled', '').show();

                    $('#btn-submit').removeAttr('disabled', '').show();
                }

            } else {
                $('#btn-approval').attr('disabled', 'disabled').hide();
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


        function endEditing() {
            if (editIndex === undefined) { return true; }
            if (dgdetail.datagrid('validateRow', editIndex)) {
                dgdetail.datagrid('endEdit', editIndex);
                editIndex = undefined;
                return true;
            } else {
                return false;
            }
        }


        function footerCalculate() {
            var totalDebit = Number(0);
            var totalCredit = Number(0);
            var rows = dgdetail.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                totalDebit += Math.round(Number(rows[i].debit || 0));
                totalCredit += Math.round(Number(rows[i].credit || 0));
            }
            dgdetail.datagrid('reloadFooter', [{ account_name: 'Total', debit: Math.round(totalDebit), credit: Math.round(totalCredit) }]);
        }

        function onEndEdit(index, row) {
            var account = $(this).datagrid('getEditor', {
                index: index,
                field: 'account_id'
            });

            row.account_name = $(account.target).combobox('getText');

            footerCalculate();
        }

        function onClickCell(index, field) {
            if (editIndex !== index) {
                if (endEditing()) {
                    $(this).datagrid('selectRow', index)
                        .datagrid('beginEdit', index);
                    var ed = $(this).datagrid('getEditor', { index: index, field: field });
                    if (ed) {
                        ($(ed.target).data('textbox') ? $(ed.target).textbox('textbox') : $(ed.target)).focus();
                    }
                    editIndex = index;
                } else {
                    setTimeout(function () {
                        $(this).datagrid('selectRow', editIndex);
                    }, 0);
                }
            }
        }

        function append() {
            if (endEditing()) {
                dgdetail.datagrid('appendRow', {});
                editIndex = dgdetail.datagrid('getRows').length - 1;
                dgdetail.datagrid('selectRow', editIndex)
                    .datagrid('beginEdit', editIndex);
            }
        }

        function accept() {
            if (endEditing()) {
                dgdetail.datagrid('acceptChanges');
            }
        }

        function removeit() {
            if (editIndex === undefined) { return; }

            var row = dgdetail.datagrid('getSelected');

            if (row && row.id) {
                console.log(row);
                var ids = [];
                ids.push(row.id);
                DeleteData(ids);
            } else {
                dgdetail.datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                editIndex = undefined;
            }
        }

        function reject() {
            dgdetail.datagrid('rejectChanges');
            editIndex = undefined;
        }

        function getChanges() {
            var rows = dgdetail.datagrid('getChanges');
            alert(rows.length + ' rows are changed!');
        }

        $('#btn-append').click(function () {
            append();
        });


        $('#btn-accept').click(function () {
            accept();
        });

        $('#btn-reject').click(function () {
            reject();
        });

        $('#btn-removeit').click(function () {
            removeit();
        });

        $('#btn-save').click(function () {
            var btn = $(this);
            if (_formBankPayment.valid()) {
                var financialTransaction = _formBankPayment.serializeToJSON();
                var detailTransactions = dgdetail.datagrid('getData');
                var data = {
                    bank_payment: financialTransaction,
                    details: detailTransactions.rows
                };
                console.log(data);

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/BankPayment/savetodraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="payment_number"]').val(r.data.payment_number);
                            transactionDetails.reload();
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
            if ($('input[name="approval_id"]').val() !== '') {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/finance/FinancialTransaction/submited?RecordId=" + financial_transaction_id),
                    success: function (r) {
                        if (r && r.status.success) {
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                            formControllSubmited(true);
                            //transactionDetails.reload();
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
                        url: $.helper.resolveApi("~/core/Approval/removeApprovalEntity/" + $('#entity_id').val() + "/?RecordID=" + financial_transaction_id + "&ApprovalID=" + $('input[name="approval_id"]').val()),
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
            var url = $.helper.resolve("~/Core/ApprovalTemplate/SetupApproval/?EntityId=" + $('#entity_id').val() + "&RecordID=" + financial_transaction_id
                + "&Title=Bank Payment&Ref=" + $('input[name="payment_number"]').val() + "&url=~/Finance/BankPayment/Detail/?id=" + _detailID.val());
            window.location.href = url;
        });

        return {
            init: function () {
                //initialize
                loadDetailData(),
                    transactionDetails.init();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));