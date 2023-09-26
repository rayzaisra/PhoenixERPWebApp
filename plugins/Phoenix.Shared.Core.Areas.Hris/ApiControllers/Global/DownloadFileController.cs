using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;


namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// Termination API Endpoint.
    /// </summary>
    [AllowAnonymousRole]
    [Produces("application/json")]
    [Route("api/downloadFile")]
    public class DownloadFileController : Controller
    {
        readonly IFileService service;

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public DownloadFileController(IFileService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> Download(string id)
        {
            var result = await service.Download(id);
            return Ok(new ApiResponse(result));
        }
    }
}
