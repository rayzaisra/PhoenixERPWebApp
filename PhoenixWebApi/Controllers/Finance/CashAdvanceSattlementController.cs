using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;
using Phoenix.Data.Services.Um;
using PhoenixWebApi.Services;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CashAdvanceSattlementController : ControllerBase
    {
        readonly ICashAdvanceSattlementService service;
        readonly INotificationCenter notify;
        readonly ITransactionUserApprovalService trUserApproval;
        public CashAdvanceSattlementController(ICashAdvanceSattlementService service, INotificationCenter notify, ITransactionUserApprovalService trUserApproval)
        {
            this.service = service;
            this.notify = notify;
            this.trUserApproval = trUserApproval;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("{legalEntityId}/{mainServiceCategoryId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string legalEntityId, string mainServiceCategoryId)
        {
            var result = await service.Get(legalEntityId, mainServiceCategoryId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{caId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string caId)
        {
            var result = await service.Get(caId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetDetails/{caId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetail(string caId)
        {
            var result = await service.GetDetail(caId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetJournal/{casId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournal(string casId)
        {
            var result = await service.GetJournal(casId);
            return Ok(new ApiResponse(result));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Edit([FromForm]IFormCollection collection)
        {
            if (!collection.Keys.Contains(nameof(CASettlement)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<CASettlement>(collection[nameof(CASettlement)].ToString());

            if (collection.Files.Count() > 0)
            {
                model.receiptFile = collection.Files.GetFile("files_0");
                model.SectionUploadEvidenceFile = collection.Files.GetFile("files_1");
            }

            await service.AddEditAsync(model, true, false);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Add([FromForm]IFormCollection collection)
        {
            if (!collection.Keys.Contains(nameof(CASettlement)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<CASettlement>(collection[nameof(CASettlement)].ToString());

            if (collection.Files.Count() > 0)
            {
                model.receiptFile = collection.Files.GetFile("files_0");
                model.SectionUploadEvidenceFile = collection.Files.GetFile("files_1");
            }

            await service.AddEditAsync(model, false, false);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]CASettlement model)
        {
            await service.Approve(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]CASettlement model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
        }

        //[HttpPost("CreateJournal")]
        //[ActionRole(ActionMethod.Add)]
        //public async Task<IActionResult> CreateJournal([FromForm]IFormCollection collection)
        //{
        //    if (!collection.Keys.Contains(nameof(CASettlement)))
        //    {
        //        return Ok(new ApiResponse(null, false, "bad request"));
        //    }

        //    var model = JsonConvert.DeserializeObject<CASettlement>(collection[nameof(CASettlement)].ToString());

        //    if (collection.Files.Count() > 0)
        //    {
        //      model.receiptFile = collection.Files.GetFile("files_0");
        //      model.SectionUploadEvidenceFile = collection.Files.GetFile("files_1");
        //    }
        //    await service.AddEditAsync(model, false, true);
        //    return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        //}

        //[HttpPut("EditJournal")]
        //[ActionRole(ActionMethod.Add)]
        //public async Task<IActionResult> EditJournal([FromForm]IFormCollection collection)
        //{
        //    if (!collection.Keys.Contains(nameof(CASettlement)))
        //    {
        //        return Ok(new ApiResponse(null, false, "bad request"));
        //    }

        //    var model = JsonConvert.DeserializeObject<CASettlement>(collection[nameof(CASettlement)].ToString());

        //    if (collection.Files.Count() > 0)
        //    {
        //      model.receiptFile = collection.Files.GetFile("files_0");
        //      model.SectionUploadEvidenceFile = collection.Files.GetFile("files_1");
        //    }
        //    await service.AddEditAsync(model, true, true);
        //    return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        //} 
    }
}