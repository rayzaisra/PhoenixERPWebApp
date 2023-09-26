using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class TimelineController : Controller
    {
        private ITimelineServices _service;

        public TimelineController(ITimelineServices service)
        {
            _service = service;
        }
        

        #region Timeline
        [HttpGet]
        public GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineUser()
        {
            return _service.ListMasterSearchTimelineUser();
        }

        [HttpGet]
        public GeneralResponseList<MasterSearchContentDto> ListMasterSearchTimelineJob()
        {
            return _service.ListMasterSearchTimelineJob();
        }

        [HttpGet]
        public GeneralResponseList<TableTimelineJobDto> ListTimeLine(TimelineParameterInputDto param)
        {
            return _service.ListTimeline(param);
        }
        [HttpGet("{job_number}")]
        public GeneralResponseList<TableTimelineJobDto> ListByJobNumber(string job_number)
        {
            return _service.ListTimelineByJobNumber(job_number);
        }
        [HttpGet]
        public GeneralResponseList<TimelineTaskNameByUserDto> ListTaskByUserName(string usernameId)
        {
            return _service.ListTaskByUserName(usernameId);
        }

        #endregion
 
    }
}