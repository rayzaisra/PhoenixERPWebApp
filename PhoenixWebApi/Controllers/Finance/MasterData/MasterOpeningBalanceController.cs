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

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// MasterOpeningBalance API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/masteropeningbalance")]
    public class MasterOpeningBalanceController : Controller
    {
        readonly IMasterOpeningBalanceService service;

        /// <summary>
        /// And endpoint to manage MasterOpeningBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterOpeningBalanceController(IMasterOpeningBalanceService service)
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
        public async Task<IActionResult> Post([FromBody]MasterOpeningBalance model)
        {
            await service.AddEditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }
         
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get([FromBody]MasterOpeningBalanceFilter model)
        {
            var result = await service.Get(model);
            return Ok(new ApiResponse(result));
        }
    }
}