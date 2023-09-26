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
    public class LoanCategoryController : Controller
    {
        private ILoanCategoryServices _service;

        public LoanCategoryController(ILoanCategoryServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<LoanCategoryDto> List(LoanCategorySearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<LoanCategoryDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<LoanCategory> Create(LoanCategoryNew dto)
        {
            LoanCategoryDto data = new LoanCategoryDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<LoanCategory> Update(LoanCategoryDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<LoanCategory> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

        [HttpGet]
        public GeneralResponseList<LoanCategoryDto> GetWithDetil(string id)
        {
            return _service.GetWithDetil(id);
        }
    }
}