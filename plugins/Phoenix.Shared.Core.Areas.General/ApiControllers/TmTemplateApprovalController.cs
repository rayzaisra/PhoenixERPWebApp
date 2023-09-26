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
    public class TmTemplateApprovalController : Controller
    {
        private ITmTemplateApprovalServices _service;

        public TmTemplateApprovalController(ITmTemplateApprovalServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TmTemplateApprovalDto> List(SearchTmTemplateApproval parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TmTemplateApprovalDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TmTemplateApproval> Create(TmTemplateApprovalNew dto)
        {
            TmTemplateApprovalDto data = new TmTemplateApprovalDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TmTemplateApproval> Update(TmTemplateApprovalDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TmTemplateApproval> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}