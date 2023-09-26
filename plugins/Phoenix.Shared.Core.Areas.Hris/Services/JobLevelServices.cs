using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface IJobLevelServices : IBaseService<JobLevelDto, JobLevel, string>
    {
        GeneralResponseList<JobLevelDto> ListAll(JobLevelSearch filter);
    }
    public partial class JobLevelServices : BaseService<JobLevelDto, JobLevel, string>, IJobLevelServices
    {
        public JobLevelServices(IEFRepository<JobLevel, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobLevelDto> ListAll(JobLevelSearch filter)
        {

            GeneralResponseList<JobLevelDto> resp = new GeneralResponseList<JobLevelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobLevelDto> que = from tb1 in _repo.GetContext().Set<JobLevel>()
                          select new JobLevelDto()
                          {
                              Id = tb1.Id,
                              job_code = tb1.job_code,
                              job_name = tb1.job_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "job_code", filter.Search_job_code);
                que = q.filterContains(que, "job_name", filter.Search_job_name);

                

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

