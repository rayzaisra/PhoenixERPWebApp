(function ($) {
    'use strict';
    
    var pageFunction = function () {
        var _detailID = $('#preview_approval_template_id');
        var $templateBuilder = $('#builder-basic');
        $templateBuilder.queryBuilder({
            allow_groups: 1,
            plugins: {
                'unique-filter': null,
                'not-group': true,
                'sortable': true,
            },
            conditions: ["AUTO"],
            operators: [
                'equal', 'contains'
            ],
            filters: [
                {
                    id: 'application_user',
                    label: 'User Employee',
                    type: 'string',
                    //plugin: 'select2',
                    //input: 'select',
                    //valueSetter: function (rule, value) {
                    //    console.log("value setter");
                    //    return value;
                    //    //var $el = $("#" + rule.id).find(".rule-value-container select");
                    //    //$el.cmSetLookup(rule.__.data.approver_id, value);
                    //}
                }
            ]
        });
        jQuery('.rules-list btn-group btn-group-sm pull-right group-actions').hide();
        var loadTemplate = function () {
            if (_detailID.val() != '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Get Template ..."
                });
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~/core/ApprovalTemplate/template?approvaltemplateid=" + _detailID.val()),
                    success: function (r) {
                        if (r.status.success && r.data) {
                            $templateBuilder.queryBuilder('setRules', r.data);
                            
                            jQuery('.rules-list btn-group btn-group-sm pull-right group-actions').hide();
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
        }


        return {
            init: function () {
                //initialize
                loadTemplate();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


