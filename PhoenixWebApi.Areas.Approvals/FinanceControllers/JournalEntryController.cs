using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using CodeMarvel.Infrastructure.ModelShared;
using Newtonsoft.Json;
using BusinessLogic.Finance.Approvals;
using CommonTool.DataTables;
using CommonTool.QueryBuilder.Model;

namespace PhoenixWebApi.Areas.Approvals.Controllers
{
    [Route("api/approval/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Approval-Finance")]
    [ApiController]
    public class JournalEntryController : ApiBaseController
    {
        [HttpPost]
        [Route("Requested/listdatatables")]
        public async Task<IActionResult> ListDataTablesBinder([FromForm] DataTables.AspNet.Core.IDataTablesRequest dtRequest)
        {
            //var dd = JsonConvert.DeserializeObject<JsonRule>(dtRequest.Get<string>("queryBuilder"));
            //appLogger.Debug("JSON DATATABLE ");
            //appLogger.Info(JsonConvert.SerializeObject(dtRequest));
            var Service = new JournalEntryApprovalService(AppUserData);

            var listResult = await Task.Run<DataTablesResponseNetCore>(() =>
            {
                return Service.GetRequested(dtRequest, OptionalQueryBuilder: dtRequest.Get<string>("queryBuilder"));
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }
    }
}
