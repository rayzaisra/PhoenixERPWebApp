(function ($) {
    'use strict';
    var gridUI = $('#grid');
    var ListChecked = [];
    
    var pageFunction = function () {
        $.helper.kendoUI.grid(gridUI, {
            options: {
                url: $.helper.resolveApi('~/finance/PurchaseBidding/kendoGrid')
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
                            var dateNoTime = new Date(response.data[i].bidding_date);
                            response.data[i].bidding_date = new Date(
                                dateNoTime.getFullYear(),
                                dateNoTime.getMonth(),
                                dateNoTime.getDate());
                        }
                        return response;
                    },
                    model: {
                        id: "id"
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
            dataBound: function (e) {
                buildMenu();
                $.helper.kendoUI.resizeGrid();
            },
            columns: [
                {
                    title: "Actions", width: "40px",
                    attributes: {
                        style: "text-align:center;overflow: visible;"
                    },
                    template: "<ul class='action-menu' data-uid='#= uid #' data-id='#= data.id #'></ul>"
                },
                {
                    field: "request_for_quotation_number",
                    title: "RFQ Number",
                    width: "250px"
                    
                },
                {
                    field: "bidding_date",
                    title: "Date",
                    width: "250px",
                    template: "#= kendo.toString(bidding_date, 'MMM-dd-yyyy') #",
                    groupHeaderTemplate: "#= kendo.toString(value, 'MMMM, dd-yyyy') #"
                }
            ]
        });

        function buildMenu() {

            $(".action-menu").kendoMenu({
                dataSource: [
                    {
                        text: "Edit"
                    }],

                select: function (e) {
                    var operation = $(e.item).text();
                    // TODO how to get the current row data here?
                    var Uid = e.sender.element.attr('data-uid');
                    var dataSource = gridUI.data("kendoGrid").dataSource;
                    var item = dataSource.getByUid(Uid);

                    switch (operation) {
                        case "Edit":
                            window.location.href = $.helper.resolve("~/Finance/Bidding/vendorSelection/?id=" + item.id);
                            break;
                    }
                }
            });
        }

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