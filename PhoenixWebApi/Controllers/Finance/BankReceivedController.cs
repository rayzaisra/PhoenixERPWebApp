using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;
using Phoenix.Data.Services.Um;
using PhoenixWebApi;
using PhoenixWebApi.Services;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{

    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [Route("api/[controller]")]
    public class BankReceivedController : ControllerBase
    {
        readonly IBankReceivedService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        readonly DataContext context;
        public BankReceivedController(IBankReceivedService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval, DataContext context)
        {
            this.service = service;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
            this.context = context;
        }

        [HttpPost("GetBankReceivedList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPaymentRequestList([FromBody] FilterBankReceived search)
        {

            var result = await service.GetBankReceivedList(search);
            return Ok(new ApiResponse(result, true, Messages.SuccessSubmit));
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
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetListDetailAR/{bankReceivableId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetailAR(string bankReceivableId)
        {
            var result = await service.GetListDetailAR(bankReceivableId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records list ap with posted satus and filter parameter.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpPost("GetListARPosted")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListAPPosted([FromBody] PaymentRequestFilter Filter)
        {
            var result = await service.GetListARPosted(Filter);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetBankReceivedJournal/{bankReceivableId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBankReceivedJournal(string bankReceivableId)
        {
            var result = await service.GetBankReceivedJournal(bankReceivableId);
            return Ok(new ApiResponse(result));
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


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddAsync([FromBody] BankReceived model)
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
        public async Task<IActionResult> EditAsync([FromBody] BankReceived model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPut("Reconsiliation")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reconsiliation([FromBody] BankReceived model)
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
        public async Task<IActionResult> Approve([FromBody] BankReceived model)
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
        public async Task<IActionResult> Reject([FromBody] BankReceived model)
        {
            await service.RejectAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromBody] BankReceived model)
        {
            await service.CreateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
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
            await service.UpdateJournaAsync(model);
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