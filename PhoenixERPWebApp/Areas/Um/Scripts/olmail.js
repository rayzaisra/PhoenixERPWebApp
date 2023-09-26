let sentEmail = function () {
    var formData = new FormData();
    var files = $(".in-attachment").get(0).files;
    var isLarge = false;
    var filename = '';
    $.each(files, function (i, file) {
        var sz = file.size / 1024 / 1024;
        sz = parseFloat(sz).toFixed(2);
        if (sz > 2.5) {
            isLarge = true;
            filename = file.name;
        }
        formData.append('Files', file);
    });

    if (isLarge) {
        swal(
            'Information',
            'Ooops, file attachment ' + filename + ', is to large',
            'info'
        );
        return false;
    }
    var toColls = $('.in-to');
    var toList = [];
    $.each(toColls, function (x, r) {
        var to = $(r).val();
        var vmTo = { To: to };
        toList.push(vmTo);
    });

    if (toList[0].To === "" || toList[0].To.length === 0) {
        swal(
            'Information',
            'Ooops, to email address cannot empty',
            'info'
        );
        return false;
    }
    if ($('.in-subject').val().length === 0) {
        swal(
            'Information',
            'Ooops, subject email cannot empty',
            'info'
        );
        return false;
    }

    formData.append("EmailAddressTos", JSON.stringify(toList));
    formData.append("Subject", $('.in-subject').val());
    formData.append("ContentEmail", $('#editor').val());

    let urlApi = $.helper.resolveApi("~olmail/SentMail");

    $.ajax({
        type: 'post',
        url: urlApi,
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            swal({
                title: "Sent Email",
                text: "Email message successfull to be send!",
                type: "success",
                showCancelButton: false,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (ok) {
                window.location.href = "/um/onedriveoffice/listemail";
            }).catch(swal.noop);

            console.log(data.message);
        },
        error: function (error, status, xhr) {
            console.log(error);
            alert(error.responseText);
        }
    });
};


let getInboxData = function () {
    let urlApi = $.helper.resolveApi("~olmail/getinbox");
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
            bindTableInbox(data.listInbox);
            bindTableSent(data.listSent);
            $('.badge-inbox').html(data.cu);
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

let bindTableInbox = function (data) {
    console.log(data);

    let tbl = $('.tbl-inbox>tbody');
    for (var i = 0; i < data.length; i++) {
        var row = "<tr data-messageid='" + data[i].id + "'>";
        if (data[i].isRead === false) {
            row = row + "<td><b>" + data[i].from.emailAddress.name + "</b></td>";
            row = row + "<td><b>" + data[i].subject + "</b></td>";
        } else {
            row = row + "<td>" + data[i].from.emailAddress.name + "</td>";
            row = row + "<td>" + data[i].subject + "</td>";
        }

        if (data[i].hasAttachments) {
            row = row + "<td class='view-message inbox-small-cells'><i class='fa fa-paperclip'></i></td>";
        }
        else
            row = row + "<td></td>";

        if (data[i].isRead === false) 
            row = row + "<td><b>" + data[i].receivedDateTime + "</b></td>";
        else
            row = row + "<td>" + data[i].receivedDateTime + "</td>";
        row = row + "</tr>";
        tbl.append(row);
    }

    $('.tbl-inbox>tbody>tr>').css('cursor', 'pointer');
    $('.tbl-inbox>tbody>tr').on('click', function (e) {
        readMessageInbox(e.currentTarget.dataset.messageid);
    });
};

let bindTableSent = function (data) {
    let tbl = $('.tbl-sent>tbody');
    for (var i = 0; i < data.length; i++) {
        var row = "<tr data-messageid='" + data[i].id+"'>";
        row = row + "<td>" + data[i].from.emailAddress.name + "</td>";
        row = row + "<td>" + data[i].subject + "</td>";
        if (data[i].hasAttachments) {
            row = row + "<td class='view-message inbox-small-cells'><i class='fa fa-paperclip'></i></td>";
        }
        else
            row = row + "<td></td>";
        row = row + "<td>" + data[i].sentDateTime + "</td>";
        row = row + "</tr>";
        tbl.append(row);
    }
    $('.tbl-sent>tbody>tr>').css('cursor', 'pointer');
    $('.tbl-sent>tbody>tr').on('click', function (e) {
        readMessageSent(e.currentTarget.dataset.messageid);
    });

};

let displayInbox = function () {
    $('.li-inbox').addClass('active');
    $('.li-sent').removeClass('active');
    $('.div-content-inbox').css('display','block');
    $('.div-content-sent').css('display', 'none');
    $('.div-content-message-email').css('display', 'none');
    $('.h-info-box').html('Inbox');
    $('.inbox-content').css('overflow', 'auto');
    $('.inbox-content').css('max-height', '650px');
    
};
let displaySent = function () {
    $('.li-inbox').removeClass('active');
    $('.li-sent').addClass('active');
    $('.div-content-inbox').css('display', 'none');
    $('.div-content-sent').css('display', 'block');
    $('.div-content-message-email').css('display', 'none');
    $('.h-info-box').html('Sent Email');
    $('.inbox-content').css('overflow', 'auto');
    $('.inbox-content').css('max-height', '650px');
};

let readMessageInbox = function (id) {
    let urlApi = $.helper.resolveApi("~olmail/GetMailInboxContent/" + id);
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
                openContentMessageEmail('inbox',data);
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

let readMessageSent = function (id) {
    let urlApi = $.helper.resolveApi("~olmail/GetMailSentContent/" + id);
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
            openContentMessageEmail('sent', data);
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

let openContentMessageEmail = function (tipe, data) {
    if (tipe === 'inbox') {
        $('.li-inbox').addClass('active');
        $('.li-sent').removeClass('active');
        $('.h-info-box').html('Read Email On Inbox');
        $('.date-message').html(data.receivedDateTime);

        $('.sp-from-to').html("<b>From : </b>");
        $('.ul-list-receipt').html("");
        var li = '<span class="sender-from">' + data.from.emailAddress.name + '(<i>' + data.from.emailAddress.address + '</i>); </span>';
        $('.ul-list-receipt').append(li);
    }
    else {
        $('.li-inbox').removeClass('active');
        $('.li-sent').addClass('active');
        $('.h-info-box').html('Read Email On Sent Folder');
        $('.date-message').html(data.sentDateTime);

        $('.sp-from-to').html("<b>To : </b>");
        $('.ul-list-receipt').html("");
        $.each(data.toRecipients, function (x, r) {
            var li = '<span class="sender-from">' + r.emailAddress.name + '(<i>' + r.emailAddress.address + '</i>); </span>';
            $('.ul-list-receipt').append(li);
        });

    }
    $('.div-content-inbox').css('display', 'none');
    $('.div-content-sent').css('display', 'none');
    $('.div-content-message-email').css('display', 'block');
    $('.inbox-content').css('overflow', 'hidden');
    $('.inbox-content').css('max-height', '100%');
        
    $('#div-body-email').html(data.body.content);
    $('.h-subject').html(data.subject);
    $(".open-weblink").attr("href", data.webLink);
    
};

let openFormComposeEmail = function () {
    window.location.href = "/um/onedriveoffice/composeemail";
};

function S4() { return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1); }
let addTo = function () {
    let codeunique = S4() + S4();
    var row = "<tr style='border: 0' class='" + codeunique+"'>";
    row = row + "<td style='border: 0'><input class='form-control form-control-sm in-to' type='email'></td>";
    row = row + "<td style='border: 0'><button class='btn btn-sm btn-danger' type='button' onclick='removeTo(\"" + codeunique + "\")'> <i class='la la-remove'></i></button></td>";
    row = row + "</tr>";
    $('.tbl-add-to>tbody').append(row);
};

let removeTo = function (codex) {
    $('.' + codex).remove();
};



let bindTemplateEmail = function () {
    let urlApi = $.helper.resolveApi("~emailtemplate/getlisttemplate");
    $.ajax({
        type: "GET",
        dataType: 'json',
        url: urlApi,
        success: function (data) {
                var ddl = $('#ddlTemplate');
                ddl.empty();
                $(ddl).append($('<option>', {
                    value: "",
                    text: "SELECT TEMPLATE"
                }));

                $.each(data.data, function (i, item) {
                    $(ddl).append($('<option>', {
                        value: item.id,
                        text: item.subject
                    }));
                });
        },
        error: function (e, t, s) {
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
            console.log(e);
        }
    });
};

let bindBodyEmail = function (id) {
    let urlApi = $.helper.resolveApi("~emailtemplate/gettemplatebyid/" + id);
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
                var $preview, editor, mobileToolbar, toolbar;
                Simditor.locale = 'en-US';
                toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', 'code', 'table', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment'];
                mobileToolbar = ["bold", "underline", "strikethrough", "color", "ul", "ol"];
                editor = new Simditor({
                    textarea: $('#editor'),
                    placeholder: 'write content email here',
                    toolbar: toolbar,
                    pasteImage: true,
                    upload: location.search === '?upload' ? {
                        url: '/upload'
                    } : false
            });
            var txt = $("<div>").html(data.data.body).text();
            editor.setValue(txt);
            $('#summernote').summernote('code', txt);
        },
        error: function (e, t, s) {
            //swal(
            //    'Information',
            //    'Ooops, something went wrong !',
            //    'info'
            //);
            console.log(e);
        }
    }).then(setTimeout(function () {mApp.unblockPage();}, 2e3));
};