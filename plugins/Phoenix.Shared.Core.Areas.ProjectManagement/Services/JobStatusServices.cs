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
    public partial interface IJobStatusServices : IBaseService<JobStatusDto, JobStatus, string>
    {
        GeneralResponseList<JobStatusDto> ListAll(JobStatusSearch filter);
    }
    public partial class JobStatusServices : BaseService<JobStatusDto, JobStatus, string>, IJobStatusServices
    {
        public JobStatusServices(IEFRepository<JobStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobStatusDto> ListAll(JobStatusSearch filter)
        {

            GeneralResponseList<JobStatusDto> resp = new GeneralResponseList<JobStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobStatusDto> que = from tb1 in _repo.GetContext().Set<JobStatus>()
                          select new JobStatusDto()
                          {
                              Id = tb1.Id,
                              name = tb1.name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "name", filter.Search_name);

                

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

