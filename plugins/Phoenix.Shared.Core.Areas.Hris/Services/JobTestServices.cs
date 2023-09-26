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
    public partial interface IJobTestServices : IBaseService<JobTestDto, JobTest, string>
    {
        GeneralResponseList<JobTestDto> ListAll(JobTestSearch filter);
    }
    public partial class JobTestServices : BaseService<JobTestDto, JobTest, string>, IJobTestServices
    {
        public JobTestServices(IEFRepository<JobTest, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobTestDto> ListAll(JobTestSearch filter)
        {

            GeneralResponseList<JobTestDto> resp = new GeneralResponseList<JobTestDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobTestDto> que = from tb1 in _repo.GetContext().Set<JobTest>()
                          select new JobTestDto()
                          {
                              Id = tb1.Id,
                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                              job_test_description = tb1.job_test_description,
                              notes = tb1.notes,
                              test_title = tb1.test_title,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);
                que = q.filterContains(que, "job_test_description", filter.Search_job_test_description);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterContains(que, "test_title", filter.Search_test_title);

                

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

