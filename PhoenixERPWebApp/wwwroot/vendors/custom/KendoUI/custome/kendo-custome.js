function errorHandler(e) {
    if (e.status === 'customerror') {
        if (e.errors) {
            var message = '';
            $.each(e.errors, function (key, value) {
                title = key;
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
            });

            $('.k-grid').each(function (item) {
                var grid = $(this).data("kendoGrid");
                if (e.sender === grid.dataSource) {
                    grid.cancelChanges();
                }
            });

            swal('Error', message, 'error');
        }
    } else if (e.status === "error") {
        if (e.xhr.responseJSON !== undefined) {
            var errorMessage = e.xhr.responseJSON.errorMessage;
            if (errorMessage === "" || errorMessage === undefined) {
                errorMessage = "Ooops, something went wrong !";
            }
            //console.log(errorMessage);
            swal('Error', errorMessage, 'error');
        }
    }
}

function dataItem(e) {
    var $elem = $(e);
    var grid = $elem.closest("div[data-role='grid']").data("kendoGrid");
    var item = grid.dataItem($elem.closest("tr"));
    return item;
}

function refreshGrid(e) {
    var $elem = $(e);
    var grid = $elem.closest("div[data-role='grid']").data("kendoGrid");
    grid.dataSource.read();
}

function deleteRow(e, url) {
    var item = dataItem(e);
    swal({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (ok) {
        if (ok.value) {
            mApp.block("#m_blockui_list", {
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });

            $.ajax({
                type: "DELETE",
                url: url,
                data: JSON.parse(JSON.stringify(item)),
                success: function (result) {
                    if (result.success) {
                        swal('Information', result.message, 'success')
                            .then(function (ok) {
                                if (ok.value) {
                                    if (result.url !== undefined) { window.location = result.url; } else { refreshGrid(e); }
                                }
                            });
                    } else {
                        swal('Warning', result.message, 'warning');
                    }
                    mApp.unblock("#m_blockui_list");
                },
                error: function (e, t, s) {
                    var errorMessage = e.responseJSON.errorMessage;
                    if (errorMessage === "" || errorMessage === undefined) {
                        errorMessage = "Ooops, something went wrong !";
                    }
                    //console.log(errorMessage);
                    swal('Error', errorMessage, 'error');
                    mApp.unblock("#m_blockui_list");
                }
            }).then(/*(function () {
                mApp.unblock("#m_blockui_list");
            }, 2e3)*/);
        }
    }).catch(swal.noop);
}

function deleteGridRowDataSource(e) {
    swal({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        type: "warning",
        showCancelButton: !0,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        reverseButtons: !0
    }).then(function (ok) {
        if (ok.value) {
            mApp.block("#m_blockui_list", {
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });

            var item = dataItem(e);
            $("#grid").data("kendoGrid").dataSource.remove(item);
        }
    }).catch(swal.noop);
}

function callAction(parameter) {
    if (parameter.formId !== undefined) {
        if (parameter.formId.substring(0, 1) !== "#") parameter.formId = "#" + parameter.formId;
        var valid = $(parameter.formId).validate().form();
        //console.log(parameter.formId);
        //console.log(valid);
        if (valid) {
            swal({
                title: "Confirmation",
                text: 'Are you sure want to ' + parameter.title + ' this data?',
                type: "question",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (ok) {
                if (ok.value) {
                    mApp.block("#m_blockui_list", {
                        overlayColor: "#000000",
                        type: "loader",
                        state: "primary",
                        message: "Processing..."
                    });

                    //var data = $(parameter.formId).serialize();

                    if (document.getElementById("newLoaderModal")) {
                        $('#newLoaderModal').css('zIndex', 9999);
                        $('#newLoaderModal').modal('show');

                    } else {
                        console.log('Modal Not Exists');
                    }

                    var object = {};
                    var params = $(parameter.formId).serializeArray();
                    $.each(params, function (i, val) {
                        object[val.name] = val.value;
                    });

                    if (parameter.objects !== undefined) {
                        $.each(parameter.objects, function (i, val) {
                            var arry = [];
                            for (var z = 0; z < val.length; z++) {
                                var obj = JSON.parse(JSON.stringify(val[z]));
                                arry.push(obj);
                            }
                            object[i] = arry;
                        });
                    }

                    var url = parameter.url;
                    if (url === undefined) url = $(parameter.formId).attr('action');
                    //console.log(url);
                    $.ajax({
                        type: parameter.type,
                        url: url,
                        data: object,
                        dataType: 'json',
                        success: function (result) {
                            if (result.success) {
                                if (result.message === undefined || result.message === null) {
                                    if (result.url !== undefined) window.location = result.url;
                                } else {
                                    swal('Information', result.message, 'success')
                                        .then(function (ok) {
                                            if (ok.value) {
                                                if (parameter.callback !== undefined) parameter.callback(result);
                                                if (result.data !== undefined) {
                                                    $("#Id").val(result.data.id);
                                                    $("#InvoiceNumber").val(result.data.invoiceNumber);
                                                    $("#RequestCode").val(result.data.requestCode);
                                                    $("#RequestNumber").val(result.data.requestNumber);
                                                    $("#Code").val(result.data.code);
                                                    $("#CreatedBy").val(result.data.createdBy);
                                                    $("#CreatedOn").val(result.data.createdOn);
                                                    //var status = "Draft";
                                                    //if (result.data.Status !== undefined) status = result.data.Status; $("#Status").val(status);
                                                    //if (result.data.StatusDesc !== undefined) status = result.data.StatusDesc; $("#StatusDesc").val(status);

                                                }

                                                if (result.url !== undefined) window.location = result.url;

                                                if (result.setHiddenValue !== undefined) {
                                                    $.each(result.setHiddenValue, function (index, data) {
                                                        $('#' + data.id).val(data.value);
                                                    });
                                                }

                                                if (result.hideModalPopUp !== undefined)
                                                    $('#' + result.hideModalPopUp).modal('hide');

                                                if (result.btnTriggerId !== undefined) {
                                                    debugger;
                                                    $('#' + result.btnTriggerId).click();
                                                }

                                                if (result.refreshGrid !== undefined) {
                                                    var gridArray = result.refreshGrid.split(";");
                                                    $.each(gridArray, function (index, value) {
                                                        $('#' + value).data('kendoGrid').dataSource.read();
                                                        $('#' + value).data('kendoGrid').refresh();
                                                    });
                                                }

                                                if (result.refreshTreeList !== undefined) {
                                                    var gridTreeListArray = result.refreshGrid.split(";");
                                                    $.each(gridTreeListArray, function (index, value) {
                                                        $('#' + value).data('kendoTreeList').dataSource.read();
                                                        $('#' + value).data('kendoTreeList').refresh();
                                                    });
                                                }
                                            }
                                        });
                                }
                            } else {
                                swal('Warning', result.message, 'warning');
                            }
                            mApp.unblock("#m_blockui_list");

                            if (document.getElementById("newLoaderModal")) {
                                $('#newLoaderModal').css('zIndex', 9999);
                                $('#newLoaderModal').modal('hide');

                            } else {
                                console.log('Modal Not Exists');
                            }

                        },
                        error: function (e, t, s) {
                            var errorMessage = e.responseJSON.errorMessage;
                            if (errorMessage === "" || errorMessage === undefined) {
                                errorMessage = "Ooops, something went wrong !";
                            }
                            //console.log(errorMessage);
                            swal('Error', errorMessage, 'error');
                            mApp.unblock("#m_blockui_list");

                            if (document.getElementById("newLoaderModal")) {
                                $('#newLoaderModal').css('zIndex', 9999);
                                $('#newLoaderModal').modal('hide');

                            } else {
                                console.log('Modal Not Exists');
                            }

                        }
                    });
                }
            }).catch(swal.noop);
        }
    }
}

function callActionFile(parameter) {
    if (parameter.formId !== undefined) {
        if (parameter.formId.substring(0, 1) !== "#") parameter.formId = "#" + parameter.formId;
        var valid = $(parameter.formId).validate().form();
        if (valid) {
            swal({
                title: "Confirmation",
                text: 'Are you sure want to ' + parameter.title + ' this data?',
                type: "question",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (ok) {
                if (ok.value) {
                    mApp.block("#m_blockui_list", {
                        overlayColor: "#000000",
                        type: "loader",
                        state: "primary",
                        message: "Processing..."
                    });

                    if (document.getElementById("newLoaderModal")) {
                        $('#newLoaderModal').css('zIndex', 9999);
                        $('#newLoaderModal').modal('show');

                    } else {
                        console.log('Modal Not Exists');
                    }

                    var formData = new FormData();
                    $.each($(parameter.formId).find("input[type='file']"), function (i, tag) {
                        $.each($(tag)[0].files, function (i, file) {
                            formData.append(tag.name, file);
                        });
                    });

                    var params = $(parameter.formId).serializeArray();
                    $.each(params, function (i, val) {
                        formData.append(val.name, val.value);
                    });

                    var object = {};
                    if (parameter.objects !== undefined) {
                        $.each(parameter.objects, function (i, val) {
                            var arry = [];
                            for (var z = 0; z < val.length; z++) {
                                var obj = JSON.parse(JSON.stringify(val[z]));
                                arry.push(obj);
                            }
                            object[i] = arry;
                        });
                    }
                    formData.appendRecursive(object, "");

                    var url = parameter.url;
                    if (url === undefined) url = $(parameter.formId).attr('action');

                    $.ajax({
                        type: parameter.type,
                        url: url,
                        cache: false,
                        contentType: false,
                        processData: false,
                        data: formData,
                        success: function (result) {
                            if (result.success) {
                                swal('Information', result.message, 'success')
                                    .then(function (ok) {
                                        if (ok.value) {
                                            if (parameter.callback !== undefined) parameter.callback(result);
                                            if (result.data !== undefined) {
                                                $("#Id").val(result.data.id);
                                                $("#InvoiceNumber").val(result.data.invoiceNumber);
                                                $("#RequestCode").val(result.data.requestCode);
                                                $("#RequestNumber").val(result.data.requestNumber);
                                                $("#Code").val(result.data.code);
                                                $("#CreatedBy").val(result.data.createdBy);
                                                $("#CreatedOn").val(result.data.createdOn);
                                            }

                                            if (result.url !== undefined) window.location = result.url;

                                            if (result.setHiddenValue !== undefined) {
                                                $.each(result.setHiddenValue, function (index, data) {
                                                    $('#' + data.id).val(data.value);
                                                });
                                            }

                                            if (result.hideModalPopUp !== undefined)
                                                $('#' + result.hideModalPopUp).modal('hide');

                                            if (result.btnTriggerId !== undefined) {
                                                $('#' + result.btnTriggerId).click();
                                            }

                                            if (result.refreshGrid !== undefined) {
                                                var gridArray = result.refreshGrid.split(";");
                                                $.each(gridArray, function (index, value) {
                                                    $('#' + value).data('kendoGrid').dataSource.read();
                                                    $('#' + value).data('kendoGrid').refresh();
                                                });
                                            }

                                            if (result.refreshTreeList !== undefined) {
                                                var gridTreeListArray = result.refreshGrid.split(";");
                                                $.each(gridTreeListArray, function (index, value) {
                                                    $('#' + value).data('kendoTreeList').dataSource.read();
                                                    $('#' + value).data('kendoTreeList').refresh();
                                                });
                                            }
                                        }
                                    });
                            } else {
                                swal('Warning', result.message, 'warning');
                            }
                            mApp.unblock("#m_blockui_list");

                            if (document.getElementById("newLoaderModal")) {
                                $('#newLoaderModal').modal('hide');
                            } else {
                                console.log('Modal Not Exists');
                            }
                        },
                        error: function (e, t, s) {
                            var errorMessage = e.responseJSON.errorMessage;
                            if (errorMessage === "" || errorMessage === undefined) {
                                errorMessage = "Ooops, something went wrong !";
                            }
                            //console.log(errorMessage);
                            swal('Error', errorMessage, 'error');
                            mApp.unblock("#m_blockui_list");

                            if (document.getElementById("newLoaderModal")) {
                                $('#newLoaderModal').modal('hide');
                            } else {
                                console.log('Modal Not Exists');
                            }
                        }
                    });/*.then(setTimeout(function () {
                        mApp.unblock("#m_blockui_list");

                        console.log("Finish 4");
                    }, 2e3));*/
                }
            }).catch(swal.noop);
        }
    }
}

function callActionWithDetails(parameter) {
    if (parameter.formId !== undefined) {
        if (parameter.formId.substring(0, 1) !== "#") parameter.formId = "#" + parameter.formId;
        var valid = $(parameter.formId).validate().form();
        if (valid) {
            swal({
                title: "Confirmation",
                text: 'Are you sure want to ' + parameter.title + ' this data?',
                type: "question",
                showCancelButton: !0,
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                reverseButtons: !0
            }).then(function (ok) {
                if (ok.value) {
                    mApp.block("#m_blockui_list", {
                        overlayColor: "#000000",
                        type: "loader",
                        state: "primary",
                        message: "Processing..."
                    });

                    if (document.getElementById("newLoaderModal")) {
                        $('#newLoaderModal').css('zIndex', 9999);
                        $('#newLoaderModal').modal('show');
                    } else {
                        console.log('Modal Not Exists');
                    }

                    var formData = new FormData();
                    var params = $(parameter.formId).serializeArray();


                    var data = {};
                    $(params).each(function (index, obj) {
                        data[obj.name] = obj.value;
                    });
                    data.details = parameter.objects;
                    //var a = {
                    //    TrMediaOrderTVDetails: parameter.objects
                    //};
                    //$.each(params, function (i, val) {
                    //    formData.append(val.name, val.value);
                    //});
                    //if (parameter.objects !== undefined) {
                    //    formData.append(parameter.objectName, parameter.objects);
                    //}
                    var url = parameter.url;
                    if (url === undefined) url = $(parameter.formId).attr('action');

                    $.ajax({
                        type: parameter.type,
                        url: url,
                        data: data,
                        success: function (result) {
                            if (result.success) {
                                swal('Information', result.message, 'success')
                                    .then(function (ok) {
                                        if (ok.value) {
                                            if (result.url !== undefined) window.location = result.url;
                                        }
                                    });
                            } else {
                                swal('Warning', result.message, 'warning');
                            }
                            mApp.unblock("#m_blockui_list");

                            if (document.getElementById("newLoaderModal")) {
                                $('#newLoaderModal').modal('hide');
                            } else {
                                console.log('Modal Not Exists');
                            }
                        },
                        error: function (e, t, s) {
                            var errorMessage = e.responseJSON.errorMessage;
                            if (errorMessage === "" || errorMessage === undefined) {
                                errorMessage = "Ooops, something went wrong !";
                            }
                            //console.log(errorMessage);
                            swal('Error', errorMessage, 'error');
                            mApp.unblock("#m_blockui_list");

                            if (document.getElementById("newLoaderModal")) {
                                $('#newLoaderModal').modal('hide');
                            } else {
                                console.log('Modal Not Exists');
                            }
                        }
                    });/*.then(setTimeout(function () {
                        mApp.unblock("#m_blockui_list");
                    }, 2e3));*/
                }
            }).catch(swal.noop);
        }
    }
}

function comboboxSelectEvent(e) {
    var dataItem = e.dataItem;
    if (dataItem === null && dataItem === undefined)
        this.value("");
}

function downloadAction(parameter) {
    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        async: false,
        data: parameter.data,
        url: parameter.url,
        success: function (result) {
            if (result.File !== undefined) {
                var extensions = result.FileName.split('.').pop();
                if (extensions.toLowerCase() == "pdf") {
                    let pdfWindow = window.open("");
                    pdfWindow.document.write("<iframe type='application/pdf' width='100%' height='100%' src='data:application/pdf;base64," + result.File + "' download='" + result.FileName + "'></iframe>");
                }
                else {
                    kendo.saveAs({
                        dataURI: "data:text/plain;base64," + result.File,
                        fileName: result.FileName
                    });
                }
            }
        }
    });
}

function dataBoundEvent(e) {
    var rows = e.sender.tbody.children();
    $(rows).each(function () {
        var rowLabel = $(this).find(".row-number");
        var pageSize = (e.sender.dataSource.pageSize() * (e.sender.dataSource.page() - 1));
        if (isNaN(pageSize)) { pageSize = 0; }
        var index = $(this).index() + 1 + pageSize;
        $(rowLabel).html(index);
    });
}

function dataBoundBgColorEvent(e, params) {
    // iterate the table rows and apply custom row and cell styling 
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        var rowLabel = $(rows[j]).find(".row-number");
        var pageSize = (e.sender.dataSource.pageSize() * (e.sender.dataSource.page() - 1));
        if (isNaN(pageSize)) { pageSize = 0; }
        var index = $(rows[j]).index() + 1 + pageSize;
        $(rowLabel).html(index);

        var dataItem = e.sender.dataItem(row);
        var status = null;

        $.each(params.split('.'), function (i, val) {
            if (status === null)
                status = dataItem.get(val);
            else
                status = status.get(val);
        });

        //Claer Background color grid row
        row.removeClass("currentApprove");

        if (status !== null) {
            if (status.toString().toLowerCase() === "1") {
                row.addClass("currentApprove");
            }
        }
    }
}

function yesNo(element) {
    element.kendoDropDownList({
        dataSource: [{ Text: "Yes", Value: "Yes" }, { Text: "No", Value: "No" }],
        dataTextField: "Text",
        dataValueField: "Value",
        optionLabel: "--Select Value--"
    });
}

function addTreeListRow(elem) {
    try {
        var treelist = $(elem).closest("div[data-role='treelist']").data("kendoTreeList");
        var item = treelist.dataItem($(elem).closest("tr"));
        treelist.addRow(item);
    }
    catch (err) {
        swal('Error', err.message, 'error');
    }
}

function editTreeListRow(elem) {
    try {
        var treelist = $(elem).closest("div[data-role='treelist']").data("kendoTreeList");
        var item = treelist.dataItem($(elem).closest("tr"));
        treelist.editRow(item);
    }
    catch (err) {
        swal('Error', err.message, 'error');
    }
}

function deleteTreeListRow(elem) {
    var treelist = $(elem).closest("div[data-role='treelist']").data("kendoTreeList");
    var item = treelist.dataItem($(elem).closest("tr"));
    treelist.removeRow($(elem).closest("tr"));
    //bootbox.confirm({
    //    title: 'Deletion Confirmation',
    //    message: 'Are you sure want to delete this data?',
    //    size: 'small',
    //    buttons: {
    //        cancel: {
    //            label: '<i class="fa fa-times"></i> Cancel'
    //        },
    //        confirm: {
    //            label: '<i class="fa fa-check"></i> Confirm'
    //        }
    //    },
    //    callback: function (result) {
    //        if (result) {
    //            treelist.removeRow($(elem).closest("tr"));
    //        }
    //    }
    //});
}


FormData.prototype.appendRecursive = function (data, wrapper) {
    for (var x in data) {
        if (typeof data[x] == 'object' || data[x].constructor === Array) {
            this.appendRecursive(data[x], wrapper + '[' + x + ']');
        } else {
            this.append(wrapper + '[' + x + ']', data[x]);
        }
    }
}

var exportFlag = true;
function excelExportEvent(e) {
    var index = null;
    for (var i = 0; i < e.sender.columns.length; i++) {
        if (e.sender.columns[i].title === "Action") { // columns[i].title -- You can also use title property here but for this you have to assign title for all columns
            index = i;
        }
    }

    var grid = e.sender;
    var fields = grid.dataSource.options.fields;
    var fieldsModels = grid.dataSource.options.schema.model.fields;
    var columns = grid.columns;
    var dateCells = [];

    for (var i = 0; i < fields.length; i++) {
        if (index !== i) {
            var currentField = fields[i].field;
            var currentModel = fieldsModels[currentField];
            if (currentModel !== undefined) {
                if (currentModel.type === "date") {
                    dateCells.push(i - 1);
                }
            }
        }
    }

    //========Start Menampilkan data Client Template =============
    var data = e.data;
    var gridColumns = e.sender.columns;
    var sheet = e.workbook.sheets[0];
    var visibleGridColumns = [];
    var columnTemplates = [];
    // Create element to generate templates in.
    var elem = document.createElement('div');

    // Get a list of visible columns
    for (var i = 0; i < gridColumns.length; i++) {
        if (!gridColumns[i].hidden) {
            visibleGridColumns.push(gridColumns[i]);
        }
    }

    // Create a collection of the column templates, together with the current column index
    for (var i = 0; i < visibleGridColumns.length; i++) {
        if (visibleGridColumns[i].template) {
            columnTemplates.push({ cellIndex: i, template: kendo.template(visibleGridColumns[i].template) });
        }
    }
     
    ////Auto Width
    //sheet.columns.forEach(function (column) {
    //    delete column.width;
    //    column.autoWidth = true;
    //});
    var indexcellNumber = null;
    for (var rowIndex = 0; rowIndex < sheet.rows.length; rowIndex++) {
        var row = sheet.rows[rowIndex];

        if (row.type === "footer") {
            //replace BR to New Line excel
            for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
                var cell = row.cells[cellIndex];
                if (cell.value && cell.value.toString().replace(' ', '').indexOf("<br/>") >= 0) {
                    cell.value = cell.value.replace(/<br\s*\/?>/mg, "\n");
                    cell.wrap = true;
                }
            }
        }

        //Header
        if (row.type === "header") {
            row.height = 30;
            var borderColor = "#" + (206).toString(16) + (206).toString(16) + (210).toString(16);
            for (var i = 0; i < row.cells.length; i++) {
                row.cells[i].background = "#60b5ff";//"#" + (245).toString(16) + (245).toString(16) + (246).toString(16); //"#0070C0";
                row.cells[i].color = "#FFFFFF";//"black";
                row.cells[i].bold = true;
                row.cells[i].hAlign = "center";
                row.cells[i].borderTop = { color: borderColor, size: 5 };
                row.cells[i].borderBottom = { color: borderColor, size: 5 };
                row.cells[i].borderLeft = { color: borderColor, size: 5 };
                row.cells[i].borderRight = { color: borderColor, size: 5 };
                if (row.cells[i].value === "#") { indexcellNumber = i; }
            }
        }

        //Detail
        if (row.type === "data") {

             // Get the data item corresponding to the current row.
            var dataItem = data[i - 1];
            if(dataItem != null){
                for (var j = 0; j < columnTemplates.length; j++) {
                    var columnTemplate = columnTemplates[j];
                    // Generate the template content for the current cell.
                    elem.innerHTML = columnTemplate.template(dataItem);
                    if (row.cells[columnTemplate.cellIndex] != undefined) {
                        var value = "";
                        if (elem.textContent != null) {
                            value = elem.textContent.replace(/^\s+|\s+$/g, '');//remove line break/enter di depan ataupun di bekalang
                        } else if (elem.innerText != null) {
                            value = elem.innerText.replace(/^\s+|\s+$/g, '');//remove line break/enter di depan ataupun di bekalang
                        }

                        // Output the text content of the templated cell into the exported cell.
                        row.cells[columnTemplate.cellIndex-indexcellNumber].value = value;
                    }
                }
            }

            if (indexcellNumber !== null) { row.cells[indexcellNumber].value = rowIndex; }
            if (rowIndex % 2 === 0) {
                for (var i = 0; i < row.cells.length; i++) {
                    row.cells[i].background = "#" + (246).toString(16) + (247).toString(16) + (250).toString(16);//"#DCE6F1";
                }
            }

            //Set posisi text
            //sheet.rows[i].cells.reverse();
            for (var i = 0; i < row.cells.length; i++) {
                //sheet.rows[i].cells[ci].hAlign = "right";
                row.cells[i].vAlign = "center";
                row.cells[i].borderTop = { color: borderColor, size: 5 };
                row.cells[i].borderBottom = { color: borderColor, size: 5 };
                row.cells[i].borderLeft = { color: borderColor, size: 5 };
                row.cells[i].borderRight = { color: borderColor, size: 5 };
                row.cells[i].wrap = true;
                row.cells[i].autoWidth = true;
            }

            //--------------Start Setting Date Format -------------
            if (rowIndex >= 1) {//header di exlude
                for (var q = 0; q < dateCells.length; q++) {
                    var cellIndex = dateCells[q];
                    var value = row.cells[cellIndex].value;
                    if (value !== null) {
                        //row.cells[cellIndex].value = value;
                        row.cells[cellIndex].format = "dd-MMM-yyyy";
                    }
                }
            }
            //--------------End Setting Date Format -------------
        }


        //var colTemplates = [];  //Initialize new array
        //var data = e.data;//this.dataSource.view();

        ////-----------------------------Start Set Value Client Template --------------------------------------------
        //for (var i = 0; i < e.sender.columns.length; i++) {
        //    if (index !== i) {
        //        if (e.sender.columns[i].template) {
        //            colTemplates.push(kendo.template(e.sender.columns[i].template)); //Add kendo template to colTemplate array if it exists for the given index's column
        //        } else {
        //            colTemplates.push(null); //Add null to given index if template does not exist for the column index. This ensures columns are mapped to the proper index.
        //        }
        //    }
        //}

        ////colTemplate array has been built now: Example of array at this point)     colTemplate: { templateCol[0], null, templateCol[3], null, null  }
        //for (var i = 0; i < colTemplates.length; i++) {
        //    for (var j = 0; j < data.length; j++) {
        //        var col = colTemplates[i];
        //        if (col !== null) {
        //            var value = col(data[j]);
        //            if (value !== undefined) {
        //                var cell = sheet.rows[j + 1].cells[i];
        //                cell.value = value; //Loop through all colTemplates and all data in the grid to build the excel sheet. Skip null colTemplates.
        //            }
        //        }
        //    }
        //}
        ////-----------------------------End Set Value Client Template --------------------------------------------
    }

    if (index !== null) {
        if (exportFlag) {
            e.sender.hideColumn(index);
            e.preventDefault();
            exportFlag = false;
            e.sender.saveAsExcel();
        } else {
            e.sender.showColumn(index);
            //e.sender.table.css("width", "100%");
            exportFlag = true;
        }
    }
}

function pdfExportEvent(e) {
    var index = null;
    for (var i = 0; i < e.sender.columns.length; i++) {
        if (e.sender.columns[i].title === "Action") { // columns[i].title -- You can also use title property here but for this you have to assign title for all columns
            index = i;
        }
    }

    if (index !== null) {
        if (exportFlag) {
            e.sender.hideColumn(index);
            e.preventDefault();
            exportFlag = false;

            e.sender.saveAsPDF().then(function () {
                e.sender.showColumn(index);
                exportFlag = true;
            });
        }
    }
}

function GetBodyExportExcel(e) {
    var grid = e.sender;
    var fields = grid.dataSource.options.fields;
    var schema = grid.dataSource.options.schema;
    var model = schema.model;
    if (model != undefined) {
        model = model.fields;
    }
    var fieldsModels = model;
    var columns = grid.columns;
    var dateCells = [];

    if (fieldsModels != undefined){
        for (var i = 0; i < fields.length; i++) {
            var currentField = fields[i].field;
            var currentModel = fieldsModels[currentField];
            if (currentModel !== undefined) {
                if (currentModel.type === "date") {
                    dateCells.push(i - 1);
                }
            }
        }
    }

    //========Start Menampilkan data Client Template =============
    var data = e.data;
    var gridColumns = e.sender.columns;
    var sheet = e.workbook.sheets[0];
    var visibleGridColumns = [];
    var columnTemplates = [];
    // Create element to generate templates in.
    var elem = document.createElement('div');

    // Get a list of visible columns
    for (var i = 0; i < gridColumns.length; i++) {
        if (!gridColumns[i].hidden) {
            visibleGridColumns.push(gridColumns[i]);
        }
    }

    // Create a collection of the column templates, together with the current column index
    for (var i = 0; i < visibleGridColumns.length; i++) {
        if (visibleGridColumns[i].template) {
            columnTemplates.push({ cellIndex: i, template: kendo.template(visibleGridColumns[i].template) });
        }
    }

    // Traverse all exported rows.
    for (var i = 1; i < sheet.rows.length; i++) {
        var row = sheet.rows[i];
        // Traverse the column templates and apply them for each row at the stored column position.

        // Get the data item corresponding to the current row.
        var dataItem = data[i - 1];
        for (var j = 0; j < columnTemplates.length; j++) {
            var columnTemplate = columnTemplates[j];
            // Generate the template content for the current cell.
            elem.innerHTML = columnTemplate.template(dataItem);
            if (row.cells[columnTemplate.cellIndex] != undefined) {
                var value = "";
                if (elem.textContent != null) {
                    value = elem.textContent.replace(/^\s+|\s+$/g, '');//remove line break/enter di depan ataupun di bekalang
                } else if (elem.innerText != null) {
                    value = elem.innerText.replace(/^\s+|\s+$/g, '');//remove line break/enter di depan ataupun di bekalang
                }

                // Output the text content of the templated cell into the exported cell.
                row.cells[columnTemplate.cellIndex].value = value;
            }
        }
    }

    //===============End Menampilkan data Client Template================ 
    var indexcellNumber = null;
    for (var rowIndex = 0; rowIndex < sheet.rows.length; rowIndex++) {
        var row = sheet.rows[rowIndex];

        if (row.type === "footer") {
            //replace BR to New Line excel
            for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
                var cell = row.cells[cellIndex];
                if (cell.value && cell.value.toString().replace(' ', '').indexOf("<br/>") >= 0) {
                    cell.value = cell.value.replace(/<br\s*\/?>/mg, "\n");
                    cell.wrap = true;
                }
            }
        }

        //Header
        if (row.type === "header") {
            row.height = 30;
            var borderColor = "#" + (206).toString(16) + (206).toString(16) + (210).toString(16);
            for (var i = 0; i < row.cells.length; i++) {
                row.cells[i].background = "#60b5ff";//"#" + (245).toString(16) + (245).toString(16) + (246).toString(16); //"#0070C0";
                row.cells[i].color = "#FFFFFF";//"black";
                row.cells[i].bold = true;
                row.cells[i].hAlign = "center";
                row.cells[i].borderTop = { color: borderColor, size: 5 };
                row.cells[i].borderBottom = { color: borderColor, size: 5 };
                row.cells[i].borderLeft = { color: borderColor, size: 5 };
                row.cells[i].borderRight = { color: borderColor, size: 5 };
                if (row.cells[i].value === "#") { indexcellNumber = i; }
            }
        }

        //Detail
        if (row.type === "data") {
            if (indexcellNumber !== null) { row.cells[indexcellNumber].value = rowIndex; }
            if (rowIndex % 2 === 0) {
                for (var i = 0; i < row.cells.length; i++) {
                    row.cells[i].background = "#" + (246).toString(16) + (247).toString(16) + (250).toString(16);//"#DCE6F1";
                }
            }

            //Set posisi text
            //sheet.rows[i].cells.reverse();
            for (var i = 0; i < row.cells.length; i++) {
                //sheet.rows[i].cells[ci].hAlign = "right";
                row.cells[i].vAlign = "center";
                row.cells[i].borderTop = { color: borderColor, size: 5 };
                row.cells[i].borderBottom = { color: borderColor, size: 5 };
                row.cells[i].borderLeft = { color: borderColor, size: 5 };
                row.cells[i].borderRight = { color: borderColor, size: 5 };
                row.cells[i].wrap = true;
                row.cells[i].autoWidth = true;
            }

            //--------------Start Setting Date Format -------------
            if (rowIndex >= 1) {//header di exlude
                for (var q = 0; q < dateCells.length; q++) {
                    var cellIndex = dateCells[q];
                    var value = row.cells[cellIndex].value;
                    if (value !== null) {
                        //row.cells[cellIndex].value = value;
                        row.cells[cellIndex].format = "dd-MMM-yyyy";
                    }
                }
            }
            //--------------End Setting Date Format -------------
        }
    }
    return sheet;
}