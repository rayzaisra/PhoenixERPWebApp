using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data;
using Phoenix.Data.Models;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/accountpayableaging")]
    public class AccountPayableAgingController : ControllerBase
    {
        readonly IAccountPayableAgingService service;
        public AccountPayableAgingController(IAccountPayableAgingService service)
        {
            this.service = service;
        }

        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get([FromBody] VwAgingApFilter Filter)
        {
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{reffNo}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string reffNo)
        {
            var result = await service.Get(reffNo);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("JournalList/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournal(string Id)
        {
            var result = await service.GetJournal(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody] JournalWriteOffAp model)
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
        public async Task<IActionResult> Put([FromBody] JournalWriteOffAp model)
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
        public async Task<IActionResult> CreateJournal([FromBody] JournalWriteOffAp model)
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
        public async Task<IActionResult> EditJournal([FromBody] JournalWriteOffAp model)
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
            await service.DeleteSoftAsync(Id);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }
    }
}