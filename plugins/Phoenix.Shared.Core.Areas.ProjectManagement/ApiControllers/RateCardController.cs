using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class RateCardController : Controller
    {
        private IRateCardServices _service;

        public RateCardController(IRateCardServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<RateCardDto> List(RateCardSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<RateCardDto> Get(string id)
        {
            return _service.GetRateCard(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<RateCardNew> Create([FromBody] RateCardNew dto)
        {
     
            return _service.CreateRateCard(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<RateCardDto> Update([FromBody] RateCardDto dto)
        {
            return _service.UpdateRateCard(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<RateCard> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}