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
    public class EmployeeChecklistCategoryController : Controller
    {
        private IEmployeeChecklistCategoryServices _service;

        public EmployeeChecklistCategoryController(IEmployeeChecklistCategoryServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeChecklistCategoryDto> List(SearchEmployeeChecklistCategory parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeChecklistCategoryDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<EmployeeChecklistCategory> Create(EmployeeChecklistCategoryNew dto)
        {
            EmployeeChecklistCategoryDto data = new EmployeeChecklistCategoryDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeChecklistCategory> Update(EmployeeChecklistCategoryDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeChecklistCategory> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}