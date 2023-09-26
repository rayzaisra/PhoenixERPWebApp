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
    public class TrainingRegistrationController : Controller
    {
        private ITrainingRegistrationServices _service;

        public TrainingRegistrationController(ITrainingRegistrationServices service)
        {
            _service = service;
        }
       
        [HttpGet("{id}")]
        public GeneralResponse<TrainingRequisitionDto> GetWithDetial(string id)
        {
            return _service.GetWithDetial(id);
        }
       

    }
}