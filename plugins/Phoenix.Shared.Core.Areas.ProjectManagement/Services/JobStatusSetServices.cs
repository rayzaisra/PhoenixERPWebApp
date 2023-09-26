using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IJobStatusSetServices : IBaseService<JobStatusSetDto, JobStatusSet, string>
    {
        GeneralResponseList<JobStatusSetDto> ListAll(JobStatusSetSearch filter);
    }
    public partial class JobStatusSetServices : BaseService<JobStatusSetDto, JobStatusSet, string>, IJobStatusSetServices
    {
        public JobStatusSetServices(IEFRepository<JobStatusSet, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobStatusSetDto> ListAll(JobStatusSetSearch filter)
        {

            GeneralResponseList<JobStatusSetDto> resp = new GeneralResponseList<JobStatusSetDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobStatusSetDto> que = from tb1 in _repo.GetContext().Set<JobStatusSet>()
                          select new JobStatusSetDto()
                          {
                              Id = tb1.Id,
                              job_status_set = tb1.job_status_set,
                              job_status_set_desc = tb1.job_status_set_desc,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "job_status_set", filter.Search_job_status_set);
                que = q.filterContains(que, "job_status_set_desc", filter.Search_job_status_set_desc);

                

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

