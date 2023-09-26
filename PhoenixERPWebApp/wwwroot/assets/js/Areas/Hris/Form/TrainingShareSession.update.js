var webUrl = WEB + "/Hris/TrainingRequisition";
var ajaxUrl = API + "Hris/TrainingRequisition";
var ajaxUrlUpdate = ajaxUrl + "/UpdateDetial";
var xform = "TrainingRequisitionFormUpdate";
var ajaxUrlGet = ajaxUrl + "/GetWithDetial"; 

var ajaxUrlTrainingList = API + "Hris/Training/List";
$(function () {
    var xid = $("#" + xform +' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            setFormAction(xform, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                var xret = $("#" + xform).valid();
                if (xret) startprocess();
                return xret;
            }, function (response, statusText, xhr, $form) {
                endprocess();
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    redirecttolink(webUrl);
                } else {
                    DangerNotif(response.message);
                }
            });
            InitForm(xform, optionsForm);
            setFormAction(xform, ajaxUrlUpdate);
            initlanguage(lang_hr);
            FormLoadByDataUsingName(response.data, xform);

            $('#training_idu').append("<option value=" + response.data.training_id + ">" + response.data.traning_name + "</option>");
            $('#training_idu').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlTrainingList,
                maxItems: 0,
                searchparam: 'Search_name',
                labelField: 'name',
                valueField: 'id'
            });

            if (response != null) {
                $.each(response.data.listParticipantTraining, function (index, value) {
                    CheckAndAddRow(value.employee_basic_info_id, value.name_employee, value.job_title_name, value.job_grade_name, value.participantBusinessUnit.name);
                });
            }
        });
    }
    


    
    
});
  
