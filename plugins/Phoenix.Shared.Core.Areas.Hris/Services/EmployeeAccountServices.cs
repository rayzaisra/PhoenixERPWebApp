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
    public partial interface IEmployeeAccountServices : IBaseService<EmployeeAccountDto, EmployeeAccount, string>
    {
        GeneralResponseList<EmployeeAccountDto> ListAll(EmployeeAccountSearch filter);
    }
    public partial class EmployeeAccountServices : BaseService<EmployeeAccountDto, EmployeeAccount, string>, IEmployeeAccountServices
    {
        public EmployeeAccountServices(IEFRepository<EmployeeAccount, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeAccountDto> ListAll(EmployeeAccountSearch filter)
        {

            GeneralResponseList<EmployeeAccountDto> resp = new GeneralResponseList<EmployeeAccountDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeAccountDto> que = from tb1 in _repo.GetContext().Set<EmployeeAccount>()
                          select new EmployeeAccountDto()
                          {
                              Id = tb1.Id,
                              account_no = tb1.account_no,
                              bankbranch_id = tb1.bankbranch_id,
                              employeeaccount_id = tb1.employeeaccount_id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "account_no", filter.Search_account_no);
                que = q.filterEquals(que, "bankbranch_id", filter.Search_bankbranch_id);
                que = q.filterEquals(que, "employeeaccount_id", filter.Search_employeeaccount_id);
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

