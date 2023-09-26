var ajaxUrlGet = API + "/ProjectManagement/Job/get";
var ajaxUrl = API + "/ProjectManagement/Timeline/ListByJobNumber";
var ajaxUrljoblist = API + "/ProjectManagement/job/List";
var g_job = Array();
var xform = "JobTimeline";  
fnJob = {
    setupjobform: function (data) {
   
        $("#accountmanagerlist").empty();
        if (data.accountmanager) {
            $("#accountmanagerlist").append('<ul>');
            $.each(data.accountmanager, function (key, value) {

                $("#accountmanagerlist").append('<li>' + value + '</li>');
            });
            $("#accountmanagerlist").append('</ul>');
        }
        
        initlanguage(lang_pm);
        FormTextByData(data, xform);
    }

};
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
 
 
    ajaxGet(ajaxUrl + "/" + RecordId, {}, function (response) {
        response = parseJson(response);
        if (response.rows) {
            //---------top hierarki
            $.each(response.rows, function (key, item) {
                var xf = g_job.find(x => x.id === item.central_resource_id);
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
                    if (xd=== undefined) {
 
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
                        'id': item.central_resource_id,
                        'name': item.central_resource_name,
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
    var xbottom = parseISOString(lowerdate);
    var xtop = parseISOString(upperdate);
    xbottom = xbottom.addDays(-10);
    xtop = xtop.addDays(30);
    $("#gantt_chart").empty();
    $("#gantt_chart").ganttView({
        data: ganttData,
        startDate: getformatdate(xbottom.toISOString()),
        endDate: getformatdate(xtop.toISOString()),
        behavior: {
            resizable: true,
            draggable: true,
            clickable: true,
            onClick: function (data) {
                console.log("You clicked on an event: " + "\n", data);
            },
            onResize: function (data) {
                console.log('You resized an event: ' + "\n", data);
                startTime = moment(data.start).format("YYYY-MM-DD HH:mm:ss");
                endTime = moment(data.end).format("YYYY-MM-DD HH:mm:ss");
                console.log(startTime + '-' + endTime);
            },
            onDrag: function (data) {
                console.log("You dragged an event: " + "\n", data);
                startTime = moment(data.start).format("YYYY-MM-DD HH:mm:ss");
                endTime = moment(data.end).format("YYYY-MM-DD HH:mm:ss");
                console.log(startTime + '-' + endTime);
            }
        }
    });
}
$(function () {
    ajaxGet(ajaxUrlGet + "/" + RecordId, {}, function (response) {
        response = parseJson(response);
        if (response.success) {
            $("#divxloader").hide();
            $("#divxactive").show();
            fnJob.setupjobform(response.data);

        } else {
            $("#divxactive").hide();
        }


    });
    getdata();


})