var ajaxUrlGet = API + "/ProjectManagement/Job/get";
var ajaxUrlassignpm = API + "/ProjectManagement/job/assignpm";
var ajaxUrlListPM = API + "/ProjectManagement/Job/Listpm";
var xform = "JobAssignpmForm";   
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
                xid = $("#" + xform + ' [name="id"]').val();
                convault(null, WEB + "/projectmanagement/job/detail/" + xid);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xform, optionsForm);
        setFormAction(xform, ajaxUrlassignpm)
        if (data.pm_id) {
            $('#user_id').append('<option value="' + data.pm_id + '" selected>' + data.pm +'</option>')
        }
        $('#user_id').setcombobox({
            data: {
                'rows': 100,
                'sortBy': 'app_fullname',
                'sortDir':'asc'
            },
            url: ajaxUrlListPM,
            searchparam: 'search',
            labelField: 'app_fullname',
            valueField: 'user_id',
            render: function (data) {
                if (data.odata) {
                    var stateNo = mUtil.getRandomInt(0, 7);
                    var states = [
                        'success',
                        'brand',
                        'danger',
                        'accent',
                        'warning',
                        'metal',
                        'primary',
                        'info'];
                    var state = states[stateNo];
                    return '<div class="m-card-user m-card-user--sm">' +
                        '<div class="m-card-user__pic">' +
                        '<div class="m-card-user__no-photo m--bg-fill-' + state + '"><span>' + data.odata.app_fullname.substring(0, 1) + '</span></div>' +
                        '</div>' +
                        '<div class="m-card-user__details">' +
                        '<span class="m-card-user__name">' + data.odata.app_fullname + '</span>' +
                        '<a href="javascript:return;" class="m-card-user__email m-link">' + data.odata.app_username + '</a>' +
                        '</div></div>';
                }
            }
        });
        $("#accountmanagerlist").empty();
        if (data.accountmanager) {
            $("#accountmanagerlist").append('<ul>');
            $.each(data.accountmanager, function (key, value) {

                $("#accountmanagerlist").append('<li>' + value + '</li>');
            });
            $("#accountmanagerlist").append('</ul>');
        }
        if (data.pm) {
            $("#pmname").text(data.pm.app_fullname);
        }
        initlanguage(lang_pm);
        FormTextByData(data, xform);
    }
 
};
 
$(function () {
    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#JobAssignpminit").hide();
                $("#JobAssignpmcontent").show();
                fnJob.setupjobform(response.data);

            } else {
                $("#JobAssignpmcontent").hide();
            }
            

        });
    }

   
});

