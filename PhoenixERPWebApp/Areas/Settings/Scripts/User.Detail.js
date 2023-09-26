
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('#detail-id').val();
        var _formPersonalInfo = $('#form-personal_info');
        var $employeeLookup = $('#employee_id');
        var _formChangePassword = $('#form-change_password');

        //BEGIN::Form Validation
        _formChangePassword.validate({
            rules: {
                app_password: {
                    required: true
                },
                app_password_confirm: {
                    required: true,
                    equalTo: "#app_password"
                }
            },
            invalidHandler: function (e, r) {

                mUtil.scrollTo(_formPersonalInfo, -200);
            },
            submitHandler: function (e) { }
        });

        _formPersonalInfo.validate({
            rules: {

                app_username: {
                    required: true
                },
                app_fullname: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true
                },
                is_active: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {

                mUtil.scrollTo(_formPersonalInfo, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation


        //BEGIN::LOOKUP

        $employeeLookup.kendoComboBox({
            placeholder: "Choose ..",
            dataTextField: "name_employee",
            dataValueField: "id",
            filter: "contains",
            delay: 1000,
            autoBind: false,
            dataSource: {
                type: "json",
                pageSize: 100,
                serverPaging: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: $.helper.resolveApi("~/Hris/EmployeeBasicInfo/List?page=1&rows=100&sortBy=name_employee&sortDir=asc"),
                        contentType: 'application/json',
                        dataType: 'json',
                        type: 'GET',
                        cache: true
                    },
                    parameterMap: function (data, operation) {

                        return {
                            page: data.page,
                            rows: 100
                            //search_name_employee: (data.filter.filters.length ==0 ? '' : data.filter.filters[0].value) 
                        };
                    }
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "rows"
                }

            }
        });

        //BEGIN::Load Datatable
        var loadDetailData = function () {

            if (_detailID !== '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Processing..."
                }),
                    $.ajax({
                        type: "GET",
                        dataType: 'json',
                        url: $.helper.resolveApi("~/core/User/" + _detailID + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                $('#info-app_fullname').text(record.app_fullname);
                                $('#info-email').text(record.email);


                                $('input[name="app_username"]').val(record.app_username);
                                $('input[name="app_fullname"]').val(record.app_fullname);
                                $('input[name="email"]').val(record.email);
                                $('input[name="phone"]').val(record.phone);

                                $("input[name=is_system_administrator][value='" + record.is_system_administrator + "']").prop("checked", true);
                                $("input[name=is_active][value='" + record.is_active + "']").prop("checked", true);
                                $("input[name=is_msuser][value='" + record.is_msuser + "']").prop("checked", true);
                                $.helper.kendoUI.combobox.setValue($employeeLookup, { id: record.employee_basic_info_id, name_employee: record.name_employee });
                            } else {
                                swal(
                                    'Information',
                                    r.status.message,
                                    'info'
                                );
                            }
                        },
                        error: function (e, t, s) {
                            //swal(
                            //    'Information',
                            //    'Ooops, something went wrong !',
                            //    'info'
                            //);
                            console.log(e);
                        }
                    }).then(setTimeout(function () {
                        mApp.unblockPage();
                    }, 2e3));

            }

        };
        //END::Load Datatable

        //BEGIN::EVENT BUTTON

        $('#btn-changepassword').click(function (e) {
            var btn = $(this);
            var data = _formChangePassword.serialize();
            if (_formChangePassword.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: $.helper.resolve("~/Settings/User/ChangePassword"),
                    data: data,
                    success: function (data) {

                        if (data || data.Success === true) {
                            toastr.success(data.Message, "Information");
                            console.log(data);
                            //history.pushState('', 'ID', location.hash.split('/')[location.hash.split('/').length] + '/' + data.Id);
                        } else if (data || data.Success === false) {
                            swal(
                                'Saving',
                                data.Message,
                                'error'
                            );
                        }
                    },
                    error: function () {
                        //swal(
                        //    'Information',
                        //    'Ooops, something when wrong!',
                        //    'error'
                        //);
                        console.log(e);
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    }
                }).then(function () { btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); });
            }
        });

        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formPersonalInfo.serializeToJSON();
            if (_formPersonalInfo.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/User/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            toastr.success(r.status.message, "Information");
                            console.log(r.data);
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


