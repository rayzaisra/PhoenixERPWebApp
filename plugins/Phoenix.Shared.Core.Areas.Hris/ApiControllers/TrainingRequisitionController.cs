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
    public class TrainingRequisitionController : Controller
    {
        private ITrainingRequisitionServices _service;

        public TrainingRequisitionController(ITrainingRequisitionServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TrainingRequisitionDto> List(TrainingRequisitionSearch parameter)
        {
            return _service.ListAll(parameter);
        }
        
        [HttpGet]
        public GeneralResponseList<TrainingSchedule> GetTrainingSchedule()
        {
            return _service.GetTrainingSchedule();
        }
        [HttpGet]
        public GeneralResponseList<TrainingBondingDto> GetTrainingBonding(TrainingRequisitionSearch parameter)
        {
            return _service.GetTrainingBonding(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TrainingRequisitionDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TrainingRequisitionDto> GetWithDetial(string id)
        {
            return _service.GetWithDetial(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TrainingRequisition> Create(TrainingRequisitionNew dto)
        {
            TrainingRequisitionDto data = new TrainingRequisitionDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TrainingRequisition> CreateWithDetail(TrainingRequisitionNew dto)
        {  
            return _service.CreateWithDetail(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TrainingRequisition> Update(TrainingRequisitionDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TrainingRequisition> UpdateDetial(TrainingRequisitionDto dto)
        {
            return _service.UpdateDetial(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TrainingRequisition> UpdateStatus([FromBody]TrainingRequisitionDto dto)
        {
            return _service.UpdateStatus(dto);
        }
        [HttpPost, ValidateModel]
        public GeneralResponse<TrainingRequisition> ProcessPaymentandAdministration([FromBody]TrainingRequisitionDto dto)
        {
            return _service.ProcessPaymentandAdministration(dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TrainingRequisition> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<TrainingRequisition> DeleteDetail(string id)
        {
            return _service.DeleteDetail(id);
        }

    }
}