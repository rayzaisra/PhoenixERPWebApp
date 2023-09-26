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
    public class DivisionController : Controller
    {
        private IDivisionServices _service;

        public DivisionController(IDivisionServices service)
        {
            _service = service;
        }
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponseList<DivisionDto> List(DivisionSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponse<DivisionDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        [ActionRole(ActionMethod.Add)]
        public GeneralResponse<Division> Create(DivisionNew dto)
        {
            DivisionDto data = new DivisionDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        [ActionRole(ActionMethod.Edit)]
        public GeneralResponse<Division> Update(DivisionDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public GeneralResponse<Division> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}