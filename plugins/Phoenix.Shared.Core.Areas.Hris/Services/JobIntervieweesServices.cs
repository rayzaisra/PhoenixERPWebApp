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
    public partial interface IJobIntervieweesServices : IBaseService<JobIntervieweesDto, JobInterviewees, string>
    {
        GeneralResponseList<JobIntervieweesDto> ListAll(JobIntervieweesSearch filter);
    }
    public partial class JobIntervieweesServices : BaseService<JobIntervieweesDto, JobInterviewees, string>, IJobIntervieweesServices
    {
        public JobIntervieweesServices(IEFRepository<JobInterviewees, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobIntervieweesDto> ListAll(JobIntervieweesSearch filter)
        {

            GeneralResponseList<JobIntervieweesDto> resp = new GeneralResponseList<JobIntervieweesDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobIntervieweesDto> que = from tb1 in _repo.GetContext().Set<JobInterviewees>()
                          select new JobIntervieweesDto()
                          {
                              Id = tb1.Id,
                              candidate_recruitment_id = tb1.candidate_recruitment_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "candidate_recruitment_id", filter.Search_candidate_recruitment_id);

                

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

