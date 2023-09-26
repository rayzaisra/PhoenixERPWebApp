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
    public partial interface IJobInterviewerServices : IBaseService<JobInterviewerDto, JobInterviewer, string>
    {
        GeneralResponseList<JobInterviewerDto> ListAll(JobInterviewerSearch filter);
    }
    public partial class JobInterviewerServices : BaseService<JobInterviewerDto, JobInterviewer, string>, IJobInterviewerServices
    {
        public JobInterviewerServices(IEFRepository<JobInterviewer, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobInterviewerDto> ListAll(JobInterviewerSearch filter)
        {

            GeneralResponseList<JobInterviewerDto> resp = new GeneralResponseList<JobInterviewerDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobInterviewerDto> que = from tb1 in _repo.GetContext().Set<JobInterviewer>()
                          select new JobInterviewerDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);

                

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

