(function ($) {
    'use strict';

    var _detailID = $('#detail-id');
    var dgdetail = $('#dgdetail');
    var legalEntityId = $('#legal_entity_id').val();

    var pageFunction = function () {

        var _formFinancialTransaction = $('#form-financial_transaction');
        console.log($('input[name="approval_id"]').val());

        //BEGIN::Form Validation
        _formFinancialTransaction.validate({
            rules: {
                transaction_datetime: {
                    required: true
                },
                self_reference: {
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
            filters: " r.transaction_type like @0 AND not (r.transaction_type like '%balance%' OR r.transaction_type like '%budget%' )  ",
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
            }
        }).on('select2:select', function (e) {
            var data = e.params.data;
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
            transactionDetails.reload();
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

        $("#transaction_datetime").datetimepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd hh:ii"
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
                        url: $.helper.resolveApi("~/Finance/FinancialTransaction/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="approval_id"]').val(record.approval_id);
                                $('input[name="transaction_number"]').val(record.transaction_number);
                                $('input[name="transaction_datetime"]').val(record.transaction_datetime);
                                $('input[name="self_reference"]').val(record.self_reference);
                                $('input[name="reference"]').val(record.reference);
                                $('input[name="description"]').val(record.description);

                                $companyLookup.cmSetLookup(record.company_id, record.company_name);

                                $('input[name="business_unit_id"]').val(record.business_unit_id);
                                $('input[id="business_unit_id"]').val(record.unit_name);

                                legalEntityId = record.legal_entity_id;
                                
                                $legalEntityLookup.cmSetLookup(record.legal_entity_id, record.legal_entity_name);
                                $affiliateLookup.cmSetLookup(record.affiliation_id, record.affiliation_name);
                                $transactionType.cmSetLookup(record.transaction_type_id, record.transaction_type);

                                /*
                                 * check submited
                                 * submited = is_locked eq true
    
                                */
                                formControll();
                                if (record.is_locked) {
                                    formControllSubmited(true);
                                    setStatus('submited');
                                } else {
                                    setStatus('draft');
                                }
                                transactionDetails.reload();

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

        //BEGIN::Load Financial Transaction Detail
        var transactionDetails = function () {
            var Geturl = function () {
                return $.helper.resolveApi('~/finance/FinancialTransactionDetail/dgtable?financialtransactionid=' + _detailID.val());
            };

            //async=false
            var dgOptions = function () {
                var accountDataLookup = undefined;
                $.ajax({
                    url: $.helper.resolveApi("~/finance/Account/lookup?length=1000"),
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
                            field: 'account_id', title: 'Account', align: 'center', width: '20%',
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
                            field: 'id', title: 'ID', width: 0, align: 'center', rowspan: 2,
                            formatter: function (value, row) {
                                return row.id;
                            }
                        },
                        { field: 'debit', title: 'Debit', rowspan: 2, width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } },
                        { field: 'credit', title: 'Credit', rowspan: 2, width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } },
                        { field: 'ref', title: 'Reference', align: 'center', colspan: 3, editor: 'text' }
                    ],
                    [
                        {
                            field: 'company_id', title: 'Company', width: '20%', align: 'center',
                            formatter: function (value, row) {

                                return row.company_name;
                            },
                            editor: {
                                type: 'combogrid',
                                options: {
                                    panelWidth: 500,
                                    mode: 'remote',
                                    idField: 'id',
                                    textField: 'company_name',
                                    fitColumns: true,
                                    loader: function (param, success, error) {
                                        var url = $.helper.resolveApi("~/finance/CompanyProfile/lookup");
                                        var filters = " r.company_name like @0 or r.registration_code like @0 ";
                                        var parameters = [];
                                        if (param.q !== undefined) parameters.push('%' + param.q + '%');
                                        else parameters.push('%%');
                                        $.ajax({
                                            type: 'GET',
                                            url: url,
                                            data: {
                                                length: 100,
                                                filters: "( " + filters + " )",
                                                parameters: parameters.toString()  // search term
                                            },
                                            dataType: 'json',
                                            success: function (data) {

                                                success(data.items);
                                            },
                                            error: function () {
                                                error.apply(this, arguments);
                                            }
                                        });
                                    },
                                    columns: [[
                                        { field: 'company_name', title: 'Company Name', width: 200 },
                                        { field: 'registration_code', title: 'Registration Code', width: 100 },
                                    ]]
                                }
                            }
                        },
                        {
                            field: 'business_unit_id', title: 'Business Unit', width: '20%', align: 'center',
                            formatter: function (value, row) {
                                return row.unit_name || value;
                            },
                            editor: {
                                type: 'combotree',
                                options: {
                                    panelWidth: 500,
                                    valueField: 'id',
                                    textField: 'unit_name',
                                    loader: function (param, success, error) {
                                        var url = $.helper.resolveApi("~/core/BusinessUnit");
                                        $.ajax({
                                            type: 'GET',
                                            url: url,
                                            dataType: 'json',
                                            success: function (data) {
                                                success(data.data);
                                            },
                                            error: function () {
                                                error.apply(this, arguments);
                                            }
                                        });
                                    }

                                }
                            }
                        },
                        {
                            field: 'affiliation_id', title: 'Affiliation', width: '20%', align: 'center',
                            formatter: function (value, row) {
                                return row.affiliation_name;
                            },
                            editor: {
                                type: 'combobox',
                                options: {
                                    panelWidth: 200,
                                    mode: 'remote',
                                    valueField: 'id',
                                    textField: 'affiliation_name',
                                    fitColumns: true,
                                    loader: function (param, success, error) {
                                        var url = $.helper.resolveApi("~/Core/Affiliation/lookup");
                                        var filters = " r.affiliation_name like @0 ";
                                        var parameters = [];
                                        if (param.q !== undefined) parameters.push('%' + param.q + '%');
                                        else parameters.push('%%');
                                        $.ajax({
                                            type: 'GET',
                                            url: url,
                                            data: {
                                                length: 100,
                                                filters: "( " + filters + " )",
                                                parameters: parameters.toString()  // search term
                                            },
                                            dataType: 'json',
                                            success: function (data) {

                                                success(data.items);
                                            },
                                            error: function () {
                                                error.apply(this, arguments);
                                            }
                                        });
                                    }
                                }
                            }
                        }
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

        function endEditing() {
            if (editIndex === undefined) { return true }
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
            var account = dgdetail.datagrid('getEditor', {
                index: index,
                field: 'account_id'
            });

            var company = dgdetail.datagrid('getEditor', {
                index: index,
                field: 'company_id'
            });

            var business_unit = dgdetail.datagrid('getEditor', {
                index: index,
                field: 'business_unit_id'
            });

            row.account_name = $(account.target).combobox('getText');
            row.company_name = $(company.target).combobox('getText');
            row.unit_name = $(business_unit.target).combotree('getText');

            footerCalculate();
        }

        function onClickCell(index, field) {
            if (editIndex !== index) {
                
                if (endEditing()) {
                    dgdetail.datagrid('selectRow', index)
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
            if (editIndex === undefined) { return }

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
            accept();
            if (_formFinancialTransaction.valid()) {
                var financialTransaction = _formFinancialTransaction.serializeToJSON();
                console.log(financialTransaction);
                var detailTransactions = dgdetail.datagrid('getData');
                var data = {
                    transaction: financialTransaction,
                    details: detailTransactions.rows
                };

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
                            transactionDetails.reload();
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
            accept();
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
                + "&Title=Adjustment Journal Entry&Ref=" + $('input[name="transaction_number"]').val() + "&url=~/Finance/AdjustmentJournalEntry/Detail/?id=" + _detailID.val());
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

