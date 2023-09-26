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
    public partial interface IJobWorkflowServices : IBaseService<JobWorkflowDto, JobWorkflow, string>
    {
        GeneralResponseList<JobWorkflowDto> ListAll(JobWorkflowSearch filter);
    }
    public partial class JobWorkflowServices : BaseService<JobWorkflowDto, JobWorkflow, string>, IJobWorkflowServices
    {
        public JobWorkflowServices(IEFRepository<JobWorkflow, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobWorkflowDto> ListAll(JobWorkflowSearch filter)
        {

            GeneralResponseList<JobWorkflowDto> resp = new GeneralResponseList<JobWorkflowDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobWorkflowDto> que = from tb1 in _repo.GetContext().Set<JobWorkflow>()
                          select new JobWorkflowDto()
                          {
                              Id = tb1.Id,
                              description = tb1.description,
                              workflow_name=tb1.workflow_name

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
    }
}

