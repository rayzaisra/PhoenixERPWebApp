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
    public class THRSettingController : Controller
    {
        private ITHRSettingServices _service;

        public THRSettingController(ITHRSettingServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<THRSettingDto> List(THRSettingSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<THRSettingDto> Get(string id)
        {
            //return _service.GetByIDResponseDto(id);
            return _service.GetWithName(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<THRSetting> Create(THRSetting dto)
        {
            THRSettingDto data = new THRSettingDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<THRSetting> Update(THRSettingDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<THRSetting> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}