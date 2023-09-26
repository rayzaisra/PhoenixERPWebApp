using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Services.Um;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class CandidateRecruitmentController : Controller
    {
        private ICandidateRecruitmentServices _service;
        private readonly IManageUserAppService _repo;

        public CandidateRecruitmentController(ICandidateRecruitmentServices service, IManageUserAppService repo)
        {
            _service = service;
            _repo = repo;
        }
        [HttpGet]
        public GeneralResponseList<CandidateRecruitmentDto> List(CandidateRecruitmentSearch parameter)
        {
        
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CandidateRecruitmentDto> Get(string id)
        {
            string groupid = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetGroupAccess(HttpContext);
            GeneralResponse<CandidateRecruitmentDto> data = new GeneralResponse<CandidateRecruitmentDto>();
            //data = _service.GetByIDResponseDto(id);

            //data.Data.cv_file = _service.DataFileMaster(data.Data.cv_file_master_id);
            //data.Data.photo_file = _service.DataFileMaster(data.Data.photo_file_master_id);
            //data.Data.portofolio_file = _service.DataFileMaster(data.Data.portofolio_file_master_id);
            data = _service.GetDataDetail(id);
            if (data != null)
            {
                data.Data.groupId = groupid;
            }
           

            


            return data;
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitment> Create(CandidateRecruitmentNew dto)
        {
            CandidateRecruitmentDto data = new CandidateRecruitmentDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CandidateRecruitment> CreateDetil(CandidateRecruitmentNew dto)
        {
            return _service.CreateDetil(dto);
        }

        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitment> Update(CandidateRecruitmentDto dto)
        {
            return _service.UpdateCustom(dto);
            //return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitment> UpdateCustom([FromBody]CandidateRecruitmentDto dto)
        {
            return _service.UpdateCustom(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitment> CheckApprovalCustom([FromBody]CandidateRecruitmentDto dto)
        {
            return _service.CheckApprovalCustom(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CandidateRecruitment> UpdateCustomWithContract([FromBody]CandidateRecruitmentDto dto)
        {
            return _service.UpdateCustomWithContract(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CandidateRecruitment> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpPost]
        public GeneralResponse<CandidateRecruitment> DeleteDetil([FromBody]CandidateRecruitmentDto dto)
        {
            return _service.DeleteDetil(dto);
        }

    }
}