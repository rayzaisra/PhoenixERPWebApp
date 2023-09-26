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
    public class CandidateRecruitmentEmploymentHistoryController : Controller
    {
        private ICandidateRecruitmentEmploymentHistoryServices _service;

        public CandidateRecruitmentEmploymentHistoryController(ICandidateRecruitmentEmploymentHistoryServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> List(CandidateRecruitmentEmploymentHistorySearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentEmploymentHistoryDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponseList<CandidateRecruitmentEmploymentHistoryDto> GetListWithIDCandidate(string id)
        {
            return _service.GetListWithIDCandidate(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitmentEmploymentHistory> Create(CandidateRecruitmentEmploymentHistoryNew dto)
        {
            CandidateRecruitmentEmploymentHistoryDto data = new CandidateRecruitmentEmploymentHistoryDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitmentEmploymentHistory> Update(CandidateRecruitmentEmploymentHistoryDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CandidateRecruitmentEmploymentHistory> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}