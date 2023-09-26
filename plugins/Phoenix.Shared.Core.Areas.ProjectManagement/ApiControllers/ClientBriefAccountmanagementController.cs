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
    public class ClientBriefAccountmanagementController : Controller
    {
        private IClientBriefAccountmanagementServices _service;

        public ClientBriefAccountmanagementController(IClientBriefAccountmanagementServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<ClientBriefAccountmanagementDto> List(ClientBriefAccountmanagementSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<ClientBriefAccountmanagementDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<ClientBriefAccountmanagement> Create(ClientBriefAccountmanagementNew dto)
        {
            ClientBriefAccountmanagementDto data = new ClientBriefAccountmanagementDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<ClientBriefAccountmanagement> Update(ClientBriefAccountmanagementDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<ClientBriefAccountmanagement> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}