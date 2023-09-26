using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Entities.Hris;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class JobVacancyJobSouceController : Controller
    {
        private IJobVacancyJobSourceServices _service;

        public JobVacancyJobSouceController(IJobVacancyJobSourceServices service)
        {
            _service = service;
        }
       
        [HttpGet("{id}")]
        public GeneralResponse<JobVacancyJobSourceDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobVacancyJobSouce> Create(JobVacancyJobSourceNew dto)
        {
            JobVacancyJobSourceDto data = new JobVacancyJobSourceDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<JobVacancyJobSouce> Update(JobVacancyJobSourceDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<JobVacancyJobSouce> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}