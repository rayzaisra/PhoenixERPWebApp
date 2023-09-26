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
using Phoenix.Data.Services.Pm.Report;
using PhoenixWebApi;

namespace PhoenixWebApi.Controllers.Pm
{
    /// <summary>
    /// Pce API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/pm/[controller]")]
    public class PceController : Controller
    {
        readonly IPceService service;
        readonly IPceReportServices pceReportServices;
        /// <summary>
        /// And endpoint to manage Pce
        /// </summary>
        /// <param name="context">Database context</param>
        public PceController(IPceService service, IPceReportServices pceReportServices)
        {
            this.service = service;
            this.pceReportServices = pceReportServices;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> Post([FromBody] Pce model)
        {
            await service.AddAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessAdd));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("Approve")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Approve([FromBody] Pce model)
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
        public async Task<IActionResult> Reject([FromBody] Pce model)
        {
            await service.ApproveAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessReject));
        }

        /// <summary>
        /// Edits existing record based on primary key id.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> Put([FromBody] Pce model)
        {
            await service.EditAsync(model);
            return Ok(new ApiResponse(model, true, Messages.SuccessEdit));
        }


        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPut("SubmitToClient")]
        [ActionRole(ActionMethod.Edit)]
        public async Task<IActionResult> SubmitToClient([FromBody] Pce model)
        {
            await service.SubmitToClientAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessSubmit));
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
        [HttpGet("GetList/{status?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetList(string status)
        {
            var result = await service.GetList(status, true);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetFinancePCEList/{status?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetFinancePCEList(string status)
        {
            var result = await service.GetList(status, false);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("GetReportPceDiego")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetReportPceDiego(string status)
        {
            var result = await pceReportServices.GetReportPceDiego();
            return Ok(new ApiResponse(result));
        }


        ///// <summary>
        ///// Gets all records without parameter as criteria.
        ///// </summary>
        ///// <returns>List of json object.</returns>
        //[HttpGet("approvalList")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetApprovalList()
        //{
        //    var result = await service.GetApprovalList();
        //    return Ok(new ApiResponse(result));
        //}

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <param name="pceId">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("invoiceList/{pceId?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetInvoiceDetailList(string pceId = "")
        {
            var result = await service.GetInvoiceDetailList(pceId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("PceTaskList/{id?}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetTaskList(string id)
        {
            var result = await service.GetTaskList(id);
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

        ///// <summary>
        ///// Gets single record based on specific primary key id.
        ///// </summary>
        ///// <param name="id">Key id parameter.</param>
        ///// <returns>A json object with given id.</returns>
        //[HttpGet("GetByJobId/{jobId}")]
        //[ActionRole(ActionMethod.Read)]
        //public async Task<IActionResult> GetByJobId(string jobId)
        //{
        //    var result = await service.GetByJobId(jobId);
        //    return Ok(new ApiResponse(result));
        //}

        /// <summary>
        /// Gets single record based on specific primary key id.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>A json object with given id.</returns>
        [HttpGet("print/{Id}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Print(string Id)
        {
            var result = await service.Print(Id);
            return Ok(new ApiResponse(result));
        }
    }
}