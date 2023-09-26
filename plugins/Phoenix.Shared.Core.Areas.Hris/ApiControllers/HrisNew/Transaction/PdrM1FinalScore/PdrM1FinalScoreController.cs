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
    /// PdrApprailsalM1Header API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/pdrm1finalscore")]
    public class PdrM1FinalScoreController : Controller
    {
        readonly IPdrM1FinalService service;

        /// <summary>
        /// And endpoint to manage PdrApprailsalM1Header
        /// </summary>
        /// <param name="context">Database context</param>
        public PdrM1FinalScoreController(IPdrM1FinalService service)
        {
            this.service = service;
        }


        [HttpGet("getPdrApprailsalM1List/{employeeId}/{startReview?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPdrApprailsalM1List(string employeeId, DateTime? startReview)
        {
            var result = await service.GetPdrApprailsalM1List(employeeId, startReview);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getPdrPerformancelM1List/{employeeId}/{startReview?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPdrPerformanceM1List(string employeeId, DateTime? startReview)
        {
            var result = await service.GetDataPdrPerformance(employeeId, startReview);
            return Ok(new ApiResponse(result));
        }
    }
}
