using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixWebApi.Areas.Finance.Controllers.Shared
{
    [Produces("application/json")]
    [Route("api/financedropdown")]
    public class DropdownController : Controller
    {
        readonly IDropdownService service;

        /// <summary>
        /// And endpoint to manage MasterDivisionSelfservice
        /// </summary>
        /// <param name="context">Database context</param>
        public DropdownController(IDropdownService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("currency")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCurrency()
        {
            var result = await service.GetCurrencies();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("get_job_id")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetJobId()
        {
            var result = await service.GetJobId();
            return Ok(new ApiResponse(result));
        }

        [HttpGet("get_companys")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetCompanys()
        {
            var result = await service.GetCompanys();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("TAPDDLList")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetTAPDDL()
        {
            var result = await service.GetTAPDDL();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("coa/{param1}/{level}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetChartOfAccount(string param1, string level)
        {
            var result = await service.GetChartOfAccount(param1, level);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records with Text Value property. It is used for dropdownlist.
        /// </summary>
        /// <returns>List of json object with Text Value property.</returns>
        [HttpGet("GetMainServiceCategory")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetMainServiceCategory()
        {
            var result = await service.GetMainServiceCategory();
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("PoClient/{clientId}/{invoiceId?}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPurchaceOrderByClientId(string clientId, string invoiceId)
        {
            var result = await service.GetPurchaseOrderByClientId(clientId, invoiceId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("PoVendor/{vendorId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetPurchaseOrderByVendorId(string vendorId)
        {
            var result = await service.GetPurchaseOrderByVendorId(vendorId);
            return Ok(new ApiResponse(result));
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpGet("invoiceVendor/{poId}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> GetInvoiceVendorByPoId(string poId)
        {
            var result = await service.GetInvoiceVendorByPoId(poId);
            return Ok(new ApiResponse(result));
        }

        [HttpGet("MoveLocation")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> MoveLocation()
        {
            var result = await service.MoveLocation();
            return Ok(new ApiResponse(result));
        }
        [HttpGet("LocationWarehouse")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> LocationWarehouse()
        {
            var result = await service.LocationWarehouse();
            return Ok(new ApiResponse(result));
        }
        
        [HttpGet("InvGetform/{param}")]
        [ActionRole(ActionMethod.Allowed)]
        public async Task<IActionResult> InvGetform(string param)
        {
            var result = await service.InvGetform(param);
            return Ok(new ApiResponse(result));
        }


        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        //[HttpGet("getClients")]
        //[ActionRole(ActionMethod.Allowed)]
        //public async Task<IActionResult> GetClients()
        //{
        //    var result = await service.GetClients();
        //    return Ok(new ApiResponse(result));
        //}

    }
}
