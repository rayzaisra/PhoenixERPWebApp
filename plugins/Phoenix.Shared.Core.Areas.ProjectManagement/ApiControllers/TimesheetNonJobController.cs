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
    public class TimesheetNonJobController : Controller
    {
        private ITimesheetNonJobServices _service;

        public TimesheetNonJobController(ITimesheetNonJobServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TimesheetNonJobDto> List(TimesheetNonJobSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TimesheetNonJobDto> Get(string id)
        {
            return _service.GetTimesheetNonJob(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TimesheetNonJobNew> Create([FromBody] TimesheetNonJobNew dto)
        {
     
            return _service.CreateTimesheetNonJob(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TimesheetNonJobDto> Update([FromBody] TimesheetNonJobDto dto)
        {
            return _service.UpdateTimesheetNonJob(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TimesheetNonJob> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}