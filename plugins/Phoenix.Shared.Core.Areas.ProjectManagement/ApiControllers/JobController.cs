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
using Microsoft.Graph;
using Phoenix.Data.MsGraphExtention;
using System.Threading.Tasks;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]
    public class JobController : Controller
    {
        private IJobDetailServices _service;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        public JobController(IJobDetailServices service, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure)
        {
            _service = service;
            this._repoAzure = repoAzure;
        }
        [HttpGet]
        public GeneralResponseList<JobDetailList> List([FromQuery]JobDetailSearch parameter)
        {
            return _service.ListAll(parameter);
        }

        [HttpGet]
        public GeneralResponseList<JobDetailList> ListClientBrief([FromQuery]JobDetailSearch parameter)
        {
            return _service.ListInClientBrief(parameter);
        }

        [HttpGet]
        public GeneralResponseList<JobDetailList> ListComboBox([FromQuery]JobDetailSearch parameter)
        {
            return _service.ListAllComboBox(parameter);
        }

        [HttpGet]
        public GeneralResponseList<BrandListDto> BrandList(SearchParameter parameter)
        {
            return _service.BrandList(parameter);
        }

        [HttpGet]
        public GeneralResponseList<JobDetailDto> JobList(SearchParameter parameter)
        {
            return _service.JobList(parameter);
        }

        
        [HttpGet("{id}")]
        public GeneralResponseList<BrandListDto> BrandListByCompanyId(string id)
        {
            return _service.BrandListByCompanyId(id);
        }

        [HttpGet]
        public GeneralResponseList<MasterDepartmentList> ListDepartment(SearchParameter parameter)
        {
            return _service.DepartmentList(parameter);
        }
        [HttpGet]
        public GeneralResponseList<MainServiceList> ListMainService(SearchParameter parameter)
        {
            return _service.MainServiceList(parameter);
        }
        [HttpGet]
        public GeneralResponseList<JobDetailList> ListApproval(SearchParameter parameter)
        {
            return _service.ListApproval(parameter);
        }
        [HttpGet]
        //public GeneralResponseList<JobDetailList> ListOverdue(SearchParameter parameter)
        public GeneralResponseList<DashJob> ListOverdue(SearchParameter parameter)
        {
            //return _service.ListOverdue(parameter);
            return _service.DashListOverdue(parameter);
        }

        [HttpGet]
        public GeneralResponseList<JobDetailList> ListDueToday(SearchParameter parameter)
        {
            return _service.ListDueToday(parameter);
        }

        [HttpGet]
        public GeneralResponseList<DashJob> MyTask(SearchParameter parameter)
        {
            return _service.MyTask(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponseList<jTaskList> GetTaskList(string id)
        {
            return _service.GetTaskList(id);
        }

        [HttpGet]
        public GeneralResponseList<UserAcountList> Listpm(JobPMSearch parameter)
        {
            return _service.Listpm(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponseList<PeFileDto> GetListFile(string id)
        {
            return _service.GetListFile(id);
        }
        [HttpGet("{id}")]
        public BaseGeneralResponse DeleteFile(string id)
        {
            return _service.DeleteFile(id);
        }

        [HttpGet("{id}")]
        public BaseGeneralResponse CheckIsSubmitStatus(string id)
        {
            return _service.CheckIsSubmitStatus(id);
        }

        [HttpGet("{id}")]
        public GeneralResponse<JobDetailForm> Get(string id)
        {
            return _service.GetByIDDetail(id);
        }
        [HttpGet("{id}")]
        public GeneralResponse<JobDetailFormWithTask> GetWithTask(string id)
        {
            return _service.GetByIDDetailWithTask(id);
        }
        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse SetStatus(JobAssignStatus data)
        {
            return _service.SetStatus(data);
        }

        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse AssignPM(JobAssignPM data)
        {
            return _service.AssignPM(data);
        }
        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse AssignCentral(JobAssignCentral data)
        {
            return _service.AssignCentral(data);
        }
        [HttpPost]
        [ValidateModel]
        public BaseGeneralResponse AssignBoard(JobAssignBoard data)
        {
            return _service.AssignBoard(data);
        }
        [HttpPost]
        [ValidateModel]
        public GeneralResponse<JobDetailDto> Create(JobDetailNew dto)
        {
          
            var result = _service.CreateJob(dto);
            return result;
        }
        [HttpPost, ValidateModel]
        public BaseGeneralResponse Update(JobDetailUpdate dto)
        {
            return _service.UpdateJob(dto);
        }
        [HttpDelete("{id}")]
        //public GeneralResponse<JobDetail> Delete(string id)
        public GeneralResponse<JobDetailList> Delete(string id)
        {
            return _service.Delete(id);
        }

        [HttpGet("{id}")]
        public GeneralResponse<PeFileFolderDto> GetFolderPath(string id)
        {
            return _service.GetFolderPath(id);
        }
    }
}