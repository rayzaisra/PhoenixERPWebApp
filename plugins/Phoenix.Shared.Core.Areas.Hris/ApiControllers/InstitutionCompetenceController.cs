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
    public class InstitutionCompetenceController : Controller
    {
        private IInstitutionCompetenceServices _service;

        public InstitutionCompetenceController(IInstitutionCompetenceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<InstitutionCompetenceDto> List(InstitutionCompetenceSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<InstitutionCompetenceDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<InstitutionCompetence> Create(InstitutionCompetenceNew dto)
        {
            InstitutionCompetenceDto data = new InstitutionCompetenceDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<InstitutionCompetence> Update(InstitutionCompetenceDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<InstitutionCompetence> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}