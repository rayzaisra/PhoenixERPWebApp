using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Area(PhoenixModule.GENERAL)]
    [Route(PhoenixModule.GENERAL_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.GENERAL)]
    public class TeamController : Controller
    {
        private ITeamServices _service;

        public TeamController(ITeamServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TeamDto> List(TeamSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet]
        public GeneralResponseList<vUserList> GetAllUser(UserlistSearch filter)
        {
            return _service.GetAllUser(filter);
        }


        [HttpGet("{id}")]
        public GeneralResponse<TeamWithUserList> GetUserList(string id)
        {
            return _service.GetTeamUsers(id); 
        }
        [HttpGet("{id}")]
        public GeneralResponse<TeamDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<Team> Create(TeamNew dto)
        {
            TeamDto data = new TeamDto();
            PropertyMapper.All(dto, data);
            if (!string.IsNullOrEmpty(dto.istemplate))
            {
                data.is_template = true;
            }
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<Team> Update(TeamDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<Team> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpPost, ValidateModel]
        public BaseGeneralResponse UpdateTeam([FromBody] TeamUsers dto)
        {
            BaseGeneralResponse retval = new BaseGeneralResponse();
            retval.Success = _service.manageteamusers(dto);
            return retval;
        }
        [HttpPost, ValidateModel]
        public BaseGeneralResponse UpdateJobTeam([FromBody] TeamJobUsers dto)
        {
            BaseGeneralResponse retval = new BaseGeneralResponse();
            retval.Success = _service.managejobteamusers(dto);
            return retval;
        }
    }
}