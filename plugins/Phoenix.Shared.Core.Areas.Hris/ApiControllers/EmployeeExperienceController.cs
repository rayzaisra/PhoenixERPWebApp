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
    public class EmployeeExperienceController : Controller
    {
        private IEmployeeExperienceServices _service;

        public EmployeeExperienceController(IEmployeeExperienceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeExperienceDto> List(EmployeeExperienceSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeExperienceDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<EmployeeExperience> Create([FromBody]EmployeeExperienceNew dto)
        {
            EmployeeExperienceDto data = new EmployeeExperienceDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeExperience> Update([FromBody]EmployeeExperienceDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeExperience> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}