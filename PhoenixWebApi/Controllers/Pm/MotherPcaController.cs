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
    public class MotherPcaController : Controller
    {
        readonly IMotherPCAService service;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public MotherPcaController(IMotherPCAService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{id}")]
        [AllowAnonymousRole]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.Get(id);
            return Ok(new ApiResponse(result));
        }
    }
}