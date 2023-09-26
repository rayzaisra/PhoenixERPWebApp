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
    public class EmployeeCampaignController : Controller
    {
        private IEmployeeCampaignServices _service;

        public EmployeeCampaignController(IEmployeeCampaignServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<EmployeeCampaignDto> List(EmployeeCampaignSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<EmployeeCampaignDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<EmployeeCampaign> Create(EmployeeCampaignNew dto)
        {
            EmployeeCampaignDto data = new EmployeeCampaignDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<EmployeeCampaign> Update(EmployeeCampaignDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<EmployeeCampaign> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}