(function ($) {
    'use strict';
    var dt_basic;
    var ListChecked = [];
    var ListNotChecked = [];


    var pageFunction = function () {
        var LoadDataTable = function () {
            // begin first table
            var url = $.helper.resolveApi('~/core/Warehouse/listdatatables');
            var e = $("#dt_basic");

            dt_basic = e.cmDataTable2({
                dom: "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                stateSaveParams: function (settings, data) {
                    $("#form-search-dt_basic .searchbox").val(data.search.search);
                },
                pageLength: 10,
                ajax: {
                    url: url,
                    type: "POST",
                    data: function (d) {
                        return d;
                    }
                },
                order: [[2, "asc"]],
                columns: [
                    {
                        data: "id",
                        orderable: false,
                        searchable: true,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    if (ListChecked.indexOf(data) > -1) {
                                        output = `
                                            <label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand">
                                                <input type="checkbox" class="checkboxes m-checkable" id="row-` + data + `" value="` + data + `" checked=true'>
                                                <span></span>
                                            </label>`;
                                    }
                                    else {
                                        output = `
                                            <label class="m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand">
                                                <input type="checkbox" class="checkboxes m-checkable" id="row-` + data + `" value="` + data + `"'>
                                                <span></span>
                                            </label>`;
                                    }
                                }
                                return output;
                            }
                            return data;
                        }
                    },
                    { data: "warehouse_name" },
                    { data: "address" },
                    { data: "phone" },
                    {
                        data: "id",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                return `
                                    <span class="dropdown">
                                        <a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">
                                            <i class="la la-ellipsis-h"></i>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-right">
                                            <a class="dropdown-item" href="`+ $.helper.resolve("~/Core/Warehouse/Detail/?id=" + row.id) + `"><i class="la la-edit"></i> Edit Details</a>
                                            <a class="dropdown-item row-deleted" data-id="`+ row.id +`" href="javascript:void(0);" ><i class="la la-trash"></i> Delete </a>
                                        </div>
                                    </span>`;
                            }
                            return data;
                        }
                    }
                ],
                drawCallback: function (o) {
                    $('.dataTables_paginate > .pagination').addClass('pagination-sm');
                }
            }, function (e, settings, json) {

                var $table = e; // table selector 
                //BEGIN Select All row dt_basic_user_role
                $('#dt_basic input[name=select_all]').on('click', function () {
                    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
                    $.each($tdCheckbox, function (i, d) {
                        $(this).trigger("click");
                    });
                    $tdCheckbox.prop('checked', this.checked);

                });

                //ROW CLICK
                $(document).on('click', '#dt_basic tbody tr .row-deleted', function () {
                    var ArrID = [];
                    ArrID.push($(this).attr('data-id'));
                    DeleteData(ArrID);
                });
                $(document).on('click', '#dt_basic tbody tr input[type="checkbox"]', function () {
                    var CheckID = $(this).val();
                    var removeItem;
                    if ($(this).is(":checked")) {
                        if (ListChecked.indexOf(CheckID) < 0) {
                            ListChecked.push(CheckID);
                            removeItem = ListNotChecked.indexOf(CheckID);
                            ListNotChecked.splice(removeItem, 1);
                        }
                    }
                    else {
                        if (ListNotChecked.indexOf(CheckID) < 0) {
                            ListNotChecked.push(CheckID);
                            removeItem = ListChecked.indexOf(CheckID);
                            ListChecked.splice(removeItem, 1);
                        }
                    }
                    //clear check all
                    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body                    
                    var $tdCheckboxChecked = $table.find('tbody input:checkbox:checked');//Collect all checked checkboxes from tbody tag
                    //if length of already checked checkboxes inside tbody tag is the same as all tbody checkboxes length, then set property of main checkbox to "true", else set to "false"
                    $('#dt_basic input[name=select_all]').prop('checked', $tdCheckboxChecked.length === $tdCheckbox.length);
                });

                $("#form-search-dt_basic").submit(function (e) {
                    e.preventDefault();
                    var v = $(this).find('.searchbox').val();
                    dt_basic.search(v).draw();
                });
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
                        url: $.helper.resolveApi("~/core/Warehouse/delete"),
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
                                    dt_basic.ajax.reload();
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
                LoadDataTable();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


