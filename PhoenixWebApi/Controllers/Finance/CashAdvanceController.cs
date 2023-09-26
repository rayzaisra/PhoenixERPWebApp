using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;
using Phoenix.Data.Services.Um;
using PhoenixWebApi.Services;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CashAdvanceController : ControllerBase
    {
        readonly ICashAdvanceService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        /// <summary>
        /// And endpoint to manage CashAdvance
        /// </summary>
        /// <param name="context">Database context</param>
        public CashAdvanceController(ICashAdvanceService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
        {
            this.service = service;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody] CashAdvance model)
        {
            await service.AddEditAsync(model, false);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));

        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody] CashAdvance model)
        {
            await service.AddEditAsync(model, true);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Approve existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody] CashAdvance model)
        {
            await service.Approve(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        /// <summary>
        /// Reject existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody] CashAdvance model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
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
        [HttpGet("{legalEntityId}/{mainServiceCategoryId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string legalEntityId, string mainServiceCategoryId)
        {
            var result = await service.Get(legalEntityId, mainServiceCategoryId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetails/{caId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetails(string caId)
        {
            var result = await service.GetDetails(caId);
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
         
        [HttpGet("GetCompanyInfo/{companyid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetCompanyInfo(string companyid)
        {
            var result = await service.GetCompanyInfo(companyid);
            return Ok(new ApiResponse(result));
        }
    }
}