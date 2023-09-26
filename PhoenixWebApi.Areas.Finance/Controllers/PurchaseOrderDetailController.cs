using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using DataAccess.PhoenixERP.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.Dto.Custom.Finance;
using CommonTool.JEasyUI.DataGrid;
using BusinessLogic.Finance;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public partial class PurchaseOrderDetailController : ApiBaseController
    {

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var service = new PurchaseOrderDetail(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                try
                {
                    service.Delete(Ids, msg => result.Status.Message = msg);
                    result.Status.Success = true;
                }
                catch (Exception ex)
                {
                    result.Status.Success = false;
                    result.Status.Message = ex.Message;
                }

                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

        [HttpPost]
        [Route("dgtable")]
        public async Task<IActionResult> DataGridTable([FromForm] DgRequest dgRequest, [FromQuery(Name = "purchaseorderid")] String PurchaseOrderId)
        {
            var Service = new PurchaseOrderDetail(AppUserData);
            var listResult = await Task.Run<DgResponse>(() =>
            {
                return Service.GetDataGridEasyUI(dgRequest, PurchaseOrderId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> KendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest, [FromQuery(Name = "purchaseorderid")]String OrderId)
        {
            var Service = new PurchaseOrderDetail(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest, OrderId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("KendoGridFromGoodsReceipt")]
        public async Task<IActionResult> KendoGridFromGoodsReceipt(CommonTool.KendoUI.Grid.DataRequest dtRequest, [FromQuery(Name = "purchaseorderid")]String OrderId)
        {
            var Service = new PurchaseOrderDetail(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGridFromGoodReceipt(dtRequest, OrderId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }
    }
}
