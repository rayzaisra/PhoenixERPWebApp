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
    public partial interface IEmployeeChecklistCategoryServices : IBaseService<EmployeeChecklistCategoryDto, EmployeeChecklistCategory, string>
    {
        GeneralResponseList<EmployeeChecklistCategoryDto> ListAll(SearchEmployeeChecklistCategory filter);
    }
    public partial class EmployeeChecklistCategoryServices : BaseService<EmployeeChecklistCategoryDto, EmployeeChecklistCategory, string>, IEmployeeChecklistCategoryServices
    {
        public EmployeeChecklistCategoryServices(IEFRepository<EmployeeChecklistCategory, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeChecklistCategoryDto> ListAll(SearchEmployeeChecklistCategory filter)
        {

            GeneralResponseList<EmployeeChecklistCategoryDto> resp = new GeneralResponseList<EmployeeChecklistCategoryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeChecklistCategoryDto> que = from tb1 in _repo.GetContext().Set<EmployeeChecklistCategory>()
                          select new EmployeeChecklistCategoryDto()
                          {
                              Id = tb1.Id,
                              checklist_category = tb1.checklist_category,
                              created_on = tb1.created_on
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

