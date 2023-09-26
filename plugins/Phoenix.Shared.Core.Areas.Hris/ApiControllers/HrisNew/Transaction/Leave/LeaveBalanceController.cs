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
    /// LeaveBalance API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/leavebalance")]
    public class LeaveBalanceController : Controller
    {
        readonly ILeaveBalanceService service;

        /// <summary>
        /// And endpoint to manage LeaveBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public LeaveBalanceController(ILeaveBalanceService service)
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
        public async Task<IActionResult> Post([FromBody]string Type)
        {
            await service.Generate(Type);
            return Ok(new ApiResponse(true, "Data has been successfully generated."));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }

        ///// <summary>
        ///// Gets all records without parameter as criteria.
        ///// </summary>
        ///// <returns>List of json object.</returns>
        //[HttpGet("getById/{employeeId}")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetById(string employeeId)
        //{
        //    var result = await service.GetById(employeeId);
        //    return Ok(new ApiResponse(result));
        //}

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("getEmployeeLeaveBalaceInfoList/{employeeId}/{leaveRequestTypeId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetEmployeeLeaveBalaceInfoList(string employeeId, string leaveRequestTypeId)
        {
            var result = await service.GetEmployeeLeaveBalaceInfoList(employeeId, leaveRequestTypeId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GenerateByLeaveRequestTypeId/{LeaveRequestTypeId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GenerateByLeaveRequestTypeId(string LeaveRequestTypeId)
        {
            var result = await service.GenerateByLeaveRequestTypeId(LeaveRequestTypeId);
            return Ok(new ApiResponse(result));
        }


        ///// <summary>
        ///// Gets all records without parameter as criteria.
        ///// </summary>
        ///// <returns>List of json object.</returns>
        //[HttpGet("getLeaveHistoryList/{employeeId}")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetLeaveHistoryList(string id)
        //{
        //    var result = await service.GetLeaveHistoryList(id);
        //    return Ok(new ApiResponse(result));
        //}

    }
}