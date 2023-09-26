(function ($) {
    'use strict';
    var ListChecked = [];
    var url = '';
    var pageFunction = function () {

        var gridUI = $('#grid');
        var griduidialog = $('#dialog_grid');

        function loadDataGrid() {
            $.helper.kendoUI.grid($("#grid"), {
                options: {
                    url: $.helper.resolveApi('~/finance/FixedAsset/KendoGrid')
                },
                //height: 550,
                filterable: false,
                dataSource: {
                    pageSize: 10,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true
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
                marvelCheckboxSelectable: {
                    enable: true,
                    ListCheckedTemp: ListChecked
                },
                dataBound: function (e) {
                    buildMenu();
                    var data = e.sender.dataSource.view().toJSON();
                    $.each(data, function (i, v) {
                        var approval = $('span.approval[data-id="' + v.id + '"]');

                        if ((v.is_locked && v.is_active) || v.approval_id != null) {
                            var mApproval = new buildViewer(approval, {
                                approvalId: v.approval_id,
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
                            $(approval).html('<span class="m-badge m-badge--warning m-badge--wide">Draft</span>');
                        }
                    });

                    $.helper.kendoUI.resizeGrid();
                },
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    input: true
                },
                columns: [
                    {
                        title: "Actions", width: "230px",
                        attributes: {
                            style: "text-align:center;overflow: visible;"
                        },
                        template: "<ul class='action-menu' data-uid='#= uid #' data-id='#= data.id #'></ul>"
                    },
                    {
                        field: "id",
                        title: "Fixed Asset ID",
                        width: "150px",
                        searchable: true
                    },
                    {
                        field: "purchase_order_number",
                        title: "PO Number",
                        width: "150px"
                    },
                    //{
                    //    field: "division",
                    //    title: "Division",
                    //    width: "150px",
                    //    searchable: true
                    //},
                    //{
                    //    field: "unit",
                    //    title: "Unit",
                    //    width: "70px",
                    //    searchable: true
                    //},
                    //{
                    //    field: "classification",
                    //    title: "Classification",
                    //    width: "100px",
                    //    searchable: true
                    //},
                    //{
                    //    field: "invoice_no",
                    //    title: "Invoice No",
                    //    width: "100px",
                    //    searchable: true
                    //},
                    {
                        field: "fixed_asset_name",
                        title: "Name",
                        width: "100px",
                        searchable: true
                    },
                    {
                        field: "currency_code",
                        title: "Currency",
                        width: "70px"
                    },
                    {
                        field: "current_location",
                        title: "Place / Location",
                        width: "100px"
                    },
                    {
                        field: "exchange_rate",
                        title: "Exchange",
                        width: "100px"
                    },
                    {
                        field: "price",
                        title: "Price",
                        width: "100px"
                    },
                    {
                        field: "registration_date",
                        title: "Date",
                        width: "150px",
                        template: "#= kendo.toString(registration_date, 'MMM-dd-yyyy') #",
                        groupHeaderTemplate: "#= kendo.toString(value, 'MMMM, dd-yyyy') #"
                    },
                    {
                        field: "economic_period",
                        title: "Economic Period",
                        width: "80px"
                    },
                    {
                        field: "description",
                        title: "Asset Description",
                        width: "250px"
                    }
                ]
            });
        }

        var getMenuItem = function (target, el) {
            var itemIndexes = target.split(/[.,]/),
                item = el;

            if (itemIndexes[0] !== "") {
                for (var i = 0, len = itemIndexes.length; i < len; i++) {
                    item = item.children("li").eq(itemIndexes[i]);
                    if (i < len - 1) {
                        item = item.find("ul:first");
                    }
                }
            }

            return item;
        };

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
                            { text: "Create PO" },
                            { text: "Go to Bidding" }
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
                        var el = menu.element.find('span:contains("Create PO")');
                        menu.remove(el);
                    } else {
                        var el = menu.element.find('span:contains("Go to Bidding")');
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
                            window.location.href = $.helper.resolve("~/Finance/FixedAsset/Detail/?id=" + item.id);
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
                                                url: $.helper.resolveApi("~/finance/FixedAsset/createpo/?FixedAssetid=" + item.id),
                                                success: function (result) {
                                                    if (result.status.success)
                                                        setTimeout(function () {
                                                            debugger;
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
                            debugger;
                            if (item.purchase_bidding_id != null) {
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
            if ((e.data.is_locked && e.data.is_active) || e.data.approval_id != null) {
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
                                    if ($rfqType.val() == "project") {
                                        reloadProjectGrid();
                                    } else {
                                        reloadGrid();
                                    }
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
                loadDataGrid();
                //LoadDataTable();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


