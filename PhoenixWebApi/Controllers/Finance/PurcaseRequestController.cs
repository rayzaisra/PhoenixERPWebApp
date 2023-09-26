using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PurchaseRequestController : ControllerBase
    {
        readonly IPurchaseRequestService service;
        public PurchaseRequestController(IPurchaseRequestService service)
        {
            this.service = service;
        }

        [HttpGet("GetPRList/{status?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPRList(string status)
        {
            var result = await service.GetPRList(status);
            return Ok(new ApiResponse(result));
        }
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetail/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetail(string Id)
        {
            var result = await service.GetDetail(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetBudgetByPRId/{hrisDivisionId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetByPRId(string hrisDivisionId)
        {
            var result = await service.GetBudgetByPRId(hrisDivisionId);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(PurchaseRequest)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<PurchaseRequest>(collection[nameof(PurchaseRequest)].ToString());
            if (collection.Files.Count > 0)
            {
                model.file = collection.Files[0];
            }
            else
            {
                model.file = null;
            }

            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(PurchaseRequest)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<PurchaseRequest>(collection[nameof(PurchaseRequest)].ToString());
            if (collection.Files.Count > 0)
            {
                model.file = collection.Files[0];
            }
            else
            {
                model.file = null;
            }

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]PurchaseRequest model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]PurchaseRequest model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }

        [HttpDelete("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await service.Get(id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        [HttpGet("GetBudgetMasterById/{budgetMasterId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBudgetMasterById(string budgetMasterId)
        {
            var result = await service.GetBudgetMasterById(budgetMasterId);
            return Ok(new ApiResponse(result));
        }
    }
}