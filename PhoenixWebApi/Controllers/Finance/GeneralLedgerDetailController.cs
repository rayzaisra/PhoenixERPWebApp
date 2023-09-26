using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;


namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralLedgerDetailController : ControllerBase
    {
        readonly IGeneralLedgerDetailService service;
        public GeneralLedgerDetailController(IGeneralLedgerDetailService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all records without parameter as criteria.
        /// </summary>
        /// <returns>List of json object.</returns>
        [HttpPost("GetList")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetList([FromBody]GeneralLedgerDetailSearch model)
        {
            var result = await service.GetList(model);
            return Ok(new ApiResponse(result));
        }
    }
}