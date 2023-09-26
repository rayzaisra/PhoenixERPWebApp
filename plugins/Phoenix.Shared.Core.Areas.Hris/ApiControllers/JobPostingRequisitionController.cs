using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class JobPostingRequisitionController : Controller
    {
        private IJobPostingRequisitionServices _service;

        public JobPostingRequisitionController(IJobPostingRequisitionServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<JobPostingRequisitionDto> List(JobPostingRequisitionSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<JobPostingRequisitionDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobPostingRequisition> Create(JobPostingRequisitionNew dto)
        {
            JobPostingRequisitionDto data = new JobPostingRequisitionDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<JobPostingRequisition> Update(JobPostingRequisitionDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<JobPostingRequisition> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}