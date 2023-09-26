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
    [Route("api/rptexpensesaffiliation")]
    public class RptExpensesAffiliationController : ControllerBase
    {
        readonly IRptExpensesAffiliationService service;
        public RptExpensesAffiliationController(IRptExpensesAffiliationService service)
        {
            this.service = service;
        }

        [HttpPost("GetExpensesAffiliation")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetExpensesAffiliation([FromBody] RptExpensesAffiliationFilter Filter)
        {
            var result = await service.GetExpensesAffiliation(Filter);
            return Ok(new ApiResponse(result));
        }
    }
}