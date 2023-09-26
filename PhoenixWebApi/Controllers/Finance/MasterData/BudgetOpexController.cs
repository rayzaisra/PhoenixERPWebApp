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
using Phoenix.Data.Models.Enum;
using Phoenix.Data.RestApi;

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// BudgetMasterHeader API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/budgetopex")]
    public class BudgetOpexController : Controller
    {
        readonly IBudgetMasterService service;

        /// <summary>
        /// And endpoint to manage BudgetMasterHeader
        /// </summary>
        /// <param name="context">Database context</param>
        public BudgetOpexController(IBudgetMasterService service)
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
        public async Task<IActionResult> Post([FromBody]BudgetMasterHeader model)
        {
            model.Source = BudgetCodeType.OPEX;
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
        public async Task<IActionResult> Put([FromBody]BudgetMasterHeader model)
        {
            model.Source = BudgetCodeType.OPEX;
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]BudgetMasterHeader model)
        {
            model.Source = BudgetCodeType.OPEX;
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]BudgetMasterHeader model)
        {
            model.Source = BudgetCodeType.OPEX;
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
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("Detail/{masterId}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> DeleteDetail(string masterId)
        {
            await service.DeleteDetailSoftAsync(masterId);
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
            var result = await service.GetList(BudgetCodeType.OPEX);
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
        /// <param name="budgetCodeId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetMasterBudgetCode/{budgetCodeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterBudgetCode(string budgetCodeId)
        {
            var result = await service.GetMasterBudgetCode(budgetCodeId, BudgetCodeType.OPEX);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBudgetMaster/{budgetMasterHeaderId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetMaster(string budgetMasterHeaderId)
        {
            var result = await service.GetBudgetMaster(BudgetCodeType.OPEX, budgetMasterHeaderId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBudgetMasterDetails/{costSharingAllocationId}/{budgetMasterId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetMasterDetails(string costSharingAllocationId, string budgetMasterId)
        {
            var result = await service.GetBudgetMasterDetails(BudgetCodeType.OPEX, costSharingAllocationId, budgetMasterId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBudgetMasterById/{masterBudgetCodeId}/{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetMasterById(string masterBudgetCodeId, string id)
        {
            var result = await service.GetBudgetMasterById(masterBudgetCodeId, id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("IsRequestor/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public IActionResult IsRequestor(string Id)
        {
            var result = service.IsRequestor(BudgetCodeType.OPEX, Id);
            return Ok(new ApiResponse(result));
        }
    }
}