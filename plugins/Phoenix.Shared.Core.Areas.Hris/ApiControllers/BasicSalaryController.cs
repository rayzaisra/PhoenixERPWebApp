using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using System;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class BasicSalaryController : Controller
    {
        private IBasicSalaryServices _service;

        public BasicSalaryController(IBasicSalaryServices service)
        {
            _service = service;
        }

        [HttpGet]
        public GeneralResponseList<BasicSalaryDto> List(BasicSalarySearch parameter)
        {
            return _service.ListAll(parameter);
        }

        [HttpGet("{id}")]
        public GeneralResponse<BasicSalaryDto> Get(Guid id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<BasicSalary> Create(BasicSalaryNew dto)
        {
            BasicSalaryDto data = new BasicSalaryDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<BasicSalary> Update(BasicSalaryDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<BasicSalary> Delete(Guid id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}