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
    public class HiringRequestController : Controller
    {
        private IHiringRequestServices _service;

        public HiringRequestController(IHiringRequestServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<HiringRequestDto> List(HiringRequestSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet]
        public GeneralResponseList<HiringRequestDto> ListAllWithDetial(HiringRequestSearch parameter)
        {
            return _service.ListAllWithDetial(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<HiringRequestDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<HiringRequestDto> GetWithDetil(string id)
        {
            return _service.GetWithDetil(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponseList<MppDetailDto> GetListHiringMppDetail([FromBody]HiringRequestGetMppDetail dto)
        {
            return _service.GetListHiringMppDetail(dto);
        }
        [HttpGet("{id}")]
        public GeneralResponse<HiringRequestDto> GetListHiringMppWithID(string id)
        {
            return _service.GetListHiringMppWithID(id);
        }
        [HttpGet("{code}")]
        public GeneralResponse<HiringRequestDto> GetListHiringMppWithCode(string code)
        {
            return _service.GetListHiringMppWithCode(code);
        }

        [HttpPost]
        [ValidateModel]
        public GeneralResponse<MppDetailDto> GetGradeDataInBusinessUnit([FromBody]HiringRequestGetMppDetail dto)
        {
            return _service.GetGradeDataInBusinessUnit(dto);
        }

        [HttpGet("{id}")]
        public GeneralResponse<HiringRequestDto> GetDataRequester(string id)
        {
            string groupid = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetGroupAccess(HttpContext);
            GeneralResponse<HiringRequestDto> data = new GeneralResponse<HiringRequestDto>();
            data = _service.GetDataRequester(id);
            data.Data.groupId = groupid;
            return data;
            //return _service.GetDataRequester(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<HiringRequest> Create(HiringRequestNew dto)
        {             
            return _service.CreateCustomDetail(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<HiringRequest> Update(HiringRequestDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<HiringRequest> UpdateStatusHiringRequest([FromBody]HiringRequestDto dto)
        {
            return _service.UpdateStatusHiringRequest(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<HiringRequest> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<HiringRequest> DeleteWithDetail(string id)
        {
            return _service.DeleteWithDetail(id);
        }
        
    }
}