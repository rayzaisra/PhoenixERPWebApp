(function ($) {
    'use strict';
    var dt_basic_user;
    var ListChecked = [];
    var ListNotChecked = [];
    var pageFunction = function () {
        var TeamId = $('#detail-id').val();
        var LoadDataTable = function () {
            // begin first table
            var url = $.helper.resolveApi('~/core/UserRole/' + TeamId + '/listdatatablesavailableassignusermember');
            var e = $("#dt_basic_user");
            dt_basic_user = e.cmDataTable(url, {
                goTopage: false,
                pageLength: 5,
                isDomDefault: false,
                stateSaveParams: function (d) {
                    $("#form-search-dt_basic_user .searchbox").val(d.search.search);
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
                                output = `<div class="m-widget2__desc">
															<span class="m-widget2__text">
																`+data+`
															</span><br>
															<span class="m-widget2__user-name">
																<a href="#" class="m-widget2__link">
																	`+row.email+`
																</a>
															</span>
														</div>`;
                                return output;
                            }
                            return data;
                        }

                    }

                ]
            }, function (e) {
                var $table = e; // table selector 
                //BEGIN Select All row dt_basic
                $('#dt_basic_user thead th input[name=select_all]').on('click', function () {
                    var $tdCheckbox = $table.find('tbody input:checkbox'); // checboxes inside table body   
                    $.each($tdCheckbox, function (i, d) {
                        $(this).trigger("click");
                    });
                    $tdCheckbox.prop('checked', this.checked);
                });

                $(document).on('click', '#dt_basic_user tbody tr input[type="checkbox"]', function () {
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
                    $('#dt_basic_user input[name=select_all]').prop('checked', $tdCheckboxChecked.length === $tdCheckbox.length);
                });
            }),

                $("#form-search-dt_basic_user").submit(function (e) {
                    e.preventDefault();
                    var v = $(this).find('.searchbox').val();
                    dt_basic_user.search(v).draw();
                });
        };

        var AssignUserRoleMember = function (ArrID) {
            var btn = $('#btn-addselected');

            swal({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                type: "info",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (e) {

                if (e.value) {
                    btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        contentType: 'application/json',
                        url: $.helper.resolveApi("~/core/UserRole/addmember/?roleId=" + TeamId),
                        data: JSON.stringify(ArrID),
                        success: function (r) {
                            if (r.status.success) {
                                swal({
                                    title: 'Information',
                                    text:  'Success Assign to User Role Member',
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
                            btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                            dt_basic_user.ajax.reload();
                        },
                        error: function (e, t, s) {
                            swal(
                                'Information',
                                'Ooops, something went wrong !',
                                'info'
                            );
                        }
                    }).then(function () { btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); });
                }


            });
        };
        //BEGIN::EVENT BUTTON
        $('#btn-addselected').click(function () {
            if (ListChecked.length > 0)
                AssignUserRoleMember(ListChecked);
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


