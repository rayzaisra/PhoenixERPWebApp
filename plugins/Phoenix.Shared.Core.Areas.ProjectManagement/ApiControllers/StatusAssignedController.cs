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
    public class StatusAssignedController : Controller
    {
        private IStatusAssignedServices _service;

        public StatusAssignedController(IStatusAssignedServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<StatusAssignedDto> List(StatusAssignedSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<StatusAssignedDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<StatusAssigned> Create(StatusAssignedNew dto)
        {
            StatusAssignedDto data = new StatusAssignedDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<StatusAssigned> Update(StatusAssignedDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<StatusAssigned> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

        #region AddonService
        [HttpPost, ValidateModel]
        public GeneralResponse<SetStatusDetailDto> DetailJobNumber(string JobNumber)
        {
            return _service.GetDetailSetStatusByJobNumber(JobNumber);
        }

        [HttpPost, ValidateModel]
        public GeneralResponse<SetStatusResponseDto> SubmitSetStatus(SetStatusParameterDto parameter)
        {
            return _service.SubmitSetStatus(parameter);
        }
        #endregion

    }
}