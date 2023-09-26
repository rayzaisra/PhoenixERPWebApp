(function ($) {
    'use strict';
    var pageFunction = function () {
        var $applicationModule = $('#application_module_id');

        $applicationModule.cmSelect2({
            url: $.helper.resolveApi("~/core/ApplicationModule/lookup"),
            filters: ' r.module_name LIKE @0 ',
            //paramaters: [],
            result: {
                id: 'id',
                text: 'module_name',
            }
        }).on('select2:select', function (e) {
            var data = e.params.data;
            TreeView(data.id);
        });

        var editRecord = function (id = null, parent = '#',isGroup =false,myParentIsGroup = false) {
            var url = "~/Core/Menu/Detail/";
            mApp.block("#m_blockui_list", {
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });

            var moduleSelected = $applicationModule.val();
            if (moduleSelected == null) {
                alert('please select module');
                mApp.unblock("#m_blockui_list");
                return;
            }

            url += "?id=" + (id === null ? "" : id);
            url += "&parentid=" + (parent == '#' ? "" : parent);
            url += "&moduleid=" + $applicationModule.val();
            url += "&is_group=" + isGroup;
            url += "&parent_is_group=" + myParentIsGroup;
            $('.modal-container').load($.helper.resolve(url), function (result) {
                $('#m_modal_menu').modal({ show: true });
                mApp.unblock("#m_blockui_list");
            });
        }

        var TreeView = function (categoryId) {
            $('#tree_menu').jstree("destroy").empty();
            var tree = $('#tree_menu').jstree({
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
                            url: $.helper.resolveApi("~core/Menu/treelist/" + categoryId),
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
                    "file": {
                        "icon": "fa fa-file icon-state-warning icon-lg"
                    }
                },
                state: { "key": "demo2" },
                plugins: ["dnd", "state", "types", "contextmenu", "checkbox"]


            });
            function customMenu(node) {
                // The default set of all items
                var items = {
                    Add: {
                        label: "Add",
                        icon: "icon-plus",
                        action: function (n) {
                            editRecord(null, node.id);
                        }
                    },
                    Edit: {
                        label: "Edit",
                        icon: "icon-note",
                        action: function () {
                            editRecord(node.id, node.parent, node.original.is_group, node.original.myParentIsGroup || false);
                        }
                    },
                    "Rename": {
                        "label": "Rename",
                        "action": function (obj) {
                            alert("Rename");
                            //console.log(obj);
                            //var inst = $.jstree.reference(obj.reference),
                            //    data = inst.get_node(obj.reference),
                            //    id = data.id;

                            //inst.edit(data);
                        }
                    },
                    Delete: {
                        label: "Delete",
                        icon: "icon-trash",
                        action: function () {
                            alert("Delete");
                        }
                    },
                    SetPermission: {
                        label: "Set Permission",
                        icon: "icon-wrench",
                        action: function () {
                            alert('test');
                        }
                    }
                };

                return items;
            }

            tree.bind("move_node.jstree rename_node.jstree", function (e, data) {
                if (e.type == "move_node") {
                    $.get($.helper.resolveApi("~/core/Menu/orderMenu"),
                        {
                            recordId: data.node.id,
                            targetId: data.parent,
                            newOrderIndex: data.old_position < data.position ? data.position + 1 : data.position
                        }, function (data) {
                        if (data.status.success) {
                            tree.jstree(true).refresh();
                        }
                    });
                }
            });
        };

        //BEGIN::EVENT BUTTON
        $('#btn-add').click(function () {
            editRecord(null,'#');
        });
        $('#btn-add_group').click(function () {
            editRecord(null, '#', true);
        });
        

        
        //END::EVENT BUTTON


        //BEGIN::EVENT BUTTON
        
        //END::EVENT BUTTON
        return {
            init: function () {
                //initialize
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


