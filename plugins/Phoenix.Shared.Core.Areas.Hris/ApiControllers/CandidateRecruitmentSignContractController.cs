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
    public class CandidateRecruitmentSignContractController : Controller
    {
        private ICandidateRecruitmentSignContractServices _service;

        public CandidateRecruitmentSignContractController(ICandidateRecruitmentSignContractServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentSignContractDto> List(CandidateRecruitmentSignContractSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentSignContractDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitmentSignContract> Create([FromBody]CandidateRecruitmentSignContractNew dto)
        {
            CandidateRecruitmentSignContractDto data = new CandidateRecruitmentSignContractDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitmentSignContract> Update(CandidateRecruitmentSignContractDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CandidateRecruitmentSignContract> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}