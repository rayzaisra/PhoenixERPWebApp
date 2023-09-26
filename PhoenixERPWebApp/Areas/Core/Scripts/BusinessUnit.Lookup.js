(function ($) {
    'use strict';

    var pageFunction = function () {
        var $tree = $('#tree_businessunit');
        var TreeView = function () {
            var e;
            e = $tree.jstree({
                "themes": {
                    "theme": "default",
                    "dots": false,
                    "icons": true
                },
                'core': {
                    "themes": {
                        "responsive": false
                    },
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
                plugins: ["dnd", "state", "types"]


            }), e.bind('loaded.jstree', function (e, data) {
                // invoked after jstree has loaded
                $('#btn-group-selectected').show();
            });



        };

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


