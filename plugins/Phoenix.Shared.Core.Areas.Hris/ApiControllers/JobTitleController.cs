using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.Hris.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Helpers;
using System;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Api.Areas.Hris.Controllers
{
    [Area(PhoenixModule.HRIS)]
    [Route(PhoenixModule.HRIS_ROUTE_API)]
    //[JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.HRIS)]
    public class JobTitleController : Controller
    {
        private IJobTitleServices _service;

        public JobTitleController(IJobTitleServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<JobTitleDto> List(JobTitleSearch parameter)
        {
            return _service.ListAllFilter(parameter);
        }
        [HttpGet]
        public GeneralResponseList<JobTitleDto> ListTitleInsideBu(JobTitleSearch parameter)
        {
            string groupid = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetGroupAccess(HttpContext);
            parameter.groupId = groupid;
            GeneralResponseList<JobTitleDto> data = new GeneralResponseList<JobTitleDto>();
            data = _service.ListTitleInsideBu(parameter);
            return data;

            //return _service.ListTitleInsideBu(parameter);
        }
        [HttpGet]
        public GeneralResponseList<JobTitleDto> ListAllFilter(JobTitleSearch parameter)
        {
            return _service.ListAllFilter(parameter);
        }
        [HttpGet]
        public GeneralResponseList<BusinessUnitDto> ListFilter(BusinessUnitSearch parameter)
        {
            return _service.ListFilter(parameter);
        }
        [HttpGet]
        public GeneralResponseList<BusinessUnitParent> ListWithParent(BusinessUnitSearch parameter)
        {
            return _service.ListWithParent(parameter);
        }

        [HttpGet("{id}")]
        public GeneralResponse<JobTitleDto> Get(string id)
        {
            return _service.GetByIDResponseDto(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobTitle> Create(JobTitleNew dto)
        {
            JobTitleDto data = new JobTitleDto();
            PropertyMapper.All(dto, data);        
            return _service.CreateResponse(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobTitleDto> CreateJobTitleParalel(JobTitleNew dto)
        {
            JobTitleDto newInputData = new JobTitleDto();
            PropertyMapper.All(dto, newInputData);
            return _service.CreateJobTitleParalel(newInputData);

        }
        [HttpPost, ValidateModel]
        public GeneralResponse<JobTitle> Update(JobTitleDto dto)
        {
            return _service.UpdateResponse(dto.Id, dto);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<JobTitle> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}