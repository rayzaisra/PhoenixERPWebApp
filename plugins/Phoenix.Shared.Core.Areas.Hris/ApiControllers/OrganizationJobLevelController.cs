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
    public class OrganizationJobLevelController : Controller
    {
        private IOrganizationJobLevelServices _service;

        public OrganizationJobLevelController(IOrganizationJobLevelServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<OrganizationJobLevelDto> List(OrganizationJobLevelSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<OrganizationJobLevelDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<OrganizationJobLevel> Create(OrganizationJobLevelNew dto)
        {
            OrganizationJobLevelDto data = new OrganizationJobLevelDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<OrganizationJobLevel> Update(OrganizationJobLevelDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<OrganizationJobLevel> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}