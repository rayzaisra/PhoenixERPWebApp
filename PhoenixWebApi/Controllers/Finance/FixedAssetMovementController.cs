using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetMovementController : ControllerBase
    {
        readonly IFixedAssetMovementService service;

        public FixedAssetMovementController(IFixedAssetMovementService service)
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


        //[HttpGet("GetData")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetData()
        //{
        //    var result = await service.GetData();
        //    return Ok(new ApiResponse(result));
        //}

        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }
        [HttpGet("GetData/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetData(string Id)
        {
            var result = await service.GetData(Id);
            return Ok(new ApiResponse(result));
        }


        [HttpGet("getDetail/{headerId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetail(string headerId)
        {
            var result = await service.GetListDetail(headerId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetListFixedAssetmovementDetail/{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListFixedAssetmovementDetail(string id)
        {
            var result = await service.GetListFixedAssetmovementDetail(id);
            return Ok(new ApiResponse(result));
        }
        

        [HttpGet("getByFixedAsset/{faId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByFixedAsset(string faId)
        {
            var result = await service.GetByFixedAsset(faId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetListFixedAssetOutDetail/{headerId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListFixedAssetOutDetail(string headerId)
        {
            var result = await service.GetListFixedAssetOutDetail(headerId);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]FixedAssetMovementDto model)
        {
            var Header = new FixedAssetMovement();
            Header = model.Header;
            var Details = new List<FixedAssetMovementDetail>();
            Details = model.Details;

            await service.AddWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut("ApproveCustom")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> ApproveCustom([FromBody]FixedAssetMovementDto model)
        {
            await service.ApproveCustom(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        [HttpPut("RejectData")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> RejectData([FromBody]FixedAssetMovement model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
        }


        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]FixedAssetMovementDto model)
        {
            var Header = new FixedAssetMovement();
            Header = model.Header;
            var Details = new List<FixedAssetMovementDetail>();
            Details = model.Details;

            await service.EditWithDetailAsync(Header, Details);

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