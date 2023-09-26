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
    [Produces("application/json")]
    [Route("api/hcprocesstermination")]
    public class HCProcessTerminationController : Controller
    {
        readonly IHCProcessTerminationService service;

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public HCProcessTerminationController(IHCProcessTerminationService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(Termination)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<Termination>(collection[nameof(Termination)].ToString());
            if (collection.Files.Count > 0)
            {
                model.file = collection.Files[0];
            }
            else
            {
                model.file = null;
            }
           

            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.GetById(id);
            return Ok(new ApiResponse(result));
        }
    }
}