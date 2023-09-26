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
    public partial interface IEmployeeServices : IBaseService<EmployeeDto, Employee, string>
    {
        GeneralResponseList<EmployeeDto> ListAll(EmployeeSearch filter);
    }
    public partial class EmployeeServices : BaseService<EmployeeDto, Employee, string>, IEmployeeServices
    {
        public EmployeeServices(IEFRepository<Employee, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<EmployeeDto> ListAll(EmployeeSearch filter)
        {

            GeneralResponseList<EmployeeDto> resp = new GeneralResponseList<EmployeeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<EmployeeDto> que = from tb1 in _repo.GetContext().Set<Employee>()
                          select new EmployeeDto()
                          {
                              Id = tb1.Id,
                              account_no = tb1.account_no,
                              bankbranch_id = tb1.bankbranch_id,
                             // Employee_id = tb1.Employee_id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "account_no", filter.Search_account_no);
                que = q.filterEquals(que, "bankbranch_id", filter.Search_bankbranch_id);
                que = q.filterEquals(que, "Employee_id", filter.Search_Employee_id);
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

