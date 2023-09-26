using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class CandidateRecruitmentInterviewController : Controller
    {
        private ICandidateRecruitmentInterviewServices _service;

        public CandidateRecruitmentInterviewController(ICandidateRecruitmentInterviewServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentInterviewDto> List(CandidateRecruitmentInterviewSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentInterviewDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitmentInterview> Create(CandidateRecruitmentInterviewNew dto)
        {
            CandidateRecruitmentInterviewDto data = new CandidateRecruitmentInterviewDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitmentInterview> Update(CandidateRecruitmentInterviewDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CandidateRecruitmentInterview> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}