using Microsoft.AspNetCore.SignalR;
using Phoenix.Data.Models.Um;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.General.Helpers;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using server.Hubs;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ITeamServices : IBaseService<TeamDto, Team, string>
    {
        GeneralResponseList<TeamDto> ListAll(TeamSearch filter);
        GeneralResponseList<vUserList> GetAllUser(UserlistSearch filter);
        GeneralResponse<TeamWithUserList> GetTeamUsers(String id);
        bool manageteamusers(TeamUsers data);
        bool managejobteamusers(TeamJobUsers data);

    }
    public partial class TeamServices : BaseService<TeamDto, Team, string>, ITeamServices
    {
        const string unassigned = "unassigned";
        const string assigned = "assignmember";
        IEFRepository<TeamMember, string> _repotm;
        IEFRepository<JobDetail, string> _repojob;
        IEFRepository<JobUser, string> _repojobuser;
        IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> _repoTemplateEmail;
        IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> _repoUserApp;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        protected Notify _notif;
        public TeamServices(IEFRepository<Team, string> repo, IEFRepository<TeamMember, string> repotm, IEFRepository<JobUser, string> repojobuser
            , IEFRepository<JobDetail, string> repojob,IEFRepository<Notification, string> reponotif,
            IEFRepository<NotificationType, string> repotype, IHubContext<NotificationHub> hubContext, IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> repoTemplateEmail,
            IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> repoUserApp, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure) : base(repo)
        {
            _repo = repo;
            _repotm = repotm;
            _repojob = repojob;
            _repojobuser = repojobuser;
            _notif = new Notify(hubContext, reponotif, repotype, repoTemplateEmail, repoUserApp, repoAzure);
        }
        public bool manageteamusers(TeamUsers data)
        {
            bool retval = false;
            try
            {
                var delall = _repo.Rawquery("delete from [pm].[pm_team_member] where team_id='" + data.team_id + "'");
                if (data.team != null)
                {
                    int urutan = 0;
                    foreach (Teamlist useritem in data.team)
                    {
                        urutan++;
                        TeamMember xuser = new TeamMember();
                        xuser.application_user_id = useritem.user_id;
                        xuser.team_id = data.team_id;
                        xuser.act_as = useritem.act_as;
                        _repotm.Create(xuser);
                    }
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }

            return retval;
        }
        public bool managejobteamusers(TeamJobUsers data)
        {
            bool retval = false;
            try
            {
                JobDetail job = _repojob.FindByID(data.job_id);
                if (job != null)
                {
                    var team = _repo.GetContext().Set<Team>().Where(p => p.job_id== data.job_id).FirstOrDefault();
                    var delall = _repo.Rawquery("delete from [pm].[pm_team_member] where team_id='" + team.Id + "'");

                    if (data.team != null)
                    {
                        int urutan = 0;

                        foreach (Teamlist useritem in data.team)
                        {

                            urutan++;
                            var check = _repo.GetContext().Set<TeamMember>().Where(x => x.application_user_id == useritem.user_id && x.team_id == team.Id).FirstOrDefault();
                            if (check == null)
                            {
                                TeamMember xuser = new TeamMember();
                                xuser.application_user_id = useritem.user_id;
                                xuser.team_id = team.Id;
                                xuser.act_as = useritem.act_as;

                                _repotm.Create(xuser);
                                _repotm.SaveChanges();

                                JobUser ju = new JobUser();
                                ju.job_detail_id = data.job_id;
                                ju.user_id = xuser.application_user_id;

                                _repojobuser.Create(ju);
                                _repojobuser.SaveChanges();
                                //if (useritem.act_as == "JO")
                                //{

                                //}
                                _notif.createNotification(ju.user_id, job.Id, "pm", "jobassignmember", job.job_name);
                            }
                            
                        }
                        if (job.job_status_id == unassigned)
                        {
                            job.job_status_id = assigned;
                            _repojob.Update(job);
                            _repojob.SaveChanges();
                        }

                        retval = true;
                    }
                }

            }
            catch (Exception ex)
            {
                retval = false;
            }

            return retval;
        }
        public GeneralResponse<TeamWithUserList> GetTeamUsers(String id)
        {
            GeneralResponse<TeamWithUserList> resp = new GeneralResponse<TeamWithUserList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TeamWithUserList> que = from tb1 in _repo.GetContext().Set<Team>()
                                                   where tb1.Id == id
                                                   select new TeamWithUserList()
                                                   {
                                                       Id = tb1.Id,
                                                       team_name = tb1.team_name,
                                                       userlist = (from a in _repo.GetContext().Set<TeamMember>()
                                                                   join b in _repo.GetContext().Set<vUserList>() on a.application_user_id equals b.Id
                                                                   where a.team_id == tb1.Id
                                                                   select new UserList()
                                                                   {

                                                                       user_id = a.application_user_id,
                                                                       username = b.AliasName,
                                                                       email = b.Email,
                                                                       act_as = a.act_as
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
        public GeneralResponseList<vUserList> GetAllUser(UserlistSearch filter)
        {
            GeneralResponseList<vUserList> resp = new GeneralResponseList<vUserList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<vUserList> que = from tb1 in _repo.GetContext().Set<vUserList>()
                                            select new vUserList()
                                            {
                                                Id = tb1.Id,
                                                AliasName = tb1.AliasName,
                                                business_unit_departement_id = tb1.business_unit_departement_id,
                                                business_unit_division_id = tb1.business_unit_division_id,
                                                business_unit_group_id = tb1.business_unit_group_id,
                                                business_unit_subgroup_id = tb1.business_unit_subgroup_id,
                                                Email = tb1.Email,
                                                EmployeeBasicInfoId = tb1.EmployeeBasicInfoId,
                                                GroupId = tb1.GroupId,
                                                IsActive = tb1.IsActive,
                                                IsDeleted = tb1.IsDeleted,
                                                IsMsUser = tb1.IsMsUser
                                            };
                resp.RecordsTotal = que.Count();
                if (filter.search != null)
                {
                    que = que.Where(w => w.Id.Contains(filter.search));
                }

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

        public GeneralResponseList<TeamDto> ListAll(TeamSearch filter)
        {

            GeneralResponseList<TeamDto> resp = new GeneralResponseList<TeamDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TeamDto> que = from tb1 in _repo.GetContext().Set<Team>()
                                          where tb1.is_template == true
                                          select new TeamDto()
                                          {
                                              Id = tb1.Id,
                                              organization_id = tb1.organization_id,
                                              team_leader = tb1.team_leader,
                                              team_name = tb1.team_name,
                                              job_id = tb1.job_id
                                          };


                if (filter.search != null)
                {
                    que = que.Where(w => w.team_name.Contains(filter.search));
                }

                if (!string.IsNullOrEmpty(filter.Search_job_id_is_null))
                {
                    que = que.Where(w => w.job_id == null);
                }

                resp.RecordsTotal = que.Count();

                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);
                que = q.filterContains(que, "team_leader", filter.Search_team_leader);
                que = q.filterContains(que, "team_name", filter.Search_team_name);



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
    }
}

