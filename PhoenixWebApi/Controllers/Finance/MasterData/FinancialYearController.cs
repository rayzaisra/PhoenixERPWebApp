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
    /// FinancialYear API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FinancialYearController : Controller
    {
        readonly IFinancialYearService service;

        /// <summary>
        /// And endpoint to manage FinancialYear
        /// </summary>
        /// <param name="context">Database context</param>
        public FinancialYearController(IFinancialYearService service)
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
        public async Task<IActionResult> Post([FromBody]FinancialYear model)
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
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]FinancialYear model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]FinancialYear model)
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
        public async Task<IActionResult> Reject([FromBody]FinancialYear model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
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


        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MaxDate/{legalEntityId}")]
        public IActionResult MaxDate(string legalEntityId)
        {
            return Ok(service.MaxDate(legalEntityId));
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDefaultYearOpeningBalance/{legalEntityId}")]
        public IActionResult GetDefaultYearOpeningBalance(string legalEntityId)
        {
            return Ok(service.GetDefaultYearOpeningBalance(legalEntityId));
        }
    }
}