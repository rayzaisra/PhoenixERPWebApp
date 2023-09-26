(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
         

        };
        pageFunction.popUpRef = function (e) {
            e.preventDefault();
          
            $.ajax({
                type: 'GET',
                url: param.indexRefUrl,
                
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
      
        
      
    });
}(jQuery));