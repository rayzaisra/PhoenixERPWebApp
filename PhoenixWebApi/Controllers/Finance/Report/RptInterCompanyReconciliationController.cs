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
    [Route("api/rptintercompanyreconciliation")]
    public class RptInterCompanyReconciliationController : ControllerBase
    {
        readonly IRptInterCompanyReconciliationService service;
        public RptInterCompanyReconciliationController(IRptInterCompanyReconciliationService service)
        {
            this.service = service;
        }

        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetData([FromBody] RptInterCompanyReconciliationFilter Filter)
        {
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }
    }
}