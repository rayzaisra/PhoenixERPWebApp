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
    [AllowAnonymousRoleAttribute]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class AccountManagementController : Controller
    {
        private IAccountManagementServices _service;

        public AccountManagementController(IAccountManagementServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<AccountManagementList> List(AccountManagementSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet]
        public GeneralResponseList<Actor> ListActor()
        {
            return _service.ListActor();
        }
        [HttpGet("{id}")]
        public GeneralResponse<AccountManagementDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<AccountManagement> Create(AccountManagementNew dto)
        {
            AccountManagementDto data = new AccountManagementDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<AccountManagement> Update(AccountManagementDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<AccountManagement> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}