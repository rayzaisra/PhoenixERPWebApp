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
    /// CompanyBrand API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/companybrand")]
    public class CompanyBrandController : Controller
    {
        readonly ICompanyBrandService service;

        /// <summary>
        /// And endpoint to manage CompanyBrand
        /// </summary>
        /// <param name="context">Database context</param>
        public CompanyBrandController(ICompanyBrandService service)
        {
            this.service = service;
        }

        //--------------------------------------Client--------------------------------


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Client")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> ClientPost([FromBody]CompanyBrand model)
        {
            await service.ClientAddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Client")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> ClientPut([FromBody]CompanyBrand model)
        {
            await service.ClientEditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("Client/{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> ClientDelete(string Id)
        {
            var model = await service.ClientGet(Id);
            await service.ClientDeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Client")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> ClientGet()
        {
            var result = await service.ClientGet();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Client/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> ClientGet(string Id)
        {
            var result = await service.ClientGet(Id);
            return Ok(new ApiResponse(result));
        }


        //--------------------------------------Vendor--------------------------------

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("Vendor")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> VendorPost([FromBody]CompanyBrand model)
        {
            await service.VendorAddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Vendor")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> VendorPut([FromBody]CompanyBrand model)
        {
            await service.VendorEditAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("Vendor/{Id}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> VendorDelete(string Id)
        {
            var model = await service.VendorGet(Id);
            await service.VendorDeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Vendor")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> VendorGet()
        {
            var result = await service.VendorGet();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("Vendor/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> VendorGet(string Id)
        {
            var result = await service.VendorGet(Id);
            return Ok(new ApiResponse(result));
        }
    }
}