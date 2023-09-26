using Microsoft.EntityFrameworkCore;
using Phoenix.Data;
using Phoenix.Data.Attributes;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Entities.ProjectManagement;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.Models.Um;
using Phoenix.Shared.Core.Areas.General.Dtos;
using System.IO;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IDamServices : IBaseService<DamDto, Dam, string>
    {
        GeneralResponse<DamJob> GetListByJob(string job_id);
        GeneralResponseList<DamTypeStatusList> GetDamTypelist(string id = null);
        Task<GeneralResponse<DamFileSharing>> AddShare(DamFileSharingDto dto);
        Task<BaseGeneralResponse> MoveToMyDam(DamFileSharingDto dto);
    }
    public partial class DamServices : BaseService<DamDto, Dam, string>, IDamServices
    {
        CommonHelper _commonHelper = new CommonHelper();
        IEFRepository<MyDam, string> _repoMyDam;
        IEFRepository<DamFileSharing, string> _repoDamFileSharing;
        readonly DataContext context;
        readonly IManageMenuService menuService;
        readonly GlobalFunctionApproval globalFcApproval;
        private readonly IManageTemplateApprovalService _repoTemplate;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        readonly StatusLogService log;
        public DamServices(
            IEFRepository<Dam, string> repo,
            IEFRepository<MyDam, string> repoMyDam,
            IEFRepository<DamFileSharing, string> repoDamFileSharing,
            IManageMenuService menuService,
            GlobalFunctionApproval globalFcApproval, 
            Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure,
            IManageTemplateApprovalService repoTemplate,
            StatusLogService log,
            DataContext context) : base(repo)
        {
            _repo = repo;
            _repoMyDam = repoMyDam;
            _repoDamFileSharing = repoDamFileSharing;
            this.context = context;
            this.menuService = menuService;
            this.globalFcApproval = globalFcApproval;
            _repoAzure = repoAzure;
            _repoTemplate = repoTemplate;
            this.log = log;
        }

        public async Task<BaseGeneralResponse> MoveToMyDam(DamFileSharingDto dto)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {
                var requestorId = context.GetEmployeeId();
                var requestorName = context.EmployeeBasicInfos.Where(x => x.Id == requestorId && !(x.IsDeleted ?? false)).Select(x => x.NameEmployee).FirstOrDefault();
                for (int i = 0; i < dto.FileShareId.Length; i++)
                {
                    string[] FileShare = dto.FileShareId[i].Split(new string[] { "::" }, StringSplitOptions.None);

                    var jobId = FileShare[0];
                    var fileType = FileShare[1];
                    var filePath = FileShare[2].Replace('*', '/');
                    var jobname = context.JobDetail.Where(x => x.Id == jobId).Select(x => x.JobName).FirstOrDefault();
                    var fileid = context.PeFiles.Where(x => x.parent_id == jobId && x.file_path.Contains(filePath) && !(x.is_deleted ?? false)).ToList();
                    if (fileid.Count > 0)
                    {
                        foreach (var itemFile in fileid)
                        {

                            var model = _repoAzure.GetOnlyOne().Result;
                            var graph = new MsGraph(model.ClientId, model.TenantId);
                            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
                            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                            string currentDirectory = Path.GetDirectoryName(itemFile.file_path);
                            var filename = Path.GetFileName(itemFile.file_path);
                            //var FinalFilename = jobname + "-" + filename;
                            var FinalFilename = jobId + "-" + filename;
                            var copyFile = await MsGraphExtension.MoveFileToMyDam(graphClient, currentDirectory, FinalFilename, filename);
                            
                            if (copyFile.Success)
                            {
                                MyDam myDamFileData = new MyDam();
                                myDamFileData.FileName = FinalFilename;
                                myDamFileData.FileSize = itemFile.file_size;
                                myDamFileData.FilePath = "PROJECTMANAGEMENT/DAM/" + FinalFilename;
                                myDamFileData.MimeType = itemFile.mimetype;
                                myDamFileData.upload_date = DateTime.Now;
                                myDamFileData.created_by = requestorId;
                                myDamFileData.FileType = itemFile.file_type;
                                myDamFileData.ParentId = itemFile.parent_id;
                                myDamFileData.is_active = true;
                                myDamFileData.is_deleted = false;

                                _repoMyDam.Create(myDamFileData);
                                _repoMyDam.SaveChanges();
                            }
                        }
                        resp.Message = "Data berhasil dipindahkan ke MyDam";
                    }
                    else
                    {
                        resp.Message = "File tidak ditemukan";
                    }

                    resp.Code = "200";
                    resp.Success = true;

                }
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<GeneralResponse<DamFileSharing>> AddShare(DamFileSharingDto dto)
        {
            GeneralResponse<DamFileSharing> resp = new GeneralResponse<DamFileSharing>();
            var isApprove = true;
            var employeeName = "";
            var requestorName = "";
            var jobOwner = "";
            try
            {
                foreach (var item in dto.ShareToEmployeeId)
                {
                    employeeName = context.EmployeeBasicInfos.Where(x => x.Id == item && !(x.IsDeleted ?? false)).Select(x => x.NameEmployee).FirstOrDefault();
                    var requestorId = context.GetEmployeeId();
                    requestorName = context.EmployeeBasicInfos.Where(x => x.Id == requestorId && !(x.IsDeleted ?? false)).Select(x => x.NameEmployee).FirstOrDefault();
                    if (employeeName != null)
                    {
                        for (int i = 0; i < dto.FileShareId.Length; i++)
                        {
                            string[] FileShare = dto.FileShareId[i].Split(new string[] { "::" }, StringSplitOptions.None);

                            var jobId = FileShare[0];
                            var fileType = FileShare[1];
                            var filePath = FileShare[2].Replace('*', '/');

                            var fileid = context.PeFiles.Where(x => x.parent_id == jobId && x.file_path.Contains(filePath) && !(x.is_deleted ?? false)).ToList();
                            if (fileid.Count > 0)
                            {
                                foreach (var itemFile in fileid)
                                {
                                    var obj = (from q in context.PmTeams
                                               join r in context.PmTeamMembers on q.Id equals r.TeamId
                                               join s in context.TmUserApps on r.ApplicationUserId equals s.Id
                                               where r.ActAs == "JO" && q.JobId == jobId && !(q.IsDeleted ?? false)
                                               select new TeamMemberDto()
                                               {
                                                   application_user_id = r.ApplicationUserId,
                                                   team_id = r.TeamId,
                                                   Id = s.EmployeeBasicInfoId
                                               }).FirstOrDefault();

                                    var model = _repoAzure.GetOnlyOne().Result;
                                    var graph = new MsGraph(model.ClientId, model.TenantId);
                                    var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
                                    var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                                    string currentDirectory = Path.GetDirectoryName(itemFile.file_path);
                                    var filename = Path.GetFileName(itemFile.file_path);
                                    var copyFile = await MsGraphExtension.CopyFile(graphClient, currentDirectory, filename, employeeName);

                                    if (copyFile.Success)
                                    {
                                        DamFileSharing data = new DamFileSharing();
                                        data.share_to = item;
                                        data.job_id = jobId;
                                        data.file_id = itemFile.Id;
                                        data.status = "Waiting Approval";
                                        data.is_active = true;
                                        data.file_path ="USER/" + employeeName + "/" + itemFile.file_path;
                                        _repoDamFileSharing.Create(data);
                                        _repoDamFileSharing.SaveChanges();

                                        var damFileId = context.DamFileSharings.Where(x => x.file_path == data.file_path && x.file_id == data.file_id && x.job_id == data.job_id && x.share_to == item).FirstOrDefault();

                                        var approval = await context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == itemFile.Id).ToListAsync();
                                        if (approval.Count > 0)
                                        {
                                            var userApproval = await context.TrUserApprovals.AsNoTracking().Where(x => approval.Any(y => y.Id == x.TrTempApprovalId)).ToListAsync();
                                            var conditionUserApproval = await context.TrConditionUserApprovals.AsNoTracking().Where(x => userApproval.Any(y => y.Id == x.TrUserApprovalId)).ToListAsync();

                                            context.TrConditionUserApprovals.RemoveRange(conditionUserApproval);
                                            context.TrUserApprovals.RemoveRange(userApproval);
                                            context.TrTemplateApprovals.RemoveRange(approval);
                                        }

                                        var dataMenu = await menuService.GetByUniqueName(Data.Models.MenuUnique.DamFileShared).ConfigureAwait(false);
                                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                                        var vm = new Data.Models.ViewModel.TransApprovalHrisVm()
                                        {
                                            IdTemplate = dataTempate.Id,
                                            MenuId = dataMenu.Id,
                                            RefId = damFileId.Id,
                                            DetailLink = $"{Data.Models.ApprovalLink.DamApproval}?Id={damFileId.Id}",
                                            NotificationType = Phoenix.Data.Models.NotificationTypeCode.DAMNotification,
                                            Module = Phoenix.Data.Models.NotificationModuleType.ModulePM
                                        };

                                        var subGroupId = PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                                        var divisionId = PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                                        var employeeId = PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                                        var dataTemp =  await _repoTemplate.GetById(vm.IdTemplate).ConfigureAwait(false);

                                        var modelTrTempApproval = new Phoenix.Data.Models.Um.TrTemplateApproval();
                                        modelTrTempApproval.TemplateName = dataTemp.TemplateName;
                                        modelTrTempApproval.IsNotifyByWeb = dataTemp.IsNotifyByWeb;
                                        modelTrTempApproval.IsNotifyByEmail = dataTemp.IsNotifyByEmail;
                                        modelTrTempApproval.DueDate = dataTemp.DueDate;
                                        modelTrTempApproval.DetailLink = vm.DetailLink;
                                        modelTrTempApproval.StatusApproved = StatusApproved.WaitingApproval;
                                        modelTrTempApproval.Reminder = dataTemp.Reminder;
                                        modelTrTempApproval.ApprovalCategory = dataTemp.ApprovalCategory;
                                        modelTrTempApproval.ApprovalType = dataTemp.ApprovalType;
                                        modelTrTempApproval.FormReqName = dataTemp.TemplateName;
                                        modelTrTempApproval.CreatedOn = DateTime.Now;
                                        modelTrTempApproval.CreatedBy = employeeId;
                                        modelTrTempApproval.DivisionId = divisionId;
                                        modelTrTempApproval.IsDeleted = false;
                                        modelTrTempApproval.RefId = vm.RefId;
                                        var tempId = Guid.NewGuid().ToString();
                                        modelTrTempApproval.Id = tempId;

                                        await context.PhoenixAddAsync(modelTrTempApproval);

                                        var vmUser = new Phoenix.Data.Models.Um.TrUserApproval();
                                        vmUser.EmployeeBasicInfoId = obj.Id;
                                        vmUser.IndexUser = 1;
                                        vmUser.TrTempApprovalId = tempId;
                                        vmUser.Id = Guid.NewGuid().ToString();
                                        vmUser.StatusApproved = StatusApproved.CurrentApproval;
                                        var notif = await context.TmUserApps.AsNoTracking().Where(x => x.EmployeeBasicInfoId == obj.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                                        var sendNotif = await globalFcApproval.AddNotification(vm.NotificationType, notif.Id, vm.Module, vm.RefId, dataTemp.IsNotifyByWeb, dataTemp.IsNotifyByEmail);
                                        await context.PhoenixAddAsync(vmUser);
                                        await context.SaveChangesAsync();
                                        await log.AddAsync(new Phoenix.Data.Models.StatusLog()
                                        {
                                            TransactionId = damFileId.Id,
                                            Status = Phoenix.Data.Models.StatusTransaction.StatusName(Phoenix.Data.Models.StatusTransaction.WaitingApproval),
                                            Description = "Request to share file : "+ filePath + " to : " + employeeName + " from : " + requestorName
                                        });
                                    }
                                }
                                resp.Message = "Data berhasil dishare";
                            }
                            else
                            {
                                resp.Message = "File tidak ditemukan";
                            }
                        }
                    }
                    else
                    {
                        resp.Message = "Data employee tidak ditemukan";
                    }
                    
                }
                resp.Code = "200";
                resp.Success = true;
                
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Success = false;
                resp.Data = null;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponseList<DamTypeStatusList> GetDamTypelist(string id = null)
        {
            GeneralResponseList<DamTypeStatusList> resp = new GeneralResponseList<DamTypeStatusList>();
            resp.Success = false;
            try
            {
                IQueryable<DamTypeStatusList> que = from tb1 in _repo.GetContext().Set<JobStatus>()
                                                    join tb2 in _repo.GetContext().Set<DamTypeStatus>() on tb1.Id equals tb2.job_status_id into tb2j
                                                    from tb2lj in tb2j.DefaultIfEmpty()
                                                    join tb3 in _repo.GetContext().Set<DamType>() on tb2lj.dam_type_id equals tb3.Id into tb3j
                                                    from tb3lj in tb3j.DefaultIfEmpty()
                                                    select new DamTypeStatusList()
                                                    {
                                                        dam_type_id = tb2lj.dam_type_id,
                                                        dam_type_name = tb3lj.name,
                                                        job_status_id = tb1.Id,
                                                        job_status_name = tb1.name,
                                                        is_default = tb1.is_default
                                                    };
                que = que.OrderBy(x => x.job_status_name).ThenBy(x => x.dam_type_name).Where(x => x.job_status_id != "closed");
                if (id == "1")
                {
                    que = que.Where(x => x.is_default == true);

                }
                else
                {
                    que = que.Where(x => x.is_default == true);
                    que = que.Where(x => x.job_status_id != "lostpitch");
                }
                resp.Rows = que.ToList();

                resp.Success = true;


            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<DamJob> GetListByJob(string id)
        {
            GeneralResponse<DamJob> resp = new GeneralResponse<DamJob>();
            resp.Success = false;
            try
            {
                IQueryable<DamJob> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                         join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                         join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                         join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                         join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                         join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                         join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                         from pmuser in lj1.DefaultIfEmpty()
                                         join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                         where tb1.Id == id
                                         select new DamJob()
                                         {
                                             Id = tb1.Id,
                                             job_number = tb1.job_number,
                                             job_name = tb1.job_name,
                                             deadline = tb1.deadline,
                                             final_delivery = tb1.final_delivery,
                                             client_brief_id = tb1.client_brief_id,
                                             campaign_name = tb2.campaign_name,
                                             job_status_id = tb1.job_status_id,
                                             job_description = tb1.job_description,
                                             company_name = tb4.company_name,
                                             company_id = tb4.Id,
                                             brand_name = tb3.brand_name,
                                             brand_id = tb3.Id,
                                             status_name = tb5.name,
                                             business_unit_name = tb6.unit_name,
                                             business_type = tb2.bussiness_type_id,
                                             business_type_name = (from x in _repo.GetContext().Set<BussinessType>()
                                                                   where x.Id == tb2.bussiness_type_id
                                                                   select x.propose_type).FirstOrDefault(),
                                             pm_id = tb1.pm_id,
                                             pm = pmuser.app_fullname,
                                             DamFile = (from a in _repo.GetContext().Set<Dam>()
                                                        where a.job_detail_id == tb1.Id
                                                        select new DamNew()
                                                        {
                                                            dam_type_id = a.dam_type_id,
                                                            file_id = a.file_id,
                                                            job_detail_id = a.job_detail_id,
                                                            notes = a.notes,
                                                            sub_task_id = a.sub_task_id,
                                                            task_id = a.task_id
                                                        }).ToList(),
                                             Centralresource = (from a in _repo.GetContext().Set<CentralResource>()
                                                                join b in _repo.GetContext().Set<ClientBriefCentralResource>() on a.Id equals b.central_resource_id
                                                                where b.job_detail_id == tb1.Id
                                                                select new Centralresourcelist()
                                                                {
                                                                    Id = b.central_resource_id,
                                                                    name = a.name
                                                                }).ToList(),

                                         };
                resp.Data = que.FirstOrDefault();
                if (resp.Data != null)
                {
                    resp.Success = true;
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;

        }
    }
}

