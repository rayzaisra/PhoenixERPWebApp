using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        readonly IPurchaseOrderService service;
        public PurchaseOrderController(IPurchaseOrderService service)
        {
            this.service = service;
        }

        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetPOList/{status?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPOList(string status)
        {
            var result = await service.GetPOList(status);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetailPOList/{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetailPOList(string id)
        {
            var result = await service.GetDetailPOList(id);
            return Ok(new ApiResponse(result));
        }


        [HttpGet("GetByVendor/{vendorId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByVendor(string vendorId)
        {
            var result = await service.GetByVendor(vendorId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]PurchaseOrder model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]PurchaseOrder model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]PurchaseOrder model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]PurchaseOrder model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpGet("GetEmployeeData/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetEmployeeData(string Id)
        {
            var result = await service.GetDataEmployee(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("print/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Print(string Id)
        {
            var result = await service.Print(Id);
            return Ok(new ApiResponse(result));
        }
    }
}