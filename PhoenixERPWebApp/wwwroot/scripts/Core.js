(function ($) {
    var basePath = '',
        apiBasePath = '',
        currAuthentication = '',
        tokenData = [];

    var coreHelper = {
        Approval: {

        },
        getCheckedValues: function (wrapper) {
            var arr = [],
                $wrapper = $(wrapper);

            $.each($wrapper.find("input:checked"), function (k, v) {
                var $el = $(v),
                    value = $el.val();

                arr.push(value);
            });

            return arr;
        },
        controlCheckboxValue: function () {
            $("input:checkbox").on("change", function () {
                $(this).val($(this).is(':checked') ? true : false);
            });
        },
        loadScript: function (url, callback) {
            var script = document.createElement("script")
            script.type = "text/javascript";

            if (script.readyState) {  //IE
                script.onreadystatechange = function () {
                    if (script.readyState == "loaded" ||
                        script.readyState == "complete") {
                        script.onreadystatechange = null;
                        callback();
                    }
                };
            } else {  //Others
                script.onload = function () {
                    callback();
                };
            }

            script.src = url;
            document.getElementsByTagName("head")[0].appendChild(script);
        },
        setBasePath: function (path) {
            basePath = path;
            this.setBasePath = null;
        },
        setApiBasePath: function (path) {
            apiBasePath = path;
            this.setApiBasePath = null;
        },
        resolveApi: function (url) {
            return url.replace('~', apiBasePath);
        },
        resolve: function (url) {
            return url.replace('~', basePath);
        },
        setTokenData: function (payload) {
            tokenData = payload;
            sessionStorage.setItem("PhoenixWebUserSession", tokenData);
        },
        getTokenData: function () {
            tokenData = tokenData || sessionStorage.getItem("PhoenixWebUserSession");
            return tokenData;
        },
        getJqueryObject: function (obj) {
            return (typeof obj === 'string') ? $('#' + obj) : (obj.jquery ? obj : $(obj));
        },
        toDefault: function (data, defaultValue) {
            return typeof data === 'undefined' || data === null || data === '' ? defaultValue : data;
        },
        traverse: function (data, childField, callback) {
            var stack = [data];
            var n;

            while (stack.length > 0) {
                n = stack.pop();
                callback(n);

                if (!n[childField]) {
                    continue;
                }

                for (var i = n[childField].length - 1; i >= 0; i--) {
                    stack.push(n[childField][i]);
                }
            }
        },
        close: function (el, parEl) {
            $(el).closest(parEl).remove();
        },
        onError: function (el, args) {
            var $obj = $.helper.getJqueryObject(el),
                modelState = args.responseJSON.ModelState,
                validationMessageTmpl = kendo.template($("<script type='text/kendo-template' id='message'><div class='k-widget k-tooltip k-tooltip-validation k-invalid-msg field-validation-error' style='margin: 0.5em; display: block; ' data-for='#=field#' data-valmsg-for='#=field#' id='#=field#_validationMessage'><span class='k-icon k-i-warning'></span>#=message# <span class='k-icon k-i-close-circle' style='cursor: pointer' onclick='$.helper.close(this, \".k-widget\");'></span><div class='k-callout k-callout-n'></div></div></script>").html());

            coreHelper.clearError($obj);

            function showMessage(container, name, errors) {
                var wrapper = container.find('span[data-valmsg-for=' + name + '],span[data-val-msg-for=' + name + ']');

                wrapper.parent().append(validationMessageTmpl({ field: name, message: errors[0] }));
            }

            for (var key in modelState) {
                var arr = modelState[key];

                showMessage($obj, key.substr(key.indexOf('.') + 1), arr);
            }
        }
    };

    var controlHelper = {
        form: {
            clear: function (form, excludes) {
                excludes = typeof excludes === 'undefined' ? [] : excludes;
                var $el = coreHelper.getJqueryObject(form);

                $el.find('input, select, textarea').each(function (e) {
                    var obj = $(this),
                        defaultValue = obj.data("defaultValue") || "",
                        includeFilter = typeof obj.data("clearFilter") === 'undefined' ? true : obj.data("clearFilter");

                    if ($.inArray(obj[0].name, excludes) == -1 && includeFilter) {
                        if (obj[0].tagName == 'SELECT') {
                            if (obj.hasClass('select2')) {
                                obj.val([]);
                                obj.trigger('change');
                            } else {
                                obj[0].selectedIndex = 0;
                                obj.trigger('change');
                            }
                        } else {
                            if (typeof obj.data("kendoUpload") !== 'undefined') {
                                obj.closest(".k-upload").find(".k-i-close").trigger("click");
                            }
                            else if (typeof obj.data("kendoDropDownList") !== 'undefined') {
                                obj.data("kendoDropDownList").value("-1");
                                obj.data("kendoDropDownList").trigger("change");
                            } else if (obj.attr('type') == 'file') {
                                var root = obj.closest(".fileinput");

                                if (root.length > 0) {
                                    root.fileinput('clear');
                                    root.find("a.submit").off("click").attr("disabled", "disabled");
                                }
                                //obj.closest(".fileinput").find("a[data-dismiss='fileinput']").trigger("click");
                            } else {
                                obj.val(defaultValue);

                                if (obj.attr('data-role') == 'tagsinput') {
                                    obj.tagsinput('removeAll');
                                }
                            }
                        }
                    }
                });
            }
        }
    };

    var KendoHelper = {
        kendoUI: {
            notification: {

            },
            multiSelect: {
                setValue: function (el, value) {
                    if (value == null) return;
                    var t = $(el).data("kendoMultiSelect");
                    t.dataSource.filter({}); //clear applied filter before setting value
                    t.value(value);
                }
            },
            dropdownlist: {
                build: function () {

                },
                setValue: function (el, value) {
                    if (value.id == null) return;
                    var t = $(el).data("kendoDropDownList");
                    t.dataSource.add(value);
                    t.value(value.id);
                }
            },
            combobox: {
                build: function (el, options) {
                    var defaultOptions = {
                        placeholder: "Choose ...",
                        dataTextField: "text",
                        dataValueField: "id",
                        filter: "contains",
                        autoBind: false,
                        dataSource: {
                            type: "json",
                            serverFiltering: true,
                            transport: {
                                read: {
                                    url: options.options.url || '/',
                                    contentType: 'application/json',
                                    dataType: 'json',
                                    type: 'POST',
                                    cache: false
                                },
                                parameterMap: function (data, operation) {
                                    var mapRequest = data;
                                    return JSON.stringify(mapRequest);
                                }
                            },
                            schema: {
                                model: {
                                    id: "id",
                                    children: "items"
                                },
                                data: "items"
                            }

                        }
                    }
                },
                setValue: function (el, value) {
                    if (value.id == null) return;
                    var t = $(el).data("kendoComboBox");
                    t.dataSource.add(value);
                    t.value(value.id);
                }
            },
            dropDownTree: {
                setValue: function (el, value) {
                    var t = $(el).data("kendoDropDownTree");
                    t.dataSource.add(value);
                    t.value(value.id);
                }
            },
            grid: function (el, options) {
                var defaultOptions = {
                    dataSource: {
                        transport: {
                            read: {
                                url: options.options.url || '/',
                                contentType: 'application/json',
                                dataType: 'json',
                                type: 'POST',
                                cache: false
                            },
                            parameterMap: function (data, operation) {
                                if (operation !== "read" && data) {
                                    //options.options.parameterMap(data, operation);
                                    console.log("paramdata");
                                    console.log(data);
                                    //console.log(Array.isArray(data.model))
                                    return JSON.stringify(data);
                                }
                                //if (operation !== "read" && data.models) {
                                //    console.log(data)
                                //    console.log(data.models)
                                //    return { models: kendo.stringify(data.models) };
                                //}

                                //if (operation !== "read" && options.models) {
                                //    return { models: kendo.stringify(options.models) };
                                //}

                                if (operation == "read") {
                                    var mapRequest = data;
                                    var extractOption = { columns: [], search: options.options.search || { value: "", regex: false } };
                                    $.each(options.columns, function (i, v) {
                                        extractOption.columns.push(
                                            {
                                                field: v.field,
                                                title: v.title,
                                                aggregates: v.aggregates,
                                                searchable: v.searchable || false,
                                                sortable: v.sortable || true
                                            });
                                    });
                                    mapRequest = $.extend(true, mapRequest, extractOption);
                                    return JSON.stringify(mapRequest);
                                }
                            }
                        },
                        schema: {
                            model: {
                                id: "id",
                                fields: {
                                    id: { editable: true, nullable: true }
                                }
                            },
                            data: "data",
                            total: "recordsTotal"
                        },
                        pageSize: 5,
                        serverPaging: true,
                        serverSorting: true
                    },
                    groupable: true,
                    sortable: true,
                    pageable: {
                        refresh: true,
                        pageSizes: true,
                        buttonCount: 5
                    },
                    columns: [],
                    marvelCheckboxSelectable: {
                        enable: true,
                        listCheckedTemp: options.marvelCheckboxSelectable.listCheckedTemp || []
                    }
                };

                defaultOptions = $.extend(true, defaultOptions, options);
                //console.log(defaultOptions);
                if (defaultOptions.marvelCheckboxSelectable.enable) {

                    defaultOptions.columns.splice(0, 0, {
                        selectable: true, width: "50px"
                    });
                }
                $(el).kendoGrid(defaultOptions);

                if (defaultOptions.marvelCheckboxSelectable.enable) {
                    $(el).data('kendoGrid').bind("change", function (e, arg) {
                        var grid = e.sender;
                        var items = grid.items();
                        items.each(function (idx, row) {
                            var idValue = grid.dataItem(row).get(options.schema.model.id);
                            var isChecked = row.className.indexOf("k-state-selected") >= 0;
                            if (isChecked) {
                                if (defaultOptions.marvelCheckboxSelectable.listCheckedTemp.indexOf(idValue) < 0) {
                                    defaultOptions.marvelCheckboxSelectable.listCheckedTemp.push(idValue);
                                }
                            }
                            else {
                                if (defaultOptions.marvelCheckboxSelectable.listCheckedTemp.indexOf(idValue) >= 0) {
                                    defaultOptions.marvelCheckboxSelectable.listCheckedTemp.splice(defaultOptions.marvelCheckboxSelectable.listCheckedTemp.indexOf(idValue), 1);
                                }
                            }
                        });
                    });

                    $(el).data('kendoGrid').bind("dataBound", function (e) {
                        var grid = e.sender;

                        grid.wrapper.find(".k-grouping-row .k-icon").on("click", function () {
                            $.helper.kendoUI.resizeGrid();
                        });


                        var items = grid.items();
                        var itemsToSelect = [];
                        items.each(function (idx, row) {
                            var dataItem = grid.dataItem(row);
                            if (defaultOptions.marvelCheckboxSelectable.listCheckedTemp.indexOf(dataItem[options.schema.model.id]) >= 0) {
                                itemsToSelect.push(row);
                            }
                        });
                        e.sender.select(itemsToSelect);
                    });
                }



            },
            //resizeGrid: function (grid) {
            //    setTimeout(function () {
            //        var lockedContent = grid.wrapper.children(".k-grid-content-locked")
            //        var content = grid.wrapper.children(".k-grid-content");

            //        grid.wrapper.height("");
            //        lockedContent.height("");
            //        content.height("");

            //        grid.wrapper.height(grid.wrapper.height());

            //        grid.resize();
            //    });
            //}
            resizeGrid: function (options) {
                if (options === null || options === undefined) {
                    options = {
                        size: 0.55,
                        gridContentCss: ".k-grid-content",
                        gridLockedContentCss: ".k-grid-content-locked",
                        gridsToResize: []
                    };
                }


                var windowHeight = $(window).height() * options.size;

                if (options.gridsToResize !== null && options.gridsToResize.length > 0) {
                    options.gridsToResize.forEach(function (item) {
                        var gridContent = $('#' + item + ' > ' + options.gridContentCss);


                        var lockedContent = $('#' + item + ' > ' + options.gridLockedContentCss);

                        gridContent.height(windowHeight);

                        if (lockedContent !== null && lockedContent !== undefined) {
                            lockedContent.height(windowHeight);

                        }
                    });
                }
                else {
                    var gridContent = $(options.gridContentCss);
                    var lockedContent = $(options.gridLockedContentCss);

                    gridContent.height(windowHeight);

                    if (lockedContent !== null && lockedContent !== undefined) {
                        lockedContent.height(windowHeight);

                    }
                }

                $(window).resize(function () {
                    $.helper.kendoUI.resizeGrid(options);
                });

            }
        }
    };
    $.helper = $.extend({}, controlHelper, coreHelper, KendoHelper);

    $(function () {

        $(".marvel-datepicker").datepicker({
            todayHighlight: !0,
            autoclose: !0,
            orientation: "bottom-left",
            format: "mm/dd/yyyy"
        });

        //$(".marvel-k-date").kendoDatePicker({
        //    format: "MM/dd/yyyy"
        //});

        $("body").on("click", ".trigger-modal", function () {
            var $this = $(this),
                $modal = $.helper.getJqueryObject($this.data("modal")),
                refreshComponents = typeof $this.data("refreshComponents") === 'undefined' ? false : $this.data("refreshComponents"),
                hasLoader = $modal.find(".loader").length > 0;

            if (hasLoader) {
                var $loader = $modal.find(".loader"),
                    dataUrl = $loader.data("url"),
                    parameters = typeof $this.data("parameters") !== 'undefined' ? $this.data("parameters") : $loader.data("parameters"),
                    sendParameters = typeof parameters === 'string' ? window[parameters]() : parameters;

                $loader.html('');
                $modal.modal("show");
                App.blockUI({ boxed: true, target: $loader });
                $loader.load(dataUrl, sendParameters, function (d) {
                    $modal.find(".loader").html(d);
                    App.unblockUI($loader);
                });
            } else {
                var $form = $modal.find("form");

                if ($form.length > 0) {
                    $.helper.clearError($form);
                    $.helper.form.clear($form);
                    $modifiedByEl = $form.find("input[name='ModifiedBy']");

                    if ($modifiedByEl.length > 0) {
                        $modifiedByEl.val(currAuthentication);
                    }
                }

                $modal.modal("show");
            }

        });

    });

    $.fn.serializeFormJSON = function () {

        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                    console.log('618');
                }
                o[this.name].push(this.value || null);
                console.log('621');
            } else {
                o[this.name] = this.value || null;
                console.log('623');
            }
        });
        return o;
    };


    $.fn.cmserializeToJSON = function () {
        var obj = {};
        /* Iterate over every input. */
        var data = this.serializeArray();
        data.map(function (data, i) {
            if (data.name.indexOf(".") > 0) {
                var prop = data.name.split(".");
                var key = prop[0]
                var val = prop[1]
                if (val.indexOf("[") > 0) {
                    var index = parseInt(val.match(/\d+/), 10);
                    if (!Array.isArray(obj[key])) {
                        obj[key] = []
                    }
                    if (typeof obj[key][index] != "object") {
                        obj[key][index] = {}
                    }
                    val = val.substr(0, val.length - 3);
                    obj[key][index][val] = data.value;
                } else {
                    if (typeof obj[key] != "object") {
                        obj[key] = {}
                    }
                    obj[key][val] = data.value;
                }
            } else {
                console.log("data value : " + data.value);
                if (data.value == undefined || data.value == "")
                    obj[data.name] = null;
                else
                    obj[data.name] = data.value;
            }
        });

        /* Stringify the object and return it. */
        return JSON.stringify(obj, null, 4);
    };
}(jQuery));