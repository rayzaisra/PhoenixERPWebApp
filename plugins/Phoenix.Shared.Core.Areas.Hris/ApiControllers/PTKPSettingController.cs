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
    public class PTKPSettingController : Controller
    {
        private IPTKPSettingServices _service;

        public PTKPSettingController(IPTKPSettingServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<PTKPSettingDto> List(PTKPSettingSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<PTKPSettingDto> Get(string id)
        {
            //return _service.GetByIDResponseDto(id);
            return _service.GetWithName(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<PTKPSetting> Create(PTKPSetting dto)
        {
            PTKPSettingDto data = new PTKPSettingDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<PTKPSetting> Update(PTKPSettingDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<PTKPSetting> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}