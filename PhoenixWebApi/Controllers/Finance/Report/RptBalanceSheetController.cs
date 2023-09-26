using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/rptbalancesheet")]
    public class BalanceSheetController : ControllerBase
    {
        readonly IRptBalanceSheetService service;
        public BalanceSheetController(IRptBalanceSheetService service)
        {
            this.service = service;
        }

        [HttpPost("GetBalanceSheet")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBalanceSheet([FromBody] RptBalanceSheetFilter Filter)
        {
            var result = await service.GetBalanceSheet(Filter);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetBalanceSheetAffiliation")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBalanceSheetAffiliation([FromBody] RptBalanceSheetAffiliationFilter Filter)
        {
            var result = await service.GetBalanceSheetAffiliation(Filter);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetBalanceSheetComparison")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBalanceSheetComparison([FromBody] RptBalanceSheetFilter Filter)
        {
            var result = await service.GetBalanceSheetComparison(Filter);
            return Ok(new ApiResponse(result));
        }

    }
}