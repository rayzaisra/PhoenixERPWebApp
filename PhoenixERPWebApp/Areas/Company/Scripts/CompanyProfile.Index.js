(function ($) {
    'use strict';
    var pageFunction = function () {

        var counter = {};
  

        /* Create and log the result. */
        //BEGIN::EVENT BUTTON
        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = $("form#form-company").cmserializeToJSON();
            console.log(data);
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

        //Begin:: Function Add Form
        function addForm(data,callBack) {
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

            $(row).attr({
                "class": "row " + data[2],
                "data-idx": num
            }).html(dataRow);

            $(data[0]).append(row);
            
            $('div.' + data[2] + '[data-idx="' + num + '"] button.btn-remove-bank').off('click').on('click', function () {
                var idx = $(this).attr('data-index');

                if ($('div.' + data[2]).length > 1) {
                    $('div.' + data[2] + '[data-idx="' + idx + '"]').remove();
                    counter[data[2]] = $('div.' + data[2]).length;
                }               
            });

            // bwt generate select2 dan plugin yg lainnya
            if (callBack) {
                callBack(num,data);
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


        var initFormDynamic = function () {
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
                        text: 'currency_symbol',
                    }
                });
            });
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
            });
        };


        //End:: Document Ready

        //Begin:: Button Add Bank
        $('#btn-add-bank').click(function (e) {
            if (counter["counter-contact"] && counter["counter-bank"] < 4) {
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
                            text: 'currency_symbol',
                        }
                    });
                });
            } else {
                swal('', 'Maximum Of 4 Data', 'info');
            }
        });
        //End:: Button Add Bank

        //Begin:: Button Add Contact Person
        $('#btn-add-contact').click(function (e) {
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
                });
            } else {
                swal('', 'Maximum Of 4 Data', 'info');
            }
        });
        //End:: Button Add Contact Person

   
        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
                initLookup();
                initFormDynamic();
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


