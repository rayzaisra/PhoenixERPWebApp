(function ($) {
    'use strict';
    var pageFunction = (function (pageFunction) {
        pageFunction.init = function () {
            //initialize
            var id = $("#Id").val();
            if (id !== null) {
                var mediaPlanNo = $("#MediaPlanNo").data("kendoDropDownList");
                $("#SelectedMediaPlanNo").val(mediaPlanNo.text());
                mediaPlanNo.readonly();
            }
        };
        pageFunction.save = function () {
            var details = $("#grid").data("kendoGrid").dataSource.data().toJSON();
            var list = [];
            for (var i = 0; i < details.length; i++) {
                var z = JSON.parse(JSON.stringify(details[i]));
                list.push(z);
            }

            //var data = { TrMediaOrderTVDetails: list };

            callActionWithDetails({ formId: "form", title: "save", type: "POST", url: param.saveUrl, objects: list });
            
        };
        pageFunction.getData = function () {
            var id = $("#MediaPlanNo").data("kendoDropDownList").value();
            var mediaPlanNo = $("#MediaPlanNo").data("kendoDropDownList").text();
            mApp.block("#m_blockui_list", {
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });
            $.ajax({
                type: 'GET',
                url: param.getPlanUrl,
                data: {
                    Id: id
                },
                dataType: 'json',
                success: function (result) {
                    console.log(result);
                    //Set Header
                    $('#TrMediaPlanTvId').val(result.Id);
                    $('#TrMediaFileMasterId').val(result.TrMediaFileMasterId);
                    $('#EmployeeBasicInfoId').val(result.EmployeeBasicInfoId);
                    $('#Version').val(result.Version);
                    $('#Duration').val(result.Duration);
                    $('#TargetAudience').val(result.TargetAudience);
                    $('#PeriodCampaignStart').val(result.PeriodCampaignStart);
                    $('#PeriodCampaignEnd').val(result.PeriodCampaignEnd);
                    $('#AgencyServiceFee').val(result.AgencyServiceFee);
                    $('#AgencyServiceFeePercent').val(result.AgencyServiceFeePercent);
                    $('#TaxBased').val(result.TaxBased);

                    var ClientDDL = $("#ClientId").data("kendoDropDownList");
                    ClientDDL.value(result.ClientId);
                    ClientDDL.refresh();

                    var VendorDDL = $("#BrandId").data("kendoDropDownList");
                    VendorDDL.value(result.BrandId);
                    VendorDDL.refresh();

                    $("#RevisionNo").val(result.RevisionNo);

                    //Set Detail
                    var dataSource = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: param.getPlanDetailUrl,
                                data: {
                                    Id: id
                                }
                            }
                        },
                        pageSize: 10
                    });

                    $("#SelectedMediaPlanNo").val(mediaPlanNo);
                    var grid = $('#grid').data('kendoGrid');
                    grid.setDataSource(dataSource);
                    mApp.unblock("#m_blockui_list");
                },
                error: function (e, t, s) {
                    //console.log(e)
                    console.log(e);
                    mApp.unblock("#m_blockui_list");
                }
            }).then(setTimeout(function () {
                mApp.unblock("#m_blockui_list");
            }, 2e3));
           
            
        };
        pageFunction.prorate = function () {
            var bulkNumber = $("#BulkNumber").data("kendoNumericTextBox").value();
            var grid = $("#grid").data("kendoGrid");
            var data = grid.dataSource.data();
            var total = 0;
            for (var i = 0; i < data.length; i++) {
                //access record using for loop - here i is value from loop
                var firstItem = $('#grid').data().kendoGrid.dataSource.data()[i];

                //set col reason value value
                firstItem["TotalNetCost"] = bulkNumber;
                total += bulkNumber;

                //refresh the grid
                grid.refresh();
            }  
            $('#TotalNetCost').data('kendoNumericTextBox').value(total);
        };
        pageFunction.isTotal = function () {
            if (this.checked) {
                var bulkNumber = $("#BulkNumber").data("kendoNumericTextBox").value();
                var grid = $("#grid").data("kendoGrid");
                var data = grid.dataSource.data();
                for (var i = 0; i < data.length; i++) {
                    //access record using for loop - here i is value from loop
                    var firstItem = $('#grid').data().kendoGrid.dataSource.data()[i];

                    //set col reason value value
                    firstItem["TotalNetCost"] = 0;

                    //refresh the grid
                    grid.refresh();
                }
                $('#TotalNetCost').data('kendoNumericTextBox').value(bulkNumber);
                $('#btn-prorate').enable(false);
            } else {
                $('#TotalNetCost').data('kendoNumericTextBox').value(0);
                $('#btn-prorate').enable(true);
            }
        };

        pageFunction.process = function () {
            var grossValue = 0;
            var grossPaid = 0;
            var nett = 0;
            var vat = 0;
            var grandTotal = 0;
            var grid = $("#grid").data("kendoGrid");
            var data = grid.dataSource.data();
            for (var i = 0; i < data.length; i++) {
                //access record using for loop - here i is value from loop
                var firstItem = $('#grid').data().kendoGrid.dataSource.data()[i];

                nett += firstItem["TotalNetCost"];
                grossValue += firstItem["TotalGrossValue"];
                vat += firstItem["TotalGrossValue"] * (10/100);
            }
            grossPaid = grossValue + vat;
            $('#TotalNetCost').data('kendoNumericTextBox').value(nett);
            $('#Vat').data('kendoNumericTextBox').value(vat);
            $('#TotalGrossValue').data('kendoNumericTextBox').value(grossValue);
            $('#GrandTotal').data('kendoNumericTextBox').value(grossPaid);
        };

        pageFunction.submit = function () {
            callAction({ formId: "form", title: "submit", type: "POST", url: param.submitUrl });
        };

        pageFunction.arrange = function () {
            window.location.href = param.arrangeUrl;
        };

        pageFunction.schedule = function (e) {

            var rowElement = this;
            var row = $(rowElement);
            var grid = $("#grid").getKendoGrid();
            var item = grid.dataItem(row);
            console.log(item);

            $.ajax({
                type: 'GET',
                url: param.indexScheduleUrl,
                data: {
                    Id: item.Id
                },
                success: function (result) {
                    console.log(result);
                    $('#scheduleModal').html(result);

                    $('#scheduleModal').modal('show');
                    mApp.unblock("#m_blockui_list");
                },
                error: function (e, t, s) {
                    //console.log(e)
                    console.log(e);
                    mApp.unblock("#m_blockui_list");
                }
            }).then(setTimeout(function () {
                mApp.unblock("#m_blockui_list");
            }, 2e3));
        };
        
        return pageFunction;
    })(pageFunction || {});

    $(document).ready(function () {
        pageFunction.init();
        $('#btn-save').click(pageFunction.save);
        $('#btn-getdata').click(pageFunction.getData);
        $('#btn-prorate').click(pageFunction.prorate);
        $('#IsTotal').click(pageFunction.isTotal);
        $('#btn-process').click(pageFunction.process);
        $('#btn-submit').click(pageFunction.submit);
        $('#btn-arrange').click(pageFunction.arrange);
        $("#grid tbody").on("dblclick", "tr", pageFuntion.schedule);
    });
}(jQuery));