using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [Route("api/[controller]")]
    public class BankPaymentController : ControllerBase
    {
        readonly IBankPaymentService service;
        public BankPaymentController(IBankPaymentService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpPost("GetBankPaymentList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> Get([FromBody] FilterPaymentRequest search)
        {
            var result = await service.Get(search);
            return Ok(new ApiResponse(result));
        }
    }
}