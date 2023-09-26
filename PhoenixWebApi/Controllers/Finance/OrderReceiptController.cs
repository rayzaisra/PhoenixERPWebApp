using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderReceiptController : ControllerBase
    {
        readonly IOrderReceiptService service;

        public OrderReceiptController(IOrderReceiptService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }
                
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }
        
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(OrderReceipt)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<OrderReceipt>(collection[nameof(OrderReceipt)].ToString());

            if (collection.Files.Count() > 0)
            {
                if (collection.Files[0] != null)
                    model.fileImage = collection.Files[0];
                if (collection.Files[1] != null)
                    model.fileDoc = collection.Files[1];
            }

            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
        
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(OrderReceipt)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<OrderReceipt>(collection[nameof(OrderReceipt)].ToString());

            if (collection.Files.Count() > 0)
            {   
                if(collection.Files[0]!=null)
                    model.fileImage = collection.Files[0];
                if (collection.Files[1] != null)
                    model.fileDoc = collection.Files[1];
            }

            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }
        
        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }
    }
}