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
    /// ExitClearance API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/exitchecklist")]
    public class ExitChecklistController : Controller
    {
        readonly IExitChecklistService service;

        /// <summary>
        /// And endpoint to manage ExitClearance
        /// </summary>
        /// <param name="context">Database context</param>
        public ExitChecklistController(IExitChecklistService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]Termination model)
        {
            //model.Status = TerminationStatus.ExitClearance;
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody] Termination model)
        {
            //model.Status = TerminationStatus.ExitClearance;
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
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
        /// <param name="terminationId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{terminationId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string terminationId)
        {
            var result = await service.Get(terminationId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getJobHandoverList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJobHandoverList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetJobHandoverList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getOfficeManagementList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetOfficeManagementList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetOfficeManagementList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getInformationTechnologyList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInformationTechnologyList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetInformationTechnologyList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getFinanceList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFinanceList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetFinanceList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getHumanResourceList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetHumanResourceList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetHumanResourceList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getOtherList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetOtherList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetOtherList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="terminationId">Key id parameter.</param>
        /// <param name="employeeBasicInfoId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getFinalHandoverList/{terminationId}/{employeeBasicInfoId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFinalHandoverList(string terminationId, string employeeBasicInfoId)
        {
            var result = await service.GetFinalHandoverList(terminationId, employeeBasicInfoId);
            return Ok(new ApiResponse(result));
        }
    }
}