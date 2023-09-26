using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Services.Finance.Report;
using Phoenix.Data.RestApi;
using Phoenix.Data.Models.Finance;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetGridReportController : ControllerBase
    {
        private readonly ITImeSheetReportServices _iTImeSheetReportServices;
        public TimeSheetGridReportController(ITImeSheetReportServices tImeSheetReportServices)
        {
            this._iTImeSheetReportServices = tImeSheetReportServices;
        }

        [HttpGet("get_by_job")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string job_id, string user_id, string client_id, DateTime startDate, DateTime endDate, string legalEntityId, string divisionId, string isNonJob)
        {
            var result = await _iTImeSheetReportServices.TimeSheetByJob(job_id, user_id, client_id, startDate, endDate, legalEntityId, divisionId, isNonJob);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(result));
        }

        [HttpGet("get_by_client")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByClient(string client_id)
        {
            var result = await _iTImeSheetReportServices.GetReportClient(client_id);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(result));
        }

        [HttpGet("get_by_business_unit")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByBusinessUnit(string jobId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId)
        {
            var result = await _iTImeSheetReportServices.GetReportClientByBusinessUnit(jobId, clientId, startDate, endDate, legalEntityId);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(result));
        }

        [HttpGet("GetListTimeSheetByBusinessUnit")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await _iTImeSheetReportServices.GetListTimeSheetByBusinessUnit();
            return Ok(new Phoenix.Data.RestApi.ApiResponse(result));
        }

        [HttpDelete("DeleteReportTimeSheetByBusinessUnit/{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await _iTImeSheetReportServices.GetListTimeSheetByBusinessUnitById(Id);
            await _iTImeSheetReportServices.DeleteReportTimeSheetByBusinessUnit(model);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(true, Messages.SuccessDelete));

        }

        [HttpGet("ReportTimeSheetByBusinessUnit/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListTimeSheetByBusinessUnitById(string Id)
        {
            var result = await _iTImeSheetReportServices.GetListTimeSheetByBusinessUnitById(Id);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody] TimeSheetReportByBusinessUnit model)
        {
            await _iTImeSheetReportServices.AddAsync(model);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(model, true, Messages.SuccessAdd));

        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody] TimeSheetReportByBusinessUnit model)
        {
            await _iTImeSheetReportServices.EditAsync(model);
            return Ok(new Phoenix.Data.RestApi.ApiResponse(model, true, Messages.SuccessEdit));

        }
    }
}