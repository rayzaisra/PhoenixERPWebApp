
using Phoenix.Data.RestApi;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Services.Finance.Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountReceivableController : ControllerBase
    {
        readonly IAccountReceiveableService service;
        public AccountReceivableController(IAccountReceiveableService service)
        {
            this.service = service;
        }
         
        [HttpPost("GetARList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetARList([FromBody] FilterInvoiceClient search)
        {
            var result = await service.GetARList(search);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.Get(id);
            return Ok(new ApiResponse(result));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InvoiceClient)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InvoiceClient>(collection[nameof(InvoiceClient)].ToString());
            model.VatDocFile = collection.Files.Where(x => x.FileName == model.VatDocFileName).FirstOrDefault();

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }


        [HttpGet("GetDataJournal/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDataJournal(string Id)
        {
            var result = await service.GetDataJournal(Id);
            return Ok(new ApiResponse(result));
        }
    }
}
