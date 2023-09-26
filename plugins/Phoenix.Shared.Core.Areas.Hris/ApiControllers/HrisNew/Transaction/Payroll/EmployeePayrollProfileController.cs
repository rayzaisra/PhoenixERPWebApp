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
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;

namespace Phoenix.Shared.Core.Areas.Hris.Controllers
{
    /// <summary>
    /// EmployeeSubcomponentSalary API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/employeepayrollprofile")]
    public class EmployeePayrollProfileController : Controller
    {
        readonly IEmployeePayrollProfileService service;

        /// <summary>
        /// And endpoint to manage EmployeeSubcomponentSalary
        /// </summary>
        /// <param name="context">Database context</param>
        public EmployeePayrollProfileController(IEmployeePayrollProfileService service)
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
        public async Task<IActionResult> Post([FromBody]EmployeePayrollProfile model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }


		/// <summary>
		/// Gets all records without parameter as criteria.
		/// </summary>
		/// <returns>List of json object.</returns>
		[HttpGet("gets/{emplyeeName?}")]
		[ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Gets(string emplyeeName)
        {
            var result = await service.Gets(emplyeeName);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
		/// Gets all records without parameter as criteria.
		/// </summary>
		/// <returns>List of json object.</returns>
		[HttpGet("payrollHistory/{empId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> payrollHistory(string empId)
        {
            var result = await service.HistoryPayroll(empId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key EmployeeId.
        /// </summary>
        /// <param name="EmployeeId">Key EmployeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string EmployeeId)
        {
            var result = await service.Get(EmployeeId);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets single record based on specific primary key EmployeeId.
        /// </summary>
        /// <param name="EmployeeId">Key EmployeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("additional/{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetAdditionalList(string EmployeeId)
        {
            var result = await service.GetAdditionalList(EmployeeId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key EmployeeId.
        /// </summary>
        /// <param name="EmployeeId">Key EmployeeId parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("deducation/{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetDeductionList(string EmployeeId)
        {
            var result = await service.GetDeductionList(EmployeeId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("getpph21/{id}/{employeeId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> getpph21(string id, string employeeId)
        {
            var result = await service.getpph21(id, employeeId);
            return Ok(new ApiResponse(result));
        }
    }
}