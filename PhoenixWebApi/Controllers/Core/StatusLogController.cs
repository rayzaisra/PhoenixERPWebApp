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
    /// StatusLog API Endpoint.
    /// </summary>
    [AllowAnonymousRole]
    [Produces("application/json")]
    [Route("api/statuslog")]
    public class StatusLogController : Controller
    {
        readonly IStatusLogService service;

        /// <summary>
        /// And endpoint to manage StatusLog
        /// </summary>
        /// <param name="context">Database context</param>
        public StatusLogController(IStatusLogService service)
        {
            this.service = service;
        }
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("{id?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.Get(id);
            return Ok(new ApiResponse(result));
        }
    }
}