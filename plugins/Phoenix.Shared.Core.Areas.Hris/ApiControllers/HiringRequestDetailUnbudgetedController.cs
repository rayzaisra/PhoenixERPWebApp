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
    public class HiringRequestDetailUnbudgetedController : Controller
    {
        private IHiringRequestDetailUnbudgetedServices _service;

        public HiringRequestDetailUnbudgetedController(IHiringRequestDetailUnbudgetedServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<HiringRequestDetailUnbudgetedDto> List(HiringRequestDetailUnbudgetedSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<HiringRequestDetailUnbudgetedDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<HiringRequestDetailUnbudgeted> Create(HiringRequestDetailUnbudgetedNew dto)
        {
            HiringRequestDetailUnbudgetedDto data = new HiringRequestDetailUnbudgetedDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<HiringRequestDetailUnbudgeted> Update(HiringRequestDetailUnbudgetedDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<HiringRequestDetailUnbudgeted> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}