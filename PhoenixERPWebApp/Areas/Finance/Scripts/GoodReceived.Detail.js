(function ($) {
    'use strict';

    var _detailID = $('#detail-id');
    var dgdetail = $('#dgdetail');
    var pageFunction = function () {
        var _formGoodReceived = $('#form-GoodReceived');
        var $legalEntityLookup = $("#legal_entity_id");
        var $affiliationLookup = $("#affiliation_id");
        var $companyLookup = $("#company_id");
        var $receiptByLookup = $("#receipt_by");
        var $approvalLookup = $("#approverId");
        var $requestTypeLookup = $("#request_type_id");
        var $purchaseOrderNumber = $("#purchase_order_number");
        var $purchaseOrderStatusLookup = $("#purchase_order_status");

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
        $receiptByLookup.select2({
            enable: false,
            width: "auto",
            theme: "bootstrap",
            placeholder: "Receipt By",
            ajax: {
                url: $.helper.resolveApi("~/core/User/lookup"),
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var filters = " r.app_fullname LIKE @0 ";
                    var parameters = [];
                    if (params.term == undefined) {
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
                    console.log(data);
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
        $purchaseOrderStatusLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/PurchaseOrderStatus/lookup"),
            filters: ' r.status_name LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'status_name'
            }
        });
        $companyLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/CompanyProfile/lookup"),
            filters: ' r.company_name LIKE @0 and r.is_vendor = 1',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'company_name'
            }
        }).on('change', function (e) {
            $('input[name="company_name"]').val($(this).text());
            });
        $purchaseOrderNumber.cmSelect2({
            url: $.helper.resolveApi("~/finance/PurchaseOrder/lookup"),
            filters: ' r.purchase_order_number LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'purchase_order_number'
            }
        })
        $requestTypeLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/BankPaymentMethod/lookup"),
            filters: ' r.method_name LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'method_name'
            }
        })

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
                    if (params.term == undefined) {
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
                    console.log(data);
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

        $affiliationLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/Affiliation/lookup"),
            filters: ' r.affiliation_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'affiliation_name',
            }
        });
        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'legal_entity_name',
            }
        });
        //END: Lookup

        //BEGIN::Form Validation
        _formGoodReceived.validate({
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
                mUtil.scrollTo(_formGoodReceived, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

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
                        url: $.helper.resolveApi("~/finance/GoodReceived/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data.purchase_order;
                                var approverId = r.data.approverId;
                                setLookup_Approver(approverId);
                                $('input[name="business_unit_id"]').val(record.business_unit_id);
                                $('input[name="company_reference"]').val(record.company_reference);
                                $('input[name="currency_id"]').val(record.currency_code);
                                $('input[name="description"]').val(record.description);
                                $('input[name="exchange_rate"]').val(record.exchange_rate);
                                $('input[name="purchase_order_date"]').val(record.purchase_order_date);
                                $('input[name="purchase_order_number"]').val(record.purchase_order_number);
                                $('input[id="business_unit_id"]').val(record.unit_name);
                                $('input[id="discount_percent"]').val(record.discount_percent);
                                $('input[id="discount_amount"]').val(record.discount_amount);
                                $('input[id="amount"]').val(record.amount);
                                console.log(record);
                                $affiliationLookup.cmSetLookup(record.affiliation_id, record.affiliation_name);
                                $companyLookup.cmSetLookup(record.company_id, record.company_name);
                                $legalEntityLookup.cmSetLookup(record.legal_entity_id, record.legal_entity_name);
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

        //BEGIN::Purchase Order Detail
        var goodReceivedDetail = function () {
            var Geturl = function () {
                return $.helper.resolveApi('~/finance/GoodReceivedDetail/dgtable?purchaseOrderid=' + _detailID.val());
            };

            //async=false
            var dgOptions = function () {

                var options = {
                    toolbar: '#tb',
                    iconCls: 'icon-edit',
                    singleSelect: true,
                    toolbar: '#tb',
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

                    columns: [[
                        {
                            field: 'id', title: 'ID', width: 0, align: 'center',
                            formatter: function (value, row) {
                                return row.id;
                            }
                        }, {
                            field: 'item_id', title: 'Code', width: '10%', align: 'center',
                            formatter: function (value, row) {
                                return row.item_name;
                            },
                            editor: {
                                type: 'combogrid',
                                options: {
                                    panelWidth: 500,
                                    mode: 'remote',
                                    idField: 'id',
                                    textField: 'item_name',
                                    fitColumns: true,
                                    loader: function (param, success, error) {
                                        var url = $.helper.resolveApi("~/core/Item/lookup");
                                        var filters = " r.item_name like @0";
                                        var parameters = [];
                                        if (param.q != undefined) parameters.push('%' + param.q + '%');
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
                                        { field: 'item_name', title: 'Item Name', width: 200 },
                                        { field: 'item_code', title: 'Item Code', width: 100 },
                                    ]]
                                }
                            }
                        },
                        { field: 'type', title: 'Type', width: '20%', align: 'center', editor: { type: 'textbox', options: { precision: 1 } } },
                        { field: 'item_name', title: 'Item Name', width: '20%', align: 'center', editor: { type: 'textbox', options: { precision: 1 } } },
                        { field: 'delivery_time', title: 'Delivery Time', width: '20%', align: 'center', editor: { type: 'numberbox', options: { precision: 1 } } },
                        { field: 'qty_po', title: 'Qty PO', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 1 } } },
                        { field: 'qty_received', title: 'Qty Received', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 1 } } },
                        { field: 'qty_remaining', title: 'Qty Remaining', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 1 } } },
                        { field: 'qty_count_in', title: 'Qty Count In', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 1 } } },
                        { field: 'attachment', title: 'Attachment', width: '10%', align: 'center', editor: { type: 'numberbox', options: { precision: 1 } } },
                        { field: 'remarks', title: 'Remarks', width: '10%', align: 'center', editor: { type: 'textbox', options: { precision: 1 } } }
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
            if (editIndex == undefined) { return true }
            if (dgdetail.datagrid('validateRow', editIndex)) {
                dgdetail.datagrid('endEdit', editIndex);
                editIndex = undefined;
                return true;
            } else {
                return false;
            }
        }


        function footerCalculate() {
            var totalDebit = parseInt(0);
            var totalCredit = parseInt(0);
            var rows = dgdetail.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                totalDebit += parseInt(rows[i].debit || 0);
                totalCredit += parseInt(rows[i].credit || 0);
            }
            dgdetail.datagrid('reloadFooter', [{ account_name: 'Total', debit: totalDebit, credit: totalCredit }]);
        }

        function onEndEdit(index, row) {
            var affiliation = $(this).datagrid('getEditor', {
                index: index,
                field: 'affiliation_id'
            });
            var item = $(this).datagrid('getEditor', {
                index: index,
                field: 'item_id'
            });
            var company = $(this).datagrid('getEditor', {
                index: index,
                field: 'company_id'
            });

            var uom = $(this).datagrid('getEditor', {
                index: index,
                field: 'uom_id'
            });
            var business_unit = $(this).datagrid('getEditor', {
                index: index,
                field: 'business_unit_id'
            });

            var legal_entity = $(this).datagrid('getEditor', {
                index: index,
                field: 'legal_entity_id'
            });

            row.item_name = $(item.target).combobox('getText');
            row.uom_name = $(uom.target).combobox('getText');
            row.affiliation_name = $(affiliation.target).combobox('getText');
            row.company_name = $(company.target).combobox('getText');
            row.unit_name = $(business_unit.target).combotree('getText');
            row.legal_entity_name = $(legal_entity.target).combobox('getText');

            footerCalculate();
        }

        function onClickCell(index, field) {
            if (editIndex != index) {
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
            if (editIndex == undefined) { return }

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
            if (_formGoodReceived.valid()) {
                var purchaseOrder = _formGoodReceived.serializeToJSON();
                var detailTransactions = dgdetail.datagrid('getData');
                var approverId = $('#approverId').val();
                var data = {
                    approverId: approverId,
                    purchase_order: purchaseOrder,
                    purchase_order_details: detailTransactions.rows
                };

                //Function Accept
                accept();
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/GoodReceived/savetodraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="purchase_order_number"]').val(r.data.purchase_order_number);
                            goodReceivedDetail.reload();
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
            if (_formGoodReceived.valid()) {
                var purchaseOrder = _formGoodReceived.serializeToJSON();
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
                    url: $.helper.resolveApi("~/finance/PurchaseOrder/submited"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                            formControll(true);
                            goodReceivedDetail.reload();
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

        //BEGIN::EVENT BUTTON
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
        //END::EVENT BUTTON

        return {
            init: function () {
                //initialize
                loadDetailData(),
                    dgOptions(),
                    goodReceivedDetail.init();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));