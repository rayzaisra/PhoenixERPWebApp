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
    public class SubSalaryComponentController : Controller
    {
        private ISubSalaryComponentServices _service;

        public SubSalaryComponentController(ISubSalaryComponentServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<SubSalaryComponentDto> List(SubSalaryComponentSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<SubSalaryComponentDto> Get(string id)
        {
            //return _service.GetByIDResponseDto(id);
            return _service.GetWithName(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<SubSalaryComponent> Create(SubSalaryComponent dto)
        {
            SubSalaryComponentDto data = new SubSalaryComponentDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<SubSalaryComponent> Update(SubSalaryComponentDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<SubSalaryComponent> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}