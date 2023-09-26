
(function ($) {
    'use strict';
    
    var dg = $('#dt_basic_eui');

    var pageFunction = function () {
        $("#transaction_date").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            pickerPosition: "bottom-left",
            format: "yyyy/mm/dd"
        });

        var LoadDataTable = function (_dateTime, transactionNumber, account) {
            // begin first table
            ///api/report/finance/GeneralLedger/dgtable
            var url = $.helper.resolveApi('~/report/finance/GeneralLedger/dgtable?dateTime=' + _dateTime +
                '&transactionNumber=' + transactionNumber + '&account=' + account);
            var dg_options = {
                //toolbar: '#tb',
                url: url,
                idField: 'id',
                //generalFilterColumns: [
                //    {
                //        name: 'account_number',
                //        filterable: true
                //    },
                //    {
                //        name: 'account_name',
                //        filterable: true
                //    },
                //    {
                //        name: 'account_group_name',
                //        filterable: true
                //    }],
                generalSearch: $('#dt_basic_eui-search'),
                orders: [{ sortName: 'account_number', sortOrder: 'asc' }],
                view: groupview,
                groupField: 'account_number',
                groupFormatter: function (value, rows) {
                    return value;
                },
                columns: [[
                    { field: 'account_name', title: 'Account Name', width: 100, sortable: true },
                    { field: 'debit', title: 'Debit', align: 'center', width: 200, sortable: true },
                    { field: 'credit', title: 'Credit', align: 'center', width: 200, sortable: true }
                ]],
                renderRow: function (target, fields, frozen, rowIndex, rowData) {
                    console.log('render row');
                }
            };

            dg.cmDataGrid(dg_options);
        };
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
                        url: $.helper.resolveApi("~/finance/Account/delete"),
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Deleted!',
                                    text: r.status.message,
                                    type: "success"
                                }).then(function () {
                                    dg.datagrid('reload');
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
        $('#btn-view').click(function () {
            var params = $('#form-search').serializeToJSON();
            console.log(params);
            LoadDataTable(params.transaction_date, params.transaction_number, params.account);
        });
        
        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
            }
        };
    }();



    $(document).ready(function () {
        //pageFunction.init();
    });
}(jQuery));


