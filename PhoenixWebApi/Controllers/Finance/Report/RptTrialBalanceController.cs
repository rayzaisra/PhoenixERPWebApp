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
    [Route("api/rpttrialbalance")]
    public class RptTrialBalanceController : ControllerBase
    {
        readonly IRptTrialBalanceService service;
        public RptTrialBalanceController(IRptTrialBalanceService service)
        {
            this.service = service;
        }

        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetData([FromBody] RptTrialBalanceFilter Filter)
        {
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }

        //[HttpPost("GetTotal")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetTotal([FromBody] RptTrialBalanceFilter Filter)
        //{
        //    var result = await service.GetTotal(Filter);
        //    return Ok(new ApiResponse(result));
        //}
    }
}