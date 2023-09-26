
(function ($) {
    'use strict';
    
    var $legalEntityLookup = $("#legal_entity_id");
    var $accountLookup = $("#account_id");

    var pageFunction = function () {

        $legalEntityLookup.cmSelect2({
            url: $.helper.resolveApi("~/core/LegalEntity/lookup"),
            filters: ' r.legal_entity_name like @0 ',
            result: {
                id: 'id',
                text: 'legal_entity_name'
            }
        }).on('change', function () {
            var legalEntityId = $legalEntityLookup.val();
            $accountLookup.removeAttr("disabled");
            $accountLookup.cmSelect2({
                url: $.helper.resolveApi("~/Finance/Account/lookup"),
                maximumSelectionLength: 2,
                filters: ' ( r.account_name like @0 OR r.account_number like @0 ) ' +
                    ' AND r.legal_entity_id = @1 AND r.account_level = 5 ',
                parameters: [legalEntityId],
                result: {
                    id: 'id',
                    text: 'account_name'
                }
            });

        });


        //BEGIN::EVENT BUTTON
        $('#btn-view').click(function () {
            var params = $('#form-search').serializeToJSON();
            var _legalEntity = (($legalEntityLookup.val() || null) == null ? '' : $legalEntityLookup.val());
            var _account = (($accountLookup.val() || null) == null ? '' : $accountLookup.val());
            window.open($.helper.resolve("~/PhoenixReport/Viewer?reportName=General%20Ledger.mrt&legal_entity_id=" + _legalEntity + "&account_id=" + _account), "_blank");

        });
        
        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
            }
        };
    }();



    $(document).ready(function () {
        //pageFunction.init();
    });
}(jQuery));


