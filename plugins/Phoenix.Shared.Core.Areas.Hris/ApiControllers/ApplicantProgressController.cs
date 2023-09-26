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
    public class ApplicantProgressController : Controller
    {
        private IApplicantProgressServices _service;

        public ApplicantProgressController(IApplicantProgressServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<ApplicantProgressDto> List(ApplicantProgressSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<ApplicantProgressDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<ApplicantProgress> Create(ApplicantProgressNew dto)
        {
            ApplicantProgressDto data = new ApplicantProgressDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<ApplicantProgress> Update(ApplicantProgressDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<ApplicantProgress> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}