using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/auditdata")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class AuditTrailsController : Controller
    {
        private readonly IAuditTrailsService _repo;
        public AuditTrailsController(IAuditTrailsService repo)
        {
            _repo = repo;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        [Route("Get/{key?}")]
        public async Task<IActionResult> Get(string key = "")
        {
            return Ok(await _repo.GetBySearch(key).ConfigureAwait(false));
        }

    }

}