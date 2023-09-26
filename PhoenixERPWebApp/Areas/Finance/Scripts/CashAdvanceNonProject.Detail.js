function calculateAmount(dataItem) {
    dataItem.amount = dataItem.qty * dataItem.unit_price;
    return dataItem.amount;
}
(function ($) {
    'use strict';

    var pageFunction = function () {
        var _detailId = $('#detail-id');
        var gridUI = $('#grid');

        var _formCashAdvance = $('#form-CashAdvanceNonProject');

        var $businessUnitLookup = $("#business_unit_id");
        var $legalEntityLookup = $("#legal_entity_id");
        var $approvalLookup = $("#approval_id");
        var $bankLookup = $("#payee_bank_id");
        var $employeeLookup = $("#employee_id");

        //BEGIN::Lookup
        $bankLookup.cmSelect2({
            url: $.helper.resolveApi("~/finance/Bank/lookup"),
            filters: ' r.bank_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'bank_name'
            }
        });

        $employeeLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/User/lookup"),
            filters: ' r.app_fullname LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'app_fullname'
            }
        });

        $businessUnitLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/BusinessUnit/lookup"),
            filters: ' r.unit_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'unit_name'
            }
        });

        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        });

        //END: Lookup
        $("#requested_on").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $("#ca_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $("#ca_period_from").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $("#ca_period_to").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        //BEGIN::Form Validation
        _formCashAdvance.validate({
            rules: {

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
                mUtil.scrollTo(_formCashAdvance, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation


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

        var editIndex = undefined;
        //BEGIN::Load Datatable
        var loadDetailData = function () {

            if (_detailId.val() !== '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Processing..."
                }),
                    $.ajax({
                        type: "GET",
                        dataType: 'json',
                    url: $.helper.resolveApi("~/Finance/CashAdvance/" + _detailId.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data
                                var approverId = record.approval_id;
                                console.log(approverId)
                                $('input[name="approval_id"]').val(approverId);
                                $('input[name="ca_number"]').val(record.ca_number);
                                $('input[name="ca_date"]').val(record.ca_date);
                                $('input[name="revision"]').val(record.revision);
                                $('input[name="requested_on"]').val(record.requested_on);
                                $('input[name="client_name"]').val(record.client_name);
                                $employeeLookup.cmSetLookup(record.requested_by, record.requester_name);
                                $businessUnitLookup.cmSetLookup(record.business_unit_id, record.business_unit_name);
                                $legalEntityLookup.cmSetLookup(record.legal_entity_id, record.legal_entity_name);
                                $bankLookup.cmSetLookup(record.payee_bank_id, record.payee_bank_name);
                                $('input[name="payee_account_number"]').val(record.payee_account_number);
                                $('input[name="payee_account_name"]').val(record.payee_account_name);
                                $('input[name="payee_bank_branch"]').val(record.payee_bank_branch);
                                $('input[name="ca_period_from"]').val(record.ca_period_from);
                                $('input[name="ca_period_to"]').val(record.ca_period_to);
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
        //END::Load Datatable

        var taskDataSource = new kendo.data.DataSource({
            type: "json",
            transport: {
                read: {
                    url: $.helper.resolveApi("~/ProjectManagement/Task/List"),
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
                data: "rows",
                total: "total"
            },
        });

        taskDataSource.read();

        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/CashAdvanceDetail/KendoGrid?cashAdvanceid=' + _detailId.val())
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
                        id: { editable: true, nullable: true }
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

                $(footerAmount).html(TotalAmount);
                $.helper.kendoUI.resizeGrid();
            },
            toolbar: ["create", "cancel", "Reload"],
            columns: [
                {
                    field: "task_detail",
                    title: "Task Detail",
                    width: "250px",
                    //locked: true,
                    //lockable: false,
                    groupable: false,
                    editor: taskEditor
                },
                {
                    field: "qty",
                    title: "Qty",
                    attributes: {
                        style: "text-align:right;"
                    },
                    
                    width: "250px",
                },
                {
                    field: "unit_price",
                    title: "Unit Price",
                    attributes: {
                        style: "text-align:right;"
                    },
                    footerTemplate: "Total",
                    width: "250px",
                },
                {
                    field: "amount",
                    title: "Amount",
                    type: "Number",
                    editable: function () { return false },
                    attributes: {
                        style: "text-align:right;"
                    },
                    template: "<b>#= calculateAmount(data) #</b>",
                    width: "150px",
                    footerTemplate: "<span class='amount'></span>",
                },
                {
                    command: [
                        {
                            name: "Undo", click: function (e) {
                                e.preventDefault();
                                var row = e.target.closest('tr');
                                var uid = $(row).data(uid);
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);
                                dataSource.cancelChanges(item);
                                this.refresh()
                            }
                        },
                        {
                            name: "rowDelete", text: "Delete", click: function (e) {
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

        function taskEditor(container, options) {
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoMultiColumnComboBox({
                    autoBind:false,
                    dataTextField: "task_name",
                    dataValueField: "task_name", 
                    value: options.model.task_id,
                    text: options.model.task_name,
                    height: 550,
                    columns: [
                        { field: "id", title: "Task Id", width: 200 },
                        { field: "task_name", title: "Task Name", width: 200 }
                    ],
                    dataSource: taskDataSource,
                    change: function (e) {
                        console.log(e);
                        var equipmentModel = this.dataItem(e.item);
                        console.warn(equipmentModel);
                        console.log(options.model);
                        console.log("task id update : " + equipmentModel.id);
                        options.model.task_id = equipmentModel.id;
                        options.model.set("task_detail", equipmentModel.task_name);
                        //options.model.Unit = equipmentModel.DefaultUnit;
                        //options.model.Price = equipmentModel.DefaultPrice;
                        //alert(equipmentModel.DefaultUnit);
                    }
                });
            var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
            tooltipElement.appendTo(container);
        }

        var formControll = function () {
            $('#btn-save').removeAttr('disabled').show();

            if (_detailId.val() !== '') {
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
        }


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
            var item = $(this).datagrid('getEditor', {
                index: index,
                field: 'item_id'
            });
            var item_code = $(this).datagrid('getEditor', {
                index: index,
                field: 'item_code'
            });
            var uom = $(this).datagrid('getEditor', {
                index: index,
                field: 'uom_id'
            });
            row.item_code = $(item_code.target).combobox('getText');
            row.item_name = $(item.target).combobox('getText');
            row.uom_name = $(uom.target).combobox('getText');
            row.line_total = row['unit_price'] * row['qty'];
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
            if (_formCashAdvance.valid()) {
                //accept();
                var cashAdvance = _formCashAdvance.serializeToJSON();
                var detailTransactions = $("#grid").data("kendoGrid");
                var approverId = $('#approval_id').val();
                var data = {
                    cashAdvance: cashAdvance,
                    cashAdvanceDetails: detailTransactions.dataSource.view().toJSON()
                    //approverIds: approverId$
                };

                //Function Accept
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/CashAdvance/AddCashAdvanceWithApprovals"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailId.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="ca_number"]').val(r.data.ca_number);
                            reloadGrid();
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
                    url: $.helper.resolveApi("~/finance/CashAdvance/submited?RecordId=" + _detailId.val()),
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
                        url: $.helper.resolveApi("~/core/Approval/removeApprovalEntity/" + $('#entity_id').val() + "/?RecordID=" + _detailId.val() + "&ApprovalID=" + $('input[name="approval_id"]').val()),
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

        $('#btn-approval').click(function () {
            var url = $.helper.resolve("~/Core/ApprovalTemplate/SetupApproval/?EntityId=" + $('#entity_id').val() + "&RecordID=" + _detailId.val()
                + "&Title=Cash Advance Non Project&Ref=" + $('input[name="ca_number"]').val() + "&url=~/Finance/CashAdvanceNonProject/Detail/?id=" + _detailId.val());
            window.location.href = url;
        });
        //END::EVENT BUTTON

        return {
            init: function () {
                //initialize
                loadDetailData()
                    //cashAdvanceDetails.init();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));