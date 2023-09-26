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
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class CandidateRecruitmentEducationInfoController : Controller
    {
        private ICandidateRecruitmentEducationInfoServices _service;

        public CandidateRecruitmentEducationInfoController(ICandidateRecruitmentEducationInfoServices service)
        {
            _service = service;
        }

        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentEducationInfoDto> List(CandidateRecruitmentEducationInfoSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentEducationInfoDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentEducationInfoDto> GetDetail(string id)
        {
            return _service.GetDetail(id);
        }
        [HttpGet("{id}")]
        public GeneralResponseList<CandidateRecruitmentEducationInfoDto> GetListWithIDCandidate(string id)
        {
            return _service.GetListWithIDCandidate(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitmentEducationInfo> Create(CandidateRecruitmentEducationInfoNew dto)
        {
            CandidateRecruitmentEducationInfoDto data = new CandidateRecruitmentEducationInfoDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitmentEducationInfo> Update(CandidateRecruitmentEducationInfoDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentEducationInfo> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}