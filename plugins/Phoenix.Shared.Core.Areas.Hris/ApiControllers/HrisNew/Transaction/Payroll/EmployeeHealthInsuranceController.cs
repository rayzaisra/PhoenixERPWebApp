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
    /// EmployeeHealthInsurance API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/employeehealthinsurance")]
    public class EmployeeHealthInsuranceController : Controller
    {
        readonly IEmployeeHealthInsuranceService service;

        /// <summary>
        /// And endpoint to manage EmployeeHealthInsurance
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeHealthInsuranceController(IEmployeeHealthInsuranceService service)
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
        public async Task<IActionResult> Post([FromBody]EmployeeHealthInsurance model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="employeeId">Key employeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string employeeId)
        {
            var result = await service.Get(employeeId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="employeeId">Key employeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getFamilyList/{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFamilyList(string employeeId)
        {
            var result = await service.GetFamilyList(employeeId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="familyId">Key familyId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getFamilyById/{familyId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFamilyById(string familyId)
        {
            var result = await service.GetFamilyById(familyId);
            return Ok(new ApiResponse(result));
        }
    }
}