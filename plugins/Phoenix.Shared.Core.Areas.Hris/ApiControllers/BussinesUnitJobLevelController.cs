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
    public class BussinesUnitJobLevelController : Controller
    {
        private IBussinesUnitJobLevelServices _service;

        public BussinesUnitJobLevelController(IBussinesUnitJobLevelServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<BussinesUnitJobLevelDto> List(BussinesUnitJobLevelSearch parameter)
        {
            return _service.ListAll(parameter);
        }

        [HttpGet]
        public GeneralResponseList<BussinesUnitJobLevelDto> ListSupervisor(BussinesUnitJobLevelSearch parameter)
        {
            return _service.ListSupervisor(parameter);
        }

        [HttpGet]
        public GeneralResponseList<BusinessUnitJobLevelChart> Chart(BussinesUnitJobLevelSearch parameter)
        {
            return _service.Chart(parameter);
        }
        [HttpGet]
        public GeneralResponseList<BussinesUnitJobLevelDto> ListDetail(BussinesUnitJobLevelSearch parameter)
        {
            return _service.ListAllDetail(parameter);
        }
        [HttpGet]
        public GeneralResponseList<BusinessUnitJobLevelParent> ListWithParentDetail(BussinesUnitJobLevelSearch parameter)
        {
            return _service.ListWithParentDetail(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BussinesUnitJobLevelDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BussinesUnitJobLevelDto> GetDetail(string id)
        {
            return _service.GetDetail(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<BusinessUnitJobLevel> Create(BussinesUnitJobLevelNew dto)
        {
            BussinesUnitJobLevelDto data = new BussinesUnitJobLevelDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<BusinessUnitJobLevel> Update(BussinesUnitJobLevelDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<BusinessUnitJobLevel> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}