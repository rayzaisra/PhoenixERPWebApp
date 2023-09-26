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
    public partial interface IBasicInfoGradeTittleServices : IBaseService<BasicInfoGradeTittleDto, BasicInfoGradeTittle, string>
    {
        GeneralResponseList<BasicInfoGradeTittleDto> ListAll(BasicInfoGradeTittleSearch filter);
    }
    public partial class BasicInfoGradeTittleServices : BaseService<BasicInfoGradeTittleDto, BasicInfoGradeTittle, string>, IBasicInfoGradeTittleServices
    {
        public BasicInfoGradeTittleServices(IEFRepository<BasicInfoGradeTittle, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<BasicInfoGradeTittleDto> ListAll(BasicInfoGradeTittleSearch filter)
        {

            GeneralResponseList<BasicInfoGradeTittleDto> resp = new GeneralResponseList<BasicInfoGradeTittleDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<BasicInfoGradeTittleDto> que = from tb1 in _repo.GetContext().Set<BasicInfoGradeTittle>()
                          select new BasicInfoGradeTittleDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              job_grade_id = tb1.job_grade_id,
                              job_tittle_id = tb1.job_tittle_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "job_grade_id", filter.Search_job_grade_id);
                que = q.filterEquals(que, "job_tittle_id", filter.Search_job_tittle_id);

                

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

