using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class CompanyBrandController : Controller
    {
        private ICompanyBrandServices _service;

        public CompanyBrandController(ICompanyBrandServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<CompanyBrandList> List(CompanyBrandSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet]
        public GeneralResponseList<CompanyList> CompanyList(SearchParameter parameter)
        {
            return _service.CompanyListAll(parameter);
        }
        [HttpGet]
        public GeneralResponseList<AccountList> AccountList(SearchParameter parameter)
        {
            return _service.AccountList(parameter);
        }

        [HttpGet]
        public GeneralResponseList<LegalList> LegalList(SearchParameter parameter)
        {
            return _service.LegalList(parameter);
        }

        [HttpGet]
        public GeneralResponseList<AffilateList> AffiliateList(SearchParameter parameter)
        {
            return _service.AffilateList(parameter);
        }

        [HttpGet]
        public GeneralResponseList<DivisionList> DivisionList(SearchParameter parameter)
        {
            return _service.DivisionList(parameter);
        }

        [HttpGet("{id}")]
        public GeneralResponseList<MasterDepartmentList> DepartementList(string id)
        {
            return _service.DepartementList(id);
        }

        [HttpGet("{id}")]
        public GeneralResponse<CompanyBrandList> Get(string id)
        {
            return _service.GetBrand(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<CompanyBrandWithTeamList> Getwithteam(string id)
        {
            return _service.GetBrandWithTeam(id);
        }
        
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<CompanyBrand> Create(CompanyBrandNew dto)
        {
            CompanyBrandDto data = new CompanyBrandDto();
            PropertyMapper.All(dto, data);
            var retval = _service.CreateResponse(data);
            if (retval.Success)
            {
                _service.managebrandaccount(retval.Data.Id, dto.brand_account);
            }
            return retval;
 
        }

        [HttpPost]
        [ValidateModel]
        public GeneralResponse<BrandDetailDto> CreateBrand(BrandDetailDto dto)
        {
            return _service.CreateBrand(dto);

        }

        [HttpPost, ValidateModel]
        public BaseGeneralResponse UpdateTeam([FromBody] CompanyBrandTeam dto)
        {
            BaseGeneralResponse retval = new BaseGeneralResponse();
            retval.Success = _service.managebrandteam(dto);
            return retval;
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<CompanyBrand> Update(CompanyBrandDto dto)
        {
            var retval= _service.UpdateResponse(dto.Id, dto);
            if (retval.Success)
            {
                _service.managebrandaccount(dto.Id, dto.brand_account);
            }
            return retval;
        }
        [HttpDelete("{id}")]
        public GeneralResponse<CompanyBrand> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}