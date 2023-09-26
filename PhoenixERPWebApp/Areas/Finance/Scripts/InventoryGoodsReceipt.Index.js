(function ($) {
    'use strict';
    var dt_basic;
    var gridUI = $('#grid');
    var ListChecked = [];
    var ListNotChecked = [];


    var pageFunction = function () {

        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/InventoryGoodsReceipt/kendoGrid')
            },
            navigatable: true,
            //height: 550,
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
                        for (var i = 0; i < response.data.length; i++) {
                            var dateNoTime = new Date(response.data[i].transaction_datetime);
                            response.data[i].transaction_datetime = new Date(
                                dateNoTime.getFullYear(),
                                dateNoTime.getMonth(),
                                dateNoTime.getDate());
                        }
                        return response;
                    },
                    model: {
                        id: "id",
                        fields: {
                            transaction_datetime: { type: "date" }
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
            //detailTemplate: kendo.template($("#grid_template").html()),
            //detailInit: detailInit,
            dataBound: function (e) {
                buildMenu();
                $.helper.kendoUI.resizeGrid();
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
                    field: "goods_receipt_number",
                    title: "Good Receipt Number",
                    width: "150px",
                    searchable: true
                },
                {
                    field: "organization_name",
                    title: "Unit Name",
                    width: "150px",
                    searchable: true
                },
                {
                    field: "legal_entity_name",
                    title: "Legal Entity",
                    width: "150px",
                    searchable: true
                },
                {
                    field: "vendor_name",
                    title: "Vendor",
                    width: "150px",
                    searchable: true
                },
                {
                    field: "created_on",
                    title: "Date",
                    width: "150px",
                    template: "#= kendo.toString(new Date(created_on), 'dd-MM-yyyy') #"
                    //groupHeaderTemplate: "#= kendo.toString(value, 'dd-MM-yyyy') #"
                },
                {
                    field: "total_amount",
                    title: "Total Amount",
                    width: "150px"
                },
                {
                    field: "remarks",
                    title: "Remarks",
                    width: "150px"
                }
            ]
        });

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
                            { text: "Create RFQ" }
                        ],
                        menu: false
                    }],

                select: function (e) {
                    var operation = $(e.item).text();
                    // TODO how to get the current row data here?
                    var Uid = e.sender.element.attr('data-uid');
                    var dataSource = gridUI.data("kendoGrid").dataSource;
                    var item = dataSource.getByUid(Uid);

                    switch (operation) {
                        case "Edit":
                            window.location.href = $.helper.resolve("~/Finance/InventoryGoodsReceipt/Detail/?id=" + item.id);
                            break;
                        case "Delete":
                            if (item.id) DeleteData([item.id]);

                            break;
                        case "Create RFQ":
                            var template = kendo.template($("#create_rfq_template").html());
                            ListCheckedGrid = [];
                            kendoUIWindow.kendoWindow({
                                width: "615px",

                                title: "Create Request For Quotation",
                                visible: false,
                                actions: [
                                    "Maximize",
                                    "Close"
                                ],
                                open: function () { onOpen(item) },
                                close: onClose,
                                //restore: function (e) {
                                //    console.log(e);
                                //    var dialog = kendoUIWindow.data("kendoWindow");
                                //    dialog.center();
                                //}
                            }).data("kendoWindow").content(template(item)).center().open();

                            function onClose() {
                                undo.fadeIn();
                            }
                            break;
                    }
                }
            });
        }


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
        function reloadGrid() {
            gridUI.data("kendoGrid").dataSource.read();
            gridUI.data('kendoGrid').refresh();
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
                        url: $.helper.resolveApi("~/Finance/FinancialTransaction/delete"),
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                }).then(function () {
                                    ListChecked = [];
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
                        mApp.unblock("#m_blockui_list");
                    }, 2e3));
                }


            });
        };

        //BEGIN::EVENT BUTTON
        $('#btn-delete').click(function () {
            console.log(ListChecked);
            //return;

            if (ListChecked.length > 0)
                DeleteData(ListChecked);
        });
        //END::EVENT BUTTON


        //BEGIN::EVENT BUTTON

        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));