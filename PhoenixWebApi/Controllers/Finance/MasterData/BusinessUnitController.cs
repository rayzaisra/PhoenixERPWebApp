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
    /// BusinessUnit API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/businessunit")]
    public class BusinessUnitController : Controller
    {
        readonly IBusinessUnitService service;

        /// <summary>
        /// And endpoint to manage BusinessUnit
        /// </summary>
        /// <param name="context">Database context</param>
        public BusinessUnitController(IBusinessUnitService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("finance")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> PostFinance([FromBody]BusinessUnit model)
        {
            await service.AddFinanceAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        ///// <summary>
        ///// Creates new record.
        ///// </summary>
        ///// <param name="model">Object parameter to post in json format.</param>
        ///// <returns>Json message if the process is successfully done or failed.</returns>
        //[HttpPost("hris")]
        //[ActionRole(ActionMethod.Add)]
        //public async Task<IActionResult> PostHris([FromBody]BusinessUnit model)
        //{
        //    await service.AddHrisAsync(model);
        //    return Ok(new ApiResponse(true, Messages.SuccessAdd));
        //}

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("finance")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> PutFinance([FromBody]BusinessUnit model)
        {
            await service.EditFinanceAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        ///// <summary>
        ///// Edits existing record based on primary key id.
        ///// </summary>
        ///// <param name="model">Object parameter to post in json format.</param>
        ///// <returns>Json message if the process is successfully done or failed.</returns>
        //[HttpPut("hris")]
        //[ActionRole(ActionMethod.Edit)]
        //public async Task<IActionResult> PutHris([FromBody]BusinessUnit model)
        //{
        //    await service.EditHrisAsync(model);
        //    return Ok(new ApiResponse(true, Messages.SuccessEdit));
        //}

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("finance/{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> DeleteFinance(string Id)
        {
            var model = await service.Get(Id);
            await service.DeleteFinanceSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        ///// <summary>
        ///// Deletes specific record based on given id parameter.
        ///// </summary>
        ///// <param name="id">Key id parameter.</param>
        ///// <returns>Json message if the process is successfully done or failed.</returns>
        //[HttpDelete("hris/{Id}")]
        //[ActionRole(ActionMethod.Delete)]
        //public async Task<IActionResult> DeleteHris(string Id)
        //{
        //    var model = await service.Get(Id);
        //    await service.DeleteHrisSoftAsync(model);
        //    return Ok(new ApiResponse(true, Messages.SuccessDelete));
        //}

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("finance")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFinance()
        {
            var result = await service.GetFinance();
            return Ok(new ApiResponse(result));
        }

        ///// <summary>
        ///// Gets all records without parameter as criteria.
        ///// </summary>
        ///// <returns>List of json object.</returns>
        //[HttpGet("hris")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetHris()
        //{
        //    var result = await service.GetHris();
        //    return Ok(new ApiResponse(result));
        //}

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