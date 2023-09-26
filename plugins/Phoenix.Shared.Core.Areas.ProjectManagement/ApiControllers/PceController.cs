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
    public class PceController : Controller
    {
        private IPceServices _service;

        public PceController(IPceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<PceDto> List(PceSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<PceDto> Get(string id)
        {
            return _service.GetPce(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<PceNew> Create([FromBody] PceNew dto)
        {

            return _service.CreatePce(dto);
        }
        [HttpPost("{id}")]
        public BaseGeneralResponse openpce(string id)
        {
            return _service.openpce(id);
        }

        [HttpPost, ValidateModel]
        public GeneralResponse<PceDto> Update([FromBody] PceDto dto)
        {
            return _service.UpdatePce(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<PCE> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}