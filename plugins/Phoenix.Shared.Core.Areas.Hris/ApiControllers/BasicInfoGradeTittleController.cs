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
    public class BasicInfoGradeTittleController : Controller
    {
        private IBasicInfoGradeTittleServices _service;

        public BasicInfoGradeTittleController(IBasicInfoGradeTittleServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<BasicInfoGradeTittleDto> List(BasicInfoGradeTittleSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BasicInfoGradeTittleDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<BasicInfoGradeTittle> Create(BasicInfoGradeTittleNew dto)
        {
            BasicInfoGradeTittleDto data = new BasicInfoGradeTittleDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<BasicInfoGradeTittle> Update(BasicInfoGradeTittleDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<BasicInfoGradeTittle> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}