(function ($) {
    'use strict';

    var _detailID = $('#detail-id');

    var dgdetail = $('#dgdetail');
    var financial_transaction_id = $('#financial_transaction_id');
    var legalEntityId = $('#legal_entity_id').val();


    var pageFunction = function () {

        var _formPettyCash = $('#form-PettyCash');

        //BEGIN::Form Validation
        _formPettyCash.validate({
            rules: {
                business_unit_id: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                mUtil.scrollTo(_formPettyCash, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::Dropdown

        var $approvalLookup = $("#approval_id");
        var $legalEntityLookup = $("#legal_entity_id");
        var $affiliationLookup = $("#affiliation_id");
        var $currencyDropdown = $("#currency_id");

        var dropDownCurrency = function () {

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

        //BEGIN::LOOKUP
        $affiliationLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Affiliation/lookup"),
            filters: ' r.affiliation_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'affiliation_name'
            }
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
            pettyCashDetails.reload();
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
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: $.helper.resolveApi("~/core/User/" + Id + "/details"),
                success: function (r) {
                    var record = r.data;
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

        //END::LOOKUP

        $("#settlement_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });


        $("#beginning_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $("#ending_date").datepicker({
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
                        url: $.helper.resolveApi("~/Finance/PettyCash/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                console.log(record);
                                //return;
                                //$('input[name="approval_id"]').val(record.approval_id);
                                //$('input[name="transaction_number"]').val(record.transaction_number);
                                //$('input[name="transaction_datetime"]').val(record.transaction_datetime);
                                //$('input[name="reference"]').val(record.reference);
                                //$('input[name="description"]').val(record.description);

                                //$companyLookup.cmSetLookup(record.company_id, record.company_name);

                                //$('input[name="business_unit_id"]').val(record.business_unit_id);
                                //$('input[id="business_unit_id"]').val(record.unit_name);
                                //$("input[name=is_internal][value='" + record.is_internal + "']").trigger('click');
                                //legalEntityId = record.legal_entity_id;
                                //$legalEntityLookup.cmSetLookup(record.legal_entity_id, record.legal_entity_name);
                                //if (legalEntityId !== '') $legalEntityLookup.attr('disabled', 'disabled');


                                //$affiliateLookup.cmSetLookup(record.affiliation_id, record.affiliation_name);
                                //$transactionType.cmSetLookup(record.transaction_type_id, record.transaction_type);

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

        //BEGIN::Petty Cash Detail
        var pettyCashDetails = function () {
            var Geturl = function () {
                return $.helper.resolveApi('~/finance/PettyCashDetail/dgtable?pettycashid=' + _detailID.val());
            };

            //async=false
            var dgOptions = function () {
                var accountDataLookup = undefined;
                console.warn(legalEntityId)
                $.ajax({
                    url: $.helper.resolveApi("~/finance/Account/lookup?length=1000"),
                    data: {
                        filters: ' ( r.legal_entity_id = @0 AND r.account_level = 5 ) ',
                        parameters: legalEntityId
                    },
                    type: "GET",
                    success: function (response) {
                        accountDataLookup = response;
                        console.log(accountDataLookup);
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
                    onEndEdit: onEndEdit,
                    url: Geturl(),
                    idField: 'id',
                    generalFilterColumns: [
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
                                        console.log(row);
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
                        { field: 'transaction_date', title: 'Date', width: '20%', align: 'center', editor: { type: 'datebox', options: { precision: 1 } } },
                        { field: 'file_name', title: 'Nota', width: '20%', align: 'center', editor: { type: 'filebox', options: { precision: 1 } } },
                        { field: 'reference', title: 'Reference', width: '20%', align: 'center', editor: { type: 'textbox', options: { precision: 1 } } },
                        { field: 'transaction_detail', title: 'Transaction', width: '40%', align: 'center', editor: { type: 'textbox', options: { precision: 1 } } },
                        { field: 'debit', title: 'Debit', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } },
                        { field: 'credit', title: 'Credit', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } }

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
        //END::Load Petty Cash Detail

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
            if (_formPettyCash.valid()) {
                var financialTransaction = _formPettyCash.serializeToJSON();
                var detailTransactions = dgdetail.datagrid('getData');
                var data = {
                    pettyCash: financialTransaction,
                    details: detailTransactions.rows
                };
                console.log(data);

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/PettyCash/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="payment_number"]').val(r.data.payment_number);
                            pettyCashDetails.reload();
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
                dropDownCurrency(),
                loadDetailData(),
                    pettyCashDetails.init();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));