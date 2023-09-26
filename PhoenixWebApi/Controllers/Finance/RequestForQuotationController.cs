using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RequestForQuotationController : ControllerBase
    {
        readonly IRequestForQuotationService service;
        public RequestForQuotationController(IRequestForQuotationService service)
        {
            this.service = service;
        }

        [HttpGet("GetRFQList/{mainserviceCategoryId}/{status?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetRFQList(string mainserviceCategoryId, string status)
        {
            var result = await service.GetRFQList(mainserviceCategoryId, status);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetTaskList/{RfqId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetTaskList(string RfqId)
        {
            var result = await service.GetTaskList(RfqId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetVendorList/{RfqId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetVendorList(string RfqId)
        {
            var result = await service.GetVendorList(RfqId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetailRFQList/{RfqId}/{VendorId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailRFQList(string RfqId, string VendorId)
        {
            var result = await service.GetDetailRFQList(RfqId, VendorId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetItem/{itemTypeId}/{ItemId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetItem(string itemTypeId, string ItemId)
        {
            var result = await service.GetItem(itemTypeId, ItemId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Vendor")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> PutVendor([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(RequestForQuotation)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<RequestForQuotation>(collection[nameof(RequestForQuotation)].ToString());

            if (collection.Files.Count() == 1)
            {
                model.file = collection.Files[0];
            }

            await service.AddVendorAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SucessSave));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]RequestForQuotation model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]RequestForQuotation model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }
        
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(RequestForQuotation)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<RequestForQuotation>(collection[nameof(RequestForQuotation)].ToString());
            if (collection.Files.Count > 0)
            {
                model.file = collection.Files[0];
            }
            else
            {
                model.file = null;
            }

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(RequestForQuotation)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<RequestForQuotation>(collection[nameof(RequestForQuotation)].ToString());
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

        [HttpPut("task")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> TaskEditAsync([FromBody]RequestForQuotationTask model)
        {
            await service.TaskEditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpDelete("vendor/{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> DeleteVendor(string Id)
        {
            var model = await service.GetVendorById(Id);
            await service.DeleteVendorSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }
    }
}
