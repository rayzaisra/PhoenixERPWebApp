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
    public class MppDetailController : Controller
    {
        private IMppDetailServices _service;

        public MppDetailController(IMppDetailServices service)
        {
            _service = service;
        }
        
        [HttpGet]
        public GeneralResponseList<MppDetailDto> List(MppDetailSearch parameter)
        {
            return _service.ListAll(parameter);
        }

        [HttpGet]
        public GeneralResponseList<FiscalYearDto> GetFiscalYear(MppDetailSearch filter)
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return _service.ListFiscalYear(filter, userId);
        }
        [HttpGet("{id}")]
        public GeneralResponse<MppDetailDto> Get(string id)
        {
            return _service.GetWithName(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<MppDetail> Create(MppDetailNew dto)
        {
            MppDetailDto data = new MppDetailDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        
        [HttpPost, ValidateModel]
        public GeneralResponse<MppDetail> Update(MppDetailDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<MppDetail> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}