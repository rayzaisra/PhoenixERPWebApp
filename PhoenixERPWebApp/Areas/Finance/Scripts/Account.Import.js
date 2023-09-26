(function ($) {
    'use strict';
    var pageFunction = function () {
        //var _detailID = $('#detail-id').val();
        var _formAccount = $('#form-Account');
        var $legalEntity = $("#legal_entity_id");
        var legalEntityId = $legalEntity.val();

        //BEGIN Lookup

        $legalEntity.cmSelect2({
            url: $.helper.resolveApi("~/Core/LegalEntity/Lookup"),
            filters: ' r.legal_entity_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        }).on('change', function (e) {
            legalEntityId = $legalEntity.val();
        });

        //END Lookup

        //BEGIN::Form Validation

        _formAccount.validate({
            rules: {
                legal_entity_id: {
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
                mUtil.scrollTo(_formAccount, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        $('#btn-save').click(function (e) {
            var btn = $(this);

            var fileData = new FormData();
            fileData.append('legal_entity_id', $('#legal_entity_id').val());

            var files = $("#imported_file").get(0).files;
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
                if (files[i].size > 1000000) {
                    alertError('Max. allowed size is 1 MB');
                    return;
                }

                // select first file only
                break;
            }

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            console.log(fileData);

            $.ajax({
                type: "POST",
                contentType: false,
                processData: false,
                url: $.helper.resolveApi("~/finance/Account/Upload"),
                data: fileData,
                success: function (r) {
                    console.log(r);
                    if (r && r.status.success) {
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
            }).then(function () {
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            });
        });


        //BEGIN::EVENT BUTTON
        //do stuff ...
        //END::EVENT BUTTON


        //BEGIN::EVENT BUTTON

        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
                //loadDetailData();
            }
        };
    }();


    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));