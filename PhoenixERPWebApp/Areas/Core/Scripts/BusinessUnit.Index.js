(function ($) {
    'use strict';
    var pageFunction = function () {
        var $tree = $('#tree_businessunit');

        var TreeView = function () {

            var tree = $tree.jstree({
                "themes": {
                    "theme": "default",
                    "dots": false,
                    "icons": true
                },
                contextmenu: {
                    select_node: false,
                    items: customMenu
                },
                checkbox: {
                    cascade: 'undetermined',
                    visible: true,
                    three_state: true
                },
                'core': {
                    "themes": {
                        "responsive": false
                    },
                    "check_callback": true,
                    'data': function (obj, callback) {
                        $.ajax({
                            type: "GET",
                            dataType: 'json',
                            url: $.helper.resolveApi("~core/BusinessUnit"),
                            success: function (r) {
                                console.log(r);
                                if (r.status.success) {
                                    callback.call(this, r.data);
                                }
                            }
                        });
                    }

                },
                "types": {
                    "default": {
                        "icon": "fa fa-folder icon-state-warning icon-lg"
                    },
                    "root": {
                        "class": "css-custom"
                    },
                    "file": {
                        "icon": "fa fa-file icon-state-warning icon-lg"
                    }
                },
                state: { "key": "demo2" },
                plugins: ["dnd", "state", "types", "contextmenu", "checkbox"]


            });
            function customMenu(node) {
                // The default set of all items
                var parentnode = node;

                var items = {
                    Create: {
                        label: "Create",
                        icon: "icon-plus",
                        action: function (n) {
                            window.location.href = "/Core/BusinessUnit/Detail?parentid=" + node.id + "&parentname=" + node.text;
                        }
                    },
                    Edit: {
                        label: "Edit",
                        icon: "icon-note",
                        action: function () {
                            if (node.parent == "#") {
                                window.location.href = "/Core/BusinessUnit/Detail?id=" + node.id;
                            } else {
                                window.location.href = "/Core/BusinessUnit/Detail?id=" + node.id + "&parentid=" + node.parent + "&parentname=" + node.text;

                            }

                        }
                    }
                    //"Rename": {
                    //    "label": "Rename",
                    //    "action": function (obj) {
                    //        alert("Rename");
                    //        //console.log(obj);
                    //        //var inst = $.jstree.reference(obj.reference),
                    //        //    data = inst.get_node(obj.reference),
                    //        //    id = data.id;

                    //        //inst.edit(data);
                    //    }
                    //},
                    //Delete: {
                    //    label: "Delete",
                    //    icon: "icon-trash",
                    //    action: function () {
                    //        alert("Delete");
                    //        //var data = menu.jstree(true).get_selected();
                    //        //App.blockUI({
                    //        //    boxed: true
                    //        //});
                    //        //var item = data;
                    //        //var token = $('input[name="__RequestVerificationToken"]').val();
                    //        //$.ajax({
                    //        //    url: "/Settings/Menu/Delete",
                    //        //    dataType: 'json',
                    //        //    data: {
                    //        //        Ids: item,
                    //        //        __RequestVerificationToken: token
                    //        //    },
                    //        //    type: 'POST',
                    //        //    success: function (data) {
                    //        //        if (data && data.Success) {
                    //        //            window.setTimeout(function () {
                    //        //                App.unblockUI();
                    //        //                swal({
                    //        //                    title: data.Message,
                    //        //                    type: "success"
                    //        //                }, function () {
                    //        //                    menu.jstree(true).refresh();
                    //        //                });
                    //        //            }, 2000);
                    //        //        } else {
                    //        //            window.setTimeout(function () {
                    //        //                App.unblockUI();
                    //        //                swal({
                    //        //                    title: data.Message,
                    //        //                    text: 'All Transaction has Canceled!',
                    //        //                    type: "error"
                    //        //                });
                    //        //            }, 2000);
                    //        //        }
                    //        //    }
                    //        //});
                    //    }
                    //},
                    //SetPermission: {
                    //    label: "Set Permission",
                    //    icon: "icon-wrench",
                    //    action: function () {
                    //        alert('test');
                    //    }
                    //}
                };

                return items;
            }

        };

        var DeleteData = function (ArrID, callback = null) {
            $.ajax({
                type: "DELETE",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/core/BusinessUnit/delete"),
                data: JSON.stringify(ArrID),
                success: function (r) {
                    if (r.status.success) {
                        swal({
                            title: 'Deleted!',
                            text: r.status.message,
                            type: "success"
                        }).then(function () {
                            ListChecked = [];
                            ListNotChecked = [];
                            dt_basic.ajax.reload();
                        });
                    } else {
                        swal(
                            'Information',
                            r.status.message,
                            'info'
                        );
                    }
                },
                error: function (e, t, s) {
                    swal(
                        'Information',
                        'Ooops, something went wrong !',
                        'info'
                    );
                }
            }).then(setTimeout(function () {
                if (callback != null) {
                    callback();
                }
            }, 2e3));
        };

        //BEGIN::EVENT BUTTON
        $('#btn-add').click(function () {
            window.location.href = $.helper.resolve("~/Core/BusinessUnit/Detail/");
        });

        $('#btn-delete').click(function () {
            var node = $tree.jstree("get_selected", true);
            console.log(node);
            console.warn($tree.jstree().get_path(node[0], ' > '));

            //var btn = $(this);
            //if (ListChecked.length > 0) {
            //    btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            //    swal({
            //        title: "Are you sure?",
            //        text: "You won't be able to revert this!",
            //        type: "warning",
            //        showCancelButton: !0,
            //        confirmButtonText: "Yes",
            //        cancelButtonText: "No",
            //        reverseButtons: !0
            //    }).then(function (e) {
            //        if (e.value) {
            //            DeleteData(ListChecked, function () {
            //                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            //            });
            //        }
            //    });
            //}


        });

        //END::EVENT BUTTON


        //BEGIN::EVENT BUTTON

        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
                TreeView();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


