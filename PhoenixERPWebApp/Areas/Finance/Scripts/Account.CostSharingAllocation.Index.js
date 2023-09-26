(function ($) {
    'use strict';
    var dt_basic;
    var ListChecked = [];
    var ListNotChecked = [];


    var pageFunction = function () {
        var LoadDataTable = function () {
            // begin first table
            var url = $.helper.resolveApi('~/finance/CostSharingAllocation/listdatatables');
            var e = $("#dt_sharing_allocation");
            dt_basic = e.cmDataTable(url, {
                goTopage: false,
                pageLength: 5,
                isDomDefault: false,
                stateSaveParams: function (d) {
                    $("#form-search-dt_sharing_allocation .searchbox").val(d.search.search);
                },
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
                    { data: "cost_sharing_allocation_name" },
                    {
                        data: "start_date",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var dateFormat = moment(data, "YYYY-MM-DD").format("DD-MMMM-YYYY");
                                return dateFormat;
                            }
                            return data;
                        }
                    },
                    
                    {
                        data: "end_date",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var dateFormat = moment(data, "YYYY-MM-DD").format("DD-MMMM-YYYY");
                                return dateFormat;
                            }
                            return data;
                        }
                    }
                ]
            }, function (e) {
                var $table = e; // table selector 
                //BEGIN Select All row dt_basic
                $('#dt_sharing_allocation input[name=select_all]').on('click', function () {
                    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
                    $.each($tdCheckbox, function (i, d) {
                        $(this).trigger("click");
                    });
                    $tdCheckbox.prop('checked', this.checked);

                });

                //ROW CLICK
                //$(document).on('click', '#dt_sharing_allocation tbody tr .edit-row', function () {
                //    var recordId = $(this).attr('data-value');
                //    $('.modal-container').load($.helper.resolve("~/Finance/CostSharingAllocation/Detail/?id=" + recordId), function (result) {
                //        $('#m_modal_detail').modal({ show: true });
                //    });
                //});

                $(document).on('click', '#dt_sharing_allocation tbody tr input[type="checkbox"]', function () {
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
                    $('#dt_sharing_allocation input[name=select_all]').prop('checked', $tdCheckboxChecked.length === $tdCheckbox.length);
                });
            }),

                $("#form-search-dt_sharing_allocation").submit(function (e) {
                    e.preventDefault();
                    var v = $(this).find('.searchbox').val();
                    dt_basic.search(v).draw();
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
                        url: $.helper.resolveApi("~/finance/CostSharingAllocation/delete"),
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

        $('#btn-add').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            $('.modal-container').load($.helper.resolve("~/Finance/CostSharingAllocation/Detail"), function (result) {
                $('#m_modal_detail').modal({ show: true });
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            });
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


