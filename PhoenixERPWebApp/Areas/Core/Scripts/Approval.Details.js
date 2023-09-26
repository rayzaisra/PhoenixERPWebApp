(function ($) {
    'use strict';
    var dt_basic = $('#dt_basic');
    var ListChecked = [];
    var ListNotChecked = [];

    var $entityId = $('input[name="entity_id"]');
    var $title = $('input[name="title"]');

    var approvalEntityRegistered = [
        {
            Title: 'Journal Entry',
            EntityId: '1a60d708-94e2-80e8-eabe-3e74b9145849',
            drawCallback: function () {
                var api = this.api();
                var json = api.ajax.json();
                var $table = dt_basic; // table selector 

                var approvalIds = [];
                $.each(json.data, function (i, e) {
                    var approvalId = e.id;
                    if (approvalId !== null) {
                        approvalIds.push(approvalId);
                    }
                });
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/Approval/GetApprovalStatusGroupLevel"),
                    data: JSON.stringify(approvalIds),
                    success: function (r) {
                        if (r.status.success) {
                            //note div class = .build-view
                            //attribute : row-id=row.id
                            //attribute : record-id=row.approval_id

                            $table.find('.build-view').each(function (e, v) {
                                var rowId = $(this).attr('row-id');
                                var recordId = $(this).attr('record-id');
                                var transactionNumber = $(this).attr('transaction-number');
                                var transactionDatetime = $(this).attr('transaction-datetime');

                                //moment(d.transactionNumber).format('DD-MMM-YYYY')
                                var output = '';
                                $.each(r.data, function (i, d) {
                                    if (d.approval_id == rowId) {
                                        output =
                                            ` <span class="m-timeline-2__item-time">` + transactionNumber + `</br>` + moment(transactionDatetime).format('DD-MMM-YYYY')+`  </span>
														<div class="m-timeline-2__item-cricle">
															<i class="fa fa-genderless m--font-danger"></i>
														</div>
														<div class="m-timeline-2__item-text  m--padding-top-5">`;
											
                                        $.each(d.data, function (iDetail, dData) {
                                            output += `<div class="m-list-timeline__item">
                                                            <span class="m-list-timeline__badge"></span>
                                                            <span class="m-list-timeline__text">`;
                                            output += `<div class="col-lg-12">`;
                                            output += `<div class="row">`;

                                            if (dData.details.length > 1) {
                                                $.each(dData.details, function (i, data) {
                                                    if (data.status_name == "REQUESTED") {
                                                        output += ` <div class="col-lg-6">`;
                                                        output += ` ` + data.app_fullname + ` </br>`;
                                                        output += ` <div class="btn-group m-btn-group m-btn-group--pill" role="group" aria-label="...">
											                     <button type="button" class="m-btn btn btn-sm btn-primary">Approve</button>
											                     <button type="button" class="m-btn btn btn-sm btn-warning">Reject</button>`;
                                                        output += ` </div>`;
                                                        output += ` </div>`;
                                                    } else {
                                                        output += ` <div class="col-lg-6">`;
                                                        output += ` ` + data.app_fullname + ` </br>`;
                                                        output += `<span class="m-badge m-badge--success m-badge--wide">` + data.status_name + `</span>`;
                                                        output += ` </div>`;
                                                    }

                                                    
                                                });
                                            } else if (dData.details.length == 1) {

                                                if (dData.details[0].status_name == "REQUESTED") {
                                                    output += ` <div class="col-lg-12">`;
                                                    output += ` ` + dData.details[0].app_fullname + ` </br>`;
                                                    output += ` <div class="btn-group m-btn-group m-btn-group--pill" role="group" aria-label="...">
											                     <button type="button" class="m-btn btn btn-sm btn-primary">Approve</button>
											                     <button type="button" class="m-btn btn btn-sm btn-warning">Reject</button>`;
                                                    output += ` </div>`;

                                                    output += ` </div>`;
                                                } else {
                                                    output += ` <div class="col-lg-12">`;
                                                    output += ` ` + dData.details[0].app_fullname + ` </br>`;
                                                    output += `<span class="m-badge m-badge--success m-badge--wide">` + dData.details[0].status_name + `</span>`;
                                                    output += ` </div>`;
                                                }
                                                
                                            }
                                            output += `</div>`;
                                            output += `</div>`;
                                            output += `</span></div> `;
                                        });

                                        output += `</div>`;
                                    }
                                });

                                $(this).html(output);
                            });

                        }
                    },
                    error: function (e, t, s) {

                    }
                });



            },
            initComplete: function (settings, json) {
                var $table = $(this); // table selector 
            }
        } 
    ];

    var pageFunction = function () {

        var LoadDataTable = function (drawCallback, initComplete) {
            // begin first table
            var url = $.helper.resolveApi('~/Core/Approval/Requested/listdatatables?entity_id=' + $entityId.val());
            dt_basic.cmDataTable2({
                dom: "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                stateSaveParams: function (settings, data) {
                    //$("#form-search-dt_basic .searchbox").val(data.search.search);
                },
                pageLength: 5,
                ajax: {
                    url: url,
                    type: "POST",
                    data: function (d) {
                        return d;
                    }
                },
                order: [[0, "asc"]],
                columns: [
                    {
                        data: "record_id",
                        orderable: true,
                        searchable: true,
                        class:"m-timeline-2__item",
                        render: function (data, type, row) {
                            if (type === 'display') {
                                return `<div class="build-view" row-id="` + row.id + `" record-id="` + row.record_id + `" transaction-datetime="` + row.transaction_datetime + `" transaction-number="` + row.transaction_number +`" ></div>`;
                            }
                            return data;
                        }
                    }
                ],
                drawCallback: drawCallback,
                initComplete: initComplete
            });


        };

        //BEGIN::EVENT BUTTON


        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
                

                $.each(approvalEntityRegistered, function (i, o) {
                    if (o.Title == $title.val() && o.EntityId == $entityId.val()) {
                        LoadDataTable(o.drawCallback,o.initComplete);
                    }
                });


            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


