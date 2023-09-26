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
    public class EmployeeLoanController : Controller
    {
        private IEmployeeLoanServices _service;

        public EmployeeLoanController(IEmployeeLoanServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeLoanEditDto> List(EmployeeLoanSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeLoanEditDto> Get(string id)
        {
            //return _service.GetByIDResponseDto(id);
            //return _service.GetWithName(id);
            return _service.GetWithEdit(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<EmployeeLoan> Create(EmployeeLoan dto)
        {
            EmployeeLoanDto data = new EmployeeLoanDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeLoan> Update(EmployeeLoanDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeLoan> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}