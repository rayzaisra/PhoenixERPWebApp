using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Parameters;
using System.Collections.Generic;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class PcaController : Controller
    {
        private IPcaServices _service;

        public PcaController(IPcaServices service)
        {
            _service = service;
        }

   

        [HttpGet]
        public GeneralResponseList<AccountDto> ListAccount(SearchParameter filter)
        {
            return _service.ListAccount(filter);
        }
        [HttpGet]
        public GeneralResponseList<PcaDto> List(PcaSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<PcaDto> Get(string id)
        {
            return _service.GetPca(id);
        }
 
        [HttpPost("{pca_id}")]
        [ValidateModel]
        public BaseGeneralResponse submitpce(string pca_id)
        {

            return _service.submittopce(pca_id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<PcaNew> Manage([FromBody] PcaNew dto)
        {

            return _service.CreatePca(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<PcaDto> Update([FromBody] PcaDto dto)
        {
            return _service.UpdatePca(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<Pca> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpPost]
        public GeneralResponse<PcaRfq> createRfq([FromBody]PcaRfq data)
        {

            return _service.CreateRfq(data);
        }

    

    }
}