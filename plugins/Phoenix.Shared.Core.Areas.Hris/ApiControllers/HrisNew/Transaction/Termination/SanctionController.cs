using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// Sanction API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/sanction")]
    public class SanctionController : Controller
    {
        readonly ISanctionService service;

        /// <summary>
        /// And endpoint to manage Sanction
        /// </summary>
        /// <param name="context">Database context</param>
        public SanctionController(ISanctionService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(Termination)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<Termination>(collection[nameof(Termination)].ToString());

            if (collection.Files.Count() == 1)
            {
                model.fileInterviewWarning = collection.Files[0];
                model.fileStatementOfPolice = null;
            } else if (collection.Files.Count() == 2)
            {
                model.fileInterviewWarning = collection.Files[0];
                model.fileStatementOfPolice = collection.Files[1];
            }

            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="collection">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromForm]IFormCollection collection)
        {
            if (collection.Count == 0 || !collection.Keys.Contains(nameof(Termination)))
            {
                return Ok(new ApiResponse(null, false, "bad request"));
            }

            var model = JsonConvert.DeserializeObject<Termination>(collection[nameof(Termination)].ToString());

            if (collection.Files.Count() == 2)
            {
                model.fileInterviewWarning = collection.Files[0];
                model.fileStatementOfPolice = collection.Files[1];
            }

            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]Termination model)
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
        public async Task<IActionResult> Reject([FromBody]Termination model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await service.Get(id);
            await service.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
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
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("approvalList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetApproval()
        {
            var result = await service.GetApprovalList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.GetById(id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="employeeId">Key employeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getLastSanctionById/{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLastSanctionById(string employeeId)
        {
            var result = await service.GetLastSanctionById(employeeId);
            return Ok(new ApiResponse(result));
        }
    }
}