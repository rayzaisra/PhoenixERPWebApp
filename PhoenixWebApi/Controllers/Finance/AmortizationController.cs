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

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// Amortization API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AmortizationController : Controller
    {
        readonly IAmortizationService service;

        /// <summary>
        /// And endpoint to manage Amortization
        /// </summary>
        /// <param name="context">Database context</param>
        public AmortizationController(IAmortizationService service)
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
        public async Task<IActionResult> Post([FromBody]Amortization model)
        {
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
        public async Task<IActionResult> Put([FromBody]Amortization model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromBody]Amortization model)
        {
            await service.CreateJournal(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("EditJournal")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditJournal([FromBody]Amortization model)
        {
            await service.EditJournal(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
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
        [HttpPost("GetList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetList([FromBody]Amortization model)
        {
            var result = await service.GetList(model);
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

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("DetailJournalList/{costSharingAllocationId}/{condition}/{amount}/{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailJournal(string costSharingAllocationId, string condition, decimal amount, string id)
        {
            var result = await service.GetDetailJournal(costSharingAllocationId, condition, amount, id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("JournalList/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournal(string Id)
        {
            var result = await service.GetJournal(Id);
            return Ok(new ApiResponse(result));
        }
    }
}