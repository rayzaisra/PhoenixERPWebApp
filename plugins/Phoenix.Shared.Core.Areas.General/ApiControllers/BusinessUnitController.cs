using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;
using System.Collections.Generic;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Area(PhoenixModule.GENERAL)]
    [Route(PhoenixModule.GENERAL_ROUTE_API)]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.GENERAL)]
    [AllowAnonymousRoleAttribute]
    public class BusinessUnitController : Controller
    {
        private IBusinessUnitServices _service;

        public BusinessUnitController(IBusinessUnitServices service)
        {
            _service = service;
        }

        [HttpGet]
        public GeneralResponseList<BusinessUnitDto> List(BusinessUnitSearch parameter)
        {
            return _service.ListFilter(parameter);
        }

        [HttpGet]
        public GeneralResponseList<BusinessUnitParent> ListWithParent(BusinessUnitSearch parameter)
        {
            return _service.ListWithParent(parameter);
        }
        [HttpGet]
        public GeneralResponseList<BusinessUnitKendoUIParent> ListWithChildKendoUI(BusinessUnitSearch parameter)
        {
            return _service.GetTreeKenoUiFormatData(parameter);
        }
        [HttpGet("{parent_id}")]
        public GeneralResponse<List<BusinessUnitTree>> Listtree(string parent_id)
        {
            return _service.ListTree(parent_id);
        }
        [HttpGet]
        public GeneralResponse<List<BusinessUnitTree>> Listtree()
        {
            return _service.ListTree(null);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BusinessUnitParent> rootparent(string id)
        {
            return _service.GetParent(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BusinessUnitDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<BusinessUnit> Create(BusinessUnitNew dto)
        {
            BusinessUnitDto data = new BusinessUnitDto();
            PropertyMapper.All(dto, data);
            return _service.CreateResponse(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<BusinessUnitDto> CreateParalel(BusinessUnitNew dto)
        {
            BusinessUnitNew data = new BusinessUnitNew();
            PropertyMapper.All(dto, data);
            return _service.CreateParalel(data);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<BusinessUnit> Update(BusinessUnitDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BusinessUnit> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<BusinessUnit> DeletePralel(string id)
        {
            return _service.DeletePralel(id);
        }

    }
}