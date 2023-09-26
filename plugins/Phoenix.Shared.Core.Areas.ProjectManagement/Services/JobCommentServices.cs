using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.General.Helpers;
using Phoenix.Shared.Core.Areas.General.Services;
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IJobCommentServices : IBaseService<JobCommentDto, JobComment, string>
    {
        GeneralResponseList<JobCommentModel> GetComments(string id);
        GeneralResponseList<JobCommentModel> CreateComments(JobCommentModel dto);
    }

    public class JobCommentServices : BaseService<JobCommentDto, JobComment, string>, IJobCommentServices
    {
        CommonHelper _commonHelper = new CommonHelper();
        IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> _repoTemplateEmail;
        IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> _repoUserApp;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        protected Notify _notif;

        public JobCommentServices(
            IEFRepository<Notification, string> reponotif,
                      IEFRepository<NotificationType, string> repotype,
            IEFRepository<JobComment, string> repo,
            IHubContext<NotificationHub> hubContext, IEFRepository<Phoenix.Shared.Core.Entities.TmEmailTemplate, string> repoTemplateEmail,
            IEFRepository<Phoenix.Shared.Core.Entities.TmUserApp, string> repoUserApp, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure) : base(repo)
        {
            _repo = repo;
            _notif = new Notify(hubContext, reponotif, repotype, repoTemplateEmail, repoUserApp, repoAzure);
        }


        public GeneralResponseList<JobCommentModel> GetComments(string id)
        {
            GeneralResponseList<JobCommentModel> resp = new GeneralResponseList<JobCommentModel>();
            try
            {
                IQueryable<JobCommentModel> que = from tb1 in _repo.GetContext().Set<JobComment>()
                                                  where tb1.job_number == id
                                                  select new JobCommentModel()
                                                  {
                                                      job_number = tb1.job_number,
                                                      comment_message = tb1.comment_message,
                                                      comment_date = tb1.comment_date,
                                                      hour_comment_date = Convert.ToDateTime(tb1.comment_date).ToString("HH:mm"),
                                                      user_id = tb1.user_id,
                                                      user_name = tb1.user_name
                                                  };
                resp.RecordsTotal = que.Count();
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

        public GeneralResponseList<JobCommentModel> CreateComments(JobCommentModel data)
        {
            GeneralResponseList<JobCommentModel> resp = new GeneralResponseList<JobCommentModel>();
            resp.Success = false;
            try
            {
                _repo.Create(true, new JobComment() { job_number = data.job_number, comment_message = data.comment_message, comment_date = DateTime.Now, user_id = data.user_id, user_name = data.user_name });
                IQueryable<JobCommentModel> que = from tb1 in _repo.GetContext().Set<JobComment>()
                                                  where tb1.job_number == data.job_number
                                                  select new JobCommentModel()
                                                  {
                                                      job_number = tb1.job_number,
                                                      comment_message = tb1.comment_message,
                                                      comment_date = tb1.comment_date,
                                                      hour_comment_date = Convert.ToDateTime(tb1.comment_date).ToString("HH:mm"),
                                                      user_id = tb1.user_id,
                                                      user_name = tb1.user_name
                                                  };
                resp.RecordsTotal = que.Count();
                resp.Total = resp.RecordsTotal;
                resp.Rows = que.ToList();
                resp.Success = true;
            }
            catch (Exception ex)
            {

                resp.Message = ex.Message;
            }
            return resp;
        }

    }
}
