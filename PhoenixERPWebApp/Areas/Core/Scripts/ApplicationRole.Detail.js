
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('#detail-id');
        var _formApplicationRole = $('#form-application_role');
        var $optionalMember = $("#optional");
        var $selectedMember = $("#selected");
        $.helper.controlCheckboxValue();

        var asideLeftToggle = new mToggle('m_aside_left_minimize_toggle', {
            target: 'body',
            targetState: 'm-brand--minimize m-aside-left--minimize',
            togglerState: 'm-brand__toggler--active'
        });
        asideLeftToggle.toggleOn();


        var dataSourceUserMember = new kendo.data.DataSource({
            type: "json",
            pageSize: 100,
            serverPaging: true,
            serverFiltering: true,
            transport: {
                read: {
                    url: $.helper.resolveApi("~/core/UserRole/kendoLookupAssignableUserMember?roleId=" + _detailID.val() ),
                    contentType: 'application/json',
                    dataType: 'json',
                    type: 'POST',
                    cache: true
                },
                parameterMap: function (data, operation) {
                    var mapRequest = data;
                    return JSON.stringify(mapRequest);
                }
            },
            requestStart: function () {
                kendo.ui.progress($(".demo-section"), true);
            },
            requestEnd: function () {
                kendo.ui.progress($(".demo-section"), false);
            },
            batch: false,
            schema: {
                model: {
                    id: "id",
                    fields: {
                        is_member: { type: "boolean" },
                    }
                },
                data: "items"
            }
        });
        fetchDataSource();
        function fetchDataSource() {
            dataSourceUserMember.fetch(function () {
                kendo.ui.progress($(".demo-section"), true);
                var data = this.data();
                var optional = $optionalMember.data("kendoListBox");
                var selected = $selectedMember.data("kendoListBox");
                var dataValidate = [];
                //clear data optional
                optional.remove(optional.items());
                //get current data selected
                var itemSelected = selected.items();
                for (var i = 0; i < data.length; i++) {
                    var already = false;
                    for (var iSel = 0; iSel < itemSelected.length; iSel++) {
                        if (data[i].id == selected.dataItem(itemSelected[iSel]).id) {
                            already = true;
                            break;
                        }
                    }
                    if (already==false) {
                        dataValidate.push(data[i]);
                    }
                }
                for (var i = 0; i < dataValidate.length; i++) {
                    if (dataValidate[i].vacant) {
                        optional.add(dataValidate[i]);
                    }
                    else {
                        selected.add(dataValidate[i]);
                    }
                }
                kendo.ui.progress($(".demo-section"), false);
                disableMember(true);
            });
        }

        function disableMember(act) {
            act &= (_detailID.val() == '');
            $optionalMember.data("kendoListBox").enable(".k-item", !act);
            $selectedMember.data("kendoListBox").enable(".k-item", !act);
            $("#example").find('.searchbox').enable(!act);
        }

        $("#example").find('.searchbox').keyup(function (e) {
            if (e.keyCode === 13) {
                var searchValue = $(this).val();
                dataSourceUserMember.filter({ field: "app_fullname", operator: "contains", value: searchValue });
                fetchDataSource();
            }
        });

        var memberTemplate = '<span class="k-state-default" style="background-image: url(\'/assets/img/avatars/user.png\')"></span>' +
            '<span class="k-state-default"><h3>#: data.app_fullname #</h3><p>#: data.email #</p></span>';
        $optionalMember.kendoListBox({
            disabled: true,
            selectable: "multiple",
            dataTextField: "app_fullname",
            dataValueField: "id",
            template: memberTemplate,
            draggable: { placeholder: customPlaceholder },
            dropSources: ["selected"],
            connectWith: "selected",
            toolbar: {
                position: "right",
                tools: ["moveUp", "moveDown", "transferTo", "transferFrom", "transferAllTo", "transferAllFrom"]
            }
        });

        $selectedMember.kendoListBox({
            dataTextField: "app_fullname",
            dataValueField: "id",
            template: memberTemplate,
            draggable: { placeholder: customPlaceholder },
            dropSources: ["optional"],
            connectWith: "optional",
            add: function (e) {
                kendo.ui.progress($(".demo-section"), true);
                var Ids = [];
                $.each(e.dataItems, function (i, v) {
                    Ids.push(v.id);
                });
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/UserRole/addmember/?roleId=" + _detailID.val()),
                    data: JSON.stringify(Ids),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            kendo.ui.progress($(".demo-section"), false);
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
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

            },
            remove: function (e) {
                console.log(e);
                kendo.ui.progress($(".demo-section"), true);
                var Ids = [];
                console.log(e.dataItems);
                $.each(e.dataItems, function (i, v) {
                    Ids.push(v.id);
                });
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/UserRole/deleteMember/?roleId=" + _detailID.val()),
                    data: JSON.stringify(Ids),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            kendo.ui.progress($(".demo-section"), false);
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
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

            }
        });

        function customPlaceholder(draggedItem) {
            return draggedItem
                .clone()
                .addClass("custom-placeholder")
                .removeClass("k-ghost");
        }

        //BEGIN::Form Validation
        _formApplicationRole.validate({
            rules: {
                role_name: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                var errors = r.numberOfInvalids();
                if (errors) {
                    var message = errors === 1
                        ? 'Please correct the following error:\n'
                        : 'Please correct the following ' + errors + ' errors.\n';

                    if (r.errorList.length > 0) {
                        for (var x = 0; x < r.errorList.length; x++) {
                            console.warn(r.errorList[x]);
                            errors += "\n\u25CF " + r.errorList[x].message;
                        }
                    }
                    alert(message + errors);
                }
                mUtil.scrollTo(_formApplicationRole, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::Load Datatable
        var loadDetailData = function () {

            if (_detailID.val() !== '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Processing..."
                }),
                    $.ajax({
                        type: "GET",
                        dataType: 'json',
                        url: $.helper.resolveApi("~/core/ApplicationRole/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('input[name="role_name"]').val(record.role_name);
                                if (record.is_default_role) {
                                    $('input[name="is_default_role"][value="true"]').prop("checked", true);
                                }
                                mApp.unblockPage();
                                loadRoleAccess(record.id);
                                disableMember(false);

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
                        mApp.unblockPage();
                    }, 2e3));

            } else {
                $('.no-record').removeClass("collapse");

            }

        };

        var loadRoleAccess = function (roleid) {
            var $load = $('#loadroleaccess');
            mApp.block("#m_tabs_role_access", {
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });
            var url = $load.attr('data-url') + "?roleId=" + roleid;
            $load.load(url, function (result) {
                mApp.unblock("#m_tabs_role_access");
                //$('#gridRoleAccess').data('kendoGrid').refresh();
            });
        }


        //END::Load Datatable

        //BEGIN::EVENT BUTTON
        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formApplicationRole.serializeToJSON();

            if (_formApplicationRole.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/ApplicationRole/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            _detailID.val(r.data.recordID);
                            toastr.success(r.status.message, "Information");
                            disableMember(false);
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
                    },
                    error: function (r) {
                        swal(
                            '' + r.status,
                            r.statusText,
                            'error'
                        );
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    }
                }).then(function () { btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); });

            }
        });
        //END::EVENT BUTTON

        return {
            init: function () {
                //initialize
                loadDetailData();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


