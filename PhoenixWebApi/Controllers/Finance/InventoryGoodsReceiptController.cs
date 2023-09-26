using DataAccess.Dto.Custom.Finance;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryGoodsReceiptController : ControllerBase
    {
        readonly IInventoryGoodsReceiptService service;

        public InventoryGoodsReceiptController(IInventoryGoodsReceiptService service)
        {
            this.service = service;
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]InventoryGoodsReceipt model)
        {
            var result = await service.AddEdit(model, false);

            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpPost("GeneratedInvoiceGoodReceipt")]
        public async Task<IActionResult> GeneratedInvoiceGoodReceipt([FromBody]InventoryGoodsReceipt model)
        {
            var result = await service.Generated(model);

            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]InventoryGoodsReceipt model)
        {
            await service.AddEdit(model, true);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
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

        [HttpGet("getDetail/{poId}/{grId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetail(string poId, string grId)
        {
            var result = await service.GetDetail(poId, grId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetailHistory/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailHistory(string poId)
        {
            var result = await service.GetDetailHistory(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getInventoryDetail/{grId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInventoryDetail(string grId)
        {
            var result = await service.GetInventoryDetail(grId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getFixedAsset/{grId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFixedAsset(string grId)
        {
            var result = await service.GetFixedAsset(grId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getPO/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPO(string poId)
        {
            var result = await service.GetPOById(poId);
            return Ok(new ApiResponse(result));
        }


        [HttpPost("UpdateInventoryDetail")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> UpdateInventoryDetail([FromBody]InventoryDetail model)
        {
            var result = await service.UpdateInventoryDetail(model);

            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }
    }
}
