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
    public class CandidateRecruitmentCommentController : Controller
    {
        private ICandidateRecruitmentCommentServices _service;

        public CandidateRecruitmentCommentController(ICandidateRecruitmentCommentServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentCommentDto> List(CandidateRecruitmentCommentSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentCommentDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        public GeneralResponse<CandidateRecruitmentComment> Create([FromBody]CandidateRecruitmentCommentNew dto)
        {
            CandidateRecruitmentCommentDto data = new CandidateRecruitmentCommentDto();
            if (string.IsNullOrEmpty(dto.employee_basic_info_id))
            {
                dto.employee_basic_info_id = Glosing.Instance.EmployeeId;
            }
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitmentComment> Update(CandidateRecruitmentCommentDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CandidateRecruitmentComment> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}