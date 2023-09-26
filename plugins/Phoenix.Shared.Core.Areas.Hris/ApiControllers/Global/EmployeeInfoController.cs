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
    /// Termination API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/employeeInfo")]
    [AllowAnonymousRoleAttribute]
    public class EmployeeInfoController : Controller
    {
        readonly IRequestorService service;

        /// <summary>
        /// And endpoint to manage Termination
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeeInfoController(IRequestorService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getByUserLogin")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetByUserLogin()
        {
            var result = await service.GetByUserLogin();
            return Ok(new ApiResponse(result));
        }
        
        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getById/{employeeId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetById(string employeeId)
        {
            var result = await service.GetEmployeeinfoById(employeeId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getByIdAndLeaveRequest/{employeeId}/{leaverequestId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> getByIdLeaveRequest(string employeeId, string leaverequestId)
        {
            var result = await service.GetEmployeeinfoByIdAndLeaveRequest(employeeId, leaverequestId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getAllEmployeByLeaveRequest/{leaverequestId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> getAllEmployeByLeaveRequest( string leaverequestId)
        {
            var result = await service.getAllEmployeByLeaveRequest(leaverequestId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getByName/{employeeName}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetByName(string employeeName)
        {
            var result = await service.GetEmployeeinfoByName(employeeName);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getLoanInfoById/{employeeId}/{isCorporateloan}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetLoanInfoById(string employeeId, string isCorporateloan)
        {
            var result = await service.GetEmployeeLoaninfoById(employeeId, isCorporateloan);
            return Ok(new ApiResponse(result));
        }
    }
}