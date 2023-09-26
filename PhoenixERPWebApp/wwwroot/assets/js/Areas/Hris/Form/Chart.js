var ajaxUrl = API + "General/BusinessUnit";
var ajaxUrlList = ajaxUrl + "/ListWithParent";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxBussinesUnitJobLevel = API + "hris/BussinesUnitJobLevel/ListWithParentDetail";
var ajaxUrlListChart = API + "hris/BussinesUnitJobLevel/Chart";

var tableList = "SubgroupList";
var formSearch = "SubgroupFormSearch";
var modalSearch = "SubgroupModalSearch";
var pageheader = "";


$(function () {
    initlanguage(lang_hr);
    var id = null;
    var datadef = "token";
 
    $('#group').setcombobox({
        data: { 'rows': 100, Search_business_unit_level:1000},
        url: ajaxUrlList,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#subgroup').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlList,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#division').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlList,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });
    $('#departement').setcombobox({
        data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
        url: ajaxUrlList,
        maxItems: 0,
        searchparam: 'unit_name',
        labelField: 'unit_name',
        valueField: 'id'
    });

    $('#group').on('select2:select', function (e) {
        id = e.params.data.id;
        datadef = 900;
        $('#subgroup').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: datadef, Search_parent_unit: id },
            url: ajaxUrlList,
            maxItems: 0,
            searchparam: 'unit_name',
            labelField: 'unit_name',
            valueField: 'id'
        });

       

       
    });
   

    $('#subgroup').on('select2:select', function (e) {
        id = e.params.data.id;

      
        $('#division').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: 800, Search_parent_unit: id },
            url: ajaxUrlList,
            maxItems: 0,
            searchparam: 'unit_name',
            labelField: 'unit_name',
            valueField: 'id'
        });

        ajaxGet(ajaxUrlListChart, { Rows:1000, Search_business_unit_group_id: $('#group').val(), Search_business_unit_subgroup_id: id }, function (source) {
            console.log(parseJson(source.rows));
            var databaru = [];

            $.each(parseJson(source.rows), function (index, value) {
                var namelogic = value.name;
                if (namelogic === null || namelogic === "") {
                    namelogic = "Vacant";
                }
                var img = value.image;
                if (img === null || img === "") {
                    img = "../assets/img/avatars/no-image-icon-11.PNG";
                }

                databaru.push({
                    "Id": value.id,
                    "parentId": value.parentId,
                    "Title": value.title,
                    "Name": namelogic,
                    "Image": img,
                    "Grade": value.grade
                });
            });
            console.log(databaru);
            var peopleElement = document.getElementById("ChartGrafik");
            var orgChart = new getOrgChart(peopleElement, {
                //theme: "myTheme",
                siblingSeparation: 80,
                subtreeSeparation: 200,
                theme: "ada",
                boxSizeInPercentage: {
                    minBoxSize: {
                        width: 5,
                        height: 5
                    },
                    boxSize: {
                        width: 20,
                        height: 20
                    },
                    maxBoxSize: {
                        width: 150,
                        height: 100
                    }
                },
                primaryFields: ["Title", "Name", "Grade"],
                photoFields: ["Image"],
                linkType: "M",
                enableEdit: false,
                enableDetailsView: false,
                dataSource: databaru
            });

        });

    });

    $('#division').on('select2:select', function (e) {
        id = e.params.data.id;
        $('#departement').setcombobox({
            data: { 'rows': 100, Search_business_unit_level: 700, Search_parent_unit: id },
            url: ajaxUrlList,
            maxItems: 0,
            searchparam: 'unit_name',
            labelField: 'unit_name',
            valueField: 'id'
        });

        ajaxGet(ajaxUrlListChart, { Rows: 1000, Search_business_unit_group_id: $('#group').val(), Search_business_unit_subgroup_id: $('#subgroup').val(), Search_business_unit_division_id: id }, function (source) {
            //console.log(parseJson(source.rows));
            var databaru = [];

            $.each(parseJson(source.rows), function (index, value) {
                var namelogic = value.name;
                if (namelogic === null || namelogic === "") {
                    namelogic = "Vacant";
                }
                var img = value.image;
                if (img === null || img === "") {
                    img = "../assets/img/avatars/no-image-icon-11.PNG";
                }

                databaru.push({
                    "Id": value.id,
                    "parentId": value.parentId,
                    "Title": value.title,
                    "Name": namelogic,
                    "Image": img,
                    "Grade": value.grade
                });
            });
            //console.log(databaru);
            var peopleElement = document.getElementById("ChartGrafik");
            var orgChart = new getOrgChart(peopleElement, {
                //theme: "myTheme",
                siblingSeparation: 80,
                subtreeSeparation: 200,
                theme: "ada",
                boxSizeInPercentage: {
                    minBoxSize: {
                        width: 5,
                        height: 5
                    },
                    boxSize: {
                        width: 20,
                        height: 20
                    },
                    maxBoxSize: {
                        width: 150,
                        height: 100
                    }
                },
                primaryFields: ["Title", "Name", "Grade"],
                photoFields: ["Image"],
                linkType: "M",
                enableEdit: false,
                enableDetailsView: false,
                dataSource: databaru
            });

        });
    });

    $('#departement').on('select2:select', function (e) {
        id = e.params.data.id;

        ajaxGet(ajaxUrlListChart, { Rows: 1000, Search_business_unit_group_id: $('#group').val(), Search_business_unit_subgroup_id: $('#subgroup').val(), Search_business_unit_division_id: $('#division').val(), Search_business_unit_departement_id: id }, function (source) {
            console.log(parseJson(source.rows));
            var databaru = [];

            $.each(parseJson(source.rows), function (index, value) {
                var namelogic = value.name;
                if (namelogic === null || namelogic === "") {
                    namelogic = "Vacant";
                }
                var img = value.image;
                if (img === null || img === "") {
                    img = "../assets/img/avatars/no-image-icon-11.PNG";
                }

                databaru.push({
                    "Id": value.id,
                    "parentId": value.parentId,
                    "Title": value.title,
                    "Name": namelogic,
                    "Image": img,
                    "Grade": value.grade
                });
            });
            console.log(databaru);
            var peopleElement = document.getElementById("ChartGrafik");
            var orgChart = new getOrgChart(peopleElement, {
                // theme: "myTheme",
                siblingSeparation: 80,
                subtreeSeparation: 200,
                theme: "ada",
                boxSizeInPercentage: {
                    minBoxSize: {
                        width: 5,
                        height: 5
                    },
                    boxSize: {
                        width: 20,
                        height: 20
                    },
                    maxBoxSize: {
                        width: 150,
                        height: 100
                    }
                },
                primaryFields: ["Title", "Name", "Grade"],
                photoFields: ["Image"],
                linkType: "M",
                enableEdit: false,
                enableDetailsView: false,
                dataSource: databaru
            });

        });

    });



    getOrgChart.themes.myTheme =
    {
        size: [330, 260],
        toolbarHeight: 46,
        textPoints: [
            { x: 20, y: 45, width: 300 },
            { x: 120, y: 100, width: 200 },
            { x: 120, y: 125, width: 200 }
        ],
        //textPointsNoImage: [] NOT IMPLEMENTED,
        box: '<rect x="0" y="0" height="260" width="330" rx="10" ry="10" class="get-box"></rect>'
            + '<g transform="matrix(0.25,0,0,0.25,123,142)"><path d="M48.014,42.889l-9.553-4.776C37.56,37.662,37,36.756,37,35.748v-3.381c0.229-0.28,0.47-0.599,0.719-0.951  c1.239-1.75,2.232-3.698,2.954-5.799C42.084,24.97,43,23.575,43,22v-4c0-0.963-0.36-1.896-1-2.625v-5.319  c0.056-0.55,0.276-3.824-2.092-6.525C37.854,1.188,34.521,0,30,0s-7.854,1.188-9.908,3.53C17.724,6.231,17.944,9.506,18,10.056  v5.319c-0.64,0.729-1,1.662-1,2.625v4c0,1.217,0.553,2.352,1.497,3.109c0.916,3.627,2.833,6.36,3.503,7.237v3.309  c0,0.968-0.528,1.856-1.377,2.32l-8.921,4.866C8.801,44.424,7,47.458,7,50.762V54c0,4.746,15.045,6,23,6s23-1.254,23-6v-3.043  C53,47.519,51.089,44.427,48.014,42.889z M51,54c0,1.357-7.412,4-21,4S9,55.357,9,54v-3.238c0-2.571,1.402-4.934,3.659-6.164  l8.921-4.866C23.073,38.917,24,37.354,24,35.655v-4.019l-0.233-0.278c-0.024-0.029-2.475-2.994-3.41-7.065l-0.091-0.396l-0.341-0.22  C19.346,23.303,19,22.676,19,22v-4c0-0.561,0.238-1.084,0.67-1.475L20,16.228V10l-0.009-0.131c-0.003-0.027-0.343-2.799,1.605-5.021  C23.253,2.958,26.081,2,30,2c3.905,0,6.727,0.951,8.386,2.828c1.947,2.201,1.625,5.017,1.623,5.041L40,16.228l0.33,0.298  C40.762,16.916,41,17.439,41,18v4c0,0.873-0.572,1.637-1.422,1.899l-0.498,0.153l-0.16,0.495c-0.669,2.081-1.622,4.003-2.834,5.713  c-0.297,0.421-0.586,0.794-0.837,1.079L35,31.623v4.125c0,1.77,0.983,3.361,2.566,4.153l9.553,4.776  C49.513,45.874,51,48.28,51,50.957V54z" fill="#FFFFFF"/></g>'
            + '<g transform="matrix(1,0,0,1,20,190)" class="btn" data-action="edit"><path d="M5 0 L97 0 Q97 0 97 0 L97 45 Q97 45 97 45 L5 45 Q0 45 0 40 L0 5 Q0 0 5 0 Z"></path><text x="35" y="27" width="60">Edit</text></g>'
            + '<g transform="matrix(1,0,0,1,214,190)" class="btn" data-action="preview"><path d="M0 0 L92 0 Q97 0 97 5 L97 40 Q97 45 92 45 L0 45 Q0 45 0 45 L0 0 Q0 0 0 0 Z"></path><text x="25" y="27" width="60">Profile</text></g>',
        text: '<text width="[width]" class="get-text get-text-[index]" x="[x]" y="[y]">[text]</text>',
        image: '<clipPath id="clip"><circle cx="60" cy="120" r="40" /></clipPath><image preserveAspectRatio="xMidYMid slice" clip-path="url(#clip)" xlink:href="[href]" x="20" y="80" height="80" width="80"/>',
        expandCollapseBtnRadius: 20
    };

    ajaxGet(ajaxUrlListChart, { Rows: 1000 }, function (source) {
        //console.log(parseJson(source));
        var databaru = [];

        $.each(parseJson(source.rows), function (index, value) {
            var namelogic = value.name;
            if (namelogic === null || namelogic === "") {
                namelogic = "Vacant";
            }
            var img = value.image;
            if (img === null || img === "") {
                img = "../assets/img/avatars/no-image-icon-11.PNG";
            }

            databaru.push({
                "Id": value.id,
                "parentId": value.parentId,
                "Title": value.title,
                "Name": namelogic,
                "Image": img,
                "Grade": value.grade
            });
        });
        //console.log(databaru);
        var peopleElement = document.getElementById("ChartGrafik");
        var orgChart = new getOrgChart(peopleElement, {
            //theme: "myTheme",
            siblingSeparation: 80,
            subtreeSeparation: 200,
            theme: "sara",
            boxSizeInPercentage: {
                minBoxSize: {
                    width: 5,
                    height: 5
                },
                boxSize: {
                    width: 20,
                    height: 20
                },
                maxBoxSize: {
                    width: 150,
                    height: 100
                }
            },
            primaryFields: ["Title", "Name", "Grade"],
            photoFields: ["Image"],
            linkType: "M",
            enableEdit: false,
            enableDetailsView: false,
            dataSource: databaru
        });

    });

    function getNodeByClickedBtn(el) {
        while (el.parentNode) {
            el = el.parentNode;
            if (el.getAttribute("data-node-id"))
                return el;
        }
        return null;
    }


    var init = function () {
        var btns = document.getElementsByClassName("btn");
        for (var i = 0; i < btns.length; i++) {

            btns[i].addEventListener("click", function () {
                var nodeElement = getNodeByClickedBtn(this);
                var action = this.getAttribute("data-action");
                var id = nodeElement.getAttribute("data-node-id");
                var node = orgChart.nodes[id];

                switch (action) {
                    case "edit":
                        //DangerNotif("edit");
                        orgChart.showEditView(id);
                        break;
                    case "preview":
                        orgChart.showDetailsView(id);
                        break;
                }
            });
        }
    };
    init();
});



 

