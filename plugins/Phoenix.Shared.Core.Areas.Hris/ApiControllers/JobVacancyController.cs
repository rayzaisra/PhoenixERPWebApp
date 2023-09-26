using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Entities.Hris;
using Phoenix.Shared.Core.Parameters;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class JobVacancyController : Controller
    {
        private IJobVacancyServices _service;

        public JobVacancyController(IJobVacancyServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<JobVacancyDto> List(JobVacancySearch parameter)
        {
            return _service.ListAll(parameter);
        }

        [HttpGet]
        public GeneralResponseList<MasterLocationDto> GetAllLocation(SearchParameter parameter)
        {
            return _service.ListMasterLocation(parameter);
        }
        
        [HttpGet("{id}")]
        public GeneralResponse<JobVacancyDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<JobVacancyDto> GetWithDetial(string id)
        {
            return _service.GetWithDetial(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<JobVacancyDto> GetDetialWithIDHiring(string id)
        {
            return _service.GetDetialWithIDHiring(id);
        }

        [HttpGet("{id}")]
        public GeneralResponse<JobVacancyDto> GetVancacyNameByCode(string id)
        {
            return _service.GetVancacyNameByCode(id);
        }

        [HttpGet]
        public GeneralResponseList<JobVacancyDto> ListPostingCode()
        {
            return _service.ListPostingCode();
        }

        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobVacancy> Create(NewJobVacancyDto dto)
        {
            JobVacancyDto data = new JobVacancyDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }

        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobVacancy>CreateDetail(NewJobVacancyDto dto)
        {
            JobVacancyDto data = new JobVacancyDto();
            PropertyMapper.All(dto, data);
            return _service.CreateDetail(data);
        }

        [HttpPost, ValidateModel]
        public GeneralResponse<JobVacancy> Update(JobVacancyDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<JobVacancy> UpdatePostingStatus([FromBody]JobVacancyDto dto)
        {
            return _service.UpdatePostingStatus(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<JobVacancy> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<JobVacancy> DeleteDetil(string id)
        {
            return _service.DeleteDetil(id);
        }

    }
}