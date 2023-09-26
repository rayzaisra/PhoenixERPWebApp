using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class MotherPcaController : Controller
    {
        private IMotherPcaServices _service;

        public MotherPcaController(IMotherPcaServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<MotherPcaDto> List(MotherPcaSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<MotherPcaDto> Get(string id)
        {
            return _service.GetMotherPca(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<MotherPcaNew> Create([FromBody] MotherPcaNew dto)
        {

            return _service.CreateMotherPca(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<MotherPcaDto> Update([FromBody] MotherPcaDto dto)
        {
            return _service.UpdateMotherPca(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<MotherPca> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}