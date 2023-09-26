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
    public class PaDetailController : Controller
    {
        private IPaDetailServices _service;

        public PaDetailController(IPaDetailServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<PaDetailDto> List(PaDetailSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<PaDetailDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<PaDetail> Create(PaDetailNew dto)
        {
            PaDetailDto data = new PaDetailDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
 
        [HttpDelete("{id}")]
        public GeneralResponse<PaDetail> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}