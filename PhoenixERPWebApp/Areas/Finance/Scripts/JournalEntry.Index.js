(function ($) {
    'use strict';
    var dt_basic;
    var gridUI = $('#grid');
    var ListChecked = [];
    var ListNotChecked = [];


    var pageFunction = function () {

        $.helper.kendoUI.grid($("#grid"), {
            options: {
                url: $.helper.resolveApi('~/finance/FinancialTransaction/kendoGrid')
            },
            navigatable: true,
            //height: 550,
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 100]
            },
            filterable:true,
            resizable: true,
            dataSource: {
                pageSize: 10,
                serverPaging: true,
                serverSorting: true,
                serverFiltering:true,
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
            detailTemplate: kendo.template($("#grid_template").html()),
            detailInit: detailInit,
            dataBound: function (e) {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
                $.helper.kendoUI.resizeGrid();
            },
            columns: [
                {
                    command: [
                        {
                            name: "Edit", text: "Edit", click: function (e) {
                                e.preventDefault();
                                var row = e.target.closest('tr')
                                var uid = $(row).data(uid)
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);
                                window.location.href = $.helper.resolve("~/Finance/JournalEntry/Detail/?id=" + item.id);
                            }
                        },
                        {
                            name: "rowDelete", text: "Delete", click: function (e) {
                                e.preventDefault();
                                var row = e.target.closest('tr')
                                var uid = $(row).data(uid);
                                var dataSource = gridUI.data("kendoGrid").dataSource;
                                var item = dataSource.getByUid(uid.uid);
                                alert(item.id);
                                if (item.id) DeleteData([item.id]);
                                else {
                                    dataSource.cancelChanges(item);
                                }
                            }
                        }
                    ], title: "&nbsp;", width: "200px"
                },
                {
                    field: "transaction_number",
                    title: "Transaction Number",
                    width: "150px",
                    searchable: true
                },
                {
                    field: "transaction_datetime",
                    title: "Date",
                    width: "250px",
                    template: "#= kendo.toString(transaction_datetime, 'MMM-dd-yyyy') #",
                    groupHeaderTemplate: "#= kendo.toString(value, 'MMMM, dd-yyyy') #"
                },
                {
                    field: "legal_entity_name",
                    title: "Legal Entity",
                    width: "150px",
                    searchable:true
                },
                {
                    field: "company_name",
                    title: "Company",
                    width: "150px"
                },
                {
                    field: "current_company_name",
                    title: "Current Company",
                    width: "150px"
                },
                {
                    field: "unit_name",
                    title: "Business Unit",
                    width: "150px"
                },
                {
                    field: "affiliation_name",
                    title: "Affiliation",
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
                onApprove: function (approvalId,e) {
                   
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
                onReject: function (approvalId,e) {
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
            return;
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