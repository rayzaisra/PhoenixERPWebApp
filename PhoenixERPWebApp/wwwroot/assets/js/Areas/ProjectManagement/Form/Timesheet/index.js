var ajaxUrl = API + "/ProjectManagement/TimesheetNonJob";
var ajaxUrltsnonjob = API + "ProjectManagement/TimesheetNonJob/List";

//var ajaxUrlJobList = API + "ProjectManagement/Job/List";


//var ajaxUrlJobList = API + "ProjectManagement/JobUser/List";
var ajaxUrlJobList = API + "ProjectManagement/TimesheetUser/ListAllJob";

//TimesheetUser
var ajaxurltimesheet = API + "ProjectManagement/TimesheetUser/List";

var webUrl = API + "ProjectManagement/TimesheetUser";
var WebIndex = API + "ProjectManagement/TimesheetUser/entry";

var ajaxUrlCreateTimesheet = API + "ProjectManagement/TimesheetUser/Create";
var ajaxUrlDelete = API + "ProjectManagement/TimesheetUser/Delete";
//var ajaxUrlDelete = API + "ProjectManagement/TimesheetUser//Delete";

var xform = "TimesheetTaskCreate";
var yform = "TimesheetCreate";
//Task
var ajaxTaskUrl = API + "/ProjectManagement/Task";
var ajaxUrlTasklList = ajaxTaskUrl + "/List";
var ajaxUrlTasklListFilter = ajaxTaskUrl + "/TaskFilter";


fnTimesheet = {
    getJobLList: function () {


        var xlayoutdetiltmp = $("#tmpl_acc_item").html();
        ajaxGet(ajaxUrltsnonjob, { 'rows': 1000, 'active': true }, function (response) {
            categorylist = response.rows;
            vstr = "";
            $.each(response.rows, function (key, item) {

                var xlayoutdetil = xlayoutdetiltmp;

                xlayoutdetil = replaceAll(xlayoutdetil, "{{id}}", item.id);
                xlayoutdetil = replaceAll(xlayoutdetil, "{{title}}", item.name);
                vstloopdata = "";
                if (item.detail) {
                    vstloopdata += '<table class="table  table-hover" id="tb1">';
                    //new
                    vstloopdata += '<input type="hidden" value="' + item.id + '" name="mark" />';
                    //end(new)
                    $.each(item.detail, function (key, itemtask) {
                        vstloopdata += '<tr id="tr">';
                        vstloopdata += '<td style="width:90%">';
                        vstloopdata += itemtask.task_name;
                        vstloopdata += '</td>';
                        vstloopdata += '<td>';
                        vstloopdata += '<input type="checkbox" value="' + itemtask.category_id + '" id="category_id" name="category_id">';
                        vstloopdata += '</td>';
                        vstloopdata += '</tr>';
                    });
                    vstloopdata += '</table>';
                }
                xlayoutdetil = replaceAll(xlayoutdetil, "{{loopdata}}", vstloopdata);
                vstr += xlayoutdetil;

            });
            $("#acclist").html(vstr);
        });


    },

    generatelist: function (dtax) {
        moment.updateLocale('en', {
            week: {
                dow: 1,
            },
        })
        var tsdate = GetValueOrDefault($('#to_date').val(), "");
        if (tsdate === "") {

            tsdate = currentdate;

        };
        var curdate = moment(currentdate, "YYYY-MM-DD");
        var cdate = moment(tsdate, "YYYY-MM-DD");
        var disdate = cdate.add(1, 'days');
        var cyear = cdate.format('YYYY');
        var startdate = cdate.startOf('week');
        var enddate = cdate.endOf('week');
        var weekdate = cdate.startOf('week') + "-" + cdate.year();

        var dtable = $("#timesheettable");
        $("#timesheettable thead").empty();
        var vstr = '<tr><th  rowspan="2" colspan="3">&nbsp;</th><th colspan="7"><center>' + cyear + '</center></th></tr>';
        vstr += '<tr>';
        var ndate = startdate.clone();
        for (var iw = 0; iw < 7; iw++) {
            var hrn = ndate.weekday();
            var isWeekend = (hrn === 6) || (hrn === 5);
            xclass = "";
            if (isWeekend) {
                xclass = "sabming";
            }
            vstr += '<th class="w50px ' + xclass + '"><input name="mark_date" type="hidden" value="' + ndate.format('DD-MM-YYYY') + '">' + ndate.format('DD MMM') + '</th>';
            ndate = ndate.add(1, 'days');
        };


        vstr += '</tr> ';
        $("#timesheettable thead").append(vstr);


        //generate task

        var odatax;
        var oi;

        var tsdata = [];
        //var tsdata = [
        //    { 'id': '0', 'category': 'Meeting', 'task': 'General meeting', 'detail': [] },
        //    { 'id': '1', 'category': 'XXX', 'task': 'xxx', 'detail': [] },
        //    { 'id': '2', 'category': 'XXX', 'task': 'xxx', 'detail': [] },
        //    { 'id': '3', 'category': 'XXX', 'task': 'xxx', 'detail': [] },
        //    { 'id': '4', 'category': 'XXX', 'task': 'xxx', 'detail': [] },
        //    { 'id': '5', 'category': 'XXX', 'task': 'xxx', 'detail': [] },
        //    { 'id': '6', 'category': 'XXX', 'task': 'xxx', 'detail': [] }];
        //console.log(tsdata);

        //vstr = '';
        //$("#timesheettable tbody").empty();


        //vstr = '';
        //$("#timesheettable tbody").empty();

        //$.each(tsdata, function (key, item) {

        //    vstr += '<tr>';
        //    vstr += '<td><h6><b>' + item.category + '</b> ' + item.task + '</h6><br /></td>';

        //    var ndate = startdate.clone();
        //    console.log("current", curdate.format('MMDD'));
        //    console.log(item.category_name);
        //    for (var iw = 0; iw < 7; iw++) {

        //        xclass = "";
        //        var showinput = true;
        //        var cid = ndate.format('MMDD');
        //        var cod = ndate.format('YYYY-MM-DD');

        //        if (ndate.isAfter(curdate)) {
        //            showinput = false;
        //        }
        //        var hrn = ndate.weekday();
        //        var isWeekend = (hrn === 6) || (hrn === 5);
        //        xclass = "";
        //        if (isWeekend) {
        //            xclass = "sabming";
        //        }

        //        if (showinput) {
        //            vstr += '<td class="' + xclass + ' timewrap"><input type="hidden" value=' + cod + '> <input type="number" id="ipt_' + item.id + '_' + cid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
        //        } else {
        //            vstr += '<td class="' + xclass + ' timewrap">&nbsp;</td>';
        //        }

        //        ndate = ndate.add(1, 'days');
        //    }
        //    vstr += '</tr>';
        //});

        //$("#timesheettable tbody").append(vstr);
        //$('.xtime').mask('00', { reverse: true });
        //hitungtotal();
        dtax = $('#to_date').val();

        ajaxGet(ajaxurltimesheet + "/" + dtax, {'rows': 1000, 'active': true }, function (response) {
            response = parseJson(response);
           
            vstr = '';
            $("#timesheettable tbody").empty();

            $.each(response.rows, function (key, item) {
                console.log(item);
                vstr += '<tr><input type="hidden" name="id" value="'+ item.id +'">';
               
                vstr += '<td><h6><b class="job-id">' + item.job_id + '</b> ' + item.job_name + '</h6><br /><span class="remarks">' + item.remark + '</span></td>';
                vstr += '<td class="align-middle"><a class="btn btn-secondary btn-icon btn-circle"><i class="la la-play-circle"></i></a></td>';
                vstr += '<td class="align-middle"><div class="btn-group"><a class="m-btn btn btn-sm btn-primary"><i class="la la-edit"></i></a><a class="m-btn btn btn-sm btn-danger" onClick="fnTimesheet.delete(\'' + item.id + '\')" href="javascript:void(0)"><i class="la la-trash"></i></a></div></td>';
                var ndate = startdate.clone();
                console.log("current", curdate.format('MMDD'));
                console.log(item.category_name);
                for (var iw = 0; iw < 7; iw++) {

                    //$.each(item.time, function (key, sItem) {

                    //});

                    xclass = "";
                    var showinput = true;
                    var cid = ndate.format('MMDD');
                    var cod = ndate.format('YYYY-MM-DD');

                    if (ndate.isAfter(curdate)) {
                        showinput = false;
                    }
                    var hrn = ndate.weekday();
                    var isWeekend = (hrn === 6) || (hrn === 5);
                    xclass = "";
                    if (isWeekend) {
                        xclass = "sabming";
                    }

                    //var u = item.time[iw].id;
                    //var h = item.time[iw].timesheet_hour;
                    
                    



                    if (showinput) {
                        if (typeof item.time[iw] === "undefined") {

                            vstr += '<td class="' + xclass + ' timewrap slice"><input type="hidden" name="id"><input type="hidden" name="date" value=' + cod + '> <input type="number" id="ipt__' + cid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
                        }
                        else {
                            vstr += '<td class="' + xclass + ' timewrap slice"><input type="hidden" name="id" value="' + item.time[iw].id + '"><input type="hidden" name="date" value=' + cod + '> <input type="number" value="' + item.time[iw].timesheet_hour + '" id="ipt_' + item.id + '_' + cid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
                        }
                        
                        
                    } else {
                        vstr += '<td class="' + xclass + ' timewrap">&nbsp;</td>';
                    }

                    ndate = ndate.add(1, 'days');
                }
                vstr += '</tr>';
            });

            $("#timesheettable tbody").append(vstr);

            $('#addBtn').on('click', function () {
                var out = {
                    job_id: $("#job_number option:selected").val(),
                    job_name: $('#job_number option:selected').text(),
                    start_date_weekly: $('#dateStart').val(),
                    end_date_weekly: $('#dateEnd').val(),
                    Non_job: $("#nonjobname").val(),
                    remarks: $("#remarks").val(),
                    //timesheet_date: $(".datepick").val(),
                    //timesheet_hour: $(".timepick").val()
                };

                var isValidate = $('#TimesheetTaskCreate').valid();

                if (isValidate) {
                    var xvstr = '';
                    xvstr += '<tr><input type="hidden" name="id"><input type="hidden" name="startdate" value="' + out.start_date_weekly + '"><input type="hidden" name="enddate" value="' + out.end_date_weekly + '">';

                    xvstr += '<td><h6><b class="job-id">' + out.job_id + '</b> ' + out.job_name + '</h6><br /><span class="remarks">' + out.remarks + '</span></td>';
                    xvstr += '<td class="align-middle"></td>';
                    xvstr += '<td class="align-middle"><div class="btn-group"></div></td>';
                    var ndate = startdate.clone();
                    console.log("current", curdate.format('MMDD'));

                    for (var iw = 0; iw < 7; iw++) {

                        //$.each(item.time, function (key, sItem) {

                        //});

                        xclass = "";
                        var showinput = true;
                        var cid = ndate.format('MMDD');
                        var cod = ndate.format('YYYY-MM-DD');

                        if (ndate.isAfter(curdate)) {
                            showinput = false;
                        }
                        var hrn = ndate.weekday();
                        var isWeekend = (hrn === 6) || (hrn === 5);
                        xclass = "";
                        if (isWeekend) {
                            xclass = "sabming";
                        }

                        if (showinput) {
                            xvstr += '<td class="' + xclass + ' timewrap slice"><input type="hidden" name="id"><input type="hidden" name="date" value=' + cod + '> <input type="number"  id="ipt_' + cid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
                        } else {
                            xvstr += '<td class="' + xclass + ' timewrap">&nbsp;</td>';
                        }

                        ndate = ndate.add(1, 'days');
                    }
                    xvstr += '</tr>';

                        $("#timesheettable tbody").append(xvstr);
                        $('#categorylist').modal('hide');
                        out = {
                        job_id: $("#job_number").val(''),
                        job_name: $("#job_number").text(''),
                        Non_job: $("#nonjobname").val(''),
                        remarks: $("#remarks").val(''),
                        timesheet_date: $(".datepick").val(''),
                        timesheet_hour: $(".timepick").val('')
                    };
                }

                //var xvstr = '';
                //xvstr += '<tr><input type="hidden" name="id">';

                //xvstr += '<td><h6><b class="job-id">' + out.job_id + '</b> ' + out.job_name + '</h6><br /><span class="remarks">' + out.remarks + '</span></td>';
                //xvstr += '<td class="align-middle"></td>';
                //xvstr += '<td class="align-middle"><div class="btn-group"></div></td>';
                //var ndate = startdate.clone();
                //console.log("current", curdate.format('MMDD'));
              
                //for (var iw = 0; iw < 7; iw++) {

                //    //$.each(item.time, function (key, sItem) {

                //    //});

                //    xclass = "";
                //    var showinput = true;
                //    var cid = ndate.format('MMDD');
                //    var cod = ndate.format('YYYY-MM-DD');

                //    if (ndate.isAfter(curdate)) {
                //        showinput = false;
                //    }
                //    var hrn = ndate.weekday();
                //    var isWeekend = (hrn === 6) || (hrn === 5);
                //    xclass = "";
                //    if (isWeekend) {
                //        xclass = "sabming";
                //    }

                //    if (showinput) {
                //        xvstr += '<td class="' + xclass + ' timewrap slice"><input type="hidden" name="id"><input type="hidden" name="date" value=' + cod + '> <input type="number"  id="ipt_' + cid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
                //    } else {
                //        xvstr += '<td class="' + xclass + ' timewrap">&nbsp;</td>';
                //    }

                //    ndate = ndate.add(1, 'days');
                //}
                //xvstr += '</tr>';

                //$("#timesheettable tbody").append(xvstr);
                //$('#categorylist').modal('hide');
                //out = {
                //    job_id: $("#job_number").val(''),
                //    job_name: $("#job_number").text(''),
                //    Non_job: $("#nonjobname").val(''),
                //    remarks: $("#remarks").val(''),
                //    timesheet_date: $(".datepick").val(''),
                //    timesheet_hour: $(".timepick").val('')
                //};


            });

            $('.xtime').mask('00', { reverse: true });
            hitungtotal();
        });



        ////origin
        //$.each(getck, function (key, item) {
        //    vstr += '<tr>';
        //    vstr += '<td><b>' + item.category + '</b><br /><small>' + item.task + '</small></td>';

        //    var ndate = startdate.clone();
        //    console.log("current", curdate.format('MMDD'));
        //    console.log(item.category_name);
        //    for (var iw = 0; iw < 7; iw++) {

        //        xclass = "";
        //        var showinput = true;
        //        var cid = ndate.format('MMDD');

        //        if (ndate.isAfter(curdate)) {
        //            showinput = false;
        //        }
        //        var hrn = ndate.weekday();
        //        var isWeekend = (hrn === 6) || (hrn === 5);
        //        xclass = "";
        //        if (isWeekend) {
        //            xclass = "sabming";
        //        }

        //        if (showinput) {
        //            vstr += '<td class="' + xclass + ' timewrap"> <input type="number" id="ipt_' + item.id + '_' + cid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
        //        } else {
        //            vstr += '<td class="' + xclass + ' timewrap">&nbsp;</td>';
        //        }

        //        ndate = ndate.add(1, 'days');
        //    }
        //    vstr += '</tr>';
        //});


        //$("#timesheettable tbody").append(vstr);
        //$('.xtime').mask('00', { reverse: true });
        //hitungtotal()
        ////end-origin
    },
    opencategorylist: function () {
        
        $("#categorylist").modal('show');
        
    },
    changeWeek: function () {



        $('#previous_week').click(function () {

            var new_date = moment($('#to_date').val(), "YYYY-MM-DD");
            //var new_date = moment(currentdate, "YYYY-MM-DD");

            var day = new_date.subtract(1, 'week').format('DD');
            var month = new_date.format('MM');
            var year = new_date.format('YYYY');

            var pervious = year + '-' + month + '-' + day;


            $('#to_date').val(pervious).change();


            //console.log();

        });

        $('#next_week').click(function () {

            var new_date = moment($('#to_date').val(), "YYYY-MM-DD");
            //var new_date = moment(currentdate, "YYYY-MM-DD");
            //ctg

            var day = new_date.add(1, 'week').format('DD');
            var month = new_date.format('MM');
            var year = new_date.format('YYYY');

            var pervious = year + '-' + month + '-' + day;


            $('#to_date').val(pervious).change();


            console.log();

        });

        $('#this_week').click(function () {


            var new_date = moment(currentdate, "YYYY-MM-DD");

            var day = new_date.format('DD');
            var month = new_date.format('MM');
            var year = new_date.format('YYYY');

            var pervious = year + '-' + month + '-' + day;


            $('#to_date').val(pervious).change();


            console.log();

        });

        


    },
    addNewTask: function () {
        var out = {
            job_id: $("#job_number option:selected").val(),
            job_name: $('#job_number option:selected').text(),
            Non_job: $("#nonjobname").val(),
            remarks: $("#remarks").val(),
            timesheet_date: $(".datepick").val(),
            timesheet_hour: $(".timepick").val()
        };

        moment.updateLocale('en', {
            week: {
                dow: 1
            }
        });
        var tsdate = GetValueOrDefault($('#to_date').val(), "");
        if (tsdate === "") {

            tsdate = currentdate;

        };
        var curdate = moment(currentdate, "YYYY-MM-DD");
        var cdate = moment(tsdate, "YYYY-MM-DD");
        var disdate = cdate.add(1, 'days');
        var cyear = cdate.format('YYYY');
        var startdate = cdate.startOf('week');
        var enddate = cdate.endOf('week');
        var weekdate = cdate.startOf('week') + "-" + cdate.year();

        var dtable = $("#timesheettable");
        //$("#timesheettable thead").empty();
        //var vstr = '<tr><th  rowspan="2">&nbsp;</th><th colspan="7"><center>' + cyear + '</center></th></tr>';
        //vstr += '<tr>';
        var ndate = startdate.clone();
        //for (var iw = 0; iw < 7; iw++) {
            //var hrn = ndate.weekday();
            //var isWeekend = (hrn === 6) || (hrn === 5);
            //xclass = "";
            //if (isWeekend) {
            //    xclass = "sabming";
            //}
            //vstr += '<th class="w50px ' + xclass + '">' + ndate.format('DD MMM') + '</th>';
            //ndate = ndate.add(1, 'days');
        //}


        vstr += '<tr id=""><input type="hidden" name="id">';

        vstr += '<td><input type="hidden" name="job_id" value=' + out.job_id + '><input type="hidden" name="remark" value=' + out.remarks + '><h6><b>' + out.job_id + ' </b><span class="job_name">' + out.job_name + '</span></h6></br><span class="remarks">' + out.remarks + '</span></td>';
        vstr += '<td class="align-middle"><a class="btn btn-secondary btn-icon btn-circle"><i class="la la-play-circle"></i></a></td>';
        vstr += '<td class="align-middle"><div class="btn-group"></div></td>';

        xclass = "";
        var showinput = true;
        var cid = ndate.format('MMDD');
        var cod = ndate.format('YYYY-MM-DD');

        if (ndate.isAfter(curdate)) {
            showinput = false;
        }
        var xhrn = ndate.weekday();
        var xisWeekend = (xhrn === 6) || (xhrn === 5);

        for (var iw = 0; iw < 7; iw++) {

            xclass = "";
            var xshowinput = true;
            var xcid = ndate.format('MMDD');
            var xcod = ndate.format('YYYY-MM-DD');

            if (ndate.isAfter(curdate)) {
                showinput = false;
            }
            var hrn = ndate.weekday();
            var isWeekend = (hrn === 6) || (hrn === 5);
            xclass = "";
            //if (isWeekend) {
            //    xclass = "sabming";
            //}

            if (xshowinput) {
                vstr += '<td class="' + xclass + 'timewrap slice"><input type="hidden" name="id"><input type="hidden" name="date" value=' + xcod + '> <input type="number" id="ipt_' + out.job_id + '_' + xcid + '" class="xtime' + iw + ' xtime form-control form-control-sm m-input m-input--square" placeholder="-" ></td>';
            } else {
                vstr += '<td class="' + xclass + 'timewrap">&nbsp;</td>';
            }

            ndate = ndate.add(1, 'days');
        }

        vstr += '</tr>';
 
        

        $("#timesheettable tbody").append(vstr);



    },

    addNewTimesheet: function () {
        var day = [];
        var pout = {};
        var pts = [];

        $('#timesheettable tbody tr').each(function () {
            var id = $(this).find('input[name=id]').val();
            var jid = $(this).find('td').find('.job-id').text();
            var remark = $(this).find('td').find('.remarks').text();
            var startdate = $(this).find('input[name=startdate]').val();
            var enddate = $(this).find('input[name=enddate]').val();
            var data = {};
            var time = [];
            var i = '';
            var h = '';
            var d = '';
            var times = {};
            //var i = $(this).find('input[name=id]').val();
            //var h = $(this).find('input[type=number]').val();
            //var d = $(this).find('input[name=date]').val();
            //times.id = i;
            //times.timesheet_hour = h;
            //times.timesheet_date = d;
            //time.push(times);

            $(this).find('.slice').each(function () {
                i = $(this).find('input[name=id]').val();
                h = $(this).find('input[type=number]').val();
                d = $(this).find('input[name=date]').val();
                //times.id = i;
                //times.timesheet_hour = h;
                //times.timesheet_date = d;
                ////time.push(times);
                time.push({
                    id: i,
                    timesheet_hour: h,
                    timesheet_date: d
                });
            });

            //$('.each').each(function () {
            //    var i = $(this).find('input[name=id]').val();
            //    var h = $(this).find('input[type=number]').val();
            //    var d = $(this).find('input[name=date]').val();
            //    times.id = i;
            //    times.timesheet_hour = h;
            //    times.timesheet_date = d;
            //    time.push(times);
            //});

            data.id = id;
            data.job_id = jid;
            data.remark = remark;
            data.start_date_weekly = startdate;
            data.end_date_weekly = enddate;
            data.time = time;

            pts.push(data);
           

        });

        

        ajaxPost(ajaxUrlCreateTimesheet, parseJson(pts), function (response) {
            response = parseJson(response);
            if (response.success) {
                //$('.modal-backdrop').remove();
                SuccessNotif(t_createsuccess);
                redirecttolink('/projectmanagement/timesheet/entry');
            } else {
                DangerNotif(response.message);
            }
            console.log(response);
        });

        console.log(parseJson(pts));


    },

    addTimehseet: function () {
        //var cout = [];
        var day = [];
        $('#timesheettable tbody tr').each(function () {
            var id = $(this).find('input[name=id]').val();
            var jid = $(this).find('td').find('input[name=job_id]').val();
            var remark = $(this).find('td').find('input[name=remark]').val();
            var rm = $(this).find('td').find('.remarks').text();
            
            $('#timesheettable tbody .slice').each(function () {
                var i = $(this).find('input[name=id]').val();
                var h = $(this).find('input[type=number]').val();
                var d = $(this).find('input[name=date]').val();
                day.push({
                    id:i,
                    timesheet_hour: h,
                    timesheet_date: d
                
                });


            });
            

            //cout.push({
            //    job_id: jid,
            //    remark: rm,
            //    time: day

            //});

            var cout = {
                id : id,
                job_id: jid,
                remark: remark,
                time: day
            };

            ajaxPost(ajaxUrlCreateTimesheet, cout, function (response) {
            response = parseJson(response);
            if (response.success) {
                //$('.modal-backdrop').remove();
                SuccessNotif(t_createsuccess);
                redirecttolink('/projectmanagement/timesheet/entry');
            } else {
                DangerNotif(response.message);
            }
            console.log(response);
        });
            
        });

        console.log(cout);
    },

    delete: function (id) {
        var text = t_delete;
        confirmDialog(text, function () {
            ajaxDelete(ajaxUrlDelete + "/" + id, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_deletesuccess);
                    //fnTimesheet .reloadlist();
                    redirecttolink('/projectmanagement/timesheet/entry');
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }


    //addnewtask: function () {

        
    //    var out =
    //    {

    //        job_id: $("#job_number option:selected").val(),
    //        Non_job: $("#nonjobname").val(),
    //        remarks: $("#remarks").val(),
    //        timesheet_date: $(".datepick").val(),
    //        timesheet_hour: $(".timepick").val(),

    //    };
  
        

    //    ajaxPost(ajaxUrlCreateTimesheet, out, function (response) {
    //        response = parseJson(response);
    //        if (response.success) {
    //            $('.modal-backdrop').remove();
    //            SuccessNotif(t_createsuccess);
    //            redirecttolink('/projectmanagement/timesheet/entry')
    //        } else {
    //            DangerNotif(response.message);
    //        }
    //        console.log(response);
    //    });
    //},

    //generateJobList: function () {
    //    ajaxGet(ajaxUrlJobList, { 'rows': 1000, 'active': true }, function (response) {
    //        response = parseJson(response);

    //        var _option = ''
    //        $.each(response.rows, function (i, value) {
    //            _option += ('<option value="' + value.id + '"><small>' + value.job_number + '</small></option>');
    //        });

    //        $('select[name=job_number]').append(_option);
    //    })
    //},
    //generateTasklist: function () {

    //    ajaxGet(ajaxUrlTasklList, { 'rows': 1000, 'active': true }, function (response) {
    //        response = parseJson(response);

    //        var _option = ''
    //        $.each(response.rows, function (i, value) {
    //            _option += ('<option value="' + value.id + '">' + value.task_name + '</option>');
    //        });

    //        $('select[name=task_id]').append(_option);

    //        //console.log(response.rows);
    //    });

    //},
    //getest: function () {


    //    ajaxGet(ajaxurltimesheet, { 'rows': 1000, 'active': true }, function (response) {
    //        response = parseJson(response);
    //        console.log(response);
    //    });
    //}

};


$(function () {

    $('.datepick').setdatepicker();
    //$('.timepick').timepicker();

    //$('.timepick').datetimepicker({
    //    pickDate: false,
    //    format: 'hh:mm',
    //});

    $('.timepick').datetimepicker({
        format: 'HH:ii',
        autoclose: true,
        startView: 1,
        maxView: 1,
        language: "fr",
        pickDate: false
    }).on("show", function () {
        $(".table-condensed th").text("");
    });

    var optionsForm = GetOptionsForm(function () {

        var xret = $("#" + xform).valid();
        var cdata = getdata();
        if (cdata) {
            postdata(cdata);
        }

        return false;
    },  function (response, statusText, xhr, $form) {


    });

    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlCreateTimesheet);
    initlanguage(lang_gn);

    $('#job_number').setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlJobList,
        searchparam: 'search',
        labelField: 'job_name',
        valueField: 'id'
    });


    //$('#job_number').on('select2:select', function (e) {
    //    var data = e.params.data;
    //    var did = $('#job_number').val()
    //    ajaxGet(ajaxUrlTasklListFilter + '/' + did, {}, function (response) {
    //        jobtask = [];
    //        alltask = [];
    //        if (response.rows) {
    //            jobtask = response.rows;
    //            var xdata = response.rows;
    //            vstr = "";
    //            $(jobtask).each(function (index, item) {
    //                vstr += '<option value="' + item.id + '"><small>' + item.text + '</small></option>';
    //            });
    //            $('#taskList').append(vstr);
    //        }
    //    });
    //});

});

//$('select[name=task_id]').change(function () {

//    if ($('select[name=task_id] option:selected').text() == "baru") {
//        $('.non-user').show();
//    }
//    else {
//        $('.non-user').hide();
//    }

//});


$('#usenonjob').click(function () {


    if ($(this).is(":checked")) {

        var newInput = $('<div id="jobinput"><label><span class="translate" data-args="Non Job">Non Job</span></label><input type="text" id="nonjobname" name="tasknj_name" class="form-control" /></div>');

        $('#jobselect').remove();
        $('#optitem').append(newInput);
    }
    else {

      

        $(function () {

            var optionsForm = GetOptionsForm(function () {

                var xret = $("#" + xform).valid();
                var cdata = getdata();
                if (cdata) {
                    postdata(cdata);
                }

                return false;
            }, function (response, statusText, xhr, $form) {


            });

            InitForm(xform, optionsForm);
            setFormAction(xform, ajaxUrlCreateTimesheet);
            initlanguage(lang_gn);

            $('#job_number').setcombobox({
                data: { 'rows': 100 },
                url: ajaxUrlJobList,
                searchparam: 'search',
                labelField: 'job_name',
                valueField: 'id'
            });


            $('#job_number').on('select2:select', function (e) {
                var data = e.params.data;
                var did = $('#job_number').val();
                ajaxGet(ajaxUrlTasklListFilter + '/' + did, {}, function (response) {
                    jobtask = [];
                    alltask = [];
                    if (response.rows) {
                        jobtask = response.rows;
                        var xdata = response.rows;
                        vstr = "";
                        $(jobtask).each(function (index, item) {
                            vstr += '<option value="' + item.id + '"><small>' + item.text + '</small></option>';
                        });
                        $('#taskList').append(vstr);
                    }
                });
            });

        });
        $('#jobinput').remove();
        var newSelect = $('<div id="jobselect"><label><span class="translate" data-args="Job"></span></label><select id="job_number" name="task_id"></select></div>');

        $('#optitem').append(newSelect);
        
    }

});


$(document).on('change', '.xtime', function () {
    var value = $(this).val();

    if ((value !== '') && (value.indexOf('.') === -1)) {

        $(this).val(Math.max(Math.min(value, 24), -24));
    }
    hitungtotal()
});
function hitungtotal() {
    var xtotal = 0;
    for (var ic = 0; ic < 7; ic++) {
        vttl = 0;
        $("#ttltime_" + ic).val("");
        $(".xtime" + ic).each(function (index, item) {


            vttl += getnumber($(this).val());

        });

        $("#ttltime_" + ic).val(vttl);
    }

}

function postdata() {

}


$(function () {
    fnTimesheet.changeWeek();
    ////fnTimesheet.getest();
    ////fnTimesheet.generateTasklist();
    ////fnTimesheet.generateJobList();

    $("#addMods").click(function () {
        $("#categorylist").removeAttr("tabindex");
    });
    

    fnTimesheet.getJobLList();
    $("#to_date").setdatepicker();
    

    $("#to_date").change(function () {

        fnTimesheet.generatelist();


    });

    $('#dateStart').datepicker({
        
        changeMonth: false,
        changeYear: false,
        timepicker: false,
        closeOnDateSelect: true,
        scrollInput: false,
        minDate: 0,
        daysOfWeekDisabled: [0, 2, 3, 4, 5, 6],
        autoclose: true
    });

    $('#dateEnd').datepicker({

        changeMonth: false,
        changeYear: false,
        timepicker: false,
        closeOnDateSelect: true,
        scrollInput: false,
        minDate: 0,
        daysOfWeekDisabled: [1, 2, 3, 4, 5, 6],
        autoclose: true
    });


    $("#to_date").val(currentdate).change();
    initlanguage(lang_gn);

});




