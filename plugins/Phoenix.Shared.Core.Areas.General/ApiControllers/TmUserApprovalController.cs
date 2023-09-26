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
    public class TmUserApprovalController : Controller
    {
        private ITmUserApprovalServices _service;

        public TmUserApprovalController(ITmUserApprovalServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TmUserApprovalDto> List(SearchTmUserApproval parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TmUserApprovalDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TmUserApproval> Create(TmUserApprovalNew dto)
        {
            TmUserApprovalDto data = new TmUserApprovalDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TmUserApproval> Update(TmUserApprovalDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TmUserApproval> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}