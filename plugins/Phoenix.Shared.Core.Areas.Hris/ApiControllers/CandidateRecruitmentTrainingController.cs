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
    public class CandidateRecruitmentTrainingController : Controller
    {
        private ICandidateRecruitmentTrainingServices _service;

        public CandidateRecruitmentTrainingController(ICandidateRecruitmentTrainingServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentTrainingDto> List(CandidateRecruitmentTrainingSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentTrainingDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitmentTraining> Create(CandidateRecruitmentTrainingNew dto)
        {
            CandidateRecruitmentTrainingDto data = new CandidateRecruitmentTrainingDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitmentTraining> Update(CandidateRecruitmentTrainingDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CandidateRecruitmentTraining> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}