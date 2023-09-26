var ajaxUrl = API + "/Hris/SubDeductions";
var ajaxUrlList = API_HRIS + "/SubDeductions/List";
var ajaxUrlList2 = API_HRIS + "/SubDeductions/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlListSalaryComponent = API_HRIS + "/SubDeductions/GetWithDetil";
var ajaxUrlListOrganization = API_GENERAL + "/BusinessUnit/ListWithParent";


var tableList = "SubDeductionsList";
var formCreate = "SubDeductionsFormCreate";
var formUpdate = "SubDeductionsFormUpdate";
var formSearch = "SubDeductionsFormSearch";
var modalCreate = "SubDeductionsModalCreate";
var modalUpdate = "SubDeductionsModalUpdate";
var modalSearch = "SubDeductionsModalSearch";
var pageheader = "";
var $salarycomponentid = $("#salary_component_id");
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name != "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
}
var columnsIndex = [
    {
        "data": null,
        "title": "No",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "salary_component",
        "title": "<span class='translate'  data-args='salary_component'>Salary Component</span>",
        "sClass": "",
        orderable: true
    }
    , {
        "data": "name",
        "title": "<span class='translate'  data-args='name'>Name</span>",
        "sClass": "",
        orderable: true
    }, {
        "data": "description",
        "title": "<span class='translate'  data-args='description'>Description</span>",
        "sClass": "",
        orderable: true
    },
    {
        "data": "created_on",
        "title": "<span class='translate'  data-args='created_on'>Created Date</span>",
        "sClass": "",
        orderable: false,
        "render": function (data, type, row) {
            return moment(row.created_on).locale('id').format('LL');
        }
    },
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            retval = '';
            retval += '<span class="dropdown">';
            retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
            retval += '<i class="la la-ellipsis-h"></i>';
            retval += '</a>';
            retval += '<div class="dropdown-menu dropdown-menu-right">';
            retval += '<a class="dropdown-item fakelink" href="/Hris/SubDeductions/update/' + row.id + '"><i class="la la-edit"></i> Edit</a>';
            retval += '<a class="dropdown-item" onClick="fnSubDeductions.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;
            //return '<a href="javascript:void(0)" onClick="fnSubSalaryComponent.update(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnSubSalaryComponent.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
            //return '<a href="\'' + $.helper.resolve('~/Hris/SubSalaryComponent/Get/?id=\'' + row.id +'\')" ><i class="md-icon material-icons uk-text-primary">edit</i></a><a href="javascript:void(0)" onClick="fnSubSalaryComponent.delete(\'' + row.id + '\')"><i class="md-icon material-icons uk-text-danger">delete</i></a>';
        }
    }
];

fnSubDeductions = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex);
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        UIkit.modal("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x == "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();
        UIkit.modal("#" + modalSearch).hide();
    },

    /*create: function () {
        var optionsForm = GetOptionsForm(function () {
            return $("#" + formCreate).parsley().isValid();
        }, function (response, statusText, xhr, $form) {
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_createsuccess);
                fnSubSalaryComponent.reloadlist();
                UIkit.modal("#" + modalCreate).hide();
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(formCreate, optionsForm);
        setFormAction(formCreate, ajaxUrlCreate);
        UIkit.modal("#" + modalCreate).show();
    
    update: function (id) {
        ajaxGet(ajaxUrlGet + "/" + id, {}, function (response) {
            response = parseJson(response);

            UIkit.modal("#" + modalUpdate).show();
            setFormAction(formUpdate, ajaxUrlUpdate);
            var optionsForm = GetOptionsForm(function () {
                return $("#" + formUpdate).parsley().isValid();
            }, function (response, statusText, xhr, $form) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_updatesuccess);
                    fnSubSalaryComponent.reloadlist();
                    UIkit.modal("#" + modalUpdate).hide();
                } else {
                    DangerNotif(response.message);
                }
            });
            InitForm(formUpdate, optionsForm);
            FormLoadByDataUsingName(response.data, formUpdate);
        });
    },*/
    /*delete: function (id) {
        var text = t_delete;
        confirmDialog(text, function () {
            ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_deletesuccess);
                    fnSubSalaryComponent.reloadlist();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    */
    delete: function (id) {
        var text = t_delete;
        swal({
            title: "Confirmation",
            text: t_delete,
            type: "warning",
            showCancelButton: !0,
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            reverseButtons: !0
        }).then(function (e) {
            if (e.value) {
                ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                    response = parseJson(response);
                    if (response.success) {
                        SuccessNotif(t_deletesuccess);
                        fnSubDeductions.reloadlist();
                        hideAllModal();
                    } else {
                        DangerNotif(response.message);
                    }
                });
            }


        });

    }
};

$(function () {
    //alert(ajaxUrlList);
    fnSubDeductions.generatelist();
    initlanguage(lang_hr);


    /*$('#salary_component_id').setcombobox({
        data: { 'rows': 100, 'salary_component': 'salary_component' },
        url: ajaxUrlListSalaryComponent,
        //url: ajaxUrlList,
        searchparam: 'id',
        labelField: 'salary_component',
        valueField: 'id',
        searchField: 'salary_component'
    });*/
});

