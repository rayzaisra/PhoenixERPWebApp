
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
    public class InvoiceClientController : ControllerBase
    {
        readonly IInvoiceClientService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        public InvoiceClientController(IInvoiceClientService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
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

        [HttpGet("{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.Get(id);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("GetCompanyInfo/{companyid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetCompanyInfo(string companyid)
        {
            var result = await service.GetCompanyInfo(companyid);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("GetInfoBank/{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInfoBank(string id)
        {
            var result = await service.GetInfoBank(id);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("GetAffiliation/{divisiid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetAffiliation(string divisiid)
        {
            var result = await service.GetAffiliation(divisiid);
            return Ok(new ApiResponse(result, true));
        }


        [HttpGet("GetPCEDetail/{pceno}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPCEDetail(string pceno)
        {
            var result = await service.GetPCEDetail(pceno);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("GetInvoiceDetail/{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInvoiceDetail(string id)
        {
            var result = await service.GetInvoiceDetail(id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("ReadHistoryInvoice/{pceId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> ReadInvHistory(string pceId)
        {
            var result = await service.ReadHistoryInvoice(pceId);
            return Ok(new ApiResponse(result));
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
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
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

        [HttpGet("GetPceList/{clientid}/{legalid}/{divisionid}/{pceId}/{jobIdName}/{currencyId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPceList(string clientid, string legalid, string divisionid, string pceId, string jobIdName, string currencyId)
        {
            var result = await service.GetPceList(clientid, legalid, divisionid, pceId, jobIdName, currencyId);
            return Ok(new ApiResponse(result, true));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]InvoiceClient model)
        {
            await service.Approve(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        //[HttpPut("ForceClose")]
        //[ActionRole(ActionMethod.Edit)]
        //public async Task<IActionResult> ForceClose([FromBody]InvoiceClient model)
        //{ 
        //        await service.ForceClose(model);
        //        return Ok(new ApiResponse(model, true, Messages.SuccessApprove)); 
        //}

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]InvoiceClient model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
        }

        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromForm] IFormCollection collection)
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
    }
}
