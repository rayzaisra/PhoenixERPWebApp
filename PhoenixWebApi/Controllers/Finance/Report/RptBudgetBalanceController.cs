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
using Phoenix.Data.Models.Enum;
using Phoenix.Data.RestApi;

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// Reportpnl API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/rptbudgetbalance")]
    public class RptBudgetBalanceController : Controller
    {
        readonly IRptBudgetBalanceService service;

        /// <summary>
        /// And endpoint to manage Reportpnl
        /// </summary>
        /// <param name="context">Database context</param>
        public RptBudgetBalanceController(IRptBudgetBalanceService service)
        {
            this.service = service;
        }

        [HttpPost("GetBudgetOpex")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetOpex([FromBody] RptBudgetBalanceFilter Filter)
        {
            Filter.Source = BudgetCodeType.OPEX;
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetBudgetCapex")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetCapex([FromBody] RptBudgetBalanceFilter Filter)
        {
            Filter.Source = BudgetCodeType.CAPEX;
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }
    }
}