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
    public partial interface IJobWorkflowHistoryServices : IBaseService<JobWorkflowHistoryDto, JobWorkflowHistory, string>
    {
        GeneralResponseList<JobWorkflowHistoryDto> ListAll(JobWorkflowHistorySearch filter);
    }
    public partial class JobWorkflowHistoryServices : BaseService<JobWorkflowHistoryDto, JobWorkflowHistory, string>, IJobWorkflowHistoryServices
    {
        public JobWorkflowHistoryServices(IEFRepository<JobWorkflowHistory, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobWorkflowHistoryDto> ListAll(JobWorkflowHistorySearch filter)
        {

            GeneralResponseList<JobWorkflowHistoryDto> resp = new GeneralResponseList<JobWorkflowHistoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobWorkflowHistoryDto> que = from tb1 in _repo.GetContext().Set<JobWorkflowHistory>()
                          select new JobWorkflowHistoryDto()
                          {
                              Id = tb1.Id,
                              current_status_created_by = tb1.current_status_created_by,
                              current_status_created_date = tb1.current_status_created_date,
                              current_status_id = tb1.current_status_id,
                              job_number = tb1.job_number,
                              last_status_created_by = tb1.last_status_created_by,
                              last_status_created_date = tb1.last_status_created_date,
                              last_status_id = tb1.last_status_id,
                              seq = tb1.seq,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "current_status_created_by", filter.Search_current_status_created_by);
                que = q.filterEquals(que, "current_status_created_date", filter.Search_current_status_created_date);
                que = q.filterEquals(que, "current_status_id", filter.Search_current_status_id);
                que = q.filterContains(que, "job_number", filter.Search_job_number);
                que = q.filterContains(que, "last_status_created_by", filter.Search_last_status_created_by);
                que = q.filterEquals(que, "last_status_created_date", filter.Search_last_status_created_date);
                que = q.filterEquals(que, "last_status_id", filter.Search_last_status_id);
                que = q.filterEquals(que, "seq", filter.Search_seq);

                

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

