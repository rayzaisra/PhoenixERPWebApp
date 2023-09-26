(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
            
            
        };
        pageFunction.popUpRef = function (e) {
            e.preventDefault();
            var aptype = 1;
            //if ($('#APType_4').is(':checked')) {
            //    aptype = $('#APType_4').val();
            //} else if ($('#APType_3').is(':checked')) {
            //    aptype = $('#APType_3').val();
            //} else if ($('#APType_2').is(':checked')) {
            //    aptype = $('#APType_2').val();
            //} else if ($('#APType_1').is(':checked')) {
            //    aptype = $('#APType_1').val();
            //}
            $.ajax({
                type: 'GET',
                url: param.indexRefUrl,
                data: {
                    aptype: aptype,
                    VendorId: $('#VendorId').val()
                },
                success: function (result) {
                    $('#refModal').html(result);

                    $('#refModal').modal('show');
                    mApp.unblock("#m_blockui_list");
                },
                error: function (e, t, s) {
                    console.log(e)
                    mApp.unblock("#m_blockui_list");
                }
            }).then(setTimeout(function () {
                mApp.unblock("#m_blockui_list");
            }, 2e3));
        };
        pageFunction.attachment = function (e) {
            e.preventDefault();
            $.ajax({
                type: 'GET',
                url: param.attachmentUrl,
                success: function (result) {
                    $('#refModal').html(result);

                    $('#refModal').modal('show');
                    mApp.unblock("#m_blockui_list");
                },
                error: function (e, t, s) {
                    console.log(e)
                    mApp.unblock("#m_blockui_list");
                }
            }).then(setTimeout(function () {
                mApp.unblock("#m_blockui_list");
            }, 2e3));
        };
        pageFunction.save = function () {
            //callAction({ formId: "form", title: "save", type: "POST", url: param.saveUrl });
            var details = $("#grid").data("kendoGrid").dataSource.data().toJSON();
            var list = [];
            for (var i = 0; i < details.length; i++) {
                var z = JSON.parse(JSON.stringify(details[i]));
                list.push(z);
            }

            callActionWithDetails({ formId: "form", title: "save", type: "POST", url: param.saveUrl, objects: list });

        };

        //pageFunction.submit = function () {
        //    //callAction({ formId: "form", title: "save", type: "POST", url: param.saveUrl });
        //    var details = $("#grid").data("kendoGrid").dataSource.data().toJSON();
        //    var list = [];
        //    for (var i = 0; i < details.length; i++) {
        //        var z = JSON.parse(JSON.stringify(details[i]));
        //        list.push(z);
        //    }

        //    callActionWithDetails({ formId: "form", title: "save", type: "POST", url: param.saveUrl, objects: list });

        //};
        pageFunction.taxVerification = function () {

            swal({
                title: "Confirmation",
                text: 'Are you sure want to verify the tax?',
                type: "question",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (ok) {
                swal('Information', result.message, 'success')
                                    .then(function (ok) {
                                        if (ok.value) {
                                            if (result.url !== undefined) window.location = result.url;
                                        }
                                    });
                //if (ok.value) {
                //    mApp.block("#m_blockui_list", {
                //        overlayColor: "#000000",
                //        type: "loader",
                //        state: "primary",
                //        message: "Processing..."
                //    });
                //    var formData = new FormData();
                //    var params = $(parameter.formId).serializeArray();

                //    var data = {};
                //    $(params).each(function (index, obj) {
                //        data[obj.name] = obj.value;
                //    });
                //    data.details = parameter.objects;
                //    //var a = {
                //    //    TrMediaOrderTVDetails: parameter.objects
                //    //};
                //    //$.each(params, function (i, val) {
                //    //    formData.append(val.name, val.value);
                //    //});
                //    //if (parameter.objects !== undefined) {
                //    //    formData.append(parameter.objectName, parameter.objects);
                //    //}
                //    var url = parameter.url;
                //    if (url === undefined) url = $(parameter.formId).attr('action');

                //    $.ajax({
                //        type: parameter.type,
                //        url: url,
                //        data: data,
                //        success: function (result) {
                //            if (result.success) {
                //                swal('Information', result.message, 'success')
                //                    .then(function (ok) {
                //                        if (ok.value) {
                //                            if (result.url !== undefined) window.location = result.url;
                //                        }
                //                    });
                //            } else {
                //                swal('Error', result.message, 'error');
                //            }
                //            mApp.unblock("#m_blockui_list");
                //        },
                //        error: function (e, t, s) {
                //            swal('Error', 'Ooops, something went wrong !', 'error');
                //            mApp.unblock("#m_blockui_list");
                //        }
                //    }).then(setTimeout(function () {
                //        mApp.unblock("#m_blockui_list");
                //    }, 2e3));
                //}
            }).catch(swal.noop);

        };
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $('#btn-PopUpRef').click(pageFunction.popUpRef);
        $('#btn-save').click(pageFunction.save);
        $('#btn-attachment').click(pageFunction.attachment);
        $('#btn-taxVerification').click(pageFunction.taxVerification);
       
        if ($('#Id').val() != null || $('#Id').val() != "" || $('#Id').val() != "undefined") {
            //add default value forinv detail
            $("#NetAmount").data('kendoNumericTextBox').value(0);
            $("#ValueAddedTax").data('kendoNumericTextBox').value(0);
            $("#ValueAddedTaxOn").data('kendoNumericTextBox').value(0);
            $("#ValueAddedTaxPercent").data('kendoNumericTextBox').value(0);
            $("#TotalCostVendor").data('kendoNumericTextBox').value(0);
            $("#PromptPayment").data('kendoNumericTextBox').value(0);
            $("#PromptPayment2").data('kendoNumericTextBox').value(0);
            $("#TaxPayable").data('kendoNumericTextBox').value(0);
            $("#TaxPayableOn").data('kendoNumericTextBox').value(0);
            $("#TaxPayablePercent").data('kendoNumericTextBox').value(0);
            $("#OtherDiscount").data('kendoNumericTextBox').value(0);
            $("#AdvertisingTax").data('kendoNumericTextBox').value(0);
        }
    });
}(jQuery));