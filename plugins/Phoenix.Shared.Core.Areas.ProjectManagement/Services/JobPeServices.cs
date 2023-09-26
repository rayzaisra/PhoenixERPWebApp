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
    public partial interface IJobPeServices : IBaseService<JobPeDto, JobPe, string>
    {
        GeneralResponseList<JobPeDto> ListAll(JobPeSearch filter);
    }
    public partial class JobPeServices : BaseService<JobPeDto, JobPe, string>, IJobPeServices
    {
        public JobPeServices(IEFRepository<JobPe, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobPeDto> ListAll(JobPeSearch filter)
        {

            GeneralResponseList<JobPeDto> resp = new GeneralResponseList<JobPeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobPeDto> que = from tb1 in _repo.GetContext().Set<JobPe>()
                          select new JobPeDto()
                          {
                              Id = tb1.Id,
                              approval_id = tb1.approval_id,
                              ca_number = tb1.ca_number,
                              child_pce_number = tb1.child_pce_number,
                              detail = tb1.detail,
                              due_date = tb1.due_date,
                              file_id = tb1.file_id,
                              job_detail_id = tb1.job_detail_id,
                              job_pa_id = tb1.job_pa_id,
                              margin = tb1.margin,
                              pe_cost = tb1.pe_cost,
                              pe_date = tb1.pe_date,
                              pe_line_item = tb1.pe_line_item,
                              pe_number = tb1.pe_number,
                              proc_number = tb1.proc_number,
                              submitted_by = tb1.submitted_by,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                que = q.filterContains(que, "ca_number", filter.Search_ca_number);
                que = q.filterContains(que, "child_pce_number", filter.Search_child_pce_number);
                que = q.filterContains(que, "detail", filter.Search_detail);
                que = q.filterEquals(que, "due_date", filter.Search_due_date);
                que = q.filterEquals(que, "file_id", filter.Search_file_id);
                que = q.filterEquals(que, "job_detail_id", filter.Search_job_detail_id);
                que = q.filterEquals(que, "job_pa_id", filter.Search_job_pa_id);
                que = q.filterEquals(que, "margin", filter.Search_margin);
                que = q.filterEquals(que, "pe_cost", filter.Search_pe_cost);
                que = q.filterEquals(que, "pe_date", filter.Search_pe_date);
                que = q.filterEquals(que, "pe_line_item", filter.Search_pe_line_item);
                que = q.filterEquals(que, "pe_number", filter.Search_pe_number);
                que = q.filterContains(que, "proc_number", filter.Search_proc_number);
                que = q.filterContains(que, "submitted_by", filter.Search_submitted_by);

                

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

