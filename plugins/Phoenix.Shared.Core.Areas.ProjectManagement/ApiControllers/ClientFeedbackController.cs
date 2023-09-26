using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class ClientFeedbackController : Controller
    {
        private IClientFeedbackServices _service;

        public ClientFeedbackController(IClientFeedbackServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<ClientFeedbackDto> List(ClientFeedbackSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<ClientFeedbackDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<ClientFeedback> Create(ClientFeedbackNew dto)
        {
            ClientFeedbackDto data = new ClientFeedbackDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<ClientFeedback> Update(ClientFeedbackDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<ClientFeedback> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }


        #region AddonService
        [HttpGet]
        public GeneralResponseList<ApprovalTypeDto> ListApprovalType()
        {
            return _service.GetApprovalType();
        }

        [HttpPost, ValidateModel]
        public GeneralResponse<ClientFeedbackDetailDto> GetDetailClientFeedBack(ClientFeedbackParameterDto parameter)
        {
            return _service.GetDetailClientFeedBack(parameter);
        }

        [HttpPost, ValidateModel]
        public GeneralResponse<ClientFeedbackResponseDto> SubmitClientFeedback(ClientFeedbackParameterDto parameter)
        {
            return _service.SubmitClientFeedback(parameter);
        }

        #endregion

    }
}