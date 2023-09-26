
using Phoenix.Data.RestApi;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Services.Finance.Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Phoenix.Data.Models.Finance.Transaction;
using System.Linq;
using Phoenix.Data.Attributes;
using PhoenixWebApi.Services;
using Phoenix.Data.Services.Um;
using Phoenix.Data.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceClientMultiPceController : ControllerBase
    {
        readonly IInvoiceClientMultiPceService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        public InvoiceClientMultiPceController(IInvoiceClientMultiPceService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
        {
            this.service = service;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
        }

        [HttpPost("GetInvoiceList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInvoiceList([FromBody] FilterInvoiceClient search)
        {
            var result = await service.GetInvoiceList(search);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetPceInfo/{multipceid?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string multipceid)
        {
            var result = await service.Get(multipceid);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("GetInvoiceDetail/{multipceid?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInvoiceDetail(string multipceid)
        {
            var result = await service.GetInvoiceDetail(multipceid);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("GetPceList/{clientid}/{legalid}/{brandid}/{mainservice}/{currencyid}/{divisionid}/{invoiceId}/{pceTypeCrossBilling?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPceList(string clientid, string legalid, string brandid, string mainservice, string currencyid, string divisionid, string invoiceId, bool? pceTypeCrossBilling)
        {
            var result = await service.GetPceList(clientid, legalid, brandid, mainservice, currencyid, divisionid, invoiceId, pceTypeCrossBilling);
            return Ok(new ApiResponse(result, true));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceClient)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceClient>(collection[nameof(InvoiceClient)].ToString());
            model.VatDocFile = collection.Files.Where(x => x.FileName == model.VatDocFileName).FirstOrDefault();
            model.PoDocFile = collection.Files.Where(x => x.FileName == model.PoDocFileName).FirstOrDefault();

            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }


        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceClient)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceClient>(collection[nameof(InvoiceClient)].ToString());
            model.VatDocFile = collection.Files.Where(x => x.FileName == model.VatDocFileName).FirstOrDefault();
            model.PoDocFile = collection.Files.Where(x => x.FileName == model.PoDocFileName).FirstOrDefault();

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]InvoiceClient model)
        {
            await service.Approve(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]InvoiceClient model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
        }

        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceClient)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceClient>(collection[nameof(InvoiceClient)].ToString());
            model.VatDocFile = collection.Files.Where(x => x.FileName == model.VatDocFileName).FirstOrDefault();
            model.PoDocFile = collection.Files.Where(x => x.FileName == model.PoDocFileName).FirstOrDefault();

            await service.CreateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpGet("GetDataJournal/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDataJournal(string Id)
        {
            var result = await service.GetDataJournal(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetPceTask/{pceid?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPceTask(string pceid)
        {
            var result = await service.GetPceTask(pceid);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("UpdateJournal")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> UpdateJournal([FromBody]Journal model)
        {
            await service.UpdateJournalAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpGet("GetMultipleBrandList/{multipceid?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMultipleBrandList(string multipceid)
        {
            var result = await service.GetMultipleBrandList(multipceid);
            return Ok(new ApiResponse(result));
        }
    }
}
