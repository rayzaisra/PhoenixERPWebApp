
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
    public class InvoiceDeliveryNotesController : ControllerBase
    {
        readonly IInvoiceDeliveryNotesService service;
        public InvoiceDeliveryNotesController(IInvoiceDeliveryNotesService service)
        {
            this.service = service;
        }

        [HttpGet("{status?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string status)
        {
            var result = await service.Get(status);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetById/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await service.GetById(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetInvoiceChoiceList/{invoiceid}/{clientid}/{brandid}/{divisionid}/{legalid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInvoiceChoiceList(string invoiceid, string clientid, string brandid, string divisionid, string legalid)
        {
            var result = await service.GetInvoiceChoiceList(invoiceid, clientid, brandid, divisionid, legalid);
            return Ok(new ApiResponse(result, true));
        }

        [HttpGet("ReadDetail/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> ReadDetail(string Id)
        {
            var result = await service.ReadDetail(Id);
            return Ok(new ApiResponse(result, true));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            var model = JsonConvert.DeserializeObject<InvoiceDeliveryNotes>(collection[nameof(InvoiceDeliveryNotes)].ToString());
            if (collection.Files.Count() > 0) { model.DocFile = collection.Files[0]; }

            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            var model = JsonConvert.DeserializeObject<InvoiceDeliveryNotes>(collection[nameof(InvoiceDeliveryNotes)].ToString());
            if (collection.Files.Count() > 0) { model.DocFile = collection.Files[0]; }

            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }
    }
}
