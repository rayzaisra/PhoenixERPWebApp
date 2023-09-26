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
    public class EmployeeChecklistController : Controller
    {
        private IEmployeeChecklistServices _service;

        public EmployeeChecklistController(IEmployeeChecklistServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeChecklistDto> List(SearchEmployeeChecklist parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeChecklistDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<EmployeeChecklist> Create(EmployeeChecklistNew dto)
        {
            EmployeeChecklistDto data = new EmployeeChecklistDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeChecklist> Update(EmployeeChecklistDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeChecklist> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}