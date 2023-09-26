//#region
// call this function when clicked from header notification
//#endregion
let openTemplateChat = function () {
    var chatSetup = sessionStorage.getItem('cset-x');
    if (chatSetup === 'NAN') {
        sessionStorage.setItem('cset-x', 'flx');
        $('#chat-snippet').show(500);
        $('#chat-snippet').css('display', 'flex');
        console.log('Li Team Ada : '+ $('#chat-snippet').find('.ul-li-team').length);
        if ($('#chat-snippet').find('.ul-li-team').length === 0) getListOfMember();
    }
    else {
        sessionStorage.setItem('cset-x', 'NAN');
        $('#chat-snippet').hide(500);
        $('#chat-snippet').css('display', 'none');
    }
};

//#region
// Function for get team of member current..
//#endregion

let getListOfMember = function () {
    let urlApi = $.helper.resolveApi("~chat/GetListContact/");

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            console.log(data);
            bindListTeam(data.team);
            bindListGroupChat(data.groupChat);
        },
        error: function (e, t, s) {
            console.log(e);
            console.log(t);
            console.log(s);
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
        }
    });
};

let bindContact = function () {
    var li =  '<li>'+
        '<a href="#">'+
        '    <div class="d-flex bd-highlight">' +
        '        <div class="img_cont">' +
        '            <img src="https://cdn3.iconfinder.com/data/icons/digital-marketing-line-art/512/09-48.png" class="rounded-circle user_img">' +
        '        </div>' +
        '        <div class="user_info">' +
        '            <span>Ryan Reki Andrian</span>' +
        '            <p>mehr.khadija@nava-plus.com</p>' +
        '        </div>' +
        '    </div>' +
        '<a/>' +
        '</li>';
    var li2 = '<li>' +
        '<a href="#">' +
        '    <div class="d-flex bd-highlight">' +
        '        <div class="img_cont">' +
        '            <img src="https://cdn3.iconfinder.com/data/icons/digital-marketing-line-art/512/09-48.png" class="rounded-circle user_img">' +
        '        </div>' +
        '        <div class="user_info">' +
        '            <span style="color:#2fd291">M.Ahmad Satibi</span>' +
        '            <p>mehr.khadija@nava-plus.com</p>' +
        '        </div>' +
        '    </div>' +
        '   <a/>' +
        '</li>';
    var li3 = '<li>' +
        '<a href="#">' +
        '    <div class="d-flex bd-highlight">' +
        '        <div class="img_cont">' +
        '            <img src="https://cdn3.iconfinder.com/data/icons/digital-marketing-line-art/512/09-48.png" class="rounded-circle user_img">' +
        '        </div>' +
        '        <div class="user_info">' +
        '            <span style="color:#333">M.Ahmad Satibi</span>' +
        '            <p>mehr.khadija@nava-plus.com</p>' +
        '        </div>' +
        '    </div>' +
        '   <a/>' +
        '</li>';
    for (var i = 0; i < 10; i++) {
        if (i === 0 || i === 3 || i === 4)
            $('.card-body>.contact-person').append(li3);
        else if (i === 5 || i === 7 || i === 8)
            $('.card-body>.contact-person').append(li2);
        else
            $('.card-body>.contact-person').append(li);
    }

};

let bindListTeam = function (data) {
    $('.member-of-team').empty();
    var teamTipe = 'T';
    $.each(data, function (x, r) {
        var li = '<li class="' + r.id + ' ul-li-team" style="line-height: 1em;padding:5px;margin:0">' +
            '   <a href="javascript:openChatTeam(\'' + r.id + '\', \'' + r.teamName + '\',\'' + teamTipe + '\')">' +
            '      <div class="d-flex bd-highlight">' +
            '            <div class="img_cont">' +
            '                <img src="../../assets/img/chat_img/team-2.jpg" class="rounded-circle user_img">' +
            '            </div>' +
            '            <div class="user_info user-info-chat">' +
            '                <span style="font-size:11px">' + r.teamName +'</span>' +
            '                <p>'+r.jobName+'</p>' +
            '            </div>' +
            '        </div>' +
            '    </a>' +
            '</li>';
        $('.member-of-team').append(li);
    });
};
let bindListGroupChat = function (data) {

    var teamTipe = 'G';
    $.each(data, function (x, r) {
        var li = '<li class="' + r.id + ' ul-li-team">' +
            '   <a href="javascript:openChatTeam(\'' + r.id + '\', \'' + r.groupName + '\',\'' + teamTipe + '\')">' +
            '      <div class="d-flex bd-highlight">' +
            '            <div class="img_cont">' +
            '                <img src="../assets/img/chat_img/team-3.png" class="rounded-circle user_img">' +
            '            </div>' +
            '            <div class="user_info user-info-chat">' +
            '                <span style="font-size:11px">' + r.groupName + '</span>' +
            '                <p>Group Chatting</p>' +
            '            </div>' +
            '        </div>' +
            '    </a>' +
            '</li>';
        $('.member-of-team').append(li);
    });
};

let openChatTeam = function (teamId,teamName,tipe) {
    $('.ul-li-team').removeClass('active');
    $('.' + teamId).addClass('active');
    $('.chat-with-team').text('Chat with ' + teamName);
    $('#HidRef').val(teamId);
    $('#HidChatType').val(tipe);
    $('.msg-chat-body').empty();
    $('.badge-notif-chat-team-' + teamId).remove();

    let colBadge = $('.member-of-team').find('.badge-notif-chat-team');
    if (colBadge.length === 0)
        $('#m_quick_sidebar_toggle').find('#sp-badge-notif-chat')[0].innerHTML = '';

    //bind ajax...
    let urlApi = $.helper.resolveApi("~chat/GetListChatTeam/" + teamId);

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
            //..bind data..//
            bindBodyMsg(data);
        },
        error: function (e, t, s) {
            console.log(e);
            console.log(t);
            console.log(s);
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
        }
    });
};

let bindBodyMsg = function (data) {
    var ds = sessionStorage.getItem('data');
    ds = JSON.parse(ds);
    $.each(data, function (x, r) {
        if (ds.AppUserId === r.memberCode) {
            var contentSelf = '<div class="d-flex justify-content-end mb-4">' +
                '   <div class="msg_cotainer_send">' + r.chatText +
                '       <span class="msg_time_send">' + moment(r.createdOn).format('hh:mm') + ' , ' + moment(r.createdOn).fromNow() + '</span>' +
                '   </div>' +
                '   <div class="img_cont_msg">' +
                '       <img src="../assets/img/chat_img/self-talk.png" class="rounded-circle user_img_msg">' +
                '   </div>' +
                '</div>';
            $('.msg-chat-body').append(contentSelf);
        }
        else {
            var divContent = '<div class="d-flex justify-content-start mb-4">' +
                '        <div class="msg_cotainer">' +
                '            <b>' + r.memberName + '</b> <i>(' + r.memberCode + ')</i><br />' + r.chatText  +
                '           <span class="msg_time">' + moment(r.createdOn).format('hh:mm') + ' , ' + moment(r.createdOn).fromNow() + '</span>' +
                '       </div>' +
                '   </div>';
            $('.msg-chat-body').append(divContent);
        }
    });
};


let actionSendTextChatting = function () {
    var txtMsg = $('#TbTextChatMsg').val();
    $('#TbTextChatMsg').val('');
    if (txtMsg === '' || txtMsg.length === 0) {
        swal(
            'Information',
            'Ooops, chat message cannot empty!',
            'info'
        );
        return false;
    }
    if ($('#HidRef').val() === '0') {
        swal(
            'Information',
            'Ooops, please select team before sent chat!',
            'info'
        );
        return false;
    }

    var now = new Date();
    var hh = now.getHours() + ":" + now.getMinutes();
    var divContent = '';
    if ($('#HidChatType').val() === 'T' || $('#HidChatType').val() === 'G') {
        divContent = '<div class="d-flex justify-content-end mb-4">' +
            '   <div class="msg_cotainer_send">' + txtMsg +
            '       <span class="msg_time_send">'+hh+' Today</span>' +
            '   </div>' +
            '   <div class="img_cont_msg">' +
            '       <img src="../assets/img/chat_img/self-talk.png" class="rounded-circle user_img_msg">' +
            '   </div>' +
            '</div>';
        $('.msg-chat-body').append(divContent);
        sentChatTeam(txtMsg, $('#HidChatType').val());
    }
};

let sentChatTeam = function (txtMsg,tipe) {
    var ds = sessionStorage.getItem('data');
    ds = JSON.parse(ds);
    var model = {
        ChatText : txtMsg,
        FileExt : '',
        CollaborateCode: $('#HidRef').val(),
        MemberName: ds.FullName,
        Type: tipe
    };

    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~/chat/PostChatTeam"),
        data: JSON.stringify(model),
        success: function (r) {
            console.log('success add chat');
            $('#TbTextChatMsg').val('');
        },
        error: function (r) {
            console.log('masuk error');
            console.log(r);
            swal(
                '' + r.status,
                r.statusText,
                'error'
            );
        }
    });
};


let writeMsgChat = function (teamId, fromMember, memberName, message, time) {
    var cs = sessionStorage.getItem('cset-x');
    //var countChat = $('#m_quick_sidebar_toggle').find('#sp-badge-notif-chat')[0].innerHTML;
    //var cc = parseInt(countChat) + 1;
    $('#m_quick_sidebar_toggle').find('#sp-badge-notif-chat')[0].innerHTML = 'new';

    if (cs === 'NAN') {
        // cek span count team, ada or not...
        var $spanCount = $('.' + teamId).find('.badge-notif-chat-team-' + teamId);
        var spBadge = '<span class="m-nav__link-badge m-badge m-badge--danger badge-notif-chat-team-' + teamId + '" style="padding:2px;font-size:10px">new</span>';
        if ($spanCount.length === 0) // span blm ada
        {
            $('.' + teamId).find('.user-info')[0].prepand(spBadge);
        }
        else {
            //var sc = $('.' + teamId).find('.badge-notif-chat-team')[0].innerHTML;
            //sc = parseInt(sc) + 1;
            $('.' + teamId).find('.badge-notif-chat-team-' + teamId)[0].innerHTML = 'new';
        }
    }
    else {
        //cek apakah li team nya aktif or not...
        var isactive = $('.' + teamId).hasClass('active');
        if (isactive) { // jika li team nya aktiv, langsung cetak pesan nya...
            var liTeam = $('.member-of-team').find('.' + teamId);
            if (liTeam.length > 0) {
                var divContent = '<div class="d-flex justify-content-start mb-4">' +
                    '        <div class="msg_cotainer">' +
                    '            <b>' + memberName + '</b> <i>(' + fromMember + ')</i><br />' + message +
                    '           <span class="msg_time">' + time + '</span>' +
                    '       </div>' +
                    '   </div>';
                $('.msg-chat-body').append(divContent);
            }
        }
        else {// jika tidak aktiv li team nya, di count li team nya...
            var $spanCountx = $('.' + teamId).find('.badge-notif-chat-team-' + teamId);
            if ($spanCountx.length === 0) // span blm ada
            {
                var spanLiTeam = '<span class="m-nav__link-badge m-badge m-badge--danger badge-notif-chat-team-' + teamId + '" style="padding:2px;font-size:10px">new</span>';
                var $div = $('.' + teamId).find('.user-info-chat')[0];
                $($div).prepend(spanLiTeam);
            }
            else {
                //var scx = $('.' + teamId).find('.badge-notif-chat-team')[0].innerHTML;
                //scx = parseInt(sc) + 1;
                $('.' + teamId).find('.badge-notif-chat-team-' + teamId)[0].innerHTML = 'new';
            }
        }
    }

    //prepend Li to be TOP....

    var liContent = $('.' + teamId)[0].outerHTML;
    $('.' + teamId).remove();
    $('.member-of-team').prepend(liContent);
    //console.log(liContent);
};  

$(function () {
    var chatSetup = sessionStorage.getItem('cset-x');
    if (chatSetup === undefined || chatSetup === null) {
        sessionStorage.setItem('cset-x', 'NAN');
        //$('#chat-snippet').css('display', 'none');
    }
    //else {
    //    if (chatSetup === 'NAN')
    //        $('#chat-snippet').css('display', 'none');
    //    else $('#chat-snippet').css('display', 'flex');
    //}

    $('#action_menu_btn').click(function () {
        $('.action_menu').toggle();
    });
    $('.refresh-contact-chat').on('click', function () { getListOfMember(); });
    $('#bntSentChatText ').on('click', function () { actionSendTextChatting(); });
       
});