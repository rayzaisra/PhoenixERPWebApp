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
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;
using Phoenix.Data.Services.Um;
using PhoenixWebApi.Services;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [Route("api/[controller]")]
    public class PaymentRequestController : ControllerBase
    {
        readonly IPaymentRequestService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        readonly DataContext context;
        public PaymentRequestController(IPaymentRequestService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval, DataContext context)
        {
            this.service = service;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
            this.context = context;
        }

        [HttpPost("GetPaymentRequestList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPaymentRequestList([FromBody] FilterPaymentRequest search)
        {
            var result = await service.GetPaymentRequestList(search);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetListDetailAP/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetailAP(string Id)
        {
            var result = await service.GetListDetailAP(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetListDetailPC/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetailPC(string Id)
        {
            var result = await service.GetListDetailPC(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetListDetailSPR/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetailSPR(string Id)
        {
            var result = await service.GetListDetailSPR(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBankPaymentJournal/{BankPaymentId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBankPaymentJournal(string BankPaymentId)
        {
            var result = await service.GetBankPaymentJournal(BankPaymentId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddWithDetail([FromBody] PaymentRequest model)
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
        public async Task<IActionResult> EditWithDetail([FromBody] PaymentRequest model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPut("Reconsiliation")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reconsiliation([FromBody] PaymentRequest model)
        {
            await service.Reconsiliation(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody] PaymentRequest model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody] PaymentRequest model)
        {
            await service.RejectAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromBody] PaymentRequest model)
        {
            await service.CreateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpGet("GetAccountNumberData/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetAccountNumberData(string Id)
        {
            var result = await service.GetAccountNumberData(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetVendorData/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetVendorData(string Id)
        {
            var result = await service.GetVendorData(Id);
            return Ok(new ApiResponse(result));
        }

        //[HttpGet("GetCAByNumber/{caNumber}")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetCAByNumber(string caNumber)
        //{
        //    var result = await service.GetCAByNumber(caNumber);
        //    return Ok(new ApiResponse(result));
        //}

        [HttpGet("GetDetail/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetail(string Id)
        {
            var result = await service.GetDetail(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records list ap with posted satus and filter parameter.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpPost("GetListAPPosted")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListAPPosted([FromBody] PaymentRequestFilter Filter)
        {
            var result = await service.GetListAPPosted(Filter);
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
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("UpdateJournal")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> UpdateJournal([FromBody] Journal model)
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