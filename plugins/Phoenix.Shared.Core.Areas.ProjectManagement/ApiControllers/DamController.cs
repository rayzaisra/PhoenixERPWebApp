using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;
using Phoenix.Shared.Core.Entities.ProjectManagement;
using System.Threading.Tasks;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class DamController : Controller
    {
        private IDamServices _service;

        public DamController(IDamServices service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public GeneralResponse<DamJob> GetByJob(string id)
        {
            return _service.GetListByJob(id);
        }
        [HttpGet("{id}")]
        public GeneralResponseList<DamTypeStatusList> GetDamTypelist(string id = null)
        {
            return _service.GetDamTypelist(id);
        }
        
        [HttpGet("{id}")]
        public GeneralResponse<DamDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<GeneralResponse<DamFileSharing>> AddShare([FromBody]DamFileSharingDto dto)
        {
            return await _service.AddShare(dto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<BaseGeneralResponse> MoveToMyDam([FromBody]DamFileSharingDto dto)
        {
            return await _service.MoveToMyDam(dto);
        }

        [HttpPost]
        [ValidateModel]
        public GeneralResponse<Dam> Create(DamNew dto)
        {
            DamDto data = new DamDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<Dam> Update(DamDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<Dam> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}