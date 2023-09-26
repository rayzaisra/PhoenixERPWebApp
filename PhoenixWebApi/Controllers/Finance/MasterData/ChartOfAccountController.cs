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
using Phoenix.Data.Models.Finance.Master;
using Phoenix.Data.RestApi;

namespace PhoenixWebApi.Controllers.Finance
{
    /// <summary>
    /// Chartofaccount API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ChartOfAccountController : Controller
    {
        readonly IChartOfAccountService service;

        /// <summary>
        /// And endpoint to manage Chartofaccount
        /// </summary>
        /// <param name="context">Database context</param>
        public ChartOfAccountController(IChartOfAccountService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Level1")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddLevel1Async([FromBody]ChartOfAccount model)
        {
            await service.AddLevel1Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Level1")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditLevel1Async([FromBody]ChartOfAccount model)
        {
            await service.EditLevel1Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Level2")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddLevel2Async([FromBody]ChartOfAccount model)
        {
            await service.AddLevel2Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Level2")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditLevel2Async([FromBody]ChartOfAccount model)
        {
            await service.EditLevel2Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Level3")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddLevel3Async([FromBody]ChartOfAccount model)
        {
            await service.AddLevel3Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Level3")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditLevel3Async([FromBody]ChartOfAccount model)
        {
            await service.EditLevel3Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Level4")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddLevel4Async([FromBody]ChartOfAccount model)
        {
            await service.AddLevel4Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Level4")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditLevel4Async([FromBody]ChartOfAccount model)
        {
            await service.EditLevel4Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Level5")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddLevel5Async([FromBody]ChartOfAccount model)
        {
            await service.AddLevel5Async(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Level5")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> EditLevel5Async([FromBody]ChartOfAccount model)
        {
            await service.EditLevel5Async(model);
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
        [HttpGet("Level1")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel1()
        {
            var result = await service.GetLevel1();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Level2")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel2()
        {
            var result = await service.GetLevel2();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Level3")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel3()
        {
            var result = await service.GetLevel3();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Level4")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel4()
        {
            var result = await service.GetLevel4();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Level5")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel5()
        {
            var result = await service.GetLevel5();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Level1/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel1(string Id)
        {
            var result = await service.GetLevel1(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Level2/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel2(string Id)
        {
            var result = await service.GetLevel2(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Level3/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel3(string Id)
        {
            var result = await service.GetLevel3(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Level4/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel4(string Id)
        {
            var result = await service.GetLevel4(Id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Level5/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetLevel5(string Id)
        {
            var result = await service.GetLevel5(Id);
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