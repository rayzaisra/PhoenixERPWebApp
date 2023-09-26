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
using Phoenix.Data.Services.Um;
using PhoenixWebApi.Services;

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// Promotion API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/promotion")]
    public class PromotionController : Controller
    {
        readonly IPromotionService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;

        /// <summary>
        /// And endpoint to manage Promotion
        /// </summary>
        /// <param name="context">Database context</param>
        public PromotionController(IPromotionService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
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
        public async Task<IActionResult> Post([FromBody]Promotion model)
        {
            await service.AddAsync(model);
            //await Notification(model.Id, model.Status);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]Promotion model)
        {
            await service.EditAsync(model);
            //await Notification(model.Id, model.Status);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }
        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]Promotion model)
        {
            await service.ApproveAsync(model);
           // await Notification(model.Id, model.Status);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]Promotion model)
        {
            await service.ApproveAsync(model);
           // await Notification(model.Id, model.Status);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }

        private async Task Notification(string id, int? status)
        {
            if (status != StatusTransaction.Draft)
            {
                var listUserApproval = await trUserApproval.GetAppIdFromReferenceId(id);
                foreach (var data in listUserApproval)
                {
                    await Task.Run(() => notify.AddNotification(NotificationTypeCode.PromotionRequestApproval, data.AppIdUser, NotificationModuleType.ModuleHRIS));
                }
            }
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
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("approvalList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetApprovalList()
        {
            var result = await service.GetApprovalList();
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
        /// Gets single record based on specific primary key EmployeeId.
        /// </summary>
        /// <param name="EmployeeId">Key EmployeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("additional/{employeeId}/{promotionId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetAdditionalList(string employeeId, string promotionId)
        {
            var result = await service.GetAdditionalList(employeeId, promotionId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key EmployeeId.
        /// </summary>
        /// <param name="EmployeeId">Key EmployeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("deducation/{employeeId}/{promotionId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDeductionList(string employeeId, string promotionId)
        {
            var result = await service.GetDeductionList(employeeId, promotionId);
            return Ok(new ApiResponse(result));
        }
    }
}