using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class DepartementController : Controller
    {
        private IDepartementServices _service;

        public DepartementController(IDepartementServices service)
        {
            _service = service;
        }
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponseList<DepartementDto> List(DepartementSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponse<DepartementDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        [ActionRole(ActionMethod.Add)]
        public GeneralResponse<Departement> Create(DepartementNew dto)
        {
            DepartementDto data = new DepartementDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        [ActionRole(ActionMethod.Edit)]
        public GeneralResponse<Departement> Update(DepartementDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public GeneralResponse<Departement> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}