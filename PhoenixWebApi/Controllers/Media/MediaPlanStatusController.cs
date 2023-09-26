using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Media.MasterData.MediaPlanStatus;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Media.MasterData.MediaPlanStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Media
{
    [Produces("application/json")]
    [Route("api/mediaplanstatus")]
    public class MediaPlanStatusController : Controller
    {
        readonly IMediaPlanStatusService service;

        /// <summary>
        /// And endpoint to manage MasterMappingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public MediaPlanStatusController(IMediaPlanStatusService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TmMediaPlanStatus model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]TmMediaPlanStatus model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{Id}")]
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
        public async Task<IActionResult> Get()
        {
            var result = await service.GetList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }
    }
}
