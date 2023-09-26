(function ($) {
    'use strict';
    var ListChecked = [];
    var url = '';
    var pageFunction = function () {
        var $vendorType = $('select#vendor_type');
        var $rfqType = $('#rfqType');
        var gridUI = $('#grid');
        var griduidialog = $('#dialog_grid');

        $vendorType.change(function (e) {
            
            var filters = [];
            var venddor_type = ($(this).val() === "true") ? { field: "is_prefered_vendor", operator: "eq", value: 1 } : { field: "is_prefered_vendor", operator: "eq", value: 0 };
            var rfq_type = ($rfqType.val() === "internal") ? { field: "job_id", operator: "isnull", value: "isnull" } : { field: "job_id", operator: "isnotnull", value:"isnotnull" };
            
            filters.push(venddor_type);
            filters.push(rfq_type);
            loadDataGrid(filters);
        });

        $rfqType.change(function (e) {
            
            var filters = [];
            var venddor_type = ($vendorType.val() === "true") ? { field: "is_prefered_vendor", operator: "eq", value: 1 } : { field: "is_prefered_vendor", operator: "eq", value: 0 };
            var rfq_type = (this.value === "internal") ? { field: "job_id", operator: "isnull", value: "isnull" } : { field: "job_id", operator: "isnotnull", value: "isnotnull" };

            filters.push(venddor_type);
            filters.push(rfq_type);
            loadDataGrid(filters);
        });

        function loadDataGrid(filters = [], isInternal) {
            
            $("#grid").empty();
            var urlApi = $.helper.resolveApi('~/finance/VendorQuotation/KendoGrid');
            $.helper.kendoUI.grid($("#grid"), {
                options: {
                    url: urlApi
                    //url: $.helper.resolveApi('~/finance/VendorQuotation/KendoWithDetailGrid')
                },
                filterable: false,
                dataSource: {
                    pageSize: 10,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    filter: filters
                },
                schema: {
                    model: {
                        id: "id",
                        fields: {
                            id: { editable: true, nullable: true }
                        }
                    },
                    data: "data",
                    total: "recordsTotal"
                },
                pageable: true,
                refresh: true,
                pageSizes: true,
                input: true,
                marvelCheckboxSelectable: {
                    enable: true,
                    ListCheckedTemp: ListChecked
                },
                dataBound: function (e) {
                    buildMenu();

                    //if (!isInternal) {
                    //    $(".k-command-controll-createPO").remove();
                    //}

                    //var data = e.sender.dataSource.view().toJSON();
                    //console.log(data);
                    //$.each(data, function (i, v) {

                    //    if (!v.is_prefered_vendor) {
                    //        $(".k-command-controll-createPO").remove();
                    //    }
                    //    var approval = $('span.approval[data-id="' + v.id + '"]');

                    //    if ((v.is_locked && v.is_active) || v.approval_id != null) {
                    //        var mApproval = new buildViewer(approval, {
                    //            approvalId: v.approval_id,
                    //            onApprove: function (approvalId, e) {

                    //                $.ajax({
                    //                    type: "POST",
                    //                    contentType: "application/json; charset=utf-8",
                    //                    url: $.helper.resolveApi("~/core/Approval/SetApprovalStatus/" + approvalId + "?ApprovalStatus=APPROVED"),
                    //                    success: function (result) {
                    //                        e.refresh();
                    //                    },
                    //                    error: function () {
                    //                        alert('err');
                    //                    }
                    //                });
                    //            },
                    //            onReject: function (approvalId, e) {
                    //                $.ajax({
                    //                    type: "POST",
                    //                    contentType: "application/json; charset=utf-8",
                    //                    url: $.helper.resolveApi("~/core/Approval/SetApprovalStatus/" + approvalId + "?ApprovalStatus=REJECTED"),
                    //                    success: function (result) {
                    //                        e.refresh();
                    //                    },
                    //                    error: function () {
                    //                        alert('err');
                    //                    }
                    //                });
                    //            }
                    //        });
                    //    } else {
                    //        $(approval).html('<span class="m-badge m-badge--warning m-badge--wide">Draft</span>');
                    //    }
                    //});

                    $.helper.kendoUI.resizeGrid();
                },
                columns: [
                    {
                        command: [
                            {
                                name: "Edit", width: "50px", text: "<i class='la la-edit '></i>", click: function (e) {
                                    e.preventDefault();
                                    var row = e.target.closest('tr');
                                    var uid = $(row).data(uid);
                                    var dataSource = gridUI.data("kendoGrid").dataSource;
                                    var item = dataSource.getByUid(uid.uid);
                                    console.log(item);
                                    window.location.href = $.helper.resolve("~/Finance/VendorQuotation/Detail/?id=" + item.id);
                                    //if (item.is_prefered_vendor === true) {
                                    //    window.location.href = $.helper.resolve("~/Finance/VendorQuotation/Detail/?id=" + item.id);
                                    //} else {
                                    //    window.location.href = $.helper.resolve("~/Finance/VendorQuotation/DetailMultiple/?id=" + item.id);
                                    //}

                                }
                            },
                            {
                                name: "rowDelete", width: "50px", text: "<i class='la la-trash'></i>", click: function (e) {
                                    e.preventDefault();
                                    var row = e.target.closest('tr');
                                    var uid = $(row).data(uid);
                                    var dataSource = gridUI.data("kendoGrid").dataSource;
                                    var item = dataSource.getByUid(uid.uid);
                                    alert(item.id);
                                    if (item.id) DeleteData([item.id]);
                                    else {
                                        dataSource.cancelChanges(item);
                                    }
                                }
                            },
                            {
                                name: "createPO", width: "50px", className: "k-command-controll-createPO", text: "<i class='la la-shopping-cart'></i>", click: function (e) {
                                    e.preventDefault();
                                    var row = e.target.closest('tr');
                                    var uid = $(row).data(uid);
                                    var dataSource = gridUI.data("kendoGrid").dataSource;
                                    var dataItem = dataSource.getByUid(uid.uid);
                                    kendo.ui.progress(gridUI, true);
                                    $.ajax({
                                        type: "post",
                                        contenttype: "application/json; charset=utf-8",
                                        url: $.helper.resolveApi("~/finance/vendorquotation/createpo/?vendorquotationid=" + dataItem.id),
                                        success: function (result) {
                                            if (result.status.success)
                                                setTimeout(function () {
                                                    kendo.ui.progress(gridUI, false);
                                                    toastr.success(result.status.message, "information");
                                                    window.location.href = $.helper.resolve("~/finance/purchaseorder/detail/?id=" + result.data.recordID);
                                                }, 3000);
                                            else {
                                                kendo.ui.progress(gridUI, false);
                                                toastr.success(result.status.message, "error");
                                            }
                                        },
                                        error: function (ex) {
                                            console.error(ex);
                                            kendo.ui.progress(gridUI, false);
                                        }
                                    });

                                }
                            }
                        ], title: "&nbsp;", width: "250px"
                    },
                    {
                        field: "request_for_quotation_number",
                        title: "RFQ Number",
                        width: "100px"

                    },
                    {
                        field: "quotation_number",
                        title: "Quotation Number",
                        width: "100px"

                    },
                    {
                        field: "quotation_date",
                        title: "Quotation date",
                        width: "120px",
                        template: "#= kendo.toString(kendo.parseDate(quotation_date, 'yyyy-MM-dd'), 'MMMM-dd-yyyy') #"

                    },
                    {
                        field: "vendor_name",
                        title: "Vendor",
                        width: "120px"
                    },
                    {
                        field: "amount_final",
                        title: "Total Amount",
                        width: "120px",
                        attributes: {
                            style: "text-align:right;"
                        },
                        template: function (dataItem) {
                            return "<b>Rp." + (dataItem.amount_final || 0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,') || 0 + "</b>";
                        },
                    },
                    {
                        field: "delivery_date",
                        title: "Delivery Date",
                        template: "#= kendo.toString(kendo.parseDate(delivery_date, 'yyyy-MM-dd'), 'MMMM-dd-yyyy') #",
                        width: "120px"
                    },
                    {
                        field: "purchase_order_id",
                        title: "PO Status",
                        width: "150px",
                        template: function (dataItem) {
                            var span = "";
                            if (dataItem.purchase_order_id === null) {
                                span = '<span class="m-badge m-badge--warning m-badge--wide">Waiting</span>';
                            } else {
                                if (isInternal)
                                    span = '<a href="' + $.helper.resolve("~/Finance/Bidding/vendorSelection/?id=" + dataItem.purchase_bidding_id + "") + '" class="m-link m-link--state m-link--primary">Complete</a>';
                                else
                                    span = '<a href="' + $.helper.resolve("~/Finance/Bidding/vendorSelection/?id=" + dataItem.purchase_bidding_id + "") + '" class="m-link m-link--state m-link--primary">Go to Bidding</a>';

                            }
                            return span;
                        }
                    },
                    {
                        field: "delivery_address",
                        title: "Ship To",
                        width: "300px"
                    }

                ]
            });

            gridUI.kendoTooltip({
                filter: ".k-grid-createPO",
                content: function (e) {
                    return "Create Purchase Order";
                }
            });
        }

        function buildMenu() {
            $(".action-menu").kendoMenu({
                dataSource: [
                    {
                        text: "Edit"
                    },
                    {
                        text: "Delete"
                    },
                    {
                        text: "Other", // TODO how to get the rows first column value here?
                        items: [
                            { text: "Create PO" }
                        ],
                        menu: false
                    }],
                open: function (e) {

                    var menu = e.sender;
                    var operation = $(e.item).text();
                    // TODO how to get the current row data here?
                    var Uid = e.sender.element.attr('data-uid');
                    var dataSource = gridUI.data("kendoGrid").dataSource;
                    var item = dataSource.getByUid(Uid);

                    if (!item.is_prefered_vendor) {
                        el = menu.element.find('span:contains("Create PO")');
                        menu.remove(el);
                    } else {
                        el = menu.element.find('span:contains("Go to Bidding")');
                        menu.remove(el);
                    }
                },

                select: function (e) {
                    var operation = $(e.item).text();
                    // TODO how to get the current row data here?
                    var Uid = e.sender.element.attr('data-uid');
                    var dataSource = gridUI.data("kendoGrid").dataSource;
                    var item = dataSource.getByUid(Uid);

                    switch (operation) {
                        case "Edit":
                            window.location.href = $.helper.resolve("~/Finance/VendorQuotation/Detail/?id=" + item.id);
                            break;
                        case "Delete":
                            if (item.id) DeleteData([item.id]);
                            break;
                        case "Create PO":
                            griduidialog.kendoDialog({
                                width: "400px",
                                title: "create purchase order",
                                closable: false,
                                modal: false,
                                //content: template(item),
                                content: "<p>create purchase order of rfq number <strong>" + item.request_for_quotation_number + "</strong>?<p>",
                                actions: [
                                    { text: 'close' },
                                    {
                                        text: 'create', primary: true,
                                        action: function (e) {
                                            kendo.ui.progress(griduidialog, true);

                                            $.ajax({
                                                type: "post",
                                                contenttype: "application/json; charset=utf-8",
                                                url: $.helper.resolveApi("~/finance/vendorquotation/createpo/?vendorquotationid=" + item.id),
                                                success: function (result) {
                                                    if (result.status.success)
                                                        setTimeout(function () {
                                                            griduidialog.data("kendoDialog").close();
                                                            toastr.success(result.status.message, "information");
                                                            window.location.href = $.helper.resolve("~/finance/purchaseorder/detail/?id=" + result.data.recordID);
                                                        }, 3000);
                                                    else {
                                                        griduidialog.data("kendoDialog").close();
                                                        toastr.success(result.status.message, "error");
                                                    }
                                                },
                                                error: function () {
                                                    alert('err');
                                                }
                                            });

                                            return false;
                                        }
                                    }
                                ],

                            });
                            griduidialog.data("kendoDialog").open();
                            break;
                        case "Go to Bidding":
                            if (item.purchase_bidding_id !== null) {
                                window.location.href = $.helper.resolve("~/Finance/Bidding/vendorSelection/?id=" + item.purchase_bidding_id + "");
                            } else {
                                alert('Please create bidding first.');
                            }
                            break;
                    }
                }
            });
        }
        // End : Load Datatable

        function reloadGrid() {
            gridUI.data("kendoGrid").dataSource.read();
            gridUI.data('kendoGrid').refresh();
        }

        gridUI.find(".k-grid-toolbar").on("click", ".k-grid-Reload", function (e) {
            e.preventDefault();
            reloadGrid();
        });

        function detailInit(e) {
            var detailRow = e.detailRow;
            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });
            if ((e.data.is_locked && e.data.is_active) || e.data.approval_id !== null) {
                var mApproval = new buildViewer(detailRow.find(".status-approval"), {
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
            } else {
                detailRow.find(".status-approval").html('<span class="m-badge m-badge--warning m-badge--wide">Draft</span>');
            }
        }

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

                        url: url,
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                }).then(function () {
                                    ListChecked = [];
                                    if ($rfqType.val() === "project") {
                                        reloadProjectGrid();
                                    } else {
                                        reloadGrid();
                                    }
                                    location.reload();
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
        //BEGIN::EVENT BUTTON
        $('#btn-delete').click(function () {
            if (ListChecked.length > 0)
                DeleteData(ListChecked);

        });
        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
                //$vendorType.trigger('change');
                $rfqType.trigger('change');
                //loadDataGrid();
                //LoadDataTable();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


