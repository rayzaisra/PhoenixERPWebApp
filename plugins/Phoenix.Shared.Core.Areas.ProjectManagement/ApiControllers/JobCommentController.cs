using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class JobCommentController : Controller
    {
        private IJobCommentServices _service;

        public JobCommentController(IJobCommentServices service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public GeneralResponseList<JobCommentModel> GetComments(string id)
        {
            return _service.GetComments(id);
        }

        [HttpPost]
        public GeneralResponseList<JobCommentModel> CreateComments([FromBody]JobCommentModel dto)
        {
            return _service.CreateComments(dto);
        }
    }
}
