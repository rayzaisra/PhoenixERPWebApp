function calculateTotalAmount() {
    var amount = Number($('input[name=amount]').val()||0);
    var discountAmount = Number($('input[name=discount]').val() || 0);
    t = amount - discountAmount,
        $('#total_final_amount').text(t.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0);
}

//function setAmount(dataItem) {
//    dataItem.amount =  dataItem.qty * (dataItem.unit_price - dataItem.discount_amount);
//}


function getAmount(dataItem) {
    return dataItem.qty * (dataItem.unit_price - dataItem.discount_amount);
}


function calculateVAT(dataItem) {
    dataItem.tax_base = 0.1;
    var amount = getAmount(dataItem);
    var VAT = amount * dataItem.tax_base;
    dataItem.tax_vat = VAT || 0;
    return dataItem.tax_vat.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0;
}

function calculateAmount(dataItem) {
    dataItem.amount = getAmount(dataItem);
    return (dataItem.amount || 0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") || 0;
}


function itemCodeEditable(dataItem) {
    return dataItem.type_name == "Inventory" ? true : false;
}
(function ($) {
    'use strict';

    var _detailID = $('#detail-id');
    var gridUI = $('#grid');

    var pageFunction = function () {
        var _formRequestForQuotation = $('#form-RequestForQuotation');
        var $month = $("#month");
        var $year = $("#year");
        var $closing = $("#closing");
        var $legal = $("#legal");


        //BEGIN::Form Validation
        _formRequestForQuotation.validate({
            rules: {
                month: {
                    required: true
                },
                year: {
                    required: true
                },
                closing: {
                    required: true
                },
                legal: {
                    required: true
                },
                
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
                mUtil.scrollTo(_formRequestForQuotation, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation


        //BEGIN::Lookup

        $month.kendoDropDownList({
            placeholder: "Choose Month",
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Januari", value: "1" },
                { text: "Februari", value: "2" },
                { text: "Maret", value: "3" },
                { text: "April", value: "4" },
                { text: "Mei", value: "5" },
                { text: "Juni", value: "6" },
                { text: "Juli", value: "7" },
                { text: "Agustus", value: "8" },
                { text: "Oktober", value: "9" },
                { text: "September", value: "10" },
                { text: "November", value: "11" },
                { text: "Desember", value: "12" },
            ],
            change: function (e) {
              

            }
        });

        $year.kendoComboBox({
            placeholder: "Choose Year",
            dataTextField: "text",
            dataValueField: "value",
            filter: "contains",
            dataSource: [
                { text: "2018", value: "2018" },
                { text: "2019", value: "2019" },
                { text: "2020", value: "2020" },
                { text: "2021", value: "2021" },
               
            ],
            
        });

        $closing.kendoComboBox({
            placeholder: "Choose Category",
            dataTextField: "text",
            dataValueField: "value",
            filter: "contains",
            dataSource: [
                { text: "Cashback", value: "1" },
                { text: "Account Payable", value: "2" },
                { text: "Account Receiveable", value: "3" },
                { text: "Purchase Order", value: "4" },
                { text: "Project Management", value: "5" },
                { text: "Human Receiveable", value: "6" },
               
            ],
            
        });

        $legal.kendoComboBox({
            placeholder: "Choose Legal",
            dataTextField: "text",
            dataValueField: "value",
            filter: "contains",
            dataSource: [
                { text: "Iris", value: "1" },
                { text: "Advis", value: "2" },
                { text: "Interface", value: "3" },
                { text: "Skor", value: "4" },
                { text: "Nava Plus", value: "5" },
                
               
            ],
            
        });
        //END::Load Request For Quotation Detail

        var formControll = function () {
            $('#btn-save').removeAttr('disabled').show();

            if (_detailID.val() !== '') {
                $('#btn-approval').removeAttr('disabled', '').show();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();

                if ($('input[name="approval_id"]').val() !== '') {
                    $('#btn-approval').attr('disabled', 'disabled').hide();
                    $('#btn-remove_approval').removeAttr('disabled', '').show();

                    $('#btn-submit').removeAttr('disabled', '').show();
                }

            } else {
                $('#btn-approval').attr('disabled', 'disabled').hide();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();
            }
            $('#tb').show();
        };

        var formControllSubmited = function (submited) {
            if (!submited) {
                $('#tb').show();
            }
            else {
                $('#tb').hide();
                $('#btn-save').attr('disabled', 'disabled').hide();
                $('#btn-approval').attr('disabled', 'disabled').hide();
                $('#btn-remove_approval').attr('disabled', 'disabled').hide();
                $('#btn-submit').attr('disabled', 'disabled').hide();
            }
        };

        var setStatus = function (state) {
            var output = '';
            switch (state) {
                case 'new':
                    output = `<span class="m-badge m-badge--info m-badge--wide">New</span>`;
                    break;
                case 'draft':
                    output = `<span class="m-badge m-badge--warning m-badge--wide">Draft</span>`;
                    break;
                case 'submited':
                    output = `<span class="m-badge m-badge--danger m-badge--wide">Submited</span>`;
                    break;
                case 'approved':
                    output = `<span class="m-badge m-badge--success m-badge--wide">Approved</span>`;
                    break;
            }
            $('#status').html(output);
        };

        $('#btn-save').click(function () {
           
        });

        $('#btn-submit').click(function () {
            var btn = $(this);
           

        });

        $('#btn-remove_approval').click(function () {
            var btn = $(this);
        
        });

        $('#btn-approval').click(function () {
       
        });

     
        return {
            init: function () {
                //initialize
            //    loadDetailData()
            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));