using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/up")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class UserProfileController : Controller
    {
        public UserProfileController()
        { }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("getroot")]
        [Route("getroot/{folder?}")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok();
        }
    }
}