var ajaxUrl = API + "Hris/TrainingRequisition";
var ajaxUrlList = ajaxUrl + "/GetTrainingBonding";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/DeleteDetail";

var tableList = "TrainingBondingList";
var formSearch = "TrainingBondingFormSearch";
var modalSearch = "TrainingBondingModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
}
var columnsIndex = [
    {
        "data": null,
        "title": "#",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return meta.row + meta.settings._iDisplayStart + 1;
        }
    }, {
        "data": "code",
        "title": "Code",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var datastring = '';
            if (row.status === "0" || row.status === 0) {
                datastring = '<a class="fakelink" href="/Hris/TrainingRequisition/Approval/' + row.id + '">' + data + '</a>';
            } else {
                datastring = data;
            }
            return datastring;
        }
    }    , {
        "data": null,
        "title": "Request By",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return row.detialTrainingRequester.request_by;
        }
    }, {
        "data": null,
        "title": "Job Title",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return row.detialTrainingRequester.job_title_name;
        }
    }, {
        "data": null,
        "title": "Grade",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            return row.detialTrainingRequester.job_grade_name;
        }
    }  ,{
        "data": "traning_name",
        "title": "Training Name",
        "sClass": "",
        orderable: true
    }, {
        "data": "training_type",
        "title": "Training Type",
        "sClass": "",
        orderable: true
    }, {
        "data": "start_date",
        "title": "Start Date",
        "sClass": "",
        orderable: true
    }, {
        "data": "end_date",
        "title": "End Date",
        "sClass": "",
        orderable: true
    } ,
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
            retval += '<a class="dropdown-item fakelink" href="/Hris/TrainingRequisition/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
            retval += '<a class="dropdown-item" onClick="fnTrainingRequisition.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
            retval += '</div>';
            retval += '</span>';
            return retval;

        }
    }   
];

fnTrainingRequisition = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'id', 'asc');
        setcolvis(tableList, columnsIndex);
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        $("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableList].ajax.reload();
        $("#" + modalSearch).modal('hide');
    },
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
                        fnTrainingRequisition.reloadlist();
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
    fnTrainingRequisition.generatelist();
    initlanguage(lang_hr);
});
 

