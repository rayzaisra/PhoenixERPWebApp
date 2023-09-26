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
    [Route("api/jobcard")]
    public class JobCardController : Controller
    {
        readonly IJobCardService service;

        /// <summary>
        /// And endpoint to manage JobClosingHeader
        /// </summary>
        /// <param name="context">Database context</param>
        public JobCardController(IJobCardService service)
        {
            this.service = service;
        }

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
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{jobid}")]
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
        [HttpGet("GetReportJobCard/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCard(string jobId)
        {
            var result = await service.GetReportJobCard(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetReportJobCardCOBAp/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCardCOBAp(string jobId)
        {
            var result = await service.GetReportJobCardCOBAp(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetReportJobCardCOBCa/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCardCOBCa(string jobId)
        {
            var result = await service.GetReportJobCardCOBCa(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetReportJobCardCOBAr/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCardCOBAr(string jobId)
        {
            var result = await service.GetReportJobCardCOBAr(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetReportJobCardCOBBr/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCardCOBBr(string jobId)
        {
            var result = await service.GetReportJobCardCOBBr(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetReportJobCardCOBBp/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCardCOBBp(string jobId)
        {
            var result = await service.GetReportJobCardCOBBp(jobId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetReportJobCardRfqCa/{jobid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportJobCardRfqCa(string jobId)
        {
            var result = await service.GetReportJobCardRfqCa(jobId);
            return Ok(new ApiResponse(result));
        }
    }
}