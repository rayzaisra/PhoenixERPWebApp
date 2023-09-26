
var ajaxUrl = API + "ProjectManagement/Task";
var ajaxUrlGet = API + "ProjectManagement/Job/get";
var ajaxUrlGetTask = ajaxUrl + "/GetByJobNumber";
var ajaxUrlCreateTask = ajaxUrl + "/create";
var ajaxUrlEditTask = ajaxUrl + "/update";
var ajaxUrlDeleteTask = ajaxUrl + "/DeleteTask";
var ajaxUrlCreateSubTask = ajaxUrl + "/createsubtask";
var ajaxUrlupdatetaskdate = ajaxUrl + "/updatesubtaskdate";
var ajaxUrlupdatesubtaskuser = ajaxUrl + "/updatesubtaskuser";
var ajaxUrlEditSubTask = ajaxUrl + "/updatesubtask";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListActor = API_PM + "/AccountManagement/listactor";
xtaskform = "TaskForm";
xtaskeditform = "EditTaskForm";
var jobnumber = '';
var listActor = [];
var ganttdata = [];
fnTask = {
    setFormAjax: function (xform, ajaxUrl, notiftitle) {
        var optsForm = GetOptionsForm(function () {
            var xret = $("#" + xform).valid();
            return xret;
        }, function (response, statusText, xhr, $form) {
            response = parseJson(response);
            if (response.success) {
                gettaskdata();
                $('.modal').modal('hide');
                SuccessNotif(notiftitle);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xform, optsForm);
        setFormAction(xform, ajaxUrl);
    }
    //setTaskFormUpdate: function () {

    //},
};

function getjobdetail() {
    ajaxGet(ajaxUrlListActor, {}, function (response) {
        var respact = parseJson(response);
        listActor = respact.rows;
        ajaxGet(ajaxUrlGet + "/" + RecordID, {}, function (response) {
            response = parseJson(response);
            $('#title_task').html(function () {
                return $(this).html().replace("{JobName}", response.data.job_name).replace("{JobNumber}", response.data.job_number).replace("{JobNumber}", response.data.job_number).replace("{Division}", response.data.business_unit_name);
            });
            $('#EditUserForm #job_number_sub_task').val(response.data.job_number);

            generatelist(response.data.teamMember);
        });
    });

}

function generatelist(data) {
    $("#TeamusersList tbody").empty();
    $("#TeamusersPickList tbody").empty();
    astr = '';
    cstr = '';
    $.each(data, function (key, row) {
        vstr = '<tr>';
      
        vstr += '<td>' + row.username + '</td>';
        vstr += '<td>' + row.email + '</td>';
        retval = "";
        xdata = listActor.find(x => x.id === row.act_as);
        if (xdata) retval = xdata.name;

        vstr += '<td>' + retval + '</td>';
        cstr += vstr + '<td><input class="stuser" name="user_id" value="' + row.user_id+'" type="checkbox"></td></tr>';
        vstr += '</tr>';
        astr += vstr;
    });
    $("#TeamusersList tbody").append(astr);
    $("#TeamusersPickList tbody").append(cstr);
}

$(function () {
    $('#TaskForm #central_resources_id').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: 700 },
        url: ajaxUrlListOrganization,
        searchparam: 'search',
        labelField: 'unit_name',
        valueField: 'id'
    });

    $(".datepicker").setdatepicker();
    //SubTaskForm;
    //fnPanel.startprocess();
    getjobdetail();
    gettaskdata();

    //-------------assign form
    fnTask.setFormAjax('TaskForm', ajaxUrlCreateTask, 'Task Created');
    fnTask.setFormAjax('EditTaskForm', ajaxUrlEditTask, 'Task Updated');
    fnTask.setFormAjax('SubTaskForm', ajaxUrlCreateSubTask, 'Sub Task Created');
    fnTask.setFormAjax('EditSubTaskForm', ajaxUrlEditSubTask, 'Sub Task Updated');
    fnTask.setFormAjax('EditUserForm', ajaxUrlupdatesubtaskuser, 'Sub Task Created');
    //------------------------------------
   
});

function gettaskdata() {
    ajaxGet(ajaxUrlGetTask + "/" + RecordID, {}, function (response) {
        response = parseJson(response);
        console.log(response);
        if (response.success) {
            $.each(response.rows, function (idx, item) {
                var a = item.current_id;
                console.log("index of " + a.search("GN"));
                if (a.search("GN") >= 0 ) {
                    $('#start_task').val(moment(item.start_date).format('YYYY-MM-DD'));
                    $('#deadline_task').val(moment(item.endDate).format('YYYY-MM-DD'));
                    $('#start_date').val(moment(item.start_date).format('YYYY-MM-DD'));
                    $('#due_date').val(moment(item.endDate).format('YYYY-MM-DD'));
                } 
                
            });
            var datarow = mappingdata(response.rows);

            gantt.clearAll();
            var databaru = { data: datarow };
            gantt.parse(databaru);
        }
        //fnPanel.endprocess();
    });
}

function setdatabaru() {

    var databaru = {
        data: [{
            "id": "1520310249451", "text": "Project", "start_date": null, "end_date": null, "progress": "0", "open": true
        }, {
            "id": "1520310249452", "text": "Task", "start_date": "12-12-2019", "end_date": "13-12-2019", "parent": "1520310249451", "progress": "0", "open": true
        },
        {
            "id": "1520310249453", "text": "Task", "start_date": "12-12-2019", "end_date": "13-12-2019", "parent": "1520310249451", "progress": "100", "open": true
        }]
    };
}

function mappingdata(datamap) {
    newdata = [];
    $.each(datamap, function (idx, item) {

        var newitem = {
            "job_id": item.job_id,
            "priority": item.priority,
            "current_id": item.current_id,
            "task_id": item.task_id,
            "id": item.id,
            "hour": item.hour,
            "description": item.description,
            "text": item.text,
            'users': item.users,
            "type": item.type,
            "parent": item.parent,
            "progress": item.progress,
            "start_date": item.start_date !== null ? moment(item.start_date).format('DD-MM-YYYY') : null,
            "end_date": item.end_date !== null ? moment(item.end_date).format('DD-MM-YYYY') : null,
            'open': true,
            'color': colorbyType(item.type)
        };

        newdata.push(newitem);
    });
    ganttdata = newdata;
    return newdata;
}

function colorbyType(type) {
    var xcolor = '#4286f4';
    if (type === 'job') {
        xcolor = '#41f456';
    } else if (type === 'task') {
        xcolor = '#c7f441';
    } else if (type === 'subtask') {
        xcolor = '#4286f4';

    }
    return xcolor;
}

function setScaleConfig(level) {
    switch (level) {
        case "day":
            gantt.config.scale_unit = "day";
            gantt.config.step = 1;
            gantt.config.date_scale = "%d %M";
            gantt.templates.date_scale = null;

            gantt.config.scale_height = 27;

            gantt.config.subscales = [];
            break;
        case "week":
            var weekScaleTemplate = function (date) {
                var dateToStr = gantt.date.date_to_str("%d %M");
                var endDate = gantt.date.add(gantt.date.add(date, 1, "week"), -1, "day");
                return dateToStr(date) + " - " + dateToStr(endDate);
            };

            gantt.config.scale_unit = "week";
            gantt.config.step = 1;
            gantt.templates.date_scale = weekScaleTemplate;

            gantt.config.scale_height = 50;
            gantt.config.row_height = 50;
            gantt.config.subscales = [
                { unit: "day", step: 1, date: "%D" }
            ];
            break;
        case "month":
            gantt.config.scale_unit = "month";
            gantt.config.date_scale = "%F, %Y";
            gantt.templates.date_scale = null;

            gantt.config.scale_height = 50;

            gantt.config.subscales = [
                { unit: "day", step: 1, date: "%j" }
            ];

            break;
        case "year":
            gantt.config.scale_unit = "year";
            gantt.config.step = 1;
            gantt.config.date_scale = "%Y";
            gantt.templates.date_scale = null;

            gantt.config.min_column_width = 50;
            gantt.config.scale_height = 90;

            gantt.config.subscales = [
                { unit: "month", step: 1, date: "%M" }
            ];
            break;
    }
}

var els = document.querySelectorAll("input[name='scale']");

for (var i = 0; i < els.length; i++) {
    els[i].onclick = function (e) {
        e = e || window.event;
        var el = e.target || e.srcElement;
        var value = el.value;
        setScaleConfig(value);
        gantt.render();
    };
}

var data = { data: [] };

gantt.config.fit_tasks = true;
gantt.config.max_column_width = 125;
gantt.config.drag_progress = false;
gantt.config.duration_unit = "day";
gantt.config.scale_unit = "day";
gantt.config.show_links = false;
gantt.config.grid_width = 490;


fntask = {
    addtask: function (id) {
        $("#TaskForm #job_number").val(id);
        $("#CreateTask").modal('show');
    },
    addsubtask: function (id) {
        $("#SubTaskForm #task_id").val(id);
        $("#SubTask").modal('show');
    },
    viewuserjob: function (id) {
        $("#UserList").modal('show');
    },
    viewusertask: function (id) {
        $("#TaskForm #job_number").val(id);
        $("#CreateTask").modal('show');
    },
    manageuser: function (id) {
        $('input:checkbox.stuser').removeAttr('checked');
        $('input:checkbox.stuser').each(function () {
            $(this).prop('checked', false);
        });
        var xdata = ganttdata.find(x => x.current_id === id);
        if (xdata.users) {
            var arrayuser = xdata.users.split(',');

            $.each(arrayuser, function (key, itemuser) {
                itemuser = itemuser.trim();

                $('input:checkbox.stuser[value="' + itemuser + '"]').prop('checked', true);
            });

        }
        $("#EditUserForm #sub_task_id").val(id);

        $("#EditUser").modal('show');
    },
    managetask: function (id) {
        var xdata = ganttdata.find(x => x.current_id === id);

        $("#EditTaskForm #task_name").val(xdata.text);
        $('#EditTaskForm [name="task_priority_id"][value="' + xdata.priority + '"]').prop('checked', true);
        $("#EditTaskForm #description").val(xdata.description);
        var cdate = moment(xdata.start_date).format('YYYY-MM-DD');

        $("#EditTaskForm #start_task").val(cdate);
        cdate = moment(xdata.end_date).format('YYYY-MM-DD');
        $("#EditTaskForm #deadline_task").val(cdate);

        $("#EditTaskForm #id").val(id);
        $("#EditTask").modal('show');
    },
    managesubtask: function (id) {
        var xdata = ganttdata.find(x => x.current_id === id);

        $("#EditSubTaskForm #task_id").val(xdata.task_id);
        $("#EditSubTaskForm #sub_task").val(xdata.text);
        $("#EditSubTaskForm #hour").val(xdata.hour);
        $("#EditSubTaskForm #description").val(xdata.description);
        var cdate = moment(xdata.start_date).format('YYYY-MM-DD');

        $("#EditSubTaskForm #start_date").val(cdate);
        cdate = moment(xdata.end_date).format('YYYY-MM-DD');
        $("#EditSubTaskForm #due_date").val(cdate);

        $("#EditSubTaskForm #id").val(id);
        $("#EditSubTask").modal('show');
    },
    updatesubtaskuser: function () {
        users = [];
        $('input:checkbox.stuser').each(function () {
            if (this.checked) {
                users.push($(this).val());
            }
        });

        var datauser = {
            "sub_task_id": $("#EditUserForm #id").val(),
            "user_id": users,
            "job_number_sub_task": $("#job_number_sub_task").val()
        };
        
        ajaxPost(ajaxUrlupdatesubtaskuser, datauser, function (response) {
            if (resp.success) {
                SuccessNotif("User Updated");
            } else {
                DangerNotif(resp.message);
            }
        });
    },
    delete: function (id, task, name) {
        confirmDialog("Are you sure, want to delete " + task + " " + name, function () {
            ajaxDelete(ajaxUrlDeleteTask + "/" + id, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    SuccessNotif(t_deletesuccess);
                    gettaskdata();
                    hideAllModal();
                } else {
                    DangerNotif(response.message);
                }
            });
        });
    }

};

gantt.config.scale_height = 80;
setScaleConfig("month");
gantt.config.columns = [
    {
        name: "text", label: "Task Name", width: "*", tree: true
    },
    {
        name: "progress", label: "progress", width: "40"
    },
    {
        name: "start_date", label: "start", width: "40", template: function (obj) {
            return moment(obj.start_date).format('DD/MM');
        }
    },
    {
        name: "end_date", label: "end", width: "40", template: function (obj) {
            return moment(obj.end_date).format('DD/MM');
        }
    },
    {
        name: "text", label: "update", width: "125", align: "left", template: function (obj) {
            vstr = '';
            
            vstr += '<div class="m-btn-group  btn-group  btn-group-sm">';
            if (obj.type === 'job') {
                
                vstr += '<a title="Add Task" style="width:40px;"  onclick="fntask.addtask(\'' + obj.current_id + '\')"  class="btn btn-sm btn-outline-info" href="javascript:void(0)"> <i class="fa fa-plus"></i></a>'
                vstr += '<a title="View User" style="width:40px;" onclick="fntask.viewuserjob(\'' + obj.current_id + '\')"  class="btn btn-sm btn-outline-info" href="javascript:void(0)"><i class="fa fa-users"></i></a>';
               
            } else if (obj.type === 'task') {
                vstr += '<a title="Manage" style="width:40px;"  onclick="fntask.managetask(\'' + obj.current_id + '\')"  class="btn btn-sm btn-outline-info" href="javascript:void(0)"><i class="fa fa-edit"></i></a>';
                vstr += '<a title="Add Sub Task" style="width:40px;"  onclick="fntask.addsubtask(\'' + obj.current_id + '\')" class="btn btn-sm btn-outline-info" href="javascript:void(0)"> <i class="fa fa-plus"></i></a>'
                vstr += '<a title="Delete Task"  onclick="fntask.delete(\'' + obj.current_id + '\', \'subtask\', \''+obj.text+'\' )" class="btn btn-sm btn-outline-info" href="javascript:void(0)"><i class="la la-trash"></i></a>';
            } else if (obj.type === 'subtask') {
                vstr += '<a title="Manage" style="width:40px;" onclick="fntask.managesubtask(\'' + obj.current_id + '\')"  class="btn btn-sm btn-outline-info" href="javascript:void(0)"><i class="fa fa-edit"></i></a>';
                vstr += '<a title="Manage User" style="width:40px;"  onclick="fntask.manageuser(\'' + obj.current_id + '\')" class="btn btn-sm btn-outline-info" href="javascript:void(0)"><i class="fa fa-user-cog"></i></a>';
                vstr += '<a title="Delete Task"  onclick="fntask.delete(\'' + obj.current_id + '\' , \'subtask\', \'' + obj.text +'\')" class="btn btn-sm btn-outline-info" href="javascript:void(0)"><i class="la la-trash"></i></a>';
            }
            vstr += '</div>';
            return vstr;
        }
    }
];
gantt.config.show_errors = false;

//---------------------
gantt.attachEvent("onTaskDblClick", function (id, e) {

    return false;
});
gantt.attachEvent("onAfterTaskDrag", function(id, mode, e){
    var xdata = ganttdata.find(x => x.id === id);
    var datatask = {
        "id": xdata.current_id,
        "start_date": moment(xdata.start_date).format('YYYY-MM-DD'),
        "due_date": moment(xdata.end_date).format('YYYY-MM-DD')
    };
    ajaxPost(ajaxUrlupdatetaskdate, datatask, function (response) {
    });
 
});
gantt.init("gantt_place");
