/*
 Using file for Dashboard....By M.Salih RIsmadi
 */
let getSectionGeneral = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/GetDsGeneral");
    mApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: "Processing..."
    }),
        $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            console.log(data);
            // Count TimeSheet DS....
            $('.sp-th-tm-ds').text(data.currentTotal);
            $('.sp-tp-tm-ds').text(data.percentageTime);
            if (parseInt(data.percentageTime) > 100) $('.pb-tm-ds').attr('style', 'width:100%');
            else $('.pb-tm-ds').attr('style','width:'+data.percentageTime+'%');

            // Count Leave DS....
            $('.sp-user-tl-ds').text(data.usedLeave);
            $('.sp-tl-lv-ds').text(data.totalLeave);

            var pctage = (parseInt(data.usedLeave) * 100) / parseInt(data.totalLeave)
            $('.pgb-tl-ds').attr('style','width:'+pctage+'%');
    
            },
            error: function (e, t, s) {
                //swal(
                //    'Information',
                //    'Ooops, something went wrong !',
                //    'info'
                //);
                console.log(e);
            }
        }).then(setTimeout(function () {
            mApp.unblockPage();
        }, 355));

};

let getSectionKanban = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/GetDsKanban");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            drawKanban(data);
        },
        error: function (e, t, s) {
            console.error('get info dashboard kanban getting error');
        }
    });
};

let drawKanban = function (data) {
    var div = $('.divDraw-kanban');
    div.empty();
    const bg = ["bg-primary", "bg-success", "bg-info", "bg-warning", "bg-danger", "bg-dark"];

    $.each(data, function (x, r) {
        const ranBg = bg[Math.floor(Math.random() * bg.length)];
        var temp = '<div class="col-lg-4 col-xs-4" style="padding-bottom:15px"> ' +
             '     <div class="card" style="background:#fff !important">'+
            '         <div class="card-header '+ranBg+' text-white">' + r.name + '</div>' +
            '         <div class="card-body">' + drawDetailJob(r.listJobDetails) + '</div>' +
             '     </div>'+
            ' </div>';

        div.append(temp);
    });
};

let drawDetailJob = function (data) {
    var temp = '';
    $.each(data, function (x, r) {
        temp = temp + '<div class="col-md-12">' +
            '                 <div class="alert alert-primary" role="alert">' + r.jobName + ' </div>' +
            '             </div>';
    });
    return temp;
};

let getSectionGrafik = function () {
};

let getSectionSummary = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/GetDsSummary");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            drawSummary(data);
        },
        error: function (e, t, s) {
            console.error('get info dashboard kanban getting error');
        }
    });
};

let drawSummary = function (data) {
    $(".lblHiring").html(data.totalHiring);
    $(".lblPromotion").html(data.totalPromotion);
    $(".lblSanction").html(data.totalSanction);
    $(".lblTraining").html(data.totalTraining);
    $(".lblEndOfContract").html(data.totalEndOfContract);
    $(".lblJobOpen").html(data.jobOpen);
    $(".lblJobInProgress").html(data.jobInProgress);
    $(".lblJobDelivered").html(data.jobDelivered);
    $(".lblJobClosed").html(data.jobClosed);
    $(".lblRFQ").html(data.totalRFQ);
    $(".lblPO").html(data.totalPO);
    $(".lblGR").html(data.totalGR);
    $(".lblpmApproved").html(data.totalPmApproved);
    $(".lblpmRejected").html(data.totalPmRejected);
    $(".lblpmWaiting").html(data.totalPmWaiting);
    $(".lblpmtask").html(data.totalPmTask);
    $(".lblpmoverdue").html(data.totalPmOverdue);
    $(".lblpminprogress").html(data.totalPmInprogress);

};

let getSectionPMApproval = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/GetDsPmApproval");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            drawPmApproval(data);
        },
        error: function (e, t, s) {
            console.error('get info dashboard kanban getting error');
        }
    });
};

let drawPmApproval = function (data) {
    
    var count = 0;
    $.each(data, function (x, r) {
        var html = '';
        if (count < 5) {
            html = '<div class="col-md-12" style="padding-bottom:10px;padding-top:10px; border:1px solid #ddd">' +
                "<a href='javascript: newTabToDetailNotif(\"" + r.refid + "\",\"" + r.detaillink + "\")' title='click here to see detail'><div><p>" + r.text + "</p></div></a>" +
                '</div>';
            $('#pm-approval-list-body').append(html);
        }
        count++;
    });

};

let getSectionPMMyTask = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/GetDsPmMyTask");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            drawPMMyTask(data);
        },
        error: function (e, t, s) {
            console.error('get info dashboard kanban getting error');
        }
    });
};

let drawPMMyTask = function (data) {
    var count = 0;
    $.each(data, function (x, r) {
        var html = '';
        if (count < 5) {
            html = '<div class="col-md-12" style="padding-bottom:10px;padding-top:10px; border:1px solid #ddd">' +
                "<a href='javascript: newTabToDetailNotif(\"" + r.refid + "\",\"" + r.detaillink + "\")' title='click here to see detail'><div><p>" + r.text + "</p></div></a>" +
                '</div>';
            $('#pm-my-task-body').append(html);
        }
        count++;
    });

};

var columnsIndexDashboardPM = [
    {
        "data": "text",
        "title": "My Task Name",
        "sClass": "lcol",
        orderable: true
    }
];

function ViewListNotificationPmMyTask() {
    $("#ListDashboardPmMyTaskModal").modal('show');
    generatelistPmMyTask();
}

function ViewListNotificationPmApproval() {
    $("#ListDashboardPmApprovalModal").modal('show');
    generatelistPmApproval();
}

function generatelistPmMyTask() {
    var ListData = [];
    if (!$.fn.DataTable.isDataTable('#tableListDashboardPmMyTask')) {
        $('#tableListDashboardPmMyTask').DataTable({
            "ajax": function (data, callback, settings) {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~notifycenter/GetDsPmMyTask"),
                    success: function (data) {
                        $.each(data, function (x, r) {
                            var obj = {};
                            obj["text"] = "<a href='javascript: newTabToDetailNotif(\"" + r.refid + "\",\"" + r.detaillink + "\")' title='click here to see detail'><div><p>" + r.text + "</p></div></a>";
                            ListData.push(obj);

                        });
                        callback({
                            data: ListData,
                            recordsTotal: data.length,
                            recordsFiltered: 0
                        });
                    }
                });
            },
            "columns": columnsIndexDashboardPM
        });
    }


}

function generatelistPmApproval() {
    var ListData = [];
    if (!$.fn.DataTable.isDataTable('#tableListDashboardPmApproval')) {
        $('#tableListDashboardPmApproval').DataTable({
            "ajax": function (data, callback, settings) {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~notifycenter/GetDsPmApproval"),
                    success: function (data) {
                        $.each(data, function (x, r) {
                            var obj = {};
                            obj["text"] = "<a href='javascript: newTabToDetailNotif(\"" + r.refid + "\",\"" + r.detaillink + "\")' title='click here to see detail'><div><p>" + r.text + "</p></div></a>";
                            ListData.push(obj);

                        });
                        callback({
                            data: ListData,
                            recordsTotal: data.length,
                            recordsFiltered: 0
                        });
                    }
                });
            },
            "columns": columnsIndexDashboardPM
        });
    }


}


$(function () {
    getSectionGeneral();
    getSectionKanban(); 
    getSectionSummary();
    getSectionPMApproval();
    getSectionPMMyTask();
});
