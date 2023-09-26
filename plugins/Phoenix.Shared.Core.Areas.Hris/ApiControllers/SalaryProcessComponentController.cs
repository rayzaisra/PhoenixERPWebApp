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
    public class SalaryProcessComponentController : Controller
    {
        private ISalaryProcessComponentServices _service;

        public SalaryProcessComponentController(ISalaryProcessComponentServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<SalaryProcessComponentDto> List(SalaryProcessComponentSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<SalaryProcessComponentDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<SalaryProcessComponent> Create(SalaryProcessComponentNew dto)
        {
            SalaryProcessComponentDto data = new SalaryProcessComponentDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<SalaryProcessComponent> Update(SalaryProcessComponentDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<SalaryProcessComponent> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}