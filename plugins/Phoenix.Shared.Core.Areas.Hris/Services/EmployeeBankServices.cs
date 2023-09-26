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
    public partial interface IEmployeeBankServices : IBaseService<EmployeeBankDto, EmployeeBank, string>
    {
        GeneralResponseList<EmployeeBankDto> ListAll(EmployeeBankSearch filter);
    }
    public partial class EmployeeBankServices : BaseService<EmployeeBankDto, EmployeeBank, string>, IEmployeeBankServices
    {
        public EmployeeBankServices(IEFRepository<EmployeeBank, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeBankDto> ListAll(EmployeeBankSearch filter)
        {

            GeneralResponseList<EmployeeBankDto> resp = new GeneralResponseList<EmployeeBankDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeBankDto> que = from tb1 in _repo.GetContext().Set<EmployeeBank>()
                          select new EmployeeBankDto()
                          {
                              Id = tb1.Id,
                              account_name = tb1.account_name,
                              account_no = tb1.account_no,
                              bank_address = tb1.bank_address,
                              bank_city = tb1.bank_city,
                              bank_id = tb1.bank_id,
                              branch = tb1.branch,
                              currency_id = tb1.currency_id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_date = tb1.end_date,
                              start_date = tb1.start_date,
                              sub_bank_id = tb1.sub_bank_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "account_name", filter.Search_account_name);
                que = q.filterContains(que, "account_no", filter.Search_account_no);
                que = q.filterContains(que, "bank_address", filter.Search_bank_address);
                que = q.filterContains(que, "bank_city", filter.Search_bank_city);
                que = q.filterEquals(que, "bank_id", filter.Search_bank_id);
                que = q.filterContains(que, "branch", filter.Search_branch);
                que = q.filterEquals(que, "currency_id", filter.Search_currency_id);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_date", filter.Search_end_date);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);
                que = q.filterEquals(que, "sub_bank_id", filter.Search_sub_bank_id);

                

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

