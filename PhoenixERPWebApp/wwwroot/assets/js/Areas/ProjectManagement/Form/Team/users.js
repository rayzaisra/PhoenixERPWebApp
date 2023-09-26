var webUrl = WEB + "/ProjectManagement/team";
var ajaxUrl = API + "/General/team";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var xform = "TeamusersFormTeam";
var ajaxUrlGet = ajaxUrl + "/GetUserList";
var ajaxUrlListAccountManagement = API_PM + "/AccountManagement/list";
var ajaxUrlListActor = API_PM + "/AccountManagement/listactor";
var ajaxUrlPost = ajaxUrl + "/UpdateTeam";
var tableList = "TeamusersList";
var vselactor = "";
var tempteam = [];

fnTeamusers = {
    delete: function (user_id) {
        tempteam = $.grep(tempteam, function (e) {
            return e.user_id !== user_id;
        });
        fnTeamusers.generatelist(tempteam)
    },
    pickuser: function (xthis,user_id, username, email) {

        var foundteam = tempteam.find(x => x.user_id === user_id);
        if (!foundteam) {
            tempteam.push({ 'user_id': user_id, 'username': username, 'email': email });
            fnTeamusers.generatelist(tempteam);
        } else {
            swal(
                'Information',
                'User Already Added',
                'warning'
            );
        }
        $(xthis).hide();
    },
    getdata: function (xid) {
        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response = parseJson(response);
            tempteam = [];
            if (response.data) {
                if (response.data.userlist) {
                    tempteam = response.data.userlist;
                    fnTeamusers.generatelist(tempteam);
                }
            }

            setFormAction(xform, ajaxUrlUpdate);

           

            FormLoadByDataUsingID(response.data, xform);


        });
    },
    adduser: function () {
        $("#modaluserlist").modal('show');

    },
    generatelist: function (data) {
        $("#TeamusersList tbody").empty();
        vstr = '';
        $.each(data, function (key, row) {
            var cname = replaceNonAlphaNum(row.user_id);
            vstr += '<tr>';
            vstr += '<td><a class="m-btn btn btn-sm btn-danger" onClick="fnTeamusers.delete(\'' + row.user_id + '\')" href="javascript:void(0)"><i class="la la-trash"></i></a></td>';
            vstr += '<td>' + row.username + '</td>';
            vstr += '<td>' + row.email + '</td>';
            retval = replaceAll(vselactor, "item_id", 'act_as_' + cname);
            retval = replaceAll(retval, 'value="' + row.act_as + '"', 'value="' + row.act_as + '" selected ');
            vstr += '<td>' + retval + '</td>';
            vstr += '</tr>';
        });
        $("#TeamusersList tbody").append(vstr);
    },
    savedata: function () {
        var valid = true;
        var data = tempteam;
        var dataform = [];
        $.each(tempteam, function (key, item) {
            var cname = replaceNonAlphaNum(item.user_id);
      
                var actx = $("#act_as_" + cname).val();
                if (actx !== '') {
                    dataform.push({
                        "user_id": item.user_id,
                        "act_as": $("#act_as_" + cname).val()
                    });
                } else {
                    valid = false;
                }
 
           
        });
        if (valid) {
            var xid = $("#" + xform + ' [name="id"]').val();
            if (xid) {
                datapost = {
                    'team_id': xid,
                    'team': dataform
                };
 
                ajaxPost(ajaxUrlPost, datapost, function (response) {
                    var respact = parseJson(response);
                    SuccessNotif("Data Updated");
                    convault(null, WEB + "/projectmanagement/team");
                });
            }
        } else {
            DangerNotif("Please Complete the Form");
        }

    }
}
function replaceNonAlphaNum(text) {
    return text.replace(/[\W_]+/g, "_");
}
$(function () {
    var xid = $("#" + xform + ' [name="id"]').val();
    if (xid) {
        var optionsForm = GetOptionsForm(function () {

            var xret = $("#" + xform).valid();
            if (xret) startprocess();
            return xret;
        }, function (response, statusText, xhr, $form) {
            endprocess();
            response = parseJson(response);
            if (response.success) {
                SuccessNotif(t_updatesuccess);
                redirecttolink(webUrl);
            } else {
                DangerNotif(response.message);
            }
        });
        InitForm(xform, optionsForm);
        setFormAction(xform, ajaxUrlUpdate);
        initlanguage(lang_gn);
        ajaxGet(ajaxUrlListActor, {}, function (response) {
            var respact = parseJson(response);
            vselactor = '<select id="item_id"><option value="">-</value>';
            $.each(respact.rows, function (key, item) {
                vselactor += '<option value="' + item.id + '">' + item.name + '</value>';
            });
            vselactor += '</select>';
        });
        fnTeamusers.getdata(xid);
        fnUserList.generatelist();
    }





});

