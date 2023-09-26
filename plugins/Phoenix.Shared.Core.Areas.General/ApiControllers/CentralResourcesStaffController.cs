using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Area(PhoenixModule.GENERAL)]
    [Route(PhoenixModule.GENERAL_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.GENERAL)]
    public class CentralResourcesStaffController : Controller
    {
        private ICentralResourcesStaffServices _service;

        public CentralResourcesStaffController(ICentralResourcesStaffServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CentralResourcesStaffDto> List(CentralResourcesStaffSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet]
        public GeneralResponseList<CentralResourcesStaffList> UserList(CentralResourcesStaffSearch parameter)
        {
            return _service.UserListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CentralResourcesStaffDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CentralResourcesStaff> Create(CentralResourcesStaffNew dto)
        {
            CentralResourcesStaffDto data = new CentralResourcesStaffDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CentralResourcesStaff> Update(CentralResourcesStaffDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CentralResourcesStaff> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}