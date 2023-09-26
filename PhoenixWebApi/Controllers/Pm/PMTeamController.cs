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
using PhoenixWebApi;

namespace PhoenixWebApi.Controllers.Pm
{
    /// <summary>
    /// Pca API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/pm/[controller]")]
    public class PMTeamController : Controller
    {
        readonly IPMTeamService service;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public PMTeamController(IPMTeamService service)
        {
            this.service = service;
        }

        /// <summary>
        /// </summary>
        /// <param name="jobId">Key id parameter.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsPMTeam/{JobId}")]
        [AllowAnonymousRole]
        public IActionResult IsPMTeam(string jobId)
        {
            return Ok(service.IsPMTeam(jobId));
        }
    }
}