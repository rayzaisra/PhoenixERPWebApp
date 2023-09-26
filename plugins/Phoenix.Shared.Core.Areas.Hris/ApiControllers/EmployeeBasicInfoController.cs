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
    public class EmployeeBasicInfoController : Controller
    {
        private IEmployeeBasicInfoServices _service;

        public EmployeeBasicInfoController(IEmployeeBasicInfoServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeBasicInfoDto> List(EmployeeBasicInfoSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeBasicInfoDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeBasicInfoDto> GetWithDetail(string id)
        {
            return _service.GetWithDetail(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeBasicInfoDto> GetWithEmployeeId(string id)
        {
            return _service.GetWithEmployeeId(id);
        }
        [HttpGet("{id}")]
        public GeneralResponseList<EmployeeBasicInfoDto> GetWithDivisionId(string id)
        {
            return _service.GetWithDivisionId(id);
        }
        
        [HttpPost]
        public GeneralResponse<EmployeeBasicInfoDto> Create([FromBody]EmployeeBasicInfoNew dto)
        {
            EmployeeBasicInfoNew data = new EmployeeBasicInfoNew();
            PropertyMapper.All(dto, data);
            return _service.CreateWithReference(data);
        }
        [HttpPost]
        public GeneralResponse<EmployeeBasicInfoDto> CreateEmployeeFromCandidate(EmployeeBasicInfoNew dto)
        {
            EmployeeBasicInfoNew data = new EmployeeBasicInfoNew();
            PropertyMapper.All(dto, data);
            return _service.CreateEmployeeFromCandidate(data);
        }
        [HttpPost]
        public GeneralResponse<EmployeeBasicInfoNew> CreateNewEmployeeBasic(EmployeeBasicInfoNew dto)
        {
            EmployeeBasicInfoNew data = new EmployeeBasicInfoNew();
            PropertyMapper.All(dto, data);
            return _service.CreateNewEmployeeBasic(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeBasicInfo> Update(EmployeeBasicInfoDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeBasicInfo> UpdateNewEmployeeBasic(EmployeeBasicInfoNew dto)
        {
            return _service.UpdateNewEmployeeBasic(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeBasicInfo> Delete(string id)
        {
            return _service.DeleteWithReference(id);
        }

    }
}