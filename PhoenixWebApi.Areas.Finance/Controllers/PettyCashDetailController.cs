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
    public partial class PettyCashDetailController : ApiBaseController
    {

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] Ids)
        {
            var service = new PettyCashDetail(AppUserData);
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
        [Route("save")]
        public async Task<IActionResult> Save([FromBody] petty_cash_detailDTO Data)
        {
            var svc = new PettyCashDetail(AppUserData);

            var saveResult = await Task.Run<ApiResponse>(() =>
            {
                bool isNew = false;
                var result = new ApiResponse();

                try
                {
                    result.Status.Success = svc.SaveEntity(Data, ref isNew);

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
                            recordID = Data.id
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

        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> KendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest, [FromQuery(Name = "pettycashid")]String pettycashid)
        {
            var Service = new PettyCashDetail(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest, pettycashid);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpPost]
        [Route("dgtable")]
        public async Task<IActionResult> DataGridTable([FromForm] DgRequest dgRequest, [FromQuery(Name = "pettycashid")] String PettyCashId)
        {
            var Service = new PurchaseOrderDetail(AppUserData);
            var listResult = await Task.Run<DgResponse>(() =>
            {
                return Service.GetDataGridEasyUI(dgRequest, PettyCashId);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }
    }
}
