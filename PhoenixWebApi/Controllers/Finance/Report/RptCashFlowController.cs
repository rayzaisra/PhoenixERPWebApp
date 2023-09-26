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
    /// <summary>
    /// Reportpnl API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/rptcashflow")]
    public class RptCashFlowController : Controller
    {
        readonly IRptCashFlowService service;

        /// <summary>
        /// And endpoint to manage Reportpnl
        /// </summary>
        /// <param name="context">Database context</param>
        public RptCashFlowController(IRptCashFlowService service)
        {
            this.service = service;
        }
        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetData([FromBody] RptCashFlowFilter Filter)
        {
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }
    }
}