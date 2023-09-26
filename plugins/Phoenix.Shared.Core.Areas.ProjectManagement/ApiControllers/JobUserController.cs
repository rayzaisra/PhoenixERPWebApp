using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.ApiControllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    [Route("api/[controller]/[action]")]
    public class JobUserController : Controller
    {
        private IJobUserServices _service;

        public JobUserController(IJobUserServices service)
        {
            _service = service;
        }

        [HttpGet]
        public GeneralResponseList<JobUserDto> List(JobUserDto data)
        {
            return _service.ListAll(data);
        }

    }
 
}
