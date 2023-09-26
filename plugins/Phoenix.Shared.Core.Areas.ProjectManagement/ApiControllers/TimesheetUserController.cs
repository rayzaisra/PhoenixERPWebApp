using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Data.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.ApiControllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    [Route("api/[controller]/[action]")]
    public class TimesheetUserController : Controller
    {
        private ITimesheetUserServices _service;

        public TimesheetUserController(ITimesheetUserServices service)
        {
            _service = service;
        }
        //[HttpGet]
        //public GeneralResponseList<TimesheetUserDto> List(TimesheetUserDto parameter)
        //{
        //    return _service.ListAll(parameter);
        //}

        //[HttpGet]
        //public GeneralResponseList<TimeSheetCreateDto> List(string data)
        //{
        //    return _service.ListAllTimeSheet(data);
        //}

        [HttpGet("{id}")]
        public GeneralResponseList<TimeSheetCreateDto> List(string id)
        {
            return _service.ListAllTimeSheet(id);
        }

        [HttpGet("{id}")]
        public async Task<string> ListTimeSheet(string id)
        {
            var data = await _service.ListTimeSheetUser(id,null);
            string result = JsonConvert.SerializeObject(data);
            return result;
        }
        [HttpPost]
        public async Task<string> ListTimeSheetMobile(string date, string userid)
        {
            var data = await _service.ListTimeSheetUser(date, userid);
            string result = JsonConvert.SerializeObject(data);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<string> GetTimeSheetDataById(string id)
        {
            var data = await _service.GetTimeSheetDataById(id);
            string result = JsonConvert.SerializeObject(data);
            return result;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<GeneralResponse<string>> SaveTimeSheet([FromBody] SaveTimeSheetDto dto)
        {
            return await _service.SaveTimeSheet(dto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<GeneralResponse<TimeSheetDto>> UpdateTimeSheet([FromBody] TimeSheetDto dto)
        {
            return await _service.UpdateTimeSheet(dto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<BaseGeneralResponse> SubmitTimeSheet([FromBody] SaveTimeSheetDto dto)
        {
            return await _service.SubmitTimeSheet(dto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<BaseGeneralResponse> Approval([FromBody] SaveTimeSheetDto dto)
        {
            return await _service.Approval(dto);
        }
        
        [HttpPost]
        [ValidateModel]
        public async Task<BaseGeneralResponse> ActivatedTimeSheet([FromBody] SaveTimeSheetDto dto)
        {
            return await _service.ActivatedTimeSheet(dto);
        }

        [HttpGet("{userid}")]
        public async Task<BaseGeneralResponse> CheckIsUserLocked(string userid)
        {
            return await _service.CheckIsUserLocked(userid);
        }

        [HttpGet]
        public GeneralResponseList<JobResultDto> ListAllJob(JobResultDto parameter)
        {
            return _service.ListAllJob(parameter);
        }

        //[HttpPost]
        //[ValidateModel]
        //public GeneralResponse<TimeSheetCreateDto> Create([FromBody] TimeSheetCreateDto dto)
        //{
        //    return _service.CreateTimeSheet(dto);
        //}

        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TimeSheetRow> Create([FromBody] List<TimeSheetCreateDto> dto)
        {
            return _service.CreateTableSheet(dto);
        }

        [HttpDelete("{id}")]
        public GeneralResponse<TimesheetUser> Delete(string id)
        {
            return _service.DeleteByIDResponse(id);
        }


        [HttpPost]
        [ValidateModel]
        public async Task<GeneralResponse<string>> DeleteNonJob([FromBody] TimeSheetDto dto)
        {
            return await _service.DeleteNonJob(dto);
        }



        [HttpGet]
        public async Task<string> approvalList()
        {
            var data = await _service.GetApprovalList();
            string result = JsonConvert.SerializeObject(data);
            return result;
        }


    }

}
