using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// Holiday API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/TrainingRequisition")]
    public class TrainingRequisitionController : Controller
    {
        readonly ITrainingRequisitionService service;

        /// <summary>
        /// And endpoint to manage Holiday
        /// </summary>
        /// <param name="context">Database context</param>
        public TrainingRequisitionController(ITrainingRequisitionService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]TrainingRequisition model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("AddWithDetail")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddWithDetail([FromBody]TrainingRequisitionDTO model)
        {
            await service.AddWithDetail(model.Header,model.Details);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }


        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]TrainingRequisition model)
        {
            try
            {
                await service.Approve(model);
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }


        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]TrainingRequisition model)
        {
            try
            {
                await service.Reject(model);
                return Ok(new ApiResponse(model, true, Messages.SuccessApprove));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse(false, ex.Message.ToString()));
            }

        }


        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]TrainingRequisition model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("EditWithDetail")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditWithDetail([FromBody]TrainingRequisitionDTO model)
        {
            await service.EditWithDetail(model.Header,model.Details);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await service.Get(Id);
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
        [HttpGet("TrainingRequest")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetTrainingRequestList()
        {
            var result = await service.GetTrainingRequestList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string Id)
        {
            var result = await service.Get(Id);
            return Ok(new ApiResponse(result));
        }

    }
}