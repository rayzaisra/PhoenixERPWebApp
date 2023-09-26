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
    public partial interface IJobRepositoryServices : IBaseService<JobRepositoryDto, JobRepository, string>
    {
        GeneralResponseList<JobRepositoryDto> ListAll(JobRepositorySearch filter);
    }
    public partial class JobRepositoryServices : BaseService<JobRepositoryDto, JobRepository, string>, IJobRepositoryServices
    {
        public JobRepositoryServices(IEFRepository<JobRepository, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobRepositoryDto> ListAll(JobRepositorySearch filter)
        {

            GeneralResponseList<JobRepositoryDto> resp = new GeneralResponseList<JobRepositoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobRepositoryDto> que = from tb1 in _repo.GetContext().Set<JobRepository>()
                          select new JobRepositoryDto()
                          {
                              Id = tb1.Id,
                              date = tb1.date,
                              external_id = tb1.external_id,
                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                              pe_file_id = tb1.pe_file_id,
                              remark = tb1.remark,
                              version = tb1.version,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "date", filter.Search_date);
                que = q.filterEquals(que, "external_id", filter.Search_external_id);
                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);
                que = q.filterEquals(que, "pe_file_id", filter.Search_pe_file_id);
                que = q.filterEquals(que, "remark", filter.Search_remark);
                que = q.filterEquals(que, "version", filter.Search_version);

                

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

