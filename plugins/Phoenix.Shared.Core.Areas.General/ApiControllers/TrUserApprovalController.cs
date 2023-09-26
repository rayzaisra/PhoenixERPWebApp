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
    public class TrUserApprovalController : Controller
    {
        private ITrUserApprovalServices _service;

        public TrUserApprovalController(ITrUserApprovalServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TrUserApprovalDto> List(SearchTrUserApproval parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TrUserApprovalDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TrUserApproval> Create(TrUserApprovalNew dto)
        {
            TrUserApprovalDto data = new TrUserApprovalDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TrUserApproval> Update(TrUserApprovalDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TrUserApproval> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}