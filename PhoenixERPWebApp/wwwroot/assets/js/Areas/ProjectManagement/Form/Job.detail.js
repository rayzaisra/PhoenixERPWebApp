var ajaxUrlGet = API + "/ProjectManagement/Job/get";
var ajaxUrlGetCentral = API + "/General/CentralResource/List";
var xform = "JobDetailForm";   
fnJob = {
    setupjobform: function (data) {
        var optionsForm = GetOptionsForm(function () {
            var xret = $("#" + xform).valid();
            if (xret) startprocess();
            return xret;
        }, function (response, statusText, xhr, $form) {
            endprocess();
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_updatesuccess);

            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xform, optionsForm);
        $("#accountmanagerlist").empty();
        if (data.accountmanager) {
            $.each(data.accountmanager, function (key, value) {

                $("#accountmanagerlist").append('<li>' + value + '</li>');
            });
        }
        $("#clientbrieflink").attr("href", WEB +"ProjectManagement/clientbrief/job/"+data.client_brief_id)
        $("#centralresourcelist").empty();
        if (data.centralresource) {
            $.each(data.centralresource, function (key, value) {
                $("#centralresourcelist").append('<li>' + value.name + '</li>');
            });
        }

        if (data.teamMember) {
            generateuserlist(data.teamMember);
        }
        initlanguage(lang_pm);
        FormTextByData(data, xform);
    },
    centralchange: function () {
        var crid = $(".cr_id");
        $("#jobtasksrc").empty();
        $('input[type=checkbox].cr_id').each(function () {
            if (this.checked) {
                console.log($(this).val() + ":" + $(this).attr("data-name"));
                settask($(this).attr("data-name"));
            }
        });
    }
    
 
};
function generateuserlist(data) {
    $("#TeamusersList tbody").empty();
 
    astr = '';
    cstr = '';
    $.each(data, function (key, row) {
        vstr = '<tr>';

        vstr += '<td>' + row.username + '</td>';
        vstr += '<td>' + row.email + '</td>';
        vstr += '<td>' + row.act_as_name + '</td>';
       
        vstr += '</tr>';
    });
    $("#TeamusersList tbody").append(astr);

}
$(function () {
    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#jobdetailinit").hide();
                $("#jobdetailcontent").show();
                fnJob.setupjobform(response.data);

            } else {
                show404();
            }
            

        });
    }

   
});

