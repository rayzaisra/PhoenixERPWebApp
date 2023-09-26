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
    public class DamTypeController : Controller
    {
        private IDamTypeServices _service;

        public DamTypeController(IDamTypeServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<DamTypeDto> List(DamTypeSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<DamTypeDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<DamType> Create(DamTypeNew dto)
        {
            DamTypeDto data = new DamTypeDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<DamType> Update(DamTypeDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<DamType> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}