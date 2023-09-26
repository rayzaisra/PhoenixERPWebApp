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
    public partial interface IJobTypeServices : IBaseService<JobTypeDto, JobType, string>
    {
        GeneralResponseList<JobTypeDto> ListAll(JobTypeSearch filter);
    }
    public partial class JobTypeServices : BaseService<JobTypeDto, JobType, string>, IJobTypeServices
    {
        public JobTypeServices(IEFRepository<JobType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobTypeDto> ListAll(JobTypeSearch filter)
        {

            GeneralResponseList<JobTypeDto> resp = new GeneralResponseList<JobTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobTypeDto> que = from tb1 in _repo.GetContext().Set<JobType>()
                          select new JobTypeDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              name = tb1.name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "description", filter.Search_description);
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

