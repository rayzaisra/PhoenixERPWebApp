using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Parameters;
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
    public class ClientBriefController : Controller
    {
        private IClientBriefServices _service;

        public ClientBriefController(IClientBriefServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<ClientBriefDto> List(ClientBriefSearch parameter)
        {
            return _service.ListAll(parameter);
        }

        [HttpGet]
        public GeneralResponse<ClientBriefDto> CheckUserId()
        {
            return _service.CheckUserId();
        }

        [HttpGet]
        public GeneralResponseList<CompanyDto> CompanyList(SearchParameter data)
        {
            return _service.CompanyList(data);
        }

        [HttpGet("{id}")]
        public GeneralResponse<ClientBriefDetil> Get(string id)
        {
            return _service.GetDetil(id);
        }
        [HttpPost]
        //[ValidateModel]
        public GeneralResponse<ClientBriefNew> Create(ClientBriefNew dto)
        {
         
            return _service.CreateClientBrief(dto);
        }
        [HttpPost]
        public GeneralResponse<ClientBriefDetil> Update(ClientBriefUpdate dto)
        {
            return _service.UpdateClientBrief(dto);
        }
        [HttpDelete("{id}")]
        //public GeneralResponse<ClientBrief> Delete(string id)
        public GeneralResponse<ClientBriefDto> Delete(ClientBriefDto data)
        {
            //return _service.DeleteByIDResponse(id);
            return _service.Delete(data);
        }

        

    }
}