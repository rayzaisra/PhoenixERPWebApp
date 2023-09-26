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
    public class ClientBriefCentralResourceController : Controller
    {
        private IClientBriefCentralResourceServices _service;

        public ClientBriefCentralResourceController(IClientBriefCentralResourceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<ClientBriefCentralResourceDto> List(ClientBriefCentralResourceSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<ClientBriefCentralResourceDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<ClientBriefCentralResource> Create(ClientBriefCentralResourceNew dto)
        {
            ClientBriefCentralResourceDto data = new ClientBriefCentralResourceDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<ClientBriefCentralResource> Update(ClientBriefCentralResourceDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<ClientBriefCentralResource> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}