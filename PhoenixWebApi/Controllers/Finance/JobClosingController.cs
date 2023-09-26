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

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// JobClosingHeader API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/jobclosing")]
    public class JobClosingController : Controller
    {
        readonly IJobClosingService service;

        /// <summary>
        /// And endpoint to manage JobClosingHeader
        /// </summary>
        /// <param name="context">Database context</param>
        public JobClosingController(IJobClosingService service)
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
        public async Task<IActionResult> Post([FromBody]JobClosingHeader model)
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
        public async Task<IActionResult> Put([FromBody]JobClosingHeader model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]JobClosingHeader model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]JobClosingHeader model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }


        ///// <summary>
        ///// Deletes specific record based on given id parameter.
        ///// </summary>
        ///// <param name="id">Key id parameter.</param>
        ///// <returns>Json message if the process is successfully done or failed.</returns>
        //[HttpDelete("{Id}")]
        //[ActionRole(ActionMethod.Delete)]
        //public async Task<IActionResult> Delete(string Id)
        //{
        //    var model = await service.Get(Id);
        //    await service.DeleteSoftAsync(model);
        //    return Ok(new ApiResponse(true, Messages.SuccessDelete));
        //}

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.GetList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("jobclosingaplist/{JobId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetAPList(string jobId)
        {
            var result = await service.GetAPList(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("jobclosingarlist/{JobId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetARList(string jobId)
        {
            var result = await service.GetARList(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{JobId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string JobId)
        {
            var result = await service.Get(JobId);
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
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("JobClosingSummary/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJobClosingSummary(string Id)
        {
            var result = await service.GetJobClosingSummary(Id);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromBody]JobClosingHeader model)
        {
            await service.CreateJournal(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }


        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("UpdateJournal")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> UpdateJournal([FromBody]Journal model)
        {
            await service.UpdateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }


        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("EntryJournalList/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetEntryJournalList(string Id)
        {
            var result = await service.GetEntryJournalList(Id);
            return Ok(new ApiResponse(result));
        }
    }
}