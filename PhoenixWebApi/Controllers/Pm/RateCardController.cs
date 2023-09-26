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
using PhoenixWebApi;

namespace PhoenixWebApi.Controllers.Pm
{
    /// <summary>
    /// Pca API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/pm/[controller]")]
    public class RateCardController : Controller
    {
        readonly IRateCardService service;

        /// <summary>
        /// And endpoint to manage Pca
        /// </summary>
        /// <param name="context">Database context</param>
        public RateCardController(IRateCardService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="rateCardId">Key id parameter.</param>
        /// <param name="taskRateCardName">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetTaskRateCard/{rateCardId}/{taskRateCardName}")]
        [AllowAnonymousRole]
        public async Task<IActionResult> GetTaskRateCard(string rateCardId, string taskRateCardName)
        {
            var result = await service.GetTaskRateCard(rateCardId, taskRateCardName);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="rateCardId">Key id parameter.</param>
        /// <param name="taskRateCardName">Key id parameter.</param>
        /// <param name="subTaskRateCardName">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("GetSubTaskRateCard/{rateCardId}/{taskRateCardName}/{subTaskRateCardName}")]
        [AllowAnonymousRole]
        public async Task<IActionResult> GetSubTaskRateCard(string rateCardId, string taskRateCardName, string subTaskRateCardName)
        {
            var result = await service.GetSubTaskRateCard(rateCardId, taskRateCardName, subTaskRateCardName);
            return Ok(new ApiResponse(result));
        }
    }
}