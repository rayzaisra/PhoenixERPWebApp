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
    [Route("api/rptbillingandrevenue")]
    public class RptBillingAndRevenueController : Controller
    {
        readonly IRptBillingAndRevenueService service;

        /// <summary>
        /// And endpoint to manage Reportpnl
        /// </summary>
        /// <param name="context">Database context</param>
        public RptBillingAndRevenueController(IRptBillingAndRevenueService service)
        {
            this.service = service;
        }

        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetData([FromBody] RptBillingAndRevenueFilter Filter)
        {
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetJob")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJob([FromBody] RptBillingAndRevenueFilter Filter)
        {
            var result = await service.GetJob(Filter);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetSummary")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetSummary([FromBody] RptBillingAndRevenueFilter Filter)
        {
            var result = await service.GetSummary(Filter);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetBU")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBU([FromBody] RptBillingAndRevenueFilter Filter)
        {
            var result = await service.GetBU(Filter);
            return Ok(new ApiResponse(result));
        }
    }
}