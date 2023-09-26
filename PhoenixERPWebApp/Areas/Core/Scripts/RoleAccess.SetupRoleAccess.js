(function ($) {
    'use strict';
    var dt_basic_role_access;
    var gridUI = $('#gridRoleAccess');
    var $applicationRoleId = $('#application_role_id');
    var ListChecked = [];
    var ListNotChecked = [];
    var pageFunction = function () {
        var getRoleId = function () { return $applicationRoleId.val(); }

        var updateAccess = function (entityID, name, value) {
            var obj = {};
            var RoleAccessId = getRoleId();
            switch (name) {
                case "access_create":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_create: value
                    }
                    break;
                case "access_read":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_read: value
                    }
                    break;
                case "access_update":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_update: value
                    }
                    break;
                case "access_delete":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_delete: value
                    }
                    break;
                case "access_approve":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_approve: value
                    }
                    break;
                case "access_lock":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_lock: value
                    }
                    break;
                case "access_activate":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_activate: value
                    }
                    break;
                case "access_append":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_append: value
                    }
                    break;
                case "access_share":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_share: value
                    }
                    break;
                case "access_level":
                    obj = {
                        id: entityID,
                        application_role_id: RoleAccessId,
                        access_level: value
                    }
                    break;

            }

            kendo.ui.progress($(".demo-section"), true);
            $.ajax({
                type: "POST",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/core/RoleAccess/save"),
                data: JSON.stringify(obj),
                success: function (r) {
                    if (r && r.status.success) {
                        toastr.success(r.status.message, "Information");
                    }
                    else {
                        swal(
                            'Saving',
                            r.status.message,
                            'info'
                        );
                    }
                    kendo.ui.progress($(".demo-section"), false);
                },
                error: function (r) {
                    swal(
                        '' + r.status,
                        r.statusText,
                        'error'
                    );
                    kendo.ui.progress($(".demo-section"), false);
                }
            });
        };

        function accessTemplate(data, field) {
            var output = '<input  type="text" name="' + field + '" data-id="' + data.id + '" value="' + data[field] + '" class="entity-pie form-control form-control-sm m-input" />';
            return output;
        }

        var LoadKendoGrid = function () {
            var RoleAccessId = getRoleId();
            var url = $.helper.resolveApi('~/core/RoleAccess/' + RoleAccessId + '/EntityRoleKendoGrid');
            $.helper.kendoUI.grid($("#gridRoleAccess"), {
                options: {
                    url: url
                },
                navigatable: true,
                pageable: false,
                //height: 300,
                pageable: {
                    refresh: true,
                    pageSizes: [5, 10, 20, 100]
                },
                resizable: true,
                dataSource: {
                    pageSize: 10,
                    serverPaging: true,
                    serverSorting: true,
                    serverOperation: false,
                    schema: {
                        model: {
                            id: "id"
                        },
                        data: "data",
                        total: "recordsTotal"
                    },
                },
                marvelCheckboxSelectable: {
                    enable: false
                },
                dataBound: function (e) {
                    $('input.entity-pie').cmRole(function (input) {
                        var entityID = $(input).attr('data-id');
                        var name = $(input).attr('name');
                        var value = $(input).val();
                        updateAccess(entityID, name, value);
                    });
                    $.helper.kendoUI.resizeGrid();

                },
                dataBinding: function (e) {

                },
                columns: [
                    {
                        template: "<b>#= data.display_name ? display_name : ''#</b>",
                        field: "display_name",
                        title: "Entity",
                        width: "300px"
                    },
                    {
                        field: "access_create",
                        title: "Create",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_create"); }
                    },
                    {
                        field: "access_read",
                        title: "Read",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_read"); }
                    },
                    {
                        field: "access_update",
                        title: "Update",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_update"); }
                    },
                    {
                        field: "access_delete",
                        title: "Delete",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_delete"); }
                    },
                    {
                        field: "access_approve",
                        title: "Approve",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_approve"); }
                    },
                    {
                        field: "access_lock",
                        title: "Lock",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_lock"); }
                    },
                    {
                        field: "access_activate",
                        title: "Activate",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_activate"); }
                    },
                    {
                        field: "access_append",
                        title: "Append",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_append"); }
                    },
                    {
                        field: "access_share",
                        title: "Share",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_share"); }
                    },
                    {
                        field: "access_level",
                        title: "Level",
                        width: "80px",
                        headerAttributes: { style: "text-align:center" },
                        template: function (data) { return accessTemplate(data, "access_level"); }
                    }
                ]
            });

        }

        $('input[type="checkbox"][name="is_locked_column"]').change(function () {
            var grid = gridUI.data("kendoGrid");
            var options = grid.getOptions();
            if ($(this).is(':checked')) {
                options.columns[0].locked = true;
                options.columns[0].lockable = false;

            } else {
                options.columns[0].locked = false;
                options.columns[0].lockable = false;
            }
            grid.setOptions(options);
        });


        var LoadDataTable = function () {
            // begin first table
            var url = $.helper.resolveApi('~/core/RoleAccess/' + RoleAccessId + '/listdatatables');
            var e = $("#dt_basic_role_access");

            var dt_basic_role_access = e.cmDataTable2({
                dom: "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                stateSaveParams: function (settings, data) {
                    $("#form-search-dt_basic_role_access .searchbox").val(data.search.search);
                },
                pageLength: 10,
                ajax: {
                    url: url,
                    type: "POST",
                    data: function (d) {
                        return d;
                    }
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
                    {
                        data: "display_name",
                        orderable: true,
                        searchable: true,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    return '' + row.display_name + '';
                                }
                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_create",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_create" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_read",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_read" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_update",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_update" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_delete",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_delete" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_approve",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_approve" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_lock",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_lock" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_activate",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_activate" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_append",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_append" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_share",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_share" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    },
                    {
                        data: "access_level",
                        orderable: false,
                        searchable: false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    output = '<input type="text" name="access_level" data-id="' + row.id + '" value="' + data + '" class="entity-pie form-control form-control-sm m-input" />';
                                    return output;
                                }

                                return output;
                            }
                            return data;
                        }

                    }
                ],
                drawCallback: function (oSettings) {
                    $('input.entity-pie').cmRole(function (input) {
                        var entityID = $(input).attr('data-id');
                        var name = $(input).attr('name');
                        var value = $(input).val();
                        updateAccess(entityID, name, value);
                    });
                }
            }, function (e, settings, json) {
                var $table = e; // table selector 
                //BEGIN Select All row dt_basic_role_access
                $('#dt_basic_role_access input[name=select_all]').on('click', function () {
                    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
                    $.each($tdCheckbox, function (i, d) {
                        $(this).trigger("click");
                    });
                    $tdCheckbox.prop('checked', this.checked);

                });

                //ROW CLICK
                $(document).on('click', '#dt_basic_role_access tbody tr input[type="checkbox"]', function () {
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
                    $('#dt_basic_role_access input[name=select_all]').prop('checked', $tdCheckboxChecked.length === $tdCheckbox.length);
                });

                $("#form-search-dt_basic_role_access").submit(function (e) {
                    e.preventDefault();
                    var v = $(this).find('.searchbox').val();
                    dt_basic_role_access.search(v).draw();
                });
            });
        };


        //BEGIN::EVENT BUTTON
        $('#btn-add-user_role_member').click(function () {
            var btn = $(this);
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            $('.modal-container').load($.helper.resolve("~/Core/UserRole/AddUserRoleMember/?roleId=" + RoleId), function (result) {
                $('#m_modal_user_role_member').modal({ show: true });
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }).then(function () { btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); });
        });


        $('#btn-remove-user_role_member').click(function () {
            var btn = $(this);
            if (ListChecked.length > 0) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
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
                        DeleteData(ListChecked, function () {
                            btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                        });
                    }
                });
            }
        });
        //END::EVENT BUTTON


        //BEGIN::EVENT BUTTON

        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
                //LoadDataTable();
                LoadKendoGrid();


            }
        };
    }();



    $(document).ready(function () {

        pageFunction.init(),
            $.helper.kendoUI.resizeGrid();
    });
}(jQuery));


