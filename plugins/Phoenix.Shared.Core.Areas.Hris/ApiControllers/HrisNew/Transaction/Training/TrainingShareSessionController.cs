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

namespace Phoenix.Shared.Core.Areas.Hris.ApiControllers.HrisNew.Transaction.Training
{
	[Produces("application/json")]
	[Route("api/TrainingShareSession")]
	public class TrainingShareSessionController : Controller
	{
		readonly ITrainingShareSessionService service;

		public TrainingShareSessionController(ITrainingShareSessionService service)
		{
			this.service = service;
		}

        [HttpGet("TrainingList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetTrainingShareSessionList()
        {
            var result = await service.GetTrainingShareSessionList();
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

        [HttpGet("gettrainingcompleted/{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetTrainingCompletedById(string Id)
        {
            var result = await service.GetTrainingCompletedById(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]TrainingShareSession model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]TrainingShareSession model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }
    }
}
