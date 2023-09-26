using Microsoft.AspNetCore.SignalR;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.General.Helpers;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Responses;
using server.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface ITaskServices : IBaseService<TaskDto, Task, string>
    {
        GeneralResponseList<TaskDto> ListAll(TaskSearch filter);
        GeneralResponseList<vTaskList> ListFilter(string id);
        GeneralResponseList<CentralTaskList> ListByJobNumber(string jobNumber);
        GeneralResponse<TaskDto> Manage(TaskDto data);
        GeneralResponse<TaskNew> Create(TaskNew data);
        GeneralResponse<TaskUpdate> Update(TaskUpdate data);
        GeneralResponse<TaskDto> GetDetail(string id);
        GeneralResponseList<vTaskList> GetByJobNumber(string jobNumber);
        GeneralResponse<SubTaskUpdate> UpdateSubtask(SubTaskUpdate data);
        GeneralResponse<SubTaskNew> CreateSubtask(SubTaskNew data);
        GeneralResponse<SubTaskUpdatedate> UpdateSubtaskdate(SubTaskUpdatedate data);
        GeneralResponseList<Taskcommentlist> comment(Tasknewcomment data);
        GeneralResponseList<Taskcommentlist> commentlist(string task_id);
        bool manageSubTaskUsers(subTaskUsers studata);
        BaseGeneralResponse updateprogress(Tasksubpercentage data);
        BaseGeneralResponse updatestatus(Taskstatusset data);
        BaseGeneralResponse updateDumpstatus(List<Taskstatusset> data);

        BaseGeneralResponse sendnotification(string jobnumber);

    }
    public partial class TaskServices : BaseService<TaskDto, Task, string>, ITaskServices
    {
        IEFRepository<SubTask, string> _reposubtask;
        IEFRepository<TaskComment, string> _repotaskcomment;
        IEFRepository<JobDetail, string> _repojob;
        IEFRepository<SubTaskUser, string> _reposubtaskuser;
        IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> _repoTemplateEmail;
        IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> _repoUserApp;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        protected Notify _notif;
        public TaskServices(IEFRepository<Task, string> repo,
            IEFRepository<SubTask, string> reposubtask,
            IEFRepository<Notification, string> reponotif,
            IEFRepository<NotificationType, string> repotype,
            IEFRepository<SubTaskUser, string> reposubtaskuser,
            IEFRepository<TaskComment, string> repotaskcomment,
            IEFRepository<JobDetail, string> repojob, IHubContext<NotificationHub> hubContext,
            IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> repoTemplateEmail,
            IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> repoUserApp, 
            Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure) : base(repo)
        {
            _repo = repo;
            _repojob = repojob;
            _reposubtask = reposubtask;
            _reposubtaskuser = reposubtaskuser;
            _repotaskcomment = repotaskcomment;
            _notif = new Notify(hubContext, reponotif, repotype, repoTemplateEmail, repoUserApp, repoAzure);
        }


        public GeneralResponseList<vTaskList> ListFilter(string id)
        {
            GeneralResponseList<vTaskList> resp = new GeneralResponseList<vTaskList>();

            try
            {
                IQueryable<vTaskList> que = from tb1 in _repo.GetContext().Set<Task>()
                                            where tb1.job_number == id
                                            select new vTaskList()
                                            {
                                                id = tb1.Id,

                                                text = tb1.task_name
                                            };

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
        public GeneralResponseList<vTaskList> GetByJobNumber(string jobNumber)
        {
            GeneralResponseList<vTaskList> resp = new GeneralResponseList<vTaskList>();
            try
            {
                IQueryable<vTaskList> que = from tb1 in _repo.GetContext().Set<vTaskList>()
                                            where tb1.job_id == jobNumber
                                            select new vTaskList()
                                            {
                                                current_id = tb1.current_id,
                                                id = tb1.id,
                                                job_id = tb1.job_id,
                                                text = tb1.text,
                                                start_date = tb1.start_date,
                                                end_date = tb1.end_date,
                                                parent = tb1.parent,
                                                progress = tb1.progress,
                                                task_id = tb1.task_id,
                                                type = tb1.type,
                                                priority = tb1.priority,
                                                description = tb1.description,
                                                hour = tb1.hour,
                                                users = tb1.users
                                            };

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
        public GeneralResponse<TaskNew> Create(TaskNew data)
        {
            GeneralResponse<TaskNew> resp = new GeneralResponse<TaskNew>();
            try
            {

                Task newdata = new Task();
                PropertyMapper.All(data, newdata);
                newdata.task_status_id = "1";
                _repo.Create(newdata);
                SubTask newsubdata = new SubTask();
                newsubdata.due_date = newdata.deadline_task;
                newsubdata.start_date = newdata.start_task;
                newsubdata.task_status_id = "1";
                newsubdata.description = newdata.description;
                newsubdata.hour = data.hour;
                newsubdata.task_id = newdata.Id;
                newsubdata.sub_task = newdata.task_name;
                _reposubtask.Create(newsubdata);


                data.Id = newdata.Id;
                resp.Data = data;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponseList<Taskcommentlist> commentlist(string task_id)
        {
            GeneralResponseList<Taskcommentlist> resp = new GeneralResponseList<Taskcommentlist>();
            try
            {
                IQueryable<Taskcommentlist> que = from tb1 in _repo.GetContext().Set<TaskComment>()
                                                  join tb2 in _repo.GetContext().Set<vUserList>() on tb1.user_id equals tb2.Id
                                                  where tb1.task_id == task_id
                                                  select new Taskcommentlist()
                                                  {
                                                      comment = tb1.comment,
                                                      comment_date = tb1.comment_date,
                                                      task_id = tb1.task_id,
                                                      user_id = tb1.user_id,
                                                      user_name = tb2.AliasName
                                                  };

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
        public BaseGeneralResponse updatestatus(Taskstatusset data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {

                Task updata = _repo.FindByID(data.task_id);
                if (updata != null)
                {
                    updata.task_status_id = data.task_status_id;
                }
                _repo.Update(updata);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public BaseGeneralResponse updateDumpstatus(List<Taskstatusset> data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {
                foreach (Taskstatusset _data in data)
                {
                    Task updata = _repo.FindByID(_data.task_id);
                    if (updata != null)
                    {
                        updata.task_status_id = _data.task_status_id;
                    }
                    _repo.Update(updata);
                }
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public BaseGeneralResponse sendnotification(string jobnumber)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();

            try
            {
                IQueryable<JobDetailForm> que = from tb1 in _repo.GetContext().Set<JobDetail>()
                                                join tb2 in _repo.GetContext().Set<ClientBrief>() on tb1.client_brief_id equals tb2.Id
                                                join tb3 in _repo.GetContext().Set<CompanyBrand>() on tb1.brand_id equals tb3.Id
                                                join tb4 in _repo.GetContext().Set<Company>() on tb3.company_id equals tb4.Id
                                                join tb5 in _repo.GetContext().Set<JobStatus>() on tb1.job_status_id equals tb5.Id
                                                join tb6 in _repo.GetContext().Set<BusinessUnit>() on tb2.business_unit_id equals tb6.Id
                                                join tb8 in _repo.GetContext().Set<vClientBriefList>() on tb2.Id equals tb8.Id

                                                where tb1.Id == jobnumber
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
                                                                  }).ToList(),
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

                var model = que.FirstOrDefault();

                foreach (var member in model.TeamMember)
                {
                    _notif.createNotification(member.user_id, jobnumber, "pm", "kanban", model.job_name);
                }
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public BaseGeneralResponse updateprogress(Tasksubpercentage data)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {

                SubTask updata = _reposubtask.FindByID(data.sub_task_id);
                if (updata != null)
                {
                    updata.percentage = data.percentage;
                }
                _reposubtask.Update(updata);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponseList<Taskcommentlist> comment(Tasknewcomment data)
        {
            GeneralResponseList<Taskcommentlist> resp = new GeneralResponseList<Taskcommentlist>();
            try
            {

                TaskComment newdata = new TaskComment();
                newdata.comment = data.comment;
                newdata.task_id = data.task_id;
                newdata.comment_date = DateTime.Now;
                newdata.user_id = Glosing.Instance.Username;
                _repotaskcomment.Create(newdata);
                resp = commentlist(data.task_id);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<SubTaskNew> CreateSubtask(SubTaskNew data)
        {
            GeneralResponse<SubTaskNew> resp = new GeneralResponse<SubTaskNew>();
            try
            {

                SubTask newdata = new SubTask();
                PropertyMapper.All(data, newdata);
                newdata.task_status_id = "1";
                _reposubtask.Create(newdata);
                resp.Data = data;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GeneralResponse<SubTaskUpdate> UpdateSubtask(SubTaskUpdate data)
        {
            GeneralResponse<SubTaskUpdate> resp = new GeneralResponse<SubTaskUpdate>();
            SubTask newdata = _reposubtask.FindByID(data.id);
            if (newdata != null)
            {
                PropertyMapper.All(data, newdata);
                _reposubtask.Update(newdata);

                resp.Data = data;
                resp.Success = true;
            }
            else
            {

                resp.Success = false;
            }
            return resp;
        }
        public GeneralResponse<SubTaskUpdatedate> UpdateSubtaskdate(SubTaskUpdatedate data)
        {
            GeneralResponse<SubTaskUpdatedate> resp = new GeneralResponse<SubTaskUpdatedate>();
            SubTask newdata = _reposubtask.FindByID(data.id);

            if (newdata != null)
            {
                newdata.start_date = data.start_date;
                newdata.due_date = data.due_date;
                _reposubtask.Update(newdata);
                Task taskdata = _repo.FindByID(newdata.task_id);
                if (taskdata != null)
                {
                    bool needupdate = false;
                    if (taskdata.start_task >= data.start_date)
                    {
                        taskdata.start_task = data.start_date;
                        needupdate = true;
                    }
                    if (taskdata.deadline_task <= data.due_date)
                    {
                        taskdata.deadline_task = data.due_date;
                        needupdate = true;
                    }
                    if (needupdate)
                    {
                        _repo.Update(taskdata);
                    }
                }
                if (taskdata != null)
                {
                    JobDetail jobdata = _repojob.FindByID(taskdata.job_number);
                    if (jobdata != null)
                    {
                        bool needupdate = false;
                        if (jobdata.start_job >= data.start_date)
                        {
                            jobdata.start_job = data.start_date;
                            needupdate = true;
                        }
                        if (jobdata.deadline <= data.due_date)
                        {
                            jobdata.deadline = data.due_date;
                            needupdate = true;
                        }
                        if (needupdate)
                        {
                            _repojob.Update(jobdata);
                        }
                    }
                }

                resp.Data = data;
                resp.Success = true;
            }
            else
            {

                resp.Success = false;
            }
            return resp;
        }
        public GeneralResponse<TaskUpdate> Update(TaskUpdate data)
        {
            GeneralResponse<TaskUpdate> resp = new GeneralResponse<TaskUpdate>();
            try
            {

                Task newdata = _repo.FindByID(data.Id);
                if (newdata != null)
                {
                    PropertyMapper.All(data, newdata);
                    _repo.Update(newdata);

                    data.Id = newdata.Id;
                    resp.Data = data;
                    resp.Success = true;
                }
                else
                {

                    resp.Success = false;
                }

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<TaskDto> GetDetail(string id)
        {
            GeneralResponse<TaskDto> resp = new GeneralResponse<TaskDto>();
            try
            {

                IQueryable<TaskDto> que = from tb1 in _repo.GetContext().Set<Task>()
                                          where tb1.Id == id
                                          select new TaskDto()
                                          {
                                              Id = tb1.Id,
                                              central_resources_id = tb1.central_resources_id,
                                              deadline_task = tb1.deadline_task,
                                              description = tb1.description,
                                              job_number = tb1.job_number,
                                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                                              start_task = tb1.start_task,
                                              task_name = tb1.task_name,
                                              task_priority_id = tb1.task_priority_id,
                                              task_status_id = tb1.task_status_id,
                                              subtask = (from a in _repo.GetContext().Set<SubTask>()
                                                         where a.task_id == tb1.Id
                                                         select new SubTaskDto()
                                                         {
                                                             Id = a.Id,
                                                             sub_task = a.sub_task,
                                                             hour = a.hour,
                                                             description = a.description,
                                                             attachment = a.attachment,
                                                             percentage = a.percentage,
                                                             due_date = a.due_date,
                                                             start_date = a.start_date,
                                                             task_id = a.task_id,
                                                             subtaskuser = (from u in _repo.GetContext().Set<EmployeeBasicInfo>()
                                                                            join tu in _repo.GetContext().Set<SubTaskUser>() on u.email equals tu.user_id
                                                                            where tu.sub_task_id == a.Id
                                                                            select new SubTaskUserDto()
                                                                            {
                                                                                app_fullname = u.name_employee,
                                                                                app_username = u.email,
                                                                                user_id = u.Id
                                                                            }).ToList()
                                                         }).ToList()
                                          };


                resp.Data = que.FirstOrDefault();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }



        public GeneralResponseList<TaskDto> ListAll(TaskSearch filter)
        {

            GeneralResponseList<TaskDto> resp = new GeneralResponseList<TaskDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TaskDto> que = from tb1 in _repo.GetContext().Set<Task>()
                                          select new TaskDto()
                                          {
                                              Id = tb1.Id,
                                              central_resources_id = tb1.central_resources_id,
                                              deadline_task = tb1.deadline_task,
                                              description = tb1.description,
                                              job_number = tb1.job_number,
                                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                                              start_task = tb1.start_task,
                                              task_name = tb1.task_name,
                                              task_priority_id = tb1.task_priority_id,
                                              task_status_id = tb1.task_status_id,
                                          };

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "central_resources_id", filter.Search_central_resources_id);
                que = q.filterEquals(que, "deadline_task", filter.Search_deadline_task);
                que = q.filterContains(que, "description", filter.Search_description);
                que = q.filterContains(que, "job_number", filter.Search_job_number);
                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);
                que = q.filterEquals(que, "start_task", filter.Search_start_task);
                que = q.filterContains(que, "task_name", filter.Search_task_name);
                que = q.filterEquals(que, "task_priority_id", filter.Search_task_priority_id);
                que = q.filterEquals(que, "task_status_id", filter.Search_task_status_id);



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

        public GeneralResponseList<CentralTaskList> ListByJobNumber(string jobNumber)
        {
            GeneralResponseList<CentralTaskList> resp = new GeneralResponseList<CentralTaskList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CentralTaskList> que = from tb1 in _repo.GetContext().Set<CentralResource>()
                                                  join tb2 in _repo.GetContext().Set<ClientBriefCentralResource>() on tb1.Id equals tb2.central_resource_id
                                                  join tb3 in _repo.GetContext().Set<JobDetail>() on tb2.job_detail_id equals tb3.Id
                                                  where tb3.job_number == jobNumber
                                                  select new CentralTaskList()
                                                  {
                                                      Id = tb1.Id,
                                                      central_resources_id = tb1.Id,
                                                      central_resources_name = tb1.name,
                                                      task = (from a in _repo.GetContext().Set<Task>()
                                                              where a.central_resources_id == tb1.Id && a.job_number == jobNumber
                                                              select new TaskList()
                                                              {
                                                                  central_resources_id = a.central_resources_id,
                                                                  deadline_task = a.deadline_task,
                                                                  start_task = a.start_task,
                                                                  task_priority_id = a.task_priority_id,
                                                                  task_status_id = a.task_status_id,
                                                                  description = a.description
                                                              }).ToList()

                                                  };



                que = q.filterEquals(que, "job_number", jobNumber);

                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", "Id", "Id");


                resp.Rows = que.ToList();
                resp.Total = que.Count();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public bool manageSubTaskUsers(subTaskUsers studata)
        {
            bool resp = false;
            if (studata != null)
            {
                var subtask = _repo.GetContext().Set<SubTask>().Where(x => x.Id == studata.sub_task_id).FirstOrDefault();
                
                try
                {
                    var delall = _repo.Rawquery("delete from [pm].[sub_task_user] where sub_task_id='" + studata.sub_task_id + "'");
                    foreach (string userid in studata.user_id)
                    {
                        SubTaskUser newstuData = new SubTaskUser();
                        newstuData.sub_task_id = studata.sub_task_id;
                        newstuData.user_id = userid;
                        _reposubtaskuser.Create(false, newstuData);
                        _notif.createNotification(userid, studata.job_number_sub_task, "pm", "task", subtask.sub_task + " assigned");
                    }



                    _reposubtaskuser.SaveChanges();
                    resp = true;
                }
                catch (Exception ex)
                {
                    resp = false;
                }
            }
            return resp;
        }
        public GeneralResponse<TaskDto> Manage(TaskDto data)
        {
            GeneralResponse<TaskDto> resp = new GeneralResponse<TaskDto>();
            try
            {
                Task newData = new Task();


                if (data.Id == null)
                {
                    PropertyMapper.All(data, newData);
                    newData.task_status_id = "1";
                    _repo.Create(false, newData);
                    data.Id = newData.Id;
                }
                else
                {
                    newData = _repo.FindByID(data.Id);
                    PropertyMapper.All(data, newData);
                    _repo.Update(false, newData);
                }

                if (data.subtask != null)
                {
                    foreach (SubTaskDto stdata in data.subtask)
                    {
                        SubTask newstData = new SubTask();
                        stdata.task_id = newData.Id;




                        if (stdata.Id == null)
                        {
                            PropertyMapper.All(stdata, newstData);
                            newstData.task_status_id = "1";
                            _reposubtask.Create(false, newstData);
                            stdata.Id = newstData.Id;
                        }
                        else
                        {
                            newstData = _reposubtask.FindByID(stdata.Id);
                            PropertyMapper.All(stdata, newstData);
                            _reposubtask.Update(false, newstData);
                        }
                        var delall = _repo.Rawquery("delete from [pm].[sub_task_user] where sub_task_id='" + newstData.Id + "'");
                        if (stdata.subtaskusers != null)
                        {
                            foreach (string stuserdata in stdata.subtaskusers)
                            {
                                SubTaskUser newstuData = new SubTaskUser();
                                newstuData.sub_task_id = newstData.Id;
                                newstuData.user_id = stuserdata;
                                _reposubtaskuser.Create(false, newstuData);
                            }
                        }

                    }


                }

                _repo.SaveChanges();
                _reposubtask.SaveChanges();
                _reposubtaskuser.SaveChanges();
                _repo.Dispose();
                _reposubtask.Dispose();
                _reposubtaskuser.Dispose();
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            resp.Data = data;
            _repo.Dispose();
            _reposubtask.Dispose();
            return resp;
        }
    }
}

