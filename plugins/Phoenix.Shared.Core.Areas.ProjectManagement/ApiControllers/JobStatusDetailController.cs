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
    public class JobStatusDetailController : Controller
    {
        private IJobStatusDetailServices _service;

        public JobStatusDetailController(IJobStatusDetailServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<JobStatusDetailDto> List(JobStatusDetailSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<JobStatusDetailDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobStatusDetail> Create(JobStatusDetailNew dto)
        {
            JobStatusDetailDto data = new JobStatusDetailDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<JobStatusDetail> Update(JobStatusDetailDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<JobStatusDetail> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}