using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using System.Collections.Generic;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class TaskController : Controller
    {
        private ITaskServices _service;

        public TaskController(ITaskServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<TaskDto> List(TaskSearch parameter)
        {
            return _service.ListAll(parameter);
        }



        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse updatesubtaskuser(subTaskUsers data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            resp.Success = _service.manageSubTaskUsers(data);
            if (!resp.Success)
            {
                resp.Message = "failed to update user list";
            }
            return resp;
        }

        [HttpGet]
        public GeneralResponseList<CentralTaskList> ListByJobNumber(string JobNumber)
        {
            return _service.ListByJobNumber(JobNumber);
        }

        [HttpGet("{id}")]
        public GeneralResponseList<vTaskList> TaskFilter(string id)
        {
            return _service.ListFilter(id);
        }

        [HttpGet("{JobNumber}")]
        public GeneralResponseList<vTaskList> GetByJobNumber(string JobNumber)
        {
            return _service.GetByJobNumber(JobNumber);
        }
        [HttpGet("{id}")]
        public GeneralResponse<TaskDto> Get(string id)
        {
            return _service.GetDetail(id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponseList<Taskcommentlist> sendcomment([FromBody] Tasknewcomment data)
        {
            return _service.comment(data);
        }
        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse updateprogress([FromBody] Tasksubpercentage data)
        {
            return _service.updateprogress(data);
        }
        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse updatestatus([FromBody] Taskstatusset data)
        {
            return _service.updatestatus(data);
        }


        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse updateDumpstatus([FromBody] List<Taskstatusset> data)
        {
            return _service.updateDumpstatus(data);
        }

        [HttpPost("{jobnumber}")]
        public BaseGeneralResponse sendnotification(string jobnumber)
        {
            return _service.sendnotification(jobnumber);
        }

        [HttpGet("{task_id}")]
        public GeneralResponseList<Taskcommentlist> commentlist(string task_id)
        {
            return _service.commentlist(task_id);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TaskDto> Manage(TaskDto data)
        {
        
            return _service.Manage(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TaskNew> create(TaskNew data)
        {

            return _service.Create(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<TaskUpdate> update(TaskUpdate data)
        {

            return _service.Update(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<SubTaskNew> createsubtask(SubTaskNew data)
        {

            return _service.CreateSubtask(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<SubTaskUpdate> updatesubtask(SubTaskUpdate data)
        {

            return _service.UpdateSubtask(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<SubTaskUpdatedate> updatesubtaskdate([FromBody] SubTaskUpdatedate data)
        {

            return _service.UpdateSubtaskdate(data);
        }
        [HttpDelete("{id}")]
        public GeneralResponse<Task> DeleteTask(string id)
        {
            return _service.DeleteByIDResponse(id);
        }

    }
}