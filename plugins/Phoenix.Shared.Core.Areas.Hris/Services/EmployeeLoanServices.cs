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
    public partial interface IEmployeeLoanServices : IBaseService<EmployeeLoanDto, EmployeeLoan, string>
    {
        GeneralResponseList<EmployeeLoanEditDto> ListAll(EmployeeLoanSearch filter);
        GeneralResponse<EmployeeLoanDto> GetWithName(string id);

        GeneralResponse<EmployeeLoanEditDto> GetWithEdit(string id);
    }
    public partial class EmployeeLoanServices : BaseService<EmployeeLoanDto, EmployeeLoan, string>, IEmployeeLoanServices
    {
        private IEFRepository<EmployeeLoan, string> _repoSalarycomponent;
        public EmployeeLoanServices(IEFRepository<EmployeeLoan, string> repo, IEFRepository<EmployeeLoan, string> repoSalarycomponent) : base(repo)
        {
            _repo = repo;
            _repoSalarycomponent = repoSalarycomponent;
        }

        public GeneralResponse<EmployeeLoanEditDto> GetWithEdit(string id)
        {
            GeneralResponse<EmployeeLoanEditDto> resp = new GeneralResponse<EmployeeLoanEditDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeLoanEditDto> que = from tb1 in _repo.GetContext().Set<EmployeeLoan>()
                                                  join tb2 in _repo.GetContext().Set<LoanCategory>() on tb1.loan_category_id equals tb2.Id
                                                  where tb1.Id == id
                                                  select new EmployeeLoanEditDto()
                                                  {
                                                      Id = tb1.Id,
                                                      name = tb1.name,
                                                      value = tb1.value,
                                                      description = tb1.description,
                                                      loan_category_name = tb2.name,
                                                      date_loan = String.Format("{0:MM/dd/yyyy}", tb1.date_loan),
                                                      payroll_deduction = tb1.payroll_deduction,
                                                      date_repaymentstartdate = String.Format("{0:MM/dd/yyyy}", tb1.date_repaymentstartdate),
                                                      remaining = 0,
                                                      grade = tb1.grade,
                                                      loan_category_id = tb1.loan_category_id
                                                  };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponse<EmployeeLoanDto> GetWithName(string id)
        {
            GeneralResponse<EmployeeLoanDto> resp = new GeneralResponse<EmployeeLoanDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeLoanDto> que = from tb1 in _repo.GetContext().Set<EmployeeLoan>()
                                                        join tb2 in _repo.GetContext().Set<LoanCategory>() on tb1.loan_category_id equals tb2.Id
                                                        where tb1.Id == id
                                                        select new EmployeeLoanDto()
                                                        {
                                                            Id = tb1.Id,
                                                            name = tb1.name,
                                                            value = tb1.value,
                                                            description = tb1.description,
                                                            loan_category_name = tb2.name,
                                                            date_loan = tb1.date_loan,
                                                            payroll_deduction = tb1.payroll_deduction,
                                                            date_repaymentstartdate = tb1.date_repaymentstartdate,
                                                            remaining = 0,
                                                            grade = tb1.grade,
                                                            loan_category_id = tb1.loan_category_id
                                                        };
                resp.Data = que.FirstOrDefault();
                resp.Success = true;
                resp.Code = "00";
                resp.Message = "sucess";

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public GeneralResponseList<EmployeeLoanEditDto> ListAll(EmployeeLoanSearch filter)
        {

            GeneralResponseList<EmployeeLoanEditDto> resp = new GeneralResponseList<EmployeeLoanEditDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeLoanEditDto> que = from tb1 in _repo.GetContext().Set<EmployeeLoan>()
                                                        join tb2 in _repo.GetContext().Set<LoanCategory>() on tb1.loan_category_id equals tb2.Id
                                                        select new EmployeeLoanEditDto()
                                                        {
                                                            Id = tb1.Id,
                                                            name = tb1.name,
                                                            value = tb1.value,
                                                            description = tb1.description,
                                                            loan_category_name = tb2.name,
                                                            date_loan = String.Format("{0:MM/dd/yyyy}", tb1.date_loan),
                                                            payroll_deduction = tb1.payroll_deduction,
                                                            date_repaymentstartdate = String.Format("{0:MM/dd/yyyy}", tb1.date_repaymentstartdate),
                                                            remaining = 0
                                                        };

                resp.RecordsTotal = que.Count();

                que = q.filterContains(que, "name", filter.Search_name);
                //que = q.filterEquals(que, "salary_component", filter.Search_salary_component);
                //que = q.filterEquals(que, "mpp_budged", filter.Search_mpp_budged);
                //que = q.filterEquals(que, "mpp_id", filter.Search_mpp_id);
                //que = q.filterContains(que, "ttf", filter.Search_ttf);



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

