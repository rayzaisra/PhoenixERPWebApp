using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GoodRequestController : ControllerBase
    {
        readonly IGoodRequestService service;

        public GoodRequestController(IGoodRequestService service)
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

        [HttpGet("GetMasterConditionProcurement")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterConditionProcurement()
        {
            var result = await service.GetMasterConditionProcurement();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("getByNumber/{Number}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByNumber(string Number)
        {
            var result = await service.GetByNumber(Number);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetail/{headerId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetail(string headerId)
        {
            var result = await service.GetListDetail(headerId);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]GoodRequestDTO model)
        {
            var Header = new GoodRequest();
            Header = model.Header;
            var Details = new List<GoodRequestDetail>();
            Details = model.Details;

            await service.AddWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]GoodRequestDTO model)
        {
            var Header = new GoodRequest();
            Header = model.Header;
            var Details = new List<GoodRequestDetail>();
            Details = model.Details;

            await service.EditWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]GoodRequestDTO model)
        {   
            await service.Approve(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        [HttpPut("RejectData")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> RejectData([FromBody]GoodRequest model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
        }
        //[HttpPut("Complete")]
        //[ActionRole(ActionMethod.Edit)]
        //public async Task<IActionResult> Complete([FromBody]GoodRequest model)
        //{
        //    await service.Complete(model);
        //    return Ok(new ApiResponse(model, true, "Success Complete"));
        //}

        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await service.Get(id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }
        [HttpGet("GetBehalf/{behalf}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetBussiess(string behalf)
        {
            var result = await service.GetBehalf(behalf);
            return Ok(new ApiResponse(result));
        }
        [HttpGet("GetDivLogin/{BasicLogin}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDivLogin(string BasicLogin)
        {
            var result = await service.GetDivLogin(BasicLogin);
            return Ok(new ApiResponse(result));
        }
        
    }
}