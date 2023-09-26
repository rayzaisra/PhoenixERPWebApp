function calculatePerPrice(dataItem) {
    var price = ((dataItem.unit_price - dataItem.discount_amount) * dataItem.exchange_rate) ;
    return price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0;
}

(function ($) {
    'use strict';
    var _detailID = $('#detail-id');
    var ListChecked = [];
    var ListItemSelected = [];

    var pageFunction = function () {
        var template = kendo.template($('#detail-information-template').html());

        var loadDetail = function () {
            if (_detailID.val() !== '') {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/finance/PurchaseBidding/" + _detailID.val() + "/details"),
                    success: function (r) {
                        if (r.status.success && r.data) {
                            
                            var record = r.data;
                            $('input[name="bidding_number"]').val(record.bidding_number);
                            $('input[name="bidding_date"]').val(record.bidding_date);
                            getVendorQuotation(record.rfq_id);
                        }
                    },
                    error: function () {
                        alert('err');
                    }
                });
            }

            var getVendorQuotation = function (rfqId) {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/finance/VendorQuotation/getlistdetails?rfq_id=" + rfqId),
                    success: function (r) {
                        if (r.status.success && r.data) {
                            var record = r.data;
                            $.ajax({
                                type: "GET",
                                dataType: 'json',
                                url: $.helper.resolveApi("~/finance/PurchaseBidding/getItemApproved?id=" + _detailID.val()),
                                async: false,
                                success: function (r) {

                                    if (r.status.success && r.data) {
                                        $.each(r.data, function (i, v) {
                                            ListChecked.push(v.vendor_quotation_detail_id);
                                            ListItemSelected.push(v.rfq_detail_id);
                                        });

                                    }
                                }
                            });

                            $('#detail-information').html('');
                            $.each(record, function (i, v) {
                                var h = v.vendorQuotation;
                                var d = v.details;
                                renderTemplate(h, d);
                            });
                        }
                    },
                    error: function () {
                        alert('err');
                    }
                });
            }

            var renderTemplate = function (header, detail) {
                debugger;                
                var temp = template(header);
                $('#detail-information').append(temp);

                $('#detail-information').find('div[data-id=' + header.id + '][class=grid-item]').kendoGrid({
                    dataSource: {
                        data: detail,
                        schema: {
                            model: {
                                id: "id"
                            }
                        },
                        pageSize: 5
                    },
                    height: 200,
                    pageable: true,
                    persistSelection: true,
                    dataBound: function (e) {
                        var grid = e.sender;
                        var items = grid.items();
                        var itemsToSelect = [];
                        items.each(function (idx, row) {
                            var dataItem = grid.dataItem(row);
                            if (ListChecked.indexOf(dataItem["id"]) >= 0) {
                                itemsToSelect.push(row);
                            }
                        });
                        e.sender.select(itemsToSelect);
                    },
                    change: function (e, arg) {
                        var grid = e.sender;
                        var items = grid.items();
                        items.each(function (idx, row) {
                            var vendorQuotationDetailId = grid.dataItem(row).get('id');
                            var itemId = grid.dataItem(row).get('rfq_detail_id');
                            var isChecked = row.className.indexOf("k-state-selected") >= 0;

                            if (isChecked) {
                                if (ListItemSelected.indexOf(itemId) >= 0 && ListChecked.indexOf(vendorQuotationDetailId) == -1) {

                                    if ($(row).hasClass("k-state-selected")) {
                                        var selected = grid.select();
                                        selected = $.grep(selected, function (x) {
                                            var itemToRemove = grid.dataItem(row);
                                            var currentItem = grid.dataItem(x);
                                            return itemToRemove.rfq_detail_id != currentItem.rfq_detail_id;
                                        })
                                        grid.clearSelection();
                                        grid.select(selected);
                                        alert('item has selected');
                                    }
                                }

                                if (ListItemSelected.indexOf(itemId) < 0 && ListChecked.indexOf(vendorQuotationDetailId) < 0) {
                                    ListItemSelected.push(itemId);
                                    ListChecked.push(vendorQuotationDetailId);
                                }
                            }
                            else {
                                if (ListItemSelected.indexOf(itemId) >= 0 && ListChecked.indexOf(vendorQuotationDetailId) >= 0) {
                                    ListItemSelected.splice(ListItemSelected.indexOf(itemId), 1);
                                    ListChecked.splice(ListChecked.indexOf(vendorQuotationDetailId), 1);
                                }
                            }

                        });
                    },
                    columns: [
                        { selectable: true, width: "50px" },
                        {
                            field: "item_code",
                            title: "Item Code",
                            width: "150px"
                        },
                        {
                            field: "item_name",
                            title: "Item Name",
                            width: "150px"
                        },
                        {
                            field: "qty",
                            title: "QTY",
                            width: "80px",
                            filterable: false
                        },
                        {
                            field: "unit_price",
                            title: "Price Per Quantity",
                            template: "<b> Rp. #= calculatePerPrice(data) #</b>",
                            width: "150px",
                            filterable: false
                        }
                    ]
                });
            }

        }



        var previewItemSelection = function () {
            var allVendor = $('#detail-information').find('div.grid-item');
            var selected = [];
            allVendor.each(function (i, e) {
                var grid = $(e).data("kendoGrid");
                //get all selected rows (those which have the checkbox checked)  
                grid.select().each(function () {
                    //push the dataItem into the array
                    selected.push(grid.dataItem(this));
                });
            });

            console.log(kendo.stringify(selected));
        }

        $('#btn-save').on('click', function () {
            var btn = $(this);
            if (ListChecked.length > 0) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/PurchaseBidding/save?biddingId=" + _detailID.val()),
                    data: JSON.stringify(ListChecked),
                    success: function (r) {
                        if (r && r.status.success) {
                            toastr.success("Item has been updated", "Information");
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
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

        $('#btn-create-po').on('click', function () {
            var btn = $(this);

            if (ListChecked.length > 0) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                debugger;
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/finance/vendorquotation/createpofrombidding/?purchasebiddingid=" + _detailID.val()),
                    data: JSON.stringify(ListChecked),
                    success: function (r) {
                        if (r && r.status.success) {
                            toastr.success("Successfully created Purchase Order ", "Information");

                            loadDetail();
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
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
            else {
                swal(
                    'Validation',
                    'Please check at least one goods from vendors.',
                    'info'
                );
            }
        });

        return {
            init: function () {
                //initialize
                loadDetail();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));