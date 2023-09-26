using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance.Transaction;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetDepreciationController : ControllerBase
    {
        readonly IFixedAssetDepreciationService service;

        public FixedAssetDepreciationController(IFixedAssetDepreciationService service)
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

        [HttpGet("GetByYearDetail/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByYearDetail(string Id)
        {
            var result = await service.GetByYearDetail(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetByMonthDetail/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetByMonthDetail(string Id)
        {
            var result = await service.GetByMonthDetail(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetOpeningBalance/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetOpeningBalance(string Id)
        {
            var result = await service.GetOpeningBalance(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]FixedAssetDepreciation model)
        {
            await service.AddEditAsync(model, false);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]FixedAssetDepreciation model)
        {
            await service.AddEditAsync(model, true);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        [HttpPost("PostJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> PostJournal([FromBody]FixedAssetDepreciationJournal model)
        {
            await service.PostJournalAsync(model);
            return Ok(new ApiResponse(true, Messages.SucessSave));
        }

        [HttpGet("GetPostingDate/{legalEntityId}")]
        [ActionRole(ActionMethod.Read)]
        public IActionResult GetPostingDate(string legalEntityId)
        {
            var result = service.GetPostingDate(legalEntityId);
            return Ok(new ApiResponse(result));
        }
    }
}