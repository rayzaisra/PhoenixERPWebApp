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
    [Route("api/payroll")]
    public class PayrollController : Controller
    {
        readonly IPayrollService service;

        /// <summary>
        /// And endpoint to manage EmployeeSubcomponentSalary
        /// </summary>
        /// <param name="context">Database context</param>
        public PayrollController(IPayrollService service)
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
        public async Task<IActionResult> Post([FromBody]SalaryProcessHeader model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody]SalaryProcessHeader model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }
        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody]SalaryProcessHeader model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessApprove));
        }


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Reject")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Reject([FromBody]SalaryProcessHeader model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
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

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("approvalList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetApprovalList()
        {
            var result = await service.GetApprovalList();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{month}/{year}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(int month, int year)
        {
            var result = await service.Get(month, year);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await service.Get(id);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpGet("Generate/{month}/{year}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Generate(int month, int year)
        {
            var model = await service.Generate(month, year);
            return Ok(new ApiResponse(model, true, "Data has been successfully generated."));
        }

        [HttpGet("DecryptPayroll")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> DecryptPayroll()
        {
            var model = await service.DecryptPayroll();
            return Ok(new ApiResponse(model, true, "Data has been decrypted succesfully."));
        }

        [HttpGet("EncryptPayroll")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> EncryptPayroll()
        {
            var model = await service.EncryptPayroll();
            return Ok(new ApiResponse(model, true, "Data has been encrypted succesfully."));
        }
    }
}