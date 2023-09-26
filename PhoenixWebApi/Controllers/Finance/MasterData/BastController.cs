using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Finance.MasterData
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BastController : ControllerBase
    {
        readonly IBastService service;

        public BastController(IBastService service)
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

        [HttpGet("getBASTLog")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> getBASTLog(string Id)
        {
            var result = await service.getLog(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDDLServiceReceiptDetailByPO/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDDLServiceReceiptDetailByPO(string poId)
        {
            var result = await service.GetDDLServiceReceiptDetailByPO(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDDLGoodReceiptDetailByPO/{poId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDDLGoodReceiptDetailByPO(string poId)
        {
            var result = await service.GetDDLGoodReceiptDetailByPO(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetGRDetailData/{grdId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetGRDetailData(string grdId)
        {
            var result = await service.GetGRDetailData(grdId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetSRDetailData/{srdId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetSRDetailData(string srdId)
        {
            var result = await service.GetSRDetailData(srdId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getDetail/{bastId}/{type}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetListDetail(string bastId, string type)
        {
            var result = await service.GetListDetail(bastId, type);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]BastDTO model)
        {
            var Header = new Bast();
            Header = model.Header;
            var Details = new List<BastDetail>();
            Details = model.Details;

            await service.AddWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]BastDTO model)
        {
            var Header = new Bast();
            Header = model.Header;
            var Details = new List<BastDetail>();
            Details = model.Details;

            await service.EditWithDetailAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpPut("closed")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Closed([FromBody]BastDTO model)
        {
            var Header = new Bast();
            Header = model.Header;
            var Details = new List<BastDetail>();
            Details = model.Details;

            await service.EditWithDetailClosedAsync(Header, Details);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [HttpPut("rejected")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Rejected([FromBody]Bast model)
        {
            await service.Rejected(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
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