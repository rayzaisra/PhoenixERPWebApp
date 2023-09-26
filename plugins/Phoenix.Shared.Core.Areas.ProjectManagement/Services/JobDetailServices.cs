using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data;
using Phoenix.Data.Models.Um;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.General.Helpers;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Entities.Finance;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using PhoenixWebApi.Services;
using server.Hubs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Phoenix.Data.MsGraphExtention;
using System.Text.RegularExpressions;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IJobDetailServices : IBaseService<JobDetailDto, JobDetail, string>
    {
        GeneralResponse<JobDetailForm> GetByIDDetail(string id);
        GeneralResponse<PeFileFolderDto> GetFolderPath(string parentId);
        GeneralResponse<JobDetailFormWithTask> GetByIDDetailWithTask(string id);
        GeneralResponseList<JobDetailList> ListAll(JobDetailSearch filter);
        GeneralResponseList<PeFileDto> GetListFile(string parentId);
        BaseGeneralResponse DeleteFile(string id);
        BaseGeneralResponse CheckIsSubmitStatus(string id);
        GeneralResponseList<JobDetailList> ListInClientBrief(JobDetailSearch filter);

        GeneralResponseList<JobDetailList> ListAllComboBox(JobDetailSearch filter);
        GeneralResponseList<JobDetailList> ListApproval(SearchParameter filter);
        GeneralResponseList<JobDetailList> ListDueToday(SearchParameter filter);
        GeneralResponseList<JobDetailList> ListOverdue(SearchParameter filter);
        GeneralResponseList<DashJob> MyTask(SearchParameter filter);
        GeneralResponseList<DashJob> DashListOverdue(SearchParameter filter);
        GeneralResponseList<UserAcountList> Listpm(JobPMSearch filter);
        GeneralResponse<JobDetailDto> CreateJob(JobDetailNew dto);
        BaseGeneralResponse UpdateJob(JobDetailUpdate dto);
        BaseGeneralResponse AssignPM(JobAssignPM dto);
        BaseGeneralResponse AssignCentral(JobAssignCentral dto);
        BaseGeneralResponse AssignBoard(JobAssignBoard dto);
        BaseGeneralResponse SetStatus(JobAssignStatus data);
        GeneralResponseList<jTaskList> GetTaskList(string id);

        GeneralResponseList<MasterDepartmentList> DepartmentList(SearchParameter filter);
        GeneralResponseList<MainServiceList> MainServiceList(SearchParameter filter);

        GeneralResponseList<BrandListDto> BrandList(SearchParameter filter);

        GeneralResponseList<BrandListDto> BrandListByCompanyId(string id);
        GeneralResponse<JobDetailList> Delete(string id);
        BaseGeneralResponse CreateFolder(PeFileFolderDto model);
        GeneralResponseList<JobDetailDto> JobList(SearchParameter filter);

    }
    public partial class JobDetailServices : BaseService<JobDetailDto, JobDetail, string>, IJobDetailServices
    {
        private string created_team_id = "";
        const string firstcreatedstatus = "unassigned";
        const int ColumnBase = 26;
        const int DigitMax = 7;
        const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        CommonHelper _commonHelper = new CommonHelper();
        IEFRepository<ClientBriefCentralResource, string> _repocbcr;
        IEFRepository<ClientBrief, string> _repocl;
        IEFRepository<Team, string> _repoteam;
        IEFRepository<TeamMember, string> _repoteammember;
        IEFRepository<Dam, string> _repodam;
        IEFRepository<spjobnumber, string> _repojobnumber;
        IEFRepository<JobUser, string> _repojobuser;
        IEFRepository<Affiliation, string> _repoaffliation;
        IEFRepository<BusinessUnit, string> _repobusinessunit;
        IEFRepository<PeFileFolder, string> _repoFileFolder;
        IEFRepository<PeFile, string> _repoFile;
        IEFRepository<DamFolderMaster, string> _repoDamFolderMaster;
        IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> _repoTemplateEmail;
        IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> _repoUserApp;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;

        protected Notify _notif;
        public JobDetailServices(IEFRepository<JobDetail, string> repo,
            IEFRepository<ClientBriefCentralResource, string> repocbcr,
            IEFRepository<ClientBrief, string> repocl,
            IEFRepository<Notification, string> reponotif,
            IEFRepository<NotificationType, string> repotype,
            IEFRepository<Dam, string> repodam,
            IEFRepository<spjobnumber, string> repojobnumber,
            IEFRepository<Team, string> repoteam,
            IEFRepository<JobUser, string> repojobuser,
            IEFRepository<TeamMember, string> repoteammember,
            IEFRepository<BusinessUnit, string> repobusinessunit,
            IEFRepository<Affiliation, string> repoaffliation,
            IEFRepository<PeFileFolder, string> repoFileFolder,
            IEFRepository<PeFile, string> repoFile,
            Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure,
            IEFRepository<DamFolderMaster, string> repoDamFolderMaster,
            IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> repoTemplateEmail,
            IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> repoUserApp,
        IHubContext<NotificationHub> hubContext) : base(repo)
        {
            _repo = repo;
            _repoaffliation = repoaffliation;
            _repobusinessunit = repobusinessunit;
            _repocl = repocl;
            _repocbcr = repocbcr;
            _repodam = repodam;
            _repoteam = repoteam;
            _repoteammember = repoteammember;
            _repojobnumber = repojobnumber;
            _repojobuser = repojobuser;
            _notif = new Notify(hubContext, reponotif, repotype, repoTemplateEmail, repoUserApp, repoAzure);
            _repoFileFolder = repoFileFolder;
            _repoAzure = repoAzure;
            _repoDamFolderMaster = repoDamFolderMaster;
            _repoFile = repoFile;
        }
        public BaseGeneralResponse SetStatus(JobAssignStatus data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();

            resp.Success = false;
            try
            {
                JobDetail nu = new JobDetail();
                nu = _repo.FindByID(data.job_detail_id);
                if (nu != null)
                {
                    nu.job_status_id = data.job_status_id;
                    _repo.Update(false, nu);
                    if (data.content != null)
                    {
                        foreach (DamStatus item in data.content)
                        {
                            if (item.file_id != null)
                            {
                                if (item.id != null)
                                {
                                    Dam damfile = _repodam.FindByID(item.id);
                                    if (damfile != null)
                                    {
                                        damfile.file_id = item.file_id;
                                        damfile.job_detail_id = data.job_detail_id;
                                        _repodam.Update(false, damfile);
                                    }
                                }
                                else
                                {
                                    Dam damfile = new Dam();
                                    damfile.dam_type_id = item.dam_type_id;
                                    damfile.job_detail_id = data.job_detail_id;
                                    damfile.file_id = item.file_id;
                                    _repodam.Create(false, damfile);
                                }
                            }

                        }
                        _repodam.SaveChanges();
                    }


                    _repo.SaveChanges();
                    resp.Success = true;
                }
                else
                {
                    resp.Message = "data not found";
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public BaseGeneralResponse AssignPM(JobAssignPM data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();

            resp.Success = false;
            try
            {
                JobDetail nu = new JobDetail();
                JobUser ns = new JobUser();
                nu = _repo.FindByID(data.Id);
                if (nu != null)
                {

                    nu.pm_id = data.user_id;
                    nu.job_status_id = "2";

                    ns.user_id = data.user_id;
                    ns.job_detail_id = data.Id;

                    _repo.Update(nu);
                    _repojobuser.Create(ns);
                    _notif.user(data.user_id, nu.Id, "pm.assignpm");
                    resp.Success = true;
                }
                else
                {
                    resp.Message = "data not found";
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public BaseGeneralResponse AssignCentral(JobAssignCentral data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();

            resp.Success = false;
            try
            {
                if (data.central_resource_id != null)
                {
                    foreach (string crid in data.central_resource_id)
                    {
                        ClientBriefCentralResource nu = new ClientBriefCentralResource();
                        nu.job_detail_id = data.Id;
                        nu.central_resource_id = crid;
                        _repocbcr.Create(false, nu);
                    }
                    var delall = _repo.Rawquery("delete from [pm].[client_brief_central_resource] where job_detail_id='" + data.Id + "'");
                    _repocbcr.SaveChanges();
                    resp.Success = true;
                }


            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public BaseGeneralResponse AssignBoard(JobAssignBoard data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();

            resp.Success = false;
            try
            {
                JobDetail nu = new JobDetail();
                nu = _repo.FindByID(data.Id);
                if (nu != null)
                {

                    resp.Success = true;
                }
                else
                {
                    resp.Message = "data not found";
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<JobDetailForm> GetByIDDetail(string id)
        {
            GeneralResponse<JobDetailForm> resp = new GeneralResponse<JobDetailForm>();
            resp.Success = false;
            try
            {
                IQueryable<JobDetailForm> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                join tb9 in _repo.GetContext().Set<JobType>() on tb1.job_type equals tb9.Id
                                                join tb10 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.created_by equals tb10.Id
                                                join tb11 in _repo.GetContext().Set<LegalEntity>() on tb1.legal_entity_id equals tb11.Id
                                                where tb1.Id == id
                                                select new JobDetailForm()
                                                {
                                                    Id = tb1.Id,
                                                    job_number = tb1.job_number,
                                                    job_type = tb1.job_type,
                                                    job_name = tb1.job_name,
                                                    deadline = tb1.deadline,
                                                    departement_id = tb1.business_unit_departement_id,
                                                    affliation_id = tb1.affiliation_id,
                                                    departement_name = "",
                                                    affliation_name = "",
                                                    legalEntityID = tb1.legal_entity_id,
                                                    legalEntityName = tb11.legal_entity_name,
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
                                                    pm_id = tb1.pm_id,
                                                    start_job = tb1.start_job,
                                                    finish_job = tb1.finish_job,
                                                    job_type_name = tb9.name,
                                                    created_by = tb10.name_employee,
                                                    TeamMember = (from a in _repo.GetContext().Set<Team>()
                                                                  join b in _repo.GetContext().Set<TeamMember>() on a.Id equals b.team_id
                                                                  join c in _repo.GetContext().Set<vUserList>() on b.application_user_id equals c.Id
                                                                  join d in _repo.GetContext().Set<Actor>() on b.act_as equals d.id
                                                                  where a.job_id == tb1.job_number
                                                                  select new UserList()
                                                                  {
                                                                      user_id = b.application_user_id,
                                                                      email = c.Email,
                                                                      username = c.AliasName,
                                                                      act_as = b.act_as,
                                                                      act_as_name = d.name
                                                                  }).Distinct().ToList(),
                                                    Centralresource = (from a in _repo.GetContext().Set<CentralResource>()
                                                                       join b in _repo.GetContext().Set<ClientBriefCentralResource>() on a.Id equals b.central_resource_id
                                                                       where b.job_detail_id == tb1.Id
                                                                       select new Centralresourcelist()
                                                                       {
                                                                           Id = b.central_resource_id,
                                                                           name = a.name
                                                                       }).ToList(),
                                                    pca = (from a in _repo.GetContext().Set<Pca>()
                                                           join b in _repo.GetContext().Set<MotherPca>() on a.mother_pca_id equals b.Id into lj1
                                                           from motherpca in lj1.DefaultIfEmpty()
                                                           where a.job_id == tb1.Id
                                                           select new PcaDto()
                                                           {
                                                               Id = a.Id,
                                                               code = a.code,
                                                               mother_pca_id = a.mother_pca_id,
                                                               mother_pca_name = motherpca.code,
                                                               total = a.total
                                                           }).FirstOrDefault(),
                                                    pce = (from a in _repo.GetContext().Set<PCE>()
                                                           where a.job_id == tb1.Id
                                                           select new PceDto()
                                                           {
                                                               Id = a.Id,
                                                               code = a.code,

                                                               total = a.total
                                                           }).FirstOrDefault()
                                                };

                resp.Data = que.FirstOrDefault();
                if (resp.Data != null)
                {
                    if (!string.IsNullOrEmpty(resp.Data.departement_id))
                        resp.Data.departement_name = _repobusinessunit.FindByID(resp.Data.departement_id).unit_name;
                    if (!string.IsNullOrEmpty(resp.Data.affliation_id))
                    {
                        resp.Data.affliation_name = _repoaffliation.FindByID(resp.Data.affliation_id).affiliation_name;
                    }


                    resp.Success = true;
                }

            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<BrandListDto> BrandList(SearchParameter filter)
        {

            GeneralResponseList<BrandListDto> resp = new GeneralResponseList<BrandListDto>();

            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BrandListDto> que = from tb1 in _repo.GetContext().Set<CompanyBrand>().Where(p => p.is_deleted == false)
                                               select new BrandListDto()
                                               {
                                                   id = tb1.Id,
                                                   brand_name = tb1.brand_name
                                               };
                if (filter.search != null)
                {
                    que = que.Where(w => w.brand_name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "name", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<JobDetailDto> JobList(SearchParameter filter)
        {

            GeneralResponseList<JobDetailDto> resp = new GeneralResponseList<JobDetailDto>();

            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailDto> que = from tb1 in _repo.GetContext().Set<JobDetail>().Where(p => p.is_deleted == false)
                                               select new JobDetailDto()
                                               {
                                                   Id = tb1.Id,
                                                   job_name = tb1.job_name
                                               };
                if (filter.search != null)
                {
                    que = que.Where(w => w.job_name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "name", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<BrandListDto> BrandListByCompanyId(string id)
        {

            GeneralResponseList<BrandListDto> resp = new GeneralResponseList<BrandListDto>();

            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BrandListDto> que = from tb1 in _repo.GetContext().Set<CompanyBrand>().Where(p => p.is_deleted == false && p.company_id == id)
                                               select new BrandListDto()
                                               {
                                                   id = tb1.Id,
                                                   brand_name = tb1.brand_name
                                               };

                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<MasterDepartmentList> DepartmentList(SearchParameter filter)
        {

            GeneralResponseList<MasterDepartmentList> resp = new GeneralResponseList<MasterDepartmentList>();

            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterDepartmentList> que = from tb1 in _repo.GetContext().Set<MasterDepartment>().Where(p => p.is_deleted == false)
                                                       select new MasterDepartmentList()
                                                       {
                                                           id = tb1.Id,
                                                           name = tb1.name
                                                       };
                if (filter.search != null)
                {
                    que = que.Where(w => w.name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "name", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }

        public GeneralResponseList<MainServiceList> MainServiceList(SearchParameter filter)
        {
            GeneralResponseList<MainServiceList> resp = new GeneralResponseList<MainServiceList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MainServiceList> que = from tb1 in _repo.GetContext().Set<MainserviceCategory>().Where(p => p.is_deleted == false && p.Id != "3335194186501656444" && p.is_default == true)
                                                  select new MainServiceList() { id = tb1.Id, name = tb1.name };
                if (filter.search != null)
                {
                    que = que.Where(w => w.name.Contains(filter.search));
                }
                resp.RecordsTotal = que.Count();

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "name", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<JobDetailFormWithTask> GetByIDDetailWithTask(string id)
        {
            GeneralResponse<JobDetailFormWithTask> resp = new GeneralResponse<JobDetailFormWithTask>();
            resp.Success = false;
            try
            {
                IQueryable<JobDetailFormWithTask> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                        join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                        //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                        join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                        join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                        join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                        join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                        join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                                        from pmuser in lj1.DefaultIfEmpty()
                                                        join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                        join tb9 in _repo.GetContext().Set<LegalEntity>() on tb1.legal_entity_id equals tb9.Id
                                                        where tb1.Id == id
                                                        select new JobDetailFormWithTask()
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
                                                            legalEntityID = tb1.legal_entity_id,
                                                            legalEntityName = tb9.legal_entity_name,
                                                            company_name = tb4.company_name,
                                                            company_id = tb4.Id,
                                                            brand_name = tb3.brand_name,
                                                            brand_id = tb3.Id,
                                                            status_name = tb5.name,
                                                            business_unit_name = tb6.unit_name,
                                                            pm_id = tb1.pm_id,
                                                            pm = pmuser.app_fullname,
                                                            Tasklist = (from a in _repo.GetContext().Set<Task>()
                                                                        where a.job_number == tb1.job_number
                                                                        select new TaskList()
                                                                        {
                                                                            id = a.Id,
                                                                            task_name = a.task_name,
                                                                            task_priority_id = a.task_priority_id,
                                                                            central_resources_id = a.central_resources_id,
                                                                            start_task = a.start_task,
                                                                            deadline_task = a.deadline_task,
                                                                            task_status_id = a.task_status_id,
                                                                            description = a.description
                                                                        }).ToList(),
                                                            Centralresource = (from a in _repo.GetContext().Set<CentralResource>()
                                                                               join b in _repo.GetContext().Set<ClientBriefCentralResource>() on a.Id equals b.central_resource_id
                                                                               where b.job_detail_id == tb1.Id
                                                                               select new Centralresourcelist()
                                                                               {
                                                                                   Id = b.central_resource_id,
                                                                                   name = a.name
                                                                               }).ToList(),
                                                            TeamMember = (from a in _repo.GetContext().Set<Team>()
                                                                          join b in _repo.GetContext().Set<TeamMember>() on a.Id equals b.team_id
                                                                          join c in _repo.GetContext().Set<vUserList>() on b.application_user_id equals c.Id
                                                                          join d in _repo.GetContext().Set<Actor>() on b.act_as equals d.id
                                                                          where a.job_id == tb1.job_number
                                                                          select new UserList()
                                                                          {
                                                                              user_id = b.application_user_id,
                                                                              email = c.Email,
                                                                              username = c.AliasName,
                                                                              act_as = b.act_as,
                                                                              act_as_name = d.name
                                                                          }).ToList()
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
        public GeneralResponseList<jTaskList> GetTaskList(string id)
        {
            GeneralResponseList<jTaskList> resp = new GeneralResponseList<jTaskList>();
            resp.Success = false;
            try
            {
                IQueryable<jTaskList> que = from tb1 in _repo.GetContext().Set<Task>()
                                            where tb1.job_number == id
                                            select new jTaskList()
                                            {
                                                id = tb1.Id,
                                                task_name = tb1.task_name,
                                                sub_task = (from a in _repo.GetContext().Set<SubTask>()
                                                            where a.task_id == tb1.Id
                                                            select new jSubTaskList()
                                                            {
                                                                id = a.Id,
                                                                subtask = a.sub_task,

                                                            }).ToList()

                                            };
                resp.Rows = que.ToList();

                resp.Success = true;


            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponseList<JobDetailList> ListApproval(SearchParameter filter)
        {

            GeneralResponseList<JobDetailList> resp = new GeneralResponseList<JobDetailList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailList> que = from tb1 in _repo.GetContext().Set<JobDetail>()

                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                                from pmuser in lj1.DefaultIfEmpty()
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                select new JobDetailList()
                                                {
                                                    Id = tb1.Id,
                                                    job_number = tb1.job_number,
                                                    job_name = tb1.job_name,
                                                    deadline = tb1.deadline,
                                                    start_job = tb1.start_job,
                                                    finish_job = tb1.finish_job,
                                                    final_delivery = tb1.final_delivery,
                                                    job_status_id = tb1.job_status_id,
                                                    job_description = tb1.job_description,
                                                    campaign_name = tb2.campaign_name,
                                                    company_name = tb4.company_name,
                                                    company_id = tb4.Id,
                                                    brand_name = tb3.brand_name,
                                                    brand_id = tb3.Id,
                                                    status_name = tb5.name,
                                                    business_unit_name = tb6.unit_name,
                                                    pm_id = tb1.pm_id,
                                                    pm = pmuser.app_fullname,

                                                };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<JobDetailList> ListDueToday(SearchParameter filter)
        {

            GeneralResponseList<JobDetailList> resp = new GeneralResponseList<JobDetailList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailList> que = from tb1 in _repo.GetContext().Set<JobDetail>()

                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                                from pmuser in lj1.DefaultIfEmpty()
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                where tb1.deadline == DateTime.Now
                                                select new JobDetailList()
                                                {
                                                    Id = tb1.Id,
                                                    job_number = tb1.job_number,
                                                    job_name = tb1.job_name,
                                                    deadline = tb1.deadline,
                                                    start_job = tb1.start_job,
                                                    finish_job = tb1.finish_job,
                                                    final_delivery = tb1.final_delivery,
                                                    job_status_id = tb1.job_status_id,
                                                    job_description = tb1.job_description,
                                                    campaign_name = tb2.campaign_name,
                                                    company_name = tb4.company_name,
                                                    company_id = tb4.Id,
                                                    brand_name = tb3.brand_name,
                                                    brand_id = tb3.Id,
                                                    status_name = tb5.name,
                                                    business_unit_name = tb6.unit_name,
                                                    pm_id = tb1.pm_id,
                                                    pm = pmuser.app_fullname,

                                                };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<DashJob> DashListOverdue(SearchParameter filter)
        {
            GeneralResponseList<DashJob> resp = new GeneralResponseList<DashJob>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<DashJob> que = from tb1 in _repo.GetContext().Set<JobDetail>().Where(p => DateTime.Now > p.deadline)

                                          select new DashJob()
                                          {
                                              Id = tb1.Id,
                                              job_name = tb1.job_name
                                          };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<DashJob> MyTask(SearchParameter filter)
        {
            GeneralResponseList<DashJob> resp = new GeneralResponseList<DashJob>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<DashJob> que = from tb1 in _repo.GetContext().Set<TeamMember>().Where(p => p.application_user_id == Glosing.Instance.Username)
                                          join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.team_id equals tb2.team_id
                                          orderby tb2.created_on ascending
                                          select new DashJob()
                                          {
                                              Id = tb2.Id,
                                              job_name = tb2.job_name
                                          };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<JobDetailList> ListOverdue(SearchParameter filter)
        {

            GeneralResponseList<JobDetailList> resp = new GeneralResponseList<JobDetailList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailList> que = from tb1 in _repo.GetContext().Set<JobDetail>()

                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                                from pmuser in lj1.DefaultIfEmpty()
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                where tb1.deadline <= DateTime.Now
                                                select new JobDetailList()
                                                {
                                                    Id = tb1.Id,
                                                    job_number = tb1.job_number,
                                                    job_name = tb1.job_name,
                                                    deadline = tb1.deadline,
                                                    start_job = tb1.start_job,
                                                    finish_job = tb1.finish_job,
                                                    final_delivery = tb1.final_delivery,
                                                    job_status_id = tb1.job_status_id,
                                                    job_description = tb1.job_description,
                                                    campaign_name = tb2.campaign_name,
                                                    company_name = tb4.company_name,
                                                    company_id = tb4.Id,
                                                    brand_name = tb3.brand_name,
                                                    brand_id = tb3.Id,
                                                    status_name = tb5.name,
                                                    business_unit_name = tb6.unit_name,
                                                    pm_id = tb1.pm_id,
                                                    pm = pmuser.app_fullname,

                                                };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<JobDetailList> ListAll(JobDetailSearch filter)
        {

            GeneralResponseList<JobDetailList> resp = new GeneralResponseList<JobDetailList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailList> que = (from tb1 in _repo.GetContext().Set<JobDetail>()
                                                     //where (filter.Search_client_brief_id == null || tb1.client_brief_id.Equals(filter.Search_client_brief_id))
                                                 join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                 //join team in _repo.GetContext().Set<Team>() on tb1.Id equals team.job_id
                                                 //join team_member in _repo.GetContext().Set<TeamMember>() on team.Id equals team_member.team_id
                                                 //where team_member.application_user_id.Trim().ToLower().Contains((filter.user_id.Trim().ToLower() ?? ""))
                                                 //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                 join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                 join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                 join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                 join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                 join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                                 from pmuser in lj1.DefaultIfEmpty()
                                                 join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                 select new JobDetailList()
                                                 {
                                                     Id = tb1.Id,
                                                     job_number = tb1.job_number,
                                                     job_name = tb1.job_name,
                                                     deadline = tb1.deadline,
                                                     created_on = tb1.created_on,
                                                     start_job = tb1.start_job,
                                                     finish_job = tb1.finish_job,
                                                     final_delivery = tb1.final_delivery,
                                                     job_status_id = tb1.job_status_id,
                                                     job_description = tb1.job_description,
                                                     campaign_name = tb2.campaign_name,
                                                     company_name = tb4.company_name,
                                                     company_id = tb4.Id,
                                                     brand_name = tb3.brand_name,
                                                     brand_id = tb3.Id,
                                                     status_name = tb5.name,
                                                     business_unit_name = tb6.unit_name,
                                                     pm_id = tb1.pm_id,
                                                     pm = pmuser.app_fullname,
                                                     ListTeamMember = (from a in _repo.GetContext().Set<Team>()
                                                                       join b in _repo.GetContext().Set<TeamMember>() on a.Id equals b.team_id
                                                                       where a.job_id == tb1.Id
                                                                       select b.application_user_id
                                                                         ).ToList()
                                                 }).OrderByDescending(x => x.created_on);

                if (filter.group_id != Phoenix.Data.Models.GroupAcess.SuperAdmin)
                {
                    que = que.Where(x => x.ListTeamMember.Contains((filter.user_id ?? "")));
                }


                resp.RecordsTotal = que.Count();
                if (filter.Search_client_brief_id != null)
                {
                    //que = que.Where(x=>x.client_brief_id==filter.Search_client_brief_id);
                }
                if (filter.search != null)
                {
                    que = que.Where(w => w.job_name.Contains(filter.search) || w.Id.Contains(filter.search));
                }
                que = q.filterContains(que, "company_name", filter.Search_company_name);
                que = q.filterEquals(que, "company_id", filter.Search_company_id);
                que = q.filterContains(que, "job_name", filter.Search_job_name);
                que = q.filterContains(que, "brand_name", filter.Search_brand_name);
                que = q.filterContains(que, "job_number", filter.Search_job_number);
                que = q.filterContains(que, "job_status_detail", filter.Search_job_status_detail);
                que = q.filterEquals(que, "job_status_id", filter.Search_job_status_id);
                que = q.filterEquals(que, "pm_id", filter.Search_pm_id);
                que = q.filterContains(que, "campaign_name", filter.Search_client_brief_id);



                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<JobDetailList> ListInClientBrief(JobDetailSearch filter)
        {

            GeneralResponseList<JobDetailList> resp = new GeneralResponseList<JobDetailList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailList> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                    //where (filter.Search_client_brief_id == null || tb1.client_brief_id.Equals(filter.Search_client_brief_id))
                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id into lj1
                                                from pmuser in lj1.DefaultIfEmpty()
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                join tb9 in _repo.GetContext().Set<JobType>() on tb1.job_type equals tb9.Id
                                                join tb10 in _repo.GetContext().Set<LegalEntity>() on tb1.legal_entity_id equals tb10.Id
                                                join tb11 in _repo.GetContext().Set<MainserviceCategory>() on tb1.main_service_category equals tb11.Id
                                                join tb12 in _repo.GetContext().Set<BusinessUnit>() on tb1.business_unit_departement_id equals tb12.Id into lj2
                                                from business in lj2.DefaultIfEmpty()
                                                join tb13 in _repo.GetContext().Set<Team>() on tb1.team_id equals tb13.Id
                                                select new JobDetailList()
                                                {
                                                    Id = tb1.Id,
                                                    job_number = tb1.job_number,
                                                    job_name = tb1.job_name,
                                                    deadline = tb1.deadline,
                                                    job_type = tb1.job_type,
                                                    job_type_name = tb9.name,
                                                    team_id = tb1.team_id,
                                                    team_name = tb13.team_name,
                                                    legal_entity_id = tb1.legal_entity_id,
                                                    legal_entity_name = tb10.legal_entity_name,
                                                    business_unit_departement_id = tb1.business_unit_departement_id,
                                                    business_unit_departement_name = business.unit_name,
                                                    main_service_category = tb1.main_service_category,
                                                    main_service_category_name = tb11.name,
                                                    start_job = tb1.start_job,
                                                    finish_job = tb1.finish_job,
                                                    final_delivery = tb1.final_delivery,
                                                    job_status_id = tb1.job_status_id,
                                                    job_description = tb1.job_description,
                                                    campaign_name = tb2.campaign_name,
                                                    company_name = tb4.company_name,
                                                    company_id = tb4.Id,
                                                    brand_name = tb3.brand_name,
                                                    brand_id = tb3.Id,
                                                    status_name = tb5.name,
                                                    business_unit_name = tb6.unit_name,
                                                    pm_id = tb1.pm_id,
                                                    pm = pmuser.app_fullname,
                                                    client_brief_id = tb1.client_brief_id
                                                };

                resp.RecordsTotal = que.Count();
                if (filter.Search_client_brief_id != null)
                {
                    //que = que.Where(x=>x.client_brief_id==filter.Search_client_brief_id);
                }
                if (filter.search != null)
                {
                    que = que.Where(w => w.job_name.Contains(filter.search) || w.Id.Contains(filter.search));
                }
                que = q.filterContains(que, "company_name", filter.Search_company_name);
                que = q.filterEquals(que, "company_id", filter.Search_company_id);
                que = q.filterContains(que, "job_name", filter.Search_job_name);
                que = q.filterContains(que, "brand_name", filter.Search_brand_name);
                que = q.filterContains(que, "job_number", filter.Search_job_number);
                que = q.filterContains(que, "job_status_detail", filter.Search_job_status_detail);
                que = q.filterEquals(que, "job_status_id", filter.Search_job_status_id);
                que = q.filterEquals(que, "pm_id", filter.Search_pm_id);
                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);


                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<JobDetailList> ListAllComboBox(JobDetailSearch filter)
        {

            GeneralResponseList<JobDetailList> resp = new GeneralResponseList<JobDetailList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobDetailList> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                where (filter.Search_client_brief_id == null || tb1.client_brief_id.Equals(filter.Search_client_brief_id))
                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                join team in _repo.GetContext().Set<Team>() on tb1.Id equals team.job_id
                                                join team_member in _repo.GetContext().Set<TeamMember>() on team.Id equals team_member.team_id
                                                where team_member.application_user_id.Trim().ToLower().Contains((filter.user_id.Trim().ToLower() ?? ""))
                                                //join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb2.external_brand_id equals tb3.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb7 in _repo.GetContext().Set<ApplicationUser>() on tb1.pm_id equals tb7.Id
                                                into lj1
                                                from pmuser in lj1.DefaultIfEmpty()
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id
                                                select new JobDetailList()
                                                {
                                                    Id = tb1.Id,
                                                    job_number = tb1.job_number,
                                                    job_name = tb1.job_name,
                                                    deadline = tb1.deadline,

                                                    start_job = tb1.start_job,
                                                    finish_job = tb1.finish_job,
                                                    final_delivery = tb1.final_delivery,
                                                    job_status_id = tb1.job_status_id,
                                                    job_description = tb1.job_description,
                                                    campaign_name = tb2.campaign_name,
                                                    company_name = tb4.company_name,
                                                    company_id = tb4.Id,
                                                    brand_name = tb3.brand_name,
                                                    brand_id = tb3.Id,
                                                    status_name = tb5.name,
                                                    business_unit_name = tb6.unit_name,
                                                    pm_id = tb1.pm_id,
                                                    pm = pmuser.app_fullname,

                                                };

                resp.RecordsTotal = que.Count();
                if (filter.Search_client_brief_id != null)
                {
                    //que = que.Where(x=>x.client_brief_id==filter.Search_client_brief_id);
                }
                if (filter.search != null)
                {
                    que = que.Where(w => w.job_name.Contains(filter.search) || w.Id.Contains(filter.search));
                }
                que = q.filterContains(que, "company_name", filter.Search_company_name);
                que = q.filterEquals(que, "company_id", filter.Search_company_id);
                que = q.filterContains(que, "job_name", filter.Search_job_name);
                que = q.filterContains(que, "brand_name", filter.Search_brand_name);
                que = q.filterContains(que, "job_number", filter.Search_job_number);
                que = q.filterContains(que, "job_status_detail", filter.Search_job_status_detail);
                que = q.filterEquals(que, "job_status_id", filter.Search_job_status_id);
                que = q.filterEquals(que, "pm_id", filter.Search_pm_id);



                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                //que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public GeneralResponseList<UserAcountList> Listpm(JobPMSearch filter)
        {
            GeneralResponseList<UserAcountList> resp = new GeneralResponseList<UserAcountList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<UserAcountList> que = from tb1 in _repo.GetContext().Set<AccountManagement>()
                                                 join tb2 in _repo.GetContext().Set<ApplicationUser>() on tb1.user_id equals tb2.Id
                                                 select new UserAcountList()
                                                 {
                                                     Id = tb2.Id,
                                                     user_id = tb1.user_id,
                                                     app_username = tb2.app_username,
                                                     app_fullname = tb2.app_fullname
                                                 };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "app_fullname", filter.search);



                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public static string itvar(int index)
        {
            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= ColumnBase)
                return Digits[index - 1].ToString();

            var sb = new StringBuilder().Append(' ', DigitMax);
            var current = index;
            var offset = DigitMax;
            while (current > 0)
            {
                sb[--offset] = Digits[--current % ColumnBase];
                current /= ColumnBase;
            }
            return sb.ToString(offset, DigitMax - offset);
        }
        bool copyTeam(string id, string Job_id, string Job_name)
        {
            #region old
            bool retval = false;
            Team fromteam = _repoteam.FindByID(id);
            ExpressionParameter<Team> fparam = new ExpressionParameter<Team>();
            fparam.where = w => w.job_id.Equals(Job_id);
            Team toteam = _repoteam.FindFirstOrDefault(fparam);
            if (fromteam != null)
            {
                ExpressionParameter<TeamMember> fparamtm = new ExpressionParameter<TeamMember>();
                fparamtm.where = w => w.team_id.Equals(fromteam.Id);
                IList<TeamMember> copy_member = _repoteammember.FindAll(fparamtm);

                var NewTeamMember = _repo.GetContext().Set<TeamMember>().Where(x => x.team_id == fromteam.Id && x.application_user_id == Glosing.Instance.Username).ToList();

                if (NewTeamMember.Count == 0)
                {
                    TeamMember newData = new TeamMember();
                    newData.application_user_id = Glosing.Instance.Username;
                    newData.team_id = fromteam.Id;
                    newData.is_deleted = false;
                    newData.is_active = true;
                    newData.owner_id = Glosing.Instance.Username;
                    newData.act_as = "JO";

                    copy_member.Add(newData);
                }

                if (toteam != null)
                {
                    var delall = _repo.Rawquery("delete from [pm].[pm_team_member] where team_id='" + toteam.Id + "'");
                    copy_member.ToList().ForEach(s => s.team_id = toteam.Id);

                    _repoteammember.CreateAll(copy_member.ToArray());
                    retval = true;
                }
                else
                {
                    Team newteam = new Team();
                    PropertyMapper.All(fromteam, newteam);
                    newteam.team_name = "Team " + Job_name;
                    newteam.job_id = Job_id;
                    newteam.Id = null;
                    newteam.is_template = false;
                    _repoteam.Create(newteam);
                    created_team_id = newteam.Id;
                    copy_member.ToList().ForEach(s =>
                    {
                        s.team_id = newteam.Id;
                        s.Id = null;
                    });

                    _repoteammember.CreateAll(copy_member.ToArray());
                    retval = true;
                }
            }
            else
            {
                retval = false;
            }


            return retval;
            #endregion
            #region new
            //bool retval = false;
            //Team fromteam = _repoteam.FindByID(id);
            //ExpressionParameter<Team> fparam = new ExpressionParameter<Team>();
            //fparam.where = w => w.job_id.Equals(Job_id);
            //Team toteam = _repoteam.FindFirstOrDefault(fparam);

            //ExpressionParameter<TeamMember> fparamtm = new ExpressionParameter<TeamMember>();
            //fparamtm.where = w => w.team_id.Equals(fromteam.Id);
            //IList<TeamMember> copy_member = _repoteammember.FindAll(fparamtm);

            //var NewTeamMember = _repo.GetContext().Set<TeamMember>().Where(x => x.team_id == fromteam.Id && x.application_user_id == Glosing.Instance.Username).ToList();

            //if (NewTeamMember.Count == 0)
            //{
            //    TeamMember newData = new TeamMember();
            //    newData.application_user_id = Glosing.Instance.Username;
            //    newData.team_id = fromteam.Id;
            //    newData.is_deleted = false;
            //    newData.is_active = true;
            //    newData.owner_id = Glosing.Instance.Username;
            //    newData.act_as = "JO";

            //    copy_member.Add(newData);
            //}


            //if (fromteam != null)
            //{
            //    var delall = _repo.Rawquery("delete from [pm].[pm_team_member] where team_id='" + id + "'");
            //    copy_member.ToList().ForEach(s => s.team_id = id);
            //    fromteam.job_id = Job_id;
            //    _repoteam.Update(fromteam);
            //    _repoteammember.CreateAll(copy_member.ToArray());
            //    retval = true;
            //}
            //else
            //{
            //    Team newteam = new Team();
            //    PropertyMapper.All(fromteam, newteam);
            //    newteam.team_name = "Team " + Job_name;
            //    newteam.job_id = Job_id;
            //    newteam.Id = null;

            //    _repoteam.Create(newteam);
            //    created_team_id = newteam.Id;
            //    copy_member.ToList().ForEach(s =>
            //    {
            //        s.team_id = newteam.Id;
            //        s.Id = null;
            //    });

            //    _repoteammember.CreateAll(copy_member.ToArray());
            //    retval = true;
            //}


            //return retval;
            #endregion
        }
        public GeneralResponse<JobDetailDto> CreateJob(JobDetailNew data)
        {
            GeneralResponse<JobDetailDto> resp = new GeneralResponse<JobDetailDto>();
            JobDetailDto dataDto = new JobDetailDto();
            resp.Success = false;
            try
            {
                ClientBrief clbrief = _repocl.FindByID(data.client_brief_id);

                DateTime dt1 = data.start_job ?? DateTime.Now.AddDays(7);
                DateTime dt2 = data.deadline ?? DateTime.Now.AddDays(15);
                DateTime dt3 = data.final_delivery ?? DateTime.Now.AddMonths(1);
                DateTime dt4 = clbrief.start_campaign ?? DateTime.Now;
                int resultFinalDelivery = DateTime.Compare(dt3, dt2);
                int resultDeadline = DateTime.Compare(dt2, dt1);
                int resultCampaign = DateTime.Compare(dt1, dt4);

                if (resultCampaign < 0)
                {
                    resp.Success = false;
                    resp.Message = "Start Job is older than start campaign date";
                }
                else
                {
                    if (resultDeadline < 0)
                    {
                        resp.Success = false;
                        resp.Message = "Deadline date is older than Start Job date";
                    }
                    else
                    {
                        if (resultFinalDelivery < 0)
                        {
                            resp.Success = false;
                            resp.Message = "Final delivery is older than deadline date";
                        }
                        else
                        {
                            if (clbrief != null)
                            {
                                string jobid = "";
                                var refid = new SqlParameter("@refid", data.client_brief_id);
                                var result = _repojobnumber.GetEntities().FromSql("[pm].[sgGetJobCount] @refid", refid).FirstOrDefault();

                                if (result != null)
                                {
                                    jobid = data.client_brief_id + result.Id;   //itvar(int.Parse(result.Id));
                                }
                                JobDetail existjob = (from tb1 in _repo.GetContext().Set<JobDetail>()
                                                      where tb1.job_name == data.job_name && tb1.client_brief_id == data.client_brief_id
                                                      select new JobDetail()).FirstOrDefault();

                                if (existjob == null)
                                {
                                    JobDetail nu = new JobDetail();
                                    data.job_number = jobid;

                                    PropertyMapper.All(data, nu);
                                    var companybrand = _repo.GetContext().Set<CompanyBrand>().Where(p => p.Id == data.brand_id).FirstOrDefault();
                                    var affiliationid = _repo.GetContext().Set<BusinessUnit>().Where(p => p.Id == data.business_unit_division_id).FirstOrDefault().affiliation_id;


                                    nu.Id = jobid;
                                    nu.job_status_detail = "";
                                    nu.job_status_id = firstcreatedstatus;
                                    nu.legal_entity_id = data.legal_entity_id;
                                    nu.business_unit_departement_id = data.business_unit_departement_id;
                                    nu.business_unit_division_id = data.business_unit_division_id;
                                    nu.main_service_category = data.main_service_category;
                                    nu.brand_id = data.brand_id;
                                    nu.company_id = companybrand.company_id;
                                    nu.affiliation_id = affiliationid;

                                    if (copyTeam(data.team_id, jobid, data.job_name))
                                    {
                                        //nu.team_id = created_team_id;
                                        //nu.team_id = created_team_id;
                                        nu.team_id = data.team_id;
                                        _repo.Create(false, nu);
                                        _repo.SaveChanges();

                                        var listOfTeam = _repo.GetContext().Set<TeamMember>().Where(p => p.team_id == nu.team_id).ToList();

                                        foreach (var member in listOfTeam)
                                        {
                                            _notif.createNotification(member.application_user_id, jobid, "pm", "jobdetail", data.job_name);
                                            //_notif.addNotification("pm.job_detail.notification", member.application_user_id, "", data.job_number);
                                            //_notify.AddNotification("hr.doa.need-approval", data.AssignTo, "HRIS");
                                            //System.Threading.Tasks.Task.Run(() =>  _notif.AddNotificationPM("pm.job_detail.notification", member.application_user_id, "ProjectManagement", data.job_number));
                                        }
                                        resp.Success = true;
                                        dataDto.job_name = data.job_name;
                                        dataDto.job_number = jobid;
                                    }
                                    else
                                    {
                                        resp.Message = "Job Insert Failed";
                                    }
                                }
                                else
                                {
                                    resp.Message = "Job Exist";
                                }


                                resp.Data = dataDto;

                                if (resp.Success)
                                {
                                    PeFileFolderDto modelFolder = new PeFileFolderDto()
                                    {
                                        parent_id = jobid,
                                        parent_entity = "pm.job_detail",
                                        folder_path = "PROJECTMANAGEMENT" + "/" + dataDto.job_name + " - " + jobid
                                    };
                                    CreateFolderOneDrive("PROJECTMANAGEMENT", dataDto.job_name + " - " + jobid, jobid);
                                    var resultFolder = CreateFolder(modelFolder);
                                    if (resultFolder.Success)
                                    {
                                        var resultSubFolder = CreateSubFolder(modelFolder.folder_path, "pm.job_detail", jobid);
                                    }
                                }

                            }
                            else
                            {
                                resp.Message = "Client Brief not found";
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }

        public BaseGeneralResponse CreateFolder(PeFileFolderDto model)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {
                PeFileFolder data = new PeFileFolder();
                PropertyMapper.All(model, data);
                data.is_active = true;
                data.is_default = true;
                data.is_deleted = false;
                _repoFileFolder.Create(false, data);
                _repoFileFolder.SaveChanges();

                resp.Success = true;
                resp.Message = "Folder has been successfully created.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public BaseGeneralResponse CreateSubFolder(string root, string parentEntity, string jobid)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();

            List<string> FolderName = new List<string>();
            try
            {
                var query = (from q in _repoDamFolderMaster.GetContext().Set<DamFolderMaster>()
                             where q.is_root == false
                             select new DamFolderMasterDto()
                             {
                                 folder_name = q.folder_name,
                                 root_name = q.root_name,
                                 application_entity_id = q.application_entity_id
                             }).ToList();


                for (int i = 0; i < query.Count(); i++)
                {
                    PeFileFolder data = new PeFileFolder();
                    data.folder_path = root + "/" + query[i].folder_name;
                    data.parent_entity = parentEntity;
                    data.parent_id = jobid;
                    data.is_active = true;
                    data.is_default = true;
                    data.is_deleted = false;
                    var cek = (from q in _repoFileFolder.GetContext().Set<PeFileFolder>()
                               where q.folder_path == data.folder_path
                               select q.folder_path
                              ).FirstOrDefault();
                    if (string.IsNullOrEmpty(cek))
                    {
                        _repoFileFolder.Create(false, data);
                        _repoFileFolder.SaveChanges();
                        FolderName.Add(query[i].folder_name);
                    }
                    var result = CreateFolderOneDrive(root,query[i].folder_name, jobid);
                }



                resp.Success = true;
                resp.Message = "Folder has been successfully created.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<PeFileFolderDto> GetFolderPath(string parentId)
        {
            GeneralResponse<PeFileFolderDto> resp = new GeneralResponse<PeFileFolderDto>();
            PeFileFolderDto modelDefault = new PeFileFolderDto();
            modelDefault.folder_path = "";
            modelDefault.parent_id = parentId;
            modelDefault.parent_entity = "";
            try
            {
                var query = (from tb1 in _repo.GetContext().Set<PeFileFolder>()
                             where tb1.parent_id == parentId
                             select new PeFileFolderDto()
                             {
                                 parent_id = parentId,
                                 folder_path = tb1.folder_path,
                                 parent_entity = tb1.parent_entity
                             }).DefaultIfEmpty(modelDefault).FirstOrDefault();
                resp.Success = true;
                resp.Message = "Success get data folder.";
                resp.Data = query;

                if (resp.Data.folder_path == "")
                {
                    var data = _repo.FindByID(parentId);
                    PeFileFolderDto modelFolder = new PeFileFolderDto()
                    {
                        parent_id = parentId,
                        parent_entity = "pm.job_detail",
                        folder_path = "PROJECTMANAGEMENT" + "/" + data.job_name + " - " + parentId
                    };
                    var resultCreateFolder = CreateFolderOneDrive("PROJECTMANAGEMENT", data.job_name + " - " + parentId, parentId);

                    var resultFolder = CreateFolder(modelFolder);

                    if (resultFolder.Success)
                    {
                        var resultSubFolder = CreateSubFolder(modelFolder.folder_path, "pm.job_detail", parentId);
                    }

                    var que = (from tb1 in _repo.GetContext().Set<PeFileFolder>()
                               where tb1.parent_id == parentId
                               select new PeFileFolderDto()
                               {
                                   parent_id = parentId,
                                   folder_path = tb1.folder_path,
                                   parent_entity = tb1.parent_entity
                               }).DefaultIfEmpty(modelDefault).FirstOrDefault();
                    resp.Success = true;
                    resp.Message = "Success get data folder.";
                    resp.Data = que;
                }
                else
                {
                    var data = _repo.FindByID(parentId);
                    PeFileFolderDto modelFolder = new PeFileFolderDto()
                    {
                        parent_id = parentId,
                        parent_entity = "pm.job_detail",
                        folder_path = "PROJECTMANAGEMENT" + "/" + data.job_name + " - " + parentId
                    };
                    var resultSubFolder = CreateSubFolder(modelFolder.folder_path, "pm.job_detail", parentId);
                }
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
                resp.Data = modelDefault;
            }
            return resp;
        }

        public BaseGeneralResponse DeleteFile(string id)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            var query = _repoFile.FindByID(id);
            query.is_active = false;
            query.is_deleted = true;
            _repoFile.Update(false, query);
            _repoFile.SaveChanges();
            resp.Code = "200";
            resp.Message = "";
            resp.Success = true;
            return resp;
        }

        public BaseGeneralResponse CheckIsSubmitStatus(string id)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            var query = from tb1 in _repo.GetContext().Set<JobDetail>()
                        join tb2 in _repo.GetContext().Set<Team>() on tb1.Id equals tb2.job_id
                        join tb3 in _repo.GetContext().Set<TeamMember>() on tb2.Id equals tb3.team_id
                        where tb1.Id == id
                        select new TeamMember()
                        {
                            application_user_id = tb3.application_user_id,
                            Id = tb3.Id,
                            act_as = tb3.act_as
                        };
            var allowedAct = new[] { "HP", "JO", "PM" };
            var data = query.Where(x => x.application_user_id == Glosing.Instance.Username && allowedAct.Contains(x.act_as)).FirstOrDefault();
            if (data != null)
            {
                resp.Success = true;
                resp.Message = "Data found.";
            }
            else
            {
                resp.Success = false;
                resp.Message = "Data not found";
            }
            resp.Code = "200";
            return resp;

        }
        public GeneralResponseList<PeFileDto> GetListFile(string parentId)
        {
            GeneralResponseList<PeFileDto> resp = new GeneralResponseList<PeFileDto>();

            try
            {
                var query = from tb1 in _repo.GetContext().Set<PeFile>()
                            join tb2 in _repo.GetContext().Set<EmployeeBasicInfo>() on tb1.created_by equals tb2.Id into lj1
                            from tb12 in lj1.DefaultIfEmpty()
                            where tb1.parent_id == parentId && tb1.is_deleted != true
                            select new PeFileDto()
                            {
                                file_name = tb1.file_name,
                                file_path = tb1.file_path,
                                file_size = tb1.file_size,
                                file_type = tb1.mimetype,
                                flag = tb1.flag,
                                Id = tb1.Id,
                                upload_date = tb1.upload_date,
                                created_by = tb12.name_employee,
                                tags = tb1.tags
                            };
                resp.Rows = query.ToList();
                resp.Total = query.Count();
                resp.Success = true;
                resp.Message = "Success get data.";



            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
                resp.Total = 0;
            }
            return resp;
        }

        public BaseGeneralResponse UpdateJob(JobDetailUpdate data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            resp.Success = false;
            try
            {
                JobDetail newdata = _repo.FindByID(data.id);
                ClientBrief clbrief = _repocl.FindByID(newdata.client_brief_id);
                DateTime dt1 = data.start_job ?? DateTime.Now.AddDays(7);
                DateTime dt2 = data.deadline ?? DateTime.Now.AddDays(15);
                DateTime dt3 = data.final_delivery ?? DateTime.Now.AddMonths(1);
                DateTime dt4 = clbrief.start_campaign ?? DateTime.Now;
                int resultFinalDelivery = DateTime.Compare(dt3, dt2);
                int resultDeadline = DateTime.Compare(dt2, dt1);
                int resultCampaign = DateTime.Compare(dt1, dt4);

                if (resultCampaign < 0)
                {
                    resp.Success = false;
                    resp.Message = "Start Job is older than start campaign date";
                }
                else
                {
                    if (resultDeadline < 0)
                    {
                        resp.Success = false;
                        resp.Message = "Deadline date is older than Start Job date";
                    }
                    else
                    {
                        if (resultFinalDelivery < 0)
                        {
                            resp.Success = false;
                            resp.Message = "Final delivery is older than deadline date";
                        }
                        else
                        {
                            if (newdata != null)
                            {
                                PropertyMapper.All(data, newdata);
                                _repo.Update(newdata);
                                resp.Success = true;

                                var teamMemberId = _repo.GetContext().Set<Team>().Where(x => x.job_id == newdata.job_number).Select(x => x.Id).FirstOrDefault();

                                var NewTeamMember = _repo.GetContext().Set<TeamMember>().Where(x => x.team_id == teamMemberId && x.application_user_id == Glosing.Instance.Username).ToList();
                                if (NewTeamMember.Count == 0)
                                {
                                    TeamMember newData = new TeamMember();
                                    newData.application_user_id = Glosing.Instance.Username;
                                    newData.team_id = teamMemberId;
                                    newData.is_deleted = false;
                                    newData.is_active = true;
                                    newData.owner_id = Glosing.Instance.Username;
                                    newData.act_as = "JO";
                                    _repoteammember.Create(true, newData);
                                    _repoteammember.SaveChanges();
                                }

                                var checkFolder = GetFolderPath(newdata.job_number);
                                if (!checkFolder.Success)
                                {
                                    PeFileFolderDto modelFolder = new PeFileFolderDto()
                                    {
                                        parent_id = newdata.job_number,
                                        parent_entity = "pm.job_detail",
                                        folder_path = "PROJECTMANAGEMENT" + "/" + newdata.job_name + " - " + newdata.job_number
                                    };
                                    CreateFolderOneDrive("PROJECTMANAGEMENT", newdata.job_name + " - " + newdata.job_number, newdata.job_number);
                                    var resultFolder = CreateFolder(modelFolder);


                                    if (resultFolder.Success)
                                    {
                                        var resultSubFolder = CreateSubFolder(modelFolder.folder_path, "pm.job_detail", newdata.job_number);
                                    }
                                }

                            }
                            else
                            {
                                resp.Message = "Job not found";
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<JobDetailList> Delete(string id)
        {
            GeneralResponse<JobDetailList> resp = new GeneralResponse<JobDetailList>();
            try
            {
                var que = _repo.GetContext().Set<JobDetail>().Where(p => p.Id == id).FirstOrDefault();

                if (que == null)
                {
                    resp.Success = true;
                    resp.Message = "unassigned";
                }
                else
                {
                    DeleteByID(id);
                    resp.Success = true;
                }
            }
            catch (Exception ex)
            {
                resp.Code = "404";
                resp.Message = ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async System.Threading.Tasks.Task<bool> CreateFolderOneDrive(string rootName, string folderName, string jobid)
        {
            var result = false;
            try
            {
                var model = _repoAzure.GetOnlyOne().Result;
                var graph = new MsGraph(model.ClientId, model.TenantId);
                var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
                var Path = rootName;
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                var isroot = string.IsNullOrEmpty(Path) ? true : false;

                await MsGraphExtension.CreateFolder(graphClient, folderName, Path, isroot);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;


        }

        public async System.Threading.Tasks.Task<bool> CreateFolderOneDriveByList(string rootName, List<string> folderName, string jobid)
        {
            var result = false;
            try
            {
                var model = _repoAzure.GetOnlyOne().Result;
                var graph = new MsGraph(model.ClientId, model.TenantId);
                var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
                var Path = rootName;
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                var isroot = string.IsNullOrEmpty(Path) ? true : false;
                foreach (var item in folderName)
                {
                    await MsGraphExtension.CreateFolder(graphClient, item, Path, isroot);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }



    }
}

