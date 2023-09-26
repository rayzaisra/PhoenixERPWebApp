using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Services.Finance;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using DataAccess.PhoenixERP.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.Dto.Custom.Finance;
using CommonTool.JEasyUI.DataGrid;
using CommonTool.KendoUI.Grid;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class InventoryGoodsRequestController: ApiBaseController
    {
       
        [HttpGet]
        [Route("{Id}/details")]
        public async Task<IActionResult> Details(string Id)
        {
            var svc = new InventoryGoodsReceipt(AppUserData);

            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = svc.Details(Id);
                try
                {
                    if (record != null)
                    {
                        result.Status.Success = true;
                        result.Data = record;
                    }
                    else
                    {
                        result.Status.Message = "Record not found";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("saveasdraft")]
        public async Task<IActionResult> SaveEntity([FromBody] InventoryGoodsReceiptDetails Data)
        {
            var svc = new InventoryGoodsReceipt(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.SaveInventoryGoodsReceipt(Data, ref isNew);

                    if (result.Status.Success)
                    {
                        if (isNew)
                        {
                            result.Status.Message = "The data has been added.";
                        }
                        else
                        {
                            result.Status.Message = "The data has been updated.";
                        }

                        result.Data = new
                        {
                            recordID = Data.good_receipt.id
                        };
                    }
                    else
                    {
                        result.Status.Message = "Record is not saved !";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }

                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var svc = new InventoryGoodsReceipt(AppUserData);
            
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();

                svc.Delete(Ids, msg => result.Status.Message = msg);
                result.Status.Success = true;

                return result;
            });

            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> GetDataKendoGridBinder([FromBody] CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var svc = new InventoryGoodsReceipt(AppUserData);

            appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(dtRequest));
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return svc.GetListKendoUIGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }
    }
}
