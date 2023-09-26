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
    public class TaskTimesheetController : Controller
    {
        private ITaskTimesheetServices _service;

        public TaskTimesheetController(ITaskTimesheetServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TaskTimesheetDto> List(TaskTimesheetSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TaskTimesheetDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TaskTimesheet> Create(TaskTimesheetNew dto)
        {
            TaskTimesheetDto data = new TaskTimesheetDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TaskTimesheet> Update(TaskTimesheetDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TaskTimesheet> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
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

        [HttpGet]
        public GeneralResponseList<TimelineTaskNameByUserDto> ListTaskByUserName(string usernameId)
        {
            return _service.ListTaskByUserName(usernameId);
        }
        #endregion

        #region Timesheet
        [HttpPost]
        public GeneralResponse<TimesheetDetailDto> GetSubTaskTimesheetDetail(string subTaskId)
        {
            return _service.GetSubTaskTimesheetDetail(subTaskId);
        }

        [HttpPost]
        public GeneralResponse<string> EntryTimesheetSingle(TimesheetDurationDto param)
        {
            return _service.EntryTimesheetSingle(param);
        }

        [HttpPost]
        public GeneralResponse<string> EntryTimesheetMultiple(MultipleTimesheetDurationDto param)
        {
            return _service.EntryTimesheetMultiple(param);
        }

        [HttpGet]
        public GeneralResponseList<TimesheetDetailDto> ListSubTaskTimesheet(TimesheetParameterInputDto filter)
        {
            return _service.ListSubTaskTimesheet(filter);
        }

        [HttpPost]
        public GeneralResponse<string> StartTimeTimesheet(TimesheetPlayDto param)
        {
            return _service.StartTimeTimesheet(param);
        }

        [HttpPost]
        public GeneralResponse<string> EndTimeTimesheet(TimesheetPlayDto param)
        {
            return _service.StopTimesheet(param);
        }
        #endregion

    }
}