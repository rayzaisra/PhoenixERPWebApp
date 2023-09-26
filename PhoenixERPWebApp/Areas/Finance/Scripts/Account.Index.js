
(function ($) {
    'use strict';

    var url = '';
    var dg = $('#dt_basic_eui');
    var legalEntityId = $('#legal_entity_id').val();

    var pageFunction = function () {

        var $legalEntityLookup = $("#legal_entity_id");

        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name like @0 ',
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        }).on('change', function () {
            legalEntityId = $legalEntityLookup.val();
            console.log(legalEntityId);
            LoadDataTable();
        });

        var url
        var LoadDataTable = function () {
            // begin first table
            if (legalEntityId == null) {
                console.log('null ih');
                url = $.helper.resolveApi('~/finance/Account/dgtable');
            } else {
                url = $.helper.resolveApi('~/finance/Account/dgtable?legal_entity_id=' + legalEntityId);
            }
            
            var dg_options = {
                //toolbar: '#tb',
                url: url,
                idField: 'id',
                generalFilterColumns: [
                    {
                        name: 'account_number',
                        filterable: true
                    },
                    {
                        name: 'account_name',
                        filterable: true
                    },
                    {
                        name: 'account_group_name',
                        filterable: true
                    }],
                generalSearch: $('#dt_basic_eui-search'),
                orders: [{ sortName: 'account_number', sortOrder: 'asc' }],
                view: groupview,
                groupField: 'account_group_name',
                groupFormatter: function (value, rows) {
                    return value;
                },
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'account_number', title: 'Account Number', width: 100, sortable: true },
                    { field: 'account_name', title: 'Account Name', width: 100, sortable: true },
                    { field: 'account_level', title: 'Level', align: 'center', width: 23, sortable: true },
                    {
                        field: 'id', title: 'Actions', align: 'center', width: 23,
                        formatter: function (value, row, index) {
                            return '<a href="' + $.helper.resolve("~/Finance/Account/Detail/?id=" + row.id) + '"><i class="flaticon-edit"></i></a>';
                        }
                    }

                ]],
                renderRow: function (target, fields, frozen, rowIndex, rowData) {
                    console.log('render row');
                }
            };

            dg.cmDataGrid(dg_options);

            $("#form-search-dt_basic_eui").submit(function (e) {
                e.preventDefault();
                dg.datagrid('reload');
            });
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

        $('#btn-delete').click(function () {
            var ids = [];
            var rows = dg.datagrid('getSelections');
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            if (ids.length > 0)
                DeleteData(ids);

        });

        $('#btn-export').click(function () {
            if (legalEntityId !== null && legalEntityId !== '') {
                url = $.helper.resolveApi('~/finance/Account/Download');
                url += '?LegalEntityId=' + encodeURIComponent(legalEntityId)
                    + '&FileFormat=xlsx';
                window.location = url;
            }
        });

        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
                LoadDataTable();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));