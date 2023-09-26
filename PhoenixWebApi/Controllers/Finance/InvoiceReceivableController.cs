using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InvoiceReceivableController : ControllerBase
    {
        readonly IInvoiceReceivableService service;
        //readonly ITemporaryAccountPayableService serviceTAP;
        public InvoiceReceivableController(IInvoiceReceivableService service)//, ITemporaryAccountPayableService serviceTAP)
        {
            this.service = service;
            //this.serviceTAP = serviceTAP;
        }

        [HttpGet("GetHistoryPayment/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetHistoryPayment(string poId)
        {
            var result = await service.GetHistoryPayment(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetList/{status}/{businessUnit}/{legalEntity}/{invoiceNumber}/{invoiceVendor}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetList(string status, string businessUnit, string legalEntity, string invoiceNumber, string invoiceVendor)
        {
            var result = await service.GetList(status, businessUnit, legalEntity, invoiceNumber, invoiceVendor);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("GetAPList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetAPList([FromBody] FilterInvoiceReceivable search)
        {
            var result = await service.GetAPList(search);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetailInvoiceList/{poId}/{paymentTo}/{invId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailInvoiceList(string poId, string paymentTo, string invId)
        {
            var result = await service.GetDetailInvoiceList(poId, paymentTo, invId);
            return Ok(new ApiResponse(result));
        }


        [HttpGet("GetPoById/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPoById(string poId)
        {
            var result = await service.GetPoById(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetJournal/{invId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournal(string invId)
        {
            var result = await service.GetJournal(invId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm] IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceReceivable)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceReceivable>(collection[nameof(InvoiceReceivable)].ToString());

            model.filePoObject = collection.Files.GetFile("files_0");
            model.fileTaxObject = collection.Files.GetFile("files_1");
            model.fileContractObject = collection.Files.GetFile("files_2");
            model.fileDoObject = collection.Files.GetFile("files_3");
            model.fileGrObject = collection.Files.GetFile("files_4");
            model.fileInvObject = collection.Files.GetFile("files_5");

            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm] IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceReceivable)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceReceivable>(collection[nameof(InvoiceReceivable)].ToString());

            model.filePoObject = collection.Files.GetFile("files_0");
            model.fileTaxObject = collection.Files.GetFile("files_1");
            model.fileContractObject = collection.Files.GetFile("files_2");
            model.fileDoObject = collection.Files.GetFile("files_3");
            model.fileGrObject = collection.Files.GetFile("files_4");
            model.fileInvObject = collection.Files.GetFile("files_5");

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("CreateJournal")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> CreateJournal([FromForm] IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceReceivable)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceReceivable>(collection[nameof(InvoiceReceivable)].ToString());

            model.filePoObject = collection.Files.GetFile("files_0");
            model.fileTaxObject = collection.Files.GetFile("files_1");
            model.fileContractObject = collection.Files.GetFile("files_2");
            model.fileDoObject = collection.Files.GetFile("files_3");
            model.fileGrObject = collection.Files.GetFile("files_4");
            model.fileInvObject = collection.Files.GetFile("files_5");

            await service.CreateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("CreateJournalOther")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournalOther([FromBody] JournalTemp model)
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
            await service.UpdateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }
    }
}