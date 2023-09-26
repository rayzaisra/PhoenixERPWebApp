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
    public partial interface IEmployeeChecklistServices : IBaseService<EmployeeChecklistDto, EmployeeChecklist, string>
    {
        GeneralResponseList<EmployeeChecklistDto> ListAll(SearchEmployeeChecklist filter);
    }
    public partial class EmployeeChecklistServices : BaseService<EmployeeChecklistDto, EmployeeChecklist, string>, IEmployeeChecklistServices
    {
        public EmployeeChecklistServices(IEFRepository<EmployeeChecklist, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeChecklistDto> ListAll(SearchEmployeeChecklist filter)
        {

            GeneralResponseList<EmployeeChecklistDto> resp = new GeneralResponseList<EmployeeChecklistDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeChecklistDto> que = from tb1 in _repo.GetContext().Set<EmployeeChecklist>()
                          select new EmployeeChecklistDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              checklistitem = tb1.checklistitem,
                              description = tb1.description,
                              whentoalert = tb1.whentoalert,
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

