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

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// LegalEntity API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/legalentity")]
    public class LegalEntityController : Controller
    {
        readonly ILegalEntityService service;

        /// <summary>
        /// And endpoint to manage LegalEntity
        /// </summary>
        /// <param name="context">Database context</param>
        public LegalEntityController(ILegalEntityService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(LegalEntity)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<LegalEntity>(collection[nameof(LegalEntity)].ToString());

            if (collection.Files.Count() == 2)
            {
                model.fileImageTaxScan = collection.Files[0];
                model.fileImageLegalScan = collection.Files[1];
            }

            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(LegalEntity)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<LegalEntity>(collection[nameof(LegalEntity)].ToString());

            if (collection.Files.Count() == 2)
            {
                model.fileImageTaxScan = collection.Files[0];
                model.fileImageLegalScan = collection.Files[1];
            }

            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
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
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }
    }
}