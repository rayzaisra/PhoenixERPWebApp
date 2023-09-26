var ajaxUrl = API + "/ProjectManagement/Timeline/ListTimeLine";
var ajaxUrljoblist = API + "/ProjectManagement/job/List";
var g_job = Array();
function xnu(myNumber) {
    return ("0" + myNumber).slice(-2)
}
function getformatdate(xdate) {
    var sdate = new Date(xdate);
    xsdate = xnu(sdate.getMonth() + 1) + '/' + xnu(sdate.getDate()) + '/' + sdate.getFullYear();
    return xsdate;
}
var lowerdate = null;
var upperdate = null;
function getdata() {
    g_job = Array();
    var searchdata = $("#timelinefilter").serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name != "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });
    ajaxGet(ajaxUrl, data, function (response) {
        response = parseJson(response);
        if (response.rows) {
            //---------top hierarki
            $.each(response.rows, function (key, item) {
                var xf = g_job.find(x => x.id === item.jobNumber);
                if (lowerdate === null) {
                    lowerdate = item.taskStartDate;
                } else {
                    ldate = new Date(lowerdate);
                    sdate = new Date(item.taskStartDate);
                    if (ldate > sdate) {
                        lowerdate = item.taskStartDate;
                    }
                }
                if (upperdate === null) {
                    upperdate = item.taskEndDate;
                } else {
                    ldate = new Date(upperdate);
                    sdate = new Date(item.taskEndDate);
                    if (ldate < sdate) {
                        upperdate = item.taskEndDate;
                    }
                }
                if (xf !== undefined) {

                    var xd = xf.series.find(x => x.id === item.taskId);
                    if (xd !== undefined) {

                    } else {
                        xf.series.push({
                            id: item.taskId,
                            name: item.taskName,
                            title: item.taskName,
                            start: getformatdate(item.taskStartDate),
                            end: getformatdate(item.taskEndDate), color: "#0277BD"
                        })
                    }
                } else {
                    g_job.push({
                        'id': item.jobNumber,
                        'name': item.jobName,
                        'series': [{
                            id: item.taskId,
                            name: item.taskName,
                            title: item.taskName,
                            start: getformatdate(item.taskStartDate),
                            end: getformatdate(item.taskEndDate), color: "#0277BD"
                        }]
                    });
                }
            });

            setgantchart(g_job)
        }

    });
}
function setgantchart(ganttData) {
    $("#gantt_chart").empty();
    $("#gantt_chart").ganttView({
        data: ganttData,
        startDate: getformatdate(lowerdate),
        endDate: getformatdate(upperdate),
        behavior: {
            resizable: false,
            draggable: false,
            clickable: true,
            onClick: function (data) {
                console.log("You clicked on an event: " + "\n", data);
            },
            onResize: function (data) {
                console.log('You resized an event: ' + "\n", data);
            },
            onDrag: function (data) {
                console.log("You dragged an event: " + "\n", data);
            }
        }
    });
}
$(function () {
    $('.dtpicker').setdatepicker();
    $('#timelinefilter #jobs').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrljoblist,
        maxItems: 0,
        searchparam: 'search_job_name',
        labelField: 'job_name',
        valueField: 'id',
    });


})