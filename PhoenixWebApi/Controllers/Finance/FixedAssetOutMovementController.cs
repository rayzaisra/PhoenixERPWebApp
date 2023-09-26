using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetOutMovementController : ControllerBase
    {
        readonly IFixedAssetOutMovementService service;

        public FixedAssetOutMovementController(IFixedAssetOutMovementService service)
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

        [HttpGet("GetListFixedAssetOutDetail/{headerId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListFixedAssetOutDetail(string headerId)
        {
            var result = await service.GetListFixedAssetOutDetail(headerId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getlogoutmovement/{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLogOutMovement(string id)
        {
            var result = await service.GetLogOutMovement(id);
            return Ok(new ApiResponse(result));
        }

        

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]FixedAssetOutMovementDto model)
        {
            var Header = new FixedAssetOutMovement();
            Header = model.Header;
            var Details = new List<FixedAssetOutMovementDetail>();
            Details = model.Details;

            await service.AddWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]FixedAssetOutMovementDto model)
        {
            var Header = new FixedAssetOutMovement();
            Header = model.Header;
            var Details = new List<FixedAssetOutMovementDetail>();
            Details = model.Details;

            await service.EditWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpPut("ApproveCustom")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> ApproveCustom([FromBody]FixedAssetOutMovementDto model)
        {
            await service.ApproveCustom(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        [HttpPut("RejectData")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> RejectData([FromBody]FixedAssetOutMovement model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
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