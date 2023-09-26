var ajaxUrlGet = API + "ProjectManagement/Job/GetWithTask";
var ajaxtaskUrlGet = API + "ProjectManagement/Task/Get";
var ajaxUrlassignboard = API + "ProjectManagement/job/assignboard";
var ajaxUrlListPM = API + "ProjectManagement/Job/Listpm";
var ajaxUrlListTaskByJobNumber = API + "ProjectManagement/Task/ListByJobNumber";
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
		if(data.centralresource){
			$.each(data.centralresource, function (key, value) {
                settaskbox(value)
			});
        }
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

function managetask(centralid, id) {
    contentno = 0;
    var modtitle = "Add Task";
    if (id) {
        modtitle = "Update Task";
    }
    showModalAjaxGet(modtitle, WEB_PM + '/task/manage/' + id, {}, function () {
        $('#' + xtaskform + ' [name="central_resources_id"]').val(centralid);
        $('#' + xtaskform + ' [name="job_number"]').val(xjob_number);
        if (id) {
            ajaxGet(ajaxtaskUrlGet + "/" + id, {}, function (response) {
                response = parseJson(response);
                if (response.success) {
                    $("#taskboardinit").hide();
                    $("#taskboardcontent").show();
                    if (response.data) {
                        FormLoadByData(response.data, xtaskform);
                        var xsubtask = response.data.subtask
                        if (xsubtask) {
                            $.each(xsubtask, function (key, value) {
                                
                                addsubtask();
                                curcno = contentno - 1;

                                $('#' + xtaskform + ' [name="subtask[' + curcno + '][id]"]').val(value.id);
                                $('#' + xtaskform + ' [name="subtask[' + curcno + '][sub_task]"]').val(value.sub_task);
                                $('#' + xtaskform + ' [name="subtask[' + curcno + '][description]"]').val(value.description);
                                $('#' + xtaskform + ' [name="subtask[' + curcno + '][start_date]"]').val(value.start_date);
                                $('#' + xtaskform + ' [name="subtask[' + curcno + '][due_date]"]').val(value.due_date);
                                $('#' + xtaskform + ' [name="subtask[' + curcno + '][hour]"]').val(value.hour);
                                if (value.subtaskuser) {
                                    xusr = "";
                                    xseluser = [];
                                    $.each(value.subtaskuser, function (keyusr, valusr) {
                                        xusr += '<option value="' + valusr.id + '" selected>' + valusr.app_fullname + '</option>';
                                        xseluser.push({ id: valusr.id, text: valusr.app_fullname });
                                    });
                                    if (xusr !== "") {
                                        $('#' + xtaskform + ' [name="subtask[' + curcno + '][subtaskusers]"]').append(xusr);
                                        $('#' + xtaskform + ' [name="subtask[' + curcno + '][subtaskusers]"]').select2('data', xseluser);
                                        
                                    }
                                    
                                }
                            });
                        }
                    }

                } else {
                    $("#taskboardcontent").hide();
                }
            });
        } else {
            $("#taskboardinit").hide();
            $("#taskboardcontent").show();
        }

        $('.dtpicker').datetimepicker({ autoclose: true });
        var optionsForm = GetOptionsForm(function () {
            var xret = $("#" + xtaskform).valid();

            return xret;
        }, function (response, statusText, xhr, $form) {
 
            response = parseJson(response);
            if (response.success) {
                settaskdiv(response.data)
                generatescrum();
                SuccessNotif(t_updatesuccess);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xtaskform, optionsForm);
        setFormAction(xtaskform, ajaxUrlassigntask);

 
    },'taskmakerbox');
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
function addsubtask() {
    var divstr = '<div class="subtaskclass"><hr><div class="row"><div class="col-lg-12 nopadding"><div class="pull-right">' +
        '' +
        '<a onclick="removesubtask(this)" class="btn btn-danger m-btn m-btn--icon btn-sm m-btn--pill" type="button" href="javascript:void(0)"><i class="fa fa-trash"></i> Sub Task</a>' +
        '<div style="clear:both"></div></div></div>' +
        '</div ><div class="row">' +
        '<div class="col-lg-12 nopadding">' +
        '<div class="form-group taskfield">' +
        '<label><span class="translate" data-args="task_name">task_name</span> <span class="req">*</span>:</label>' +
        '<input class="form-control form-control-sm m-input m-input--square" name="subtask[' + contentno + '][sub_task]" id="subtaskname_' + contentno +'" required maxlength="50" type="text">' +
        '</div><div class="form-group taskfield">' +
        '<select required id="userlist' + contentno + '" name="subtask[' + contentno + '][subtaskusers]" multiple class="form-control form-control-sm m-input m-input--square"></select>' +
        '</div></div></div>' +
        '<div class="row">' +
        '<div class="col-lg-6  taskfieldnp">' +
        '<div class="form-group ">' +
        '<label><span class="translate" data-args="description">description</span> <span class="req">*</span>:</label>' +
        '<textarea class="form-control form-control-sm m-input m-input--square" name="subtask[' + contentno + '][description]" rows="4" id="subtaskdescription_' + contentno +'"></textarea>' +
        '</div></div>' +
        '<div class="col-lg-4  taskfieldnp">' +
        '<div class="form-group ">' +
        '<label><span class="translate" data-args="end_date">end_date</span> <span class="req">*</span>:</label>' +
        '<div class="input-group ">' +
        '<input type="text" class="dtpicker form-control form-control-sm m-input m-input--square" placeholder="Select date & time" id="start_' + contentno +'" name="subtask[' + contentno + '][start_date]" />' +
        '<div class="input-group-append">' +
        '<span class="input-group-text"><i class="fa fa-calendar-alt"></i></span>' +
        '</div></div></div>' +
        '<div class="form-group ">' +
        '<label><span class="translate" data-args="start_date">start_date</span> <span class="req">*</span>:</label>' +
        '<div class="input-group ">' +
        '<input type="text" class="dtpicker form-control form-control-sm m-input m-input--square" placeholder="Select date & time" id="due_' + contentno +'" name="subtask[' + contentno + '][due_date]" />' +
        '<div class="input-group-append">' +
        '<span class="input-group-text"><i class="fa fa-calendar-alt"></i></span>' +
        '</div></div></div></div><div class="col-lg-2  taskfieldnp"><div class="form-group "><label><span class="translate" data-args="hour">hour</span></label><input type="hidden" name="subtask[' + contentno + '][id]" /><input type="number" class="form-control form-control-sm m-input m-input--square" placeholder="hour" id="due_' + contentno + '" name="subtask[' + contentno + '][hour]" /></div></div></div>';
    $('#contentsubtask').append(divstr);
    $('.dtpicker').datetimepicker({ autoclose: true });
    $('#userlist' + contentno).setcombobox({
        data: { 'rows': 100 },
        url: ajaxUrlListUser,
        maxItems: 0,
        searchparam: 'search_app_username',
        labelField: 'app_fullname',
        valueField: 'user_id',
    });

    contentno++;

    
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
    strct += value.name;
    strct += '<a class="pull-right" href="javascript:void(0)" onclick="managetask(\''+value.id+'\',\'\')">';
    strct += '<i class="fa fa-plus-square m--font-info"></i>';
    strct += '</a>';
    strct += '</div>';
    strct += '<div class="board-column-content" id="central_' + value.id +'">';
    strct += '</div>';
    strct += '</div>';
    strct += '</div>';
    $("#jobtasksrc").append(strct);
    xnotask++;
} 
function settaskdiv(value) {
  
    strct = '<div class="board-item" id="' + value.id + '"><div class="board-item-content"><a class="pull-right" href="javascript:void(0)" onclick="managetask(\'' + value.central_resources_id + '\',\'' + value.id + '\')"><i class="fa fa-edit"></i></a>' + value.task_name + '<hr><div class="priority' + value.task_priority_id + '">priority:' + value.task_priority_id +'</div></div></div>';
    $('#central_' + value.central_resources_id).append(strct);

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

