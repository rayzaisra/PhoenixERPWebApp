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
using BusinessLogic.Finance.Reports;
using CommonTool.JEasyUI.DataGrid;

namespace PhoenixWebApi.Areas.Finance.ReportControllers
{
    [Route("api/report/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance-Report")]
    [ApiController]
    public class GeneralLedgerController: ApiBaseController
    {

        [HttpPost]
        [Route("dgtable")]
        public async Task<IActionResult> DataGridTable([FromForm] DgRequest dgRequest,
            [FromQuery(Name = "dateTime")] DateTime dateTime,
            [FromQuery(Name = "transactionNumber")] String transactionNumber, 
            [FromQuery(Name = "account")] String account
            )
        {
            var Service = new GeneralLedger(AppUserData);
            var listResult = await Task.Run<DgResponse>(() =>
            {
                return Service.GetData(dgRequest, dateTime, transactionNumber, account);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

       
    }
}
