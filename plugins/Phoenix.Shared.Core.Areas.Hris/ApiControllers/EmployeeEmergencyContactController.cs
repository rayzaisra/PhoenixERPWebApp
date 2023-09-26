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
    public class EmployeeEmergencyContactController : Controller
    {
        private IEmployeeEmergencyContactServices _service;

        public EmployeeEmergencyContactController(IEmployeeEmergencyContactServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeEmergencyContactDto> List(EmployeeEmergencyContactSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeEmergencyContactDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<EmployeeEmergencyContact> Create([FromBody]EmployeeEmergencyContactNew dto)
        {
            EmployeeEmergencyContactDto data = new EmployeeEmergencyContactDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeEmergencyContact> Update([FromBody]EmployeeEmergencyContactDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeEmergencyContact> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}