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
    public partial interface IJobUserServices : IBaseService<JobUserDto, JobUser, string>
    {
        GeneralResponseList<JobUserDto> ListAll(JobUserDto data);
    }

    public partial class JobUserServices : BaseService<JobUserDto, JobUser, string>, IJobUserServices
    {
        public JobUserServices(IEFRepository<JobUser, string> repo) : base(repo)
        {
            _repo = repo;

        }

        public GeneralResponseList<JobUserDto> ListAll(JobUserDto data)
        {
            GeneralResponseList<JobUserDto> resp = new GeneralResponseList<JobUserDto>();
            try
            {
                IQueryable<JobUserDto> que = from tb1 in _repo.GetContext().Set<JobUser>().Where(p => p.user_id == Glosing.Instance.Username)
                                             join tb2 in _repo.GetContext().Set<JobDetail>() on tb1.job_detail_id equals tb2.Id
                                                   select new JobUserDto()
                                                   {
                                                        Id  = tb2.Id,
                                                        job_name = tb2.job_name,
                                                      
                                                   };
                resp.Rows = que.Distinct().ToList();
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
