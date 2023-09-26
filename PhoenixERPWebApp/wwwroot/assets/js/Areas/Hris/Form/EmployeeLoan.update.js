
var webUrl = WEB + "/Hris/EmployeeLoan";
var ajaxUrl = API + "/Hris/EmployeeLoan";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlGet = ajaxUrl + "/Get";
var xform = "EmployeeLoanFormUpdate";
var ajaxUrlListSalaryComponent = API_HRIS + "/LoanCategory/GetWithDetil";

$(function () {

    $('#loan_category_id').setcombobox({
        data: { 'rows': 100, 'name': 'name' },
        url: ajaxUrlListSalaryComponent,
        //url: ajaxUrlList,
        searchparam: 'id',
        labelField: 'name',
        valueField: 'id',
        searchField: 'name'
    });

    $("#date_loan").datepicker({
        todayHighlight: !0,
        autoclose: !0,
        pickerPosition: "bottom-left",
        format: "mm/dd/yyyy"
    })

    $("#date_repaymentstartdate").datepicker({
        todayHighlight: !0,
        autoclose: !0,
        pickerPosition: "bottom-left",
        format: "mm/dd/yyyy"
    })

    var xid = $("#" + xform + ' [name="id"]').val();
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
            var record = response.data;

            //alert(record.salary_component_id);
            FormLoadByDataUsingName(response.data, xform);
            $('#loan_category_id').cmSetLookup(record.loan_category_id, record.loan_category_name);
            $('#date_loan').val(record.date_loan);
            //alert(record.date_loan);
            //var currentdate = new Date();

            //$('#date_loan').val($.datepicker.formatDate('dd-M-y', currentdate));
           // $('[name="date_loan"]').val(record.date_loan);
            //alert(record.date_loan);
        });
    }





});

