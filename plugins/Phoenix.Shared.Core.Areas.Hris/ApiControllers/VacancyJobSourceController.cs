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
    public class VacancyJobSourceController : Controller
    {
        private IVacancyJobSourceServices _service;

        public VacancyJobSourceController(IVacancyJobSourceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<VacancyJobSourceDto> List(VacancyJobSourceSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<VacancyJobSourceDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<VacancyJobSource> Create(VacancyJobSourceNew dto)
        {
            VacancyJobSourceDto data = new VacancyJobSourceDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<VacancyJobSource> Update(VacancyJobSourceDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<VacancyJobSource> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}