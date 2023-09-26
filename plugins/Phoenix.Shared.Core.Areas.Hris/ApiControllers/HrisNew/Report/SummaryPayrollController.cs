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

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// LeaveBalance API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/summarypayroll")]
    public class SummaryPayrollController : Controller
    {
        readonly ISummaryPayrollService service;

        /// <summary>
        /// And endpoint to manage LeaveBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public SummaryPayrollController(ISummaryPayrollService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("{month}/{year}/{legalEntityId?}/{divisionId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> Get(int month, int year, string legalEntityId, string divisionId)
        {
            var result = await service.Get(month, year, legalEntityId, divisionId);
            return Ok(new ApiResponse(result));
        }
    }
}