var ajaxUrlGet = API + "/ProjectManagement/Job/GetWithTask";
var ajaxtaskUrlGet = API + "/ProjectManagement/Task/Get";
var ajaxUrlassignboard = API + "/ProjectManagement/job/assignboard";
var ajaxUrlListPM = API + "/ProjectManagement/Job/Listpm";
var ajaxUrlListTaskByJobNumber = API + "/ProjectManagement/Task/ListByJobNumber";
var ajaxUrlListUser = API_PM + "/AccountManagement/list";
var ajaxUrlassigntask = API_PM + "/Task/manage";
var xform = "JobassignboardForm";   
var xtaskform = "TaskForm";
var xjob_number = "";
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
        if (data.pm) {
            $("#pmname").text(data.pm.app_fullname);
        }
        initlanguage(lang_pm);
        FormTextByData(data, xform);
        var taskstatus = [
            { 'id': '1', 'name': 'To Do' },
            { 'id': '2', 'name': 'In Progress' },
            { 'id': '3', 'name': 'Done' },
            { 'id': '4', 'name': 'Approved' }
        ];
            $.each(taskstatus, function (key, item) {
                settaskbox(item);
			});
 
        if (data.tasklist) {
            $.each(data.tasklist, function (key, value) {
                settaskdiv(value)
            });
        }
        generatescrum();
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
$(document).on('change','.cprogress', function () {
    
    var valeur = 0;
    var eqty = $('.cprogress').length;

    $('.cprogress:checked').each(function () {
        valeur++;
    });

    var xpercent = (valeur / eqty) *100
    $('.progress-bar').css('width', xpercent + '%').attr('aria-valuenow', valeur);
});
function managetask(id, modtitle) {
    contentno = 0;
    showModalAjaxGet(modtitle, WEB_PM + '/task/detil/' + id, {}, function () {
        ajaxGet(ajaxtaskUrlGet + "/" + id, {}, function (response) {
            var xsubtask = response.data.subtask;
            $("#taskdesc").html(response.data.description);
            if (xsubtask) {
                $("#tasklist").empty();
            $.each(xsubtask, function (key, value) {
                vstr = '<div class="m-checkbox-list"><label class="m-checkbox "><input type = "checkbox" ' + xcheck + ' class="cprogress" name="central_resource_id[]" value="' + value.id + '" data-name="' + value.sub_task + '" >' + value.sub_task + '<span></span ></label ></div>';
                $("#tasklist").append(vstr);
            });
        }

    });

        }, 'taskmakerbox');
 
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

function savetask() {
    var idcentral = $('#TaskForm [name="id"]').val();
    var task_name = $('#TaskForm [name="task_name"]').val();
    
    $("#central_" + idcentral).append('<div class="board-item"><div class="board-item-content"><h6>' + task_name+'</h6> xxxxxx</div></div>');
    $('#TaskForm [id="taskclose"]').trigger("click");
    generatescrum()
}
var xnotask = 0;
function settaskbox(value) {
    xnotask++;
    strct = '<div class="board-column" id="task_' + xnotask+'">';
    strct += '<div class="board-column-header">';
    strct += value.name ;
   
    strct += '</div>';
    strct += '<div class="board-column-content" id="central_' + value.id +'">';
    strct += '</div>';
    strct += '</div>';
    strct += '</div>';
    $("#jobtasksrc").append(strct);
    xnotask++;
} 
function settaskdiv(value) {
  
    strct = '<div class="board-item" id="' + value.id + '"><div class="board-item-content">' + '<a href="javascript:void(0)" onclick="managetask(\'' + value.id + '\',\'' + value.task_name + '\')">'+ value.task_name + '</a><hr><div class="priority' + value.task_priority_id + '">priority:' + value.task_priority_id +'</div></div></div>';
    $('#central_' + value.task_status_id).append(strct);

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
                    
                    fromparent = ditem.parents(".board-column-content");
                    console.log($(fromparent).attr("id"))
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

                    var ditem = $(item.getElement());
                    toparent = ditem.parents("div.board-column-content");
                    console.log(ditem.attr("id"));
 
                    console.log(toparent.attr("id"));
                    item.getElement().style.width = '';
                    item.getElement().style.height = '';
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
$(function () {
    
    loadScript([WEB + "/assets/plugins/muuri/web-animations.min.js",WEB + "/assets/plugins/muuri/hammer.min.js",WEB + "/assets/plugins/muuri/muuri.min.js"]);

    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            if (response.success) {
                $("#Jobassignboardinit").hide();
                $("#Jobassignboardcontent").show();
                fnJob.setupjobform(response.data);
                xjob_number = response.data.job_number;

            } else {
                $("#Jobassignboardcontent").hide();
            }
            

        });
    }

   
});

