using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        readonly IPettyCashService service;

        /// <summary>
        /// And endpoint to manage PettyCash
        /// </summary>
        /// <param name="context">Database context</param>
        public PettyCashController(IPettyCashService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="Id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetById/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await service.GetById(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="Id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetDetails/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDetails(string Id)
        {
            var result = await service.GetDetails(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="Id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetMasterPettyCash/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterPettyCash(string Id)
        {
            var result = await service.GetMasterPettyCash(Id);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="Id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetMasterPettyCashById/{masterPettyCashId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterPettyCashById(string masterPettyCashId)
        {
            var result = await service.GetMasterPettyCashById(masterPettyCashId);
            return Ok(new ApiResponse(result));
        }
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="Id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("JournalList/{Id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournal(string Id)
        {
            var result = await service.GetJournal(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(PettyCash)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<PettyCash>(collection[nameof(PettyCash)].ToString());
            if (collection.Files.Count() == 1)
            {
                model.File = collection.Files[0];
            }

            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(PettyCash)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<PettyCash>(collection[nameof(PettyCash)].ToString());
            if (collection.Files.Count() == 1)
            {
                model.File = collection.Files[0];
            }

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("CreateJournal")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CreateJournal([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(PettyCash)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<PettyCash>(collection[nameof(PettyCash)].ToString());
            if (collection.Files.Count() == 1)
            {
                model.File = collection.Files[0];
            }

            await service.CreateJournal(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> ApproveData([FromBody]PettyCash model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
        }

        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> RejectData([FromBody]PettyCash model)
        {
            await service.Reject(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessReject));
        }

        [HttpGet]
        [AllowAnonymousRole]
        [Route("IsHolder")]
        public IActionResult IsHolder()
        {
            return Ok(service.IsHolder());
        }

        /// <summary>
        /// Gets all Detail records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetInfoPceByJobId/{jobid?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInfoPceByJobId(string jobid)
        {
            var result = await service.GetInfoPceByJobId(jobid);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all Detail records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetPcaTasks/{pcaId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetPcaTasks(string pcaId)
        {
            var result = await service.GetPcaTasks(pcaId);
            return Ok(new ApiResponse(result));
        }
    }
}