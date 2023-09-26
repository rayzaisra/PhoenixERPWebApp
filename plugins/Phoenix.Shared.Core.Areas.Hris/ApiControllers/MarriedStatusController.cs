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
    //[JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class MarriedStatusController : Controller
    {
        private IMarriedStatusServices _service;

        public MarriedStatusController(IMarriedStatusServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<MarriedStatusDto> List(MarriedStatusSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<MarriedStatusDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<MarriedStatus> Create(MarriedStatusNew dto)
        {
            MarriedStatusDto data = new MarriedStatusDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<MarriedStatus> Update(MarriedStatusDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<MarriedStatus> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}