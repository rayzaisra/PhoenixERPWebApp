var ajaxUrlGetNofification = API + "General/Notification/List";

fnNotifikasi = {
    getdata: function (limit) {
        if (limit===null) {
            limit = 10;
        }
        ajaxGet(ajaxUrlGetNofification, { rows: limit}, function (response) {
            response = parseJson(response);
            $("#notification_list").empty();
            $("#countnotificationbadge").empty();
            $("#countnotificationbadge").append('<span class="m-nav__link-icon-wrapper"><i class="flaticon-alarm"></i></span>');
            $("#countnotificationbadge").append('<span class="m-nav__link-badge m-badge m-badge--accent" id="sp-bgnotif-count"></span>');
            $("#countnotification").empty();
            var strct = "";
            var xno = 0;
            if (response.recordsFiltered !== 0) {
                $("#countnotificationbadge").append('<span class="m-nav__link-badge m-badge m-badge--danger">' + response.recordsFiltered + '</span>');
                $('#countnotification').html(response.recordsFiltered);
            }
            $.each(response.rows, function (key, item) {
             
                xno++;
                xcheck = '';
                if (item.is_seen) {
                    xcheck = 'm-list-timeline__item--read';
                }
                
                strct += '<div class="m-list-timeline__item ' + xcheck + '" >' +
                 '<span class="m-list-timeline__badge -m-list-timeline__badge--state-success"></span>' +
                    '<span class="m-list-timeline__text">' +
                    '<a href="'+WEB+ item.href+'"> ' +  item.shorttext + '</a>' +
                    '</span><span class="m-list-timeline__time">' +
                    item.date_on +
                    '</span></div>';
            });
           

            $("#notification_list").append(strct);
        });
    }
 
};
 

$(function () {
    fnNotifikasi.getdata(10);
    console.log("masuk notifikasi");
    var xtoken = $('meta[name=JWT_TOKEN]').attr("content");
    var connection = new signalR.HubConnectionBuilder()
        .withUrl(Socket_URL+"Hubs/Notification",
            { accessTokenFactory: () => xtoken })
        .configureLogging({
        })
        .build();

    connection.start().then(() => {
        connection.invoke("JoinRoom", "ntfks_"+Current_userid).catch(function (err) {
            return console.log(err.toString());
        });
    }).catch(function (err) {

        return console.log(err.toString());
    });

    connection.on("ReceiveMessage", function (message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = " says " + msg;

        fnNotifikasi.getdata(10);
    });
    
});
