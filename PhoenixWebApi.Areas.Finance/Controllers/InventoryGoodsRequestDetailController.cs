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
using Newtonsoft.Json;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class InventoryGoodsRequestDetailController: ApiBaseController
    {

        [HttpGet]
        [Route("{Id}/listdetails")]
        public async Task<IActionResult> Details(string Id)
        {
            var service = new InventoryGoodsReceiptDetail(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                var record = service.getListDetails(Id);
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
        [Route("KendoGrid")]
        public async Task<IActionResult> GetDataKendoGridBinder([FromBody] CommonTool.KendoUI.Grid.DataRequest dtRequest, [FromQuery(Name = "InventoryGoodsReceiptId")] String InventoryGoodsReceiptId, [FromQuery(Name = "iswindow")] bool isWindow)
        {
            var Service = new InventoryGoodsReceiptDetail(AppUserData);
            appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(dtRequest));
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest, InventoryGoodsReceiptId, isWindow);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var svc = new InventoryGoodsReceiptDetail(AppUserData);
            appLogger.Debug("Delete " + JsonConvert.SerializeObject(Ids));

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
    }
}
