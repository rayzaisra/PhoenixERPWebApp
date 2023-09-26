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

    [Produces("application/json")]
    [Route("api/TrainingRegistrationPaymentReq")]
    public class TrainingRegistrationPaymentReqController : Controller
    {
        readonly ITrainingRegistrationPaymentReqService service;

        public TrainingRegistrationPaymentReqController(ITrainingRegistrationPaymentReqService service)
        {
            this.service = service;
        }

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

        [HttpGet("gettrainingregpaydetail/{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetTrainingRegPaymentDetailById(string Id)
        {
            var result = await service.GetTrainingRegPaymentDetailById(Id);
            return Ok(new ApiResponse(result));
        }

        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody]TrainingRegPayment model)
        {
            await service.AddAsync(model, false);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]TrainingRegPayment model)
        {
            await service.AddAsync(model, true);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

    }
}
