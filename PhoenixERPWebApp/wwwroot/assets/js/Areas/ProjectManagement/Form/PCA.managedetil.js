var ajaxUrlGet = API + "/ProjectManagement/Job/getPCA";
var ajaxUrlassignpm = API + "/ProjectManagement/job/assignpm";
var ajaxUrlListjob = API + "/ProjectManagement/Job/List";
var xform = "PCAForm";
var xtable = "tabledetilpca";
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
        setFormAction(xform, ajaxUrlassignpm)
        if (data.pm_id) {
            $('#user_id').append('<option value="' + data.pm_id + '" selected>' + data.pm + '</option>')
        }
       
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
   
        FormTextByData(data, xform);
        $("#" + xtable + " tbody").empty();
        if (data.tasklist) {
            var vstr = "";
            $.each(data.tasklist, function (key, item) {

                vstr += '<tr>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>' + item.task_name;
                vstr += '<a class="pull-right"  href="#"><i class="fa fa-play"></i></a>';
                vstr += ' <a class="pull-right" href="#"><i class="fa fa-plus"></i></a>';
                
                vstr +=  '</td>';
                vstr += '<td>1</td>';
                vstr += '<td><select name="x"><option></option></select></td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '<td>&nbsp;</td>';
                vstr += '</tr>';
            });

            
            $("#" + xtable + " tbody").append(vstr);
        }
    }

};

$(function () {
    $('#job_id').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListjob,
        searchparam: 'search',
        labelField: 'job_number',
        valueField: 'job_number',
    });
    $('#job_id').on("select2:selecting", function (e) {
        var value = e.params.args.data.id;
        console.log(value);
        ajaxGet(ajaxUrlGet + "/" + value, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
 
                fnJob.setupjobform(response.data);
 
 
            }


        });
    });

});

