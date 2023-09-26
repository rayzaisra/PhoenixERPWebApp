
var ajaxUrl = API + "/ProjectManagement/Task";
var ajaxUrlGet = API + "/ProjectManagement/Job/get";
var ajaxUrlGetTask = ajaxUrl + "/GetByJobNumber";
var ajaxUrlCreateTask = ajaxUrl + "/create";
var ajaxUrlEditTask = ajaxUrl + "/update";
var ajaxUrlCreateSubTask = ajaxUrl + "/createsubtask";
var ajaxUrlupdatetaskdate = ajaxUrl + "/updatesubtaskdate";
var ajaxUrlupdatesubtaskuser = ajaxUrl + "/updatesubtaskuser";
var ajaxUrlEditSubTask = ajaxUrl + "/updatesubtask";
var ajaxUrlListOrganization = API_GENERAL + "/Businessunit/ListWithParent";
var ajaxUrlListActor = API_PM + "/AccountManagement/listactor";
xtaskform = "TaskForm";
xtaskeditform = "EditTaskForm";
var listActor = [];
var ganttdata = [];
fnTask = {
    setFormAjax: function (xform, ajaxUrl,notiftitle) {
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
    },
    setTaskFormUpdate: function () {

    },
}

function getjobdetail() {

        ajaxGet(ajaxUrlGet + "/" + RecordID, {}, function (response) {
            response = parseJson(response);

            $('#title_task').html(function () {
                return $(this).html().replace("{JobName}", response.data.job_name).replace("{JobNumber}", response.data.job_number).replace("{JobNumber}", response.data.job_number).replace("{Division}", response.data.business_unit_name);
            });
        });
}
 
$(function () {
  

    getjobdetail();
    gettaskdata();
})

function gettaskdata() {
    ajaxGet(ajaxUrlGetTask + "/" + RecordID, {}, function (response) {
        response = parseJson(response);

        if (response.success) {

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
    if (type == 'job') {
        xcolor = '#41f456';
    } else if (type == 'task') {
        xcolor = '#c7f441';
    } else if (type == 'subtask') {
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
gantt.config.max_column_width = 20;
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
                itemuser = itemuser.trim()

                $('input:checkbox.stuser[value="' + itemuser+'"]').prop('checked', true);
            })

        }
        $("#EditUserForm #sub_task_id").val(id);

        $("#EditUser").modal('show');
    },
    managetask: function (id) {
        var xdata = ganttdata.find(x => x.current_id === id);

        $("#EditTaskForm #task_name").val(xdata.text);
        $('#EditTaskForm [name="task_priority_id"][value="' + xdata.priority + '"]').prop('checked', true);
        $("#EditTaskForm #description").val(xdata.description);
        var cdate = moment(xdata.start_date).format('YYYY-MM-DD') ;
 
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
                users.push($(this).val())
            }
        });
        var datauser = {
            "sub_task_id": $("#EditUserForm #id").val(),
            "user_id": users
        }
        ajaxPost(ajaxUrlupdatesubtaskuser, datauser, function (response) {
            if (resp.success) {
                SuccessNotif("User Updated");
            } else {
                DangerNotif(resp.message);
            }
        })
    }

}
gantt.config.readonly = true;
gantt.config.scale_height = 80;
setScaleConfig("month");
gantt.config.columns = [
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
    }
    ajaxPost(ajaxUrlupdatetaskdate, datatask, function (response) {
    })
 
});
gantt.init("gantt_place");
