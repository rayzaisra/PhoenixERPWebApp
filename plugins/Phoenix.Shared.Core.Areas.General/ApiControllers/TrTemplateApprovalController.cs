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
    public class TrTemplateApprovalController : Controller
    {
        private ITrTemplateApprovalServices _service;

        public TrTemplateApprovalController(ITrTemplateApprovalServices service)
        {
            _service = service;
        }

        [HttpGet]
        public GeneralResponseList<TrTemplateApprovalDto> List(SearchTrTemplateApproval parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TrTemplateApprovalDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{RefId}")]
        public GeneralResponse<TrTemplateApprovalDto> GetByRefId(string RefId)
        {
            return _service.GetByRefId(RefId);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TrTemplateApproval> Create(TrTemplateApprovalNew dto)
        {
            TrTemplateApprovalDto data = new TrTemplateApprovalDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TrTemplateApproval> Update(TrTemplateApprovalDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TrTemplateApproval> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}