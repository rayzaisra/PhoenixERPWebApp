(function ($) {
    var pageFunction = function () {
        var _detailID = $('#detail-id');
        var counter = {};

        //BEGIN::Load Datatable
        var loadDetailData = function () {

            var addressLoad = function (data) {
                $.each(data, function (i, v) {
                    var setindex = (v.address_type - 1);
                    $('input[name="company_address.id[' + setindex + ']"]').val(v.id);
                    $('input[name="company_address.address_type[' + setindex + ']"]').val(v.address_type);
                    $('textarea[name="company_address.address[' + setindex + ']"]').text(v.address);

                    $('select.country-lookup[name="company_address.country_id[' + setindex + ']')
                        .cmSetLookup(v.country_id, v.country_name);
                    $('select.city-lookup[name="company_address.city_id[' + setindex + ']')
                        .cmSetLookup(v.city_id, v.city_name);
                    $('input[name="company_address.zip_code[' + setindex + ']"]').val(v.zip_code);
                });
            }
            var bankLoad = function (data) {
                $.each(data, function (i, v) {
                    FormDynamic.addBank(function (idx) {
                        $('input[name="company_bank.id[' + idx + ']"]').val(v.id);
                        $('select.bank-lookup[name="company_bank.bank_id[' + idx + ']')
                            .cmSetLookup(v.bank_id, v.bank_name);
                        $('select.currency-lookup[name="company_bank.currency_id[' + idx + ']')
                            .cmSetLookup(v.currency_id, v.currency_code);
                        $('input[name="company_bank.account_number[' + idx + ']"]').val(v.account_number);
                    });
                });
            }

            var contactLoad = function (data) {
                $.each(data, function (i, v) {
                    FormDynamic.addContact(function (idx) {
                        $('input[name="company_contact.id[' + idx + ']"]').val(v.id);
                        $('select.salutation-lookup[name="company_contact.salutation_id[' + idx + ']')
                            .cmSetLookup(v.salutation_id, v.title);
                        $('input[name="company_contact.contact_name[' + idx + ']"]').val(v.contact_name);
                        $('input[name="company_contact.position_title[' + idx + ']"]').val(v.position_title);
                        $('input[name="company_contact.phone[' + idx + ']"]').val(v.phone);
                        $('input[name="company_contact.email[' + idx + ']"]').val(v.email);
                    });
                });
            };

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
                        url: $.helper.resolveApi("~/finance/CompanyProfile/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                var _companyRecord = record.company;
                                var _companyAddressData = record.company_address;
                                var _companyBankData = record.company_bank;
                                var _companyContactData = record.company_contact;

                                $('input[name="company.registration_code"]').val(_companyRecord.registration_code);
                                $('input[name="company.company_name"]').val(_companyRecord.company_name);
                                $('input[name="company.website"]').val(_companyRecord.website);
                                $('input[name="company.tax_registration_number"]').val(_companyRecord.tax_registration_number);
                                $('input[name="company.phone"]').val(_companyRecord.phone);
                                $('input[name="company.fax"]').val(_companyRecord.fax);
                                $('input[name="company.email"]').val(_companyRecord.email);
                                //alert(_companyRecord.is_client);
                                $("input[name='company.is_vendor'][value='true']").prop("checked", _companyRecord.is_vendor);
                                $("input[name='company.is_client'][value='true']").prop("checked", _companyRecord.is_client);

                                $('input[name="company.payment_term_day"]').val(_companyRecord.payment_term_day);
                                $('input[name="company.shipping_term_day"]').val(_companyRecord.shipping_term_day);
                                $('input[name="company.facebook"]').val(_companyRecord.facebook);
                                $('input[name="company.twitter"]').val(_companyRecord.twitter);
                                $('input[name="company.instagram"]').val(_companyRecord.instagram);


                                //Load Address
                                addressLoad(_companyAddressData);
                                //Load Bank
                                bankLoad(_companyBankData);
                                //Load Contact
                                contactLoad(_companyContactData);

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

            }
            else {
                $('.no-record').removeClass("collapse");
                $('#tabs_account_setup > .cost-sharing').hide();
                FormDynamic.addBank();
                FormDynamic.addContact();

            }
        };

        //Begin:: Function Add Form
        function addForm(data, callBack) {
            if (!counter[data[2]]) {
                counter[data[2]] = 0;
            }

            var num = 0;
            do {
                var count = $('div.' + data[2] + '[data-idx="' + num + '"]').length;

                if (count > 0) {
                    num++;
                }
                else {
                    break;
                }
            } while (true)

            var dataRow = $(data[1]).html();
            var row = document.createElement("div");
            dataRow = dataRow.replace(/\%index\%/gi, num);
            $(row).hide();
            $(row).attr({
                "class": "row " + data[2],
                "data-idx": num
            }).html(dataRow);
            $(data[0]).append(row);
            $(row).slideDown();
            $('div.' + data[2] + '[data-idx="' + num + '"] button.removeit').off('click').on('click', function () {
                var idx = $(this).attr('data-index');

                if ($('div.' + data[2]).length > 1) {
                    $('div.' + data[2] + '[data-idx="' + idx + '"]').slideUp().promise().done(function () {
                        $(this).remove();
                        counter[data[2]] = $('div.' + data[2]).length;
                    });
                }
            });

            // bwt generate select2 dan plugin yg lainnya
            if (callBack) {
                callBack(num, data);
            }

            counter[data[2]] = $('div.' + data[2]).length;
        }
        //End:: Function Add Form
        //Begin:: Document Ready
        var initLookup = function () {
            $(".city-lookup").cmSelect2({
                url: $.helper.resolveApi("~/core/City/lookup"),
                filters: ' r.city_name LIKE @0 ',
                //paramaters: [],
                result: {
                    id: 'id',
                    text: 'city_name',
                }
            });

            $(".country-lookup").cmSelect2({
                url: $.helper.resolveApi("~/core/Country/lookup"),
                filters: ' r.country_name LIKE @0 ',
                //paramaters: [],
                result: {
                    id: 'id',
                    text: 'country_name',
                }
            });
        }
        var FormDynamic = function () {
            return {
                addBank: function (callback) {
                    if (!counter["counter-bank"]) counter["counter-bank"] = 1;
                    if (counter["counter-bank"] && counter["counter-bank"] < 4) {
                        addForm(["#bank_input", ".bank", "counter-bank"], function (idx, data) {
                            // select2 bank lookup
                            $('div.' + data[2] + '[data-idx="' + idx + '"] select.bank-lookup[name="company_bank.bank_id[' + idx + ']"]').cmSelect2({
                                url: $.helper.resolveApi("~/finance/Bank/lookup"),
                                filters: ' r.bank_name LIKE @0 ',
                                //paramaters: [],
                                result: {
                                    id: 'id',
                                    text: 'bank_name',
                                }
                            });

                            // select2 currency lookup
                            $('div.' + data[2] + '[data-idx="' + idx + '"] select.currency-lookup[name="company_bank.currency_id[' + idx + ']"]').cmSelect2({
                                url: $.helper.resolveApi("~/core/Currency/lookup"),
                                filters: ' (r.currency_code LIKE @0 OR r.currency_symbol LIKE @0) ',
                                //paramaters: [],
                                result: {
                                    id: 'id',
                                    text: 'currency_code',
                                }
                            });

                            if (callback) {
                                callback(idx);
                            }
                        });
                    } else {
                        swal('', 'Maximum Of 4 Data', 'info');
                    }
                },
                addContact: function (callback) {
                    if (!counter["counter-contact"]) counter["counter-contact"] = 1;
                    if (counter["counter-contact"] && counter["counter-contact"] < 4) {
                        addForm(["#contact_person_input", ".contact-person", "counter-contact"], function (idx, data) {
                            // select2 Salutation lookup
                            $('div.' + data[2] + '[data-idx="' + idx + '"] select.salutation-lookup[name="company_contact.salutation_id[' + idx + ']"]').cmSelect2({
                                url: $.helper.resolveApi("~/core/Salutation/lookup"),
                                filters: ' r.title LIKE @0 ',
                                //paramaters: [],
                                result: {
                                    id: 'id',
                                    text: 'title',
                                }
                            });
                            if (callback) {
                                callback(idx);
                            }
                        });
                    } else {
                        swal('', 'Maximum Of 4 Data', 'info');
                    }
                }
            }
        }();
        //End:: Document Ready

        //BEGIN::EVENT BUTTON

        //Begin:: Button Add Bank
        $('#btn-add-bank').click(function (e) {
            FormDynamic.addBank();
        });
        //End:: Button Add Bank
        //Begin:: Button Add Contact Person
        $('#btn-add-contact').click(function (e) {
            FormDynamic.addContact();
        });
        //End:: Button Add Contact Person

        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = $("form#form-company").cmserializeToJSON();

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            $.ajax({
                type: "POST",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/finance/CompanyProfile/save"),
                data: data,
                success: function (r) {
                    if (r && r.status.success) {
                        history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                        //$('input[name="id"]').val(r.data.recordID);
                        toastr.success(r.status.message, "Information");
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


        });
        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
                initLookup(),
                    loadDetailData();
            }
        };
    }();
    $(document).ready(function () {
        pageFunction.init();

    });
}(jQuery));
