var ajaxUrlGet = API + "/ProjectManagement/Job/get";
var ajaxUrlassigncentral = API + "/ProjectManagement/job/assigncentral";
var ajaxUrlListPM = API + "/ProjectManagement/Job/Listpm";
var ajaxUrlGetCentral = API + "/General/CentralResource/List";
var xform = "JobassigncentralForm";   
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
        setFormAction(xform, ajaxUrlassigncentral)
        if (data.pm_id) {
            $('#user_id').append('<option value="' + data.pm_id + '" selected>' + data.pm +'</option>')
        }
        fnJob.setupcentral(data);
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
    },
	setupcentral: function (data) {
        ajaxGet(ajaxUrlGetCentral, {}, function (response) {
            response = parseJson(response);
            $("#centralresourcetable tbody").empty();
            var strct = "";
            var xno = 0;
 
            $.each(response.rows, function (key, value) {
				xcheck="";
				if(data.centralresource){
					var xsist=data.centralresource.find(x => x.id === value.id )
					if(xsist){
						xcheck="checked";
					}
					
				}
				
                xno++;
                strct += '<tr>';
                strct += '<td>' + xno + '</td>';
                strct += '<td><div class="m-checkbox-list"><label class="m-checkbox "><input type = "checkbox" '+xcheck+' class="cr_id" name="central_resource_id[]" value="' + value.id + '" data-name="' + value.name +'" >'+value.name +'<span></span ></label ></div></td>';

                strct += '</tr >';
            });
            $("#centralresourcetable tbody").append(strct);
        });
    }
 
};
 
$(function () {
    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#Jobassigncentralinit").hide();
                $("#Jobassigncentralcontent").show();
                fnJob.setupjobform(response.data);

            } else {
                $("#Jobassigncentralcontent").hide();
            }
            

        });
    }

   
});

