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
    /// Attendance API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/attendance")]
    public class AttendanceController : Controller
    {
        readonly IAttendanceService service;

        /// <summary>
        /// And endpoint to manage Attendance
        /// </summary>
        /// <param name="context">Database context</param>
        public AttendanceController(IAttendanceService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("checkin")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CheckIn([FromBody]Attendance model)
        {
            await service.CheckIn(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("checkout")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> CheckOut([FromBody]Attendance model)
        {
            await service.CheckOut(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("{dtStart}/{dtEnd}/{isNotClockOut}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string dtStart, string dtEnd, int isNotClockOut)
        {
            var result = await service.GetList(dtStart, dtEnd, isNotClockOut);
            return Ok(new ApiResponse(result));
        }

		/// <summary>
		/// Gets stasus attendance today.
		/// </summary>
		/// <returns>single json object.</returns>
		[HttpGet]
		[ActionRole(ActionMethod.Read)]
		public async Task<IActionResult> CheckStatus()
		{
			var result = await service.CheckStatus();
			return Ok(new ApiResponse(result));
		}

        /// <summary>
		/// Gets stasus attendance today.
		/// </summary>
		/// <returns>single json object.</returns>
		[HttpGet("{userid}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> CheckStatusById(string userid)
        {
            var result = await service.CheckStatus(userid);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("Report/{month}/{year}/{legalEntityId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Report(string month, string year, string legalEntityId = null)
        {
            var result = await service.ReportList(month, year, legalEntityId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getbyid")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetById()
        {
            var result = await service.GetById();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getStatusAttendanceById")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> getStatusAttendanceById()
        {
            var result = await service.getStatusAttendanceById();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("getreportById/{employeeId}/{month}/{year}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportById(string employeeId, int month, int year)
        {
            var result = await service.GetReportById(employeeId, month, year);
            return Ok(new ApiResponse(result));
        }
    }
}