var ajaxUrlGetTreeKenoUiFormatData = API + "General/BusinessUnit/ListWithChildKendoUI";

$(document).ready(function () {
       // pageFunction.init();

            $("#treeview").kendoTreeView({
                template: kendo.template($("#treeview-template").html()),

                dataSource: ajaxGet(ajaxUrlGetTreeKenoUiFormatData, { rows: 1000 }, function (response) {
                    console.log(response);
                    response = parseJson(response);
                })
            });

// Delete button behavior
            $(document).on("click", ".k-icon.k-i-close-outline", function(e) {
                e.preventDefault();
                var treeview = $("#treeview").data("kendoTreeView");
                console.log(treeview);
           // treeview.remove($(this).closest(".k-item"));
        });
 });






