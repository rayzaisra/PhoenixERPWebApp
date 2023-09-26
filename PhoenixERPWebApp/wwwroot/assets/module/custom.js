; (function () {
    axios.defaults.baseURL = API
    function retryFailedRequest(err) {
        endload();
        if (err.status === 500 && err.config && !err.config.__isRetryRequest) {
            err.config.__isRetryRequest = true;
            return axios(err.config);
        }
        throw err;
    }
    axios.interceptors.request.use(function (config) {
        startload();

        return config;
    }, retryFailedRequest);
    axios.interceptors.response.use((response) => {
        endload();

        return response;
    }, function (error) {
        endload();
        if (error.response.status == 401) {

            location.replace(WEB + "/Core/Authentication?status=sessionExpired");

        } else {
            return Promise.reject(error);
        }


    });
    if (!NOT_USING_AUTH_HEADER) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + $('meta[name=JWT_TOKEN]').attr("content");
        axios.defaults.withCredentials = true
    }
    axios.defaults.headers.common['Content-Type'] = "application/json"
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
    //    axios.defaults.withCredentials = true
})()
var NOTIF_TIMEOUT = 2000;
var NOTIF_TOP_CENTER = "top-center";
var NOTIF_TOP_LEFT = "top-left";
var NOTIF_TOP_RIGHT = "top-right";
var NOTIF_BOTTOM_CENTER = "bottom-center";
var NOTIF_BOTTOM_LEFT = "bottom-left";
var NOTIF_BOTTOM_RIGHT = "bottom-right";
var NOTIF_DEFAULT = "primary";
var NOTIF_DANGER = "danger";
var NOTIF_WARNING = "warning";
var NOTIF_SUCCESS = "success";

function GetValueOrDefault(value, defaultValue) {
    return typeof value == "undefined" ? defaultValue : value;
}

function GeneralNotif(message, status, timeout, pos) {
    message = GetValueOrDefault(message, "");
    status = GetValueOrDefault(status, NOTIF_DEFAULT);
    timeout = GetValueOrDefault(timeout, NOTIF_TIMEOUT);
    pos = GetValueOrDefault(pos, NOTIF_TOP_CENTER);
    var NOTIF_DEFAULT = "primary";
    var NOTIF_DANGER = "danger";
    var NOTIF_WARNING = "warning";
    var NOTIF_SUCCESS = "success";

    if (status === NOTIF_SUCCESS) {
        toastr.success(message);
    } else if (status === NOTIF_WARNING) {
        toastr.warning(message);
    } else if (status === NOTIF_DANGER) {
        toastr.error(message);
    } else {
        toastr.success(message);
    }


    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function CustomNotif(message, options) {
    options = typeof options != "undefined" ? options : { status: NOTIF_DEFAULT, timeout: NOTIF_TIMEOUT, pos: NOTIF_TOP_CENTER };
    GeneralNotif(message, options.status, options.timeout, options.pos);
}

function DangerNotif(message, options) {

    options = typeof options != "undefined" ? options : { status: NOTIF_DEFAULT, timeout: NOTIF_TIMEOUT, pos: NOTIF_TOP_CENTER };
    GeneralNotif(message, NOTIF_DANGER, options.timeout, options.pos);
}

function WarningNotif(message, options) {
    options = typeof options != "undefined" ? options : { status: NOTIF_DEFAULT, timeout: NOTIF_TIMEOUT, pos: NOTIF_TOP_CENTER };
    GeneralNotif(message, NOTIF_WARNING, options.timeout, options.pos);
}
function SuccessNotif(message, options) {
    options = typeof options != "undefined" ? options : { status: NOTIF_DEFAULT, timeout: NOTIF_TIMEOUT, pos: NOTIF_TOP_CENTER };
    GeneralNotif(message, NOTIF_SUCCESS, options.timeout, options.pos);
}

function DefaultNotif(message, options) {
    options = typeof options != "undefined" ? options : { status: NOTIF_DEFAULT, timeout: NOTIF_TIMEOUT, pos: NOTIF_TOP_CENTER };
    GeneralNotif(message, NOTIF_DEFAULT, options.timeout, options.pos);
}

function ajaxGet(e, p, cb) {

    axios.get(e, {
        params: p
    }).then(function (response) {

        response = parseJson(response);
        cb(response.data);
    })
        .catch(function (error) {
            console.log(error);

            DangerNotif(error);
        });
}
function ajaxDelete(e, p, cb) {
    axios.delete(e, {
        params: p
    })
        .then(function (response) {
            response = parseJson(response);
            cb(response.data);
        })
        .catch(function (error) {
            console.log(error);
            DangerNotif(error);
        })
}
function ajaxPut(e, p, cb) {
    axios.put(e, p)
        .then(function (response) {
            response = parseJson(response);
            cb(response.data);
        })
        .catch(function (error) {
            console.log(error);
            DangerNotif(error);
        })
}
function ajaxPost(e, p, cb) {
    axios.post(e, p)
        .then(function (response) {
            response = parseJson(response);
            cb(response.data);
        })
        .catch(function (error) {
            console.log(error);
            DangerNotif(error);
        })
}

function ajaxGetJquery(e, p, cb) {
    p = typeof p != "undefined" ? p : {};
    cb = typeof cb != "undefined" ? cb : function (res) { };
    $.ajax({
        method: "GET",
        url: e,
        data: p
    })
        .done(cb)
        .fail(function (error) {

            console.log(error);
            DangerNotif(error);
        });
}

function ajaxPostJquery(e, p, cb) {
    p = typeof p != "undefined" ? p : {};
    cb = typeof cb != "undefined" ? cb : function (res) { };
    $.ajax({
        method: "POST",
        url: e,
        data: p
    })
        .done(cb)
        .fail(function (error) {
            console.log(error);
            DangerNotif(error);
        });
}

//api
function ajaxGetApi(e, p, cb) {
    ajaxGet(API + e, p, cb);
}

function ajaxPostApi(e, p, cb) {
    ajaxPost(API + e, p, cb);
}

function ajaxGetApiFinance(e, p, cb) {
    ajaxGet(API_FINANCE + e, p, cb);
}

function ajaxPostApiFinance(e, p, cb) {
    ajaxPost(API_FINANCE + e, p, cb);
}

function ajaxGetApiHris(e, p, cb) {
    ajaxGet(API_HRIS + e, p, cb);
}

function ajaxPostApiHris(e, p, cb) {
    ajaxPost(API_HRIS + e, p, cb);
}


function ajaxGetApiGeneral(e, p, cb) {
    ajaxGet(API_GENERAL + e, p, cb);
}

function ajaxPostApiGeneral(e, p, cb) {
    ajaxPost(API_GENERAL + e, p, cb);
}

function ajaxGetApiPM(e, p, cb) {
    ajaxGet(API_PM + e, p, cb);
}

function ajaxPostApiPM(e, p, cb) {
    ajaxPost(API_PM + e, p, cb);
}

//web
function ajaxGetWeb(e, p, cb) {
    ajaxGetJquery(WEB + e, p, cb);
}

function ajaxPostWeb(e, p, cb) {
    ajaxPostJquery(WEB + e, p, cb);
}

function ajaxGetWebFinance(e, p, cb) {
    ajaxGetJquery(WEB_FINANCE + e, p, cb);
}

function ajaxPostWebFinance(e, p, cb) {
    ajaxPostJquery(WEB_FINANCE + e, p, cb);
}

function ajaxGetWebHris(e, p, cb) {
    ajaxGetJquery(WEB_HRIS + e, p, cb);
}

function ajaxPostWebHris(e, p, cb) {
    ajaxPostJquery(WEB_HRIS + e, p, cb);
}


function ajaxGetWebGeneral(e, p, cb) {
    ajaxGetJquery(WEB_GENERAL + e, p, cb);
}

function ajaxPostWebGeneral(e, p, cb) {
    ajaxPostJquery(WEB_GENERAL + e, p, cb);
}

function ajaxGetWebPM(e, p, cb) {
    ajaxGetJquery(WEB_PM + e, p, cb);
}

function ajaxPostWebPM(e, p, cb) {
    ajaxPostJquery(WEB_PM + e, p, cb);
}


var MODAL_BLOCKUI;
var MODAL_FOR_ALL;
var MODAL_CUSTOM = {};
var MODAL_FOR_ALL_ID = "modal_for_all";



function hideModalContent() {
    if (typeof MODAL_FOR_ALL != "undefined") {
        MODAL_FOR_ALL.hide();
    }
}

function showCustomModal(id, opts) {
    opts = typeof opts != "undefined" ? opts : {};
    if (typeof opts["style"] != "undefined") {
        $("#" + id).attr("style", opts["style"]);
    }
    MODAL_CUSTOM[id] = UIkit.modal("#" + id, opts);
    MODAL_CUSTOM[id].show();
}

function hideCustomModal(id) {
    if (typeof MODAL_CUSTOM[id] != "undefined") {
        MODAL_CUSTOM[id].hide();
    }
}

function hideAllModal() {
    $.each(MODAL_CUSTOM, function (key, value) {
        value.hide();
    });
}

function showModalBlockUI(content) {
    MODAL_BLOCKUI = UIkit.modal.blockUI(content);
}

function hideModalBlockUI() {
    if (typeof MODAL_BLOCKUI != "undefined") {
        MODAL_BLOCKUI.hide();
    }
}

function showModalAjaxGet(title, url, p, cb, xclass) {

    ajaxGetJquery(url, p, function (response) {
        if (xclass) {
            showModalContentExtend(title, response, xclass);
        } else {
            showModalContent(title, response);
        }

        if (typeof cb != "undefined") {
            setTimeout(cb, 400);
        }

    });
}

function showModalAjaxPost(title, url, p, cb) {

    ajaxPostJquery(url, p, function (response) {

        showModalContent(title, response);
        if (typeof cb != "undefined") {
            setTimeout(cb, 400);
        }
    });

}

function checkLogin() {
    /*$.get("${urlApp}/checkSession",function(resultSession){
     resultSession = parseJson(resultSession);
     if(!resultSession.isLogin){
     alert("Session login anda sudah habis ! silahkan login lagi");
     window.location.href="${urlApp}/login";
     }
     });*/
}
function checkActivity(timeout, interval, elapsed) {
    if ($.active) {
        elapsed = 0;
        $.active = false;
        $.get('checkEBOnlineSession');
    }
    if (elapsed < timeout) {
        elapsed += interval;
        setTimeout(function () {
            checkActivity(timeout, interval, elapsed);
        }, interval);
    } else {
        window.location.href = 'login'; // Redirect to login page
    }
}

function parseJson(data) {
    if (typeof data == "string")
        data = jQuery.parseJSON(data);

    return data;
}
function FormLoadByAjaxGet(formid, url, p, cb) {
    ajaxGet(url, p, function (data) {
        FormLoadByDataUsingName(formid, data, cb);
    });
}

function FormLoadByAjaxPost(formid, url, p, cb) {
    ajaxPost(url, p, function (data) {
        FormLoadByDataUsingName(formid, data, cb);
    });
}
Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}
function FormTextByData(data, formid, cb) {
    attrKey = typeof attrKey === "undefined" ? "name" : attrKey;
    $.each(data, function (key, value) {
        if (value !== null) {


            var target = $('#' + formid + ' [id="' + key + '"]')
            if (target) {
                if (target.hasClass("xdate")) {
                    d = new Date(value);
                    value = [(d.getMonth() + 1).padLeft(),
                    d.getDate().padLeft(),
                    d.getFullYear()].join('/');
                } else if (target.hasClass("xdatetime")) {
                    d = new Date(value);
                    value = [(d.getMonth() + 1).padLeft(),
                    d.getDate().padLeft(),
                    d.getFullYear()].join('/') + ' ' +
                        [d.getHours().padLeft(),
                        d.getMinutes().padLeft(),
                        d.getSeconds().padLeft()].join(':');
                }
                if (target.is("input") || target.is("textarea")) {

                    if (target.hasClass("dtpicker")) {
                        xdate = moment(value).format('YYYY-MM-DD HH:mm');
                        target.val(xdate);
                    } else if (target.hasClass("datepicker")) {
                        xdate = moment(value).format('YYYY-MM-DD');
                        target.val(xdate);
                    } else {
                        target.val(value);
                    }
                } else if (target.is("div") || target.is("span") || target.is("h6") || target.is("h5") || target.is("h4") || target.is("h3") || target.is("h2") || target.is("h1")) {
                    target.text(value);
                }


            }

        }
    });
    if (typeof cb !== "undefined") {
        cb();
    }
}
function FormLoadByData(data, formid, attrKey, cb) {

    attrKey = typeof attrKey == "undefined" ? "name" : attrKey;
    $.each(data, function (key, value) {

        //console.log(key+' '+ value);
        if ($('#' + formid + ' [' + attrKey + '="' + key + '"]').attr('type') == "datepicker") {
            //window.console.log('1');
            $('#' + formid + ' [' + attrKey + '="' + key + '"]').val(value);
        }
        else if ($('#' + formid + ' [' + attrKey + '="' + key + '"]').attr('data-role') == "combobox") {
            //window.console.log('2');
            $('#' + formid + ' [' + attrKey + '="' + key + '"]').val(value);
        }
        else if ($('#' + formid + ' [' + attrKey + '="' + key + '"]').attr('type') === "checkbox") {

            $('#' + formid + ' [' + attrKey + '="' + key + '"][value="' + value + '"]').prop('checked', true);
        }
        else if ($('#' + formid + ' [' + attrKey + '="' + key + '"]').attr('type') === "radio") {
            $('#' + formid + ' [' + attrKey + '="' + key + '"][value="' + value + '"]').prop('checked', true);
        }
        else {

            $('#' + formid + ' [' + attrKey + '="' + key + '"]').val(value);
        }
        if ($('#' + formid + ' #' + key).hasClass("frmtext")) {
            if ($('#' + formid + ' #' + key).hasClass("dtpicker")) {
                xdate = moment(value).format('YYYY-MM-DD HH:mm');
                $('#' + formid + ' #' + key).text(xdate);
            } else if ($('#' + formid + ' #' + key).hasClass("datepicker")) {
                xdate = moment(value).format('YYYY-MM-DD');
                $('#' + formid + ' #' + key).text(xdate);
            } else {
                $('#' + formid + ' #' + key).text(value);
            }

        }

        //console.log(attrKey + ":" + key + "=" + value);
        if ($('#' + formid + ' [' + attrKey + '="' + key + '"]').hasClass("dtpicker")) {
            xdate = moment(value).format('YYYY-MM-DD HH:mm');
            $('#' + formid + ' [' + attrKey + '="' + key + '"]').val(xdate);
        }
        if ($('#' + formid + ' [' + attrKey + '="' + key + '"]').hasClass("datepicker")) {
            xdate = moment(value).format('YYYY-MM-DD');
            $('#' + formid + ' [' + attrKey + '="' + key + '"]').val(xdate);
        }
        //tambahan untuk label md keatas kalau ada value;

        $('#' + formid + ' [' + attrKey + '="' + key + '"]').trigger("change");
    });
    if (typeof cb != "undefined") {
        cb();
    }
}
function FormLoadByDataUsingID(data, formid, cb) {
    cb = typeof cb != "undefined" ? cb : function () { };
    FormLoadByData(data, formid, "id", cb);
}
function FormLoadByDataUsingName(data, formid, cb) {
    cb = typeof cb != "undefined" ? cb : function () { };
    FormLoadByData(data, formid, "name", cb);
}

var TABLES = {};

function GenerateTableHeader(tableId, columns) {
    var headers = "<thead><tr>";
    $.each(columns, function (key, value) {
        headers += "<th field='" + value.data + "'>" + value.title + "</th>";
    });
    headers += "</tr></thead>";
    return headers;
    $("#" + tableId).html(headers);
}

function InitTable(tableId, ajaxUrl, ajaxParams, columns, sortby, sortdir) {
    //GenerateTableHeader(tableId, columns);
    var pglength = 10;
    if (typeof ajaxParams === "function") {
        $.each(ajaxParams(), function (key, value) {
            if (key === 'rows') {
                pglength = value;

            }

        });
    } else {
        $.each(ajaxParams, function (key, value) {
            if (key === 'rows') {
                pglength = value;

            }
        });
    }

    TABLES[tableId] = $('#' + tableId).DataTable({
        "responsive": true,
        "drawCallback": function (settings) {
            setTimeout(function () {
                //  table.responsive.rebuild();
                //  table.responsive.recalc();
            }, 50);
        },
        "pageLength": pglength,
        "autoWidth": true,
        "processing": true,
        "serverSide": true,
        "stateSave": true,
        scrollX: true,
        scrollCollapse: true,
        "bFilter": false,
        "bSort": true,
        "dom": "\tf<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
        ajax: function (data, callback, settings) {

            var page = 0;
            if (data.start !== 0) {
                page = data.start / data.length;
            }


            var allParams = {};
            if (sortby) {
                allParams["sortBy"] = sortby;
                allParams["sortDir"] = sortdir;

            }
            if (TABLES[tableId]) {
                if (data.order) {
                    if (data.order[0]) {
                        allParams["sortDir"] = data.order[0].dir;
                        allParams["sortBy"] = data.columns[data.order[0].column].data
                    }

                }

            }

            allParams["rows"] = data.length;
            allParams["page"] = page + 1;



            if (typeof ajaxParams === "function") {
                $.each(ajaxParams(), function (key, value) {
                    allParams[key] = value;
                });
            } else {
                $.each(ajaxParams, function (key, value) {

                    allParams[key] = value;
                });
            }

            if (allParams["sortBy"] === null) {
                allParams["sortDir"] = null;
            }
            ajaxGet(ajaxUrl, allParams, function (res) {
                res = parseJson(res);
                //res = res.data;

                LIST_DATA = new Object();
                if (res.success) {
                    $.each(res.rows, function (keyResult, valueResult) {
                        //LIST_DATA[valueResult.id] = valueResult;
                    });
                    callback({

                        recordsTotal: res.recordsTotal,
                        recordsFiltered: res.recordsFiltered,
                        data: res.rows
                    });
                } else {

                    DangerNotif(res.message)
                }
            });
        },
        "columns": columns
    });

}
function setcolvis(tl, columnsIndex, ObjId) {


    if ($('#colvisdiv').length) {
        $('#colvisdiv').show();
        if (!ObjId) {
            ObjId = 'colvisdiv';
        }
        $('#' + ObjId).empty();
        $('#' + ObjId).append('<a class="btn btn-default btn-sm" href="javascript:;" data-toggle="dropdown"><i class="fa fa-list"></i> show / hide <i class="fa fa-angle-down"></i></a>')
        xcolvistr = '<ul class="dropdown-menu pull-right">';
        $.each(columnsIndex, function (index, value) {
            xcolvistr += '<li>&nbsp;&nbsp;<label><input type="checkbox" value="' + index + '" onchange="excolvis(\'' + tl + '\',this)" checked />' + value.title + '</label></li>';

        });
        xcolvistr += '</ul>';
        $('#' + ObjId).append(xcolvistr);
    }

}
function excolvis(tl, x) {
    var xchecked = x.checked;
    var xcolumn = TABLES[tl].column(x.value);
    xcolumn.visible(xchecked);
}
function ReloadGrid(tableId) {
    TABLES[tableId] = $('#' + tableId).DataTable();

    TABLES[tableId].ajax.reload(function (json) {

    });
}

function GetOptionsForm(showRequestForm, showResponseForm) {
    return {
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + $('meta[name=JWT_TOKEN]').attr("content"));
        },
        beforeSubmit: showRequestForm,
        success: showResponseForm
    };
}
function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}
function InitForm(formId, optionsForm) {

    optionsForm.method = $('#' + formId).attr("method");
    $('#' + formId)[0].reset();
    $('#' + formId).ajaxForm(optionsForm);
    //$("#" + formId).parsley().reset();

}
function parseISOString(s) {
    return new Date(s);

}
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function setFormAction(formId, url) {
    $("#" + formId).attr("action", url);
}

function confirmDialog(text, cb) {
    UIkit.modal.confirm(text, cb);
}
//---------------add h3----------------------------------------
//#HIJACK link with class openmodal to open modal
$(document).on('click', 'a.openmodal', function (e) {
    vtitle = "";
    e.preventDefault();
    var xmen = $(this);
    var vlink = xmen.attr('href');
    var modaltype = xmen.attr('modaltype');
    var modalcache = xmen.attr('modalcache');
    if (modaltype == null) modaltype = "";
    vtitle = xmen.attr('title');
    if (modalcache) {
        MODAL_FOR_ALL_ID = "modal_for_all";
        if (vtitle) {
            MODAL_FOR_ALL_ID = vtitle.replace(/\W+/g, "_");
        }
        if ($("#" + MODAL_FOR_ALL_ID).length == 0) {
            ajaxGetJquery(vlink, {}, function (response) {
                showModalContentExtend(vtitle, response, modaltype, modalcache);
                if (typeof cb != "undefined") {
                    setTimeout(cb, 400);
                }

            });
        } else {

            UIkit.modal("#" + MODAL_FOR_ALL_ID).show();

        }
    } else {

        ajaxGetJquery(vlink, {}, function (response) {
            showModalContentExtend(vtitle, response, modaltype, modalcache);
            if (typeof cb != "undefined") {
                setTimeout(cb, 400);
            }

        });
    }

});
//---------reform modal
function showModalContent(title, content) {
    showModalContentExtend(title, content, 'normal');
}
function showModalContentExtend(title, content, modalclass) {
    MODAL_FOR_ALL_ID = "modal_for_all";
    if (title) {
        MODAL_FOR_ALL_ID = title.replace(/\W+/g, "_");
    }
    if ($("#" + MODAL_FOR_ALL_ID).length == 0) {


        mtypeclass = "";
        if (modalclass) {
            mtypeclass = modalclass;

        } else {
            mtypeclass = "normalbox";
        }
        var templatemodal = '<div id="' + MODAL_FOR_ALL_ID + '" class="' + mtypeclass + ' modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">' +
            '<div class="modal-dialog ">' +
            '<div class="modal-content"><div class="modal-header"><h4>' + title + '</h4><a class="close" data-dismiss="modal">×</a>' +
            '</div>' +
            '<div class="modal-body">' + content +
            '</div>' +
            '</div></div></div>';



        $(document.body).append(templatemodal);
    }
    $("#" + MODAL_FOR_ALL_ID).modal();
    $("#" + MODAL_FOR_ALL_ID).modal('show');

    $("#" + MODAL_FOR_ALL_ID).on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

}
//------------------------------------------------------------

//custom h3-----------------------------------------------------------
var loadv = 0;
function startload() {

    if (loadv === 0) {
        if ($('div.tplpanel').length > 0) {
            /*
            $('div.tplpanel').block({
                message: '<h1>Processing</h1>',
                css: { border: '3px solid #a00' }
            }); 
            */
        }

    }
    loadv++;
}
function endload() {
    loadv--;

    if (loadv <= 0) {
        loadv = 0;
        if ($('div.tplpanel').length > 0) {
            //$('div.tplpanel').unblock();
        }
    }
}
var loadvx = 0;
function startprocess() {

    if (loadvx === 0) {
        if ($('div.tplpanel').length > 0) {

            $('div.tplpanel').block({
                message: '<h1>Processing</h1>',
                css: { border: '3px solid #a00' }
            });
        }
    }
    loadv++;
}
function endprocess() {
    loadvx--;

    if (loadvx <= 0) {
        loadvx = 0;
        if ($('div.tplpanel').length > 0) {
            $('div.tplpanel').unblock();
        }
    }
}
$.ajaxSetup({
    beforeSend: function () {
        startload();
    },

    complete: function () {
        endload();
    }
});
var loadedScripts = new Array();
function loadScript(scriptArray, isasync) {

    scrsync = false;
    if (isasync) {
        scrsync = true;
    }

    if ($.isArray(scriptArray)) {
        $.each(scriptArray, function (intIndex, objValue) {
            if ($.inArray(objValue, loadedScripts) < 0) {
                $.ajax({
                    async: scrsync,
                    cache: true,
                    type: 'GET',
                    url: objValue,
                    success: function () { loadedScripts.push(objValue); },
                    dataType: 'script'

                });
            }
        });
    }
    else {
        if ($.inArray(scriptArray, loadedScripts) < 0) {
            $.ajax({
                async: scrsync,
                cache: true,
                type: 'GET',
                url: scriptArray,
                success: function () { loadedScripts.push(scriptArray); },
                dataType: 'script'

            });
        }
    }
}
var currentuploadbox;
function getuploadbox(boxid, id, filetype) {

    currentuploadbox = boxid;
    showModalAjaxGet("File Upload", WEB_GENERAL + '/file', {}, function () {
        $('#damUploadId').val(id);
        $('#customFileType').val(filetype);
    });
}

function base64ToBlob(base64, mimetype, slicesize) {
	if (!window.atob || !window.Uint8Array) {
		// The current browser doesn't have the atob function. Cannot continue
		return null;
	}
	mimetype = mimetype || '';
	slicesize = slicesize || 512;
	var bytechars = atob(base64);
	var bytearrays = [];
	for (var offset = 0; offset < bytechars.length; offset += slicesize) {
		var slice = bytechars.slice(offset, offset + slicesize);
		var bytenums = new Array(slice.length);
		for (var i = 0; i < slice.length; i++) {
			bytenums[i] = slice.charCodeAt(i);
		}
		var bytearray = new Uint8Array(bytenums);
		bytearrays[bytearrays.length] = bytearray;
	}
	return new Blob(bytearrays, { type: 'application/x-unknown' });
};


function DownloadFile(url, id) {
	$.ajax({
		method: 'GET',
		url: url,
		data: { id: id },
		beforeSend: function (xhdr) {
			xhdr.setRequestHeader(
				"Authorization", "Bearer " + $('meta[name=JWT_TOKEN]').attr("content"));
		}
	}).done(function (data, statusText, xhr) {
		var disposition = xhr.getResponseHeader('Content-Disposition');
		var type = xhr.getResponseHeader('Content-Type');
		
        var blob = base64ToBlob(data.data.file, data.data.mimeType , 512);
        //var filename = data.data.fileName + data.data.mimeType;
        var filename = data.data.fileName;
		var link = document.createElement('a');
		//link.href = window.URL.createObjectURL(blob);
		//link.download = data.data.fileName;
		//link.click();
		
		if (typeof window.navigator.msSaveBlob !== 'undefined') {
			// IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
			window.navigator.msSaveBlob(blob, filename);
			
		} else {
			var URL = window.URL || window.webkitURL;
			var downloadUrl = URL.createObjectURL(blob);

			if (filename !== '') {
				// use HTML5 a[download] attribute to specify filename
				var a = document.createElement("a");
				// safari doesn't support this yet
				if (typeof a.download === 'undefined') {
					window.location = downloadUrl;
					
				} else {
					
					a.href = downloadUrl;
					a.download = filename;
					document.body.appendChild(a);
					a.click();
				}
			} else {
				
				window.location = downloadUrl;
			}
			
			setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
		}

	}).fail(function (xhdr, statusText, errorText) {
		console.log(xhdr);
		DangerNotif('File Not Found');
	});

}

$.fn.setuploadbox = function (options) {
    var settings = $.extend({
        fileType: ''

    }, options);

	var ajaxUrlGetFile = API + "General/File/Getfile";
    currentid = this.attr("id");
    currentval = this.val();

	var fileName = '';
	var idContent = '';
	
    if (currentval) {
        if (currentval !== 'null') {
			var stringSplit = currentval.split("::");
			fileName = stringSplit[1];
			idContent = stringSplit[0];
			
			//currentval = '<a href="javascript:;" onclick="DownloadFile(\'' + ajaxUrlGetFile + '\',\'' + currentval + '\');" >' + currentval + '</a>';
			currentval = '<a href="javascript:;" onclick="DownloadFile(\'' + ajaxUrlGetFile + '\',\'' + idContent + '\');" >' + fileName + '</a>';
        } else {
			currentval = "";
			fileName = '';
			idContent = '';
        }

    }

    btnuplstr = '<div class="input-group"><label id="lblx' + currentid + '"   class="form-control form-control-sm m-input">' + currentval + '</label><div class="input-group-append"><a class="btn btn-success btn-sm" onclick="getuploadbox(\'' + currentid + '\',\'' + idContent + '\',\'' + settings.fileType + '\')" href="javascript:void(0)">' +
		'<i class="fa fa-upload" ></i ></a></div></div>';

    this.hide();
    this.after(btnuplstr);
};
$.fn.setdatetimepicker = function () {

    this.datetimepicker({ autoclose: true });
};
$.fn.setdatepicker = function () {

    this.datepicker({ autoclose: true });
};
$.fn.setcombobox = function (options) {

    var settings = $.extend({
        url: '',
        maxItems: 1,
        minimumInputLength: 1,
        data: {},
        initdata: {},
        render: null,
        create: null,
        placeholder: "",
        searchparam: 'search',
        valueField: 'id',
        labelField: 'title',
        searchField: 'title',
        type: 'GET'
    }, options);

    this.select2({
        data: settings.initdata,
        allowClear: true,
        enable: true,
        width: "100%",
        theme: "bootstrap",
        templateResult: function (rd) {

            if (settings.render) {

                return settings.render(rd);

            } else {
                return rd.text;
            }

        },
        escapeMarkup: function (m) {
            return m;
        },
        placeholder: settings.placeholder,
        minimumInputLength: 0,
        maximumSelectionLength: settings.maxItems,
        ajax: {
            url: settings.url,
            delay: 1000,
            type: settings.type,
            data: function (params) {

                allParams = settings.data;
                allParams[settings.searchparam] = "";
                if (params.term !== undefined) {
                    allParams[settings.searchparam] = params.term;
                }

                return allParams;
            },
            cache: true,
            processResults: function (data) {

                var results = [];
                $.each(data.rows, function (index, val) {
                    results.push({
                        id: val[settings.valueField],
                        text: val[settings.labelField],
                        odata: val
                    });
                });

                return {
                    results: results
                };
            }
        },
        dropdownCssClass: "bigdrop",
        escapeMarkup: function (m) {
            return m;
        }
    });
};
var loadedLanguge = new Array();
initlanguage('Global');
function initlanguage(scriptLanguage) {

    if ($.inArray(scriptLanguage, loadedLanguge) < 0) {
        $.i18n().load({
            'en': WEB + '/assets/languages/' + scriptLanguage + '/en.json',
            'id': WEB + '/assets/languages/' + scriptLanguage + '/id.json',
        }).done(function () {
            loadedScripts.push(scriptLanguage);
            execlanguage();
        });

    } else {

        execlanguage();
    }
}
function changelanguage(scriptLanguage) {
    $.ajax({
        url: WEB + '/general/language/' + scriptLanguage,
        type: "GET",
        datatype: "json",
        success: function (data) {
            if (data.success) {
                $("html").attr("lang", scriptLanguage);
                execlanguage();
            }
        }
    });
}
function execlanguage() {
    $.i18n().locale = $("html").attr("lang");
    $('.translate').each(function () {
        var args = [], $this = $(this);
        if ($this.data('args'))
            args = $this.data('args').split(',');
        $this.html($.i18n.apply(null, args));
    });
}
$(document).on('click', 'a.fakelink', function (e) {


    e.preventDefault();
    var xmen = $(this);


    var vlink = xmen.attr('href');

    var defdiv = xmen.attr('atarget');


    convault(defdiv, vlink);

    $('.m-menu__item').each(function () {
        $(this).removeClass("m-menu__item--active");
        $(this).removeClass("m-menu__item--expanded");

    });
    xmen.parent('li.m-menu__item').addClass("m-menu__item--active");
    xmen.parent('li.m-menu__item').parent('li.m-menu__item--submenu').addClass("m-menu__item--active");
});
function redirecttolink(vlink) {
    convault(null, vlink)
}
function todecimalz(value, dp) {
    return +parseFloat(value).toFixed(dp);
}
function convault(defdiv, vlink) {
    if (!defdiv) {
        defdiv = "page_content";
    }



    $('#' + defdiv).load(vlink, function (responseText, textStatus, req) {
        if (textStatus !== "error") {
            History.pushState(null, null, vlink)
        }

    });

}
function dynamicSort(property) {
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }
    return function (a, b) {
        var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
        return result * sortOrder;
    };
}
function dynamicSortMultiple() {
    /*
     * save the arguments object as it will be overwritten
     * note that arguments object is an array-like object
     * consisting of the names of the properties to sort by
     */
    var props = arguments;
    return function (obj1, obj2) {
        var i = 0, result = 0, numberOfProperties = props.length;
        /* try getting a different result from 0 (equal)
         * as long as we have extra properties to compare
         */
        while (result === 0 && i < numberOfProperties) {
            result = dynamicSort(props[i])(obj1, obj2);
            i++;
        }
        return result;
    };
}
var current_treenode = "";
var current_treename = "";
var current_approver_title_1 = "";
var current_approver_name_1 = "";
var current_approver_title_2 = "";
var current_approver_name_2 = "";
var current_approver_title_3 = "";
var current_approver_name_3 = "";
var current_mode = null;
function showtreebu(trnode, trname) {
    current_treenode = trnode;
    current_treename = trname;
    showModalAjaxGet("Treebu", WEB_GENERAL + '/treebu', {}, function () {
    });
}

function showtreebu_custom(trnode, trname, approver_title_1, approver_name_1, approver_title_2, approver_name_2, approver_title_3, approver_name_3, modex = null) {
    current_treenode = trnode;
    current_treename = trname;
    current_approver_title_1 = approver_title_1;
    current_approver_name_1 = approver_name_1;
    current_approver_title_2 = approver_title_2;
    current_approver_name_2 = approver_name_2;
    current_approver_title_3 = approver_title_3;
    current_approver_name_3 = approver_name_3;
    current_mode = modex;
    showModalAjaxGet("Treebu", WEB_GENERAL + '/treebu', {}, function () {
    });
}
function show404() {
    swal(
        'Information',
        t_notfound,
        'warning'
    )
}
function getnumber(x) {

    if (x) {

        if (x.length > 0) {
            x = x.replace(/\,/g, '');
            x = x.replace(/\./g, ',');

        }
        return parseFloat(x) || 0;

    }
    return 0;
}
function thousandsep(x) {
    if (x) {
        x = todecimalz(x);
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    }
    return 0;
}
//------------------------button style-----------------------------------------
var bstyle_add='';
var bstyle_add_icon = '';
var bstyle_edit = '';
var bstyle_edit_icon = '';
var bstyle_delete = '';
var bstyle_delete_icon = '';
//----------------------------------------------------------------------------
var t_delete = "Do you want to delete this data?";
var t_notfound = "Data not Found";
var t_deletesuccess = "Delete data is successfully !";
var t_createsuccess = "Create data is successfully !";
var t_updatesuccess = "Update data is successfully !";
var t_useasfile = "use this file ?";
var lang_gn = "GN";
var lang_fn = "FN";
var lang_pm = "PM";
var lang_hr = "HR";