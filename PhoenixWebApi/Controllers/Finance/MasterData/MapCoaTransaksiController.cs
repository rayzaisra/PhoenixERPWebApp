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
    /// MapCoaTransaksi API Endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/mapcoatransaksi")]
    public class MapCoaTransaksiController : Controller
    {
        readonly IMapCoaTransaksiService service;

        /// <summary>
        /// And endpoint to manage MapCoaTransaksi
        /// </summary>
        /// <param name="context">Database context</param>
        public MapCoaTransaksiController(IMapCoaTransaksiService service)
        {
            this.service = service;
        }

        #region Setting Journal Job Closing
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterClosing")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterClosing()
        {
            var result = await service.GetMasterClosing();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterClosingById/{mainServiceCategoryId}/{businessUnitId}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterClosingById(string mainServiceCategoryId, string businessUnitId)
        {
            var result = await service.GetMasterClosingById(mainServiceCategoryId, businessUnitId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterClosingAP/{mainServiceCategoryId}/{businessUnitId}/{isEdit}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterClosingAP(string mainServiceCategoryId, string businessUnitId, bool isEdit)
        {
            var result = await service.GetMasterClosingAP(mainServiceCategoryId, businessUnitId, isEdit);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterClosingAR/{mainServiceCategoryId}/{businessUnitId}/{isEdit}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterClosingAR(string mainServiceCategoryId, string businessUnitId, bool isEdit)
        {
            var result = await service.GetMasterClosingAR(mainServiceCategoryId, businessUnitId, isEdit);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("DeleteMasterClosing/{mainServiceCategoryId}/{businessUnitId}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> DeleteSoftMasterClosingAsync(string mainServiceCategoryId, string businessUnitId)
        {
            await service.DeleteSoftMasterClosingAsync(mainServiceCategoryId, businessUnitId);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("AddMasterClosing")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddMasterClosingAsync([FromBody]List<MapCoaTransaksi> model)
        {
            await service.AddMasterClosingAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        #endregion

        #region Setting Journal AP
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterAP")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterAP()
        {
            var result = await service.GetMasterAP();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterAPById/{mainServiceCategoryId}/{pceTypeCrossBilling}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterAPById(string mainServiceCategoryId, bool pceTypeCrossBilling)
        {
            var result = await service.GetMasterAPById(mainServiceCategoryId, pceTypeCrossBilling);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterDetailAP/{mainServiceCategoryId}/{pceTypeCrossBilling}/{isEdit}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterDetailAP(string mainServiceCategoryId, bool pceTypeCrossBilling, bool isEdit)
        {
            var result = await service.GetMasterDetailAP(mainServiceCategoryId, pceTypeCrossBilling, isEdit);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("DeleteMasterAP/{mainServiceCategoryId}/{pceTypeCrossBilling}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> DeleteSoftAPAsync(string mainServiceCategoryId, bool pceTypeCrossBilling)
        {
            await service.DeleteSoftAPAsync(mainServiceCategoryId, pceTypeCrossBilling);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("AddMasterAP")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddAPAsync([FromBody]List<MapCoaTransaksi> model)
        {
            await service.AddAPAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
        #endregion

        #region Setting Journal AR
        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterAR")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterAR()
        {
            var result = await service.GetMasterAR();
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterARById/{mainServiceCategoryId}/{pceTypeCrossBilling}/{invoiceType}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterAR(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType)
        {
            var result = await service.GetMasterARById(mainServiceCategoryId, pceTypeCrossBilling, invoiceType);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetMasterDetailAR/{mainServiceCategoryId}/{pceTypeCrossBilling}/{invoiceType}/{isEdit}")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetMasterDetailAR(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType, bool isEdit)
        {
            var result = await service.GetMasterDetailAR(mainServiceCategoryId, pceTypeCrossBilling, invoiceType, isEdit);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Deletes specific record based on given id parameter.
        /// </summary>
        /// <param name="id">Key id parameter.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpDelete("DeleteMasterAR/{mainServiceCategoryId}/{pceTypeCrossBilling}/{invoiceType}")]
        [ActionRole(ActionMethod.Delete)]
        public async Task<IActionResult> DeleteSoftARAsync(string mainServiceCategoryId, bool pceTypeCrossBilling, string invoiceType)
        {
            await service.DeleteSoftARAsync(mainServiceCategoryId, pceTypeCrossBilling, invoiceType);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("AddMasterAR")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddARAsync([FromBody]List<MapCoaTransaksi> model)
        {
            await service.AddARAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
        #endregion

        #region Setting Journal Petty Cash

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetJournalPettyCash")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournalPettyCash()
        {
            var result = await service.GetJournalPettyCash();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("AddJournalPettyCash")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddPettyCashAsync([FromBody]List<MapCoaTransaksi> model)
        {
            await service.AddPettyCashAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
        #endregion


        #region Setting Journal Salary Payment Request

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("GetJournalSalaryPaymentRequest")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetJournalSalaryPaymentRequest()
        {
            var result = await service.GetJournalSalaryPaymentRequest();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="model">Object parameter to post in json format.</param>
        /// <returns>Json message if the process is successfully done or failed.</returns>
        [HttpPost("AddJournalSalaryPaymentRequest")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> AddSalaryPaymentRequestAsync([FromBody]List<MapCoaTransaksi> model)
        {
            await service.AddSalaryPaymentRequestAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
        #endregion
    }
}