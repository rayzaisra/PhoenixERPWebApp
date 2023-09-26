var ajaxUrl = API + "Hris/TrainingRequisition";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/DeleteDetail";

var tableList = "TrainingRequisitionList";
var formSearch = "TrainingRequisitionFormSearch";
var modalSearch = "TrainingRequisitionModalSearch";
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
        "data": "purpose_start_date",
        "title": "Start Date",
        "sClass": "",
        orderable: true
    }, {
        "data": "purpose_end_date",
        "title": "End Date",
        "sClass": "",
        orderable: true
    }, {
        "data": "venue",
        "title": "Location",
        "sClass": "",
        orderable: true
    }, {
        "data": null,
        "title": "Participant",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = 0;

            $.each(row.listParticipantTraining, function (index, value) {
                dataraw = dataraw + 1;
            });
            var datastring = '';
            //if (row.posting_status === "4") {
            //    datastring = '<a href="/Hris/Applicant/TrackingNewApplicant/' + row.participantTraining.id + '">' + "<center>" + dataraw + "</center>" + '</a>';

            //} else {
            //    datastring = "<center>" + dataraw + "</center>";
            //}


            return "<center>" + dataraw +"</center>";
        }
    }, {
        "data": null,
        "title": "Status",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row, meta) {
            var dataraw = "";
            if (row.status ==0 && row.statusReal == 1) {
                dataraw = "<span class='bluetext'>Need to Approved</span>";
            } else if (row.status == 0 && row.statusReal == 2) {
                dataraw = "<span>Waiting for Approval</span>";
            } else if (row.status == 0 && row.statusReal == 3) {
                dataraw = "<span class='greentext'>Approved</span>";
            } else if (row.status == 0 && row.statusReal == 4) {
                dataraw = "<span class='redtext'>Reject</span>";
            } else if (row.status == 1 && row.statusReal == 3) {
                dataraw = "SUBMIT TO FINANCE";
            } else if (row.status == 2) {
                dataraw = "Training in Progress";
            }else {
                dataraw = "PROCURMENT PROCESS";
            }


            return dataraw;
        }
    }  ,
    {
        "data": null,
        "title": "<span class='translate'  data-args='app-action'>Actions</span>",
        "sClass": "actioncol",
        orderable: false,
        "render": function (data, type, row) {
            if (row.status == 0) {
                retval = '';
                retval += '<span class="dropdown">';
                retval += '<a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">';
                retval += '<i class="la la-ellipsis-h"></i>';
                retval += '</a>';
                retval += '<div class="dropdown-menu dropdown-menu-right">';
                if (row.statusReal == 3 && row.status == 0) {
                    retval += '<a class="dropdown-item fakelink" href="/Hris/TrainingRequisition/GetWithDetial/' + row.id + '"><i class="la la-inbox"></i> Register</a>';
                } else {
                    retval += '<a class="dropdown-item fakelink" href="/Hris/TrainingRequisition/update/' + row.id + '"><i class="la la-edit"></i> Details</a>';
                    retval += '<a class="dropdown-item" onClick="fnTrainingRequisition.delete(\'' + row.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i> Delete</a>';
                }
                retval += '</div>';
                retval += '</span>';
            } else {
                retval = "";
            }
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
 

