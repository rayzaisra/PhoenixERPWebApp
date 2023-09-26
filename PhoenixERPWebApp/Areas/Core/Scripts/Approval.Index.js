(function ($) {
    'use strict'; 
    var pageFunction = function () {
     
        //BEGIN::EVENT BUTTON
        $('.link-approval').each(function (i, e) {
            var entityID = $(this).attr("entity_id");
            var title = $(this).attr("title");
            $(this).click(function () {
                window.location.href = $.helper.resolve("~/Core/Approval/Details?entity_id=" + entityID + "&Title=" + title);
            });
        });

        //END::EVENT BUTTON


        return {
            init: function () {
                //initialize
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


