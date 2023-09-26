(function ($) {
    'use strict';
    var dt_basic_user_role;
    var ListChecked = [];
    var ListNotChecked = [];
    var pageFunction = function () {
        var RoleId = $('#detail-id').val();
        var LoadDataTable = function () {
            // begin first table
            var url = $.helper.resolveApi('~/core/UserRole/' + RoleId+'/listdatatables');
            var e = $("#dt_basic_user_role");


            var dt_basic_user_role = e.cmDataTable2({
                dom: "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                stateSaveParams: function (settings, data) {
                    $("#form-search-dt_basic_user_role .searchbox").val(data.search.search);
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
                        data: "app_fullname",
                        orderable: true,
                        searchable: true,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                var output;
                                if (type === 'display') {
                                    return '<b>' + row.app_username + '</b>' + ' - ' + row.app_fullname;
                                }
                                return output;
                            }
                            return data;
                        }

                    }
                ]
            }, function (e, settings, json) {
                var $table = e; // table selector 
                //BEGIN Select All row dt_basic_user_role
                $('#dt_basic_user_role input[name=select_all]').on('click', function () {
                    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
                    $.each($tdCheckbox, function (i, d) {
                        $(this).trigger("click");
                    });
                    $tdCheckbox.prop('checked', this.checked);

                });

                //ROW CLICK
                $(document).on('click', '#dt_basic_user_role tbody tr input[type="checkbox"]', function () {
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
                    $('#dt_basic_user_role input[name=select_all]').prop('checked', $tdCheckboxChecked.length === $tdCheckbox.length);
                });

                $("#form-search-dt_basic_user_role").submit(function (e) {
                    e.preventDefault();
                    var v = $(this).find('.searchbox').val();
                    dt_basic_user_role.search(v).draw();
                });
            });

     
        };


        var DeleteData = function (ArrID,callback=null) {
            $.ajax({
                type: "DELETE",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/core/UserRole/delete"),
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
                            dt_basic_user_role.ajax.reload();
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
                if (callback != null) {
                    callback();
                }
            }, 2e3));
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
                LoadDataTable();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


