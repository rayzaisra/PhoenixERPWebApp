(function ($) {

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    $.fn.extend(
        {
            cmRoleObj: function (fn) {
                var e = this;
                var dataValue = 0;
                var MAX_ENTITY_COUNT = 10000;
                var ACCESS_NOACCESS = 0;
                var ACCESS_OWNRECORD = 1;
                var ACCESS_BUSINESSUNIT = MAX_ENTITY_COUNT;
                var ACCESS_PARENTCHILD = MAX_ENTITY_COUNT * ACCESS_BUSINESSUNIT;
                var ACCESS_ORGANIZATION = MAX_ENTITY_COUNT * ACCESS_PARENTCHILD;

                switch (e.attr('data-value')) {
                    case "ACCESS_NOACCESS":
                        dataValue = ACCESS_NOACCESS;
                        break;
                    case "ACCESS_OWNRECORD":
                        dataValue = ACCESS_OWNRECORD;
                        break;
                    case "ACCESS_BUSINESSUNIT":
                        dataValue = ACCESS_BUSINESSUNIT;
                        break;
                    case "ACCESS_PARENTCHILD":
                        dataValue = ACCESS_PARENTCHILD;
                        break;
                    case "ACCESS_ORGANIZATION":
                        dataValue = ACCESS_ORGANIZATION;
                        break;
                    default:
                        dataValue = ACCESS_NOACCESS;
                        break;
                }

                var crud_access_options =
                    [
                        { id: ACCESS_NOACCESS, text: 'No Access', html: 'ten' },
                        { id: ACCESS_OWNRECORD, text: 'Own Record', html: 'twentyfive' },
                        { id: ACCESS_BUSINESSUNIT, text: 'Current Organization', html: 'fifty' },
                        { id: ACCESS_PARENTCHILD, text: 'Current & Sub Organization', html: 'seventyfive' },
                        { id: ACCESS_ORGANIZATION, text: 'Full Access', html: 'onehundred' }
                    ];

                var _getNextKey = function (currVal) {
                    var nextIDX = 0;


                    var nRes = false;
                    crud_access_options.map(function (item, index) {
                        if (item.id == currVal) {
                            nRes = true;
                            nextIDX = index + 1;
                        }

                        return item;
                    });

                    if (nRes) {
                        if (!(nextIDX < crud_access_options.length)) {
                            nextIDX = 0;
                        }
                    }


                    return nextIDX;
                }

                if (dataValue > 1) {
                    $(e).each(function () {
                        var m = this;

                        var _init = document.createElement("a");
                        var currVal = dataValue;
                        var nextIDX = _getNextKey(currVal);
                        nextIDX = (nextIDX > 0 ? nextIDX - 1 : nextIDX);

                        var pieID = Math.floor((Math.random() * 100000) + 1);
                        do {
                            if ($('input[data-cmpie="' + pieID + '"]').length > 0) {
                                pieID = Math.floor((Math.random() * 100000) + 1);
                            }
                            else {
                                break;
                            }
                        } while (true)

                        $(m).attr('data-cmpie', pieID);

                        $(_init).attr({
                            "href": "javascript:void(0);",
                            "title": crud_access_options[nextIDX].text,
                            "data-idx": pieID,
                            "data-value": crud_access_options[nextIDX].id
                        }).html('<pie class="' + crud_access_options[nextIDX].html + '"></pie>');

                        $(m).after(_init);
                        $(m).val(crud_access_options[nextIDX].id);
                        $(m).hide();
                    });
                }
                else {
                    if (dataValue > 0) {
                        var m = e;

                        var _init = document.createElement("a");
                        var currVal = dataValue;
                        var nextIDX = _getNextKey(currVal);
                        nextIDX = (nextIDX > 0 ? nextIDX - 1 : nextIDX);

                        var pieID = Math.floor((Math.random() * 100000) + 1);
                        do {
                            if ($('input[data-cmpie="' + pieID + '"]').length > 0) {
                                pieID = Math.floor((Math.random() * 100000) + 1);
                            }
                            else {
                                break;
                            }
                        } while (true)

                        $(m).attr('data-cmpie', pieID);

                        $(_init).attr({
                            "href": "javascript:void(0);",
                            "title": crud_access_options[nextIDX].text,
                            "data-idx": pieID,
                            "data-value": crud_access_options[nextIDX].id
                        }).html('<pie class="' + crud_access_options[nextIDX].html + '"></pie>');

                        $(m).after(_init);
                        $(m).val(crud_access_options[nextIDX].id);

                        $(m).hide();
                    }
                }
            },
            cmRole: function (fn) {
                var e = this;

                var MAX_ENTITY_COUNT = 10000;
                var ACCESS_NOACCESS = 0;
                var ACCESS_OWNRECORD = 1;
                var ACCESS_BUSINESSUNIT = MAX_ENTITY_COUNT;
                var ACCESS_PARENTCHILD = MAX_ENTITY_COUNT * ACCESS_BUSINESSUNIT;
                var ACCESS_ORGANIZATION = MAX_ENTITY_COUNT * ACCESS_PARENTCHILD;
                var crud_access_options =
                    [
                        { id: ACCESS_NOACCESS, text: 'No Access', html: 'ten' },
                        { id: ACCESS_OWNRECORD, text: 'Own Record', html: 'twentyfive' },
                        { id: ACCESS_BUSINESSUNIT, text: 'Current Organization', html: 'fifty' },
                        { id: ACCESS_PARENTCHILD, text: 'Current & Sub Organization', html: 'seventyfive' },
                        { id: ACCESS_ORGANIZATION, text: 'Full Access', html: 'onehundred' }
                    ];
                var _getNextKey = function (currVal) {
                    var nextIDX = 0;

                    if (currVal.trim() != '') {
                        var nRes = false;
                        crud_access_options.map(function (item, index) {
                            if (item.id == currVal) {
                                nRes = true;
                                nextIDX = index + 1;
                            }

                            return item;
                        });
                        if (nRes) {
                            if ((nextIDX > crud_access_options.length)) {
                                nextIDX = 0;
                            }
                        }
                    }

                    return nextIDX;
                }

                if ($(e).length > 1) {
                    $(e).each(function () {
                        var m = this;
                        var _init = document.createElement("a");
                        var currVal = $(m).val();
                        var nextIDX = _getNextKey(currVal);
                        nextIDX = (nextIDX > 0 ? nextIDX - 1 : nextIDX);

                        var pieID = Math.floor((Math.random() * 100000) + 1);
                        do {
                            if ($('input[data-cmpie="' + pieID + '"]').length > 0) {
                                pieID = Math.floor((Math.random() * 100000) + 1);
                            }
                            else {
                                break;
                            }
                        } while (true)

                        $(m).attr('data-cmpie', pieID);
                        $(_init).attr({
                            "href": "javascript:void(0);",
                            "title": crud_access_options[nextIDX].text,
                            "data-idx": pieID,
                            "data-value": crud_access_options[nextIDX].id
                        }).html('<pie class="' + crud_access_options[nextIDX].html + '"></pie>');

                        $(m).after(_init);
                        $(m).val(crud_access_options[nextIDX].id);

                        $(_init).off('click').on('click', function () {
                            var EcurrVal = $(this).attr('data-value');
                            var EpieID = $(this).attr('data-idx');
                            var EnextIDX = _getNextKey(EcurrVal);
                            EnextIDX = (EnextIDX == crud_access_options.length ? 0 : EnextIDX);
                            $(this).attr({
                                "data-value": crud_access_options[EnextIDX].id,
                                "title": crud_access_options[EnextIDX].text,
                            }).find('pie').attr({
                                'class': crud_access_options[EnextIDX].html
                            });

                            $('input[data-cmpie="' + EpieID + '"]').val(crud_access_options[EnextIDX].id);

                            if (fn) {
                                fn(m);
                            }
                        });

                        $(m).hide();
                    });
                }
                else {
                    if ($(e).length > 0) {
                        var m = e;

                        var _init = document.createElement("a");
                        var currVal = $(m).val();
                        var nextIDX = _getNextKey(currVal);
                        nextIDX = (nextIDX > 0 ? nextIDX - 1 : nextIDX);

                        var pieID = Math.floor((Math.random() * 100000) + 1);
                        do {
                            if ($('input[data-cmpie="' + pieID + '"]').length > 0) {
                                pieID = Math.floor((Math.random() * 100000) + 1);
                            }
                            else {
                                break;
                            }
                        } while (true)

                        $(m).attr('data-cmpie', pieID);

                        $(_init).attr({
                            "href": "javascript:void(0);",
                            "title": crud_access_options[nextIDX].text,
                            "data-idx": pieID,
                            "data-value": crud_access_options[nextIDX].id
                        }).html('<pie class="' + crud_access_options[nextIDX].html + '"></pie>');

                        $(m).after(_init);
                        $(m).val(crud_access_options[nextIDX].id);

                        $(_init).off('click').on('click', function () {
                            var EcurrVal = $(this).attr('data-value');
                            var EpieID = $(this).attr('data-idx');
                            var EnextIDX = _getNextKey(EcurrVal);
                            EnextIDX = (EnextIDX == crud_access_options.length ? 0 : EnextIDX);
                            $(this).attr({
                                "data-value": crud_access_options[EnextIDX].id,
                                "title": crud_access_options[EnextIDX].text
                            }).find('pie').attr({
                                'class': crud_access_options[EnextIDX].html
                            });

                            $('input[data-cmpie="' + EpieID + '"]').val(crud_access_options[EnextIDX].id);

                            if (fn) {
                                fn(m);
                            }
                        });

                        $(m).hide();
                    }
                }
            },
            cmSelect2: function (options) {
                var e = this;
                var dt_options = {
                    url: '',
                    filters: '',
                    paramaters: [],
                    result: {
                        id: '',
                        text: ''
                    },
                    options: {}
                };
                dt_options = $.extend(dt_options, options);
                if (options.url != '') {

                    var select2Options = {
                        enable: false,
                        width: "100%",
                        theme: "bootstrap",
                        placeholder: "Choose",
                        ajax: {
                            url: options.url,
                            dataType: 'json',
                            delay: 250,
                            data: function (params) {
                                var filters = options.filters;
                                var parameters = [];

                                if (params.term == undefined) {
                                    parameters.push('%%');
                                }
                                else {
                                    parameters.push('%' + params.term + '%');
                                }

                                if (Array.isArray(options.parameters) && (options.parameters.length > 0)) {

                                    options.parameters.map(function (item, index) {
                                        console.log(item);
                                        parameters.push(item);
                                    });

                                }

                                return {
                                    length: 100,
                                    filters: "( " + filters + " )",
                                    parameters: parameters.toString()  // search term

                                };
                            },
                            processResults: function (data, page) {
                                console.log(data);
                                var result = { results: [], more: false };
                                if (data && data.items) {
                                    result.more = data.totalPages > data.currentPage;
                                    $.each(data.items, function () {
                                        var i = this;
                                        var res = options.result;

                                        var obj = {};
                                        Object.keys(res).map(function (key, index) {
                                            obj[key] = i[res[key]];
                                        });

                                        result.results.push(obj);
                                    });
                                };
                                return result;
                            },
                            cache: true
                        },
                        escapeMarkup: function (markup) {
                            return markup;
                        }, // let our custom formatter work
                        minimumInputLength: 0
                    };
                    select2Options = $.extend(true,select2Options, dt_options.options);

                    if ($(e).length > 1) {
                        $(e).each(function () {
                            var m = this;
                            $(m).select2(select2Options);
                            return m;
                        });
                    }
                    else {
                        $(e).select2(select2Options);
                        return e;
                    }
                }
            },
            cmSetLookup: function (Id, Text) {
                var t = this;
                if (Id == '' || Id == null) return;
                $(t).append($("<option/>", {
                    value: Id,
                    text: Text,
                    selected: true
                }));
                $(t).val(Id);
            },
            cmMetronicDataTable: function (options, initComplete) {
                var e = this;
                var dt_options =
                {
                    data: {
                        type: "remote",
                        method: "POST",
                        source: {
                            read: {
                                    //extend recursive
                            }
                        },
                        pageSize: 10,
                        serverPaging: true,
                        serverFiltering: true,
                        serverSorting: true
                    },
                    layout: {
                        theme: "default",
                        class: "",
                        scroll: true,
                        height: "auto",
                        footer: false
                    },
                    sortable: true,
                    pagination: true,
                    toolbar: {
                        placement: ["bottom"],
                        items: {
                            pagination: {
                                pageSizeSelect: [5, 10, 20, 30, 50]
                            }
                        }
                    }
                }
                var options = $.extend(true,dt_options, options);
                console.log(options);
                return e.mDatatable(dt_options);
            },
            cmDataGrid: function (options) {
                var e = this;
                var dt_options =
                {
                    filterBtnIconCls: 'icon-filter',
                    pagination: true,
                    remoteFilter: true,
                    //url: '',
                    method: 'POST',
                    rownumbers: true,
                    multiSort: true,
                    pageSize: 10,
                    width: "100%",
                    fitColumns: true,
                    //striped: true,
                    generalFilterColumns: [],
                    orders: [],
                    collapsible: true,
                    loader: function (param, success, error) {
                        var opts = $(this).datagrid('options');
                        var filterRules = [];
                        if (!opts.url) return false;
                        if (param.filterRules != undefined) {
                            filterRules = jQuery.parseJSON(param.filterRules);
                        }
                        var paramOption = {
                            pageNumber: opts.pageNumber,
                            pageSize: opts.pageSize ,
                            orders: opts.orders ,
                            GeneralFilterColumns: opts.generalFilterColumns,
                            filterRules: filterRules,
                            Search: opts.generalSearch.val() || ''
                        };
                        console.log(paramOption);
                        if (param.sort != undefined && param.order != undefined) {
                            var col = param.sort.split(",");
                            var order = param.order.split(",");
                            paramOption.orders = [];
                            col.map(function (val, i) {
                                var obj = { "sortName": val, "sortOrder": order[i] };
                                paramOption.orders.push(obj);
                            });
                        }
                        $.ajax({
                            type: opts.method,
                            url: opts.url,
                            data: paramOption,
                            dataType: 'json',
                            success: function (response) {
                                success({
                                    total: response.total,
                                    rows: response.data
                                    //orders: response.orders
                                });
                            },
                            error: function () {
                                console.log('err');
                                error.apply(this, arguments);
                            }
                        });

                    },
                    pageSize: 10,
                    pageList: [10, 20, 30, 40, 50, 100],
                    frozenColumns:[[]],
                    columns: [[]]
                    //onLoadSuccess: function (data) {
                    //    var gcount = $(this).datagrid('options').view.groups.length;
                    //    for (var i = 0; i < gcount; i++) {
                    //        $(this).datagrid('collapseGroup', i);
                    //    }
                    //}
                }
                var options = $.extend(true, dt_options, options);
                console.log(options);
                return e.datagrid(options);
            },
            cmSetTableApprovalStatus: function (approvalIds) {
                var $table = $(this); // table selector 
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/Approval/GetApprovalStatus"),
                    data: JSON.stringify(approvalIds),
                    success: function (r) {
                        if (r.status.success) {
                            //note div class = .status-approval
                            //attribute : row-id=row.id
                            //attribute : approval-id=row.approval_id

                            $table.find('.status-approval').each(function (e, v) {
                                var rowId = $(this).attr('row-id');
                                var approvalId = $(this).attr('approval-id');
                                var output = '';
                                $.each(r.data, function (i, d) {
                                    if (d.approval_id == approvalId) {
                                        output = `<div class="m-list-timeline">
                                                        <div class="m-list-timeline__items"> `;
                                        $.each(d.details, function (iDetail, dDetail) {
                                            output += `
                                                            <div class="m-list-timeline__item">
                                                              <span class="m-list-timeline__badge"></span>
                                                              <span class="m-list-timeline__text">`+ dDetail.app_fullname +
                                                `</br><span class="m-badge m-badge--success m-badge--wide">` + dDetail.status_name +
                                                `</span></span>
                                                            </div> `;
                                        });
                                        output += `</div>
                                                    </div>`;

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
            cmSetClassApprovalStatus: function (approvalIds) {
                var $table = $(this); // table selector 
                //== Main object
                var the = this;
                var init = false;

                //== Get element object
                var element = mUtil.get(elementId);
                var body = mUtil.get('body');

                if (!element) {
                    return;
                }



                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/Approval/GetApprovalStatus"),
                    data: JSON.stringify(approvalIds),
                    success: function (r) {
                        if (r.status.success) {
                            //note div class = .status-approval
                            //attribute : row-id=row.id
                            //attribute : approval-id=row.approval_id

                            var rowId = $(this).attr('row-id');
                            var approvalId = approvalIds[0];
                            var output = '';
                            $.each(r.data, function (i, d) {
                                
                                    output = `<div class="m-list-timeline">
                                                        <div class="m-list-timeline__items"> `;
                                    $.each(d.details, function (iDetail, dDetail) {
                                        output += `
                                                            <div class="m-list-timeline__item">
                                                              <span class="m-list-timeline__badge"></span>
                                                              <span class="m-list-timeline__text">`+ dDetail.app_fullname +
                                            `</br><span class="m-badge m-badge--success m-badge--wide">` + dDetail.status_name +
                                            `</span></span>
                                                            </div> `;
                                    });
                                    output += `</div>
                                                    </div>`;

                               
                            });

                            $table.html(output);
                        }
                    },
                    error: function (e, t, s) {

                    }
                });
            },
            cmDataTable2: function (options, initComplete) {
                var e = this;
                var dt_options = {
                    processing: true,
                    serverSide: true,
                    stateSave: true,
                    searchDelay: 500,
                    responsive: true,
                    dom: "\tf<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                    lengthMenu: [[5, 10, 25, 50, 100], [5, 10, 25, 50, 100]],
                    pageLength: 10,
                    destroy: true,
                    ajax: {
                        url: '',
                        type: "POST",
                        //"contentType": "application/json",
                        data: function (d) {
                            return d;
                        }
                    },
                    multipleSelection: true,
                    order: [[1, "asc"]],
                    language: {
                        aria: {
                            sortAscending: ": activate to sort column ascending",
                            sortDescending: ": activate to sort column descending"
                        },
                        emptyTable: "No data available in table",
                        info: "Showing _START_ to _END_ of _TOTAL_ records",
                        infoEmpty: "No records found",
                        infoFiltered: "(filtered from _MAX_ total records)",
                        lengthMenu: "Show _MENU_",
                        search: "Search:",
                        zeroRecords: "No matching records found"
                    },
                    order: [[1, "asc"]],
                    autoWidth: true,
                    initComplete: function (settings, json) {
                        initComplete(e, settings, json);
                    }
                }

                var options = $.extend(dt_options, options);
                console.log(options);

                return e.DataTable(dt_options);

            },
            cmDataTable: function (url, options, callback) {
                var e = this;
                var dt_options = {
                    "processing": true,
                    "serverSide": true,
                    "stateSave": true,
                    "searchDelay": 500,
                    "scrollY": "50vh",
                    "scrollX": true,
                    "scrollCollapse": true,
                    "stateSaveParams": function (settings, data) {
                        if (options.stateSaveParams != undefined) {
                            options.stateSaveParams(data);
                        }
                    },
                    "responsive": true,
                    //"dom": "\tf<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                    //"dom": "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                    "lengthMenu": [[5, 10, 25, 50, 100], [5, 10, 25, 50, 100]],
                    "pageLength": 10,
                    "destroy": true,
                    "ajax": {
                        "url": url,
                        "type": "POST",
                        //"contentType": "application/json",
                        "data": function (d) {
                            return d;
                        }
                    },
                    "multipleSelection": true,
                    "order": [[1, "asc"]],
                    language: {
                        aria: {
                            sortAscending: ": activate to sort column ascending",
                            sortDescending: ": activate to sort column descending"
                        },
                        emptyTable: "No data available in table",
                        info: "Showing _START_ to _END_ of _TOTAL_ records",
                        infoEmpty: "No records found",
                        infoFiltered: "(filtered from _MAX_ total records)",
                        lengthMenu: "Show _MENU_",
                        search: "Search:",
                        zeroRecords: "No matching records found"
                    },
                    //
                    //responsive: {
                    //    details: {
                    //        type: 'column',
                    //        target: 'tr'
                    //    }
                    //},
                    "autoWidth": true
                };

                if ((options.isDomDefault != undefined) && (options.isDomDefault)) {
                    dt_options["dom"] = "\tf<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>";
                } else if ((options.isDomDefault != undefined) && (!options.isDomDefault)) {
                    dt_options["dom"] = "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>";
                }

                dt_options["columns"] = (options.columns == undefined ? [] : options.columns);
                dt_options["columnDefs"] = (options.columnDefs == undefined ? [] : options.columnDefs);
                dt_options["select"] = (options.select == undefined ? [] : options.select);

                dt_options["pageLength"] = (options.pageLength == undefined ? 10 : options.pageLength);
                //dt_options["pagingType"] = (options.goTopage == undefined || options.goTopage == false ? "bootstrap_full_number" : "bootstrap_extended");

                if (options.headerCallback != undefined) {

                    dt_options["headerCallback"] = options.headerCallback;
                }

                dt_options["order"] = (options.order == undefined ? [[1, "asc"]] : options.order);



                //"sDom":'t',

                var table = undefined;
                table = e.DataTable(dt_options);
                if (callback != undefined)
                    callback(e);

                return table;
            },
            disableTab: function (tabTarget) {
                var e = this;

                e.find('a[data-toggle="tab"]').each(function (tabIndex, tabControl) {
                    //console.log(tabIndex)    
                    $.each(tabTarget, function (e, i) {
                        if (i == tabIndex) {
                            $(tabControl).addClass('hide');
                        }
                    })
                });
                return this;
            },
            enableTab: function (tabTarget) {
                var e = this;
                e.find('a[data-toggle="tab"]').each(function (tabIndex, tabControl) {
                    $.each(tabTarget, function (e, i) {
                        if (i == tabIndex) {
                            $(tabControl).removeClass('hide');
                        }
                    })
                });
                return this;
            },
            cmValidate: function (paramOptions) {
                var e = this;
                var error = $('.alert-danger', e);
                var success = $('.alert-success', e);

                var options = {
                    onsubmit: false,
                    errorElement: 'span', //default input error message container
                    errorClass: 'help-block help-block-error', // default input error message class
                    focusInvalid: false, // do not focus the last invalid input
                    ignore: "",  // validate all fields including form hidden input
                    messages: {
                        select_multi: {
                            maxlength: jQuery.validator.format("Max {0} items allowed for selection"),
                            minlength: jQuery.validator.format("At least {0} items must be selected")
                        }
                    },
                    invalidHandler: function (event, validator) { //display error alert on form submit              
                        success.hide();
                        error.show();
                        App.scrollTo(error, -200);
                    },

                    errorPlacement: function (error, element) { // render error placement for each input type
                        var cont = $(element).parent('.input-group');
                        if (cont.size() > 0) {
                            cont.after(error);
                        } else {
                            element.after(error);
                        }
                    },

                    highlight: function (element) { // hightlight error inputs

                        $(element)
                            .closest('.form-group').addClass('has-error'); // set error class to the control group
                    },

                    unhighlight: function (element) { // revert the change done by hightlight
                        $(element)
                            .closest('.form-group').removeClass('has-error'); // set error class to the control group
                    },

                    success: function (label) {
                        label
                            .closest('.form-group').removeClass('has-error'); // set success class to the control group
                    },

                    submitHandler: function (form) {
                        success.show();
                        error.hide();
                    }
                };
                options["rules"] = (paramOptions.rules == undefined ? {} : paramOptions.rules);
                e.validate(options);
                e.on('err.field.fv', function (e, data) {
                    // data.element --> The field element

                    var $tabPane = data.element.parents('.tab-pane'),
                        tabId = $tabPane.attr('id');

                    $('a[href="#' + tabId + '"][data-toggle="tab"]')
                        .parent()
                        .find('i')
                        .removeClass('fa-check')
                        .addClass('fa-times');
                })

                // Called when a field is valid
                e.on('success.field.fv', function (e, data) {
                    // data.fv      --> The FormValidation instance
                    // data.element --> The field element

                    var $tabPane = data.element.parents('.tab-pane'),
                        tabId = $tabPane.attr('id'),
                        $icon = $('a[href="#' + tabId + '"][data-toggle="tab"]')
                            .parent()
                            .find('i')
                            .removeClass('fa-check fa-times');

                    // Check if all fields in tab are valid
                    var isValidTab = data.fv.isValidContainer($tabPane);
                    if (isValidTab !== null) {
                        $icon.addClass(isValidTab ? 'fa-check' : 'fa-times');
                    }
                });
            },
            genericTable: function (url, options) {
                var e = this;
                //App.blockUI({
                //    boxed: true
                //});
                var pages;
                var startIndex;
                var endIndex;
                var draw = options.draw || 1;
                var GroupRow = [];
                console.log('draw position : ' + draw);
                startIndex = (draw - 1) * options.pageLength;

                for (var c = 0; c < options.columns.length; c++) {
                    var column = options.columns[c];
                    column.name = column.data;
                    column.search = {
                        value: "",
                        regex: false
                    }
                    column.searchable = column.searchable || true;
                    column.orderable = column.orderable || true;
                }
                var data = {
                    draw: draw,
                    start: startIndex,
                    length: options.pageLength,
                    pageLength: options.pageLength,
                    destroy: true,
                    processing: true,
                    serverSide: true,
                    columns: options.columns,
                    order: options.order,
                    search: {
                        value: "",
                        regex: false
                    }
                }

                $.post(url, data, function (result) {
                    if (result) {
                        endIndex = draw * options.pageLength > result.data.length ?
                            result.data.length - 1 : draw * options.pageLength - 1;

                        // Show pager row if there is more than one page
                        var pager = '';
                        pages = Math.floor(result.recordsTotal / options.pageLength);
                        if (pages < result.recordsTotal / options.pageLength) {
                            pages += 1;
                        }
                        e.find('.pagination-info').html('<span class="pagination-info"> ' + ((draw * options.pageLength) - options.pageLength + 1) + ' to ' + (draw * options.pageLength) + ' of ' + result.recordsTotal + '&nbsp;&nbsp;&nbsp;</span>');
                        $(e.selector + ' tr:gt(0)').remove();

                        for (var i = 0; i < result.data.length; i++) {
                            var d = result.data[i];
                            var row = '<tr>';
                            row += '<td class="inbox-small-cells">';
                            row += '<label class="mt-checkbox mt-checkbox-single mt-checkbox-outline">';
                            row += '<input type="checkbox" class="mail-checkbox" value="1" />';
                            row += '<span></span>';
                            row += '</label>';
                            row += '</td>';


                            for (var c = 0; c < options.columns.length; c++) {
                                var column = options.columns[c];
                                if (typeof column.render == "function") {
                                    row += '<td>' + column.render(d[column.data], d) + '</td>';
                                } else {
                                    row += '<td class="view-message">' + d[column.data] + '</td>';
                                }

                            }
                            row += '</tr>';
                            $(e.selector).append(row);
                        }


                        //Generate Pager                        
                        if (pages > 1) {
                            e.find('.page-previous').unbind("click");
                            e.find('.page-previous').click(function (ee) {
                                ee.preventDefault();
                                if (draw < 1) return;
                                var pNum = (draw - 1);
                                options["draw"] = pNum;
                                $(e.selector).genericTable(url, options);

                            });

                            e.find('.page-next').unbind("click");
                            e.find('.page-next').click(function (ee) {
                                ee.preventDefault();
                                if (draw < 1 || draw >= pages) return;
                                var pNum = (draw + 1);
                                options["draw"] = pNum;
                                $(e.selector).genericTable(url, options);
                            });
                        }




                    }
                });

            }
        });
})(jQuery);