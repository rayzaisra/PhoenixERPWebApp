var ajaxUrlGet = API + "ProjectManagement/Job/GetWithTask";
var ajaxtaskUrlGet = API + "ProjectManagement/Task/Get";
var ajaxUrlassignboard = API + "ProjectManagement/job/assignboard";
var ajaxUrlListPM = API + "ProjectManagement/Job/Listpm";
var ajaxUrlListTaskByJobNumber = API + "ProjectManagement/Task/ListByJobNumber";
var ajaxUrlListUser = API_PM + "/AccountManagement/list";
var ajaxUrlassigntask = API_PM + "/Task/manage";
var ajaxUrltaskstatus = API_PM + "/TaskStatus/list";
var ajaxUrltaskcommentsend = API_PM + "/Task/sendcomment";
var ajaxUrltaskcommentlist = API_PM + "/Task/commentlist";
var ajaxUrltasksaveprogress = API_PM + "/Task/updateprogress";
var ajaxUrltasksavestatus = API_PM + "/Task/updatestatus";
var ajaxUrltasksaveDumpstatus = API_PM + "/Task/updateDumpstatus";
var ajaxUrltasksendNotification = API_PM + "/Task/sendnotification/";
var ajaxUrlUploadOneDrive = API + "General/File/UploadOneDrivePeFolder";
var ajaxUrlGetFile = API + "General/File/Getfile";
var ajaxUrlGetListFileTask = API + "General/File/GetFileByRecordId";
var xform = "JobassignboardForm";
var xtaskform = "TaskForm";
var xjob_number = "";
var taskstatus = [];
var current_task_id = "";
var current_done_task_id = "";
var current_done_task_container_id = "";
var list_of_done_task = [];
var list_of_done_task_temporary = [];
var listTask = []
var isClickedToManage = false, isSaveTask = false;
var isPM = false;
var jobData = {};

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
        if (data.pm) { $("#pmname").text(data.pm.app_fullname); }

        initlanguage(lang_pm);
        FormTextByData(data, xform);
        $('#jobtasksrc').empty();

        $.each(taskstatus, function (key, item) { settaskbox(item); });

        if (data.tasklist) {
            $.each(data.tasklist, function (key, value) { settaskdiv(value); });
        }
        generatescrum();
        if (list_of_done_task.length > 0) {
            if (isPM) { setButtonApproveAll(); $('#jobtasksrc').css("height", "292px"); }
        }



    }, getTask: function (data) {
        ajaxGet(ajaxUrlListTaskByJobNumber + "/" + data.job_number, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $.each(data.rows, function (key, value) {
                    settask(value.name);
                });

            } else {
                $("#Jobassignboardcontent").hide();
            }


        });
    }
};

function generateButtonApproveAll(task_id, task_status_id) {

    var alreadyExistInArray = false;


    if (task_status_id === '3') {
        for (var i = 0; i < list_of_done_task.length; i++) {
            if (list_of_done_task[i].task_id === task_id) {
                if (list_of_done_task[i].task_status_id === '3')
                    alreadyExistInArray = true;
            }
        }
        if (!alreadyExistInArray) {
            list_of_done_task.push({
                'task_id': task_id, 'task_status_id': task_status_id
            });
        }
    }

    for (var i = 0; i < list_of_done_task.length; i++) {
        if (list_of_done_task[i].task_id === task_id && task_status_id !== list_of_done_task[i].task_status_id) {
            if (list_of_done_task[i].task_status_id === '3') {
                list_of_done_task_temporary.push({
                    'task_id': list_of_done_task[i].task_id,
                    'task_status_id': list_of_done_task[i].task_status_id
                }
                );

                console.log(list_of_done_task_temporary);
                if (list_of_done_task.length === 1) { list_of_done_task.pop(); }
                else { list_of_done_task.splice(i, 1); }


            }
        }
    }
    for (var i = 0; i < list_of_done_task_temporary.length; i++) {
        if (list_of_done_task_temporary[i].task_id === task_id && task_status_id === list_of_done_task_temporary[i].task_status_id) {
            if (task_status_id === '3') {
                list_of_done_task_temporary.splice(i, 1);
            }
        }
    }

    console.log('List Task yang Done' + list_of_done_task.length);
    console.log('List Task yang Temporary' + list_of_done_task_temporary.length);
    $("#btn-approve").remove();
    if (list_of_done_task.length > 0) { setButtonApproveAll(); }
}

function setButtonApproveAll() {
    task_column_div = $("#" + current_done_task_id);
    strct = '<div id="btn-approve" style="margin:40px;"><div style="padding:10px; position:absolute; right:0; bottom:0"><a class="fakelink btn btn-secondary" id="btn-Approved" style="width:100px;" onclick="approveAll()">Approve All</a></div></div> ';
    task_column_div.append(strct);
}

function calculatecheck() {
    var valeur = 0;
    var eqty = $('.cprogress').length;
    $('.cprogress:checked').each(function () {
        valeur++;
    });

    var xpercent = (valeur / eqty) * 100
    $('.progress-bar').css('width', xpercent + '%').attr('aria-valuenow', valeur);
}

$(document).on('change', '.cprogress', function () {
    var idck = $(this).val()
    var ischecked = $(this).prop("checked");
    if (ischecked) {
        saveprogress(idck + '', 100);
    } else {
        saveprogress(idck + '', 0);
    }
    calculatecheck();
});
function nl2br(str, is_xhtml) {
    var breakTag = (is_xhtml || typeof is_xhtml === 'undefined') ? '<br />' : '<br>';
    return (str + '').replace(/([^>\r\n]?)(\r\n|\n\r|\r|\n)/g, '$1' + breakTag + '$2');
}

function generatecomment(data) {
    $('#commentarea').empty();
    vstr = '';
    $.each(data, function (key, item) {
        vstr += '<div class="row">';
        vstr += '<div class="col-lg-6"><strong>' + item.user_name + '</strong></div>';
        vstr += '<div class="col-lg-6"><div class="pull-right">' + moment(item.comment_date).format("YYYY-MM-DD HH:mm:ss") + '</div></div>';
        vstr += '</div>';
        vstr += '<div class="row">';
        vstr += '<div class="col-lg-12 textbreak">' + nl2br(item.comment) + '</div>';
        vstr += '</div>';
        vstr += '<hr>';
    });
    $('#commentarea').html(vstr);
}

function getcomment() {
    ajaxGet(ajaxUrltaskcommentlist + "/" + current_task_id, {}, function (response) {
        if (response.success) {
            generatecomment(response.rows);
        }
    });
}

function savestatus(task_id, task_status_id) {
    var savedata = { 'task_id': task_id, 'task_status_id': task_status_id };

    ajaxPost(ajaxUrltasksavestatus, savedata, function (response) {
    });

}

function saveprogress(sub_task_id, percentage) {
    var savedata = { 'sub_task_id': sub_task_id, 'percentage': percentage };

    ajaxPost(ajaxUrltasksaveprogress, savedata, function (response) {});
}

function managetask(id, modtitle, task_status_id) {
    current_task_id = id;
    isClickedToManage = true;
    console.log(isClickedToManage);
    obj = setButtonTaskStatus(task_status_id);
    contentno = 0;

    showModalAjaxGet(modtitle.toUpperCase(), WEB_PM + '/task/detil/' + id, {}, function () {
        ajaxGet(ajaxtaskUrlGet + "/" + id, {}, function (response) {
            getcomment();
            var xsubtask = response.data.subtask;
            $("#taskdesc").html(response.data.description);
            if (xsubtask) {
                $("#tasklist").empty();
                $.each(xsubtask, function (key, value) {
                    xchecked = "";
                    if (value.percentage) {
                        if (value.percentage == 100) {
                            xchecked = "checked";
                        }
                    }
                    vstr = '<div class="m-checkbox-list"><label class="m-checkbox "><input type = "checkbox" class="cprogress " ' + xchecked + ' name="sub_task[]" value="' + value.id + '" data-name="' + value.sub_task + '" >' + value.sub_task + '<span></span ></label ></div>';
                    $("#tasklist").append(vstr);
                    if (obj.status_task_name === 'In Progress') {
                        $(".m-checkbox-list input:checkbox").attr("disabled", true).attr("checked", false);
                    }
                });
                calculatecheck();
                $('#priorityTask').text("Priority : " + response.data.task_priority_id);
                if (response.data.task_priority_id === "high") {
                    $("#priorityTask").addClass("m-portlet__head-label--danger");
                    $("#priorityTask").removeClass("m-portlet__head-label--info");
                    $("#priorityTask").removeClass("m-portlet__head-label--warning");
                    $("#priorityTask").removeClass("m-portlet__head-label--primary");
                } else if (response.data.task_priority_id === "low") {
                    $("#priorityTask").removeClass("m-portlet__head-label--danger");
                    $("#priorityTask").addClass("m-portlet__head-label--info");
                    $("#priorityTask").removeClass("m-portlet__head-label--warning");
                    $("#priorityTask").removeClass("m-portlet__head-label--primary");
                } else if (response.data.task_priority_id === "medium") {
                    $("#priorityTask").removeClass("m-portlet__head-label--danger");
                    $("#priorityTask").removeClass("m-portlet__head-label--info");
                    $("#priorityTask").addClass("m-portlet__head-label--warning");
                    $("#priorityTask").removeClass("m-portlet__head-label--primary");
                } else {
                    $("#priorityTask").removeClass("m-portlet__head-label--danger");
                    $("#priorityTask").removeClass("m-portlet__head-label--info");
                    $("#priorityTask").removeClass("m-portlet__head-label--warning");
                    $("#priorityTask").addClass("m-portlet__head-label--primary");
                }
                var inHtml = "";
                var loopHtml = "";
                if (response.data.subtask.length > 0) {
                    for (var i = 0; i < response.data.subtask.length; i++) {
                        if (response.data.subtask[i].subtaskuser.length > 0) {
                            $('#table-listtaskmember').empty();
                            inHtml = '<table class="table table-striped table-bordered table-hover table-checkable">' +
                                '<tr><th>Nama</th>' +
                                '<th>Email</th></tr>';
                            $.each(response.data.subtask[i].subtaskuser, function (key, item) {
                                var name = item.app_fullname;
                                var email = item.app_username;
                                var link = '<tr><td>' + name + '</td><td>' + email + '</td></tr>';
                                loopHtml = loopHtml + link;

                            });
                            inHtml = inHtml + loopHtml;
                            inHtml = inHtml +
                                '</table>';

                            $('#table-listtaskmember').append(inHtml);
                        }
                    }
                    
                }
                else {
                    $('#table-listtaskmember').text("There is no user assign yet.");
                }


            }
        });

        getListFileTask(id);

        btnstr = '<a id="btn-' + obj.status_task_name + '" class="fakelink btn btn-secondary" style="width:100px;" onclick="savetask(\'' + id + '\',\'' + obj.status_task_id + '\' ); return false;" >' + obj.status_task_name + '</a>';
        if (obj.status_task_name != '') { $("#buttonarea").empty(); $("#buttonarea").append(btnstr); }
    }, 'taskmakerbox');

}

function setButtonTaskStatus(id) {
    var obj = { status_task_id: '', status_task_name: '' };
    if (id < taskstatus.length) { obj.status_task_id = taskstatus[id].id; obj.status_task_name = taskstatus[id].name; }
    return obj;
}

function getListFileTask(id) {
    ajaxGet(ajaxUrlGetListFileTask + "/" + id, {}, function (response) {
        resp = parseJson(response);
        var inHtml = '';
        var loopHtml = '';
        if (resp.success) {
            if (resp.total > 0) {
                $('#dam').empty();
                inHtml = '<div class="row">' +
                    '<div class="col-md-12" >' +
                    '<table class="table table-striped table-bordered table-hover table-checkable">';
                    
                $.each(resp.rows, function (key, item) {

                    var filename = item.file_name;
                    var idContent = item.id;
                    var filepath = item.file_path;
                    var link = '<tr><td><a href="javascript:;" onclick="DownloadFile(\'' + ajaxUrlGetFile + '\',\'' + idContent + '\');" >' + filename + '</a></td>';
                    var path = '<td>' + filepath+'</td></tr>';
                    loopHtml = loopHtml + link + path;

                });
                inHtml = inHtml + loopHtml;
                inHtml = inHtml + 
                    '</table>' +
                    '</div>' +
                    '</div>';

                $('#dam').append(inHtml);
            }

        } 


    });


    
}


var contentno = 0;
function removesubtask(x) {
    stexist = $("div.subtaskclass").length;
    if (stexist > 1) {
        $(x).parents('div.subtaskclass').remove();
    } else {
        DangerNotif("minimum 1 Content");
    }
}

function savetask(taskid, task_status_id) {
    var savedata = { 'task_id': taskid, 'task_status_id': task_status_id };
    ajaxPost(ajaxUrltasksavestatus, savedata, function (response) {
        if (response.success) {
            isSaveTask = true;
            loadData();
            generateButtonApproveAll(taskid, task_status_id);

            setTimeout(function () {
                $('.close').trigger("click");
            }, 1000);
        }
    });
}

function approveAll() {
    $.each(list_of_done_task, function (index, value) { value.task_status_id = '4' });
    ajaxPost(ajaxUrltasksaveDumpstatus, list_of_done_task, function (response) {
        if (response.success) {
            list_of_done_task = [];
            ajaxPost(ajaxUrltasksendNotification + jobData.job_number, {}, function (res) {
                console.log("response ok");

            });
            loadData();
            setTimeout(function () { $("#btn-approve").remove(); }, 1000);
        }
    });
}

function sendcomment() {
    var comment = $('#commenttext').val();
    if (comment) {
        ajaxPost(ajaxUrltaskcommentsend, { 'task_id': current_task_id, 'comment': comment }, function (response) {
            response = parseJson(response);
            if (response.success) { generatecomment(response.rows); }
        });
        $('#commenttext').val("");
    }

}

function setFileUpload() {
    var file = document.getElementById("fileInput").files[0];
    //var reader = new FileReader();
    //reader.onload = function (e) {
    //    var textArea = document.getElementById("commenttext");
    //    console.log(e);
    //    textArea.value = e.target.result;
    //};
    //reader.readAsText(file);

    var formData = new FormData();
    formData.append("application_entity_id", 'pm.task');
    formData.append("record_id", current_task_id);
    formData.append("file", file);
    formData.append("tags", 'task');
    formData.append("uploadid", xjob_number);
    //ajaxPostJquery(ajaxUrlUploadOneDrive, formData, function (response) {
    //    response = parseJson(response);
    //    if (response.success) { console.log("oke"); } else {
    //        console.log("not ok");
    //    }
    //});

    $.ajax({
        type: "POST",
        url: ajaxUrlUploadOneDrive,
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        beforeSend: function (xhdr) {
            xhdr.setRequestHeader(
                "Authorization", "Bearer " + $('meta[name=JWT_TOKEN]').attr("content"));
        },
        success: function (resp) {
            if (resp.success) {
                getListFileTask(current_task_id);
                swal('Information', 'File has been successfully uploaded', 'success');
            } else {
                swal('Warning', 'File failed to upload', 'warning');
            }
        },
        error: function (error) {
            ErrorMessage(error);
        }
    });
    

}

var xnotask = 0;
function settaskbox(value) {
    xnotask++;
    if (value.name.toUpperCase() === 'DONE') { current_done_task_id = ('task_' + xnotask); current_done_task_container_id = ("central_" + value.id); }
    strct = '<div class="board-column" style="width:200px;" id="task_' + xnotask + '">';
    strct += '<div class="board-column-header">';
    strct += value.name;

    strct += '</div>';
    strct += '<div class="board-column-content" data-id="' + value.id + '" id="central_' + value.id + '">';
    strct += '</div>';
    strct += '</div>';
    strct += '</div>';
    $("#jobtasksrc").append(strct);
    xnotask++;
}

function settaskdiv(value) {
    strct = '<div class="board-item" style="width:180px;" id="' + value.id + '"><div class="board-item-content">' +
        '<a href="javascript:void(0)" onclick="managetask(\'' + value.id + '\',\'' + value.task_name + '\',\'' + value.task_status_id + '\'); return false;">' + value.task_name + '</a>' +
        '<hr>' +
        '<div class="priority' + value.task_priority_id + '"> Task Priority: ' + setColorStatus(value.task_priority_id.toUpperCase()) + ' </div></div></div>';

    if (("central_" + value.task_status_id) === current_done_task_container_id) {
        if(!isSaveTask) {
            list_of_done_task.push({
                'task_id': value.id,
                'task_status_id': value.task_status_id
            }
            );
        }
        isSaveTask = false;
    }

    listTask.push({
        'task_id': value.id,
        'task_boxId': value.task_status_id
    });

    setTimeout(function () {

    }, 1000);

    $('#central_' + value.task_status_id).append(strct);
}

function setColorStatus(status) {
    var htmlString = "<span style='color: {color} '> {text} </span>";
    switch (status) {
        case "LOW": htmlString = htmlString.replace('{color}', 'green;').replace('{text}', status); break;
        case "MEDIUM": htmlString = htmlString.replace("{color}", "yellow;").replace("{text}", status); break;
        case "HIGH": htmlString = htmlString.replace("{color}", "red;").replace("{text}", status); break;
        default: htmlString = htmlString.replace("{color}", "black;").replace("{text}", status); break;

    }
    return htmlString;
}

var fromparent = "";
var toparent = "";

function generatescrum() {
    var docElem = document.documentElement;
    var kanban = document.querySelector('.kboard-area');
    var board = kanban.querySelector('.board');

    var itemContainers = Array.prototype.slice.call(kanban.querySelectorAll('.board-column-content'));
    var columnGrids = [];
    var dragCounter = 0;
    var boardGrid;

    itemContainers.forEach(function (container) {
        var muuri = new Muuri(container, {
            items: '.board-item',
            layoutOnResize: true,
            layoutDuration: 400,
            layoutEasing: 'ease',
            dragEnabled: true,
            dragSort: function () {
                return columnGrids;
            },
            dragSortInterval: 0,
            dragContainer: document.body,
            dragReleaseDuration: 400,
            dragReleaseEasing: 'ease'
        })
            .on('dragStart', function (item) {
                ++dragCounter;
                var ditem = $(item.getElement());
                fromparent = ditem;
                docElem.classList.add('dragging');
                item.getElement().style.width = item.getWidth() + 'px';
                item.getElement().style.height = item.getHeight() + 'px';
            })
            .on('dragEnd', function (item) {
                if (--dragCounter < 1) {
                    docElem.classList.remove('dragging');
                }
            })
            .on('dragReleaseEnd', function (item) {
                if (isClickedToManage) { isClickedToManage = false; return; }
             
                var ditem = $(item.getElement());
                var linkClick = ditem.find('a');
                toparent = ditem.parents("div.board-column-content");


                var taskBoxId = ''
                for (x = 0; x < listTask.length; x++) {
                    if (listTask[x].task_id === ditem.attr("id")) {
                        taskBoxId = listTask[x].task_boxId;
                    }
                }

                if (parseInt(toparent.attr("data-id")) < 4) {
                    if (taskBoxId === '4') {
                        fnJob.setupjobform(jobData); }
                    else { savestatus(ditem.attr("id"), toparent.attr("data-id")); if (isPM) generateButtonApproveAll(ditem.attr("id"), toparent.attr("data-id")); }                   
                }
                else {
                    if (isPM) {
                        savestatus(ditem.attr("id"), toparent.attr("data-id"));
                    }
                    else {
                        fnJob.setupjobform(jobData);
                    }
                }
                linkClick.prop("onclick", null).off("click");
                linkClick.click(function () { managetask(ditem.attr("id"), linkClick.text(), toparent.attr("data-id")) });
                item.getElement().style.width = '';
                item.getElement().style.height = '';

                //console.log("Task Id:" + ditem.attr("id"), "Status_Task_Id:" + toparent.attr("data-id"));


                
                columnGrids.forEach(function (muuri) {
                    muuri.refreshItems();
                });
            })
            .on('layoutStart', function () {
                boardGrid.refreshItems().layout();
               
            });

        columnGrids.push(muuri);

    });

    boardGrid = new Muuri(board, {
        layoutDuration: 400,
        layoutEasing: 'ease',
        dragEnabled: true,
        dragSortInterval: 0,
        dragStartPredicate: {
            handle: '.board-column-header'
        },
        dragReleaseDuration: 400,
        dragReleaseEasing: 'ease'
    });

}

function loadData() {
    console.log("Masokk mari");
    var boardwidth = 220;
    ajaxGet(ajaxUrltaskstatus, { 'rows': 100 }, function (response) {
        taskstatus = [];
        if (response.success) {
            $.each(response.rows, function (key, item) {
                taskstatus.push({
                    'id': item.id,
                    'name': item.name
                }
                );
                boardwidth += boardwidth;
            });
            //$(".kboard-area").width(boardwidth);
        }
    });
    var xid = $("#" + xform + ' [name="id"]').val();
    console.log("XID: " + xid);
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            var user = JSON.parse(sessionStorage.getItem("data"));
            $.each(response.data.teamMember, function (index, value) {
                if (value.user_id.trim() === user.AppUserId.trim()) {
                    console.log("User id telah sama");
                    if (value.act_as.trim() === 'PM') { console.log("IS PM"); isPM = true; }
                }
            });


            if (response.success) {

                jobData = response.data;
                $("#Jobassignboardinit").hide();
                $("#Jobassignboardcontent").show();
                fnJob.setupjobform(response.data);
                xjob_number = response.data.job_number;

            } else {
                $("#Jobassignboardcontent").hide();
            }


        });
    }
}

$(function () {
    console.log("masook mari");
    loadScript([WEB + "/assets/plugins/muuri/web-animations.min.js", WEB + "/assets/plugins/muuri/hammer.min.js", WEB + "/assets/plugins/muuri/muuri.min.js"]);
    loadData();

    
});

