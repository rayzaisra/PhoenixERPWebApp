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
    public partial interface ISalaryComponentServices : IBaseService<SalaryComponentDto, SalaryComponent, string>
    {
        GeneralResponseList<SalaryComponentDto> ListAll(SalaryComponentSearch filter);
    }
    public partial class SalaryComponentServices : BaseService<SalaryComponentDto, SalaryComponent, string>, ISalaryComponentServices
    {
        public SalaryComponentServices(IEFRepository<SalaryComponent, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SalaryComponentDto> ListAll(SalaryComponentSearch filter)
        {

            GeneralResponseList<SalaryComponentDto> resp = new GeneralResponseList<SalaryComponentDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SalaryComponentDto> que = from tb1 in _repo.GetContext().Set<SalaryComponent>()
                          select new SalaryComponentDto()
                          {
                              Id = tb1.Id,
                              code = tb1.code,
                              salary_component = tb1.salary_component,
                              description = tb1.description
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "code", filter.Search_code);
                que = q.filterContains(que, "salary_component", filter.Search_salary_component);

                

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

