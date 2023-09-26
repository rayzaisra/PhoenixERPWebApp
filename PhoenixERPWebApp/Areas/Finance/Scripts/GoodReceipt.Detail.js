(function ($) {
    'use strict';

    var _detailID = $('#detail-id');
    var _gridDetail = $('#gridDetail');
    var ListChecked = [];
    var ListNotChecked = [];

    var pageFunction = function () {
        var _formGoodReceipt = $('#form-goodreceipt');
        var $po_number = $('#po_number');
        var $legalEntityLookup = $("#legal_entity_id");
        var $affiliationLookup = $("#affiliation_id");
        var $cityLookup = $("#city_id");
        var $companyLookup = $("#company_id");
        var $currencyDropdown = $("#currency_id");
        var $approvalLookup = $("#approverId");
        var $requestTypeLookup = $("#request_type_id");
        var $purchaseOrderLookup = $("#purchase_order_id");
        
        //Begin::Grid
        $.helper.kendoUI.grid($("#gridDetail"), {
            options: {
                url: $.helper.resolveApi('~/finance/GoodReceipt/kendoGrid')
            },
            navigatable: true,
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 100]
            },
            filterable: true,
            resizable: true,
            dataSource: {
                pageSize: 10,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                schema: {
                    parse: function (response) {
                        //for (var i = 0; i < response.data.length; i++) {
                        //    var dateNoTime = new Date(response.data[i].transaction_datetime);
                        //    response.data[i].transaction_datetime = new Date(
                        //        dateNoTime.getFullYear(),
                        //        dateNoTime.getMonth(),
                        //        dateNoTime.getDate());
                        //}
                        //return response;
                    },
                    model: {
                        id: "id",
                        fields: {
                            //transaction_datetime: { type: "date" }
                        }
                    }
                }
            },
            schema: {
                model: {
                    id: "id"
                }
            },
            marvelCheckboxSelectable: {
                enable: true,
                listCheckedTemp: ListChecked
            },
            detailTemplate: kendo.template($("#grid_template").html()),
            detailInit: detailInit,
            dataBound: function (e) {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
                $.helper.kendoUI.resizeGrid();
            },
            columns: [
                {
                    field: "code",
                    title: "Code",
                    width: "100px"
                },
                {
                    field: "type",
                    title: "Type",
                    width: "100px"
                },
                {
                    field: "item_name",
                    title: "Item Name",
                    width: "250px"
                },
                {
                    field: "delivery_time",
                    title: "Delivery Time",
                    width: "100px"
                },
                {
                    field: "qty_po",
                    title: "Qty PO",
                    width: "80px"
                },
                {
                    field: "qty_received",
                    title: "Qty Received",
                    width: "80px"
                },
                {
                    field: "qty_remaining",
                    title: "Qty Remaining",
                    width: "80px"
                },
                {
                    field: "qty_count_in",
                    title: "Qty Count In",
                    width: "80px"
                },
                {
                    field: "attachment",
                    title: "Attachment",
                    width: "100px"
                },
                {
                    field: "remark",
                    title: "Remark",
                    width: "150px"
                }
            ]
        });

        function detailInit(e) {
            var detailRow = e.detailRow;

            var approvalIds = [e.data.approval_id];
            //detailRow.find(".status-approval").cmSetClassApprovalStatus(approvalIds);
            var mApproval;
            mApproval = new buildViewer(detailRow.find(".status-approval"), {
                approvalId: e.data.approval_id,
                onApprove: function (approvalId, e) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: $.helper.resolveApi("~/core/Approval/SetApprovalStatus/" + approvalId + "?ApprovalStatus=APPROVED"),
                        success: function (result) {
                            e.refresh();
                        },
                        error: function () {
                            alert('err');
                        }
                    });
                },
                onReject: function (approvalId, e) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: $.helper.resolveApi("~/core/Approval/SetApprovalStatus/" + approvalId + "?ApprovalStatus=REJECTED"),
                        success: function (result) {
                            e.refresh();
                        },
                        error: function () {
                            alert('err');
                        }
                    });
                }
            });

            //console.log(mApproval);
        }
        //End::Grid

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
        $po_number.cmSelect2({
            url: $.helper.resolveApi("~/Finance/PurchaseOrder/lookup"),
            filters: ' r.purchase_order_number LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'purchase_order_number'
            }
        }).on('change', function (e) {
            $('input[name="purchase_order_number"]').val($(this).text());
        });

        $requestTypeLookup.cmSelect2({
            url: $.helper.resolveApi("~/Finance/BankPaymentMethod/lookup"),
            filters: ' r.method_name LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'method_name'
            }
        });
        $cityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/City/lookup"),
            filters: ' r.city_name LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'city_name'
            }
        });

        $companyLookup.cmSelect2({
            url: $.helper.resolveApi("~/Finance/CompanyProfile/lookup"),
            filters: ' r.company_name LIKE @0 and r.is_vendor = 1',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'company_name',
                tax_number: 'tax_registration_number'
            }
        }).on('select2:select', function (e) {
            var data = e.params.data;
            $('input[name="company_name"]').val(data.text);
            $('input[name="tax_registration_number"]').val(data.tax_number);
            $purchaseOrderLookup.cmSelect2({
                url: $.helper.resolveApi("~/Finance/PurchaseOrder/lookup"),
                filters: ' ( r.purchase_order_number LIKE @0 AND r.company_id = @1 ) ',
                parameters: [$(this).val()],
                result: {
                    id: 'id',
                    text: 'purchase_order_number'
                }
            });
        });

        $purchaseOrderLookup.cmSelect2({
            url: $.helper.resolveApi("~/Finance/PurchaseOrder/lookup"),
            filters: ' r.purchase_order_number LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'purchase_order_number'
            }
        });

        $requestTypeLookup.cmSelect2({
            url: $.helper.resolveApi("~/Finance/BankPaymentMethod/lookup"),
            filters: ' r.method_name LIKE @0',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'method_name'
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

        $("#invoice_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });
        $("#invoice_received_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });
        $("#reference_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        $("#tax_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });
        //BEGIN::Form Validation
        _formGoodReceipt.validate({
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
                mUtil.scrollTo(_formGoodReceipt, -200);
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
                        url: $.helper.resolveApi("~/finance/InvoicePayable/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                console.log(record);
                                $('input[name="invoice_number"]').val(record.invoice_number);
                                $('input[name="invoice_date"]').val(record.invoice_date);
                                $('input[name="received_date"]').val(record.received_date);
                                $('input[name="description"]').val(record.description);
                                $('input[name="tax_registration_number"]').val(record.tax_registration_number);
                                $('input[name="purchase_order_date"]').val(record.purchase_order_date);
                                $('input[name="delivery_date"]').val(record.delivery_date);
                                $('input[name="purchase_order_number"]').val(record.purchase_order_number);
                                $('input[id="business_unit_id"]').val(record.unit_name);
                                $('input[id="company_reference"]').val(record.company_reference);
                                $('input[id="contact_person"]').val(record.contact_person);
                                $('input[id="contact_number"]').val(record.contact_number);
                                $('input[name="reference_date"]').val(record.reference_date);
                                $('input[id="exchange_rate"]').val(record.exchange_rate);
                                $('input[id="total_amount"]').val(record.total_amount);
                                $('input[id="tax_number"]').val(record.tax_number);
                                $('input[name="tax_date"]').val(record.tax_date);
                                $('input[name="business_unit_id"]').val(record.business_unit_id);
                                $('input[id="business_unit_id"]').val(record.business_unit_name);
                                $('input[name="currency_id"]').val(record.currency_id);
                                $companyLookup.cmSetLookup(record.company_id, record.company_name);
                                $purchaseOrderLookup.cmSetLookup(record.purchase_order_id, record.purchase_order_number);
                                $currencyDropdown.val(record.currency_id);
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

        var setFormControl = function (isSubmitted) {
            if (!isSubmitted) {
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


        $('#btn-save').click(function () {
            var btn = $(this);
            if (_formGoodReceipt.valid()) {
                var data = _formGoodReceipt.serializeToJSON();

                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/InvoicePayable/savetodraft"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            $('input[name="invoice_number"]').val(r.data.invoice_number);

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
            if (_formGoodReceipt.valid()) {
                var invoicePayable = _formGoodReceipt.serializeToJSON();
                var approverId = $('#approverId').val();
                var data = {
                    approverId: approverId,
                    invoice_payable: invoicePayable
                };
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/Finance/InvoicePayable/Submit"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            btn.removeClass('m-loader m-loader--right m-loader--light');
                            setFormControl(true);
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

        //BEGIN::EVENT BUTTON
        $('#btn-generate_invoice_no').click(function () {
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: $.helper.resolveApi("~/finance/InvoicePayable/nextautonumber"),
                success: function (r) {
                    var record = r.data;
                    $('input[id="invoice_number"]').val(record);
                },
                error: function (e, t, s) {
                    swal(
                        'Information',
                        'Ooops, something went wrong !',
                        'info'
                    );
                }
            });
        });
        //END::EVENT BUTTON

        return {
            init: function () {
                //initialize
                dgOptions(),
                    loadDetailData();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));