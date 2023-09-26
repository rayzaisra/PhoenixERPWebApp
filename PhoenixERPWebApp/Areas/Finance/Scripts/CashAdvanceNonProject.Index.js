function detailCashAdvance(e) {

    var detailRow = e.detailRow;
    detailRow.find(".tabstrip").kendoTabStrip({
        animation: {
            open: { effects: "fadeIn" }
        }
    });
    var approvalIds = [e.data.approval_id];
    console.log(approvalIds);
    $.helper.kendoUI.grid(detailRow.find(".ca_detail"),
        {
            options: {
                url: $.helper.resolveApi('~/finance/CashAdvanceDetail/KendoGrid?cashAdvanceid=' + e.data.id)
            },
            schema: {
                model: {
                    id: "id"
                }
            },
            marvelCheckboxSelectable: {
                enable: false
            },
            dataSource: {
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 5
            },
            scrollable: false,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: false,
                input: true
            },
            columns: [
                { field: "task_id", title: "Task ID", width: "110px" },
                { field: "task_detail", title: "Task Detail", width: "110px" },
                { field: "qty", title: "Qty", width: "40px" },
                { field: "unit_price", title: "Unit Price", width: "100px", template: "#= kendo.toString(unit_price, 'c0') #" },
                { field: "amount", title: "Amount", width: "100px" }
            ]
        });

    if (e.data.approval_id !==null && e.data.is_locked && e.data.is_active) {
        detailRow.find(".status-approval").cmSetClassApprovalStatus(approvalIds);
    } else {
        detailRow.find(".status-approval").html('<span class="m-badge m-badge--warning m-badge--wide">Draft</span>');
    }
};



(function ($) {
    'use strict';
    var dt_basic;
    var ListChecked = [];

    var pageFunction = function () {
        var gridUI = $('#grid');

        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/CashAdvance/KendoGrid')
            },
            //height: 550,           
            dataSource: {
                pageSize: 10,
                serverPaging: true,
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
                this.expandRow(this.tbody.find("tr.k-master-row").first());
            },
            pageable: {
                refresh: true,
                pageSizes:false,
                input: true
            },
            columns: [
                {
                    field: "ca_number",
                    title: "CA Number",
                    width: "120px"

                },
                {
                    field: "ca_period_from",
                    title: "CA Date",
                    width: "120px",
                    template: "#= kendo.toString(kendo.parseDate(ca_period_from, 'yyyy-MM-dd'), 'MMM-dd-yyyy') #"
                },
                {
                    field: "ca_period_to",
                    title: "Due Date",
                    width: "120px",
                    template: "#= kendo.toString(kendo.parseDate(ca_period_from, 'yyyy-MM-dd'), 'MMM-dd-yyyy') #"
                },
                {
                    title: "Actions",
                    command: [
                        {
                            name: "Edit",
                            click: function (e) {
                                console.log(e);
                                e.preventDefault();
                                var row = e.target.closest('tr');
                                var uid = $(row).data(uid);
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);
                                window.location.href = $.helper.resolve("~/Finance/CashAdvanceNonProject/Detail/?id=" + item.id);
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
            ]
        });

        gridUI.data('kendoGrid').setOptions({
            detailTemplate: kendo.template($("#detail_template").html()),
            detailInit: detailCashAdvance
        });

        function reloadGrid() {
            gridUI.data("kendoGrid").dataSource.read();
            gridUI.data('kendoGrid').refresh();
        }

        gridUI.find(".k-grid-toolbar").on("click", ".k-grid-Reload", function (e) {
            e.preventDefault();
            reloadGrid();
        });

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
                        url: $.helper.resolveApi("~/finance/CashAdvance/delete"),
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                }).then(function () {
                                    ListChecked = [];
                                    ListNotChecked = [];
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
            if (ListChecked.length > 0) {
                DeleteData(ListChecked);
                reloadGrid();
            }


        });

        $('#btn-add').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            $('.modal-container').load($.helper.resolve("~/Finance/CashAdvance/Detail"), function (result) {
                $('#m_modal_detail').modal({ show: true });
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            });
        });

        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
                //LoadDataTable();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


