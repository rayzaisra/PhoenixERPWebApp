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
    public class MppApprovedStatusController : Controller
    {
        private IMppApprovedStatusServices _service;

        public MppApprovedStatusController(IMppApprovedStatusServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<MppApprovedStatusDto> List(MppApprovedStatusSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<MppApprovedStatusDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<MppApprovedStatus> Create(MppApprovedStatusNew dto)
        {
            MppApprovedStatusDto data = new MppApprovedStatusDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<MppApprovedStatus> Update(MppApprovedStatusDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<MppApprovedStatus> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}