using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class MppController : Controller
    {
        private IMppServices _service;

        public MppController(IMppServices service)
        {
            _service = service;
        }
        [HttpGet]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponseList<MppDto> List(MppSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponse<MppDto> GetWithDetil(string id)
        {
            return _service.GetWithDetil(id);
        }
        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Read)]
        public GeneralResponse<MppDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        [ActionRole(ActionMethod.Add)]
        public GeneralResponse<Mpp> Create(MppNew dto)
        {
            MppDto data = new MppDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }

        [HttpPost]
        [ValidateModel]
        [ActionRole(ActionMethod.Add)]
        public GeneralResponse<MppDto> CreateMppWithDetail(MppNew dto)
        {
            MppNew data = new MppNew();
            PropertyMapper.All(dto, data);
            return _service.CreateMppWithDetail(data);
        }
        [HttpPost, ValidateModel]
        [ActionRole(ActionMethod.Edit)]
        public GeneralResponse<Mpp> Update(MppDto dto)
        {
            return _service.UpdateAllDataMpp(dto);
        }
        [HttpPost, ValidateModel]
        [ActionRole(ActionMethod.Edit)]
        public GeneralResponse<Mpp> UpdateStatusMpp([FromBody]MppDto dto)
        {
            return _service.UpdateStatusMpp(dto);
        }

        [HttpDelete("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public GeneralResponse<Mpp> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

        [HttpGet("{id}")]
        [ActionRole(ActionMethod.Delete)]
        public GeneralResponse<Mpp> DeleteWithDetail(string id)
        {
            return _service.DeleteWithDetail(id);
        }

    }
}