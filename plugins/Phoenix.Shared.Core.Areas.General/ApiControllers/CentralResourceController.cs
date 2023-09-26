using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Area(PhoenixModule.GENERAL)]
    [Route(PhoenixModule.GENERAL_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.GENERAL)]
    public class CentralResourceController : Controller
    {
        private ICentralResourceServices _service;

        public CentralResourceController(ICentralResourceServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CentralResourceDto> List(CentralResourceSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CentralResourceDto> Get(string id)
        {
            return _service.GetByIDResponse(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CentralResource> Create(CentralResourceNew dto)
        {
            CentralResourceDto data = new CentralResourceDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CentralResource> Update(CentralResourceDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CentralResource> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}