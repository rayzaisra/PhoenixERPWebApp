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
    public class InventoryController : ControllerBase
    {
        readonly IInventoryService service;
        public InventoryController(IInventoryService service)
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
            var result = await service.GetAsync(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetail/{invId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetail(string invId)
        {
            var result = await service.GetDetail(invId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetailForGoodRequest/{invId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailForGoodRequest(string invId)
        {
            var result = await service.GetDetailForGoodRequest(invId);
            return Ok(new ApiResponse(result));
        }

        

        [HttpGet("getInvAll/{InvId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInvAll(string InvId)
        {
            var result = await service.GetInvAll(InvId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDataInventoryStockCard/{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDataInventoryStockCard(string id)
        {
            var result = await service.GetDataInventoryStockCard(id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetailByGoodReceipt/{grId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailByGoodReceipt(string grId)
        {
            var result = await service.GetDetailByGoodReceipt(grId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetailForInventoryStockCard/{invid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailForInventoryStockCard(string invid)
        {
            var result = await service.GetDetailForInventoryStockCard(invid);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InventoryDTO)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InventoryDTO>(collection[nameof(InventoryDTO)].ToString());

            if (collection.Files.Count() == 1)
            {
                model.Header.file = collection.Files[0];
            }

            var Header = new Inventory();
            Header = model.Header;
            var Details = new List<InventoryDetail>();
            Details = model.InventoryDetails;

            await service.AddWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection, [FromForm]IFormCollection collection2)
        {

            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InventoryDTO)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InventoryDTO>(collection[nameof(InventoryDTO)].ToString());

            if (collection.Files.Count() == 1)
            {
                model.Header.file = collection.Files[0];
            }

            var Header = new Inventory();
            Header = model.Header;
            var Details = new List<InventoryDetail>();
            Details = model.InventoryDetails;

            await service.EditWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpPost("savedetail")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> PostDetail([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InventoryDetail)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InventoryDetail>(collection[nameof(InventoryDetail)].ToString());

            if (collection.Files.Count() == 1)
            {
                model.FileUpload = collection.Files[0];
            }

            await service.AddDetail(model);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut("updatedetail")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> PutDetail([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(InventoryDetail)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<InventoryDetail>(collection[nameof(InventoryDetail)].ToString());

            if (collection.Files.Count() == 1)
            {
                model.FileUpload = collection.Files[0];
            }

            await service.EditDetail(model);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.GetAsync(Id);
            await service.DeleteAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }


    }
}