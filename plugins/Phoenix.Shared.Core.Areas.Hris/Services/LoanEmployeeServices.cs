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
    public partial interface ILoanEmployeeServices : IBaseService<LoanEmployeeDto, LoanEmployee, string>
    {
        GeneralResponseList<LoanEmployeeDto> ListAll(LoanEmployeeSearch filter);
    }
    public partial class LoanEmployeeServices : BaseService<LoanEmployeeDto, LoanEmployee, string>, ILoanEmployeeServices
    {
        public LoanEmployeeServices(IEFRepository<LoanEmployee, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LoanEmployeeDto> ListAll(LoanEmployeeSearch filter)
        {

            GeneralResponseList<LoanEmployeeDto> resp = new GeneralResponseList<LoanEmployeeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LoanEmployeeDto> que = from tb1 in _repo.GetContext().Set<LoanEmployee>()
                          select new LoanEmployeeDto()
                          {
                              Id = tb1.Id,
                              count_repayment = tb1.count_repayment,
                              date_loan = tb1.date_loan,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              monthly_repayment = tb1.monthly_repayment,
                              reason = tb1.reason,
                              repayment_end_date = tb1.repayment_end_date,
                              repayment_start_date = tb1.repayment_start_date,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "count_repayment", filter.Search_count_repayment);
                que = q.filterEquals(que, "date_loan", filter.Search_date_loan);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "monthly_repayment", filter.Search_monthly_repayment);
                que = q.filterContains(que, "reason", filter.Search_reason);
                que = q.filterEquals(que, "repayment_end_date", filter.Search_repayment_end_date);
                que = q.filterEquals(que, "repayment_start_date", filter.Search_repayment_start_date);
                que = q.filterEquals(que, "value", filter.Search_value);

                

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

