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
    public class CentralResourceAssigmentController : Controller
    {
        private ICentralResourceAssigmentServices _service;

        public CentralResourceAssigmentController(ICentralResourceAssigmentServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CentralResourceAssigmentDto> List(CentralResourceAssigmentSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CentralResourceAssigmentDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CentralResourceAssigment> Create(CentralResourceAssigmentNew dto)
        {
            CentralResourceAssigmentDto data = new CentralResourceAssigmentDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CentralResourceAssigment> Update(CentralResourceAssigmentDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CentralResourceAssigment> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}