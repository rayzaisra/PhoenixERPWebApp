//** Manage Connection Nava-Hub SignalR....created by M.Salih Rismadi (SoftFocusCore Indonesia)
//** Aug 29 2019...

const connection = new signalR.HubConnectionBuilder()
    .withUrl($.helper.resolveApi('~Hubs/Notification'))
    .build();

// tell the client, if we are ready connected to hub.....
//connection.start().then(function () {
//    console.log("nava-hub connected..");
//});

// tell to client, if hub is getting error....
connection.start().catch(err => console.error('Error Nava-Hub : ' + err.toString()));



connection.on("ReceiveMsgTeam", (teamId, toMember, fromMember, memberName, message,time) => {
    var data = sessionStorage.getItem('data');
    data = JSON.parse(data);
    $('#TbTextChatMsg').val('');
    if (data.AppUserId === toMember) {
        writeMsgChat(teamId, fromMember, memberName, message, time);
    }
});

connection.on("ReceiveNTFCenter", (toMember, templateNotif, tipe) => {
    
    var data = sessionStorage.getItem('data');
    data = JSON.parse(data);
    if (data.AppUserId === toMember) {
        let cn = $('#sp-bgnotif-count')[0].innerText;
        if (cn === '' || cn === null || cn === undefined) cn = 0;
        cn = parseInt(cn) + 1;
        $('#sp-bgnotif-count')[0].innerText = cn;
        $('#countnotification')[0].innerText = cn;

        if (tipe === 1) {
            $('#notification_list').append(templateNotif);
            // let counting deep just data notif
            let cnx = $('#sp-cbgn-count')[0].innerText;
            if (cnx === '' || cnx === null || cnx === undefined) cnx = 0;
            cnx = parseInt(cnx) + 1;
            $('#sp-cbgn-count')[0].innerText = cnx;
        }
        else {
            $('#approval_list').append(templateNotif);
            // let counting deep data approval
            let cax = $('#sp-cbga-count')[0].innerText;
            if (cax === '' || cax === null || cax === undefined) cax = 0;
            cax = parseInt(cax) + 1;
            $('#sp-cbga-count')[0].innerText = cax;
        }
    }
});

//** Reconnect clients
async function start() {
    try {
        await connection.start();
        console.log("connected");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

// client wanna close and prepare to connected again....
connection.onclose(async () => {
    await start();
});

let directToDetailNotif = function (codex,directLink) {
    let urlApi = $.helper.resolveApi("~notifycenter/UpdateIsSeen");
    var model = { Codex: codex };
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: urlApi,
        data: JSON.stringify(model),
        success: function () {
        },
        error: function (e, t, s) {
            console.log('update seend notification getting error');
            console.log(e);
            console.log(t);
            console.log(s);
            console.log('======= END OF update seend notification getting error =========');
        }
    });

    window.location.href = directLink;
};

let newTabToDetailNotif = function (codex, directLink) {
    let urlApi = $.helper.resolveApi("~notifycenter/UpdateIsSeen");
    var model = { Codex: codex };
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: urlApi,
        data: JSON.stringify(model),
        success: function () {
        },
        error: function (e, t, s) {
            console.log('update seend notification getting error');
            console.log(e);
            console.log(t);
            console.log(s);
            console.log('======= END OF update seend notification getting error =========');
        }
    });

    //window.location.href = directLink;
    window.open(directLink, '_blank');
};


let bindNotifyContent = function () {
    let urlApi = $.helper.resolveApi("~notifycenter/Get");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            let cax = 0;
            let cnx = 0;
            var ctx = 0;
            var NotifLabel = "";
            var strct = "";
            $("#notification_list").empty();
            $("#approval_list").empty();
            $.each(data, function (x, r) {
                if (r.tipe === 1) {
                    if (cnx < 5) {
                        //$('#notification_list').append(r.templateNotify);
                        strct = "";
                        if (r.isSeen === false) {
                            
                            if (r.templateNotify !== null) {
                                strct += '<div class="m-list-timeline__item " >' +
                                    '<span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>' +
                                    '<span class="m-list-timeline__text">' +
                                    r.templateNotify +
                                    '</span><span class="m-list-timeline__time">' +
                                    moment(r.createdOn).format('DD MMM YYYY') +
                                    '</span></div>';
                            }
                        }
                        $("#notification_list").append(strct);
                    }

                    if (r.isSeen === false) {
                        cnx++;
                    }
                    
                }
                else {
                    if (cax < 5) {
                        //$('#notification_list').append(r.templateNotify);
                        strct = "";
                        if (r.isSeen === false) {
                            
                            if (r.templateNotify !== null) {
                                strct += '<div class="m-list-timeline__item " >' +
                                    '<span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>' +
                                    '<span class="m-list-timeline__text">' +
                                    r.templateNotify +
                                    '</span><span class="m-list-timeline__time">' +
                                    moment(r.createdOn).format('DD MMM YYYY') +
                                    '</span></div>';
                            }

                        }
                        $("#approval_list").append(strct);
                    }

                    if (r.isSeen === false) {
                        cax++;
                    }
                }

                if (r.isSeen === false) {
                    ctx++;
                }
            });

            if (cnx < 5) {
                NotifLabel = "Click to expand";
            } else {
                NotifLabel = "Show More ...";
            }

            var expand = '<center><div>' +
                '<button type="button" onclick="ViewListNotification()" class="btn btn-warning">' + NotifLabel +'</button>' +
                '</div></center>';
            $('#notification_list').append(expand);

            if (cax < 5) {
                NotifLabel = "Click to expand";
            } else {
                NotifLabel = "Show More ...";
            }
            var expandAprv = '<center><div>' +
                '<button type="button" onclick="ViewListApproval()" class="btn btn-warning">' + NotifLabel + '</button>' +
                '</div></center>';
            $('#approval_list').append(expandAprv);
            
            $('#sp-bgnotif-count')[0].innerText = ctx;
            $('#countnotification')[0].innerText = ctx;

            $('#sp-cbga-count')[0].innerText = cax;
            $('#sp-cbgn-count')[0].innerText = cnx;
        },
        error: function (e, t, s) {
            console.log('build notification getting error');
            console.log(e);
            console.log(t);
            console.log(s);
            console.log('======= END OF build notification getting error =========');
        }
    });
};

let setImageProfile = function () {
    let urlApi = $.helper.resolveApi("~odrive/GetImageProfile");
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
            if (data.webUrl !== null) {
                $(".img-profile-nava").attr("src",data.webUrl);            
            }
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
        }, 2e3));
};

let setLogoutApp = function () {
    let urlApi = $.helper.resolveApi("~manageuser/setlogout");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            window.location.href = "/core/authentication/logout";
        },
        error: function (e, t, s) {
            console.log(e);
            console.log(t);
            console.log(s);
        }
    });
};

let chekedRawSession = function () {
    let urlApi = $.helper.resolveApi("~manageuser/CheckRawSession");

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            console.log('==== Raw Session===');
            console.log(data);
            console.log('==== End Off Raw Session===');
            if (data.isSame === false) {
                alert(data.msg);
                window.location.href = "/core/authentication/logout";
            }
        },
        error: function (e, t, s) {
            console.log(e);
            console.log(t);
            console.log(s);
        }
    });
};

let setPriviledgeMenu = function () {
    var pun = $('#hidPageUniqueName').val();
    if (pun === '' || pun === undefined || pun === null) {
        sessionStorage.removeItem('uniquePage');
    }
    else {
        let urlApi = $.helper.resolveApi("~ManageUserAccess/GetMenuAccessByUniqueName/" + pun);
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: urlApi,
            success: function (data) {
                sessionStorage.setItem('uniquePage', JSON.stringify(data));
            },
            error: function (e, t, s) {
                console.error('Error Getting INformation Page Unique Name FOr User Access');
                console.log(e);
                console.log(t);
                console.log(s);
            }
        });
    }
};

var columnsIndexNotification = [
    {
        "data": "templateNotify",
        "title": "Notification",
        "sClass": "lcol",
        orderable: true
    },
    {
        "data": "createdOn",
        "title": "Created On",
        "sClass": "lcol",
        orderable: true,
        "render": function (data, type, row, meta) {
            var html = '<span style="display:none;">' + row.createdOnString + '</span>' + moment(data).format('DD MMM YYYY hh:mm');
            return html;
            //return moment(data).format('DD MMM YYYY hh:mm');
        }
    },
    //{
    //    "data": "createdOnString",
    //    "title": "Created On",
    //    "sClass": "lcol",
    //    orderable: true
    //},
    {
        "data": "module",
        "title": "Module",
        "sClass": "lcol",
        orderable: true
    },
    {
        "data": "createdBy",
        "title": "Created By",
        "sClass": "lcol",
        orderable: true
    }
];

function generatelist() {
    //$("#tableListNotification tbody").empty();
    //$("#tableListNotification tbody").empty();
    astr = '';
    cstr = '';

    //$.each(data, function (key, row) {
    //    vstr = '<tr>';

    //    vstr += '<td>' + row.templateNotify + '</td>';
    //    vstr += '<td>' + moment(row.createdOn).format('DD MMM YYY hh:mm') + '</td>';
    //    vstr += '<td>' + row.module + '</td>';
    //    vstr += '<td>' + row.createdBy + '</td>';
    //    vstr += '</tr>';
    //    astr += vstr;
    //});
    //$("#tableListNotification tbody").append(astr);
    //$("#tableListNotification tbody").append(cstr);
    //dataSet = JSON.stringify(dataSet);
    var ListData = [];
    if (!$.fn.DataTable.isDataTable('#tableListNotification')) {
        $('#tableListNotification').DataTable({
            "ajax": function (data, callback, settings) {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~notifycenter/Get"),
                    success: function (data) {
                        $.each(data, function (x, r) {
                            if (r.tipe === 1) {
                                var obj = {};
                                obj["templateNotify"] = r.templateNotify;
                                obj["createdOn"] = r.createdOn;
                                obj["createdOnString"] = r.createdOnString;
                                obj["module"] = r.module;
                                obj["createdBy"] = r.createdBy;
                                ListData.push(obj);
                            }
                           
                        });
     
                        callback({
                            data: ListData,
                            recordsTotal: data.length,
                            recordsFiltered: 0
                        });
                    }
                });
            },
            "columns": columnsIndexNotification,
            "order": [1,"desc"],
        });
    }
    

}

function generatelistApproval() {
    //$("#tableListNotification tbody").empty();
    //$("#tableListNotification tbody").empty();
    astr = '';
    cstr = '';

    //$.each(data, function (key, row) {
    //    vstr = '<tr>';

    //    vstr += '<td>' + row.templateNotify + '</td>';
    //    vstr += '<td>' + moment(row.createdOn).format('DD MMM YYY hh:mm') + '</td>';
    //    vstr += '<td>' + row.module + '</td>';
    //    vstr += '<td>' + row.createdBy + '</td>';
    //    vstr += '</tr>';
    //    astr += vstr;
    //});
    //$("#tableListNotification tbody").append(astr);
    //$("#tableListNotification tbody").append(cstr);
    //dataSet = JSON.stringify(dataSet);
    var ListData = [];
    if (!$.fn.DataTable.isDataTable('#tableListApproval')) {
        $('#tableListApproval').DataTable({
            "ajax": function (data, callback, settings) {
                $.ajax({
                    type: "GET",
                    dataType: 'json',
                    url: $.helper.resolveApi("~notifycenter/Get"),
                    success: function (data) {
                        $.each(data, function (x, r) {
                            if (r.tipe === 2) {
                                var obj = {};
                                obj["templateNotify"] = r.templateNotify;
                                obj["createdOn"] = r.createdOn;
                                obj["createdOnString"] = r.createdOnString;
                                obj["module"] = r.module;
                                obj["createdBy"] = r.createdBy;
                                ListData.push(obj);
                            }

                        });

                        callback({
                            data: ListData,
                            recordsTotal: data.length,
                            recordsFiltered: 0
                        });
                    }
                });
            },
            "columns": columnsIndexNotification,
            "order": [1, "desc"],
        });
    }
    

}

function ViewListNotification(data) {
    $("#ListNotificationModal").modal('show');
    generatelist();
}

function ViewListApproval(data) {
    $("#ListApprovalModal").modal('show');
    generatelistApproval();
}

$(function () {
    var urlName = window.location.pathname.toLowerCase();
    if (urlName.includes('core')) {
        $('#m_ver_menu').hide();
    } else {
        $('#m_ver_menu').show();
    }
    bindNotifyContent();
    chekedRawSession();
    setImageProfile();
    setPriviledgeMenu();
});
//<a href="#" title="click here to see detail"><div> <p>Assign as PM</p></div></a>