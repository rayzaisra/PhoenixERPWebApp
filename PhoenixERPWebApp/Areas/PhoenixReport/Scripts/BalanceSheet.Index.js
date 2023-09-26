
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
        });


        //BEGIN::EVENT BUTTON
        $('#btn-view').click(function () {
            var params = $('#form-search').serializeToJSON();
            var _legalEntity = (($legalEntityLookup.val() || null) == null ? '' : $legalEntityLookup.val());
            window.open($.helper.resolve("~/PhoenixReport/Viewer?reportName=BalanceSheet.mrt&legal_entity_id=" + _legalEntity ), "_blank");

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


