var ajaxUrlTeamList = API + "General/Team/GetAllUser";
var tableTeamList = "UserList";
var formSearch = "teamusersearch";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = {};
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
};


fnUserList = {
    generatelist: function () {

        var columnsIndex = [
            {
                "data": null,
                "title": "#",
                "sClass": "checktd",
                orderable: false,
                "render": function (data, type, row, meta) {
                    var foundteam = tempteam.find(x => x.user_id === row.id);
                    if (!foundteam) {
                        retval = '<button type="button" onclick="fnTeamusers.pickuser(this,\'' + row.id + '\',\'' + row.aliasName + '\',\'' + row.email + '\')" class="btn btn-default">Pick</button>';
                    } else {
                        retval = '';
                    }
                    
 
                    return retval;
                }
            }, {
                "data": "id",
                "title": "user name",
                "sClass": "",
                orderable: true
            }, {
                "data": "aliasName",
                "title": "full name",
                "sClass": "",
                orderable: true
            }, {
                "data": "email",
                "title": "Email",
                "sClass": "",
                orderable: true
            }
        ];
        InitTable(tableTeamList, ajaxUrlTeamList, paramsSearchIndex, columnsIndex, 'id', 'asc');
  
    },
    reloadlist: function () {
        TABLES[tableTeamList].ajax.reload(null, false);
    },
    search: function () {
        $("#" + modalSearch).show();
    },
    initsearch: function (x) {
        if (x === "reset") {
            $("#" + formSearch)[0].reset();
        }
        TABLES[tableTeamList].ajax.reload();
 
    } 
};

 

