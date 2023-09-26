var ajaxUrlGet = API + "/ProjectManagement/dam/GetByJob";
var ajaxUrlfiletmp = API + "/ProjectManagement/dam/GetDamTypelist";
var ajaxUrlListjob = API + "/ProjectManagement/Job/List"; 
var ajaxUrlmanage = API + "/ProjectManagement/job/setstatus";
var xform = "PCAForm";
fnJob = {
    
    getdetil: function (value) {
        fnPanel.startprocess();
        ajaxGet(ajaxUrlGet + "/" + value, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#showdetil").show();
                fnJob.setupjobform(response.data);
            }
            fnPanel.endprocess();
        });
    },
    setupjobform: function (data) {
        
        current_job_id = data.id;
 
        FormTextByData(data, xform);
        $("#setform").show();
        $("input[name=job_status_id][value=9]").attr('checked', 'checked');
        pickstatus();
        $("#accountmanagerlist").empty();
        if (data.accountmanager) {
            $("#accountmanagerlist").append('<ul>');
            $.each(data.accountmanager, function (key, value) {

                $("#accountmanagerlist").append('<li>' + value + '</li>');
            });
            $("#accountmanagerlist").append('</ul>');
        }
 
    }

};

function pickstatus() {
    var job_status_id = $("input[name='job_status_id']:checked").val();
    $("#damfilediv").empty();
    contentno = 0;
    $.each(statusdamtype, function (key, item) {
        if (item.job_status_id === job_status_id) {

        var sname = item.job_status_name;
        vstr = '<div class="form-group">';
            vstr += '<label> ' + item.dam_type_name + '  :</label>';
            vstr += '<input type="hidden"  name="content[' + contentno + '][id]" />';
            vstr += '<input type="hidden" value="' + item.dam_type_id + '"  name="content[' + contentno + '][dam_type_id]" />';
            vstr += '<input type="text" id="contentfile' + contentno + '"   name="content[' + contentno + '][file_id]" />';
        vstr += '</div>';
        $("#damfilediv").append(vstr);
        $('#contentfile' + contentno).setuploadbox();
        contentno++;

        }
    });
}  
  
var statuslist = [];
var statusdamtype;
$(function () {
    ajaxGet(ajaxUrlfiletmp, {}, function (response) {
        response = parseJson(response);
        if (response.success) {
            statusdamtype = response.rows;
            $.each(statusdamtype, function (key, item) {
                var sname = item.job_status_name;
                if ($.inArray(sname, statuslist) < 0) {
                    statuslist.push(sname);
                    vstr = '<label class="m-radio">';
                    vstr += '<input type="radio" onclick="pickstatus()"  name= "job_status_id" value="' + item.job_status_id + '">' + sname;
                    vstr += '<span></span></label>';
                    $("#jobstatusdiv").append(vstr);
                }
                
            });
        }
 
    });

    setFormAction(xform, ajaxUrlmanage);
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
    $('#job_detail_id').combogrid({
        panelWidth: 500,
        fitColumns: false,
        delay: 1000,
        dataparam: { 'rows': 100 },
        idField: 'job_number',
        textField: 'job_name',
        url: ajaxUrlListjob,
        searchparam:'search_job_name',
        mode: 'remote',
        method: 'get',
        onSelect: function (row,data) {
            var value = data.id;
            fnJob.getdetil(value);

        },
        columns: [[
            { field: 'job_number', title: 'ID', width: 150 },
            { field: 'job_name', title: 'Name', width: 150 },
            { field: 'description', title: 'description', width: 150}
        ]],
 
    });
   
});

