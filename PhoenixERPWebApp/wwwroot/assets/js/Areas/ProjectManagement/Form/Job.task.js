 
$(function () {
    
    loadScript([WEB + "/assets/plugins/muuri/web-animations.min.js",WEB + "/assets/plugins/muuri/hammer.min.js",WEB + "/assets/plugins/muuri/muuri.min.js"]);

    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#Jobassignboardinit").hide();
                $("#Jobassignboardcontent").show();
                fnJob.setupjobform(response.data);

            } else {
                $("#Jobassignboardcontent").hide();
            }
            

        });
    }

   
});

