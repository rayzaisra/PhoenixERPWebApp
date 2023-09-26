using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;
using Phoenix.Data;
using Phoenix.Data.Models.ViewModel;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [Route("api/RptStatementAP")]
    public class RptStatementAPController : ControllerBase
    {
        readonly IRptStatementAPService service;
        public RptStatementAPController(IRptStatementAPService service)
        {
            this.service = service;
        }
        [HttpPost("GetData")]
        [ActionRole(ActionMethod.Read)]
        public async Task<IActionResult> GetData([FromBody] VwStatementOfApFilter Filter)
        {
            var result = await service.Get(Filter);
            return Ok(new ApiResponse(result));
        }
    }
}