var ajaxUrl = API + "ProjectManagement/Job";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";

var tableList = "JobList_mydam";
var formCreate = "JobFormCreate";
var formUpdate = "JobFormUpdate";
var formSearch = "JobFormSearch";
var modalCreate = "JobModalCreate";
var modalUpdate = "JobModalUpdate";
var modalSearch = "JobModalSearch";
var pageheader = "";
var paramsSearchIndex = function () {
    var search_id = $("#search_id_mydam").val();
    var search_val = $("#search_val_mydam").val();
    var data = {};
    var user = JSON.parse(sessionStorage.getItem("data"));
    data = { 'user_id': user.AppUserId };
    if (search_val) {
        data[search_id] = search_val;
    }


    return data;
};

var columnsIndex = [

    {
        "data": "job_number",
        "title": "Job Number",
        "sClass": "rcol",
        orderable: true,
        render: function (data, type, row, meta) {
            retval = '';
            retval += '<a class="dropdown-item fakelink" href="/projectmanagement/MyDam/detail/' + row.id + '">' + data + '</a>';
            return retval;
        }
    },
    {
        "data": "brand_name",
        "title": "Brand",
        "sClass": "lcol",
        orderable: true,
    },
    {
        "data": "company_name",
        "title": "company_name",
        "sClass": "lcol",
        orderable: true,
    },
    {
        "data": "campaign_name",
        "title": "Campaign Name",
        "sClass": "rcol",
        orderable: true
    },
    {
        "data": "job_name",
        "title": "Job Name",
        "sClass": "rcol",
        orderable: true
    },
    {
        "data": "business_unit_name",
        "title": "Business Unit",
        "sClass": "rcol",
        orderable: false
    },
    {
        "data": "deadline",
        "title": "Deadline",
        "sClass": "rcol",
        orderable: false,
        "render": function (data) {
            var date = new Date(data);
            var month = date.getMonth() + 1;
            return date.getDate() + "/" + month + "/" + date.getFullYear();
        }
    },
    {
        "data": "final_delivery",
        "title": "Final Delivery",
        "sClass": "rcol",
        orderable: false,
        "render": function (data) {
            var date = new Date(data);
            var month = date.getMonth() + 1;
            return date.getDate() + "/" + month + "/" + date.getFullYear();
        }
    },
    {
        "data": "status_name",
        "title": "Status",
        "sClass": "rcol",
        orderable: true
    }
];

fnJob = {
    generatelist: function () {
        InitTable(tableList, ajaxUrlList, paramsSearchIndex, columnsIndex, 'created_on', 'desc');
        setcolvis(tableList, columnsIndex);
    },
    reloadlist: function () {
        TABLES[tableList].ajax.reload(null, false);
    },
    search: function () {
        UIkit.modal("#" + modalSearch).show();
    },
    initsearch: function () {
        TABLES[tableList].ajax.reload();
    }
};

$(function () {
    fnJob.generatelist();
    initlanguage(lang_pm);
});

