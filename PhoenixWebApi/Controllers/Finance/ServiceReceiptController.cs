using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance.Transaction.ServiceReceipt;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceReceiptController : ControllerBase
    {
        readonly IServiceReceiptService service;

        public ServiceReceiptController(IServiceReceiptService service)
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

        [HttpGet("getDetail/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetail(string Id)
        {
            var result = await service.GetDetail(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getPO/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPO(string poId)
        {
            var result = await service.GetPO(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getPODetail/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPODetail(string poId)
        {
            var result = await service.GetPODetail(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]ServiceReceiptDTO model)
        {
            var Header = new ServiceReceipt();
            Header = model.Header;
            var Details = new List<ServiceReceiptDetail>();
            Details = model.Details;

            await service.AddWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]ServiceReceiptDTO model)
        {
            var Header = new ServiceReceipt();
            Header = model.Header;
            var Details = new List<ServiceReceiptDetail>();
            Details = model.Details;

            await service.EditWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await service.Get(id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }
    }
}