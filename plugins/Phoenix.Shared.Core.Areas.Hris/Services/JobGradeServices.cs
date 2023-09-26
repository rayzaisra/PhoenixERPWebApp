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
    public partial interface IJobGradeServices : IBaseService<JobGradeDto, JobGrade, string>
    {
        GeneralResponseList<JobGradeDto> ListAll(JobGradeSearch filter);
    }
    public partial class JobGradeServices : BaseService<JobGradeDto, JobGrade, string>, IJobGradeServices
    {
        public JobGradeServices(IEFRepository<JobGrade, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<JobGradeDto> ListAll(JobGradeSearch filter)
        {

            GeneralResponseList<JobGradeDto> resp = new GeneralResponseList<JobGradeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<JobGradeDto> que = from tb1 in _repo.GetContext().Set<JobGrade>()
                          select new JobGradeDto()
                          {
                              Id = tb1.Id,
                              grade_code = tb1.grade_code,
                              grade_name = tb1.grade_name,
                              joblevelid = tb1.joblevelid,
                              created_on = tb1.created_on
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "grade_code", filter.Search_grade_code);
                que = q.filterContains(que, "grade_name", filter.Search_grade_name);
                que = q.filterContains(que, "joblevelid", filter.Search_joblevelid);

                

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

