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
    [Produces("application/json")]
    [Route("api/manageleavebalance")]
    public class ManageLeaveBalanceController : Controller
    {
        readonly IManageLeaveBalanceService service;

        public ManageLeaveBalanceController(IManageLeaveBalanceService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get()
        {
            var result = await service.Get();
            return Ok(new ApiResponse(result));
        }


        [HttpGet("getAnnualLeave/{employeeId}/{leaveType}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> getAnnualLeave(string employeeId, string leaveType)
        {
            var result = await service.getAnnualLeave(employeeId, leaveType);
            return Ok(new ApiResponse(result));
        }

        [HttpPost("saveManageLeaveBalance")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> saveManageLeaveBalance([FromBody] LeaveBalanceViewModel model)
        {
            var result = await service.saveManageLeaveBalance(model);
            return Ok(new ApiResponse(result,true,  Messages.SuccessAdd));
        }
    }
}
